using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;



namespace ICT4Events.Database
{
    class Database
    {
        OracleConnection connection;
        String connectionString = "User Id=system;Password=P@ssw0rd;Data Source=//192.168.20.22/xe;";

        public void openConnection()
        {
            connection = new OracleConnection();
            connection.ConnectionString = connectionString;
            connection.Open();
        }

        public DataTable voerQueryUit(String query)
        {
            openConnection();
            OracleCommand command = new OracleCommand(query, connection);
            try
            {
                OracleDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                return dataTable;
            }
            catch (Exception)
            {
                MessageBox.Show("Query kan niet worden uitgevoerd. Controleer de verbinding met de database.");
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
            return null;
        }
    }
}
