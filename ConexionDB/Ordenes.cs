using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    public class Ordenes
    {
        public decimal idOrden { get; set; }
        public DateTime fechaCreacionOden { get; set; }
        public DateTime fechaCita { get; set; }
        public DateTime? fechaInicioTrabajo { get; set; }
        public string numeroOrden { get; set; }
        public int consecutivoOrden { get; set; }
        public string comentarioOrden { get; set; }
        public bool requiereGrua { get; set; }
        public int idCatalogoEstadoUnidad { get; set; }
        public decimal idZona { get; set; }
        public decimal idUnidad { get; set; }
        public int idContratoOperacion { get; set; }
        public decimal idUsuario { get; set; }
        public int idCatalogoTipoOrdenServicio { get; set; }
        public int idTipoOrden { get; set; }
        public int idEstatusOrden { get; set; }
        public decimal idCentroTrabajo { get; set; }
        public decimal idTaller { get; set; }
        public decimal idGarantia { get; set; }
        public string motivoGarantia { get; set; }
        public List<HistoricoOrdenes> Historicos { get; set; }
 



        public static int InsertData(SqlConnection cn, Ordenes orden) {
            LogWriter log = new LogWriter();
            try
            {
                int rowsAffected = 0;
                int retorno = 0;
                string query = "Insert into ordenes([fechaCreacionOden],[fechaCita],[fechaInicioTrabajo],[numeroOrden],[consecutivoOrden],[comentarioOrden],[requiereGrua],[idCatalogoEstadoUnidad],[idZona],[idUnidad],[idContratoOperacion],[idUsuario],[idCatalogoTipoOrdenServicio],[idTipoOrden],[idEstatusOrden],[idCentroTrabajo],[idTaller],[idGarantia],[motivoGarantia]) Values (@fechaCreacionOden, @fechaCita, @fechaInicioTrabajo, @numeroOrden, @consecutivoOrden, @comentarioOrden, @requiereGrua, @idCatalogoEstadoUnidad, @idZona, @idUnidad, @idContratoOperacion, @idUsuario, @idCatalogoTipoOrdenServicio, @idTipoOrden, @idEstatusOrden, @idCentroTrabajo, @idTaller, @idGarantia, @motivoGarantia)";
                ConexionsDBs con = new ConexionsDBs();
                //SqlConnection cn = new SqlConnection(con.ReturnStringConnection((Constants.conexiones)Constants.conexiones.ASEPROTDesarrollo));
                //using (cn)
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    if (string.IsNullOrEmpty(orden.fechaCreacionOden.ToString()))
                        cmd.Parameters.Add("@fechaCreacionOden", SqlDbType.DateTime).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@fechaCreacionOden", SqlDbType.DateTime).Value = orden.fechaCreacionOden;
                    if (string.IsNullOrEmpty(orden.fechaCita.ToString()))
                        cmd.Parameters.Add("@fechaCita", SqlDbType.DateTime).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@fechaCita", SqlDbType.DateTime).Value = orden.fechaCita;
                    if (string.IsNullOrEmpty(orden.fechaInicioTrabajo.ToString()))
                        cmd.Parameters.Add("@fechaInicioTrabajo", SqlDbType.DateTime).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@fechaInicioTrabajo", SqlDbType.DateTime).Value = orden.fechaInicioTrabajo;
                    if (string.IsNullOrEmpty(orden.numeroOrden))
                        cmd.Parameters.Add("@numeroOrden", SqlDbType.VarChar, 50).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@numeroOrden", SqlDbType.VarChar, 50).Value = orden.numeroOrden;
                    if (string.IsNullOrEmpty(orden.consecutivoOrden.ToString()))
                        cmd.Parameters.Add("@consecutivoOrden", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@consecutivoOrden", SqlDbType.Int).Value = orden.consecutivoOrden;
                    if (string.IsNullOrEmpty(orden.comentarioOrden))
                        cmd.Parameters.Add("@comentarioOrden", SqlDbType.VarChar, 200).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@comentarioOrden", SqlDbType.VarChar, 200).Value = orden.comentarioOrden;
                    if (string.IsNullOrEmpty(orden.requiereGrua.ToString()))
                        cmd.Parameters.Add("@requiereGrua", SqlDbType.Bit).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@requiereGrua", SqlDbType.Bit).Value = orden.requiereGrua;

                    if (string.IsNullOrEmpty(orden.idCatalogoEstadoUnidad.ToString()))
                        cmd.Parameters.Add("@idCatalogoEstadoUnidad", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idCatalogoEstadoUnidad", SqlDbType.Int).Value = orden.idCatalogoEstadoUnidad;

                    if (string.IsNullOrEmpty(orden.idZona.ToString()))
                        cmd.Parameters.Add("@idZona", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idZona", SqlDbType.Decimal).Value = orden.idZona;

                    if (string.IsNullOrEmpty(orden.idUnidad.ToString()))
                        cmd.Parameters.Add("@idUnidad", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idUnidad", SqlDbType.Decimal).Value = orden.idUnidad;

                    if (string.IsNullOrEmpty(orden.idContratoOperacion.ToString()))
                        cmd.Parameters.Add("@idContratoOperacion", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idContratoOperacion", SqlDbType.Int).Value = orden.idContratoOperacion;

                    if (string.IsNullOrEmpty(orden.idUsuario.ToString()))
                        cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = orden.idUsuario;

                    if (string.IsNullOrEmpty(orden.idCatalogoTipoOrdenServicio.ToString()))
                        cmd.Parameters.Add("@idCatalogoTipoOrdenServicio", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idCatalogoTipoOrdenServicio", SqlDbType.Int).Value = orden.idCatalogoTipoOrdenServicio;

                    if (string.IsNullOrEmpty(orden.idTipoOrden.ToString()))
                        cmd.Parameters.Add("@idTipoOrden", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idTipoOrden", SqlDbType.Int).Value = orden.idTipoOrden;

                    if (string.IsNullOrEmpty(orden.idEstatusOrden.ToString()))
                        cmd.Parameters.Add("@idEstatusOrden", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idEstatusOrden", SqlDbType.Int).Value = orden.idEstatusOrden;

                    if (string.IsNullOrEmpty(orden.idCentroTrabajo.ToString()))
                        cmd.Parameters.Add("@idCentroTrabajo", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idCentroTrabajo", SqlDbType.Decimal).Value = orden.idCentroTrabajo;

                    if (string.IsNullOrEmpty(orden.idTaller.ToString()))
                        cmd.Parameters.Add("@idTaller", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idTaller", SqlDbType.Decimal).Value = orden.idTaller;

                    if (string.IsNullOrEmpty(orden.idGarantia.ToString()))
                        cmd.Parameters.Add("@idGarantia", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idGarantia", SqlDbType.Decimal).Value = orden.idGarantia;

                    if (string.IsNullOrEmpty(orden.motivoGarantia))
                        cmd.Parameters.Add("@motivoGarantia", SqlDbType.VarChar, 100).Value = "";
                    else
                        cmd.Parameters.Add("@motivoGarantia", SqlDbType.VarChar, 100).Value = orden.motivoGarantia;

                    cn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                    
                    cn.Close();
                    if (rowsAffected > 0)
                        log.WriteInLog("Registro de Orden insertado con exito " + orden.numeroOrden);
                }
                if (rowsAffected > 0)
                {
                    cn.Open();
                    SqlCommand cmd2 = new SqlCommand("select top 1 idOrden from Ordenes order by idOrden desc", cn);
                    DataTable dt = new DataTable();
                    dt.Load(cmd2.ExecuteReader());
                    if (dt.Rows.Count > 0)
                        retorno = int.Parse(dt.Rows[0]["idOrden"].ToString());
                }
                
                cn.Close();
                return retorno;

            }
            catch (Exception ex) {                
                log.WriteInLog("Error al insertar la orden: " + orden.numeroOrden + " Excepción:" + ex.Message);
                return 0;
            }
        }


        //public static bool InsertInTable(Ordenes orden) {
                
        //}
    }
}
