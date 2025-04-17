using System.Text.RegularExpressions;
using AgendaContato.Interfaces.Interfaces;

namespace AgendaContato.Application.Validations;

public class ValidaSenha : IValidaSenha
{
    public bool SenhaValida(string senha, out string mensagemErro)
        {
            mensagemErro = string.Empty;

            if (!VerificaSenhaVazial(senha, out mensagemErro)) return false;

            if (!VerificaTamanhoSenha(senha, out mensagemErro)) return false;

            if (!VerificaLetraMaiuscula(senha, out mensagemErro)) return false;

            if (!VerificaLetraMinuscula(senha, out mensagemErro)) return false;

            if (!VerificaLetraNumero(senha, out mensagemErro)) return false;

            if (!VerificaLetraCaracterEspecial(senha, out mensagemErro)) return false;
            
            return true;
        }

        private bool VerificaSenhaVazial(string senha, out string mensagemErro)
        {
            mensagemErro = string.Empty;

            if (string.IsNullOrWhiteSpace(senha))
            {
                mensagemErro = "Senha não pode ser vazia.";
                return false;
            }

            return true;
        }

        private bool VerificaTamanhoSenha(string senha, out string mensagemErro)
        {
            mensagemErro = string.Empty;

            if (senha.Length < 8 || senha.Length > 32)
            {
                mensagemErro = "A senha deve ter entre 8 e 32 caracteres.";
                return false;
            }

            return true;
        }

        private bool VerificaLetraMaiuscula(string senha, out string mensagemErro)
        {
            mensagemErro = string.Empty;

            if (!Regex.IsMatch(senha, "[A-Z]"))
            {
                mensagemErro = "A senha deve conter ao menos uma letra maiúscula.";
                return false;
            }

            return true;
        }

        private bool VerificaLetraMinuscula(string senha, out string mensagemErro)
        {
            mensagemErro = string.Empty;

            if (!Regex.IsMatch(senha, "[a-z]"))
            {
                mensagemErro = "A senha deve conter ao menos uma letra minúscula.";
                return false;
            }

            return true;
        }

        private bool VerificaLetraNumero(string senha, out string mensagemErro)
        {
            mensagemErro = string.Empty;

            if (!Regex.IsMatch(senha, "[0-9]"))
            {
                mensagemErro = "A senha deve conter ao menos um número.";
                return false;
            }

            return true;
        }

        private bool VerificaLetraCaracterEspecial(string senha, out string mensagemErro)
        {
            mensagemErro = string.Empty;

            if (!Regex.IsMatch(senha, "[^a-zA-Z0-9]"))
            {
                mensagemErro = "A senha deve conter ao menos um caractere especial.";
                return false;
            }

            return true;
        }


}