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

    public void NovoContato(ContatoModel contato, EnderecoContatoModel endereco, int? idUsuario)
    {
        const string sqlContato = "INSERT INTO Contato VALUES(0, @nome, @sobrenome, @idUsuario); SELECT LAST_INSERT_ID();";

        const string sqlEndereco = "INSERT INTO EnderecoContato VALUES(0, @valor, @observacao, @idTipoContato, @idContato);";

        try
        {
            using var connection = Conexao.GetConnection;
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                using var cmdContato = new MySqlCommand(sqlContato, connection, transaction);
                cmdContato.Parameters.AddWithValue("@nome", contato.Nome);
                cmdContato.Parameters.AddWithValue("@sobrenome", contato.Sobrenome);
                cmdContato.Parameters.AddWithValue("@idUsuario", idUsuario);
                int id = Convert.ToInt32(cmdContato.ExecuteScalar());

                using var cmdEndereco = new MySqlCommand(sqlEndereco, connection, transaction);
                cmdEndereco.Parameters.AddWithValue("@idContato", id);
                cmdEndereco.Parameters.AddWithValue("@valor", endereco.Valor);
                cmdEndereco.Parameters.AddWithValue("@observacao", endereco.Observacao);
                cmdEndereco.Parameters.AddWithValue("@idTipoContato", endereco.IdTipoContato);
                cmdEndereco.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception($"Erro ao cadastrar o contato '{contato.Nome}': {ex.Message}", ex);
            }
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
}