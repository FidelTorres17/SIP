using System.ComponentModel.DataAnnotations;

namespace SIP.Models
{
    public class LoginUsr
    {
        [Required(ErrorMessage = "El usuario o correo es obligatorio.")]
        public string UserName { get; set; } = "";

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        public bool RememberMe { get; set; } = false;

        public string? ReturnUrl { get; set; }
    }
}
