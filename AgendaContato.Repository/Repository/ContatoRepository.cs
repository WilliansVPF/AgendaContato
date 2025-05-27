using AgendaContato.Interfaces.Interfaces;
using AgendaContato.Models.Models;
using MySql.Data.MySqlClient;

namespace AgendaContato.Repository.Repository;

public class ContatoRepository : IContatoRepository
{
    public void NovoContato(ContatoModel contato, int idUsuario)   
    {
        string sql = "INSERT INTO Contato VALUES(0, @nome, @sobrenome, @idUsuario)";

        try
        {
            using var connection = Conexao.GetConnection;
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@nome", contato.Nome);
            command.Parameters.AddWithValue("@sobrenome", contato.Sobrenome);
            command.Parameters.AddWithValue("@idUsuario", idUsuario);

            connection.Open();
            command.ExecuteNonQuery();
        }
        catch (MySqlException sqlEx)
        {
            throw new Exception($"Erro de banco de dados ao cadastrar o contato '{contato.Nome}': {sqlEx.Message}", sqlEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro inesperado ao cadastrar o contato '{contato.Nome}'.", ex);
        }
    }

    public void NovoContato(ContatoModel contato, EnderecoContatoModel endereco, int idUsuario)
    {
        throw new NotImplementedException();
    }
}