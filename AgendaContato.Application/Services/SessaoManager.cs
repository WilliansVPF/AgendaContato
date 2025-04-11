using System.Text.Json;
using AgendaContato.Interfaces.Interfaces;
using AgendaContato.Models.Models;
using Microsoft.AspNetCore.Http;

namespace AgendaContato.Application.Services;

public class SessaoManager : ISessao
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessaoManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public void CriarSessao(UsuarioModel usuario)
    {
        string sessao = JsonSerializer.Serialize(usuario);
        _httpContextAccessor.HttpContext.Session.SetString("SessaoUsuario", sessao);
    }

    public UsuarioModel ObterUsuarioSessao()
    {
        string sessao = _httpContextAccessor.HttpContext.Session.GetString("SessaoUsuario");

        if (string.IsNullOrEmpty(sessao)) return null;

        return JsonSerializer.Deserialize<UsuarioModel>(sessao);
    }

    public void RemoverSessao()
    {
        _httpContextAccessor.HttpContext.Session.Remove("SessaoUsuario");
    }

}