using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AgendaContato.Models.Models;
using AgendaContato.Interfaces.Interfaces;

namespace AgendaContato.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUsuarioRepository _usuarioRepository;

    public HomeController(ILogger<HomeController> logger, IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
        _logger = logger;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Registrar(UsuarioModel usuarioModel)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }

        // Verifica se o email já existe
        if (_usuarioRepository.EmailExiste(usuarioModel.Email))
        {
            ModelState.AddModelError("Email", "Email já cadastrado");
            return View("Index");
        }

        var usuario = new UsuarioModel
        {
            Nome = usuarioModel.Nome,
            Email = usuarioModel.Email,
            Senha = usuarioModel.Senha
        };

        _usuarioRepository.CadastrarUsuario(usuario);

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
