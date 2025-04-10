using AgendaContato.Interfaces.Interfaces;
using AgendaContato.Models.Models;
using MySql.Data.MySqlClient;

namespace AgendaContato.Repository.Repository;

public class UsuarioRepository : IUsuarioRepository
{
    public bool CadastrarUsuario(UsuarioModel usuario)
    {
        string sql = "INSERT INTO Usuario VALUES(0, @nome, @email, @senha, @salt)";

        try
        {
            using var connection = Conexao.GetConnection;
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", usuario.Email);
            command.Parameters.AddWithValue("@nome", usuario.Nome);
            command.Parameters.AddWithValue("@senha", usuario.Senha);
            command.Parameters.AddWithValue("@salt", usuario.Salt);

            connection.Open();
            command.ExecuteNonQuery();
            return true;
        }
        catch (MySqlException sqlEx)
        {
            throw new Exception($"Erro de banco de dados ao cadastrar o usuário '{usuario.Nome}': {sqlEx.Message}", sqlEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro inesperado ao cadastrar o usuário '{usuario.Nome}'.", ex);
        }
    }

    public bool EmailExiste(string email)
    {
        string sql = "SELECT 1 FROM Usuario WHERE email = @email LIMIT 1";

        try
        {
            using var connection = Conexao.GetConnection;
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);

            connection.Open();
            
            var result = command.ExecuteScalar();
            return result != null;
        }
        catch (MySqlException sqlEx)
        {            
            throw new Exception($"Erro de banco de dados ao verificar o email '{email}': {sqlEx.Message}", sqlEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro inesperado ao verificar o email '{email}'.", ex);
        }
    }

    public UsuarioModel ObterUsuarioPorEmail(string email)
    {
        throw new NotImplementedException();
    }
}