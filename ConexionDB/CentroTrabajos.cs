using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    class CentroTrabajos
    {
        public int idCentroTrabajo { get; set; }
        public string nombreCentroTrabajo { get; set; }
        public int idOperacion { get; set; }
        public int extra1 { get; set; }

        public static List<CentroTrabajos> listarCentroTrabajos(SqlConnection serConn)
        {
            List<CentroTrabajos> centroTrabajosList = new List<CentroTrabajos>();
            if (serConn == null)
                return centroTrabajosList;

            Console.WriteLine("Consulta * from CentroTrabajos");
            SqlCommand ctCMD = new SqlCommand("select  * from ASEPROT.dbo.CentroTrabajos", serConn);
            DataTable dt = new DataTable();
            dt.Load(ctCMD.ExecuteReader());
            foreach (DataRow dr in dt.Rows)
            {
                CentroTrabajos centroTrabajo = new CentroTrabajos();
                centroTrabajo.idCentroTrabajo = int.Parse(dr["idCentroTrabajo"].ToString());
                centroTrabajo.nombreCentroTrabajo = dr["nombreCentroTrabajo"].ToString();
                centroTrabajo.idOperacion = int.Parse(dr["idOperacion"].ToString());
                centroTrabajo.extra1 = dr["extra1"].ToString() != string.Empty ? int.Parse(dr["extra1"].ToString()) : 0;
                centroTrabajosList.Add(centroTrabajo);
                Console.WriteLine("CentroTrabajo agregado a lista " + centroTrabajo);
            }

            return centroTrabajosList;
        }

    }
}
