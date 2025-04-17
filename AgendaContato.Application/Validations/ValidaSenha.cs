using System.Text.RegularExpressions;
using AgendaContato.Interfaces.Interfaces;

namespace AgendaContato.Application.Validations;

public class ValidaSenha : IValidaSenha
{
    public bool SenhaValida(string senha, out string mensagemErro)
        {
            mensagemErro = string.Empty;

            if (string.IsNullOrWhiteSpace(senha))
            {
                mensagemErro = "Senha não pode ser vazia.";
                return false;
            }

            if (senha.Length < 8 || senha.Length > 32)
            {
                mensagemErro = "A senha deve ter entre 8 e 32 caracteres.";
                return false;
            }

            if (!Regex.IsMatch(senha, "[A-Z]"))
            {
                mensagemErro = "A senha deve conter ao menos uma letra maiúscula.";
                return false;
            }

            if (!Regex.IsMatch(senha, "[a-z]"))
            {
                mensagemErro = "A senha deve conter ao menos uma letra minúscula.";
                return false;
            }

            if (!Regex.IsMatch(senha, "[0-9]"))
            {
                mensagemErro = "A senha deve conter ao menos um número.";
                return false;
            }

            if (!Regex.IsMatch(senha, "[^a-zA-Z0-9]"))
            {
                mensagemErro = "A senha deve conter ao menos um caractere especial.";
                return false;
            }

            return true;
        }
}