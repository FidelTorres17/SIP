namespace SIP.Models
{
    public class DashboardVm
    {
        public bool EsAdmin { get; set; }
        public string NombreUsuario { get; set; } = "";

        public int TotalPacientes { get; set; }
        public int CitasHoy { get; set; }
        public int SesionesSemana { get; set; }
        public decimal PagosMes { get; set; }

        public List<ProximaCitaVm> ProximasCitas { get; set; } = new();

        public List<string> EstatusCitasLabels { get; set; } = new();
        public List<int> EstatusCitasData { get; set; } = new();
    }

    public class ProximaCitaVm
    {
        public int CitaId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Paciente { get; set; } = "";
        public string Tipo { get; set; } = "";
        public string Estado { get; set; } = "";
    }
}