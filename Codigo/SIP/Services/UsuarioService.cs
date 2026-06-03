using Microsoft.Data.SqlClient;
using SIP.Models;
using System.Data;

namespace SIP.Services
{
    public class UsuarioService
    {
        private readonly string _connectionString;

        public UsuarioService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("sipConnect");
        }

        public List<Usuario> lstUsuario()
        {
            List<Usuario> _list = new List<Usuario>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("stGetUsuario", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Usuario _usuario = new Usuario();
                                _usuario.UsuarioId = reader["UsuarioId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["UsuarioId"]);
                                _usuario.Nombre = reader["Nombre"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Nombre"]);
                                _usuario.ApellidoP = reader["ApellidoP"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ApellidoP"]);
                                _usuario.ApellidoM = reader["ApellidoM"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ApellidoM"]);
                                _usuario.RolId = reader["RolId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["RolId"]);
                                _usuario.Rol = reader["Rol"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Rol"]);
                                _usuario.Email = reader["Email"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Email"]);
                                _usuario.Telefono = reader["Telefono"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Telefono"]);
                                _usuario.UserName = reader["UserName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["UserName"]);
                                _usuario.Password = reader["Password"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Password"]);
                                _list.Add(_usuario);
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

        public List<Usuario> ListarTerapeutas()
        {
            List<Usuario> _list = new List<Usuario>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("stGetTerapeuta", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Usuario _usuario = new Usuario();
                                _usuario.UsuarioId = reader["UsuarioId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["UsuarioId"]);
                                _usuario.Nombre = reader["Nombre"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Nombre"]);
                                _usuario.ApellidoP = reader["ApellidoP"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ApellidoP"]);
                                _usuario.ApellidoM = reader["ApellidoM"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ApellidoM"]);
                                _usuario.RolId = reader["RolId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["RolId"]);
                                _usuario.Rol = reader["Rol"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Rol"]);
                                _usuario.Email = reader["Email"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Email"]);
                                _usuario.Telefono = reader["Telefono"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Telefono"]);
                                _list.Add(_usuario);
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

        public Usuario getLogin(string _username, string _pass)
        {
            Usuario _usuario = new Usuario();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("stGetLogin", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserName", _username);
                        command.Parameters.AddWithValue("@Password", _pass);

                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                _usuario.UsuarioId = reader["UsuarioId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["UsuarioId"]);
                                _usuario.Nombre = reader["Nombre"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Nombre"]);
                                _usuario.ApellidoP = reader["ApellidoP"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ApellidoP"]);
                                _usuario.ApellidoM = reader["ApellidoM"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ApellidoM"]);
                                _usuario.RolId = reader["RolId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["RolId"]);
                                _usuario.Rol = reader["Rol"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Rol"]);
                                _usuario.Email = reader["Email"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Email"]);
                                _usuario.Telefono = reader["Telefono"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Telefono"]);
                                _usuario.UserName = reader["UserName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["UserName"]);
                                _usuario.Password = reader["Password"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Password"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return _usuario;
        }


        public Usuario ObtenerPorEmail(string email)
        {
            Usuario _usuario = new Usuario();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand("stGetUsuarioEmail", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Email", email);
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                
                                _usuario.UsuarioId = reader["UsuarioId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["UsuarioId"]);
                                _usuario.Nombre = reader["Nombre"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Nombre"]);
                                _usuario.ApellidoP = reader["ApellidoP"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ApellidoP"]);
                                _usuario.ApellidoM = reader["ApellidoM"] == DBNull.Value ? string.Empty : Convert.ToString(reader["ApellidoM"]);
                                _usuario.RolId = reader["RolId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["RolId"]);
                                _usuario.Rol = reader["Rol"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Rol"]);
                                _usuario.Email = reader["Email"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Email"]);
                                _usuario.Telefono = reader["Telefono"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Telefono"]);
                                _usuario.UserName = reader["UserName"] == DBNull.Value ? string.Empty : Convert.ToString(reader["UserName"]);
                                _usuario.Password = reader["Password"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Password"]);
                               
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return _usuario;
        }

        public BasicResponse crearUsuario(Usuario user)
        {

            BasicResponse _result = new BasicResponse();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("stAddUsario", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 30; // puedes ajustar

                command.Parameters.AddWithValue("@Nombre", user.Nombre);
                command.Parameters.AddWithValue("@ApellidoP", user.ApellidoP);
                command.Parameters.AddWithValue("@ApellidoM", user.ApellidoM);
                command.Parameters.AddWithValue("@RolId", user.RolId);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Telefono", user.Telefono);
                command.Parameters.AddWithValue("@UserName", user.Email);
                command.Parameters.AddWithValue("@Password", "SIP12345");

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

        public BasicResponse editarUsuario(Usuario user)
        {

            BasicResponse _result = new BasicResponse();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("stUpdUsario", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 30; // puedes ajustar

                command.Parameters.AddWithValue("@UsuarioId", user.UsuarioId);
                command.Parameters.AddWithValue("@Nombre", user.Nombre);
                command.Parameters.AddWithValue("@ApellidoP", user.ApellidoP);
                command.Parameters.AddWithValue("@ApellidoM", user.ApellidoM);
                command.Parameters.AddWithValue("@RolId", user.RolId);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Telefono", user.Telefono);
                command.Parameters.AddWithValue("@UserName", user.Email);
                command.Parameters.AddWithValue("@Password", "");

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

        public BasicResponse eliminarUsuario(int UsuarioId)
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

        public BasicResponse ActualizarPassword(int? usuarioId, string pass)
        {

            BasicResponse _result = new BasicResponse();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("stUpdUsarioPass", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 30; // puedes ajustar

                command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                command.Parameters.AddWithValue("@Password", pass);

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

        public bool CambiarPassword(int usuarioId, string passwordActual, string passwordNueva)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("stUpdCambiarPassword", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
                cmd.Parameters.AddWithValue("@PasswordActual", passwordActual);
                cmd.Parameters.AddWithValue("@PasswordNueva", passwordNueva);

                cn.Open();

                var result = Convert.ToInt32(cmd.ExecuteScalar());
                return result == 1;
            }
        }
    }
}
