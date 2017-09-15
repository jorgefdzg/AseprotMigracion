using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    public class Partidas
    {
        public decimal idPartida { get; set; }
        public string numeroParte { get; set; }
        public string descripcion { get; set; }
        public decimal precio { get; set; }
        public decimal idTaller { get; set; }

        public Partidas(decimal _idPartida, string _numeroParte, string _descripcion, decimal _precio, decimal _idTaller) {
            idPartida = _idPartida;
            numeroParte = _numeroParte;
            descripcion = _descripcion;
            precio = _precio;
            idTaller = _idTaller;
        }
    }
}
