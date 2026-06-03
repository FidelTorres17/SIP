using Microsoft.AspNetCore.Mvc;
using SIP.Models;
using SIP.Services;
using System.Diagnostics;

namespace SIP.Controllers
{
    public class HomeController : Controller
    {
        private readonly DashboardService _dashboardService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            DashboardService dashboardService,
            ILogger<HomeController> logger)
        {
            _dashboardService = dashboardService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            bool esAdmin = User.IsInRole("Administrador");

            int usuarioId = Convert.ToInt32(
                User.FindFirst("UsuarioId")?.Value ?? "0");

            string nombre = User.Identity?.Name ?? "Usuario";

            var vm = _dashboardService.ObtenerDashboard(
                esAdmin,
                usuarioId,
                nombre);

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
