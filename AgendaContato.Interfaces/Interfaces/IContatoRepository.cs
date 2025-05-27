using AgendaContato.Models.Models;

namespace AgendaContato.Interfaces.Interfaces;

public interface IContatoRepository
{
    void NovoContato(ContatoModel contato, EnderecoContatoModel endereco, int idUsuario);
}