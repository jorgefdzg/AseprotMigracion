using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;


namespace ConexionDB
{
    public class HistoricoOrdenes
    {
        private int? _idHistorialEstatusOrden = null;
        private int? _idOrden = null;
        private int _idEstatusOrden = 0;
        private DateTime _fechaInicial = DateTime.MinValue;
        private DateTime? _fechaFinal = null;
        private int _idUsuario = 0;

        public int?      idHistorialEstatusOrden { get { return _idHistorialEstatusOrden; } set { _idHistorialEstatusOrden = value; } }
        public int?      idOrden                 { get { return _idOrden; } set { _idOrden = value; } }
        public int      idEstatusOrden          { get { return _idEstatusOrden; } set { _idEstatusOrden = value; } }
        public DateTime fechaInicial            { get { return _fechaInicial; } set { _fechaInicial = value; } }
        public DateTime? fechaFinal              { get { return _fechaFinal; } set { _fechaFinal = value; } }
        public int      idUsuario               { get { return _idUsuario; } set { _idUsuario = value; } }


        public static string GuardarHistoricoOrdenes(SqlConnection serConn ,Ordenes orden,SqlTransaction aTrans)
        {
            string retorno = string.Empty;
            LogWriter log = new LogWriter();
            //////serConn.Open();

            //orden.Historicos = orden.Historicos.OrderByDescending(o => o.idEstatusOrden).ToList();
            
            int h = orden.Historicos.Count - 1;
            while (h >= 0)            
            {
                HistoricoOrdenes item = orden.Historicos[h];
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                DateTime fechaFinal = h == orden.Historicos.Count - 1 ? DateTime.MinValue : orden.Historicos[h + 1].fechaInicial;
                if (fechaFinal != DateTime.MinValue)
                    orden.Historicos[h].fechaFinal = fechaFinal;
                h--;
            }
            //orden.Historicos = orden.Historicos.OrderBy(o => o.idEstatusOrden).ToList();
            for (int i = 0; i < orden.Historicos.Count; i++)
            {
                HistoricoOrdenes item = orden.Historicos[i];
                //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                //DateTime fechaFinal = i == orden.Historicos.Count - 1 ? item.fechaInicial : (i == 0 ? DateTime.MinValue : orden.Historicos[i - 1].fechaInicial) ;
                //if (fechaFinal != DateTime.MinValue)
                //    orden.Historicos[i].fechaFinal = fechaFinal;
                DateTime fechaFinal = item.fechaFinal ?? DateTime.MinValue;
                SqlCommand cmdH = new SqlCommand();
                if (fechaFinal == DateTime.MinValue) 
                    cmdH = new SqlCommand("insert into HistorialEstatusOrden (idOrden,idEstatusOrden,fechaInicial,idUsuario) values(" + item.idOrden + "," + item.idEstatusOrden + ",CAST('" + item.fechaInicial.ToString("yyyy-MM-ddThh:mm:ss") + "' AS DATETIME)," + item.idUsuario + ")",serConn, aTrans);
                else
                    cmdH = new SqlCommand("insert into HistorialEstatusOrden (idOrden,idEstatusOrden,fechaInicial,fechaFinal,idUsuario) values(" + item.idOrden + "," + item.idEstatusOrden + ",CAST('" + item.fechaInicial.ToString("yyyy-MM-ddThh:mm:ss") + "' AS DATETIME),CAST('" + fechaFinal.ToString("yyyy-MM-ddThh:mm:ss") + "'  AS DATETIME)," + item.idUsuario + ")",serConn, aTrans);
                    

                int res = cmdH.ExecuteNonQuery();
                if (res > 0)
                {
                    retorno += "Historico de estatus '" + item.idEstatusOrden + "' generado.\r\n";                    
                }
                else
                    throw new Exception( "Ocurrio un error al generar el historio de estatus " + item.idEstatusOrden );
            }
            //////serConn.Close();
            log.WriteInLog(retorno);
            return retorno;
        }

    }


}