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
        public decimal? idPreorden { get; set; }

    }
}
