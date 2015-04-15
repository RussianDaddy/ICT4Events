using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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
            catch (OracleException exc)
            {
                MessageBox.Show(exc.Message);
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
            catch (OracleException)
            {
                MessageBox.Show("Deze gebruiker bestaat al.");
                return false;
            }
            finally
            {
                connection.Close();
            }          
        }

        public bool Update(string selectSql, string updateSql)
        {
            try
            {
                openConnection();
                OracleDataAdapter DataAdapter = new OracleDataAdapter(selectSql, connection);
                DataAdapter.UpdateCommand = new OracleCommand(updateSql, connection);
                DataSet Data = new DataSet();
                DataAdapter.Fill(Data);
                DataAdapter.Update(Data);
                return true;
            }
            catch (OracleException exc)
            {
                MessageBox.Show(exc.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public List<GebruikerBeheer.Gebruiker> GetGebruikerList(string sql)
        {
            List<GebruikerBeheer.Gebruiker> Gebruiker = new List<GebruikerBeheer.Gebruiker>();

            try
            {
                openConnection();
                OracleCommand List = new OracleCommand(sql, connection);
                OracleDataReader Reader = List.ExecuteReader();
                OracleDataAdapter Adapter = new OracleDataAdapter(List);

                string Gebruikersnaam;
                string Naam;
                string Wachtwoord;
                int Aanwezig;
                int RFID;
                string AdminCheck;

                while (Reader.Read())
                {
                    Gebruikersnaam = Convert.ToString(Reader["GEBRUIKERSNAAM"]);
                    Naam = Convert.ToString(Reader["NAAM"]);
                    Wachtwoord = Convert.ToString(Reader["WACHTWOORD"]);
                    AdminCheck = Convert.ToString(Reader["ADMINCHECK"]);
                    Aanwezig = Convert.ToInt32(Reader["AANWEZIG"]);
                    try
                    {
                        RFID = Convert.ToInt32(Reader["RFID nummer"]);
                    }
                    catch(InvalidCastException)
                    {
                        RFID = 00000;
                    }

                    if (AdminCheck == "Ja")
                    {
                        if (Aanwezig == 0)
                        {
                            Gebruiker.Add(new GebruikerBeheer.Admin(Gebruikersnaam, Naam, Wachtwoord, false, RFID, true));
                        }
                        else
                        {
                            Gebruiker.Add(new GebruikerBeheer.Admin(Gebruikersnaam, Naam, Wachtwoord, true, RFID, true));
                        }
                    }
                    else
                    {
                        if (Aanwezig == 0)
                        {
                            Gebruiker.Add(new GebruikerBeheer.Gast(Gebruikersnaam, Naam, Wachtwoord, false, RFID, false));
                        }
                        else
                        {
                            Gebruiker.Add(new GebruikerBeheer.Gast(Gebruikersnaam, Naam, Wachtwoord, true, RFID, false));
                        }
                    }
                }
                return Gebruiker;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return Gebruiker;
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
