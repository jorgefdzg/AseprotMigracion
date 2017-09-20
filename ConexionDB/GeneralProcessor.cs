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
                SqlCommand ordCMD = new SqlCommand("select * from talleres.dbo.Cita", serConn);
                DataTable dt = new DataTable();
                dt.Load(ordCMD.ExecuteReader());
                serConn.Close();
                List<Ordenes> ordenesXCitas = new List<Ordenes>();
                foreach (DataRow dr in dt.Rows)
                {
                    int idCita = int.Parse(dr["idCita"].ToString());
                    Ordenes orden = OrdenesProcessor.CrearOrdenXCita(serConn, idCita);
                    #region  insert de la orden
                    //TODO
                    int idOrden = 0;
                    #endregion
                    #region  insert  de historicos
                    orden.Historicos = OrdenesProcessor.GetHistoricosOrden(serConn, idCita,idOrden);
                    Console.WriteLine(Historico.GuardarHistoricoOrdenes(serConn, orden));
                    #endregion
                    #region insert de la tabla relacion
                    Console.WriteLine(OrdenesProcessor.GuardarRelacionCitaOrdenes(serConn,idCita,orden));
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
    }
}
