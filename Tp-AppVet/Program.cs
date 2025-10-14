using Microsoft.EntityFrameworkCore;
using Tp_AppVet.Data;
using Microsoft.AspNetCore.Authentication.Cookies; // Asegúrate de tener este using

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Configuración de la Base de Datos (DbContext)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// 2. Configuración de la Autenticación y Google
builder.Services.AddAuthentication(options =>
{
    // Establece las Cookies como el esquema de autenticación por defecto
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.Google.GoogleDefaults.AuthenticationScheme;
})
    .AddCookie() // Habilita la autenticación basada en Cookies
    .AddGoogle(options =>
    {
        // Carga las claves desde la sección "Authentication:Google" en appsettings.json
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });

builder.Services.AddAuthorization(); // Ya lo tenías.


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

// 3. Middlewares de Autenticación y Autorización
app.UseAuthentication(); // <-- Es CRUCIAL para que Google y Cookies funcionen
app.UseAuthorization();  // Ya lo tenías.

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();