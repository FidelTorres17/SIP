namespace SIP.Models
{
    public class ExpedienteDetalleVm
    {
        public int ExpedienteId { get; set; }
        public int PacienteId { get; set; }

        public string NombrePaciente { get; set; } = "";
        public string Sexo { get; set; } = "";
        public string Email { get; set; } = "";
        public string Telefono { get; set; } = "";

        public DateTime? FechaNacimiento { get; set; }
        public int Edad => FechaNacimiento.HasValue
            ? DateTime.Today.Year - FechaNacimiento.Value.Year
            : 0;

        public string TerapeutaNombre { get; set; } = "";
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaCierre { get; set; }

        public string? MotivoConsulta { get; set; }
        public string? DiagnosticoPreliminar { get; set; }
        public string? Observaciones { get; set; }
        public string? AntecedentesPersonales { get; set; }
        public string? AntecedentesFamiliares { get; set; }
        public string? HistorialMedico { get; set; }

        public List<CitaExpedienteVm> Citas { get; set; } = new();
        public List<SesionExpedienteVm> Sesiones { get; set; } = new();

        public List<PagoExpedienteVm> Pagos { get; set; } = new();

        public decimal TotalPagado => Pagos
            .Where(p => p.EstatusPago == "Pagado" || p.EstatusPago == "Parcial")
            .Sum(p => p.Monto);

        public decimal TotalPendiente => Pagos
            .Where(p => p.EstatusPago == "Pendiente")
            .Sum(p => p.Monto);
    }

    public class CitaExpedienteVm
    {
        public int CitaId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Terapeuta { get; set; } = "";
        public string Estatus { get; set; } = "";
        public string? Notas { get; set; }
    }

    public class SesionExpedienteVm
    {
        public int SesionId { get; set; }
        public int CitaId { get; set; }
        public DateTime Fecha { get; set; }
        public string Terapeuta { get; set; } = "";
        public string? Motivo { get; set; }
        public string? Diagnostico { get; set; }
        public string EstadoSesion { get; set; } = "";
    }

}
