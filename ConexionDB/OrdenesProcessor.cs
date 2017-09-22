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

        public static Ordenes CrearOrdenXCita(SqlConnection dbCnx, long aIdCita,SqlTransaction aTrans)
        {
            Ordenes orden = new Ordenes();
            //Ordenes.Export(ord)

            orden.fechaCreacionOden = GetFechaCreacion(dbCnx, aIdCita, aTrans);
            orden.fechaCita = GetFechaCita(dbCnx, aIdCita, aTrans);
            orden.fechaInicioTrabajo = GetFechaInicioTrabajo(dbCnx, aIdCita, aTrans);
            orden.consecutivoOrden = GetConsecutivoOrden(dbCnx, aIdCita, aTrans);
            orden.numeroOrden = GetNumeroOrden(dbCnx, aIdCita, orden.consecutivoOrden, aTrans);
            orden.comentarioOrden = GetComentarioOrden(dbCnx, aIdCita, aTrans);
            orden.requiereGrua = GetRequiereGrua(dbCnx, aIdCita, aTrans);
            orden.idCatalogoEstadoUnidad = GetCatalogoEstadoUnidad(dbCnx, aIdCita, aTrans);
            orden.idZona = GetZona(dbCnx, aIdCita, aTrans);
            orden.idUnidad = GetIdUnidad(dbCnx, aIdCita, aTrans);
            orden.idContratoOperacion = 3;
            orden.idUsuario = 514;
            orden.idCatalogoTipoOrdenServicio = GeIidCatalogoTipoOrdenServicio(dbCnx, aIdCita, aTrans);
            orden.idTipoOrden = GetIdTipoOrden(dbCnx, aIdCita, aTrans);
            orden.idEstatusOrden = GetIdEstatus(dbCnx, aIdCita, aTrans);
            orden.idCentroTrabajo = GetIdCentroTrabajo(dbCnx, aIdCita, aTrans);
            orden.idTaller = 0;
            orden.idGarantia = 0;
            orden.motivoGarantia = null;

            //////dbCnx.Open();
            return orden;
        }

        private static DateTime GetFechaCreacion(SqlConnection dbCnx, long aIdCita, SqlTransaction aTrans)
        {
            //////dbCnx.Open();

            SqlCommand cmd = new SqlCommand("select top 1 fecha from talleres.dbo.HistorialProceso where idTipoProceso = 2 and idEstatus = 1 and idProceso =  " + aIdCita + " order by fecha desc", dbCnx,aTrans);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            DateTime date = DateTime.Now;

            if (dt.Rows.Count > 0)
                date = DateTime.Parse(dt.Rows[0]["fecha"].ToString());

            //////dbCnx.Open();
            return date;
        }

        private static DateTime GetFechaCita(SqlConnection dbCnx, long aIdCita, SqlTransaction aTrans)
        {
            //////dbCnx.Open();

            SqlCommand cmd = new SqlCommand("select fecha from talleres.dbo.Cita where idCita = " + aIdCita + "", dbCnx, aTrans);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            DateTime date = DateTime.Now;
            if (dt.Rows.Count > 0)
                date = DateTime.Parse(dt.Rows[0]["fecha"].ToString());

            //////dbCnx.Open();
            return date;
        }

        private static DateTime GetFechaInicioTrabajo(SqlConnection dbCnx, long aIdCita, SqlTransaction aTrans)
        {
            //////dbCnx.Open();

            SqlCommand cmd = new SqlCommand("select fechaInicio from talleres.dbo.Trabajo where idCita = " + aIdCita + "", dbCnx, aTrans);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            DateTime date = DateTime.Now;
            if (dt.Rows.Count > 0)
                date = DateTime.Parse(dt.Rows[0]["fechaInicio"].ToString());

            //////dbCnx.Open();
            return date;
        }

        private static int GetConsecutivoOrden(SqlConnection dbCnx, long aIdCita, SqlTransaction aTrans)
        {

            {
                //////dbCnx.Open();
                
                SqlCommand cmd = new SqlCommand("select top 1 consecutivoOrden from dbo.Ordenes where idContratoOperacion = "+ Constants.IdContratoOperacionMigracion + " order by consecutivoOrden desc", dbCnx, aTrans);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                int consecutivo = 1;
                if (dt.Rows.Count > 0)
                    consecutivo = int.Parse(dt.Rows[0]["consecutivoOrden"].ToString()) + 1;

                //////dbCnx.Open();
                return consecutivo;
            }
        }

        private static string GetNumeroOrden(SqlConnection dbCnx, long aIdCita, int aConsecutivo, SqlTransaction aTrans)
        {
            {
                //////dbCnx.Open();

                SqlCommand cmd = new SqlCommand("select numeroTrabajo from talleres.dbo.Trabajo where idCita =  " + aIdCita + "", dbCnx, aTrans);
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
                                      where cita.idCita = " + aIdCita + "", dbCnx, aTrans);


                    dt.Load(cmd.ExecuteReader());

                    long numEco = long.Parse(dt.Rows[0]["numEconomico"].ToString());

                    int consecutivo = aConsecutivo;
                    retorno = "03-" + numEco + "-" + consecutivo;

                }
                //////dbCnx.Open();
                return retorno;
            }
        }

        private static string GetComentarioOrden(SqlConnection dbCnx, long aIdCita, SqlTransaction aTrans)
        {
            {
                //////dbCnx.Open();

                SqlCommand cmd = new SqlCommand("select {fn concat(trabajo,{fn concat('/',observacion)})} comentario from talleres.dbo.Cita where idCita = " + aIdCita + "", dbCnx, aTrans);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                string comentario = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    comentario = dt.Rows[0]["comentario"].ToString();
                }
                //////dbCnx.Open();
                return comentario;
            }
        }

        private static bool GetRequiereGrua(SqlConnection dbCnx, long aIdCita, SqlTransaction aTrans)
        {
            {
                //////dbCnx.Open();

                SqlCommand cmd = new SqlCommand("select case when idTrasladoUnidad = 2 then 1 else 0 end as requiereGrua from talleres.dbo.Cita where idCita = " + aIdCita + "", dbCnx, aTrans);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                bool bandera = false;
                if (dt.Rows.Count > 0)
                {
                    bandera = Convert.ToBoolean(dt.Rows[0]["requiereGrua"]);
                }
                //////dbCnx.Open();
                return bandera;
            }
        }

        private static int GetCatalogoEstadoUnidad(SqlConnection dbCnx, long aIdCita, SqlTransaction aTrans)
        {
            {
                //////dbCnx.Open();

                SqlCommand cmd = new SqlCommand("select case when idEstadoAutotanque = 1 then 2 else 1 end as idCatalogoEstadoUnidad from talleres.dbo.Cita where idCita = " + aIdCita + "", dbCnx, aTrans);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int id = 0;
                if (dt.Rows.Count > 0)
                {
                    id = int.Parse(dt.Rows[0]["idCatalogoEstadoUnidad"].ToString());
                }
                //////dbCnx.Open();
                return id;
            }
        }

        private static int GetZona(SqlConnection dbCnx, long aIdCita, SqlTransaction aTrans)
        {
            {
                //////dbCnx.Open();

                SqlCommand cmd = new SqlCommand(@"select ndbUni.idZona from  dbo.Unidades ndbUni
                                    left join talleres.dbo.Unidad uni on uni.numEconomico = ndbUni.numeroEconomico
                                    left join talleres.dbo.Cita cita on cita.idUnidad = uni.idUnidad
                                    where idOperacion = 3 and cita.idCita = " + aIdCita + "", dbCnx, aTrans);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int id = 0;


                if (dt.Rows.Count > 0)
                {
                    id = int.Parse(dt.Rows[0]["idZona"].ToString());
                }
                //////dbCnx.Open();
                return id;
            }
        }


        private static int GetIdUnidad(SqlConnection dbCnx, long aIdCita, SqlTransaction aTrans)
        {
            {
                //////dbCnx.Open();

                SqlCommand cmd = new SqlCommand(@"select ndbUni.idUnidad from ASEPROTDesarrollo.dbo.Unidades ndbUni
                                    left join talleres.dbo.Unidad uni on uni.numEconomico = ndbUni.numeroEconomico
                                    left join talleres.dbo.Cita cita on cita.idUnidad = uni.idUnidad
                                    where idOperacion = 3 and cita.idCita = " + aIdCita + "", dbCnx, aTrans);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int id = 0;

                if (dt.Rows.Count > 0)
                {
                    id = int.Parse(dt.Rows[0]["idUnidad"].ToString());
                }
                //////dbCnx.Open();
                return id;
            }
        }

        private static int GetIdCentroTrabajo(SqlConnection dbCnx, long aIdCita, SqlTransaction aTrans)
        {
            {
                //////dbCnx.Open();

                SqlCommand cmd = new SqlCommand(@"select ndbUni.idCentroTrabajo from dbo.Unidades ndbUni
                                        left join talleres.dbo.Unidad uni on uni.numEconomico = ndbUni.numeroEconomico
                                        left join talleres.dbo.Cita cita on cita.idUnidad = uni.idUnidad
                                        where idOperacion = 3 and cita.idCita =  " + aIdCita + "", dbCnx, aTrans);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int id = 0;

                if (dt.Rows.Count > 0)
                {
                    id = int.Parse(dt.Rows[0]["idCentroTrabajo"].ToString());
                }
                //////dbCnx.Open();
                return id;
            }
        }

        private static int GeIidCatalogoTipoOrdenServicio(SqlConnection dbCnx, long aIdCita, SqlTransaction aTrans)
        {
            {
                //////dbCnx.Open();

                SqlCommand cmd = new SqlCommand(@"select case when idTipoCita = 4 then 1 when idTipoCita < 4 then  case when idTrasladoUnidad = 2 then 3 else 2 end else 2  end
                                    as idCatalogoTipoOrdenServicio from talleres.dbo.Cita where idCita = " + aIdCita + "", dbCnx, aTrans);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int id = 0;

                if (dt.Rows.Count > 0)
                {
                    id = int.Parse(dt.Rows[0]["idCatalogoTipoOrdenServicio"].ToString());
                }
                //////dbCnx.Open();
                return id;
            }
        }

        private static int GetIdTipoOrden(SqlConnection dbCnx, long aIdCita, SqlTransaction aTrans)
        {
            {
                //////dbCnx.Open();

                SqlCommand cmd = new SqlCommand(@"select 
                                              case 
                                              when idTrasladoUnidad = 2 then 2   
                                              else 1
                                              end
                                              as idTipoOrden from talleres.dbo.Cita where idCita = " + aIdCita + "", dbCnx, aTrans);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int id = 0;

                if (dt.Rows.Count > 0)
                {
                    id = int.Parse(dt.Rows[0]["idTipoOrden"].ToString());
                }
                //////dbCnx.Open();
                return id;
            }
        }

        private static int GetIdEstatus(SqlConnection dbCnx, long aIdCita, SqlTransaction aTrans)
        {
            {
                //////dbCnx.Open();

                SqlCommand cmd = new SqlCommand(@"select case when estFinal = 1 then 1  when estFinal = 2 then 2  when estFinal = 15 then 3  when estFinal = 4 then 4  
                                        when estFinal = 5 then 5  when estFinal = 23 then 5  when estFinal = 7 then 6  when estFinal = 14 then 7  when estFinal = 19 then 7
                                        when estFinal = 20 then 7  when estFinal = 16 then 14 when estFinal = 24 then 8 when estFinal = 6 then 13 when estFinal = 22 then 13
                                        else 0
                                        end as estatus
                                        from(
                                        select 
                                        case 
										when trab.idEstatus is NULL then cita.idEstatus
                                        else trab.idEstatus
                                        end as estFinal, cita.idCita								                                       
                                        from talleres.dbo.Cita cita
                                        left
                                        join talleres.dbo.trabajo trab on trab.idCita = cita.idCita ) cit
                                        where idCita = " + aIdCita + "", dbCnx, aTrans);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                int id = 0;

                if (dt.Rows.Count > 0)
                {
                    id = int.Parse(dt.Rows[0]["estatus"].ToString());
                }

                if(id == 14)
                {
                    cmd = new SqlCommand(@"select CASE  WHEN COP_STATUS = 1 THEN 9 WHEN COP_STATUS = 2 THEN 10 WHEN COP_STATUS = 4 THEN 11 WHEN COP_STATUS = 3 THEN 12 END AS EstatusCopade from [192.168.20.31].[GAAutoExpress].[dbo].[ADE_COPADE] where COP_ORDENGLOBAL COLLATE SQL_Latin1_General_CP1_CI_AS IN(
                                    select numeroTrabajoAgrupado from talleres.dbo.TrabajoAgrupado where idTrabajoAgrupado in 
                                    (select idTrabajoAgrupado from talleres.dbo.TrabajoAgrupadoDetalle where idDatosCopadeOrden = 
                                    (select idDatosCopadeOrden from talleres.dbo.DatosCopadeOrden where idTrabajo = 
                                    (select idTrabajo from talleres.dbo.Trabajo where idCita = " + aIdCita + ")))) ",dbCnx, aTrans);
                    DataTable dt2 = new DataTable();
                    dt2.Load(cmd.ExecuteReader());
                    if(dt2.Rows.Count > 0)
                        id = int.Parse(dt2.Rows[0]["EstatusCopade"].ToString());
                }

                //////dbCnx.Open();
                return id;
            }
        }

        public static List<HistoricoOrdenes> GetHistoricosOrden(SqlConnection dbCnx, int aIdCita,int aIdOrden,int estatusOrden, SqlTransaction aTrans)
        {
            //////dbCnx.Open();
            DataTable dt = new DataTable();
            List<HistoricoOrdenes> historicos = new List<HistoricoOrdenes>();
            List<int> estatus = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 14, 13,9,10,11,12 };
            List<string> comandos = new List<string>
            {
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (1)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (2)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (15)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (4)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (5,23)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (7)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (14,19,20)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (24)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (16)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (6,22)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (99999999)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (99999999)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (99999999)  order by fecha desc" ,
                "select top 1 fecha from   (select * from talleres.dbo.HistorialProceso where  idTipoProceso <> 3 and idEstatus not in (17,3,11,12,13,21) ) his where idProceso = " + aIdCita + " and idEstatus in (99999999)  order by fecha desc" ,

            };
            SqlCommand cmd;


            for (int i = 0; i < estatus.Count; i++)
            {
                cmd = new SqlCommand(comandos[i], dbCnx, aTrans);
                dt.Load(cmd.ExecuteReader());
                if (dt.Rows.Count > 0)
                {
                    HistoricoOrdenes historico = new HistoricoOrdenes
                    {
                        idOrden = aIdOrden,
                        fechaInicial = DateTime.Parse(dt.Rows[0]["fecha"].ToString()),
                        idEstatusOrden = estatus[i],
                        idUsuario = 514
                    };
                    historicos.Add(historico);
                }else if(estatusOrden >= estatus[i])
                {
                    switch (estatusOrden)
                    {
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 13:
                            if ((estatus[i] >= 9 && estatus[i] <= 12) || estatus[i] == 14) break;
                            HistoricoOrdenes historico = new HistoricoOrdenes
                            {
                                idOrden = aIdOrden,
                                fechaInicial = historicos[historicos.Count - 1].fechaFinal ?? historicos[historicos.Count - 1].fechaInicial,
                                idEstatusOrden = estatus[i],
                                idUsuario = 514
                            };
                            historicos.Add(historico);
                            break;
                        case 14:
                            if (estatus[i] == 13) break;
                            HistoricoOrdenes historico2 = new HistoricoOrdenes
                            {
                                idOrden = aIdOrden,
                                fechaInicial = historicos[historicos.Count -1].fechaFinal ?? historicos[historicos.Count - 1].fechaInicial,
                                idEstatusOrden = estatus[i],
                                idUsuario = 514
                            };
                            historicos.Add(historico2);
                            break;
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                            bool existe = historicos.Where(o => o.idEstatusOrden == 14).ToList().Count > 0;
                            if (estatus[i] == 13 ) break;
                            if (estatusOrden < estatus[i] ) break;
                            HistoricoOrdenes historico3 = new HistoricoOrdenes
                            {
                                idOrden = aIdOrden,
                                fechaInicial = historicos[historicos.Count - 1].fechaFinal ?? historicos[historicos.Count - 1].fechaInicial,
                                idEstatusOrden = estatus[i],
                                idUsuario = 514
                            };
                            historicos.Add(historico3);
                            break;
                        default:
                            break;
                    }
                }
                
                dt.Clear();
            }

            //////dbCnx.Open();
            return historicos;
        }

        public static string GuardarRelacionCitaOrdenes(SqlConnection dbCnx, int aIdCita, int aIdOrden, SqlTransaction aTrans)
        {
            //////dbCnx.Open();
            string retorno = string.Empty;
            SqlCommand cmd = new SqlCommand("select idTrabajo from talleres.dbo.Trabajo where idCita = " + aIdCita, dbCnx, aTrans);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            LogWriter log = new LogWriter();
            if (dt.Rows.Count > 0)
            {
                string query = "insert into RelacionCitaOrdenes(idCitaTalleres,idTrabajoTalleres,idOrdenesAseprot) values("+ aIdCita + ","+ int.Parse(dt.Rows[0]["idTrabajo"].ToString()) +","+ aIdOrden + ")";
                //using (SqlConnection cn = new SqlConnection(con.ReturnStringConnection((Constants.conexiones)Constants.conexiones.ASEPROTPruebas)))
                cmd = new SqlCommand(query, dbCnx, aTrans);
                
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                    retorno = "Relación cita con orden generada.";
                else
                    throw new Exception("Ocurrio un error al insertar la el registro de relación");
            }
            else
            {
                string query = "insert into RelacionCitaOrdenes(idCitaTalleres,idOrdenesAseprot) values(" + aIdCita + "," + aIdOrden + ")";
                //using (SqlConnection cn = new SqlConnection(con.ReturnStringConnection((Constants.conexiones)Constants.conexiones.ASEPROTPruebas)))
                cmd = new SqlCommand(query, dbCnx, aTrans);
                
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                    retorno = "Relación cita con orden generada.";
                else
                    throw new Exception("Ocurrio un error al insertar la el registro de relación");
            }
            log.WriteInLog(retorno);
            //////dbCnx.Open();
            return retorno;
        }

    }
}
