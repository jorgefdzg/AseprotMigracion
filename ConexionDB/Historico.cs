using System;

namespace ConexionDB
{
    public class Historico
    {
        private int? _idHistorialEstatusOrden = null;
        private int? _idOrden = null;
        private int _idEstatusOrden = 0;
        private DateTime _fechaInicial = DateTime.MinValue;
        private DateTime? _fechaFinal = null;
        private int _idUsuario = 0;

        public int?      idHistorialEstatusOrden { get { return _idHistorialEstatusOrden; } set { _idHistorialEstatusOrden = value; } }
        public int?      idOrden                 { get { return _idOrden; } set { _idOrden = value; } }
        public int      idEstatusOrden          { get { return _idEstatusOrden; } set { _idEstatusOrden = value; } }
        public DateTime fechaInicial            { get { return _fechaInicial; } set { _fechaInicial = value; } }
        public DateTime? fechaFinal              { get { return _fechaFinal; } set { _fechaFinal = value; } }
        public int      idUsuario               { get { return _idUsuario; } set { _idUsuario = value; } }
    }
}