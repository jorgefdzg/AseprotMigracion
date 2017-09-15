using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    public class Ordenes
    {
        public decimal idOrden { get; set; }
        public DateTime fechaCreacionOden { get; set; }
        public DateTime fechaCita { get; set; }
        public DateTime fechaInicioTrabajo { get; set; }
        public string numeroOrden { get; set; }
        public int consecutivoOrden { get; set; }
        public string comentarioOrden { get; set; }
        public bool requiereGrua { get; set; }
        public int idCatalogoEstadoUnidad { get; set; }
        public decimal idZona { get; set; }
        public decimal idUnidad { get; set; }
        public int idContratoOperacion { get; set; }
        public decimal idUsuario { get; set; }
        public int idCatalogoTipoOrdenServicio { get; set; }
        public int idTipoOrden { get; set; }
        public int idEstatusOrden { get; set; }
        public decimal idCentroTrabajo { get; set; }
        public decimal idTaller { get; set; }
        public decimal idGarantia { get; set; }
        public string motivoGarantia { get; set; }

            public Ordenes(decimal _idOrden, DateTime _fechaCreacionOden, DateTime _fechaCita, DateTime _fechaInicioTrabajo, string _numeroOrden, int _consecutivoOrden, string _comentarioOrden, bool _requiereGrua, int _idCatalogoEstadoUnidad, decimal _idZona, decimal _idUnidad, int _idContratoOperacion, decimal _idUsuario, int _idCatalogoTipoOrdenServicio, int _idTipoOrden, int _idEstatusOrden, decimal _idCentroTrabajo, decimal _idTaller, decimal _idGarantia, string _motivoGarantia) {
            idOrden = _idOrden;
            fechaCreacionOden = _fechaCreacionOden;
            fechaCita = _fechaCita;
            fechaInicioTrabajo = _fechaInicioTrabajo;
            numeroOrden = _numeroOrden;
            consecutivoOrden = _consecutivoOrden;
            comentarioOrden = _comentarioOrden;
            requiereGrua = _requiereGrua;
            idCatalogoEstadoUnidad = _idCatalogoEstadoUnidad;
            idZona = _idZona;
            idUnidad = _idUnidad;
            idContratoOperacion = _idContratoOperacion;
            idUsuario = _idUsuario;
            idCatalogoTipoOrdenServicio = _idCatalogoTipoOrdenServicio;
            idTipoOrden = _idTipoOrden;
            idEstatusOrden = _idEstatusOrden;
            idCentroTrabajo = _idCentroTrabajo;
            idTaller = _idTaller;
            idGarantia = idGarantia;
            motivoGarantia = _motivoGarantia;

        }

        //public static bool InsertInTable(Ordenes orden) {
                
        //}
    }
}
