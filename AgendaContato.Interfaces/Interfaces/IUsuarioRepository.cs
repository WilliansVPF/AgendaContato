using AgendaContato.Models.Models;

namespace AgendaContato.Interfaces.Interfaces;

public interface IUsuarioRepository
{
    bool CadastrarUsuario(UsuarioModel usuario);

    bool EmailExiste(string email);
}