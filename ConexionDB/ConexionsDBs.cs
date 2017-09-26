using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ConexionDB
{
    public class ConexionsDBs
    {
        //Para despues
        //public static SqlCommand PimpCommand(Constants.conexiones conexion) {
        //    SqlConnection serConn;
        //    if (conexion==Constants.conexiones.ASEPROTDesarrollo)
        //        serConn = new SqlConnection(Constants.ASEPROTDesarrolloStringConn);
        //    if (conexion == Constants.conexiones.ASEPROT)
        //        serConn = new SqlConnection(Constants.ASEPROTStringConn);
        //    if (conexion == Constants.conexiones.ASEPROTPruebas)
        //        serConn = new SqlConnection(Constants.ASEPROTPruebasStringConn);
        //    if (conexion == Constants.conexiones.Partidas)
        //        serConn = new SqlConnection(Constants.PartidasStringConn);
        //    if (conexion == Constants.conexiones.Talleres)
        //        serConn = new SqlConnection(Constants.TalleresStringConn);

        //    return SqlCommand ordCMD = serConn.CreateCommand();

        //}
        public string ReturnStringConnection(Constants.conexiones conn)
        {

            switch (conn)
            {
                case Constants.conexiones.ASEPROTDesarrollo:
                    return Constants.ASEPROTDesarrolloStringConn;
                    
                case Constants.conexiones.ASEPROT:
                    return Constants.ASEPROTStringConn;
                   
                case Constants.conexiones.ASEPROTPruebas:
                    return Constants.ASEPROTPruebasStringConn;
                   
                case Constants.conexiones.Partidas:
                    return Constants.PartidasStringConn;
                   
                case Constants.conexiones.Talleres:
                    return Constants.TalleresStringConn;
                   
                default:
                    return "";
                    
            }
        }
    }
}
