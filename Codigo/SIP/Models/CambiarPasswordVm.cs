namespace SIP.Models
{
    public class CambiarPasswordVm
    {
        public string PasswordActual { get; set; } = "";
        public string PasswordNueva { get; set; } = "";
        public string ConfirmarPassword { get; set; } = "";
    }
}
