using Microsoft.Data.SqlClient;
using SIP.Models;
using System.Data;

namespace SIP.Services
{
    public class CitaService
    {
        private readonly string _connectionString;

        public CitaService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("sipConnect");
        }

        public List<Cita> lstCita()
        {
            List<Cita> _list = new List<Cita>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("stGetCita", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Cita _cita = new Cita();
                                _cita.CitaId = reader["CitaId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CitaId"]);
                                _cita.PacienteId = reader["PacienteId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PacienteId"]);
                                _cita.Paciente = reader["Paciente"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Paciente"]);
                                _cita.Terapeuta = reader["Terapeuta"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Terapeuta"]);
                                _cita.Estatus = reader["Estatus"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Estatus"]);
                                _cita.FechaHora = reader["FechaHora"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["FechaHora"]);
                                _cita.Notas = reader["Notas"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Notas"]);
                                _cita.EstatusCitaId = reader["EstatusCitaId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["EstatusCitaId"]);
                                _list.Add(_cita);
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

        public Cita ObtenerPorId(int citaId)
        {
            Cita _cita = new Cita();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("stGetCita", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@CitaId", citaId);
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                _cita.CitaId = reader["CitaId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["CitaId"]);
                                _cita.PacienteId = reader["PacienteId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PacienteId"]);
                                _cita.Paciente = reader["Paciente"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Paciente"]);
                                _cita.Terapeuta = reader["Terapeuta"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Terapeuta"]);
                                _cita.Estatus = reader["Estatus"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Estatus"]);
                                _cita.FechaHora = reader["FechaHora"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["FechaHora"]);
                                _cita.Notas = reader["Notas"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Notas"]);
                                _cita.EstatusCitaId = reader["EstatusCitaId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["EstatusCitaId"]);
                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return _cita;
        }

        public BasicResponse crearCita(Cita cita)
        {

            BasicResponse _result = new BasicResponse();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("stAddCita", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 30; // puedes ajustar

                command.Parameters.AddWithValue("@PacienteId", cita.PacienteId);
                command.Parameters.AddWithValue("@UsuarioId", cita.UsuarioId);
                command.Parameters.AddWithValue("@ExpedienteId", cita.ExpedienteId);
                command.Parameters.AddWithValue("@EstatusCitaId", cita.EstatusCitaId);
                command.Parameters.AddWithValue("@FechaHora", cita.FechaHora);
                command.Parameters.AddWithValue("@Notas", cita.Notas);

                SqlParameter outReturnCode = new SqlParameter("@ReturnCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(outReturnCode);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    var _returnCode = int.Parse(outReturnCode.Value.ToString());
                    _result.ReturnCode = int.Parse(outReturnCode.Value.ToString());
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

        public BasicResponse editarCita(Cita cita)
        {

            BasicResponse _result = new BasicResponse();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("stUpdCita", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 30; // puedes ajustar

                command.Parameters.AddWithValue("@CitaId", cita.CitaId);
                command.Parameters.AddWithValue("@FechaHora", cita.FechaHora);
                command.Parameters.AddWithValue("@Notas", cita.Notas);

                SqlParameter outReturnCode = new SqlParameter("@ReturnCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(outReturnCode);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    var _returnCode = int.Parse(outReturnCode.Value.ToString());
                    _result.ReturnCode = int.Parse(outReturnCode.Value.ToString());
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

        public BasicResponse estatusCita(int citaId, int estatus)
        {

            BasicResponse _result = new BasicResponse();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("stUpdEstatusCita", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 30; // puedes ajustar

                command.Parameters.AddWithValue("@CitaId", citaId);
                command.Parameters.AddWithValue("@EstatusCitaId", estatus);


                SqlParameter outReturnCode = new SqlParameter("@ReturnCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(outReturnCode);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    var _returnCode = int.Parse(outReturnCode.Value.ToString());
                    _result.ReturnCode = int.Parse(outReturnCode.Value.ToString());
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

        public List<CitaCalendarioVm> ListarCitasCalendario(int? usuarioId)
        {
            var lista = new List<CitaCalendarioVm>();

            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("stGetCitasCalendario", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UsuarioId", (object?)usuarioId ?? DBNull.Value);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new CitaCalendarioVm
                        {
                            CitaId = Convert.ToInt32(dr["CitaId"]),
                            FechaHora = Convert.ToDateTime(dr["FechaHora"]),
                            PacienteNombre = dr["PacienteNombre"]?.ToString() ?? "",
                            TerapeutaNombre = dr["TerapeutaNombre"]?.ToString() ?? "",
                            Estatus = dr["Estatus"]?.ToString() ?? "",
                            Notas = dr["Notas"]?.ToString()
                        });
                    }
                }
            }

            return lista;
        }
    }
}
