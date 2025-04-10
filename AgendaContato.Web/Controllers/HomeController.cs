using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AgendaContato.Models.Models;
using AgendaContato.Interfaces.Interfaces;
using AgendaContato.Models.ViewModels;

namespace AgendaContato.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IHashSenha _hashSenha;

    public HomeController(ILogger<HomeController> logger, IUsuarioRepository usuarioRepository, IHashSenha hashSenha)
    {
        _usuarioRepository = usuarioRepository;
        _logger = logger;
        _hashSenha = hashSenha;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login(UsuarioLogin usuarioLogin)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }

        var salt = _hashSenha.GerarSalt;
        var senha = _hashSenha.GerarHash(usuarioLogin.Senha, salt);



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

        var salt = _hashSenha.GerarSalt;
        var senha = _hashSenha.GerarHash(usuarioModel.Senha, salt);

        var usuario = new UsuarioModel
        {
            Nome = usuarioModel.Nome,
            Email = usuarioModel.Email,
            Senha = senha,
            Salt = salt
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
