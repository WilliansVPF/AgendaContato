using AgendaContato.Interfaces.Interfaces;

namespace AgendaContato.Application.Services;

public class HashSenha : IHashSenha
{
    public string GerarSalt => BCrypt.Net.BCrypt.GenerateSalt();
    public string GerarHash(string senha, string salt) => BCrypt.Net.BCrypt.HashPassword(senha, salt);
}