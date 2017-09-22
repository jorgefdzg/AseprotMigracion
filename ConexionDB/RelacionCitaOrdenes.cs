using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    public class RelacionCitaOrdenes
    {
        public int idRelacionCitaOrdenes { get; set; }
        public int idCitaTalleres { get; set; }
        public int idTrabajoTalleres { get; set; }
        public int idOrdenAseprot { get; set; }

        public static List<RelacionCitaOrdenes> listarRelacionCitaOrdenes(SqlConnection serConn)
        {
            List<RelacionCitaOrdenes> relacionCitaOrdenesList = new List<RelacionCitaOrdenes>();
            if (serConn == null)
                return relacionCitaOrdenesList;

            Console.WriteLine("Consulta * from RelacionCitaOrdenes");
            SqlCommand relacionCitaOrdenesCMD = new SqlCommand("select * from RelacionCitaOrdenes", serConn);
            DataTable dt = new DataTable();
            dt.Load(relacionCitaOrdenesCMD.ExecuteReader());
            foreach (DataRow dr in dt.Rows)
            {
                RelacionCitaOrdenes relacionCitaOrdenes = new RelacionCitaOrdenes();
                relacionCitaOrdenes.idRelacionCitaOrdenes = int.Parse(dr["idRelacionCitaOrdenes"].ToString());
                relacionCitaOrdenes.idCitaTalleres = int.Parse(dr["idCitaTalleres"].ToString());
                relacionCitaOrdenes.idTrabajoTalleres = dr["idTrabajoTalleres"].ToString() == string.Empty ? 0 : int.Parse(dr["idTrabajoTalleres"].ToString());
                relacionCitaOrdenes.idOrdenAseprot = int.Parse(dr["idOrdenesAseprot"].ToString());
                relacionCitaOrdenesList.Add(relacionCitaOrdenes);
                Console.WriteLine("RelacionCitaOrdenes agregado a lista " + relacionCitaOrdenes);
            }

            return relacionCitaOrdenesList;
        }
    }
}
