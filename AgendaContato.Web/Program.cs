using AgendaContato.Application.Services;
using AgendaContato.Application.Validations;
using AgendaContato.CrossCutting.Filters;
using AgendaContato.Interfaces.Interfaces;
using AgendaContato.Models.Models;
using AgendaContato.Models.ViewModels;
using AgendaContato.Repository.Repository;
using Microsoft.AspNetCore.Mvc;
using AgendaContato.Mappes.Mappes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

//  container de injeção de dependencia repositories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IContatoRepository, ContatoRepository>();
builder.Services.AddScoped<IEnderecoContatoRepository, EnderecoContatoRepository>();

// injeção de dependencia HttpContextAccessor
builder.Services.AddSingleton<HttpContextAccessor, HttpContextAccessor>();

// container de injeção de dependencia services
builder.Services.AddScoped<IHashSenha, HashSenha>();
builder.Services.AddScoped<ISessao, SessaoManager>();
builder.Services.AddScoped<IValidaSenha, ValidaSenha>();
builder.Services.AddScoped<IValidaEmail, ValidaEmail>();
builder.Services.AddScoped<IRequestMapper<RegistraUsuarioViewModel, UsuarioModel>, RegistraUsuarioViewModelToUsuarioModel>();

// container de injeção de dependencia filtro de exceção
builder.Services.AddScoped<GlobalExceptionFilter>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new TypeFilterAttribute(typeof(GlobalExceptionFilter)));
});

builder.Services.AddSession(
    o =>
    {
        // o.IdleTimeout = TimeSpan.FromMinutes(30);
        o.Cookie.HttpOnly = true;
        o.Cookie.IsEssential = true;
    }
);

builder.Services.Scan(scan => scan
    .FromAssemblyOf<RegistraUsuarioViewModelToUsuarioModel>()
    .AddClasses(classes => classes.AssignableTo(typeof(IRequestMapper<,>)))
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
