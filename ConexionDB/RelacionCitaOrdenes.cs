using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    public class RelacionCitaOrdenes
    {
        public int idRelacionCitaOrdenes { get; set; }
        public int idCitaTalleres { get; set; }
        public int idTrabajoTalleres { get; set; }
        public int idOrdenAseprot { get; set; }
    }
}
