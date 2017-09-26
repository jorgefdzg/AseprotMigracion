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
            SqlConnection serConn = new SqlConnection(Constants.ASEPROTFastStringConn);
            serConn.Open();
            SqlTransaction trans = serConn.BeginTransaction("General");
            try
            {

                SqlCommand ordCMD = new SqlCommand("select top 5000 * from talleres.dbo.Cita order by idCita desc", serConn, trans);

                DataTable dt = new DataTable();
                dt.Load(ordCMD.ExecuteReader());
                //serConn.Close();
                List<Ordenes> ordenesXCitas = new List<Ordenes>();
                int contador = dt.Rows.Count;
                foreach (DataRow dr in dt.Rows)
                {
                    int idCita = int.Parse(dr["idCita"].ToString());
                    Ordenes orden = OrdenesProcessor.CrearOrdenXCita(serConn, idCita, trans);
                    #region  insert de la orden                    
                    int idOrden = Ordenes.InsertData(serConn, orden, trans);
                    #endregion
                    #region  insert  de historicos

                    orden.Historicos = OrdenesProcessor.GetHistoricosOrden(serConn, idCita, idOrden, orden.idEstatusOrden, trans);
                    Console.WriteLine(HistoricoOrdenes.GuardarHistoricoOrdenes(serConn, orden, trans));
                    #endregion
                    #region insert de la tabla relacion
                    Console.WriteLine(OrdenesProcessor.GuardarRelacionCitaOrdenes(serConn, idCita, idOrden, trans));
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
                LogWriter log = new LogWriter();
                Console.WriteLine("Ocurrio un error : " + aE.Message + "\r\n");
                log.WriteInLog("Ocurrio un error : " + aE.Message + "\r\n");
                trans.Rollback();
                serConn.Close();
            }
        }


        public static void MigracionCotizacion()
        {
            try
            {
                SqlConnection serConn = new SqlConnection(Constants.ASEPROTFastStringConn);

                serConn.Open();
                SqlCommand cotCMD = new SqlCommand("select * from talleres.dbo.CotizacionMaestro where idTrabajo in (select idTrabajoTalleres from ASEPROT..RelacionCitaOrdenes)", serConn);
                DataTable dt = new DataTable();
                dt.Load(cotCMD.ExecuteReader());
                
                List<Cotizaciones> cotizacionX = new List<Cotizaciones>();

                foreach (DataRow dr in dt.Rows)
                {
                    int idCotizacion = int.Parse(dr["idCotizacion"].ToString()); 
                    Cotizaciones cotizacion = CotizacionesProcessor.GetCotizacion(serConn, idCotizacion);
                    decimal newIdCotizacion = 0;
                    if (cotizacion.idOrden == 817)
                    {
                        newIdCotizacion = 0;
                    }
                    
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
                LogWriter log = new LogWriter();
                Console.WriteLine("Ocurrio un error : " + aE.Message + "\r\n");
                log.WriteInLog("Ocurrio la Excepcion: " + aE.Message);

            }
        }
        public static void migracion8()
        {
            try
            {
                SqlConnection serConn = new SqlConnection(Constants.ASEPROTFastStringConn);
                SqlTransaction transaction = null;
                Presupuesto p = new Presupuesto();
                List<Presupuesto> presupuesto = p.listarPresupuesto(serConn);
                foreach (Presupuesto presu in presupuesto)
                    p.InsertarPresupuesto(presu, serConn, transaction);

                PresupuestoOrden presupuestoOrden = new PresupuestoOrden();
                List<PresupuestoOrden> listPresupuestoOrden = presupuestoOrden.listarPresupuestoOrden(serConn);
                foreach (PresupuestoOrden presupuestoOrdenI in listPresupuestoOrden)
                    presupuestoOrden.InsertarPresupuesto(presupuestoOrdenI, serConn, transaction);

                TraspasoPresupuesto traspasoPresupuesto = new TraspasoPresupuesto();
                List<TraspasoPresupuesto> traspasoList = traspasoPresupuesto.listarTraspasoPresupuesto(serConn);
                foreach (TraspasoPresupuesto traspaso in traspasoList)
                    traspasoPresupuesto.InsertarTraspaso(traspaso, serConn, transaction);
            }
            catch (Exception aE)
            {
                LogWriter log = new LogWriter();
                Console.WriteLine("Ocurrio un error : " + aE.Message + "\r\n");
                log.WriteInLog("Ocurrio un error : " + aE.Message + "\r\n");
            }

        }

    }
}
