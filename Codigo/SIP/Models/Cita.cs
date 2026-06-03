namespace SIP.Models
{
    public class Cita
    {
        public int CitaId { get; set; }
        public int UsuarioId { get; set; }
        public int ExpedienteId { get; set; }
        public int PacienteId { get; set; }
        public DateTime? FechaHora { get; set; }
        public string Paciente { get; set; } = "";
        public string Terapeuta { get; set; } = "";
        public int EstatusCitaId { get; set; }
        public string Estatus { get; set; } = "";
        public string? Notas { get; set; }
    }
}
