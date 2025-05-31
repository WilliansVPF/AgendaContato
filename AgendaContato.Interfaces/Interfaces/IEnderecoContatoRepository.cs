using AgendaContato.Models.Models;

namespace AgendaContato.Interfaces.Interfaces;

public interface IEnderecoContatoRepository
{
    bool CadastraEnderecoCOntato(EnderecoContatoModel endereco);

    EnderecoContatoModel BuscaEnderecoContato(int id);

    void AtualizaEndereco(EnderecoContatoModel endereco);

    void DeletaEnderecoContato(int id);
}