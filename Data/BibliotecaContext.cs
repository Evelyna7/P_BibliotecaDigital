using MySql.Data.MySqlClient;
using System.Data;

namespace BibliotecaDigital.Data
{
    public class BibliotecaContext
    {
        private readonly string connectionString;

        public BibliotecaContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}