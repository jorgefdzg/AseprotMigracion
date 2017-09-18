using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConexionDB
{
    public static class Constants
    {
        public const string ASEPROTDesarrolloStringConn = "Server = 192.168.20.9; Database = ASEPROTDesarrollo; User Id = sa; Password = S0p0rt3;";
        public const string ASEPROTStringConn = "Server = 192.168.20.18; Database = ASEPROT; User Id = sa; Password = S0p0rt3;";
        public const string ASEPROTPruebasStringConn = "Server = 192.168.20.18; Database = ASEPROTPruebas; User Id = sa; Password = S0p0rt3;";
        public const string PartidasStringConn = "Server = 192.168.20.18; Database = Partidas; User Id = sa; Password = S0p0rt3;";
        public const string TalleresStringConn = "Server = 192.168.20.18; Database = talleres; User Id = sa; Password = S0p0rt3;";
        //public static readonly string[] conexiones = { "ASEPROTDesarrollo", "ASEPROT", "ASEPROTPruebas", "Partidas", "Talleres" };
        public enum conexiones { ASEPROTDesarrollo = 1, ASEPROT, ASEPROTPruebas, Partidas, Talleres};

    }
}
