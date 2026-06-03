using Microsoft.Data.SqlClient;
using SIP.Models;
using System.Data;

namespace SIP.Services
{
    public class ReporteService
    {
        private readonly string _connectionString;

        public ReporteService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("sipConnect");
        }

        public ReporteIndexVm ObtenerReportes(DateTime fechaInicio, DateTime fechaFin, bool esAdmin, int usuarioId)
        {
            var vm = new ReporteIndexVm
            {
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                EsAdmin = esAdmin
            };

            vm.Sesiones = ListarSesiones(fechaInicio, fechaFin, esAdmin, usuarioId);
            vm.Finanzas = ListarFinanzas(fechaInicio, fechaFin, esAdmin, usuarioId);

            vm.TotalSesiones = vm.Sesiones.Count;
            vm.PacientesAtendidos = vm.Sesiones.Select(x => x.Paciente).Distinct().Count();
            vm.DuracionPromedio = vm.TotalSesiones > 0 ? 60 : 0;

            vm.TotalIngresos = vm.Finanzas.Sum(x => x.Monto);
            vm.TotalPagado = vm.Finanzas
                .Where(x => x.EstatusPago == "Pagado" || x.EstatusPago == "Parcial")
                .Sum(x => x.Monto);

            vm.TotalPendiente = vm.Finanzas
                .Where(x => x.EstatusPago == "Pendiente")
                .Sum(x => x.Monto);

            return vm;
        }

        private List<ReporteSesionVm> ListarSesiones(DateTime fechaInicio, DateTime fechaFin, bool esAdmin, int usuarioId)
        {
            var lista = new List<ReporteSesionVm>();

            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("stGetReporteSesiones", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio.Date);
                cmd.Parameters.AddWithValue("@FechaFin", fechaFin.Date);
                cmd.Parameters.AddWithValue("@EsAdmin", esAdmin);
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new ReporteSesionVm
                        {
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
                            Paciente = dr["Paciente"]?.ToString() ?? "",
                            Terapeuta = dr["Terapeuta"]?.ToString() ?? "",
                            Motivo = dr["Motivo"]?.ToString(),
                            Diagnostico = dr["Diagnostico"]?.ToString(),
                            EstadoSesion = dr["EstadoSesion"]?.ToString() ?? ""
                        });
                    }
                }
            }

            return lista;
        }

        private List<ReporteFinancieroVm> ListarFinanzas(DateTime fechaInicio, DateTime fechaFin, bool esAdmin, int usuarioId)
        {
            var lista = new List<ReporteFinancieroVm>();

            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("stGetReporteFinanciero", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio.Date);
                cmd.Parameters.AddWithValue("@FechaFin", fechaFin.Date);
                cmd.Parameters.AddWithValue("@EsAdmin", esAdmin);
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new ReporteFinancieroVm
                        {
                            FechaPago = Convert.ToDateTime(dr["FechaPago"]),
                            Paciente = dr["Paciente"]?.ToString() ?? "",
                            SesionId = Convert.ToInt32(dr["SesionId"]),
                            Monto = Convert.ToDecimal(dr["Monto"]),
                            MetodoPago = dr["MetodoPago"]?.ToString() ?? "",
                            EstatusPago = dr["EstatusPago"]?.ToString() ?? "",
                            Referencia = dr["Referencia"]?.ToString()
                        });
                    }
                }
            }

            return lista;
        }
    }
}
