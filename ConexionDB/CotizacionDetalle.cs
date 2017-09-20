using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    public class CotizacionDetalle
    {
        public static string GuardarCotizacionDetallePorCita(SqlConnection serConn, int aIdCita)
        {
            SqlCommand ordCMD = new SqlCommand(@"select relCot.idCotizacionASE, det.precio as Costo,det.cantidad, pvta.precioCliente as Venta, par.idPartida,
                      case when det.idEstatus = 8 then 1 when det.idEstatus = 25 then 1 when det.idEstatus = 9 then 2 when det.idEstatus = 10 then 4 end as Estatus 
                      from [talleres].[dbo].[CotizacionDetalle] det
                      inner join talleres.dbo.ItemPrecioCliente pVta on pVta.idItemCliente = det.idElemento
                      inner join ASEPROTDesarrollo.dbo.CotizacionTalleresASE relCot on relCot.idCotizacionTalleres = det.idCotizacion
                      inner join talleres.dbo.item itm on itm.idItem = det.idElemento
                      inner join Partidas.dbo.Partida par on par.partida = itm.numeroPartida
                      where det.idCotizacion = " + aIdCita , serConn);
            DataTable dtCotDet = new DataTable();
            dtCotDet.Load(ordCMD.ExecuteReader());
            string retorno = string.Empty;
            if (dtCotDet.Rows.Count > 0)
            {
                ConexionsDBs con = new ConexionsDBs();
                LogWriter log = new LogWriter();
                string query = "insert into CotizacionDetalle(idCotizacion,costo,cantidad,venta,idPartida,idEstatusPartida) values(@idCotizacionASE,@Costo,@cantidad,@precioCliente,@idPartida,@Estatus)";
                using (SqlConnection cn = new SqlConnection(con.ReturnStringConnection((Constants.conexiones)Constants.conexiones.ASEPROTPruebas)))
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    if (string.IsNullOrEmpty(dtCotDet.Rows[0]["idCotizacionASE"].ToString()))
                        cmd.Parameters.Add("@idCotizacionASE", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idCotizacionASE", SqlDbType.Int).Value = dtCotDet.Rows[0]["idCotizacionASE"].ToString();

                    if (string.IsNullOrEmpty(dtCotDet.Rows[0]["Costo"].ToString()))
                        cmd.Parameters.Add("@Costo", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@Costo", SqlDbType.Decimal).Value = dtCotDet.Rows[0]["Costo"].ToString();

                    if (string.IsNullOrEmpty(dtCotDet.Rows[0]["cantidad"].ToString()))
                        cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@cantidad", SqlDbType.Int).Value = dtCotDet.Rows[0]["cantidad"].ToString();

                    if (string.IsNullOrEmpty(dtCotDet.Rows[0]["precioCliente"].ToString()))
                        cmd.Parameters.Add("@precioCliente", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@precioCliente", SqlDbType.Decimal).Value = dtCotDet.Rows[0]["precioCliente"].ToString();

                    if (string.IsNullOrEmpty(dtCotDet.Rows[0]["idPartida"].ToString()))
                        cmd.Parameters.Add("@idPartida", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idPartida", SqlDbType.Int).Value = dtCotDet.Rows[0]["idPartida"].ToString();

                    if (string.IsNullOrEmpty(dtCotDet.Rows[0]["Estatus"].ToString()))
                        cmd.Parameters.Add("@Estatus", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@Estatus", SqlDbType.Int).Value = dtCotDet.Rows[0]["Estatus"].ToString();

                    cn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        SqlCommand cmd2 = new SqlCommand("Select top 1 idCotizacionDetalle from CotizacionDetalle order by idCotizacionDetalle desc", serConn);
                        DataTable dt3 = new DataTable();
                        dt3.Load(cmd2.ExecuteReader());
                        retorno = "Registro de Cotizacion Detalle insertado con exito :" + dt3.Rows[0]["idRelacionCitaOrdenes"].ToString();
                        log.WriteInLog(retorno);
                    }
                    cn.Close();
                }
            }
            return retorno;
        }

        public static string GuardarCotizacionDetalleCompleto(SqlConnection serConn)
        {
            LogWriter log = new LogWriter();
            string retorno = string.Empty;
            SqlCommand cmd = new SqlCommand(@"insert into CotizacionDetalle (idCotizacion,costo,cantidad,venta,idPartida,idEstatusPartida) 
                      select relCot.idCotizacionASE, det.precio as Costo,det.cantidad, pvta.precioCliente as Venta, par.idPartida,
                      case when det.idEstatus = 8 then 1 when det.idEstatus = 25 then 1 when det.idEstatus = 9 then 2 when det.idEstatus = 10 then 4 end as Estatus 
                      from [talleres].[dbo].[CotizacionDetalle] det
                      inner join talleres.dbo.ItemPrecioCliente pVta on pVta.idItemCliente = det.idElemento
                      inner join ASEPROTDesarrollo.dbo.CotizacionTalleresASE relCot on relCot.idCotizacionTalleres = det.idCotizacion
                      inner join talleres.dbo.item itm on itm.idItem = det.idElemento
                      inner join Partidas.dbo.Partida par on par.partida = itm.numeroPartida");
            int res = cmd.ExecuteNonQuery();
            if (res > 0)
                retorno = "Registro de Cotizaciones Detalle insertado con exito : " + res + " registor insertados.";
            log.WriteInLog(retorno);
            return retorno;
        }
    }
}
