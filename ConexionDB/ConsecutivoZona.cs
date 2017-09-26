using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    class ConsecutivoZona
    {
        public int idConsecutivo { get; set; }
        public int idZona { get; set; }
        public int idOsur { get; set; }
        public long numeroConsecutivo { get; set; }
        public DateTime fechaGeneracion { get; set; }
        public int idTrabajo { get; set; }
        public int idCliente { get; set; }

            public static List<ConsecutivoZona> listarConsecutivoZona(SqlConnection serConn)
            {
                List<ConsecutivoZona> consecZonaList = new List<ConsecutivoZona>();
                if (serConn == null)
                    return consecZonaList;

                Console.WriteLine("Consulta * from ConsecutivoZona");
                SqlCommand consecZonaCMD = new SqlCommand("select * from talleres.dbo.ConsecutivoZona where idZona is not null and idOsur is not null", serConn);
                DataTable dt = new DataTable();
                dt.Load(consecZonaCMD.ExecuteReader());
                foreach (DataRow dr in dt.Rows)
                {
                    ConsecutivoZona consecutivoZona = new ConsecutivoZona();
                    consecutivoZona.idConsecutivo = int.Parse(dr["idConsecutivo"].ToString());
                    consecutivoZona.idZona = int.Parse(dr["idZona"].ToString());
                    consecutivoZona.idOsur = int.Parse(dr["idOsur"].ToString());
                    consecutivoZona.numeroConsecutivo = long.Parse(dr["numeroConsecutivo"].ToString());
                    consecutivoZona.fechaGeneracion = DateTime.Parse(dr["fechaGeneracion"].ToString());
                    consecutivoZona.idTrabajo = int.Parse(dr["idTrabajo"].ToString());
                    //consecutivoZona.idCliente = int.Parse(dr["idCliente"].ToString());
                    consecZonaList.Add(consecutivoZona);
                    Console.WriteLine("ConsecutivoZona agregado a lista " + consecutivoZona);
                }

                return consecZonaList;
            }
    }
}
