namespace AgendaContato.Interfaces.Interfaces;

public interface IValidaSenha
{
    bool SenhaValida(string senha, out string mensagemErro);
}