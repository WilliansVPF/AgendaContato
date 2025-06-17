using AgendaContato.Interfaces.Interfaces;
using AgendaContato.Models.Models;
using AgendaContato.Models.ViewModels;

namespace AgendaContato.Mappes.Mappes;

public class RegistraUsuarioViewModelToUsuarioModel : IRequestMapper<RegistraUsuarioViewModel, UsuarioModel>
{
    public UsuarioModel ToModel(RegistraUsuarioViewModel request)
    {
        var usuario = new UsuarioModel
        {
            Nome = request.Nome,
            Email = request.Email
        };

        return usuario;
    }
}