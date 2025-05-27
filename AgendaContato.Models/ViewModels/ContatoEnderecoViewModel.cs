using AgendaContato.Models.Models;

namespace AgendaContato.Models.ViewModels;

public class ContatoEnderecoViewModel
{
    // public string Nome { get; set; }
    // public string Sobrenome { get; set; }
    // public string Telefone { get; set; }
    // public string Valor { get; set; }
    // public string Observacao { get; set; }
    // public string IdTipoContato { get; set; }
    // public int IdContato { get; set; }

    public ContatoModel Contato { get; set; }
    public EnderecoContatoModel Endereco { get; set; }
}