namespace AgendaContato.Models.Models
{
    public class ContatoModel
    {
        public int? IdContato { get; set; }
        public string Nome { get; set; }
        public string? Sobrenome { get; set; }        
        public int? IdUsuario { get; set; }
    }
}