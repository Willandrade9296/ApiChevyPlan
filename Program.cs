using ApiDatalife.Config.DBContext;
using ApiDatalife.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;



var builder = WebApplication.CreateBuilder(args);

//Jwt configuration starts here
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();
var expireMinutesjwt = builder.Configuration.GetSection("Jwt:ExpireMinutes").Get<string>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtIssuer,
         ValidAudience = jwtIssuer,
        
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
         ClockSkew = TimeSpan.Zero // Elimina los 5 minutos extra de tolerancia
     };
 });
//Jwt configuration ends here


// Add services to the container.


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
});
//builder.Services.AddControllers().AddNewtonsoftJson(); 

builder.Services.AddDbContext<DynamicDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CONSOL")));

// Configuración del DynamicDbContext como servicio scoped
builder.Services.AddScoped<IDbContextFactory, DbContextFactory>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true; // Desactiva validación automática
});

//Configuración para valores de archivos
builder.Services.Configure<StorageSettings>(builder.Configuration.GetSection("StorageSettings"));


builder.Services.AddSingleton<IConnectionStringProvider, AppSettingsConnectionStringProvider>();
builder.Services.AddSingleton<IUserConnectionManager, UserConnectionManager>();

builder.Services.AddEndpointsApiExplorer();
#region Configuracion Swagger
builder.Services.AddSwaggerGen(options =>
{
    // ...
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API NÓMINA",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa tu token JWT aquí", // Mensaje para el usuario en Swagger
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    // ...
});

#endregion Configuracion Swagger


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}



app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<DbContextMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});



app.Run();
