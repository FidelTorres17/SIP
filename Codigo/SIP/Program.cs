using Microsoft.AspNetCore.Authentication.Cookies;
using SIP.Models;
using SIP.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// AUTHENTICATION
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";     // ruta al login
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccesoDenegado";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

// REGISTRA AQUÍ TUS SERVICIOS
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<RolService>();
builder.Services.AddScoped<PacienteService>();
builder.Services.AddScoped<CitaService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<ExpedienteService>();
builder.Services.AddScoped<SesionService>();
builder.Services.AddScoped<PagoSesionService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<ReporteService>();

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
    //name: "default",
    //pattern: "{controller=Home}/{action=Index}/{id?}");
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
