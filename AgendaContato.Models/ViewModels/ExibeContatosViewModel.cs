using AgendaContato.Models.Models;

namespace AgendaContato.Models.ViewModels;

public class ExibeContatosViewModel
{
    public ContatoModel Contato { get; set; }

    public List<EnderecoContatoModel> EnderecosContato { get; set; }
}