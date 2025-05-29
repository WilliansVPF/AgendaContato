using AgendaContato.Models.Models;
using AgendaContato.Models.ViewModels;

namespace AgendaContato.Interfaces.Interfaces;

public interface IContatoRepository
{
    void NovoContato(ContatoModel contato, EnderecoContatoModel endereco, int? idUsuario);

    IEnumerable<ExibeContatosViewModel> CarregaContatos(int? idUsuario);
}