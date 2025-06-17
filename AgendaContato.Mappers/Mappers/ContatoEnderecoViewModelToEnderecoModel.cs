using AgendaContato.Interfaces.Interfaces;
using AgendaContato.Models.Models;
using AgendaContato.Models.ViewModels;

namespace AgendaContato.Mappers.Mappers
{
    public class EnderecoContatoViewModelToEnderecoContatoModel : IRequestMapper<ContatoEnderecoViewModel, EnderecoContatoModel>
    {
        public EnderecoContatoModel ToModel(ContatoEnderecoViewModel request)
        {
            if (request == null)
                return null;

            return new EnderecoContatoModel
            {
                Valor = request.Endereco.Valor,
                Observacao = request.Endereco.Observacao,
                IdTipoContato = request.Endereco.IdTipoContato
            };
        }
    }
}
