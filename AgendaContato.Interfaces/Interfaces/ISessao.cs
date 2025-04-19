using AgendaContato.Models.Models;
using AgendaContato.Models.ViewModels;

namespace AgendaContato.Interfaces.Interfaces
{
    public interface ISessao
    {
        void CriarSessao(UsuarioSessaoModel usuario);
        void RemoverSessao();
        UsuarioSessaoModel ObterUsuarioSessao();
    }
}