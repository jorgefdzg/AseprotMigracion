﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    public class Cotizaciones
    {
        public decimal idCotizacion { get; set; }
        public DateTime fechaCotizacion { get; set; }
        public decimal idTaller { get; set; }
        public decimal idUsuario { get; set; }
        public int idEstatusCotizacion { get; set; }
        public decimal idOrden { get; set; }
        public string numeroCotizacion{ get; set; }
        public int consecutivoCotizacion { get; set; }
        public int idCatalogoTipoOrdenServicio { get; set; }
        public decimal? idPreorden { get; set; }




        public static void InsertData(SqlConnection cn, Cotizaciones cotizacion, ref decimal IdCotizacionNueva)
        {
            LogWriter log = new LogWriter();
            try
            {
                string query = "INSERT INTO Cotizaciones([fechaCotizacion],[idTaller],[idUsuario],[idEstatusCotizacion],[idOrden],[numeroCotizacion],[consecutivoCotizacion],[idCatalogoTipoOrdenServicio],[idPreorden])VALUES(@fechaCotizacion, @idTaller, @idUsuario, @idEstatusCotizacion,@idOrden,@numeroCotizacion, @consecutivoCotizacion,@idCatalogoTipoOrdenServicio, @idPreorden)";
                ConexionsDBs con = new ConexionsDBs();
                //using (SqlConnection cn = new SqlConnection(con.ReturnStringConnection(Constants.conexiones.ASEPROTPruebas)))
                using (SqlCommand cmd = new SqlCommand(query,cn)) {
                    if (string.IsNullOrEmpty(cotizacion.fechaCotizacion.ToString()))
                        cmd.Parameters.Add("@fechaCotizacion", SqlDbType.DateTime).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@fechaCotizacion", SqlDbType.DateTime).Value = cotizacion.fechaCotizacion;
                    if (string.IsNullOrEmpty(cotizacion.idTaller.ToString()))
                        cmd.Parameters.Add("@idTaller", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idTaller", SqlDbType.Decimal).Value = cotizacion.idTaller;
                    if (string.IsNullOrEmpty(cotizacion.idUsuario.ToString()))
                        cmd.Parameters.Add("@idUsuario", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idUsuario", SqlDbType.Decimal).Value = cotizacion.idUsuario;
                    if (string.IsNullOrEmpty(cotizacion.idEstatusCotizacion.ToString()))
                        cmd.Parameters.Add("@idEstatusCotizacion", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idEstatusCotizacion", SqlDbType.Int).Value = cotizacion.idEstatusCotizacion;
                    if (string.IsNullOrEmpty(cotizacion.idOrden.ToString()))
                        cmd.Parameters.Add("@idOrden", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idOrden", SqlDbType.Decimal).Value = cotizacion.idOrden;
                    if (string.IsNullOrEmpty(cotizacion.numeroCotizacion))
                        cmd.Parameters.Add("@numeroCotizacion", SqlDbType.VarChar, 50).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@numeroCotizacion", SqlDbType.VarChar, 50).Value = cotizacion.numeroCotizacion;
                    if (string.IsNullOrEmpty(cotizacion.consecutivoCotizacion.ToString()))
                        cmd.Parameters.Add("@consecutivoCotizacion", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@consecutivoCotizacion", SqlDbType.Int).Value = cotizacion.consecutivoCotizacion;
                    if (string.IsNullOrEmpty(cotizacion.idCatalogoTipoOrdenServicio.ToString()))
                        cmd.Parameters.Add("@idCatalogoTipoOrdenServicio", SqlDbType.Int).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idCatalogoTipoOrdenServicio", SqlDbType.Int).Value = cotizacion.idCatalogoTipoOrdenServicio;
                    if (string.IsNullOrEmpty(cotizacion.idPreorden.ToString()))
                        cmd.Parameters.Add("@idPreorden", SqlDbType.Decimal).Value = DBNull.Value;
                    else
                        cmd.Parameters.Add("@idPreorden", SqlDbType.Decimal).Value = cotizacion.idPreorden;

                    
                    int rowsAffected = cmd.ExecuteNonQuery();
                    
                    if (rowsAffected > 0)
                    {
                        log.WriteInLog("Registro de cotización insertado con exito " + cotizacion.numeroCotizacion);
                       
                        SqlCommand cmd2 = new SqlCommand("select top 1 idCotizacion from Cotizaciones order by idCotizacion desc", cn);
                        DataTable dt = new DataTable();
                        dt.Load(cmd2.ExecuteReader());
                        if (dt.Rows.Count > 0)
                            IdCotizacionNueva = decimal.Parse(dt.Rows[0]["idCotizacion"].ToString());

                        
                    }

                }
            }
            catch (Exception ex) {
                log.WriteInLog("Error al insertar la cotización " + cotizacion.numeroCotizacion + " Excepcion: " + ex.Message);
            }
        }

    }
}
