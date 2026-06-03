namespace SIP.Models
{
    public class SesionEditVm
    {
        public int SesionId { get; set; }
        public int CitaId { get; set; }
        public int ExpedienteId { get; set; }
        public int PacienteId { get; set; }
        public int UsuarioId { get; set; } // terapeuta

        // Datos de contexto (solo lectura)
        public string PacienteNombre { get; set; } = "";
        public string TerapeutaNombre { get; set; } = "";
        public DateTime FechaHora { get; set; }
        public string EstatusCita { get; set; } = "Confirmada";

        // Campos clínicos
        public string? Motivo { get; set; }
        public string? Diagnostico { get; set; }
        public string? NotasClinicas { get; set; }
        public string? NotasCita { get; set; }
        public string? Recomendaciones { get; set; }

        // Pagos (dummy)
        public decimal CostoSesion { get; set; } = 600;
        public decimal TotalPagado { get; set; } = 300;
        public decimal Saldo => CostoSesion - TotalPagado;

        public string EstadoSesion { get; set; } = "En curso"; // En curso / Finalizada
        public DateTime? FechaFin { get; set; }               // cuando se finaliza

        public bool EstaFinalizada => EstadoSesion == "Finalizada";


        public List<PagoRowVm> Pagos { get; set; } = new();
    }

    public class PagoRowVm
    {
        public int PagoId { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string Metodo { get; set; } = "";
        public string Estatus { get; set; } = "";
        public string? Referencia { get; set; }
    }
}
