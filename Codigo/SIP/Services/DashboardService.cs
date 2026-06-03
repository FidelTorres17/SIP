using Microsoft.Data.SqlClient;
using SIP.Models;
using System.Data;

namespace SIP.Services
{
    public class DashboardService
    {
        private readonly string _connectionString;

        public DashboardService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("sipConnect");
        }

        public DashboardVm ObtenerDashboard(bool esAdmin, int usuarioId, string nombreUsuario)
        {
            var vm = new DashboardVm
            {
                EsAdmin = esAdmin,
                NombreUsuario = nombreUsuario
            };

            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("stGetDashboardResumen", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EsAdmin", esAdmin);
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        vm.TotalPacientes = Convert.ToInt32(dr["TotalPacientes"]);
                        vm.CitasHoy = Convert.ToInt32(dr["CitasHoy"]);
                        vm.SesionesSemana = Convert.ToInt32(dr["SesionesSemana"]);
                        vm.PagosMes = Convert.ToDecimal(dr["PagosMes"]);
                    }
                }
            }

            vm.ProximasCitas = ListarProximasCitas(esAdmin, usuarioId);

            vm.EstatusCitasLabels = new List<string>
{
    "Pendientes", "Confirmadas", "Canceladas", "Atendidas"
};

            vm.EstatusCitasData = new List<int>
{
    5, 8, 2, 12
};

            return vm;
        }

        private List<ProximaCitaVm> ListarProximasCitas(bool esAdmin, int usuarioId)
        {
            var lista = new List<ProximaCitaVm>();

            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("stGetDashboardProximasCitas", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EsAdmin", esAdmin);
                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new ProximaCitaVm
                        {
                            CitaId = Convert.ToInt32(dr["CitaId"]),
                            FechaHora = Convert.ToDateTime(dr["FechaHora"]),
                            Paciente = dr["Paciente"]?.ToString() ?? "",
                            Tipo = dr["Tipo"]?.ToString() ?? "",
                            Estado = dr["Estado"]?.ToString() ?? ""
                        });
                    }
                }
            }

            return lista;
        }


    }
}
