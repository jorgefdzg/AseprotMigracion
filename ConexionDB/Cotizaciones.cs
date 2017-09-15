using System;
using System.Collections.Generic;
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
        public decimal idPreorden { get; set; }

        public Cotizaciones(decimal _idCotizacion, DateTime _fechaCotizacion, decimal _idTaller, decimal _idUsuario, int _idEstatusCotizacion, decimal _idOrden, string _numeroCotizacion, int _consecutivoCotizacion, int _idCatalogoTipoOrdenServicio, int _idPreorden) {
            idCotizacion = _idCotizacion;
            fechaCotizacion = _fechaCotizacion;
            idTaller = _idTaller;
            idUsuario = _idUsuario;
            idEstatusCotizacion = _idEstatusCotizacion;
            idOrden = _idOrden;
            numeroCotizacion = _numeroCotizacion;
            consecutivoCotizacion = _consecutivoCotizacion;
            idCatalogoTipoOrdenServicio = _idCatalogoTipoOrdenServicio;
            idPreorden = _idPreorden;
        }

    }
}
