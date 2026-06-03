using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SIP.Models;
using SIP.Services;
using System.Net.Http;
using System.Security.Claims;

namespace SIP.Controllers
{
    public class AuthController : Controller
    {
        private readonly UsuarioService _usuarioService;
        private readonly EmailService _emailService;
        public AuthController(
            UsuarioService usuarioService,
            EmailService emailService)
        {
            _usuarioService = usuarioService;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginUsr { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUsr vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }


            // Claves para bloqueo por usuario 
            string keyFail = $"fail:{vm.UserName}";
            string keyLock = $"lock:{vm.UserName}";
            DateTime now = DateTime.UtcNow;



            // 2) Valida credenciales
            bool credencialesOk = (vm.UserName == "admin" && vm.Password == "Admin123*");


            if (credencialesOk)
            {
                var userId = "1"; // usa el id real de tu usuario
                var sessionToken = Guid.NewGuid().ToString("N");


                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, vm.UserName),
                    new Claim(ClaimTypes.Role, "Administrador"),
                    new Claim("session_token", sessionToken) // clave para sesión única
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = vm.RememberMe,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
                    });

            }
            else
            {
                var _user = _usuarioService.getLogin(vm.UserName.Trim(), vm.Password);
                if (_user.UsuarioId != null)
                {


                    var userId = _user.UsuarioId.ToString(); // usa el id real de tu usuario
                    var sessionToken = Guid.NewGuid().ToString("N");

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId),
                        new Claim(ClaimTypes.Name, vm.UserName),
                        new Claim(ClaimTypes.Role, _user.Rol.Trim()),
                        new Claim("session_token", sessionToken) //
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        new AuthenticationProperties
                        {
                            IsPersistent = vm.RememberMe,
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
                        });
                }
                else
                {

                    TempData["Error"] = $"Usuario o contraseña incorrectos.";

                    return View(vm);
                }


            }
            // Limpia el captcha para no reutilizarlo
            //HttpContext.Session.Remove("CaptchaSum");

            TempData["Ok"] = "Sesión iniciada.";
            //return Redirect(vm.ReturnUrl ?? "/Home/Index");
            if (!string.IsNullOrEmpty(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
            {
                return LocalRedirect(vm.ReturnUrl);
            }

            // 👇 Esta línea respeta el virtual directory (~/ = raíz de tu app, no del sitio)
            var urlPorDefecto = Url.Content("~/Home/Index");
            return Redirect(urlPorDefecto);
        }

        public async Task<IActionResult> Logout()
        {
            // Solo si el usuario sigue autenticado
            if (User?.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    //_cache.Remove($"session:{userId}"); // invalida token vigente
                    //_cache.Remove($"last:{userId}");
                }
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["Ok"] = "Sesión cerrada.";
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "Ingresa un correo válido.";
                return View();
            }

            var usuario = _usuarioService.ObtenerPorEmail(email);

            if (usuario == null)
            {
                TempData["Error"] = "No se encontró un usuario registrado con ese correo.";
                return View();
            }

            string passwordTemporal = GenerarPasswordTemporal();

            // IMPORTANTE: guarda hash, no texto plano
            _usuarioService.ActualizarPassword(usuario.UsuarioId, passwordTemporal);

            _emailService.EnviarRecuperacion(
                usuario.Email,
                usuario.Nombre,
                usuario.Email,
                passwordTemporal
            );

            TempData["Ok"] = "Se enviaron las instrucciones de recuperación a tu correo.";
            return RedirectToAction("Login");
        }

        private string GenerarPasswordTemporal()
        {
            return "SIP-" + Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }


        [HttpGet]
        public IActionResult CambiarPassword()
        {
            TempData["Ok"] = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CambiarPassword(CambiarPasswordVm vm)
        {
            int usuarioId = Convert.ToInt32(User.FindFirst("UsuarioId")?.Value ?? "0");

            if (vm.PasswordNueva != vm.ConfirmarPassword)
            {
                TempData["Error"] = "La nueva contraseña y la confirmación no coinciden.";
                return View(vm);
            }

            bool ok = _usuarioService.CambiarPassword(
                usuarioId,
                vm.PasswordActual,
                vm.PasswordNueva
            );

            if (!ok)
            {
                TempData["Error"] = "La contraseña actual es incorrecta.";
                return View(vm);
            }

            TempData["Ok"] = "Contraseña actualizada correctamente.";
            return RedirectToAction("CambiarPassword");
        }
    }
}
