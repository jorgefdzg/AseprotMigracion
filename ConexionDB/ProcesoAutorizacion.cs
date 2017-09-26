using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    public class ProcesoAutorizacion
    {
        public static void GenerarAutorizacion()
        {
            try
            {
                LogWriter log = new LogWriter();
                SqlConnection serConn = new SqlConnection(Constants.ASEPROTFastStringConn);
                serConn.Open();
                #region Notas
                SqlCommand cmd = new SqlCommand(@"use ASEPROTDesarrollo                                     
                                    BEGIN TRAN nota
                                    BEGIN TRY
                                    insert into notas (descripcionNota,idOrden,idUsuario,fechaNota,idEstatusOrden)
                                    select tn.texto,rco.idOrdenesAseprot,514,tn.fecha,1 
                                    from talleres..notas tn 
                                       inner join RelacionCitaOrdenes rco 
                                       on rco.idTrabajoTalleres=tn.idTrabajo
                                    COMMIT TRAN
                                    END TRY
                                    BEGIN CATCH
                                    ROLLBACK TRAN nota
                                    END CATCH", serConn);
                int res = cmd.ExecuteNonQuery();
                string response = string.Empty;
                if (res > 0)
                    response = "Notas insertadas correctamente.";
                log.WriteInLog(response);
                Console.WriteLine(response);
                #endregion
                serConn.Close();
                serConn.Open();
                #region Aprovaciones
                cmd = new SqlCommand(@"use ASEPROTDesarrollo

                                        BEGIN TRAN aproPro
                                        BEGIN TRY

                                        insert into AprobacionProvision(idOrden,fecha,idUsuario,estatus)
                                        (
                                          select rco.idOrdenesAseprot, ap.fecha,514, ap.estatus from talleres..AprobacionProvision ap
                                           inner join RelacionCitaOrdenes rco on rco.idTrabajoTalleres=ap.idTrabajo 
                                        ) 

                                        COMMIT TRAN aproPro
                                        END TRY
                                        BEGIN CATCH
                                        ROLLBACK TRAN aproPro
                                        END CATCH

                                        GO
                                        BEGIN TRAN aproUtil
                                        BEGIN TRY
                                        insert into AprobacionesUtilidad (idOrden,idUsuario,estatusAprobacion,margenAprobacion,fechaAprobacion)
                                        select distinct rco.idOrdenesAseprot,514,au.estatus,tablaMargen.margen,au.fecha 
                                           from talleres..AprobacionUtilidad au 
                                           inner join RelacionCitaOrdenes rco on rco.idTrabajoTalleres=au.idTrabajo
                                           inner join
                                           (   
                                           select (((tabla.totalSumaVenta-tabla.totalSumaCosto) * 100) / tabla.totalSumaVenta) margen, tabla.idTrabajo  from
                                           (   
                                           SELECT 
				                                        t.idTrabajo,
				                                        (SELECT SUM((((ISNULL(CD.precio,0) * ISNULL(CD.cantidad,0)))))
					                                        FROM talleres..CotizacionMaestro CM
					                                        INNER JOIN talleres..CotizacionDetalle CD ON CM.idCotizacion = CD.idCotizacion
					                                        INNER JOIN talleres..Item IT ON IT.idItem = CD.idElemento
					                                        INNER JOIN talleres..ItemPrecio IP ON IP.idItem = IT.idItem
					                                        INNER JOIN talleres..ListaPrecio LP ON LP.idListaPrecio = IP.idListaPrecio AND LP.idListaPrecio = (SELECT idListaPrecio FROM talleres..ListaPrecio WHERE idTaller IS NULL AND idCliente = Ct.idCliente)
					                                        INNER JOIN talleres..ItemPrecioCliente IPC ON IPC.idItemCliente = IT.idItem
					                                        INNER JOIN talleres..ListaPrecioCliente LPC ON LPC.idListaPrecioCliente = IPC.idListaPrecioCliente
					                                        INNER JOIN talleres..Trabajo TB ON CM.idTrabajo = TB.idTrabajo
					                                        INNER JOIN talleres..Cita CT ON TB.idCita = CT.idCita
					                                        INNER JOIN talleres..Unidad UD ON CT.idUnidad = UD.idUnidad
					                                        INNER JOIN talleres..Estatus E ON TB.idEstatus = E.idEstatus
					                                        WHERE TB.idTrabajo = t.idTrabajo 
					                                         AND CD.precio > 0) AS totalSumaCosto,
				                                        (SELECT 
							                                        SUM((((ISNULL(IPC.precioCliente,0) * ISNULL(CD.cantidad,0)))))
					                                        FROM talleres..CotizacionMaestro CM
					                                        INNER JOIN talleres..CotizacionDetalle CD ON CM.idCotizacion = CD.idCotizacion
					                                        INNER JOIN talleres..Item IT ON IT.idItem = CD.idElemento
					                                        INNER JOIN talleres..ItemPrecio IP ON IP.idItem = IT.idItem
					                                        INNER JOIN talleres..ListaPrecio LP ON LP.idListaPrecio = IP.idListaPrecio AND LP.idListaPrecio = (SELECT idListaPrecio FROM talleres..ListaPrecio WHERE idTaller IS NULL AND idCliente = Ct.idCliente)
					                                        INNER JOIN talleres..ItemPrecioCliente IPC ON IPC.idItemCliente = IT.idItem
					                                        INNER JOIN talleres..ListaPrecioCliente LPC ON LPC.idListaPrecioCliente = IPC.idListaPrecioCliente
					                                        INNER JOIN talleres..Trabajo TB ON CM.idTrabajo = TB.idTrabajo
					                                        INNER JOIN talleres..Cita CT ON TB.idCita = CT.idCita
					                                        INNER JOIN talleres..Unidad UD ON CT.idUnidad = UD.idUnidad
					                                        INNER JOIN talleres..Estatus E ON TB.idEstatus = E.idEstatus
					                                        WHERE TB.idTrabajo = t.idTrabajo  
					                                        ) AS totalSumaVenta 
	                                        FROM 
	                                        talleres..Trabajo t
		                                        INNER JOIN talleres..Estatus e ON e.idEstatus = t.idEstatus
		                                        INNER JOIN talleres..Cita ct ON t.idCita = ct.idCita 
		                                        JOIN talleres..Unidad U ON U.idUnidad = ct.idUnidad
		                                        JOIN talleres..Taller ta ON ct.idTaller = ta.idTaller
		                                        JOIN talleres..Tar TAR ON U.idTar = TAR.idTAR
		                                        INNER JOIN talleres..Zona Z ON Tar.idZona = z.idZona	
	                                        ) tabla 
	                                        ) tablaMargen
                                        on tablaMargen.idTrabajo=au.idTrabajo
                                        COMMIT TRAN aproUtil
                                        END TRY
                                        BEGIN CATCH
                                        ROLLBACK TRAN aproUtil
                                        END CATCH

                                         ", serConn);
                res = cmd.ExecuteNonQuery();
                response = string.Empty;
                if (res > 0)
                    response = "Aprovaciones insertadas correctamente.";
                log.WriteInLog(response);
                Console.WriteLine(response);
                #endregion
                serConn.Close();
                serConn.Open();
                #region Aprovacion respuesta
                cmd = new SqlCommand(@"use ASEPROTDesarrollo
                                

                                SET ANSI_NULLS ON
                                GO

                                SET QUOTED_IDENTIFIER ON
                                GO

                                CREATE TABLE [dbo].[AprobacionProvisionRespuesta](
	                                [idAprobacionProvisionRespuesta] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	                                [idAprobacionProvision] [numeric](18, 0) NULL,
	                                [fecha] [datetime] NULL,
	                                [idUsuario] [numeric](18, 0) NULL,
                                    CONSTRAINT [PK_AprobacionProvisionRespuesta] PRIMARY KEY CLUSTERED 
                                (
	                                [idAprobacionProvisionRespuesta] ASC
                                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                                ) ON [PRIMARY]

                                GO

                                ALTER TABLE [dbo].[AprobacionProvisionRespuesta]  WITH CHECK ADD  CONSTRAINT [FK_AprobacionProvisionRespuesta_AprobacionProvision] FOREIGN KEY([idAprobacionProvision])
                                REFERENCES [dbo].[AprobacionProvision] ([idAprobacionProvision])
                                GO

                                ALTER TABLE [dbo].[AprobacionProvisionRespuesta] CHECK CONSTRAINT [FK_AprobacionProvisionRespuesta_AprobacionProvision]
                                GO

                                BEGIN TRAN aproProRes
                                BEGIN TRY
                                insert into AprobacionProvisionRespuesta(idAprobacionProvision,fecha,idUsuario) 
                                select distinct ap.idAprobacionProvision,apr.fecha,514  from talleres..AprobacionProvisionRespuesta apr 
                                    inner join talleres..AprobacionProvision tap on tap.idAprobacionProvision=apr.idAprobacionProvision
                                    inner join RelacionCitaOrdenes rco on rco.idTrabajoTalleres=tap.idTrabajo
                                    inner join AprobacionProvision ap on rco.idOrdenesAseprot=ap.idOrden --and ap.fecha=tap.fecha  
                                where ap.idUsuario=514
                                COMMIT TRAN aproProRes
                                END TRY
                                BEGIN CATCH
                                ROLLBACK TRAN aproProRes
                                END CATCH


                                GO

                                SET ANSI_NULLS ON
                                GO

                                SET QUOTED_IDENTIFIER ON
                                GO


                                CREATE TABLE [dbo].[AprobacionUtilidadRespuesta](
	                                [idAprobacionUtilidadRespuesta] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	                                [idAprobacionUtilidad] [numeric](18, 0) NULL,
	                                [fecha] [datetime] NULL,
	                                [idUsuario] [numeric](18, 0) NULL,
                                    CONSTRAINT [PK_AprobacionUtilidadRespuesta] PRIMARY KEY CLUSTERED 
                                (
	                                [idAprobacionUtilidadRespuesta] ASC
                                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                                ) ON [PRIMARY]

                                GO

                                ALTER TABLE [dbo].[AprobacionUtilidadRespuesta]  WITH CHECK ADD  CONSTRAINT [FK_AprobacionUtilidadRespuesta_AprobacionUtilidad] FOREIGN KEY([idAprobacionUtilidad])
                                REFERENCES [dbo].[AprobacionesUtilidad] ([idAprobacionUtilidad])
                                ON DELETE CASCADE
                                GO

                                ALTER TABLE [dbo].[AprobacionUtilidadRespuesta] CHECK CONSTRAINT [FK_AprobacionUtilidadRespuesta_AprobacionUtilidad]
                                GO


                                BEGIN TRAN aproUtilRes
                                BEGIN TRY
                                insert into AprobacionUtilidadRespuesta (idAprobacionUtilidad,fecha,idUsuario)
                                select distinct au.idAprobacionUtilidad,aur.fecha,514 from talleres..AprobacionUtilidadRespuesta aur
                                    inner join talleres..AprobacionUtilidad tau on tau.idAprobacionUtilidad=aur.idAprobacionUtilidad
                                    inner join RelacionCitaOrdenes rco on tau.idTrabajo= rco.idTrabajoTalleres
                                    inner join AprobacionesUtilidad au on au.idOrden=rco.idOrdenesAseprot and au.fechaAprobacion=tau.fecha
                                where au.idUsuario=514
                                COMMIT TRAN aproUtilRes
                                END TRY
                                BEGIN CATCH
                                ROLLBACK TRAN aproUtilRes
                                END CATCH", serConn);
                res = cmd.ExecuteNonQuery();
                response = string.Empty;
                if (res > 0)
                    response = "Aprovaciones respuesta insertadas correctamente.";
                log.WriteInLog(response);
                Console.WriteLine(response);
                #endregion
                serConn.Close();
                serConn.Open();
                #region Facturacion Cotizacion
                cmd = new SqlCommand(@"use ASEPROTDesarrollo                                
                                BEGIN TRAN facCot
                                BEGIN TRY
                                insert into facturaCotizacion (idCotizacion,numFactura,uuid,fechaFactura,subTotal,iva,total,fechaAlta,idUsuario,xml,rfcEmisor)
                                select distinct cta.idCotizacionASE,fc.numFactura,fc.uuid,fc.fechaFactura,fc.subtotal,fc.iva,fc.total,fc.fechaAlta,514,fc.xmlFactura,fc.rfc from talleres..FacturaCotizacion fc
                                  inner join CotizacionTalleresASE cta on cta.idCotizacionTalleres=fc.idCotizacion 
                                COMMIT TRAN facCot
                                END TRY
                                BEGIN CATCH
                                ROLLBACK TRAN facCot
                                END CATCH", serConn);
                res = cmd.ExecuteNonQuery();
                response = string.Empty;
                if (res > 0)
                    response = "Facturacion Cotizacion insertadas correctamente.";
                log.WriteInLog(response);
                Console.WriteLine(response);
                #endregion
                serConn.Close();
                serConn.Open();
                #region Historico autorizaciones
                cmd = new SqlCommand(@"use ASEPROTDesarrollo                               
                                BEGIN TRAN histCotT
                                BEGIN TRY

                                update talleres..CotizacionMaestro
                                set idEstatus=9,idTipoCotizacion=1
                                where idCotizacion=4913


                                insert into talleres..HistorialProceso (idEstatus,fecha,idProceso,idTipoProceso)
                                select tabla.idEstatus,
                                                       isnull(
                                                       case when tabla.idEstatus=25 then tabla.fecha
                                                            when tabla.idEstatus=8  then (select fecha from talleres..HistorialProceso where idTipoProceso=3 and idProceso=tabla.idCotizacion and idEstatus=25)
							                                when tabla.idEstatus=9  then (select fecha from talleres..HistorialProceso where idTipoProceso=3 and idProceso=tabla.idCotizacion and idEstatus=8)
							                                when tabla.idEstatus=10 then (select fecha from talleres..HistorialProceso where idTipoProceso=3 and idProceso=tabla.idCotizacion and idEstatus=8)
                                                       end,tabla.fecha)
                                ,tabla.idCotizacion,3 from
                                (  
                                  select * from talleres..CotizacionMaestro cm where cm.idCotizacion not in( 
                                    select distinct cm.idCotizacion from talleres..CotizacionMaestro CM 
                                       inner join talleres..HistorialProceso HP on CM.idCotizacion = HP.idProceso and HP.idTipoProceso=3 and cm.idEstatus=hp.idEstatus
                                  )
                                )tabla

                                insert into talleres..HistorialProceso (idEstatus,fecha,idProceso,idTipoProceso)
                                select 8,fecha,idCotizacion,3 from talleres..CotizacionMaestro where idCotizacion in
                                (
                                  select distinct idProceso  from talleres..HistorialProceso where idTipoProceso=3 and idEstatus=9
                                  and idProceso not in
                                  (
                                  select distinct a.idProceso from talleres..HistorialProceso a, talleres..HistorialProceso b
                                  where a.idTipoProceso=3 and b.idTipoProceso=3 and a.idProceso=b.idProceso and a.idEstatus=9 and b.idEstatus=8
                                  )
                                  union 
                                  select distinct idProceso  from talleres..HistorialProceso where idTipoProceso=3 and idEstatus=10
                                  and idProceso not in
                                  (
                                  select distinct c.idProceso from talleres..HistorialProceso c, talleres..HistorialProceso d
                                  where c.idTipoProceso=3 and d.idTipoProceso=3 and c.idProceso=d.idProceso and c.idEstatus=10 and d.idEstatus=8
                                  ) 
                                ) 

                                COMMIT TRAN histCotT
                                END TRY
                                BEGIN CATCH
                                ROLLBACK TRAN histCotT
                                END CATCH

                                GO

                                BEGIN TRAN histCotA 
                                BEGIN TRY
                                insert into HistorialEstatusCotizacion (fechaInicial,fechaFinal,idCotizacion,idUsuario,idEstatusCotizacion)
                                select distinct (select top 1 hp.fecha from  talleres..HistorialProceso hp where hp.idTipoProceso=3 and hp.idProceso=c.idCotizacionTalleres),null,c.idCotizacionASE,514, case when hp.idEstatus=9 then 3 
                                                                                 when hp.idEstatus=10 then 4
												                                 when hp.idEstatus=8 or hp.idEstatus=25 then (select case COUNT(*)
												                                      when(select count(*) from talleres..CotizacionDetalle cd where cd.idCotizacion = hp.idProceso) then 1 
								                                                      else 2 end
												                                      )
                                                                            end
                                from  talleres..HistorialProceso hp
                                   inner join CotizacionTalleresASE c on hp.idProceso=c.idCotizacionTalleres and hp.idTipoProceso=3 order by c.idCotizacionASE
                                COMMIT TRAN histCotA 
                                END TRY
                                BEGIN CATCH
                                ROLLBACK TRAN histCotA 
                                END CATCH
                                ", serConn);
                res = cmd.ExecuteNonQuery();
                response = string.Empty;
                if (res > 0)
                    response = "Historicos autorizaciones insertados correctamente.";
                log.WriteInLog(response);
                Console.WriteLine(response);
                #endregion
                serConn.Close();
                serConn.Open();
                #region Migracion de datos en AutorizacionCotizacion 
                cmd = new SqlCommand(@"use ASEPROTDesarrollo                            
                            BEGIN TRAN autCot
                            BEGIN TRY

                            insert into AutorizacionCotizacion (fechaNotificacion,fechaAutorizacion,idCotizacion)
                            select  distinct null, (select top 1 fechaInicial from HistorialEstatusCotizacion where idCotizacion=c.idCotizacion) ,c.idCotizacion from cotizaciones c 
                              inner join CotizacionTalleresASE on c.idCotizacion=idCotizacionASE 
                              inner join HistorialEstatusCotizacion hec on hec.idCotizacion=c.idCotizacion 
                            where c.idUsuario=514 and hec.idEstatusCotizacion between 2 and 4 order by c.idCotizacion

                            COMMIT TRAN autCot
                            END TRY
                            BEGIN CATCH

                            ROLLBACK TRAN autCot
                            END CATCH

                            GO

                            BEGIN TRAN detAutCot
                            BEGIN TRY
                            insert into DetalleAutorizacionCotizacion (fechaAutorizacion,idUsuario,idAprobacionCotizacion,idEstatusAutorizacion,idCotizacionDetalle)
                            select distinct (select top 1 fechaInicial from HistorialEstatusCotizacion where idCotizacion=c.idCotizacion) ,514,ac.idAutorizacionCotizacion,cd.idEstatusPartida,cd.idCotizacionDetalle from CotizacionDetalle cd
                               inner join Cotizaciones c on cd.idCotizacion=c.idCotizacion
                               inner join AutorizacionCotizacion ac on ac.idCotizacion=c.idCotizacion
                               inner join HistorialEstatusCotizacion hec on hec.idCotizacion=c.idCotizacion
                               inner join CotizacionTalleresASE on c.idCotizacion=idCotizacionASE
                            where c.idUsuario=514 and hec.idEstatusCotizacion between 2 and 3 order by cd.idCotizacionDetalle

                            COMMIT TRAN detAutCot
                            END TRY
                            BEGIN CATCH

                            ROLLBACK TRAN detAutCot
                            END CATCH", serConn);
                res = cmd.ExecuteNonQuery();
                response = string.Empty;
                if (res > 0)
                    response = "Migracion de datos en AutorizacionCotizacion  realizado correctamente.";
                log.WriteInLog(response);
                Console.WriteLine(response);
                #endregion
                serConn.Close();
            }
            catch (Exception aE)
            {
                LogWriter log = new LogWriter();
                Console.WriteLine("Ocurrio un error : " + aE.Message + "\r\n");
                log.WriteInLog("Ocurrio un error : " + aE.Message + "\r\n");
            }
        }
    }
}
