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
            try
            {
                SqlConnection serConn = new SqlConnection(Constants.ASEPROTDesarrolloStringConn);
                

                serConn.Open();
                SqlCommand ordCMD = new SqlCommand("select  * from talleres.dbo.Cita where idCita = 89", serConn);
                DataTable dt = new DataTable();
                dt.Load(ordCMD.ExecuteReader());
                serConn.Close();
                List<Ordenes> ordenesXCitas = new List<Ordenes>();
                    
                foreach (DataRow dr in dt.Rows)
                {
                    int idCita = int.Parse(dr["idCita"].ToString());
                    Ordenes orden = OrdenesProcessor.CrearOrdenXCita(serConn, idCita);
                    #region  insert de la orden                    
                    int idOrden = Ordenes.InsertData(serConn,orden);
                    #endregion
                    #region  insert  de historicos
                    orden.Historicos = OrdenesProcessor.GetHistoricosOrden(serConn, idCita,idOrden);
                    Console.WriteLine(HistoricoOrdenes.GuardarHistoricoOrdenes(serConn, orden));
                    #endregion
                    #region insert de la tabla relacion
                    Console.WriteLine(OrdenesProcessor.GuardarRelacionCitaOrdenes(serConn,idCita, idOrden));
                    #endregion
                    #region guardar cotizacion detalle por cita
                    Console.WriteLine(CotizacionDetalle.GuardarCotizacionDetallePorCita(serConn, idCita));
                    #endregion
                    ordenesXCitas.Add(orden);
                }
                Console.WriteLine(CotizacionDetalle.GuardarCotizacionDetalleCompleto(serConn));
                serConn.Close();
            }
            catch (Exception aE)
            {
                Console.WriteLine("Ocurrio un error : " + aE.Message + "\r\n");
            }
        }

        public static void MigracionCotizacion()
        {
            try
            {
                SqlConnection serConn = new SqlConnection(Constants.ASEPROTDesarrolloStringConn);

                serConn.Open();
                SqlCommand cotCMD = new SqlCommand("select * from talleres.dbo.CotizacionMaestro", serConn);
                DataTable dt = new DataTable();
                dt.Load(cotCMD.ExecuteReader());
                serConn.Close();
                List<Cotizaciones> cotizacionX = new List<Cotizaciones>();

                foreach (DataRow dr in dt.Rows)
                {
                    int idCotizacion = int.Parse(dr["idCotizacion"].ToString()); 
                    Cotizaciones cotizacion = CotizacionesProcessor.GetCotizacion(serConn, idCotizacion);
                    decimal newIdCotizacion = 0;
                    #region  insert de la cotización
                    Cotizaciones.InsertData(serConn, cotizacion, ref newIdCotizacion);
                    #endregion
                    #region insert relación ids nuevos y viejos
                    RelacionCotizacionTalleresASE newRelacionCotizaciones = new RelacionCotizacionTalleresASE();
                    newRelacionCotizaciones.idCotizacionTalleres = idCotizacion;
                    newRelacionCotizaciones.idCotizacionASE = newIdCotizacion;

                    RelacionCotizacionTalleresASE.InsertData(serConn,newRelacionCotizaciones);
                    #endregion

                }

                serConn.Close();
            }
            catch (Exception aE)
            {
                Console.WriteLine("Ocurrio un error : " + aE.Message + "\r\n");
            }

        }
    }
}
