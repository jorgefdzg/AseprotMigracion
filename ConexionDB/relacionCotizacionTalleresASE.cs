using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ConexionDB
{
    public class RelacionCotizacionTalleresASE
    {
        public int idCotizacionTalleresASE { get; set; }
        public int idCotizacionTalleres { get; set; }
        public decimal idCotizacionASE { get; set; }

        public static void InsertData(SqlConnection cn, RelacionCotizacionTalleresASE IdsCotizacion)
        {
            LogWriter log = new LogWriter();
            try
            {
                string query = @"INSERT INTO [dbo].[CotizacionTalleresASE] ([idCotizacionTalleres],[idCotizacionASE])
                                        VALUES (@idCotizacionTalleres,@idCotizacionASE)";
                ConexionsDBs con = new ConexionsDBs();
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    if (string.IsNullOrEmpty(IdsCotizacion.idCotizacionTalleres.ToString()))
                        cmd.Parameters.Add("@idCotizacionTalleres", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idCotizacionTalleres", SqlDbType.Int).Value = IdsCotizacion.idCotizacionTalleres;

                    if (string.IsNullOrEmpty(IdsCotizacion.idCotizacionASE.ToString()))
                        cmd.Parameters.Add("@idCotizacionASE", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idCotizacionASE", SqlDbType.Decimal).Value = IdsCotizacion.idCotizacionASE;

                    cn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    cn.Close();

                    if (rowsAffected > 0)                    
                        log.WriteInLog("Registro de relación cotización insertado con exito " + IdsCotizacion.idCotizacionTalleres);

                  }
            }
            catch (Exception ex)
            {
                log.WriteInLog("Error al insertar la relación de cotización " + IdsCotizacion.idCotizacionTalleres + " Excepcion: " + ex.Message);
            }
        }
    }
}
