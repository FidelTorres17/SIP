namespace SIP.Models
{
    public class Usuario
    {
        public int? UsuarioId { get; set; }
        public string Nombre { get; set; }
        public string ApellidoP { get; set; }
        public string ApellidoM { get; set; }
        public int RolId { get; set; }
        public string Rol { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Activo { get; set; }
    }
}
