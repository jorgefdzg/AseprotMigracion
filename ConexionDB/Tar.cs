using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    class Tar
    {
        public int idTar { get; set; }
        public string GAR { get; set; }
        public string TAR { get; set; }
        public string domicilio { get; set; }
        public string localidad { get; set; }
        public string estado { get; set; }
        public string numTar { get; set; }
        public int idZona { get; set; }

        public static List<Tar> listarTar(SqlConnection serConn)
        {
            List<Tar> tarList = new List<Tar>();
            if (serConn == null)
                return tarList;

            Console.WriteLine("Consulta * from Tar");
            SqlCommand tarCMD = new SqlCommand("select * from talleres.dbo.Tar", serConn);
            DataTable dt = new DataTable();
            dt.Load(tarCMD.ExecuteReader());
            foreach (DataRow dr in dt.Rows)
            {
                Tar tar = new Tar();
                tar.idTar = int.Parse(dr["idTar"].ToString());
                tar.GAR = dr["GAR"].ToString();
                tar.TAR = dr["TAR"].ToString();
                tar.domicilio = dr["domicilio"].ToString();
                tar.localidad = dr["localidad"].ToString();
                tar.estado = dr["estado"].ToString();
                tar.numTar = dr["numTar"].ToString();
                tar.idZona = int.Parse(dr["idZona"].ToString());
                tarList.Add(tar);
                Console.WriteLine("Tar agregado a lista " + tar);
            }

            return tarList;
        }
    }
}
