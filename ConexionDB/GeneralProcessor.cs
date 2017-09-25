﻿using System;
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

                SqlCommand ordCMD = new SqlCommand("select  * from talleres.dbo.Cita where idCita = 89", serConn, trans);

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
                Console.WriteLine("Ocurrio un error : " + aE.Message + "\r\n");
                trans.Rollback();
                serConn.Close();
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
                    #region  insert de la cita
                    //TODO
                    #endregion
                }

                serConn.Close();
            }
            catch (Exception aE)
            {
                Console.WriteLine("Ocurrio un error : " + aE.Message + "\r\n");
            }
        }
        public static void migracion8()
        {
            SqlConnection serConn = new SqlConnection(Constants.ASEPROTDesarrolloStringConn);
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

    }
}
