using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    class Unidades
    {
        public int idUnidad { get; set; }
        public string numeroEconomico { get; set; }
        public string vin { get; set; }
        public int gps { get; set; }
        public int idTipoUnidad { get; set; }
        public int sustituto { get; set; }
        public int idOperacion { get; set; }
        public int idCentroTrabajo { get; set; }
        public string placas { get; set; }
        public int idZona { get; set; }
        public string modelo { get; set; }
        public string combustible { get; set; }
        public bool verificada { get; set; }
        public int idUsuario { get; set; }
        public DateTime fecha { get; set; }
        public string frente { get; set; }
        public string derecho { get; set; }
        public string izquierdo { get; set; }
        public string atras { get; set; }
        public string tarjeta { get; set; }
        public string autorizacion { get; set; }
        public string repuve { get; set; }
        public string placavin { get; set; }
        public string verificacionAmbiental { get; set; }
        public DateTime fechaVencimientoVerificacionAmbiental { get; set; }
        public string verificacionFisicoMecanica { get; set; }
        public DateTime fechaVencimientoVerificacionFisicoMecanica { get; set; }
        public string refrendo { get; set; }
        public DateTime fechaVencimientoRefrendo { get; set; }
        public string tenencia { get; set; }
        public DateTime fechaVencimientoTenencia { get; set; }

        public static List<Unidades> listarUnidades(SqlConnection serConn)
        {
            List<Unidades> unidadesList = new List<Unidades>();
            if (serConn == null)
                return unidadesList;

            Console.WriteLine("Consulta * from Unidades");
            SqlCommand unidCMD = new SqlCommand("select * from Unidades", serConn);
            DataTable dt = new DataTable();
            dt.Load(unidCMD.ExecuteReader());
            foreach (DataRow dr in dt.Rows)
            {
                Unidades unidades = new Unidades();
                unidades.idUnidad = int.Parse(dr["idUnidad"].ToString());
                //unidades.numeroEconomico = dr["numeroEconomico"].ToString();
                //unidades.vin = dr["vin"].ToString();
                //unidades.gps = int.Parse(dr["gps"].ToString());
                //unidades.idTipoUnidad = dr["idTipoUnidad"].ToString() != string.Empty ? int.Parse(dr["idTipoUnidad"].ToString()) : 0;//Preguntar
                //unidades.sustituto = dr["sustituto"].ToString() != string.Empty ? int.Parse(dr["sustituto"].ToString()) : 0;//Preguntar
                //unidades.idOperacion = int.Parse(dr["idOperacion"].ToString());
                unidades.idCentroTrabajo = int.Parse(dr["idCentroTrabajo"].ToString());
                //unidades.placas = dr["placas"].ToString();
                //unidades.idZona = int.Parse(dr["idZona"].ToString());
                //unidades.modelo = dr["modelo"].ToString();
                //unidades.combustible = dr["combustible"].ToString();
                //unidades.verificada = bool.Parse(dr["verificada"].ToString());
                //unidades.idUsuario = dr["idUsuario"].ToString() != string.Empty ? int.Parse(dr["idUsuario"].ToString()) : 0;//preguntar
                //unidades.fecha = dr["fecha"].ToString() != string.Empty ? DateTime.Parse(dr["fecha"].ToString()) : DateTime.Now;//preguntar
                //unidades.frente = dr["frente"].ToString();
                //unidades.derecho = dr["derecho"].ToString();
                //unidades.izquierdo = dr["izquierdo"].ToString();
                //unidades.atras = dr["atras"].ToString();
                //unidades.tarjeta = dr["tarjeta"].ToString();
                //unidades.autorizacion = dr["autorizacion"].ToString();
                //unidades.repuve = dr["repuve"].ToString();
                //unidades.placavin = dr["placavin"].ToString();
                //unidades.verificacionAmbiental = dr["verificacionAmbiental"].ToString();
                //unidades.fechaVencimientoVerificacionAmbiental = dr["fechaVencimientoVerificacionAmbiental"].ToString() != string.Empty ? DateTime.Parse(dr["fechaVencimientoVerificacionAmbiental"].ToString()) : DateTime.Now;//Preguntar
                //unidades.verificacionFisicoMecanica = dr["verificacionFisicoMecanica"].ToString();
                //unidades.fechaVencimientoVerificacionFisicoMecanica = dr["fechaVencimientoVerificacionFisicoMecanica"].ToString() != string.Empty ? DateTime.Parse(dr["fechaVencimientoVerificacionFisicoMecanica"].ToString()) : DateTime.Now; //Preguntar;
                //unidades.refrendo = dr["refrendo"].ToString();
                //unidades.fechaVencimientoRefrendo = dr["fechaVencimientoRefrendo"].ToString() != string.Empty ? DateTime.Parse(dr["fechaVencimientoRefrendo"].ToString()) : DateTime.Now;//Preguntar
                //unidades.tenencia = dr["tenencia"].ToString();
                //unidades.fechaVencimientoTenencia = dr["fechaVencimientoTenencia"].ToString() != string.Empty ? DateTime.Parse(dr["fechaVencimientoTenencia"].ToString()) : DateTime.Now;//Preguntar
                unidadesList.Add(unidades);
                Console.WriteLine("Unidad agregada a lista " + unidades);
            }
            return unidadesList;
        }
    }
}
