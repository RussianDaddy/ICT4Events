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
                string RFID;
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
                        RFID = Convert.ToString(Reader["RFID"]);
                    }
                    catch(InvalidCastException)
                    {
                        RFID = "";
                    }

                    if (AdminCheck == "1")
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
        public List<ReserveringBeheer.Reservering> GetReserveringen(string sql)
        {
            List<ReserveringBeheer.Reservering> Reserveringen = new List<ReserveringBeheer.Reservering>();
            try
            {
                openConnection();
                OracleCommand List = new OracleCommand(sql, connection);
                OracleDataReader Reader = List.ExecuteReader();
                OracleDataAdapter Adapter = new OracleDataAdapter(List);

                int Nummer;
                DateTime AankomstDatum;
                DateTime VertrekDatum;
                int Betaald;

                while(Reader.Read())
                {
                    Nummer = Convert.ToInt32(Reader["NUMMER"]);
                    AankomstDatum = Convert.ToDateTime(Reader["AANKOMSTDATUM"]);
                    VertrekDatum = Convert.ToDateTime(Reader["VERTREKDATUM"]);
                    Betaald = Convert.ToInt32(Reader["BETAALD"]);

                    if(Betaald == 0)
                    {
                        Reserveringen.Add(new ReserveringBeheer.Reservering(Nummer, AankomstDatum, VertrekDatum, false));
                    }
                    else
                    {
                        Reserveringen.Add(new ReserveringBeheer.Reservering(Nummer, AankomstDatum, VertrekDatum, true));
                    }
                }
            }
            catch(OracleException exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                connection.Close();
            }
            return Reserveringen;
        }

        public List<Mediabeheer.Mediafile> GetBerichtenList(string sql, List<Mediabeheer.Categorie> categorielijst)
        {
            List<Mediabeheer.Mediafile> BerichtenLijst = new List<Mediabeheer.Mediafile>();

            try
            {
                openConnection();
                OracleCommand List = new OracleCommand(sql, connection);
                OracleDataReader Reader = List.ExecuteReader();
                OracleDataAdapter Adapter = new OracleDataAdapter(List);

                int Id;
                string Naam;
                string Bericht;
                string Type;
                //string Categorie;
                string Path;
                int Like;
                int Report;
                string Gebruikersnaam;
                int CategorieID;

                while (Reader.Read())
                {
                    Id = Convert.ToInt32(Reader["ID"]);
                    Naam = Convert.ToString(Reader["Name"]);
                    Bericht = Convert.ToString(Reader["Bericht"]);
                    Type = Convert.ToString(Reader["Type"]);
                    //Categorie = Convert.ToString(Reader["Categorie"]);
                    Path = Convert.ToString(Reader["Path"]);
                    Like = Convert.ToInt32(Reader["VindIkLeuk"]);
                    Report = Convert.ToInt32(Reader["Report"]);
                    Gebruikersnaam = Convert.ToString(Reader["GebruikerGebruikersnaam"]);
                    CategorieID = Convert.ToInt32(Reader["CategorieID"]);

                    foreach (Mediabeheer.Categorie c in categorielijst)
                    {
                        if (CategorieID == c.Id)
                        {
                            BerichtenLijst.Add(new Mediabeheer.Mediafile(Id, Naam, Bericht, Type, c, Path, Like, Report, Gebruikersnaam));
                        }
                    }
                }
                return BerichtenLijst;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return BerichtenLijst;
        } 
        public List<Mediabeheer.Categorie> GetCategorieLijst(string sql, int check)
        {
            List<Mediabeheer.Categorie> CategorieLijst = new List<Mediabeheer.Categorie>();

            try
            {
                openConnection();
                OracleCommand List = new OracleCommand(sql, connection);
                OracleDataReader Reader = List.ExecuteReader();
                OracleDataAdapter Adapter = new OracleDataAdapter(List);

                int Id;
                string Naam;
                //int MediafileID;
                //string SuperCategorie;

                while (Reader.Read())
                {
                    Id = Convert.ToInt32(Reader["ID"]);
                    Naam = Convert.ToString(Reader["Naam"]);
                    //MediafileID = Convert.ToInt32(Reader["MediafileID"]);
                    //SuperCategorie = Convert.ToString(Reader["SuperCategorieID"]);
                    /*if (check == 0)
                    {*/
                    CategorieLijst.Add(new Mediabeheer.Categorie(Id, Naam));

                   // }
                    /*if(check == 1)
                    {
                        CategorieLijst.Add(new Mediabeheer.Categorie(Id, Naam));
                    }*/

                }
                return CategorieLijst;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return CategorieLijst;
        }

        public List<Mediabeheer.Reactie> GetReactieLijst(string sql)
        {
            List<Mediabeheer.Reactie> ReactieLijst = new List<Mediabeheer.Reactie>();

            try
            {
                openConnection();
                OracleCommand List = new OracleCommand(sql, connection);
                OracleDataReader Reader = List.ExecuteReader();
                OracleDataAdapter Adapter = new OracleDataAdapter(List);

                int Id;
                string Bericht;
                string Gebruikersnaam;
                int MediafileId;

                while (Reader.Read())
                {
                    Id = Convert.ToInt32(Reader["ID"]);
                    MediafileId = Convert.ToInt32(Reader["MediafileID"]);
                    Gebruikersnaam = Convert.ToString(Reader["GebruikerGebruikersnaam"]);
                    Bericht = Convert.ToString(Reader["Bericht"]);

                    ReactieLijst.Add(new Mediabeheer.Reactie(Id, Bericht, Gebruikersnaam, MediafileId));
                    
                }
                return ReactieLijst;
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return ReactieLijst;
        } 
    }
}
