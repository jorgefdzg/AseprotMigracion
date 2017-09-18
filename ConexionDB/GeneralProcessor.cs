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
                    #endregion
                    orden.Historicos = OrdenesProcessor.GetHistoricosOrden(serConn, idCita);
                    #region  insert  de historicos
                    //TODO
                    #endregion
                    ordenesXCitas.Add(orden);
                    #region insert de la tabla relacion
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
    }
}
