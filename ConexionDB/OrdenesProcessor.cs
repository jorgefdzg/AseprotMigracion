using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    public class OrdenesProcessor
    {

        public static Ordenes CrearOrdenXCita(SqlConnection dbCnx, long aIdCita)
        {
            Ordenes orden = new Ordenes();
            //Ordenes.Export(ord)

            orden.fechaCreacionOden = GetFechaCreacion(dbCnx, aIdCita);
            orden.fechaCita = GetFechaCita(dbCnx, aIdCita);
            orden.fechaInicioTrabajo = GetFechaInicioTrabajo(dbCnx, aIdCita);
            orden.consecutivoOrden = GetConsecutivoOrden(dbCnx, aIdCita);
            orden.numeroOrden = GetNumeroOrden(dbCnx, aIdCita, orden.consecutivoOrden);
            orden.comentarioOrden = GetComentarioOrden(dbCnx, aIdCita);
            orden.requiereGrua = GetRequiereGrua(dbCnx, aIdCita);
            orden.idCatalogoEstadoUnidad = GetCatalogoEstadoUnidad(dbCnx, aIdCita);
            orden.idZona = GetZona(dbCnx, aIdCita);
            orden.idUnidad = GetIdUnidad(dbCnx, aIdCita);
            orden.idContratoOperacion = 3;
            orden.idUsuario = 171;
            orden.idCatalogoTipoOrdenServicio = GeIidCatalogoTipoOrdenServicio(dbCnx, aIdCita);
            orden.idTipoOrden = GetIdTipoOrden(dbCnx, aIdCita);
            orden.idEstatusOrden = GetIdEstatus(dbCnx, aIdCita);
            orden.idCentroTrabajo = GetIdCentroTrabajo(dbCnx, aIdCita);
            orden.idTaller = 0;
            orden.idGarantia = 0;
            orden.motivoGarantia = null;

            dbCnx.Close();
            return orden;
        }

        private static DateTime GetFechaCreacion(SqlConnection dbCnx, long aIdCita)
        {
            dbCnx.Open();

            SqlCommand cmd = new SqlCommand("select top 1 fecha from talleres.dbo.HistorialProceso where idTipoProceso = 2 and idEstatus = 1 and idProceso =  " + aIdCita + " order by fecha desc", dbCnx);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            DateTime date = DateTime.Now;

            if (dt.Rows.Count > 0)
                date = DateTime.Parse(dt.Rows[0]["fecha"].ToString());

            dbCnx.Close();
            return date;
        }

        private static DateTime GetFechaCita(SqlConnection dbCnx, long aIdCita)
        {
            dbCnx.Open();

            SqlCommand cmd = new SqlCommand("select fecha from talleres.dbo.Cita where idCita = " + aIdCita + "", dbCnx);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            DateTime date = DateTime.Now;
            if (dt.Rows.Count > 0)
                date = DateTime.Parse(dt.Rows[0]["fecha"].ToString());

            dbCnx.Close();
            return date;
        }

        private static DateTime GetFechaInicioTrabajo(SqlConnection dbCnx, long aIdCita)
        {
            dbCnx.Open();

            SqlCommand cmd = new SqlCommand("select fechaInicio from talleres.dbo.Trabajo where idCita = " + aIdCita + "", dbCnx);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            DateTime date = DateTime.Now;
            if (dt.Rows.Count > 0)
                date = DateTime.Parse(dt.Rows[0]["fechaInicio"].ToString());

            dbCnx.Close();
            return date;
        }

        private static int GetConsecutivoOrden(SqlConnection dbCnx, long aIdCita)
        {

            {
                dbCnx.Open();

                SqlCommand cmd = new SqlCommand("select top 1 consecutivoOrden from dbo.Ordenes order by consecutivoOrden desc", dbCnx);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                int consecutivo = 0;
                if (dt.Rows.Count > 0)
                    consecutivo = int.Parse(dt.Rows[0]["consecutivoOrden"].ToString()) + 1;

                dbCnx.Close();
                return consecutivo;
            }
        }

        private static string GetNumeroOrden(SqlConnection dbCnx, long aIdCita, int aConsecutivo)
        {
            {
                dbCnx.Open();

                SqlCommand cmd = new SqlCommand("select numeroTrabajo from talleres.dbo.Trabajo where idCita =  " + aIdCita + "", dbCnx);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                string retorno = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    retorno = dt.Rows[0]["numeroTrabajo"].ToString();
                }
                else
                {
                    cmd = new SqlCommand(@"select numEconomico from talleres.dbo.Unidad uni
                                      inner join talleres.dbo.Cita cita on cita.idUnidad = uni.idUnidad
                                      where cita.idCita = " + aIdCita + "", dbCnx);


                    dt.Load(cmd.ExecuteReader());

                    long numEco = long.Parse(dt.Rows[0]["numEconomico"].ToString());

                    int consecutivo = aConsecutivo + 1;
                    retorno = "03-" + numEco + "-" + consecutivo;

                }
                dbCnx.Close();
                return retorno;
            }
        }

        private static string GetComentarioOrden(SqlConnection dbCnx, long aIdCita)
        {
            {
                dbCnx.Open();

                SqlCommand cmd = new SqlCommand("select {fn concat(trabajo,{fn concat('/',observacion)})} comentario from talleres.dbo.Cita where idCita = " + aIdCita + "", dbCnx);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                string comentario = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    comentario = dt.Rows[0]["comentario"].ToString();
                }
                dbCnx.Close();
                return comentario;
            }
        }

        private static bool GetRequiereGrua(SqlConnection dbCnx, long aIdCita)
        {
            {
                dbCnx.Open();

                SqlCommand cmd = new SqlCommand("select case when idTrasladoUnidad = 2 then 1 else 0 end as requiereGrua from talleres.dbo.Cita where idCita = " + aIdCita + "", dbCnx);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                bool bandera = false;
                if (dt.Rows.Count > 0)
                {
                    bandera = Convert.ToBoolean(dt.Rows[0]["requiereGrua"]);
                }
                dbCnx.Close();
                return bandera;
            }
        }

        private static int GetCatalogoEstadoUnidad(SqlConnection dbCnx, long aIdCita)
        {
            {
                dbCnx.Open();

                SqlCommand cmd = new SqlCommand("select case when idEstadoAutotanque = 1 then 2 else 1 end as idCatalogoEstadoUnidad from talleres.dbo.Cita where idCita = " + aIdCita + "", dbCnx);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int id = 0;
                if (dt.Rows.Count > 0)
                {
                    id = int.Parse(dt.Rows[0]["idCatalogoEstadoUnidad"].ToString());
                }
                dbCnx.Close();
                return id;
            }
        }

        private static int GetZona(SqlConnection dbCnx, long aIdCita)
        {
            {
                dbCnx.Open();

                SqlCommand cmd = new SqlCommand(@"select ndbUni.idZona from  dbo.Unidades ndbUni
                                    left join talleres.dbo.Unidad uni on uni.numEconomico = ndbUni.numeroEconomico
                                    left join talleres.dbo.Cita cita on cita.idUnidad = uni.idUnidad
                                    where idOperacion = 3 and cita.idCita = " + aIdCita + "", dbCnx);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int id = 0;


                if (dt.Rows.Count > 0)
                {
                    id = int.Parse(dt.Rows[0]["idZona"].ToString());
                }
                dbCnx.Close();
                return id;
            }
        }


        private static int GetIdUnidad(SqlConnection dbCnx, long aIdCita)
        {
            {
                dbCnx.Open();

                SqlCommand cmd = new SqlCommand(@"select ndbUni.idUnidad from ASEPROTDesarrollo.dbo.Unidades ndbUni
                                    left join talleres.dbo.Unidad uni on uni.numEconomico = ndbUni.numeroEconomico
                                    left join talleres.dbo.Cita cita on cita.idUnidad = uni.idUnidad
                                    where idOperacion = 3 and cita.idCita = " + aIdCita + "", dbCnx);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int id = 0;

                if (dt.Rows.Count > 0)
                {
                    id = int.Parse(dt.Rows[0]["idUnidad"].ToString());
                }
                dbCnx.Close();
                return id;
            }
        }

        private static int GetIdCentroTrabajo(SqlConnection dbCnx, long aIdCita)
        {
            {
                dbCnx.Open();

                SqlCommand cmd = new SqlCommand(@"select ndbUni.idCentroTrabajo from dbo.Unidades ndbUni
                                        left join talleres.dbo.Unidad uni on uni.numEconomico = ndbUni.numeroEconomico
                                        left join talleres.dbo.Cita cita on cita.idUnidad = uni.idUnidad
                                        where idOperacion = 3 and cita.idCita =  " + aIdCita + "", dbCnx);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int id = 0;

                if (dt.Rows.Count > 0)
                {
                    id = int.Parse(dt.Rows[0]["idCentroTrabajo"].ToString());
                }
                dbCnx.Close();
                return id;
            }
        }

        private static int GeIidCatalogoTipoOrdenServicio(SqlConnection dbCnx, long aIdCita)
        {
            {
                dbCnx.Open();

                SqlCommand cmd = new SqlCommand(@"select case when idTipoCita = 4 then 1 when idTipoCita < 4 then  case when idTrasladoUnidad = 2 then 3 else 2 end else 2  end
                                    as idCatalogoTipoOrdenServicio from talleres.dbo.Cita where idCita = " + aIdCita + "", dbCnx);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int id = 0;

                if (dt.Rows.Count > 0)
                {
                    id = int.Parse(dt.Rows[0]["idCatalogoTipoOrdenServicio"].ToString());
                }
                dbCnx.Close();
                return id;
            }
        }

        private static int GetIdTipoOrden(SqlConnection dbCnx, long aIdCita)
        {
            {
                dbCnx.Open();

                SqlCommand cmd = new SqlCommand(@"select 
                                              case 
                                              when idTrasladoUnidad = 2 then 2   
                                              else 1
                                              end
                                              as idTipoOrden from talleres.dbo.Cita where idCita = " + aIdCita + "", dbCnx);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int id = 0;

                if (dt.Rows.Count > 0)
                {
                    id = int.Parse(dt.Rows[0]["idTipoOrden"].ToString());
                }
                dbCnx.Close();
                return id;
            }
        }

        private static int GetIdEstatus(SqlConnection dbCnx, long aIdCita)
        {
            {
                dbCnx.Open();

                SqlCommand cmd = new SqlCommand(@"select case when estFinal = 1 then 1  when estFinal = 2 then 2  when estFinal = 15 then 3  when estFinal = 4 then 4  
                                        when estFinal = 5 then 5  when estFinal = 23 then 5  when estFinal = 7 then 6  when estFinal = 14 then 7  when estFinal = 19 then 7
                                        when estFinal = 20 then 7  when estFinal = 16 then 8 when estFinal = 24 then 9 when estFinal = 6 then 13 when estFinal = 22 then 13
                                        else 0
                                        end as estatus
                                        from(
                                        select 
                                        case 
                                        when trab.idEstatus != null then trab.idEstatus
                                        else cita.idEstatus
                                        end as estFinal, cita.idCita
                                        --cita.idEstatus as estatusCita, trab.idEstatus as estatusTrab
                                        from talleres.dbo.Cita cita
                                        left
                                        join talleres.dbo.trabajo trab on trab.idCita = cita.idCita ) cit
                                        where idCita = " + aIdCita + "", dbCnx);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int id = 0;

                if (dt.Rows.Count > 0)
                {
                    id = int.Parse(dt.Rows[0]["estatus"].ToString());
                }
                dbCnx.Close();
                return id;
            }
        }

        public static List<Historico> GetHistoricosOrden(SqlConnection dbCnx, int aIdCita)
        {
            dbCnx.Open();
            DataTable dt = new DataTable();
            List<Historico> historicos = new List<Historico>();
            List<int> estatus = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 13 };
            List<string> comandos = new List<string>
            {
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (1)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (2)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (15)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (4)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (5,23)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (7)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (14,19,20)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (16)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (24)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (6,22)  order by fecha desc" ,

            };
            SqlCommand cmd;


            for (int i = 0; i < estatus.Count; i++)
            {
                cmd = new SqlCommand(comandos[i], dbCnx);
                dt.Load(cmd.ExecuteReader());
                if (dt.Rows.Count > 0)
                {
                    Historico historico = new Historico
                    {
                        fechaInicial = DateTime.Parse(dt.Rows[0]["fecha"].ToString()),
                        idEstatusOrden = estatus[i],
                        idUsuario = 171
                    };
                    historicos.Add(historico);
                }
                dt.Clear();
            }

            dbCnx.Close();
            return historicos;
        }

        public static void GuardarRelacionCitaOrdenes(SqlConnection dbCnx, int aIdCita)
        {
            dbCnx.Open();

            SqlCommand cmd = new SqlCommand("select idTrabajo from talleres.dbo.Trabajo where idCita = " + aIdCita, dbCnx);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            if (dt.Rows.Count > 0)
            {

            }
            dbCnx.Close();

        }

    }
}
