using Npgsql;

namespace postgre.Services
{
    public class DbConnectionProvider : IDisposable
    {
        private readonly ILogger<DbConnectionProvider> _logger;
        private readonly IConfiguration _configuration;
        private NpgsqlConnection _connection;
        public DbConnectionProvider(IConfiguration configuration, ILogger<DbConnectionProvider> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _logger.LogInformation("CREATING connection");
            _connection = new NpgsqlConnection(configuration.GetConnectionString("postgres"));
            _connection.Open();
            _logger.LogInformation("CREATED connection");
        }

        public void OpenConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                _connection = new NpgsqlConnection(_configuration.GetConnectionString("postgres"));
                _connection.Open();
            }
        }

        public void Execute(string sql)
        {
            var cmd = new NpgsqlCommand(sql,_connection);
            cmd.ExecuteNonQuery();
        }

        public void Dispose()
        {
            _connection.Dispose();
            _logger.LogInformation("CLOSED connection postgres");
        }
    }
}
