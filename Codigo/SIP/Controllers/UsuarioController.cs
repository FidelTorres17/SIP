using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIP.Models;
using SIP.Services;

namespace SIP.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly RolService _rolService;
        private readonly UsuarioService _usuarioService;
        private readonly EmailService _emailService;

        public UsuarioController(
            RolService rolService,
            UsuarioService usuarioService,
            EmailService emailService)
        {
            _rolService = rolService;
            _usuarioService = usuarioService;
            _emailService = emailService;
        }
        public IActionResult Index()
        {
            //        var usuarios = new List<Usuario>
            //{
            //    new Usuario
            //    {
            //        UsuarioId = 1,
            //        Nombre = "Ana",
            //        ApellidoP = "López",
            //        ApellidoM = "Martínez",
            //        Email = "ana@correo.com",
            //        Telefono = "4491234567",
            //        UserName = "ana.lopez",
            //        Activo = true,
            //        RolId = 2 // Psicólogo
            //    },
            //    new Usuario
            //    {
            //        UsuarioId = 2,
            //        Nombre = "María",
            //        ApellidoP = "Pérez",
            //        ApellidoM = "García",
            //        Email = "maria@correo.com",
            //        Telefono = "4499876543",
            //        UserName = "maria.perez",
            //        Activo = false,
            //        RolId = 3 // Recepción
            //    },
            //    new Usuario
            //    {
            //        UsuarioId = 3,
            //        Nombre = "Carlos",
            //        ApellidoP = "Gómez",
            //        ApellidoM = "Ruiz",
            //        Email = "carlos@correo.com",
            //        Telefono = "4491112233",
            //        UserName = "carlos.gomez",
            //        Activo = true,
            //        RolId = 1 // Administrador
            //    }
            //};
            List<Usuario> _usuario = new List<Usuario>();
            _usuario = _usuarioService.lstUsuario();
            ViewBag.RolId = getRol();
            return View(_usuario);
        }

        // Método para guardar o actualizar
        [HttpPost]
        public JsonResult guardarUsuario(Usuario usuario)
        {
            if (usuario.UsuarioId == null)
            {
                // Crear nuevo usuario
                var _resutl = _usuarioService.crearUsuario(usuario);
                if(_resutl.ReturnCode != 1000)
                {
                    return Json(new { success = false });
                }

                _emailService.EnviarBienvenidaUsuario(
                    usuario.Email,
                    usuario.Nombre,
                    usuario.Email,
                    "SIP12345"
                );
            }
            else
            {
                // Editar usuario
                var _resutl = _usuarioService.editarUsuario(usuario);
                if (_resutl.ReturnCode != 1000)
                {
                    return Json(new { success = false });
                }
            }

            return Json(new { success = true });
        }

        // Método para eliminar
        [HttpPost]
        public JsonResult eliminarUsuario(int UsuarioId)
        {
            var _resutl = _usuarioService.eliminarUsuario(UsuarioId);
            if (_resutl.ReturnCode != 1000)
            {
                return Json(new { success = false });
            }
            return Json(new { success = true });
        }

        public SelectList getRol(string selectItem = null)
        {
            List<Rol> _rol = new List<Rol>();
            _rol = _rolService.lstRol();


            if (string.IsNullOrEmpty(selectItem))
            {
                return new SelectList(_rol.OrderBy(x => x.RolId), "RolId", "Nombre");
            }
            else
            {
                return new SelectList(_rol.OrderBy(x => x.RolId), "RolId", "Nombre", selectItem);
            }
        }
    }
}
