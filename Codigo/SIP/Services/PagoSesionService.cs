using Microsoft.Data.SqlClient;
using SIP.Models;
using System.Data;

namespace SIP.Services
{
    public class PagoSesionService
    {
        private readonly string _connectionString;

        public PagoSesionService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("sipConnect");
        }

        public int crear(PagoSesion pago)
        {
            int id = 0;

            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("stAddPagoSesion", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SesionId", pago.SesionId);
                cmd.Parameters.AddWithValue("@Monto", pago.Monto);
                cmd.Parameters.AddWithValue("@MetodoPagoId", pago.MetodoPagoId);
                cmd.Parameters.AddWithValue("@EstatusPagoId", pago.EstatusPagoId);
                cmd.Parameters.AddWithValue("@Referencia", (object?)pago.Referencia ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Comentarios", (object?)pago.Comentarios ?? DBNull.Value);

                cn.Open();

                var result = cmd.ExecuteScalar();
                if (result != null)
                    id = Convert.ToInt32(result);
            }

            return id;
        }

        public List<PagoSesion> ListarPorSesion(int sesionId)
        {
            var lista = new List<PagoSesion>();

            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("stGetPagoSesionListarPorSesion", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SesionId", sesionId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new PagoSesion
                        {
                            PagoSesionId = Convert.ToInt32(dr["PagoSesionId"]),
                            SesionId = Convert.ToInt32(dr["SesionId"]),
                            FechaPago = Convert.ToDateTime(dr["Fecha"]),
                            Monto = Convert.ToDecimal(dr["Monto"]),
                            MetodoPagoId = Convert.ToInt32(dr["MetodoPagoId"]),
                            MetodoPago = dr["MetodoPago"]?.ToString() ?? "",
                            EstatusPagoId = Convert.ToInt32(dr["EstatusPagoId"]),
                            EstatusPago = dr["EstatusPago"]?.ToString() ?? "",
                            Referencia = dr["Referencia"]?.ToString(),
                            Comentarios = dr["Comentarios"]?.ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public void Eliminar(int pagoSesionId)
        {
            using (SqlConnection cn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("stDelPagoSesion", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PagoSesionId", pagoSesionId);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
