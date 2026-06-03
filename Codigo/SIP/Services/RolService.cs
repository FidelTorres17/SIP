using Microsoft.Data.SqlClient;
using SIP.Models;
using System.Data;

namespace SIP.Services
{
    public class RolService
    {
        private readonly string _connectionString;

        public RolService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("sipConnect");
        }

        public List<Rol> lstRol()
        {
            var _list = new List<Rol>();



            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("stGetRol", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var _rol = new Rol
                            {
                                RolId = reader["RolId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["RolId"]),
                                Nombre = reader["Rol"] == DBNull.Value ? string.Empty : Convert.ToString(reader["Rol"])
                            };
                            _list.Add(_rol);
                        }
                    }

                    // Opción "Selecciona una opción"
                    _list.Add(new Rol { RolId = 0, Nombre = "Selecciona una opción" });

                    // Orden si lo necesitas
                    _list = _list.OrderByDescending(o => o.RolId).ToList();
                }
                catch (Exception ex)
                {
                    // Manejo simple de errores (mejor: logger)
                    Console.WriteLine(ex.Message);
                }
            }

            return _list;
        }
    }
}
