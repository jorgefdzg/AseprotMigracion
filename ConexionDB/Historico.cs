using System;
using System.Data;
using System.Data.SqlClient;

namespace ConexionDB
{
    public class HistoricoOrdenes
    {
        private int? _idHistorialEstatusOrden = null;
        private int? _idOrden = null;
        private int _idEstatusOrden = 0;
        private DateTime _fechaInicial = DateTime.MinValue;
        private DateTime? _fechaFinal = null;
        private int _idUsuario = 0;

        public int?      idHistorialEstatusOrden { get { return _idHistorialEstatusOrden; } set { _idHistorialEstatusOrden = value; } }
        public int?      idOrden                 { get { return _idOrden; } set { _idOrden = value; } }
        public int      idEstatusOrden          { get { return _idEstatusOrden; } set { _idEstatusOrden = value; } }
        public DateTime fechaInicial            { get { return _fechaInicial; } set { _fechaInicial = value; } }
        public DateTime? fechaFinal              { get { return _fechaFinal; } set { _fechaFinal = value; } }
        public int      idUsuario               { get { return _idUsuario; } set { _idUsuario = value; } }

        public void InsertData(HistoricoOrdenes historico) {
            LogWriter log = new LogWriter();

            try {
                string query = "INSERT INTO [dbo].[HistorialEstatusOrden]([idOrden],[idEstatusOrden],[fechaInicial],[fechaFinal],[idUsuario]) VALUES(@idOrden,@idEstatusOrden,@fechaInicial, @fechaFinal, @idUsuario)";
                ConexionsDBs con = new ConexionsDBs();
                using (SqlConnection cn = new SqlConnection(con.ReturnStringConnection(Constants.conexiones.ASEPROTPruebas)))
                using (SqlCommand cmd = new SqlCommand(query, cn)) {
                    if (string.IsNullOrEmpty(historico.idOrden.ToString()))
                        cmd.Parameters.Add("@idOrden", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idOrden", SqlDbType.Decimal).Value = historico.idOrden;
                    if (string.IsNullOrEmpty(historico.idEstatusOrden.ToString()))
                        cmd.Parameters.Add("@idEstatusOrden", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idEstatusOrden", SqlDbType.Int).Value = historico.idEstatusOrden;
                    if (string.IsNullOrEmpty(historico.fechaInicial.ToString()))
                        cmd.Parameters.Add("@fechaInicial", SqlDbType.DateTime).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@fechaInicial", SqlDbType.DateTime).Value = historico.fechaInicial;
                    if (string.IsNullOrEmpty(historico.fechaFinal.ToString()))
                        cmd.Parameters.Add("@fechaFinal", SqlDbType.DateTime).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@fechaFinal", SqlDbType.DateTime).Value = historico.fechaFinal;
                    if (string.IsNullOrEmpty(historico.idUsuario.ToString()))
                        cmd.Parameters.Add("@idUsuario", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idUsuario", SqlDbType.Decimal).Value = historico.idUsuario;


                    cn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    cn.Close();
                    if (rowsAffected > 0)
                        log.WriteInLog("Registro de historico  de la orden ingresado con exito " + historico.idOrden.ToString() + "  estatus:" + historico.idEstatusOrden.ToString());

                }
            }
            catch (Exception ex) {
                log.WriteInLog("Error al insertar historico de la orden " + historico.idOrden.ToString() + " estatus:" + historico.idEstatusOrden.ToString() + " Excepción:" +  ex.Message);
            }
        }
    }
}