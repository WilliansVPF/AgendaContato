using AgendaContato.Interfaces.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgendaContato.Web.Components
{
    [ViewComponent(Name = "LoginViewComponent")]
    public class LoginViewComponent : ViewComponent
    {
        private readonly ISessao _sessao;

        public LoginViewComponent(ISessao sessao)
        {
            _sessao = sessao;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var usuario = _sessao.ObterUsuarioSessao();

            if (usuario == null) return View("Default");
            else return View("Logado", usuario);
        }
    }
}