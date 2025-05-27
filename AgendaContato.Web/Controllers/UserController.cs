using System.Security.Cryptography.X509Certificates;
using AgendaContato.Interfaces.Interfaces;
using AgendaContato.Models.Models;
using AgendaContato.Models.ViewModels;
using AgendaContato.Repository.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AgendaContato.Web.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly ISessao _sessao;

    public UserController(ILogger<UserController> logger, ISessao sessao)
    {
        _logger = logger;
        _sessao = sessao;
    }

    public IActionResult Index()
    {
        var usuario = _sessao.ObterUsuarioSessao();
        if (usuario == null) return RedirectToAction("Index", "Home");
        return View();
    }

    public IActionResult Logout()
    {
        _sessao.RemoverSessao();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult CadastraContato()
    {
        return View();
    }


    public IActionResult RegistrarContato(ContatoEnderecoViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("CadastraContato");
        }

        ContatoModel contato = ContatoEnderecoToContato(viewModel);
        EnderecoContatoModel enderecoContato = ContatoEnderecoToEnderecoContato(viewModel);

        return View();
    }

    private ContatoModel ContatoEnderecoToContato(ContatoEnderecoViewModel viewModel)
    {
        return new ContatoModel
        {
            Nome = viewModel.Contato.Nome,
            Sobrenome = viewModel.Contato.Sobrenome
        };
    }

    private EnderecoContatoModel ContatoEnderecoToEnderecoContato(ContatoEnderecoViewModel viewModel)
    {
        return new EnderecoContatoModel
        {
            Valor = viewModel.Endereco.Valor,
            Observacao = viewModel.Endereco.Observacao,
            IdTipoContato = viewModel.Endereco.IdTipoContato
        };
    }
}