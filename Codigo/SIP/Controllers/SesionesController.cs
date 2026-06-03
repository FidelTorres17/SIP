using Microsoft.AspNetCore.Mvc;
using SIP.Models;
using SIP.Services;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SIP.Controllers
{
    public class SesionesController : Controller
    {
        private readonly CitaService _citaService;
        private readonly SesionService _sesionService;
        private readonly ExpedienteService _expedienteService;
        public SesionesController(
            CitaService citaService, SesionService sesionService, ExpedienteService expedienteService)
        {
            _citaService = citaService;
            _sesionService = sesionService;
            _expedienteService = expedienteService;
        }
        public IActionResult Iniciar(int citaId)
        {
            var cita = _citaService.ObtenerPorId(citaId);

            if (cita == null)
            {
                TempData["Error"] = "No se encontró la cita.";
                return RedirectToAction("Index", "Citas");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var expediente = _expedienteService.lstExpedienteIndex(int.Parse(userId)).FirstOrDefault(x => x.PacienteId == cita.PacienteId);

            if (expediente == null)
            {
                return RedirectToAction("Crear", "Expedientes", new { citaId = citaId });
            }

            int sesionId = _sesionService.IniciarDesdeCita(citaId, expediente.ExpedienteId);

            return RedirectToAction("Editar", "Sesiones", new { id = sesionId });
        }

        [HttpGet]
        public IActionResult Crear(int citaId)
        {
            //// 1) Validar: si ya existe sesión para esa cita, mandarlo a Editar
            //var sesionExistente = _sesionService.ObtenerPorCitaId(citaId);
            //if (sesionExistente != null)
            //    return RedirectToAction("Editar", new { id = sesionExistente.SesionId });

            //// 2) Traer info de cita (FechaHora, Paciente, Terapeuta, ExpedienteId)
            //var cita = _citaService.ObtenerPorId(citaId); // reutilizas tu back estilo BG

            //var vm = new SesionEditVm
            //{
            //    CitaId = cita.CitaId,
            //    ExpedienteId = cita.ExpedienteId ?? 0,
            //    UsuarioId = cita.UsuarioId,
            //    FechaHora = cita.FechaHora, // precargado de la cita

            //    PacienteNombre = cita.PacienteNombre,
            //    TerapeutaNombre = cita.TerapeutaNombre,
            //    CitaFechaHora = cita.FechaHora
            //};

            var vm = new SesionEditVm
            {
                CitaId = citaId,
                ExpedienteId = 0,
                UsuarioId = 1,
                FechaHora = DateTime.Now.AddDays(2).AddHours(17), // precargado de la cita

                PacienteNombre = "Ana López",
                TerapeutaNombre = "Dra. Maricela"
                //FechaHora = DateTime.Now.AddDays(2).AddHours(17)
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var vm = _sesionService.ObtenerParaEditar(id);

            if (vm == null)
            {
                TempData["Error"] = "No se encontró la sesión.";
                return RedirectToAction("Index", "Citas");
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(SesionEditVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            _sesionService.Actualizar(vm);

            TempData["Ok"] = "Sesión guardada correctamente.";
            return RedirectToAction("Editar", new { id = vm.SesionId });
        }

        //public IActionResult Editar(int id)
        //{
        //    var vm = new SesionEditVm
        //    {
        //        SesionId = id,
        //        CitaId = 501,
        //        ExpedienteId = 12,
        //        PacienteId = 9,
        //        UsuarioId = 2,

        //        PacienteNombre = "Ana López Martínez",
        //        TerapeutaNombre = "Dra. María Torres",
        //        FechaHora = DateTime.Today.AddHours(10),
        //        EstatusCita = "Confirmada",

        //        Motivo = "Ansiedad generalizada y dificultades para dormir.",
        //        Diagnostico = "Trastorno de ansiedad generalizada (preliminar).",
        //        NotasClinicas = "Se trabajó respiración diafragmática. Identifica pensamientos automáticos.\nAcordamos registro de sueño.",
        //        Recomendaciones = "Ejercicios de respiración 2 veces al día.\nRegistro de sueño por 7 días.",

        //        CostoSesion = 600,
        //        TotalPagado = 300,
        //        Pagos = new List<PagoRowVm>
        //        {
        //            new PagoRowVm{
        //                PagoId=1, FechaPago=DateTime.Today.AddHours(9),
        //                Monto=300, Metodo="Transferencia", Estatus="Parcial", Referencia="BBVA-12345"
        //            }
        //        }
        //    };

        //    return View(vm);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Editar(SesionEditVm vm)
        //{
        //    TempData["Ok"] = "Sesión actualizada (demo).";
        //    return RedirectToAction("Editar", new { id = vm.SesionId });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Finalizar(int sesionId)
        {
            // ✅ DEMO: aquí después llamas a tu service/sp
            // _sesionService.FinalizarSesion(sesionId, UserId);
            _sesionService.Finalizar(sesionId);

            TempData["Ok"] = "Sesión finalizada correctamente.";
            return RedirectToAction("Editar", new { id = sesionId });
        }


    }
}
