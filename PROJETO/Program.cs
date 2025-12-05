using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PROJETO.Models; // lembre de incluir isso para o EmailSettings

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços MVC
builder.Services.AddControllersWithViews();

// Configuração de autenticação via Cookie
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Home/Login";
        options.AccessDeniedPath = "/Home/Login";
    });

// ✅ CONFIGURAÇÃO DO EMAIL TEM QUE VIR ANTES DO build()
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);

var app = builder.Build();

// Configuração do pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();