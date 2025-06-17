namespace AgendaContato.Models.Models
{
    public class EnderecoContatoModel
    {
        public int IdEnderecoContato { get; set; }
        public string Valor { get; set; }
        public string? Observacao { get; set; }
        public int IdTipoContato { get; set; }
        public int IdContato { get; set; }
    }
}