using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    public class Partidas
    {
        public decimal idPartida { get; set; }
        public string numeroParte { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }
        public decimal idTaller { get; set; }

        public Partidas(decimal _idPartida, string _numeroParte, string _descripcion, decimal _precio, decimal _idTaller) {
            idPartida = _idPartida;
            numeroParte = _numeroParte;
            descripcion = _descripcion;
            precio = _precio;
            idTaller = _idTaller;
        }
        public static void InsertData(SqlConnection cn, Partidas partida) {
            LogWriter log = new LogWriter();
            try
            {

                string query = "INSERT INTO [dbo].[Partidas]([numeroParte],[descripcion],[precio],[idTaller]) VALUES (@numeroParte,@descripcion,@precio,@idTaller)";
                using (SqlCommand cmd = new SqlCommand(query, cn)) {
                    if (string.IsNullOrEmpty(partida.numeroParte))
                        cmd.Parameters.Add("@numeroParte", SqlDbType.VarChar, 50).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@numeroParte", SqlDbType.VarChar, 50).Value = partida.numeroParte;
                    if (string.IsNullOrEmpty(partida.descripcion))
                        cmd.Parameters.Add("@descripcion", SqlDbType.VarChar, 50).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@descripcion", SqlDbType.VarChar, 50).Value = partida.descripcion;
                    if (string.IsNullOrEmpty(partida.precio.ToString()))
                        cmd.Parameters.Add("@precio", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@precio", SqlDbType.Decimal).Value = partida.precio;
                    if (string.IsNullOrEmpty(partida.idTaller.ToString()))
                        cmd.Parameters.Add("@idTaller", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idTaller", SqlDbType.Decimal).Value = partida.idTaller;
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        log.WriteInLog("Registro de Partidas Insertado con exito numero de parte: "+ partida.numeroParte);
                }
            }
            catch (Exception ex) {
                log.WriteInLog("Error al insertar el registro de Partidas, número de parte: " + partida.numeroParte);
            }
            
            
        }
    }
}
