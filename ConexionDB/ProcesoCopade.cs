using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    public class ProcesoCopade
    {
        public static void GenerarCopade()
        {
            try
            {
                SqlConnection serConn = new SqlConnection(Constants.ASEPROTDesarrolloStringConn);
                serConn.Open();
                SqlCommand cmd = new SqlCommand(@"USE [ASEPROTDesarrollo]
                                        GO
                                        /****** Object:  Table [dbo].[DatosCopade]    Script Date: 22/09/2017 08:52:16 a. m. ******/
                                        SET ANSI_NULLS ON
                                        GO
                                        SET QUOTED_IDENTIFIER ON
                                        GO
                                        DROP TABLE OrdenAgrupadaDetalle
                                        DROP TABLE OrdenAgrupada
                                        DROP TABLE DatosCopadeOrden
                                        DROP TABLE DatosCopade
                                        DROP TABLE OrdenGlobalAbono
                                        GO

                                        CREATE TABLE [dbo].[DatosCopade](
	                                        [idDatosCopade] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	                                        [subTotal] [decimal](18, 2) NOT NULL,
	                                        [total] [decimal](18, 2) NULL,
	                                        [moneda] [nvarchar](20) NULL,
	                                        [cantidad] [decimal](18, 4) NULL,
	                                        [descripcion] [nvarchar](250) NULL,
	                                        [importeConcepto] [decimal](18, 2) NULL,
	                                        [unidad] [nvarchar](50) NULL,
	                                        [valorUnitario] [decimal](18, 2) NULL,
	                                        [totalImpuestosRetenidos] [decimal](18, 2) NULL,
	                                        [totalImpuestosTrasladados] [decimal](18, 2) NULL,
	                                        [impuesto] [nvarchar](30) NULL,
	                                        [importeTraslado] [decimal](18, 2) NULL,
	                                        [tasa] [decimal](18, 2) NULL,
	                                        [contrato] [nvarchar](150) NULL,
	                                        [ordenSurtimiento] [nvarchar](50) NOT NULL,
	                                        [numeroEstimacion] [nvarchar](250) NOT NULL,
	                                        [numeroAcreedor] [nvarchar](200) NULL,
	                                        [gestor] [nvarchar](150) NULL,
	                                        [finiquito] [nvarchar](150) NULL,
	                                        [posicionap] [nvarchar](150) NULL,
	                                        [numeroCopade] [nvarchar](250) NOT NULL,
	                                        [ejercicio] [nvarchar](150) NULL,
	                                        [fechaCarga] [datetime] NOT NULL,
	                                        [fechaRecepcionCopade] [datetime] NULL,
	                                        [xmlCopade] [nvarchar](max) NULL,
                                         CONSTRAINT [PK_DatosCopade] PRIMARY KEY CLUSTERED 
                                        (
	                                        [idDatosCopade] ASC
                                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                                        ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

                                        GO
                                        /****** Object:  Table [dbo].[DatosCopadeOrden]    Script Date: 22/09/2017 08:52:17 a. m. ******/
                                        SET ANSI_NULLS ON
                                        GO
                                        SET QUOTED_IDENTIFIER ON
                                        GO



                                        CREATE TABLE [dbo].[DatosCopadeOrden](
	                                        [idDatosCopadeOrden] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	                                        [idDatosCopade] [numeric](18, 0) NULL,
	                                        [idOrden] [numeric](18, 0) NULL,
	                                        [fechaAsignacion] [datetime] NULL,
                                         CONSTRAINT [PK_DatosCopadeOrden] PRIMARY KEY CLUSTERED 
                                        (
	                                        [idDatosCopadeOrden] ASC
                                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                                        ) ON [PRIMARY]

                                        GO
                                        /****** Object:  Table [dbo].[OrdenAgrupada]    Script Date: 22/09/2017 08:52:17 a. m. ******/
                                        SET ANSI_NULLS ON
                                        GO
                                        SET QUOTED_IDENTIFIER ON
                                        GO


                                        CREATE TABLE [dbo].[OrdenAgrupada](
	                                        [idOrdenAgrupada] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	                                        [numero] [nvarchar](70) NOT NULL,
	                                        [fecha] [datetime] NOT NULL,
	                                        [estatus] [nvarchar](30) NOT NULL,
                                         CONSTRAINT [PK_OrdenAgrupada] PRIMARY KEY CLUSTERED 
                                        (
	                                        [idOrdenAgrupada] ASC
                                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                                        ) ON [PRIMARY]

                                        GO
                                        /****** Object:  Table [dbo].[OrdenAgrupadaDetalle]    Script Date: 22/09/2017 08:52:17 a. m. ******/
                                        SET ANSI_NULLS ON
                                        GO
                                        SET QUOTED_IDENTIFIER ON
                                        GO

                                        CREATE TABLE [dbo].[OrdenAgrupadaDetalle](
	                                        [idOrdenAgrupadaDetalle] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	                                        [idOrdenAgrupada] [numeric](18, 0) NOT NULL,
	                                        [idDatosCopadeOrden] [numeric](18, 0) NOT NULL,
	                                        [fechaAsignacion] [datetime] NULL,
                                         CONSTRAINT [PK_OrdenAgrupadaDetalle] PRIMARY KEY CLUSTERED 
                                        (
	                                        [idOrdenAgrupadaDetalle] ASC
                                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                                        ) ON [PRIMARY]

                                        GO

                                        SET ANSI_NULLS ON
                                        GO
                                        SET QUOTED_IDENTIFIER ON
                                        GO
                                        CREATE TABLE [dbo].[OrdenGlobalAbono](
	                                        [idOrdenGlobalAbono] [numeric](18, 0) IDENTITY(1,1)  NOT NULL,
	                                        [numeroOrdenGlobal] [varchar](50) NULL,
                                         CONSTRAINT [PK_OrdenGloabalAbono] PRIMARY KEY CLUSTERED 
                                        (
	                                        [idOrdenGlobalAbono] ASC
                                        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                                        ) ON [PRIMARY]

                                        GO

                                        SET IDENTITY_INSERT DatosCopade on 

                                        Insert Into DatosCopade (idDatosCopade, subTotal, total, moneda, cantidad, descripcion, importeConcepto, unidad, valorUnitario, totalImpuestosRetenidos, totalImpuestosTrasladados, impuesto, importeTraslado, tasa, contrato, 
                                                                 ordenSurtimiento, numeroEstimacion, numeroAcreedor, gestor, finiquito, posicionap, numeroCopade, ejercicio, fechaCarga, fechaRecepcionCopade, xmlCopade
                                        )
                                        SELECT [idDatosCopade]
                                              ,[subTotal]
                                              ,[total]
                                              ,[moneda]
                                              ,[cantidad]
                                              ,[descripcion]
                                              ,[importeConcepto]
                                              ,[unidad]
                                              ,[valorUnitario]
                                              ,[totalImpuestosRetenidos]
                                              ,[totalImpuestosTrasladados]
                                              ,[impuesto]
                                              ,[importeTraslado]
                                              ,[tasa]
                                              ,[contrato]
                                              ,[ordenSurtimiento]
                                              ,[numeroEstimacion]
                                              ,[numeroAcreedor]
                                              ,[gestor]
                                              ,[finiquito]
                                              ,[posicionap]
                                              ,[numeroCopade]
                                              ,[ejercicio]
                                              ,[fechaCarga]
                                              ,[fechaRecepcionCopade]
                                              ,[xmlCopade]
                                          FROM [talleres].[dbo].[DatosCopade]

                                        SET IDENTITY_INSERT DatosCopade off

                                        GO


                                        SET IDENTITY_INSERT DatosCopadeOrden on  
                                        INSERT INTO DatosCopadeOrden (idDatosCopadeOrden, idDatosCopade, idOrden, fechaAsignacion)
                                        SELECT [idDatosCopadeOrden]
                                              ,[idDatosCopade]
                                              ,[idTrabajo]
                                              ,[fechaAsignacion]
                                        FROM [talleres].[dbo].[DatosCopadeOrden]

                                        SET IDENTITY_INSERT DatosCopadeOrden off

                                        GO

                                        SET IDENTITY_INSERT OrdenAgrupada on  
                                        INSERT INTO OrdenAgrupada (idOrdenAgrupada, numero, fecha, estatus)
                                        SELECT [idTrabajoAgrupado]
                                              ,[numeroTrabajoAgrupado]
                                              ,[fecha]
                                              ,[estatus]
                                        FROM [talleres].[dbo].[TrabajoAgrupado]

                                        SET IDENTITY_INSERT OrdenAgrupada off

                                        GO

                                        SET IDENTITY_INSERT OrdenAgrupadaDetalle on  
                                        INSERT INTO OrdenAgrupadaDetalle (idOrdenAgrupadaDetalle, idOrdenAgrupada, idDatosCopadeOrden, fechaAsignacion)
                                        SELECT [idTrabajoAgrupadoDetalle]
                                              ,[idTrabajoAgrupado]
                                              ,[idDatosCopadeOrden]
                                              ,[fechaAsignacion]
                                          FROM [talleres].[dbo].[TrabajoAgrupadoDetalle]


                                        SET IDENTITY_INSERT OrdenAgrupadaDetalle off

                                        GO


                                        SET IDENTITY_INSERT OrdenGlobalAbono on  
                                        INSERT INTO OrdenGlobalAbono ( idOrdenGlobalAbono, numeroOrdenGlobal)
                                        SELECT [idOrdenGlobalAbono]
                                              ,[numeroOrdenGlobal]
                                        FROM [talleres].[dbo].[OrdenGlobalAbono]


                                        SET IDENTITY_INSERT OrdenGlobalAbono off

                                        GO

                                        Update DatosCopadeOrden
                                        set idOrden = t2.idOrdenesAseprot
                                        from DatosCopadeOrden t1, ASEPROTDesarrollo.[dbo].RelacionCitaOrdenes t2
                                        where t1.idOrden = t2.idTrabajoTalleres

                                        ALTER TABLE [dbo].[DatosCopadeOrden]  WITH CHECK ADD  CONSTRAINT [FK_DatosCopadeOrden_DatosCopade] FOREIGN KEY([idDatosCopade])
                                        REFERENCES [dbo].[DatosCopade] ([idDatosCopade])
                                        GO
                                        ALTER TABLE [dbo].[DatosCopadeOrden] CHECK CONSTRAINT [FK_DatosCopadeOrden_DatosCopade]
                                        GO
                                        --ALTER TABLE [dbo].[DatosCopadeOrden]  WITH CHECK ADD  CONSTRAINT [FK_DatosCopadeOrden_Ordenes] FOREIGN KEY([idOrden])
                                        --REFERENCES [dbo].[Ordenes] ([idOrden])
                                        --GO
                                        --ALTER TABLE [dbo].[DatosCopadeOrden] CHECK CONSTRAINT [FK_DatosCopadeOrden_Ordenes]
                                        --GO
                                        ALTER TABLE [dbo].[OrdenAgrupadaDetalle]  WITH CHECK ADD  CONSTRAINT [FK_OrdenAgrupadaDetalle_DatosCopadeOrden] FOREIGN KEY([idDatosCopadeOrden])
                                        REFERENCES [dbo].[DatosCopadeOrden] ([idDatosCopadeOrden])
                                        GO
                                        ALTER TABLE [dbo].[OrdenAgrupadaDetalle] CHECK CONSTRAINT [FK_OrdenAgrupadaDetalle_DatosCopadeOrden]
                                        GO
                                        ALTER TABLE [dbo].[OrdenAgrupadaDetalle]  WITH CHECK ADD  CONSTRAINT [FK_OrdenAgrupadaDetalle_OrdenAgrupada] FOREIGN KEY([idOrdenAgrupada])
                                        REFERENCES [dbo].[OrdenAgrupada] ([idOrdenAgrupada])
                                        GO
                                        ALTER TABLE [dbo].[OrdenAgrupadaDetalle] CHECK CONSTRAINT [FK_OrdenAgrupadaDetalle_OrdenAgrupada]
                                        GO
                                ", serConn);
                int res = cmd.ExecuteNonQuery();
                serConn.Close();

                string response = string.Empty;
                if (res > 0)
                    response = "Datos copade se insertaron correctamente.";
                LogWriter log = new LogWriter();
                log.WriteInLog(response);
                Console.WriteLine(response);
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
