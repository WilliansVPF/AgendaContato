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

    private readonly IContatoRepository _contatoRepository;

    private readonly IEnderecoContatoRepository _enderecoContaoReposittory;

    private readonly IRequestMapper<ContatoEnderecoViewModel, ContatoModel> _requestContatoMapper;

    private readonly IRequestMapper<ContatoEnderecoViewModel, EnderecoContatoModel> _requestEnderecoMapper;

    public UserController(ILogger<UserController> logger, ISessao sessao, IContatoRepository contatoRepository,
                          IEnderecoContatoRepository enderecoContatoRepository, IRequestMapper<ContatoEnderecoViewModel, ContatoModel> requestContatoMapper,
                          IRequestMapper<ContatoEnderecoViewModel, EnderecoContatoModel> requestEnderecoMapper)
    {
        _logger = logger;
        _sessao = sessao;
        _contatoRepository = contatoRepository;
        _enderecoContaoReposittory = enderecoContatoRepository;
        _requestContatoMapper = requestContatoMapper;
        _requestEnderecoMapper = requestEnderecoMapper;
    }

    public IActionResult Index()
    {
        var usuario = _sessao.ObterUsuarioSessao();
        if (usuario == null) return RedirectToAction("Index", "Home");

        var listaContatos = _contatoRepository.CarregaContatosEnderecos(usuario.Id);

        return View(listaContatos);
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

    [HttpPost]
    public IActionResult RegistrarContato(ContatoEnderecoViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View("CadastraContato");
        }

        ContatoModel contato = _requestContatoMapper.ToModel(viewModel);
        EnderecoContatoModel enderecoContato = _requestEnderecoMapper.ToModel(viewModel);

        var usuario = _sessao.ObterUsuarioSessao();

        _contatoRepository.NovoContato(contato, enderecoContato, usuario.Id);

        return RedirectToAction("Index", "User");
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

    public IActionResult CadastraEnderecoContato(int idContato, int idEnderecoContato)
    {
        var enderecoContato = new EnderecoContatoModel
        {
            IdContato = idContato
        };

        if (idEnderecoContato != 0) enderecoContato = _enderecoContaoReposittory.BuscaEnderecoContato(idEnderecoContato);

        return View(enderecoContato);
    }

    public IActionResult RegistrarEnderecoContato(EnderecoContatoModel endereco)
    {
        if (!ModelState.IsValid)
        {
            return View("CadastraContato");
        }

        if (endereco.IdEnderecoContato != null) _enderecoContaoReposittory.AtualizaEndereco(endereco);
        else if (!_enderecoContaoReposittory.CadastraEnderecoCOntato(endereco)) return View();

        return RedirectToAction("Index", "User");
    }

    public IActionResult DeletaContato(int id)
    {
        if (id == 0) return NotFound();
        _contatoRepository.DeletaContato(id);
        return RedirectToAction("Index", "User");
    }

    public IActionResult AtualizaContato(int id)
    {
        if (id == 0) return NotFound();
        var contato = _contatoRepository.CarregaContato(id);
        return View(contato);
    }

    public IActionResult Atualiza(ContatoModel contato)
    {
        if (!ModelState.IsValid) return NotFound();

        _contatoRepository.AtualizaContato(contato);

        return Json(new { success = true, message = "Contato atualizado com sucesso!"});
    }

    public IActionResult DeletaEndereco(int id)
    {
        if (id == 0) return NotFound();
        _enderecoContaoReposittory.DeletaEnderecoContato(id);
        return RedirectToAction("Index", "User");
    }

    public IActionResult EditaContatoModal(int id)
    {
        var contato = _contatoRepository.CarregaContato(id);
        if (contato == null) return NotFound();

        return PartialView("_EditaContato", contato);
    }
}