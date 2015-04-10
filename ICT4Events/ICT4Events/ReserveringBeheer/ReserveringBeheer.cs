using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace ICT4Events.ReserveringBeheer
{
    class ReserveringBeheer
    {
        private OracleConnection connection;
        private String connectionString = "User Id=system;Password=P@ssw0rd;Data Source=//192.168.20.16/xe;";

        public bool Reserveren(int number, DateTime date, bool paid, int campnumber)
        {
            throw new NotImplementedException();
        }

        public DataTable AllePlaatsen()
        {
            string query = "SELECT * FROM KAMPEERPLAATS";
            connection = new OracleConnection(connectionString);
            OracleCommand command = new OracleCommand(query, connection);
            connection.Open();
            try
            {
                OracleDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                return dataTable;
            }
            catch (Exception)
            {
                MessageBox.Show("Query mislukt");
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
            return null;
        }

        public void VrijePlaatsen()
        {
            throw new NotImplementedException();
        }
    }
}
