using SangueBom.Components;
using SangueBom.Domain.Interfaces;
using SangueBom.Domain.Repositories;
using SangueBom.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Remova um dos registros duplicados
builder.Services.AddSingleton<ICadastroDoadorRepositorio, CadastroDoadorRepositorio>();
builder.Services.AddSingleton<IRepositorioDoacao, RepositorioDoacaoEmMemoria>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();