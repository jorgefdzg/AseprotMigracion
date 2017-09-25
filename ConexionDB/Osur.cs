using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    class Osur
    {
        public int idOsur { get; set; }
        public decimal presupuesto { get; set; }
        public int idTar { get; set; }
        public string folio { get; set; }
        public DateTime fechaInicial { get; set; }
        public DateTime fechaFinal { get; set; }
        public Int64 solpe { get; set; }
        public int estatus { get; set; }
        public int orden { get; set; }
        public int? idAplicacion { get; set; }
        public string presupuestoAplicacion { get; set; }
        public int idCliente { get; set; }
        public DateTime fecha { get; set; }

        public static List<Osur> listarOsur(SqlConnection serConn)
        {
            List<Osur> osurList = new List<Osur>();
            if (serConn == null)
                return osurList;

            Console.WriteLine("Consulta * from de osur");
            SqlCommand osurCMD = new SqlCommand("select  * from talleres.dbo.Osur", serConn);
            DataTable dt = new DataTable();
            dt.Load(osurCMD.ExecuteReader());
            foreach (DataRow dr in dt.Rows)
            {
                Osur objOsur = new Osur();
                objOsur.idOsur = int.Parse(dr["idOsur"].ToString());
                objOsur.presupuesto = decimal.Parse(dr["presupuesto"].ToString());
                objOsur.idTar = int.Parse(dr["idTar"].ToString());
                objOsur.folio = dr["folio"].ToString();
                objOsur.fechaInicial = DateTime.Parse(dr["fechaInicial"].ToString());
                objOsur.fechaFinal = DateTime.Parse(dr["fechaFinal"].ToString());
                objOsur.solpe = int.Parse(dr["solpe"].ToString());
                objOsur.estatus = int.Parse(dr["estatus"].ToString());
                objOsur.orden = int.Parse(dr["orden"].ToString());
                objOsur.idAplicacion = dr["idAplicacion"].ToString() != string.Empty ? int.Parse(dr["idAplicacion"].ToString()) : 0;
                objOsur.presupuestoAplicacion = dr["presupuestoAplicacion"].ToString();
                //objOsur.idCliente = int.Parse(dr["idCliente"].ToString());
                objOsur.fecha = dr["fecha"].ToString() != string.Empty ? DateTime.Parse(dr["fecha"].ToString()) : DateTime.Now;
                osurList.Add(objOsur);
                Console.WriteLine("Osur agregado a lista " + objOsur);
            }

            return osurList;
        }
    }
}
