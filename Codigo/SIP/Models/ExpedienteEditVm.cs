namespace SIP.Models
{
    public class ExpedienteEditVm
    {
        public int? ExpedienteId { get; set; }
        public int CitaId { get; set; }
        public int PacienteId { get; set; }
        public int UsuarioId { get; set; }

        public DateTime FechaInicio { get; set; } = DateTime.Now;
        public DateTime? FechaCierre { get; set; }

        public string? MotivoConsulta { get; set; }
        public string? DiagnosticoPreliminar { get; set; }
        public string? Observaciones { get; set; }
        public string? AntecedentesPersonales { get; set; }
        public string? AntecedentesFamiliares { get; set; }
        public string? HistorialMedico { get; set; }

        public string PacienteNombre { get; set; } = "";
        public string TerapeutaNombre { get; set; } = "";
    }
}
