using Microsoft.AspNetCore.Mvc;
using SIP.Services;

namespace SIP.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ReporteService _reporteService;

        public ReportesController(ReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        public IActionResult Index(DateTime? fechaInicio, DateTime? fechaFin)
        {
            bool esAdmin = User.IsInRole("Administrador");
            int usuarioId = Convert.ToInt32(User.FindFirst("UsuarioId")?.Value ?? "0");

            DateTime inicio = fechaInicio ?? DateTime.Today.AddDays(-30);
            DateTime fin = fechaFin ?? DateTime.Today;

            var vm = _reporteService.ObtenerReportes(inicio, fin, esAdmin, usuarioId);

            return View(vm);
        }
    }
}
