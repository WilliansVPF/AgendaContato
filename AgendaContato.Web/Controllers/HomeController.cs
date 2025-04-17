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
    private readonly ISessao _sessao;
    private readonly IValidaSenha _validaSenha;
    private readonly IValidaEmail _validaEmail;

    public HomeController(ILogger<HomeController> logger, IUsuarioRepository usuarioRepository, IHashSenha hashSenha, ISessao sessao, IValidaSenha validaSenha,
                          IValidaEmail validaEmail)
    {
        _usuarioRepository = usuarioRepository;
        _logger = logger;
        _hashSenha = hashSenha;
        _sessao = sessao;
        _validaSenha = validaSenha;
        _validaEmail = validaEmail;
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

        var usuario = _usuarioRepository.ObterUsuarioPorEmail(usuarioLogin.Email);



        return RedirectToAction("Index", "User");
    }

    [HttpPost]
    public IActionResult Registrar(UsuarioModel usuarioModel)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }

        // Verifica se o email é válido
        if (!_validaEmail.EmailValido(usuarioModel.Email, out string erroEmail))
        {
            ModelState.AddModelError("Email", erroEmail);
            return View("Index");
        }

        // Verifica se o email já existe
        if (_usuarioRepository.EmailExiste(usuarioModel.Email))
        {
            ModelState.AddModelError("Email", "Email já cadastrado");
            return View("Index");
        }

        if (!_validaSenha.SenhaValida(usuarioModel.Senha, out string erroSenha))
        {
            ModelState.AddModelError("Senha", erroSenha);
            return View("Index"); // Garante que o form permaneça preenchido
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

        usuario = _usuarioRepository.ObterUsuarioPorEmail(usuarioModel.Email);

        _sessao.CriarSessao(usuario);

        return RedirectToAction("Index", "User");
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
