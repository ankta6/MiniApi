using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MiniApi.Model
{
    public class MiniDBContext
    {
        private readonly string _connectionString;

        public MiniDBContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open(); // Ensure the connection is opened
            return connection;
        }
    }
}
