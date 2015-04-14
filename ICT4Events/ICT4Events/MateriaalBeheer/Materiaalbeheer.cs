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

        public static bool MateriaalHuren(int id, DateTime uitleendatum, DateTime retourdatum, Gebruiker gebruiker)
        {
            try
            {
                retourdatum = uitleendatum.AddDays(3);
                string query =
                    "INSERT INTO Uitlening (ID, Uitleendatum, Retourdatum, Gebruikersnaam) VALUES(" + id + ",'" +
                    uitleendatum.ToShortDateString() + "','" + retourdatum.ToShortDateString() + "','" + gebruiker.GetGebruikersnaam() + "')";
                database.Insert(query);
                return true;
            }

            catch (Exception)
            {
                return false;
            }
            //Uitlening uitlening = new Uitlening(id, uitleenDatum, retourDatum, exemplaar, gebruiker);
        }

        public void UitgevenRFID(Gebruiker gebruiker, int rfid)
        {

        }

        public static List<string> AlleExemplaren()
        {
            string query = "SELECT e.ID, Opmerkingen, m.Borg, m.Soort FROM Exemplaar e, Materiaal m WHERE m.ID = e.MateriaalID";
            DataTable exemplaren = database.voerQueryUit(query);
            List<string> stringList = new List<string>();
            foreach (DataRow dr in exemplaren.Rows)
            {
                stringList.Add("ID: " + dr[0] + " - " + dr[1] + " Borg: " + dr[2] + " Soort: " + dr[3]);
            }
            return stringList;
        }

        public static List<string> ZoekMateriaal(string id)
        {
            try
            {
                string query =
                    "SELECT e.ID, Opmerkingen, m.Borg, m.Soort FROM Exemplaar e, Materiaal m WHERE m.ID = e.MateriaalID AND m.ID = " +
                    id;
                DataTable materiaalZoeken = database.voerQueryUit(query);
                List<string> stringList = new List<string>();
                foreach (DataRow dr in materiaalZoeken.Rows)
                {
                    stringList.Add("ID: " + dr[0] + " - " + dr[1] + " Borg: " + dr[2] + " Soort: " + dr[3]);
                }
                return stringList;
            }
            catch (Exception)
            {
                MessageBox.Show("Materiaal ID kon niet gevonden worden.");
                return null;
            }
        }
    }
}
