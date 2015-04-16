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
    class Materiaalbeheer
    {
    //private Gebruiker Harold = new Gast("RussianDaddy", "Harold", "Egelhoorntje96", false, 1, false);
        static Database.Database database = new Database.Database();

        public static bool MateriaalHuren(int id, DateTime uitleendatum, DateTime retourdatum, string gebruikersnaam)
        {
            try
            {
                string query =
                    "INSERT INTO UITLENING (ID, Uitleendatum, Retourdatum, Gebruikersnaam) VALUES(" + id + ",TO_DATE('" +
                    uitleendatum.ToShortDateString() + "','DD/MM/YYYY')," + "TO_DATE('" + retourdatum.ToShortDateString() + "','DD/MM/YYYY'),'" + gebruikersnaam + "')";
                database.Insert(query);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void UitgevenRFID(Gebruiker gebruiker, int rfid)
        {

        }

        public List<string> AlleExemplaren()
        {
            string query = "SELECT e.ID, m.Borg, m.Soort, Opmerkingen FROM Exemplaar e, Materiaal m WHERE m.ID = e.MateriaalID";
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

        public List<string> AlleGasten()
        {
            string query = "SELECT gebruikersnaam, naam FROM Gebruiker WHERE admincheck = 'Nee'";
            DataTable gekozenGebruikersnaam = database.voerQueryUit(query);
            List<string> stringList = new List<string>();
            foreach (DataRow dr in gekozenGebruikersnaam.Rows)
            {
                stringList.Add(dr[0] + " - Naam: " + dr[1]);
            }
            return stringList;
        }
    }
}
