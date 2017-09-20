using System;
using System.Data.SqlClient;

namespace ConexionDB
{
    public class Historico
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

        public static string GuardarHistoricoOrdenes(SqlConnection serConn ,Ordenes orden)
        {
            string retorno = string.Empty;
            serConn.Open();
            for (int i = 0; i < orden.Historicos.Count; i++)
            {
                Historico item = orden.Historicos[i];
                DateTime fechaFinal = (i == 0 || i == orden.Historicos.Count - 1) ? item.fechaInicial : orden.Historicos[i - 1].fechaFinal ?? DateTime.MinValue;
                SqlCommand cmdH = new SqlCommand("insert into HistorialEstatusOrden (idOrden,idEstatusOrden,fechaInicial,fechaFinal,idUsuario) values(" + item.idOrden + "," + item.idEstatusOrden + "," + item.fechaInicial + "," + fechaFinal.ToString("yyyy-MM-dd") + "," + item.idUsuario);

                int res = cmdH.ExecuteNonQuery();
                if (res > 0)
                    Console.WriteLine("Historico de estatus '" + item.idEstatusOrden + "' generado.");
                else
                    Console.WriteLine("Ocurrio un error al generar el historio de estatus " + item.idEstatusOrden);
            }
            serConn.Close();
            return retorno;
        }
    }
}