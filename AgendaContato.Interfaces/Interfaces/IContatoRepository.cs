using AgendaContato.Models.Models;
using AgendaContato.Models.ViewModels;

namespace AgendaContato.Interfaces.Interfaces;

public interface IContatoRepository
{
    void NovoContato(ContatoModel contato, EnderecoContatoModel endereco, int? idUsuario);

    IEnumerable<ExibeContatosViewModel> CarregaContatosEnderecos(int? idUsuario);

    void DeletaContato(int id);

    void AtualizaContato(ContatoModel contato);

    ContatoModel CarregaContato(int id);
}