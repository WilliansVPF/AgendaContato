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
    private readonly IRequestMapper<RegistraUsuarioViewModel, UsuarioModel> _requestMapper;

    public HomeController(ILogger<HomeController> logger, IUsuarioRepository usuarioRepository, IHashSenha hashSenha, ISessao sessao, IValidaSenha validaSenha,
                          IValidaEmail validaEmail, IRequestMapper<RegistraUsuarioViewModel, UsuarioModel> requestMapper)
    {
        _usuarioRepository = usuarioRepository;
        _logger = logger;
        _hashSenha = hashSenha;
        _sessao = sessao;
        _validaSenha = validaSenha;
        _validaEmail = validaEmail;
        _requestMapper = requestMapper;
    }
    public IActionResult Index()
    {
        var usuario = _sessao.ObterUsuarioSessao();
        if (usuario != null) return RedirectToAction("Index", "User");

        return View();
    }

    public IActionResult Login(IFormCollection form)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        } 

        var usuario = _usuarioRepository.ObterUsuarioPorEmail(form["email"]);       

        var senha = _hashSenha.GerarHash(form["senha"], usuario.Salt);

        if (usuario.Senha != senha)
        {
            ModelState.AddModelError("Usuario", "Senha incorreta");
            return View("Index");
        }

        var usuarioSessao = new UsuarioSessaoModel
        {
            Id = usuario.IdUsuario,
            Nome = usuario.Nome,
            Email = usuario.Email
        };

        _sessao.CriarSessao(usuarioSessao);

        
        return RedirectToAction("Index", "User");
    }

    [HttpPost]
    public IActionResult Registrar(RegistraUsuarioViewModel registraUsuario)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }

        // Verifica se o email é válido
        if (!_validaEmail.EmailValido(registraUsuario.Email, out string erroEmail))
        {
            ModelState.AddModelError("Email", erroEmail);
            return View("Index");
        }

        // Verifica se o email já existe
        if (_usuarioRepository.EmailExiste(registraUsuario.Email))
        {
            ModelState.AddModelError("Email", "Email já cadastrado");
            return View("Index");
        }

        if (!_validaSenha.SenhaValida(registraUsuario.Senha, out string erroSenha))
        {
            ModelState.AddModelError("Senha", erroSenha);
            return View("Index"); // Garante que o form permaneça preenchido
        }

        var salt = _hashSenha.GerarSalt;
        var senha = _hashSenha.GerarHash(registraUsuario.Senha, salt);

        var usuario = _requestMapper.ToModel(registraUsuario);

        usuario.Senha = senha;
        usuario.Salt = salt;

        // var usuario = new UsuarioModel
        // {
        //     Nome = registraUsuario.Nome,
        //     Email = registraUsuario.Email,
        //     Senha = senha,
        //     Salt = salt
        // };

        _usuarioRepository.CadastrarUsuario(usuario);

        usuario = _usuarioRepository.ObterUsuarioPorEmail(usuario.Email);

        var usuarioSessao = new UsuarioSessaoModel
        {
            Id = usuario.IdUsuario,
            Nome = usuario.Nome,
            Email = usuario.Email
        };

        _sessao.CriarSessao(usuarioSessao);

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
