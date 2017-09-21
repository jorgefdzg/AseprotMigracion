using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ConexionDB
{
    public class CotizacionesProcessor
    {
        public static Cotizaciones GetCotizacion(long aIdCotizacion)
        {
            Cotizaciones CotizacionNueva = new Cotizaciones();

            CotizacionNueva.fechaCotizacion = GetFecha(aIdCotizacion);
            CotizacionNueva.idTaller = GetIdTaller(aIdCotizacion);
            CotizacionNueva.idUsuario = 514;
            CotizacionNueva.idEstatusCotizacion = GetEstatusCotizacion(aIdCotizacion);
            CotizacionNueva.idOrden = GetIdOrden(aIdCotizacion);
            CotizacionNueva.consecutivoCotizacion = GetConsecutivoCotizacion(CotizacionNueva.idOrden);
            CotizacionNueva.numeroCotizacion = GetNumeroCotizacion(aIdCotizacion, CotizacionNueva.consecutivoCotizacion);
            CotizacionNueva.idCatalogoTipoOrdenServicio = GetIdCatalogoTipoOrdenServicio(aIdCotizacion);
            CotizacionNueva.idPreorden = null;

            return CotizacionNueva;

        }

        private static DateTime GetFecha(long aIdCotizacion)
        {
            SqlConnection serConn = new SqlConnection(Constants.TalleresStringConn);

            serConn.Open();

            SqlCommand cmd = new SqlCommand("select fecha as fechaCotizacion from CotizacionMaestro where idCotizacion = " + aIdCotizacion, serConn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            DateTime date = DateTime.Now;

            if (dt.Rows.Count > 0)
                date = DateTime.Parse(dt.Rows[0]["fechaCotizacion"].ToString());

            serConn.Close();
            return date;

        }

        private static decimal GetIdTaller(long aIdCotizacion)
        {
            SqlConnection serConn = new SqlConnection(Constants.TalleresStringConn);

            serConn.Open();

            SqlCommand cmd = new SqlCommand(@"select isnull(P.idProveedor, (select TOP 1 Ptemp.idProveedor from Partidas..Proveedor Ptemp where Ptemp.razonSocial = T.razonSocial)) as idTaller 
                                                from Taller T
                                                left join Partidas..Proveedor P on T.razonSocial = P.razonSocial and T.direccion = P.direccion
                                                inner join Partidas..ContratoProveedor CP on CP.idProveedor = P.idProveedor and CP.idContrato = 4
                                                where idTaller = (select isnull(CM.idTaller, C.idTaller)
                                                                    from CotizacionMaestro CM
                                                                    inner
                                                                    join Trabajo T on T.idTrabajo = CM.idTrabajo
                                                              inner
                                                                    join Cita C on C.idCita = T.idCita
                                                                    where idCotizacion = " + aIdCotizacion + ") ", serConn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            decimal idTaller = 0;

            if (dt.Rows.Count > 0)
                idTaller = decimal.Parse(dt.Rows[0]["idCentroTrabajo"].ToString());

            serConn.Close();
            return idTaller;
        }

        internal static Cotizaciones GetCotizacion(SqlConnection serConn, int idCotizacion)
        {
            throw new NotImplementedException();
        }

        private static int GetEstatusCotizacion(long aIdCotizacion)
        {
            SqlConnection serConn = new SqlConnection(Constants.TalleresStringConn);

            serConn.Open();

            SqlCommand cmd = new SqlCommand(@"select case when CM.idEstatus = 9 then 3 
			                                        when CM.idEstatus = 10 then 4 
			                                        when CM.idEstatus = 8 or CM.idEstatus = 25 then (select case count(*) 
																	                                        when (select count(*) 
																			                                        from talleres..CotizacionDetalle 
																			                                        where idCotizacion = " + aIdCotizacion + ") then 1 " +
                                                                                                            @"else 2 end 
																                                        from CotizacionDetalle 
																                                        where idCotizacion = " + aIdCotizacion + " and idEstatus in (8,25)) " +
                                                    @"end as idEstatusCotizacion
                                            from talleres..CotizacionMaestro CM
                                            where CM.idCotizacion = " + aIdCotizacion, serConn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            int idEstatusCotizacion = 0;

            if (dt.Rows.Count > 0)
                idEstatusCotizacion = int.Parse(dt.Rows[0]["idEstatusCotizacion"].ToString());

            serConn.Close();
            return idEstatusCotizacion;
        }

        private static decimal GetIdOrden(long aIdCotizacion)
        {
            SqlConnection serConn = new SqlConnection(Constants.ASEPROTDesarrolloStringConn);

            serConn.Open();
            SqlCommand cmd = new SqlCommand(@"select idOrdenesAseprot
                                                from RelacionCitaOrdenes RCO
                                                inner join Talleres..CotizacionMaestro CM on CM.idTrabajo = RCO.idTrabajoTalleres
                                                where CM.idCotizacion = " + aIdCotizacion, serConn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            decimal idOrden = 0;

            if (dt.Rows.Count > 0)
                idOrden = decimal.Parse(dt.Rows[0]["idOrdenesAseprot"].ToString());

            serConn.Close();
            return idOrden;
        }

        private static int GetConsecutivoCotizacion(decimal aIdOrden)
        {
            SqlConnection serConn = new SqlConnection(Constants.ASEPROTDesarrolloStringConn);

            serConn.Open();

            SqlCommand cmd = new SqlCommand(@"IF (EXISTS(SELECT TOP 1 consecutivoCotizacion FROM [dbo].[Cotizaciones] WHERE idOrden = " + aIdOrden + ")) " +
                                                @"BEGIN
				                                    SELECT TOP 1 (consecutivoCotizacion + 1) as consecutivoCotizacion FROM [dbo].[Cotizaciones] WHERE idOrden = " + aIdOrden + " ORDER BY consecutivoCotizacion DESC" +
                                                @"END
		                                    ELSE 
			                                    BEGIN
				                                    SELECT 1 as consecutivoCotizacion
			                                    END", serConn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            int consecutivoCotizacion = 0;

            if (dt.Rows.Count > 0)
                consecutivoCotizacion = int.Parse(dt.Rows[0]["consecutivoCotizacion"].ToString());

            serConn.Close();
            return consecutivoCotizacion;
        }

        private static string GetNumeroCotizacion(long aIdCotizacion, int consecutivo)
        {
            SqlConnection serConn = new SqlConnection(Constants.ASEPROTDesarrolloStringConn);

            serConn.Open();

            SqlCommand cmd = new SqlCommand(@"select O.numeroOrden + '-' + CONVERT(varchar(3),"+consecutivo+") as numeroCotizacion " +
                                            @"from RelacionCitaOrdenes RCO
                                              inner join Talleres..CotizacionMaestro CM on CM.idTrabajo = RCO.idTrabajoTalleres
                                              inner join Ordenes O on O.idOrden = RCO.idOrdenAseprot
                                              where CM.idCotizacion = " + aIdCotizacion, serConn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            string numeroCotizacion = "";

            if (dt.Rows.Count > 0)
                numeroCotizacion = dt.Rows[0]["numeroCotizacion"].ToString();

            serConn.Close();
            return numeroCotizacion;
        }

        private static int GetIdCatalogoTipoOrdenServicio(long aIdCotizacion)
        {
            SqlConnection serConn = new SqlConnection(Constants.TalleresStringConn);

            serConn.Open();

            SqlCommand cmd = new SqlCommand(@"select case when CM.idTipoCotizacion is null 
			                                                then (case 
					                                                when C.idTipoCita = 4 then 1 
					                                                when C.idTipoCita < 4 then 
					                                                case when C.idTrasladoUnidad = 2 then 3
					                                                else 2
					                                                end   
					                                                end )
			                                                else (case CM.idTipoCotizacion when 1 then 2 else 1 end) end as idCatalogoTipoOrdenServicio
                                                from CotizacionMaestro CM
                                                inner join Trabajo T on T.idTrabajo = CM.idTrabajo
                                                inner join Cita C on C.idCita = T.idCita
                                                where Cm.idCotizacion = " + aIdCotizacion, serConn);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            int idCatalogoTipoOrdenServicio = 0;

            if (dt.Rows.Count > 0)
                idCatalogoTipoOrdenServicio = int.Parse(dt.Rows[0]["idCatalogoTipoOrdenServicio"].ToString());

            serConn.Close();
            return idCatalogoTipoOrdenServicio;

        }
    }
}
