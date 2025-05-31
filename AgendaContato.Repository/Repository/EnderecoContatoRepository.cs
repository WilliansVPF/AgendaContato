using AgendaContato.Interfaces.Interfaces;
using AgendaContato.Models.Models;
using MySql.Data.MySqlClient;

namespace AgendaContato.Repository.Repository;

public class EnderecoContatoRepository : IEnderecoContatoRepository
{
    public void AtualizaEndereco(EnderecoContatoModel endereco)
    {
        try
        {
            using var connection = Conexao.GetConnection;
            connection.Open();

            string sql = "UPDATE EnderecoContato SET valor = @valor, observacao = @observacao, idTipoContato = @idTipoContato WHERE idEnderecoContato = @idEnderecoContato";

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@valor", endereco.Valor);
            command.Parameters.AddWithValue("@observacao", endereco.Observacao);
            command.Parameters.AddWithValue("@idTipoContato", endereco.IdTipoContato);
            command.Parameters.AddWithValue("@idEnderecoContato", endereco.IdEnderecoContato);

            command.ExecuteNonQuery();
        }
        catch (MySqlException sqlEx)
        {
            throw new Exception("Erro de banco de dados ao atualizar o endereço de contato.", sqlEx);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro inesperado ao atualizar o endereço de contato.", ex);
        }        
    }

    public EnderecoContatoModel BuscaEnderecoContato(int id)
    {
        try
        {
            using var connection = Conexao.GetConnection;
            connection.Open();

            var sql = "SELECT * FROM EnderecoContato WHERE idEnderecoContato = @idEnderecoContato";

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@idEnderecoContato", id);

            using var reader = command.ExecuteReader();

            var endereco = new EnderecoContatoModel();

            while (reader.Read())
            {
                endereco.IdEnderecoContato = reader.GetInt32("idEnderecoContato");
                endereco.Valor = reader.GetString("valor");
                endereco.Observacao = reader.GetString("observacao");
                endereco.IdTipoContato = reader.GetInt32("idTipoContato");
                endereco.IdContato = reader.GetInt32("idContato");
            }

            return endereco;
        
        }
        catch (MySqlException sqlEx)
        {
            throw new Exception("Erro de banco de dados ao buscar o endereço de contato.", sqlEx);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro inesperado ao buscar o endereço de contato.", ex);
        }
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

    public void DeletaEnderecoContato(int id)
    {
        try
        {
            using var connection = Conexao.GetConnection;
            connection.Open();

            string sql = "DELETE FROM EnderecoContato WHERE idEnderecoContato = @idEnderecoContato";

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@idEnderecoContato", id);

            command.ExecuteNonQuery();
        }
        catch (MySqlException sqlEx)
        {
            throw new Exception("Erro de banco de dados ao deletar o endereço de contato.", sqlEx);
        }
        catch (Exception ex)
        {
            throw new Exception("Erro inesperado ao deletar o contato.", ex);
        }
    }
}