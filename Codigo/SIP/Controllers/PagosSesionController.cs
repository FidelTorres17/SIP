using Microsoft.AspNetCore.Mvc;
using SIP.Models;
using SIP.Services;

namespace SIP.Controllers
{
    public class PagosSesionController : Controller
    {
        private readonly PagoSesionService _pagoSesionService;

        public PagosSesionController(PagoSesionService pagoSesionService)
        {
            _pagoSesionService = pagoSesionService;
        }

        [HttpGet]
        public IActionResult Listar(int sesionId)
        {
            var pagos = _pagoSesionService.ListarPorSesion(sesionId);

            var data = pagos.Select(p => new
            {
                pagoSesionId = p.PagoSesionId,
                fecha = p.FechaPago.ToString("dd/MM/yyyy hh:mm tt"),
                montoTexto = "$ " + p.Monto.ToString("N2"),
                metodoPago = p.MetodoPago,
                estatusPago = p.EstatusPago,
                referencia = string.IsNullOrWhiteSpace(p.Referencia) ? "-" : p.Referencia,
                comentarios = p.Comentarios ?? ""
            });

            return Json(new { ok = true, data });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Guardar(PagoSesion model)
        {
            if (model.SesionId <= 0)
                return Json(new { ok = false, mensaje = "Sesión no válida." });

            if (model.Monto <= 0)
                return Json(new { ok = false, mensaje = "El monto debe ser mayor a cero." });

            if (model.MetodoPagoId <= 0)
                return Json(new { ok = false, mensaje = "Selecciona un método de pago." });

            if (model.EstatusPagoId <= 0)
                return Json(new { ok = false, mensaje = "Selecciona un estatus de pago." });

            _pagoSesionService.crear(model);

            return Json(new { ok = true, mensaje = "Pago registrado correctamente." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int pagoSesionId)
        {
            _pagoSesionService.Eliminar(pagoSesionId);

            return Json(new { ok = true, mensaje = "Pago eliminado correctamente." });
        }
    }
}
