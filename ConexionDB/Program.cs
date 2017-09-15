using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.CSharp;
using System.Data;

namespace ConexionDB
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection serConn = new SqlConnection(Constants.ASEPROTDesarrolloStringConn);

            SqlCommand ordCMD = serConn.CreateCommand();
            ordCMD.CommandText = "Select * from Ordenes";

            serConn.Open();
            DataTable t1 = new DataTable();
            using (SqlDataAdapter a = new SqlDataAdapter(ordCMD)) {
                a.Fill(t1);
            }
            serConn.Close();
            Console.ReadLine();
        }
    }
}
