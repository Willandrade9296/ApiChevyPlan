using ApiDatalife.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;

public class DynamicDbContext : DbContext, IDisposable
{
    private readonly string _connectionString;
    private readonly DatabaseProvider _provider;
    private readonly ConcurrentDictionary<Type, object> _dynamicDbSets = new ConcurrentDictionary<Type, object>();
    private readonly ConcurrentDictionary<Type, Action<EntityTypeBuilder>> _entityConfigurations = new ConcurrentDictionary<Type, Action<EntityTypeBuilder>>();


    public DbSet<Usuario> Usuarios { get; set; }
    public enum DatabaseProvider
    {
        SqlServer,
        MySql,
        PostgreSQL,
        Sqlite
    }

    public DynamicDbContext(DbContextOptions<DynamicDbContext> options) : base(options)
    {
      /*  _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _provider = provider;  */
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            switch (_provider)
            {
                case DatabaseProvider.SqlServer:
                    optionsBuilder.UseSqlServer(_connectionString, sqlOptions =>
                    {

                        // Configuración de reintentos para errores transitorios
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(60),
                            errorNumbersToAdd: null);

                        // Habilitar división de consultas para queries con múltiples JOINs
                        sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                        // Configuración del tiempo de comando
                        sqlOptions.CommandTimeout(300); // 5 minutos

                        // Habilitar compatibilidad con versiones anteriores
                      //  sqlOptions.UseCompatibilityLevel(150); // SQL Server 2019

                        // Configuración del pool de conexiones
                       // sqlOptions.MinPoolSize(5);
                       // sqlOptions.MaxPoolSize(100);

                        // Otras configuraciones
                        sqlOptions.UseRelationalNulls(); // Manejo de NULLs en queries
                        sqlOptions.MigrationsAssembly("Your.Migrations.Assembly"); // Para migraciones

                    });
                    break;
               /* case DatabaseProvider.MySql:
                    optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
                    break;
                case DatabaseProvider.PostgreSQL:
                    optionsBuilder.UseNpgsql(_connectionString);
                    break;
                case DatabaseProvider.Sqlite:
                    optionsBuilder.UseSqlite(_connectionString);
                    break;*/
                default:
                    throw new ArgumentOutOfRangeException(nameof(_provider), $"Unsupported database provider: {_provider}");
            }

            // Opciones recomendadas para entornos de producción
            optionsBuilder.EnableSensitiveDataLogging(false); // Solo para desarrollo
            optionsBuilder.EnableDetailedErrors();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplicar todas las configuraciones registradas dinámicamente
        foreach (var config in _entityConfigurations)
        {
            // Verificar si la entidad ya fue configurada
            if (modelBuilder.Model.FindEntityType(config.Key) == null)
            {
                try
                {
                    // Método más seguro para invocar Entity<T>
                    var entityMethod = typeof(ModelBuilder).GetMethods()
                        .First(m => m.Name == "Entity" && m.IsGenericMethod)
                        .MakeGenericMethod(config.Key);

                    var entityBuilder = entityMethod.Invoke(modelBuilder, null);

                    // Aplicar configuración solo si no es nula
                    if (entityBuilder != null && config.Value != null)
                    {
                        config.Value.Invoke((EntityTypeBuilder)entityBuilder);
                    }
                }
                catch (Exception ex)
                {
                    // Loggear el error para diagnóstico
                    Console.WriteLine($"Error configuring entity {config.Key.Name}: {ex.Message}");
                    throw;
                }
            }
        }

        modelBuilder.Entity<SpResult>(entity =>
        {
            entity.HasNoKey(); // Marca como entidad sin clave primaria
            entity.ToView(null); // No está mapeada a una vista/tabla
        });
    }


    public DbSet<T> GetOrCreateDbSet<T>() where T : class
    {
        return (DbSet<T>)_dynamicDbSets.GetOrAdd(typeof(T), type =>
        {
            // Verificar si el modelo ya está configurado
            if (Model.FindEntityType(typeof(T)) == null)
            {
                // Configuración automática básica si no existe configuración previa
                if (!_entityConfigurations.ContainsKey(typeof(T)))
                {
                    AddEntityModel<T>(builder =>
                    {
                        // Configuración por defecto
                        builder.HasKey("Id"); // Asume una propiedad Id

                        // Intenta configurar propiedades basadas en el tipo
                        foreach (var prop in typeof(T).GetProperties())
                        {
                            if (prop.PropertyType == typeof(string))
                            {
                                builder.Property(prop.Name).HasMaxLength(256);
                            }
                        }
                    });
                }
            }

            return Set<T>();
        });
    }

    public void AddEntityModel<T>(Action<EntityTypeBuilder<T>> configuration) where T : class
    {

        if (_entityConfigurations.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException($"Entity type {typeof(T).Name} is already configured");
        }

        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        _entityConfigurations.AddOrUpdate(
            typeof(T),
            _ =>
            {
                Action<EntityTypeBuilder> nonGenericConfig = builder =>
                    configuration((EntityTypeBuilder<T>)builder);
                return nonGenericConfig;
            },
            (_, existingConfig) =>
            {
                Action<EntityTypeBuilder> combinedConfig = builder =>
                {
                    existingConfig(builder);
                    configuration((EntityTypeBuilder<T>)builder);
                };
                return combinedConfig;
            });
    }

    public void AddEntityModel(Type entityType, Action<EntityTypeBuilder> configuration)
    {
        if (entityType == null) throw new ArgumentNullException(nameof(entityType));
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        _entityConfigurations.AddOrUpdate(
            entityType,
            configuration,
            (_, existingConfig) =>
            {
                Action<EntityTypeBuilder> combinedConfig = builder =>
                {
                    existingConfig(builder);
                    configuration(builder);
                };
                return combinedConfig;
            });
    }

    public IQueryable Query(string entityName)
    {
        var entityType = Model.GetEntityTypes().FirstOrDefault(e => e.ClrType.Name == entityName);
        if (entityType == null)
            throw new InvalidOperationException($"Entity type '{entityName}' not found in the model.");

        var method = typeof(DynamicDbContext).GetMethod(nameof(GetOrCreateDbSet)).MakeGenericMethod(entityType.ClrType);
        var dbSet = (IQueryable)method.Invoke(this, null);

        return dbSet;
    }

    public void EnsureCreatedForTypes(params Type[] entityTypes)
    {
        // Configurar modelos primero
        foreach (var type in entityTypes)
        {
            if (!_entityConfigurations.ContainsKey(type))
            {
                var method = typeof(DynamicDbContext).GetMethod(nameof(AddDefaultConfiguration)).MakeGenericMethod(type);
                method.Invoke(this, null);
            }
        }

        // Crear la base de datos si no existe
        Database.EnsureCreated();
    }

    private void AddDefaultConfiguration<T>() where T : class
    {
        AddEntityModel<T>(builder =>
        {
            builder.HasKey("Id");

            // Configuración automática de propiedades
            foreach (var prop in typeof(T).GetProperties())
            {
                var propBuilder = builder.Property(prop.Name);

                if (prop.PropertyType == typeof(string))
                {
                    propBuilder.HasMaxLength(256);
                }
                else if (prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                {
                    propBuilder.HasColumnType("datetime");
                }
                else if (prop.PropertyType == typeof(decimal) || prop.PropertyType == typeof(decimal?))
                {
                    propBuilder.HasColumnType("decimal(18,2)");
                }
            }
        });
    }

    // Métodos adicionales útiles
    public IEnumerable<Type> GetRegisteredEntityTypes()
    {
        return _entityConfigurations.Keys;
    }

    public bool IsEntityTypeRegistered(Type entityType)
    {
        return _entityConfigurations.ContainsKey(entityType);
    }
}