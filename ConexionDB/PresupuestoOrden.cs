using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    class PresupuestoOrden
    {
        public int idPresupuestoOrden { get; set; }
        public int idPresupuesto { get; set; }
        public int idOrden { get; set; }
        public DateTime fechaAlta { get; set; }
        public int idUsuario { get; set; }
        public long consecutivo { get; set; }
        public string zona { get; set; }
        public string folio { get; set; }

        public List<PresupuestoOrden> listarPresupuestoOrden(SqlConnection serConn)
        {
            Console.WriteLine("Creacion de lista presupuestoOrden");
            if(serConn.State != ConnectionState.Open)
                serConn.Open();
            Presupuesto accederPresupuesto = new Presupuesto();
            //List<Presupuesto> listaPresupuesto = accederPresupuesto.listarPresupuesto(serConn);
            List<ConsecutivoZona> listConsecutivoZona = ConsecutivoZona.listarConsecutivoZona(serConn);
            List<RelacionOsurPresupuesto> listaRelacionOsurPresupuesto = RelacionOsurPresupuesto.listarRelacionOsurPresupuesto(serConn);
            List<RelacionCitaOrdenes> listRelacionCitasOrdenes = RelacionCitaOrdenes.listarRelacionCitaOrdenes(serConn).Where(o => o.idTrabajoTalleres != null).ToList();
            List<Ordenes> listOrdenes = Ordenes.listarOrdenes(serConn);
            List<Unidades> listUnidades = Unidades.listarUnidades(serConn);
            List<CentroTrabajos> listCentroTrabajo = CentroTrabajos.listarCentroTrabajos(serConn).Where(o => o.idOperacion == 3).ToList();

            List<PresupuestoOrden> listPresupuestoOrden = new List<PresupuestoOrden>();
            foreach (ConsecutivoZona consecutivoZona in listConsecutivoZona)
            {
                //Presupuesto presupuesto = listaPresupuesto.Find(o => o.Id == consecutivoZona.idOsur);
                RelacionOsurPresupuesto relacionOsurPresu = listaRelacionOsurPresupuesto.Find(o => o.idOsur == consecutivoZona.idOsur);
                RelacionCitaOrdenes relacionCitaOrdenes = listRelacionCitasOrdenes.Find(o => o.idTrabajoTalleres == consecutivoZona.idTrabajo);
                if (relacionCitaOrdenes == null)
                    Console.Write("no existe relación de ordenes");
                Ordenes orden = null;
                Unidades unidad = null;
                CentroTrabajos centroTrabajo = null;
                if (relacionCitaOrdenes != null)
                {
                    orden = listOrdenes.Find(o => o.idOrden == relacionCitaOrdenes.idOrdenAseprot);
                    if (orden != null)
                    {
                        unidad = listUnidades.Find(o => o.idUnidad == orden.idUnidad);
                        if(unidad != null)
                            centroTrabajo = listCentroTrabajo.Find(o => o.idCentroTrabajo == unidad.idCentroTrabajo);
                        else
                            Console.Write("no existe unidad");
                    }
                    else
                        Console.Write("no existe orden");
                }
                else
                    Console.Write("no existe relación cita de ordenes");
                
                
                if (relacionOsurPresu != null && relacionCitaOrdenes != null && orden != null && unidad != null && centroTrabajo != null)
                {
                    PresupuestoOrden presupuestoOrden = new PresupuestoOrden();
                    presupuestoOrden.idPresupuesto = relacionOsurPresu.idPresupuesto;
                    presupuestoOrden.idOrden = relacionCitaOrdenes.idOrdenAseprot;
                    presupuestoOrden.fechaAlta = consecutivoZona.fechaGeneracion;
                    presupuestoOrden.idUsuario = 514;
                    presupuestoOrden.consecutivo = consecutivoZona.numeroConsecutivo;
                    presupuestoOrden.zona = consecutivoZona.idZona == 1 ? "N" : consecutivoZona.idZona == 2 ? "C" : consecutivoZona.idZona == 3 ? "P" : "G";
                    presupuestoOrden.folio = "RC-GLR" + presupuestoOrden.zona + "-" + centroTrabajo.extra1 + "-" + "00000" + consecutivoZona.numeroConsecutivo + "-" + presupuestoOrden.fechaAlta.Year;
                    listPresupuestoOrden.Add(presupuestoOrden);
                }
            }
            return listPresupuestoOrden;
        }

        public void InsertarPresupuesto(PresupuestoOrden presupuestoOrden, SqlConnection serConn, SqlTransaction transaction)
        {
            LogWriter log = new LogWriter();
            transaction = serConn.BeginTransaction("SampleTransaction");
            try
            {
                if (serConn.State != ConnectionState.Open)
                    serConn.Open();
                string query = "INSERT INTO PresupuestoOrden([idPresupuesto],[idOrden],[fechaAlta],[idUsuario],[consecutivo],[zona],[folio])VALUES(@idPresupuesto, @idOrden, @fechaAlta, @idUsuario, @consecutivo, @zona, @folio)";
                using (SqlCommand cmd = new SqlCommand(query, serConn))
                {
                    
                    cmd.Connection = serConn;
                    cmd.Transaction = transaction;


                    if (string.IsNullOrEmpty(presupuestoOrden.idPresupuesto.ToString()))
                        cmd.Parameters.Add("@idPresupuesto", SqlDbType.BigInt, 9).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idPresupuesto", SqlDbType.BigInt, 9).Value = presupuestoOrden.idPresupuesto;
                    if (string.IsNullOrEmpty(presupuestoOrden.idOrden.ToString()))
                        cmd.Parameters.Add("@idOrden", SqlDbType.BigInt, 9).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idOrden", SqlDbType.BigInt, 9).Value = presupuestoOrden.idOrden;
                    if (string.IsNullOrEmpty(presupuestoOrden.fechaAlta.ToString()))
                        cmd.Parameters.Add("@fechaAlta", SqlDbType.DateTime).Value = DateTime.Now;
                    else
                        cmd.Parameters.Add("@fechaAlta", SqlDbType.DateTime).Value = presupuestoOrden.fechaAlta;
                    if (string.IsNullOrEmpty(presupuestoOrden.idUsuario.ToString()))
                        cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = 514;
                    else
                        cmd.Parameters.Add("@idUsuario", SqlDbType.Int).Value = presupuestoOrden.idUsuario;
                    if (string.IsNullOrEmpty(presupuestoOrden.consecutivo.ToString()))
                        cmd.Parameters.Add("@consecutivo", SqlDbType.Int, 9).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@consecutivo", SqlDbType.Int, 9).Value = presupuestoOrden.consecutivo;
                    if (string.IsNullOrEmpty(presupuestoOrden.zona.ToString()))
                        cmd.Parameters.Add("@zona", SqlDbType.NVarChar, 100).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@zona", SqlDbType.NVarChar, 100).Value = presupuestoOrden.zona;
                    if (string.IsNullOrEmpty(presupuestoOrden.folio.ToString()))
                        cmd.Parameters.Add("@folio", SqlDbType.NVarChar, 300).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@folio", SqlDbType.NVarChar, 300).Value = presupuestoOrden.folio;

                    //serConn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        SqlCommand cmd2 = new SqlCommand("select top 1 idPresupuestoOrden from PresupuestoOrden order by idPresupuestoOrden desc", serConn);
                        cmd2.Connection = serConn;
                        cmd2.Transaction = transaction;
                        DataTable dt = new DataTable();
                        dt.Load(cmd2.ExecuteReader());
                        int idNuevoPresupuesto = 0;
                        string queryInsertRelacion = string.Empty;
                        if (dt.Rows.Count > 0)
                        {
                            idNuevoPresupuesto = int.Parse(dt.Rows[0]["idPresupuestoOrden"].ToString());
                            if (idNuevoPresupuesto > 0)
                            {
                                transaction.Commit();
                                log.WriteInLog("Registro de presupuesto orden " + idNuevoPresupuesto + " insertado con exito");
                            }
                            else
                                throw new Exception("Ocurrio un error al insertar la el registro de Presupuesto orden");
                        }
                    }
                    //serConn.Close();
                    
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                log.WriteInLog("Error al insertar el presupuesto orden " + presupuestoOrden.idPresupuesto + " Excepcion: " + ex.Message);
            }
        }
    }
}
