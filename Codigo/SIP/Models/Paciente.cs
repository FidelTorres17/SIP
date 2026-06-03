namespace SIP.Models
{
    public class Paciente
    {
        public int? PacienteId { get; set; }
        public string Nombre { get; set; }
        public string ApellidoP { get; set; }
        public string ApellidoM { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
    }
}
