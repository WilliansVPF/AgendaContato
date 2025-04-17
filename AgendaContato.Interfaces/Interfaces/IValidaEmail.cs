namespace AgendaContato.Interfaces.Interfaces;

public interface IValidaEmail
{
    bool EmailValido(string email, out string mensagemErro);
}