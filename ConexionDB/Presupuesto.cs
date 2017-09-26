using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    class Presupuesto
    {
        public int Id { get; set; }
        public decimal DPresupuesto { get; set; }
        public string FolioPresupuesto { get; set; }
        public DateTime FechaInicioPresupuesto { get; set; }
        public DateTime FechaFinalPresupuesto { get; set; }
        public int IdCentroTrabajo { get; set; }
        public int IdEstatusPresupuesto { get; set; }
        public int Orden { get; set; }
        public DateTime FechaAlta { get; set; }
        public int IdUsuario { get; set; }
        public string Solpe { get; set; }

        public List<Presupuesto> listarPresupuesto(SqlConnection serConn)
        {
            //SqlConnection serConn = new SqlConnection(Constants.ASEPROTDesarrolloStringConn);
            //SqlConnection serConn = new SqlConnection(Constants.ASEPROTDesarrolloStringConn);
            if(serConn.State == ConnectionState.Closed)
                serConn.Open();
            
            Console.WriteLine("Creacion de lista presupuesto");
            List<Osur> listOsur = Osur.listarOsur(serConn);
            List<CentroTrabajos> listCentroTrabajo = CentroTrabajos.listarCentroTrabajos(serConn).Where(o => o.idOperacion == 3).ToList();
            List<Tar> listTar = Tar.listarTar(serConn);

            List<Presupuesto> presupuestoList = new List<Presupuesto>();
            foreach (Osur x in listOsur)
            {
                CentroTrabajos centroTrabajo = listCentroTrabajo.Find(c => c.nombreCentroTrabajo == listTar.Where(o => o.idTar == x.idTar).FirstOrDefault().TAR);
                if(centroTrabajo != null)
                {
                    Presupuesto presu = new Presupuesto();
                    presu.Id = x.idOsur;
                    presu.DPresupuesto = x.presupuesto;
                    presu.FolioPresupuesto = x.folio;
                    presu.FechaInicioPresupuesto = x.fechaInicial;
                    presu.FechaFinalPresupuesto = x.fechaFinal;
                    presu.IdCentroTrabajo = centroTrabajo.idCentroTrabajo;
                    presu.IdEstatusPresupuesto = x.estatus;
                    presu.Orden = x.orden;
                    presu.FechaAlta = x.fecha;
                    presu.IdUsuario = 514;
                    presu.Solpe = x.solpe.ToString();
                    presupuestoList.Add(presu);
                }
            }
            //serConn.Close();
            return presupuestoList;
        }


        public void InsertarPresupuesto(Presupuesto presupuesto, SqlConnection cn, SqlTransaction transaction)
        {
            LogWriter log = new LogWriter();
            transaction = cn.BeginTransaction("SampleTransaction");
            try
            {
                if (cn.State != ConnectionState.Open)
                    cn.Open();
                string query = "INSERT INTO Presupuestos([presupuesto],[folioPresupuesto],[fechaInicioPresupuesto],[fechaFinalPresupuesto],[idCentroTrabajo],[idEstatusPresupuesto],[orden],[fechaAlta],[idUsuario],[solpe])VALUES(@DPresupuesto, @FolioPresupuesto, @FechaInicioPresupuesto, @FechaFinalPresupuesto, @IdCentroTrabajo, @IdEstatusPresupuesto, @Orden, @FechaAlta, @IdUsuario, @Solpe)";
                //ConexionsDBs con = new ConexionsDBs();
                //using (SqlConnection cn = new SqlConnection(con.ReturnStringConnection(Constants.conexiones.ASEPROTPruebas)))
                //SqlConnection cn = new SqlConnection(Constants.ASEPROTDesarrolloStringConn);
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Connection = cn;
                    cmd.Transaction = transaction;

                    if (string.IsNullOrEmpty(presupuesto.DPresupuesto.ToString()))
                        cmd.Parameters.Add("@DPresupuesto", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@DPresupuesto", SqlDbType.Decimal, 9).Value = presupuesto.DPresupuesto;
                    if (string.IsNullOrEmpty(presupuesto.FolioPresupuesto.ToString()))
                        cmd.Parameters.Add("@FolioPresupuesto", SqlDbType.VarChar, 50).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@FolioPresupuesto", SqlDbType.VarChar, 50).Value = presupuesto.FolioPresupuesto;
                    if (string.IsNullOrEmpty(presupuesto.FechaInicioPresupuesto.ToString()))
                        cmd.Parameters.Add("@FechaInicioPresupuesto", SqlDbType.DateTime).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@FechaInicioPresupuesto", SqlDbType.DateTime).Value = presupuesto.FechaInicioPresupuesto;
                    if (string.IsNullOrEmpty(presupuesto.FechaFinalPresupuesto.ToString()))
                        cmd.Parameters.Add("@FechaFinalPresupuesto", SqlDbType.DateTime).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@FechaFinalPresupuesto", SqlDbType.DateTime).Value = presupuesto.FechaFinalPresupuesto;
                    if (string.IsNullOrEmpty(presupuesto.IdCentroTrabajo.ToString()))
                        cmd.Parameters.Add("@IdCentroTrabajo", SqlDbType.Int, 9).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@IdCentroTrabajo", SqlDbType.Int, 9).Value = presupuesto.IdCentroTrabajo;
                    if (string.IsNullOrEmpty(presupuesto.IdEstatusPresupuesto.ToString()))
                        cmd.Parameters.Add("@IdEstatusPresupuesto", SqlDbType.Int, 4).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@IdEstatusPresupuesto", SqlDbType.Int, 4).Value = presupuesto.IdEstatusPresupuesto;
                    if (string.IsNullOrEmpty(presupuesto.Orden.ToString()))
                        cmd.Parameters.Add("@Orden", SqlDbType.Int, 9).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@Orden", SqlDbType.Int, 9).Value = presupuesto.Orden;
                    if (string.IsNullOrEmpty(presupuesto.FechaAlta.ToString()))
                        cmd.Parameters.Add("@FechaAlta", SqlDbType.DateTime).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@FechaAlta", SqlDbType.DateTime).Value = presupuesto.FechaAlta;
                    if (string.IsNullOrEmpty(presupuesto.IdUsuario.ToString()))
                        cmd.Parameters.Add("@IdUsuario", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@IdUsuario", SqlDbType.Decimal).Value = presupuesto.IdUsuario;
                    if (string.IsNullOrEmpty(presupuesto.Solpe.ToString()))
                        cmd.Parameters.Add("@Solpe", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@Solpe", SqlDbType.Decimal).Value = presupuesto.Solpe;

                    //cn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        log.WriteInLog("Registro de presupuesto insertado con exito");
                        SqlCommand cmd2 = new SqlCommand("select top 1 idPresupuesto from Presupuestos order by idPresupuesto desc", cn);
                        cmd2.Connection = cn;
                        cmd2.Transaction = transaction;
                        DataTable dt = new DataTable();
                        dt.Load(cmd2.ExecuteReader());
                        int idNuevoPresupuesto = 0;
                        string queryInsertRelacion = string.Empty;
                        if (dt.Rows.Count > 0)
                        {
                            idNuevoPresupuesto = int.Parse(dt.Rows[0]["idPresupuesto"].ToString());
                            queryInsertRelacion = "insert into RelacionOsurPresupuesto(IdOsur,IdPresupuesto) values(" + presupuesto.Id + "," + idNuevoPresupuesto + ")";
                            SqlCommand cmd3 = new SqlCommand(queryInsertRelacion, cn);
                            cmd3.Connection = cn;
                            cmd3.Transaction = transaction;
                            int res = cmd3.ExecuteNonQuery();
                            if (res > 0)
                                log.WriteInLog("Registro de relacionOsurPresupuesto insertado con exito id : " + idNuevoPresupuesto);
                            else
                                throw new Exception("Ocurrio un error al insertar la el registro de relacionOsurPresupuesto");
                        }
                    }
                    transaction.Commit();
                    //cn.Close();
                }
                
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                log.WriteInLog("Error al insertar el presupuesto " + presupuesto.Id + " Excepcion: " + ex.Message);
            }
        }

    }
}
