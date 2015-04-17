using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICT4Events.GebruikerBeheer;

namespace ICT4Events.MateriaalBeheer
{
    internal class Materiaalbeheer
    {
        //private Gebruiker Harold = new Gast("RussianDaddy", "Harold", "Egelhoorntje96", false, 1, false);

        private static Database.Database database = new Database.Database();



        public static bool MateriaalHuren(int id, DateTime uitleendatum, DateTime retourdatum, string gebruikersnaam)
        {
            try
            {
                string query =
                    "INSERT INTO UITLENING (ID, Uitleendatum, Retourdatum, Gebruikersnaam) VALUES(" + id + ",TO_DATE('" +
                    uitleendatum.ToShortDateString() + "','DD/MM/YYYY')," + "TO_DATE('" +
                    retourdatum.ToShortDateString() + "','DD/MM/YYYY'),'" + gebruikersnaam + "')";
                database.Insert(query);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UitgevenRFID(string gebruikersnaam, string rfid)
        {
            bool Check = false;
            List<Gebruiker> Gebruikers = new List<Gebruiker>();
            string sqlGebruiker = "SELECT * FROM GEBRUIKER WHERE GEBRUIKERSNAAM = '" + gebruikersnaam + "'";
            Gebruikers = database.GetGebruikerList(sqlGebruiker);
            try
            {  
                foreach (Gebruiker TempGebruiker in Gebruikers)
                {
                    if (TempGebruiker.RFID != "null")
                    {
                        string queryUpdate = "UPDATE Gebruiker SET RFID = '" + rfid + "' WHERE gebruikersnaam = '" + gebruikersnaam + "'";
                        database.Insert(queryUpdate);
                        Check = true;
                    }
                    else
                    {
                        Check = false;
                    }
                }

            }
            catch (Exception)
            {
                Check = false;
            }
            return Check;
        }

        public List<string> AlleExemplaren()
        {
            string query =
                "SELECT e.ID, m.Borg, m.Soort, Opmerkingen FROM Exemplaar e, Materiaal m WHERE m.ID = e.MateriaalID";
            DataTable exemplaren = database.voerQueryUit(query);
            List<string> stringList = new List<string>();
            foreach (DataRow dr in exemplaren.Rows)
            {
                stringList.Add("ID: " + dr[0] + " - Borg: " + dr[1] + " - Soort:" + dr[2] + " -  " + dr[3]);
            }
            return stringList;
        }

        public static List<string> ZoekMateriaal(string id)
        {
            try
            {
                string query =
                    "SELECT e.ID, m.Borg, m.Soort, Opmerkingen FROM Exemplaar e, Materiaal m WHERE m.ID = e.MateriaalID AND m.ID = " +
                    id;
                DataTable materiaalZoeken = database.voerQueryUit(query);
                List<string> stringList = new List<string>();
                foreach (DataRow dr in materiaalZoeken.Rows)
                {
                    stringList.Add("ID: " + dr[0] + " - Borg: " + dr[1] + " - Soort:" + dr[2] + " -  " + dr[3]);
                }
                return stringList;
            }
            catch (Exception)
            {
                MessageBox.Show("Materiaal ID kon niet gevonden worden.");
                return null;
            }
        }

        public bool UpdateUitleningId(int exemplaarId, int uitleningId)
        {
            try
            {
                string queryUpdate = "UPDATE Exemplaar SET UitleningId = " + uitleningId + " WHERE ID = " + exemplaarId;
                database.Insert(queryUpdate);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<string> AlleGasten()
        {
            string query = "SELECT gebruikersnaam, naam FROM Gebruiker";
            DataTable gekozenGebruikersnaam = database.voerQueryUit(query);
            List<string> stringList = new List<string>();
            foreach (DataRow dr in gekozenGebruikersnaam.Rows)
            {
                stringList.Add(dr[0] + " - Naam: " + dr[1]);
            }
            return stringList;
        }

        public static List<string> AlleUitleningen()
        {
            string query = "SELECT ID FROM Uitlening";
            DataTable uitlening = database.voerQueryUit(query);
            List<string> stringList = new List<string>();
            foreach (DataRow dr in uitlening.Rows)
            {
                stringList.Add(dr[0] + "");
            }
            return stringList;
        }
    }
}