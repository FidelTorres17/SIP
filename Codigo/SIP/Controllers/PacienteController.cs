using Microsoft.AspNetCore.Mvc;
using SIP.Models;
using SIP.Services;
using System.Security.Claims;

namespace SIP.Controllers
{
    public class PacienteController : Controller
    {
        private readonly PacienteService _pacienteService;
        private readonly ExpedienteService _expedienteService;

        public PacienteController(
            PacienteService pacienteService, ExpedienteService expedienteService)
        {
            _pacienteService = pacienteService;
            _expedienteService = expedienteService;
        }
        public IActionResult Index()
        {
            List<Paciente> _paciente = new List<Paciente>();
            _paciente = _pacienteService.lstPaciente();
            return View(_paciente);
        }

        // Método para guardar o actualizar
        [HttpPost]
        public JsonResult guardarPaciente(Paciente paciente)
        {
            if (paciente.PacienteId == null)
            {
                // Crear nuevo 
                var _resutl = _pacienteService.crearPaciente(paciente);
                if (_resutl.ReturnCode != 1000)
                {
                    return Json(new { success = false });
                }
            }
            else
            {
                // Editar
                var _resutl = _pacienteService.editarPaciente(paciente);
                if (_resutl.ReturnCode != 1000)
                {
                    return Json(new { success = false });
                }
            }

            return Json(new { success = true });
        }

        public IActionResult AbrirExpediente(int pacienteId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var expediente = _expedienteService.lstExpedienteIndex(int.Parse(userId)).FirstOrDefault(x => x.PacienteId == pacienteId);


            if (expediente == null)
            {
                TempData["Error"] = "El paciente no tiene expediente activo.";
                return RedirectToAction("Index");
            }

            return RedirectToAction(
                "Detalle",
                "Expedientes",
                new { id = expediente.ExpedienteId });
        }
    }
}
