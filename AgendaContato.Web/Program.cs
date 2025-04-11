using AgendaContato.Application.Services;
using AgendaContato.CrossCutting.Filters;
using AgendaContato.Interfaces.Interfaces;
using AgendaContato.Repository.Repository;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//  container de injeção de dependencia repositories
builder.Services.AddScoped<IUsuarioRepository,UsuarioRepository>();

// injeção de dependencia HttpContextAccessor
builder.Services.AddScoped<HttpContextAccessor, HttpContextAccessor>();

// container de injeção de dependencia services
builder.Services.AddScoped<IHashSenha, HashSenha>();
builder.Services.AddScoped<ISessao, SessaoManager>();

// container de injeção de dependencia filtro de exceção
builder.Services.AddScoped<GlobalExceptionFilter>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new TypeFilterAttribute(typeof(GlobalExceptionFilter)));
});

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
