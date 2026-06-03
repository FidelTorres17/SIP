namespace SIP.Models
{
    public class ReporteIndexVm
    {
        public DateTime FechaInicio { get; set; } = DateTime.Today.AddDays(-30);
        public DateTime FechaFin { get; set; } = DateTime.Today;

        public bool EsAdmin { get; set; }
        public int? UsuarioIdFiltro { get; set; }

        public int TotalSesiones { get; set; }
        public int PacientesAtendidos { get; set; }
        public decimal DuracionPromedio { get; set; }

        public decimal TotalIngresos { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal TotalPendiente { get; set; }

        public List<ReporteSesionVm> Sesiones { get; set; } = new();
        public List<ReporteFinancieroVm> Finanzas { get; set; } = new();
    }

    public class ReporteSesionVm
    {
        public DateTime Fecha { get; set; }
        public string Paciente { get; set; } = "";
        public string Terapeuta { get; set; } = "";
        public string? Motivo { get; set; }
        public string? Diagnostico { get; set; }
        public string EstadoSesion { get; set; } = "";
    }

    public class ReporteFinancieroVm
    {
        public DateTime FechaPago { get; set; }
        public string Paciente { get; set; } = "";
        public int SesionId { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } = "";
        public string EstatusPago { get; set; } = "";
        public string? Referencia { get; set; }
    }
}
