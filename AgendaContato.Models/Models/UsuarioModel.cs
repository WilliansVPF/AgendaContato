using System.ComponentModel.DataAnnotations;

namespace  AgendaContato.Models.Models
{
    public class UsuarioModel
    {
        public int? IdUsuario { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string Senha { get; set; }
        public string? Salt { get; set; }
    }
}