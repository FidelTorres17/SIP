using Microsoft.Data.SqlClient;
using SIP.Models;
using System.Data;

namespace SIP.Services
{
    public class SesionService
    {
        private readonly string _connectionString;

        public SesionService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("sipConnect");
        }

        public int IniciarDesdeCita(int citaId, int? expedienteId)
        {

            int _result = 0;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("stIniciarDesdeCita", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 30; // puedes ajustar

                command.Parameters.AddWithValue("@CitaId", citaId);
                command.Parameters.AddWithValue("@ExpedienteId", expedienteId);
                

                SqlParameter outReturnCode = new SqlParameter("@ReturnCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                SqlParameter outSesionId = new SqlParameter("@SesionId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(outReturnCode);
                command.Parameters.Add(outSesionId);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    var _returnCode = int.Parse(outReturnCode.Value.ToString());
                    _result = int.Parse(outSesionId.Value.ToString());
                    //response.ReturnCode = 0; // Éxito
                }
                catch (Exception ex)
                {
                    _result = 0;
                    //response.ReturnCode = 900; // Código de error personalizado
                    //Console.WriteLine($"Error en EditarUsuario: {ex.Message}");
                    // Aquí puedes usar tu logger o E_Log.save si ya lo tienes implementado
                    // E_Log.save(this, ex);
                }
            }

            return _result;
        }

        public SesionEditVm? ObtenerParaEditar(int sesionId)
        {
            SesionEditVm? vm = null;

            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stGetSesionId", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SesionId", sesionId);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            vm = new SesionEditVm
                            {
                                SesionId = Convert.ToInt32(dr["SesionId"]),
                                CitaId = Convert.ToInt32(dr["CitaId"]),
                                UsuarioId = Convert.ToInt32(dr["UsuarioId"]),
                                ExpedienteId = Convert.ToInt32(dr["ExpedienteId"]),
                                FechaHora = Convert.ToDateTime(dr["Fecha"]),

                                Motivo = dr["Motivo"]?.ToString(),
                                Diagnostico = dr["Diagnostico"]?.ToString(),
                                NotasClinicas = dr["NotasClinicas"]?.ToString(),
                                Recomendaciones = dr["Recomendaciones"]?.ToString(),

                                PacienteNombre = dr["PacienteNombre"]?.ToString() ?? "",
                                TerapeutaNombre = dr["TerapeutaNombre"]?.ToString() ?? "",

                                EstadoSesion = dr["EstadoSesion"]?.ToString() ?? "En curso",
                                FechaFin = dr["FechaFin"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaFin"]),
                            };
                        }
                    }
                }
            }

            return vm;
        }

        public void Actualizar(SesionEditVm vm)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("stUpdSesion", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SesionId", vm.SesionId);
                    cmd.Parameters.AddWithValue("@Motivo", (object?)vm.Motivo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Diagnostico", (object?)vm.Diagnostico ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NotasClinicas", (object?)vm.NotasClinicas ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Recomendaciones", (object?)vm.Recomendaciones ?? DBNull.Value);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Finalizar(int sesionId)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("stUpdFinalizarSesion", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SesionId", sesionId);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
