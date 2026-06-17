using Microsoft.AspNetCore.Mvc;
using SIP.Models;
using SIP.Services;

namespace SIP.Controllers
{
    public class ExpedientesController : Controller
    {
        private readonly ExpedienteService _expedienteService;
        private readonly CitaService _citaService;
        private readonly SesionService _sesionService;

        public ExpedientesController(
            ExpedienteService expedienteService,
            CitaService citaService,
            SesionService sesionService)
        {
            _expedienteService = expedienteService;
            _citaService = citaService;
            _sesionService = sesionService;
        }

        [HttpGet]
        public IActionResult Crear(int citaId)
        {
            var cita = _citaService.ObtenerPorId(citaId);

            if (cita == null)
            {
                TempData["Error"] = "No se encontró la cita.";
                return RedirectToAction("Index", "Citas");
            }

            var vm = new ExpedienteEditVm
            {
                CitaId = cita.CitaId,
                PacienteId = cita.PacienteId,
                UsuarioId = cita.UsuarioId,
                FechaInicio = DateTime.Now,
                PacienteNombre = cita.Paciente,
                TerapeutaNombre = cita.Terapeuta
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(ExpedienteEditVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var _result = _expedienteService.crearExpediente(vm);
            int? expedienteId = _result.ReturnId;

            int sesionId = _sesionService.IniciarDesdeCita(vm.CitaId, expedienteId);

            TempData["Ok"] = "Expediente creado correctamente. Ahora puedes capturar la sesión.";
            return RedirectToAction("Editar", "Sesiones", new { id = sesionId });
        }
        public IActionResult Index()
        {
            var lista = new List<ExpedienteIndexVm>
    {
        new() {
            ExpedienteId = 1,
            NombrePaciente = "Ana López",
            Edad = 24,
            Sexo = "F",
            Terapeuta = "Dra. María Torres",
            TotalSesiones = 3,
            UltimaSesionTexto = "15/01/2026",
            Estatus = "Activo"
        },
        new() {
            ExpedienteId = 2,
            NombrePaciente = "Carlos Pérez",
            Edad = 30,
            Sexo = "M",
            Terapeuta = "Dr. Juan Ruiz",
            TotalSesiones = 5,
            UltimaSesionTexto = "10/01/2026",
            Estatus = "Activo"
        }
    };

            return View(lista);
        }

        [HttpGet]
        public IActionResult Detalle(int id)
        {
            var vm = _expedienteService.ObtenerDetalle(id);

            if (vm == null)
            {
                TempData["Error"] = "No se encontró el expediente.";
                return RedirectToAction("Index", "Pacientes");
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult AbrirExpediente(int pacienteId)
        {
            var expediente = _expedienteService.ObtenerActivoPorPaciente(pacienteId);

            if (expediente == null || expediente.ExpedienteId == null)
            {
                TempData["Error"] = "Este paciente aún no tiene expediente activo. El expediente se creará al iniciar una sesión.";
                return RedirectToAction("Index", "Paciente");
            }

            return RedirectToAction("Detalle", "Expedientes", new { id = expediente.ExpedienteId });
        }
    }
}
