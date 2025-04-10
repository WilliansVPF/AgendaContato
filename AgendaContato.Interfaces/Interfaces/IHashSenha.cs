namespace AgendaContato.Interfaces.Interfaces
{
    public interface IHashSenha
    {
        string GerarSalt { get; }
        string GerarHash(string senha, string salt);
    }
}