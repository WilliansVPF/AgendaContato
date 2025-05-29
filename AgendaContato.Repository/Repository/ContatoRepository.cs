using AgendaContato.Interfaces.Interfaces;
using AgendaContato.Models.Models;
using AgendaContato.Models.ViewModels;
using Google.Protobuf;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Tsp;

namespace AgendaContato.Repository.Repository;

public class ContatoRepository : IContatoRepository
{
    public IEnumerable<ExibeContatosViewModel> CarregaContatos(int? idUsuario)
    {        
        try
        {
            using var connection = Conexao.GetConnection;
            connection.Open();

            var contatos = ObterContatos(connection, idUsuario);
            var lista = MontaListaExibeContatos(connection, contatos);

            return lista;
        }
        catch (MySqlException sqlEx)
        {
            throw new Exception($"Erro de banco de dados ao tentar carregar os contatos", sqlEx);
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro inesperado ao tentar carregar os contatos", ex);
        }        
    }

    private List<ContatoModel> ObterContatos(MySqlConnection connection, int? idUsuario)
    {
        var lista = new List<ContatoModel>();

        string sql = "SELECT * FROM Contato WHERE idUsuario = @idUsuario";

        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@idUsuario", idUsuario);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new ContatoModel
            {
                IdContato = Convert.ToInt32(reader["idContato"]),
                Nome = reader["nome"].ToString(),
                Sobrenome = reader["sobrenome"].ToString(),
                IdUsuario = Convert.ToInt32(reader["idUsuario"])
            });
        }

        return lista;
    }

    private List<ExibeContatosViewModel> MontaListaExibeContatos(MySqlConnection connection, List<ContatoModel> contatos)
    {
        var lista = new List<ExibeContatosViewModel>();

        foreach (var contato in contatos)
        {
            var enderecos = ObterEnderecosContato(connection, contato.IdContato);

            lista.Add(new ExibeContatosViewModel
            {
                Contato = contato,
                EnderecosContato = enderecos
            });
        }

        return lista;
    }

    private List<EnderecoContatoModel> ObterEnderecosContato(MySqlConnection connection, int? idContato)
    {
        var lista = new List<EnderecoContatoModel>();

        string sql = "SELECT * FROM EnderecoContato WHERE idContato = @idContato";

        using var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@idContato", idContato);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new EnderecoContatoModel
            {
            IdEnderecoContato = Convert.ToInt32(reader["idEnderecoContato"]),
            Valor = reader["valor"].ToString(),
            Observacao = reader["observacao"].ToString(),
            IdTipoContato = Convert.ToInt32(reader["idTipoContato"]),
            IdContato = Convert.ToInt32(reader["idContato"])
            });
        }

        return lista;
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