using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    public class GeneralProcessor
    {
        public static void MigracionGeneral()
        {
            SqlConnection serConn = new SqlConnection(Constants.ASEPROTDesarrolloStringConn);
            serConn.Open();
            SqlTransaction trans = serConn.BeginTransaction("General");
            try
            {
                SqlCommand ordCMD = new SqlCommand("select top 1000 * from talleres.dbo.Cita where idCita not in (13718,13719,21821)", serConn,trans);
                DataTable dt = new DataTable();
                dt.Load(ordCMD.ExecuteReader());
                //serConn.Close();
                List<Ordenes> ordenesXCitas = new List<Ordenes>();
                int contador = dt.Rows.Count;    
                foreach (DataRow dr in dt.Rows)
                {
                    int idCita = int.Parse(dr["idCita"].ToString());
                    Ordenes orden = OrdenesProcessor.CrearOrdenXCita(serConn, idCita,trans);
                    #region  insert de la orden                    
                    int idOrden = Ordenes.InsertData(serConn,orden,trans);
                    #endregion
                    #region  insert  de historicos
                    orden.Historicos = OrdenesProcessor.GetHistoricosOrden(serConn, idCita,idOrden,orden.idEstatusOrden,trans);
                    Console.WriteLine(HistoricoOrdenes.GuardarHistoricoOrdenes(serConn, orden,trans));
                    #endregion
                    #region insert de la tabla relacion
                    Console.WriteLine(OrdenesProcessor.GuardarRelacionCitaOrdenes(serConn,idCita, idOrden,trans));
                    #endregion
                    #region guardar cotizacion detalle por cita
                    //Console.WriteLine(CotizacionDetalle.GuardarCotizacionDetallePorCita(serConn, idCita));
                    #endregion
                    ordenesXCitas.Add(orden);
                    contador--;
                    Console.WriteLine("Ciclos pendientes : " + contador + "\r\n");
                }
                //Console.WriteLine(CotizacionDetalle.GuardarCotizacionDetalleCompleto(serConn));
                trans.Commit();
                serConn.Close();
            }
            catch (Exception aE)
            {
                Console.WriteLine("Ocurrio un error : " + aE.Message + "\r\n");
                trans.Rollback();
                serConn.Close();
            }
        }
    }
}
