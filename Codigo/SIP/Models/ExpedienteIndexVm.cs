namespace SIP.Models
{
    public class ExpedienteIndexVm
    {
        public int ExpedienteId { get; set; }
        public int PacienteId { get; set; }
        public string NombrePaciente { get; set; } = "";
        public int Edad { get; set; }
        public string Sexo { get; set; } = "";
        public string Terapeuta { get; set; } = "";
        public int TotalSesiones { get; set; }
        public string UltimaSesionTexto { get; set; } = "";
        public string Estatus { get; set; } = "Activo";
    }
}
