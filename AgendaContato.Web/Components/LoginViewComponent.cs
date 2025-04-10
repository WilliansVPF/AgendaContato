using Microsoft.AspNetCore.Mvc;

namespace AgendaContato.Web.Components
{
    [ViewComponent(Name = "LoginViewComponent")]
    public class LoginViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("Default");
        }
    }
}