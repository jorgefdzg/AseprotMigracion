using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
namespace ConexionDB
{
    class TraspasoPresupuesto
    {
        public int idTraspaso { get; set; }
        public int idPresupuestoOrigen { get; set; }
        public int idPresupuestoDestino { get; set; }
        public decimal monto { get; set; }

        public List<TraspasoPresupuesto> listarTraspasoPresupuesto(SqlConnection serConn)
        {
            CultureInfo[] cultures = { new CultureInfo("en-US"),
                                 new CultureInfo("fr-FR") };
            if (serConn.State != ConnectionState.Open)
                serConn.Open();
            List<TraspasoPresupuesto> traspasoPresupuestoList = new List<TraspasoPresupuesto>();
            if (serConn == null)
                return traspasoPresupuestoList;

            List<Osur> osurList = Osur.listarOsur(serConn).Where(o => o.idAplicacion != null && o.idAplicacion > 0).ToList();
            List<RelacionOsurPresupuesto> listRelacionOsurPresupuiersto = RelacionOsurPresupuesto.listarRelacionOsurPresupuesto(serConn);

            foreach(Osur osur in osurList)
            {
                RelacionOsurPresupuesto relacionOrigen = listRelacionOsurPresupuiersto.Find(o => o.idOsur == osur.idOsur);
                RelacionOsurPresupuesto relacionDestino = listRelacionOsurPresupuiersto.Find(o => o.idOsur == osur.idAplicacion);
                if (relacionOrigen != null && relacionDestino != null)
                {
                    TraspasoPresupuesto traspaso = new TraspasoPresupuesto();
                    traspaso.idPresupuestoOrigen = relacionOrigen.idPresupuesto;
                    traspaso.idPresupuestoDestino = relacionDestino.idPresupuesto;
                    traspaso.monto = Convert.ToDecimal(osur.presupuestoAplicacion, cultures[0]);
                    traspasoPresupuestoList.Add(traspaso);
                }
            }

            return traspasoPresupuestoList;
        }

        public void InsertarTraspaso(TraspasoPresupuesto traspasoPresupuesto, SqlConnection serConn, SqlTransaction transaction)
        {
            LogWriter log = new LogWriter();
            transaction = serConn.BeginTransaction("SampleTransaction");
            try
            {
                if (serConn.State != ConnectionState.Open)
                    serConn.Open();
                string query = "INSERT INTO TraspasoPresupuesto([idPresupuestoOrigen],[idPresupuestoDestino],[monto])VALUES(@idPresupuestoOrigen, @idPresupuestoDestino, @monto)";
                using (SqlCommand cmd = new SqlCommand(query, serConn))
                {

                    cmd.Connection = serConn;
                    cmd.Transaction = transaction;

                    if (string.IsNullOrEmpty(traspasoPresupuesto.idPresupuestoOrigen.ToString()))
                        cmd.Parameters.Add("@idPresupuestoOrigen", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idPresupuestoOrigen", SqlDbType.Int).Value = traspasoPresupuesto.idPresupuestoOrigen;
                    if (string.IsNullOrEmpty(traspasoPresupuesto.idPresupuestoDestino.ToString()))
                        cmd.Parameters.Add("@idPresupuestoDestino", SqlDbType.Int).Value = DateTime.Now;
                    else
                        cmd.Parameters.Add("@idPresupuestoDestino", SqlDbType.Int).Value = traspasoPresupuesto.idPresupuestoDestino;
                    if (string.IsNullOrEmpty(traspasoPresupuesto.monto.ToString()))
                        cmd.Parameters.Add("@monto", SqlDbType.Decimal).Value = 0;
                    else
                        cmd.Parameters.Add("@monto", SqlDbType.Decimal).Value = traspasoPresupuesto.monto;
                    //serConn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        SqlCommand cmd2 = new SqlCommand("select top 1 idTraspaso from TraspasoPresupuesto order by idTraspaso desc", serConn);
                        cmd2.Connection = serConn;
                        cmd2.Transaction = transaction;
                        DataTable dt = new DataTable();
                        dt.Load(cmd2.ExecuteReader());
                        int idNuevoPresupuesto = 0;
                        string queryInsertRelacion = string.Empty;
                        if (dt.Rows.Count > 0)
                        {
                            idNuevoPresupuesto = int.Parse(dt.Rows[0]["idTraspaso"].ToString());
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
                log.WriteInLog("Error al insertar el presupuesto orden " + traspasoPresupuesto.idPresupuestoOrigen + " Excepcion: " + ex.Message);
            }
        }

    }
}
