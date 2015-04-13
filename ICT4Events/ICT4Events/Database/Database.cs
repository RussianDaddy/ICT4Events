using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;



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

        public bool Insert(string sql)
        {
            try
            {
                openConnection();
                OracleDataAdapter DataAdapter = new OracleDataAdapter(sql, connection);
                DataSet Data = new DataSet();
                DataAdapter.Fill(Data);
                return true;
            }
            catch (OracleException exc)
            {
                MessageBox.Show("Deze gebruiker bestaat al.");
                return false;
            }
            finally
            {
                connection.Close();
            }          
        }

        public List<string> GetStringList(string sql)
        {
            OracleCommand List = new OracleCommand(sql, connection);
            List<string> strings = new List<string>();

            try
            {
                openConnection();
                OracleDataReader stringReader = List.ExecuteReader();

                string Record;

                while (stringReader.Read())
                {
                    Record = stringReader.ToString();
                    strings.Add(Record);
                }
            }
            catch
            {

            }
            return strings;
        }

        public List<object> GetObjectList(string sql)
        {
            return null;
        }

        public List<int> GetIntList(string sql)
        {
            return null;
        }
    }
}
