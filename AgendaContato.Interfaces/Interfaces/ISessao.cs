using AgendaContato.Models.Models;

namespace AgendaContato.Interfaces.Interfaces
{
    public interface ISessao
    {
        void CriarSessao(UsuarioModel usuario);
        void RemoverSessao();
        UsuarioModel ObterUsuarioSessao();
    }
}