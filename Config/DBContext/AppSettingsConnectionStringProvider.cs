namespace ApiDatalife.Config.DBContext
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString(string databaseName);
    }

    public class AppSettingsConnectionStringProvider : IConnectionStringProvider
    {
        private readonly IConfiguration _configuration;

        public AppSettingsConnectionStringProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString(string databaseName)
        {
            // Mapea el nombre de la base de datos a la cadena de conexión correspondiente
            var connectionString = _configuration.GetConnectionString(databaseName.ToUpper());

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException($"No se encontró conexión para la base de datos: {databaseName}");
            }

            return connectionString;
        }
    }
}
