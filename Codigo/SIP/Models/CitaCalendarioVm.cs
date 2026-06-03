namespace SIP.Models
{
    public class CitaCalendarioVm
    {
        public int CitaId { get; set; }
        public int? ExpedienteId { get; set; }
        public DateTime FechaHora { get; set; }

        public string PacienteNombre { get; set; } = "";
        public string TerapeutaNombre { get; set; } = "";
        public string Estatus { get; set; } = "";
        public string? Notas { get; set; }
    }
}
