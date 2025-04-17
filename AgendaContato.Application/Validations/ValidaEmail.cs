using System.Text.RegularExpressions;
using AgendaContato.Interfaces.Interfaces;

namespace AgendaContato.Application.Validations;


public class ValidaEmail : IValidaEmail
{
    public bool EmailValido(string email, out string mensagemErro)
    {
        if (!VerificaEmailValido(email, out mensagemErro)) return false;

        return true;
    }

    private bool VerificaEmailValido(string email, out string mensagemErro)
    {
        mensagemErro = string.Empty;

            if (!Regex.IsMatch(email.Trim(), @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                mensagemErro = "formato de E-Mail inválido";
                return false;
            }

            return true;
    }
}