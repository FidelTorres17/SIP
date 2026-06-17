using Microsoft.Data.SqlClient;
using SIP.Models;
using System.Data;

namespace SIP.Services
{
    public class ExpedienteService
    {
        private readonly string _connectionString;

        public ExpedienteService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("sipConnect");
        }

        public List<ExpedienteIndexVm> lstExpedienteIndex(int UsuarioId, int? ExpedienteId = null)
        {
            List<ExpedienteIndexVm> _list = new List<ExpedienteIndexVm>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("stGetExpedienteIndex", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ExpedienteId", ExpedienteId);
                        command.Parameters.AddWithValue("@UsuarioId", UsuarioId);
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ExpedienteIndexVm _expediente = new ExpedienteIndexVm();
                                _expediente.ExpedienteId = reader["ExpedienteId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ExpedienteId"]);
                                _expediente.PacienteId = reader["PacienteId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PacienteId"]);
                                _expediente.NombrePaciente = reader["Paciente"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Paciente"]);
                                _expediente.Edad = reader["Edad"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Edad"]);
                                _expediente.Terapeuta = reader["Terapeuta"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Terapeuta"]);
                                _expediente.Sexo = reader["Sexo"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Sexo"]);
                                _expediente.TotalSesiones = reader["TotalSesiones"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TotalSesiones"]);
                                _expediente.Estatus = reader["Activo"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Activo"]);
                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return _list;
        }

        public BasicResponse crearExpediente(ExpedienteEditVm expediente)
        {

            BasicResponse _result = new BasicResponse();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("stAddExpediente", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 30; // puedes ajustar

                command.Parameters.AddWithValue("@PacienteId", expediente.PacienteId);
                command.Parameters.AddWithValue("@UsuarioId", expediente.UsuarioId);
                command.Parameters.AddWithValue("@MotivoConsulta", expediente.MotivoConsulta);
                command.Parameters.AddWithValue("@AntecedentesPersonales", expediente.AntecedentesPersonales);
                command.Parameters.AddWithValue("@AntecedentesFamiliares", expediente.AntecedentesFamiliares);
                command.Parameters.AddWithValue("@HistorialMedico", expediente.HistorialMedico);
                command.Parameters.AddWithValue("@DiagnosticoPreliminar", expediente.DiagnosticoPreliminar);
                command.Parameters.AddWithValue("@Observaciones", expediente.Observaciones);

                SqlParameter outReturnCode = new SqlParameter("@ReturnCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                SqlParameter outReturnId = new SqlParameter("@ReturnId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(outReturnCode);
                command.Parameters.Add(outReturnId);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    var _returnCode = int.Parse(outReturnCode.Value.ToString());
                    _result.ReturnCode = int.Parse(outReturnCode.Value.ToString());
                    _result.ReturnId = int.Parse(outReturnId.Value.ToString());
                    //response.ReturnCode = 0; // Éxito
                }
                catch (Exception ex)
                {
                    _result.ReturnCode = 900;
                    //response.ReturnCode = 900; // Código de error personalizado
                    //Console.WriteLine($"Error en EditarUsuario: {ex.Message}");
                    // Aquí puedes usar tu logger o E_Log.save si ya lo tienes implementado
                    // E_Log.save(this, ex);
                }
            }

            return _result;
        }

        public ExpedienteDetalleVm? ObtenerDetalle(int expedienteId)
        {
            ExpedienteDetalleVm? vm = null;

            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("stGetExpedienteDetalle", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ExpedienteId", expedienteId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        vm = new ExpedienteDetalleVm
                        {
                            ExpedienteId = Convert.ToInt32(dr["ExpedienteId"]),
                            PacienteId = Convert.ToInt32(dr["PacienteId"]),
                            FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                            FechaCierre = dr["FechaCierre"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaCierre"]),

                            MotivoConsulta = dr["MotivoConsulta"]?.ToString(),
                            DiagnosticoPreliminar = dr["DiagnosticoPreliminar"]?.ToString(),
                            Observaciones = dr["Observaciones"]?.ToString(),
                            AntecedentesPersonales = dr["AntecedentesPersonales"]?.ToString(),
                            AntecedentesFamiliares = dr["AntecedentesFamiliares"]?.ToString(),
                            HistorialMedico = dr["HistorialMedico"]?.ToString(),

                            NombrePaciente = dr["NombrePaciente"]?.ToString() ?? "",
                            Sexo = dr["Sexo"]?.ToString() ?? "",
                            Email = dr["Email"]?.ToString() ?? "",
                            Telefono = dr["Telefono"]?.ToString() ?? "",
                            FechaNacimiento = dr["FechaNacimiento"] == DBNull.Value ? null : Convert.ToDateTime(dr["FechaNacimiento"]),
                            TerapeutaNombre = dr["TerapeutaNombre"]?.ToString() ?? ""
                        };
                    }
                }
            }

            if (vm != null)
            {
                vm.Citas = ListarCitasPorExpediente(expedienteId);
                vm.Sesiones = ListarSesionesPorExpediente(expedienteId);
                vm.Pagos = ListarPagosPorExpediente(expedienteId);
            }

            return vm;
        }

        public List<CitaExpedienteVm> ListarCitasPorExpediente(int expedienteId)
        {
            var lista = new List<CitaExpedienteVm>();

            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("stGetExpedienteCitas", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ExpedienteId", expedienteId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new CitaExpedienteVm
                        {
                            CitaId = Convert.ToInt32(dr["CitaId"]),
                            FechaHora = Convert.ToDateTime(dr["FechaHora"]),
                            Terapeuta = dr["Terapeuta"]?.ToString() ?? "",
                            Estatus = dr["Estatus"]?.ToString() ?? "",
                            Notas = dr["Notas"]?.ToString()
                        });
                    }
                }
            }

            return lista;
        }
        public List<SesionExpedienteVm> ListarSesionesPorExpediente(int expedienteId)
        {
            var lista = new List<SesionExpedienteVm>();

            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("stGetExpedienteSesiones", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ExpedienteId", expedienteId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new SesionExpedienteVm
                        {
                            SesionId = Convert.ToInt32(dr["SesionId"]),
                            CitaId = Convert.ToInt32(dr["CitaId"]),
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
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

        public List<PagoExpedienteVm> ListarPagosPorExpediente(int expedienteId)
        {
            var lista = new List<PagoExpedienteVm>();

            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("stGetExpedientePagos", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ExpedienteId", expedienteId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new PagoExpedienteVm
                        {
                            PagoSesionId = Convert.ToInt32(dr["PagoSesionId"]),
                            SesionId = Convert.ToInt32(dr["SesionId"]),
                            CitaId = Convert.ToInt32(dr["CitaId"]),
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
                            Monto = Convert.ToDecimal(dr["Monto"]),
                            MetodoPago = dr["MetodoPago"]?.ToString() ?? "",
                            EstatusPago = dr["EstatusPago"]?.ToString() ?? "",
                            Referencia = dr["Referencia"]?.ToString(),
                            Comentarios = dr["Comentarios"]?.ToString(),
                            FechaSesion = Convert.ToDateTime(dr["FechaSesion"])
                        });
                    }
                }
            }

            return lista;
        }

        public ExpedienteEditVm ObtenerActivoPorPaciente(int pacienteId)
        {
            ExpedienteEditVm expediente = null;

            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("stGetExpedienteActivoPorPaciente", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PacienteId", pacienteId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        expediente = new ExpedienteEditVm
                        {
                            ExpedienteId = Convert.ToInt32(dr["ExpedienteId"]),
                            PacienteId = Convert.ToInt32(dr["PacienteId"]),
                            UsuarioId = Convert.ToInt32(dr["UsuarioId"]),
                            FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                            FechaCierre = dr["FechaCierre"] == DBNull.Value
                                ? null
                                : Convert.ToDateTime(dr["FechaCierre"])
                        };
                    }
                }
            }

            return expediente;
        }
    }
}
