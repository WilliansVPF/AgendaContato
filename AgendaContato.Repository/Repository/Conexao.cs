using MySql.Data.MySqlClient;

namespace AgendaContato.Repository.Repository
{
    public class Conexao
    {
        private static readonly string _connectionString = Environment.GetEnvironmentVariable("AgendaContato");

        public static MySqlConnection GetConnection() => new MySqlConnection(_connectionString);
    }
}