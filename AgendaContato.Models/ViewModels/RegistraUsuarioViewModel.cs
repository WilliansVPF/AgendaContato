using System.ComponentModel.DataAnnotations;

namespace AgendaContato.Models.ViewModels;

public class RegistraUsuarioViewModel
{
    [Required(ErrorMessage = "Campo obrigatório")]
    public required string Nome { get; set; }

    [Required(ErrorMessage = "Campo obrigatório")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Campo obrigatório")]
    public required string Senha { get; set; }

    [Compare("Senha", ErrorMessage = "As senhas não conferem")]
    [Required(ErrorMessage = "Campo obrigatório")]
    public required string ConfirmarSenha { get; set; }
}