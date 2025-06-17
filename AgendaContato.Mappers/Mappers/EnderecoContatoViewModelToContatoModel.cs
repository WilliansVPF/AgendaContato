using AgendaContato.Interfaces.Interfaces;
using AgendaContato.Models.Models;
using AgendaContato.Models.ViewModels;

namespace AgendaContato.Mappers.Mappers
{
    public class ContatoEnderecoViewModelToContatoModel : IRequestMapper<ContatoEnderecoViewModel, ContatoModel>
    {
        public ContatoModel ToModel(ContatoEnderecoViewModel request)
        {
            if (request == null)
                return null;

            return new ContatoModel
            {
                Nome = request.Contato.Nome,
                Sobrenome = request.Contato.Sobrenome
            };
        }
    }
}
