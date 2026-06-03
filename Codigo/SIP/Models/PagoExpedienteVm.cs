namespace SIP.Models
{
    public class PagoExpedienteVm
    {
        public int PagoSesionId { get; set; }
        public int SesionId { get; set; }
        public int CitaId { get; set; }

        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }

        public string MetodoPago { get; set; } = "";
        public string EstatusPago { get; set; } = "";
        public string? Referencia { get; set; }
        public string? Comentarios { get; set; }

        public DateTime FechaSesion { get; set; }
    }
}
