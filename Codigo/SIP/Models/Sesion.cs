namespace SIP.Models
{
    public class Sesion
    {
        public int SesionId { get; set; }

        public int CitaId { get; set; }
        public int ExpedienteId { get; set; }
        public int UsuarioId { get; set; } // terapeuta

        public DateTime FechaHora { get; set; }

        public string? Motivo { get; set; }
        public string? Diagnostico { get; set; }
        public string? NotasClinicas { get; set; }
        public string? Recomendaciones { get; set; }
    }
}
