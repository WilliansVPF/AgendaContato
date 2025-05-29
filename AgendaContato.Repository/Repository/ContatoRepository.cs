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
        var lista = new List<ExibeContatosViewModel>();

        try
        {
            using var connection = Conexao.GetConnection;
            connection.Open();

            string sqlContatos = "SELECT * FROM Contato WHERE idUsuario = @idUsuario";

            using var commandContato = new MySqlCommand(sqlContatos, connection);
            commandContato.Parameters.AddWithValue("@idUsuario", idUsuario);

            using var readerContato = commandContato.ExecuteReader();
            var contatos = new List<ContatoModel>();

            while (readerContato.Read())
            {
                contatos.Add(new ContatoModel
                {
                    IdContato = Convert.ToInt32(readerContato["idContato"]),
                    Nome = readerContato["nome"].ToString(),
                    Sobrenome = readerContato["sobrenome"].ToString(),
                    IdUsuario = Convert.ToInt32(readerContato["idUsuario"])
                });
            }

            readerContato.Close();

            foreach (var contato in contatos)
            {
                string sqlEnderecos = "SELECT * FROM EnderecoContato WHERE idContato = @idContato";

                using var commandEndereco = new MySqlCommand(sqlEnderecos, connection);
                commandEndereco.Parameters.AddWithValue("@idContato", contato.IdContato);

                using var readEndereco = commandEndereco.ExecuteReader();
                var enderecos = new List<EnderecoContatoModel>();

                while (readEndereco.Read())
                {
                    enderecos.Add(new EnderecoContatoModel
                    {
                        IdEnderecoContato = Convert.ToInt32(readEndereco["idEnderecoContato"]),
                        Valor = readEndereco["valor"].ToString(),
                        Observacao = readEndereco["observacao"].ToString(),
                        IdTipoContato = Convert.ToInt32(readEndereco["idTipoContato"]),
                        IdContato = Convert.ToInt32(readEndereco["idContato"])
                    });
                }

                lista.Add(new ExibeContatosViewModel
                {
                    Contato = contato,
                    EnderecosContato = enderecos
                });

            }
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