using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.CSharp;
using System.Data;

namespace ConexionDB
{
    class Program
    {
        static void Main(string[] args)
        {
            GeneralProcessor.MigracionGeneral();
            GeneralProcessor.MigracionCotizacion();
            SqlConnection serConn = new SqlConnection(Constants.ASEPROTDesarrolloStringConn);
            CotizacionDetalle.GuardarCotizacionDetalleCompleto(serConn);
            GeneralProcessor.migracion8();
            ProcesoAutorizacion.GenerarAutorizacion();
            ProcesoCopade.GenerarCopade();
            Console.ReadLine();
            //InsertOrdenesData();
            //SqlConnection serConn = new SqlConnection(Constants.ASEPROTDesarrolloStringConn);

            //SqlCommand ordCMD = serConn.CreateCommand();
            //ordCMD.CommandText = "Select * from Ordenes";

            //serConn.Open();
            //DataTable t1 = new DataTable();
            //using (SqlDataAdapter a = new SqlDataAdapter(ordCMD)) {
            //    a.Fill(t1);
            //}
            //serConn.Close();
            //Console.ReadLine();

          }
        //public static void InsertOrdenesData() {
        //    Ordenes ord = new Ordenes();
        //    ord.fechaCreacionOden = DateTime.Now;
        //    ord.fechaCita = ord.fechaCreacionOden.AddHours(2);
        //    ord.numeroOrden = "01-1810057-582";
        //    ord.consecutivoOrden = 582;
        //    ord.comentarioOrden = "Prueba de migración";
        //    ord.requiereGrua = false;
        //    ord.idCatalogoEstadoUnidad = 1;
        //    ord.idZona = 34;
        //    ord.idUnidad = 1613;
        //    ord.idContratoOperacion = 1;
        //    ord.idUsuario = 510;
        //    ord.idCatalogoTipoOrdenServicio = 2;
        //    ord.idTipoOrden = 1;
        //    ord.idEstatusOrden = 1;
        //    ord.idCentroTrabajo = 20;
        //    ord.idTaller = 160;
        //    ord.idGarantia = 0;
        //    ord.motivoGarantia = "";

        //    ord.InsertData(ord);


        //}

    }
}
