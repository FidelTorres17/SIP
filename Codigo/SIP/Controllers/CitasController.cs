using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIP.Models;
using SIP.Services;

namespace SIP.Controllers
{
    public class CitasController : Controller
    {
        private readonly CitaService _citaService;
        private readonly UsuarioService _usuarioService;

        public CitasController(
            CitaService citaService, UsuarioService usuarioService)
        {
            _citaService = citaService;
            _usuarioService = usuarioService;
        }
        public IActionResult Index()
        {
            List<Cita> _cita = new List<Cita>();
            _cita = _citaService.lstCita();
            return View(_cita);
        }

        // Método para guardar o actualizar
        [HttpPost]
        public JsonResult guardarCita(Cita cita)
        {
            if (cita.CitaId == null)
            {
                // Crear nuevo 
                var _resutl = _citaService.crearCita(cita);
                if (_resutl.ReturnCode != 1000)
                {
                    return Json(new { success = false });
                }
            }
            else
            {
                // Editar
                var _resutl = _citaService.editarCita(cita);
                if (_resutl.ReturnCode != 1000)
                {
                    return Json(new { success = false });
                }
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult Calendario()
        {
            bool esAdmin = User.IsInRole("Administrador");

            var vm = new CalendarioCitasVm
            {
                EsAdmin = esAdmin
            };

            if (esAdmin)
            {
                var terapeutas = _usuarioService.ListarTerapeutas();

                vm.Terapeutas = terapeutas.Select(t => new SelectListItem
                {
                    Value = t.UsuarioId.ToString(),
                    Text = $"{t.Nombre} {t.ApellidoP} {t.ApellidoM}"
                }).ToList();
            }

            return View(vm);
        }

        //[HttpGet]
        //public IActionResult CalendarioData()
        //{
        //    var citas = _citaService.ListarCitasCalendario();

        //    var eventos = citas.Select(c => new
        //    {
        //        id = c.CitaId,
        //        title = c.PacienteNombre + " - " + c.TerapeutaNombre,
        //        start = c.FechaHora.ToString("yyyy-MM-ddTHH:mm:ss"),
        //        end = c.FechaHora.AddMinutes(60).ToString("yyyy-MM-ddTHH:mm:ss"),
        //        backgroundColor = ObtenerColorEstatus(c.Estatus),
        //        borderColor = ObtenerColorEstatus(c.Estatus),
        //        extendedProps = new
        //        {
        //            estatus = c.Estatus,
        //            notas = c.Notas
        //        }
        //    });

        //    return Json(eventos);
        //}

        [HttpGet]
        public IActionResult CalendarioData(int? usuarioId)
        {
            bool esAdmin = User.IsInRole("Administrador");

            int? filtroUsuarioId;

            if (esAdmin)
            {
                filtroUsuarioId = usuarioId; // admin puede filtrar o ver todos
            }
            else
            {
                filtroUsuarioId = Convert.ToInt32(User.FindFirst("UsuarioId")?.Value);
            }

            var citas = _citaService.ListarCitasCalendario(filtroUsuarioId);

            var eventos = citas.Select(c => new
            {
                id = c.CitaId,
                title = c.PacienteNombre + " - " + c.TerapeutaNombre,
                start = c.FechaHora.ToString("yyyy-MM-ddTHH:mm:ss"),
                end = c.FechaHora.AddMinutes(60).ToString("yyyy-MM-ddTHH:mm:ss"),
                backgroundColor = ObtenerColorEstatus(c.Estatus),
                borderColor = ObtenerColorEstatus(c.Estatus),
                extendedProps = new
                {
                    citaId = c.CitaId,
                    expedienteId = c.ExpedienteId,
                    estatus = c.Estatus,
                    notas = c.Notas
                }
            });

            return Json(eventos);
        }

        private string ObtenerColorEstatus(string estatus)
        {
            return estatus switch
            {
                "Pendiente" => "#C9A227",
                "Confirmada" => "#2E8B57",
                "Cancelada" => "#B04A3A",
                "Atendida" => "#7A4A2E",
                _ => "#6c757d"
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GuardarDesdePaciente(Cita cita)
        {
            bool esAdmin = User.IsInRole("Administrador");

            int usuarioIdLogueado =
                Convert.ToInt32(User.FindFirst("UsuarioId")?.Value ?? "0");

            if (!esAdmin)
            {
                cita.UsuarioId = usuarioIdLogueado;
            }

            if (cita.PacienteId <= 0)
                return Json(new { ok = false, mensaje = "Paciente no válido." });

            if (cita.UsuarioId <= 0)
                return Json(new { ok = false, mensaje = "Terapeuta no válido." });

            if (cita.EstatusCitaId <= 0)
                return Json(new { ok = false, mensaje = "Selecciona el estatus de la cita." });

            if (cita.FechaHora == null)
                return Json(new { ok = false, mensaje = "Selecciona fecha y hora." });

            var result = _citaService.crearCita(cita);

            if (result.ReturnCode != 1000)
                return Json(new { ok = false, mensaje = "No se pudo registrar la cita." });

            return Json(new { ok = true, mensaje = "Cita registrada correctamente." });
        }
    }
}
