using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaUPS
{
    class ConexionSQL
    {
        private SqlConnection conexion;

        public void Conectar()
        {
            try
            {
                conexion = new SqlConnection("Data Source=WIN-PS6SN3G9U9I;Initial Catalog=UPS_GIRON;Persist Security Info=True;User ID=sa;Password=SIG.root");
                conexion.Open();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public SqlConnection getConnection()
        {
            return conexion;
        }

        public void Desconectar()
        {
            conexion.Close();
        }
    }
}
