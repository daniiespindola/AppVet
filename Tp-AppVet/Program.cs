using Microsoft.EntityFrameworkCore;
using Tp_AppVet.Data;
using Microsoft.AspNetCore.Authentication.Cookies; // Aseg�rate de tener este using

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Configuraci�n de la Base de Datos (DbContext)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// 2. Configuraci�n de la Autenticaci�n y Google
builder.Services.AddAuthentication(options =>
{
    // Establece las Cookies como el esquema de autenticaci�n por defecto
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.Google.GoogleDefaults.AuthenticationScheme;
})
    .AddCookie() // Habilita la autenticaci�n basada en Cookies
    .AddGoogle(options =>
    {
        // Carga las claves desde la secci�n "Authentication:Google" en appsettings.json
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });

builder.Services.AddAuthorization(); // Ya lo ten�as.


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

// 3. Middlewares de Autenticaci�n y Autorizaci�n
app.UseAuthentication(); // <-- Es CRUCIAL para que Google y Cookies funcionen
app.UseAuthorization();  // Ya lo ten�as.

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();