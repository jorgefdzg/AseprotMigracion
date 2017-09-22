using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    class RelacionOsurPresupuesto
    {
        public int idOsur { get; set; }
        public int idPresupuesto { get; set; }

        public static List<RelacionOsurPresupuesto> listarRelacionOsurPresupuesto(SqlConnection serConn)
        {
            List<RelacionOsurPresupuesto> relaOsurPresupuestoList = new List<RelacionOsurPresupuesto>();
            if (serConn == null)
                return relaOsurPresupuestoList;

            Console.WriteLine("Consulta * from RelacionOsurPresupuestoList");
            SqlCommand relCMD = new SqlCommand("select  * from RelacionOsurPresupuesto", serConn);
            DataTable dt = new DataTable();
            dt.Load(relCMD.ExecuteReader());
            foreach (DataRow dr in dt.Rows)
            {
                RelacionOsurPresupuesto relacionOsurPresupuesto = new RelacionOsurPresupuesto();
                relacionOsurPresupuesto.idOsur = int.Parse(dr["idOsur"].ToString());
                relacionOsurPresupuesto.idPresupuesto = int.Parse(dr["idPresupuesto"].ToString());
                relaOsurPresupuestoList.Add(relacionOsurPresupuesto);
                Console.WriteLine("Relación Osur Presupuesto agregado a lista " + relaOsurPresupuestoList);
            }

            return relaOsurPresupuestoList;
        }
    }
}
