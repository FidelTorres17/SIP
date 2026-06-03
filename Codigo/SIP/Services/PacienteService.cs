using Microsoft.Data.SqlClient;
using SIP.Models;
using System.Data;

namespace SIP.Services
{
    public class PacienteService
    {
        private readonly string _connectionString;

        public PacienteService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("sipConnect");
        }

        public List<Paciente> lstPaciente()
        {
            List<Paciente> _list = new List<Paciente>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("stGetPaciente", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Paciente _paciente = new Paciente();
                                _paciente.PacienteId = reader["PacienteId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PacienteId"]);
                                _paciente.Nombre = reader["Nombre"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Nombre"]);
                                _paciente.ApellidoP = reader["ApellidoP"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ApellidoP"]);
                                _paciente.ApellidoM = reader["ApellidoM"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ApellidoM"]);
                                _paciente.FechaNacimiento = reader["FechaNacimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["FechaNacimiento"]);
                                _paciente.Sexo = reader["Sexo"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Sexo"]);
                                _paciente.Telefono = reader["Telefono"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Telefono"]);
                                _paciente.Email = reader["Email"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Email"]);
                                _list.Add(_paciente);
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

        public BasicResponse crearPaciente(Paciente paciente)
        {

            BasicResponse _result = new BasicResponse();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("stAddPaciente", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 30; // puedes ajustar

                command.Parameters.AddWithValue("@Nombre", paciente.Nombre);
                command.Parameters.AddWithValue("@ApellidoP", paciente.ApellidoP);
                command.Parameters.AddWithValue("@ApellidoM", paciente.ApellidoM);
                command.Parameters.AddWithValue("@FechaNacimiento", paciente.FechaNacimiento);
                command.Parameters.AddWithValue("@Sexo", paciente.Sexo);
                command.Parameters.AddWithValue("@Email", paciente.Email);
                command.Parameters.AddWithValue("@Telefono", paciente.Telefono);
                
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

        public BasicResponse editarPaciente(Paciente paciente)
        {

            BasicResponse _result = new BasicResponse();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("stUpdPaciente", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 30; // puedes ajustar

                command.Parameters.AddWithValue("@PacienteId", paciente.PacienteId);
                command.Parameters.AddWithValue("@Nombre", paciente.Nombre);
                command.Parameters.AddWithValue("@ApellidoP", paciente.ApellidoP);
                command.Parameters.AddWithValue("@ApellidoM", paciente.ApellidoM);
                command.Parameters.AddWithValue("@FechaNacimiento", paciente.FechaNacimiento);
                command.Parameters.AddWithValue("@Sexo", paciente.Sexo);
                command.Parameters.AddWithValue("@Email", paciente.Email);
                command.Parameters.AddWithValue("@Telefono", paciente.Telefono);

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

        public BasicResponse eliminarPaciente(int UsuarioId)
        {

            BasicResponse _result = new BasicResponse();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("stUpdUsario", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 30; // puedes ajustar

                command.Parameters.AddWithValue("@UsuarioId", UsuarioId);


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
    }
}
