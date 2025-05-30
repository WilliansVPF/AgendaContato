using AgendaContato.Interfaces.Interfaces;
using AgendaContato.Models.Models;
using MySql.Data.MySqlClient;

namespace AgendaContato.Repository.Repository;

public class EnderecoContatoRepository : IEnderecoContatoRepository
{
    public EnderecoContatoModel BuscaEnderecoContato(int id)
    {
        throw new NotImplementedException();
    }

    public bool CadastraEnderecoCOntato(EnderecoContatoModel endereco)
    {
        try
        {
            var sql = "INSERT INTO EnderecoContato VALUES(0, @valor, @observacao, @idTipoContato, @idContato);";

            using var connection = Conexao.GetConnection;
            connection.Open();

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@valor", endereco.Valor);
            command.Parameters.AddWithValue("@observacao", endereco.Observacao);
            command.Parameters.AddWithValue("@idTipoContato", endereco.IdTipoContato);
            command.Parameters.AddWithValue("@idContato", endereco.IdContato);

            command.ExecuteNonQuery();

            return true;
        }
        catch (MySqlException sqlEx)
        {
            throw new Exception("Erro de banco de dados ao cadastrar o endereço de contato.", sqlEx);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro inesperado ao cadastrar o contato.", ex);
        }
    }
}