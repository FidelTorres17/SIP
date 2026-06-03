namespace SIP.Models
{
    public class PagoSesion
    {
        public int PagoSesionId { get; set; }
        public int SesionId { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public int MetodoPagoId { get; set; } 
        public int EstatusPagoId { get; set; } 
        public string? Referencia { get; set; }
        public string? Comentarios { get; set; }

        // Solo para mostrar
        public string MetodoPago { get; set; } = "";
        public string EstatusPago { get; set; } = "";
    }
}
