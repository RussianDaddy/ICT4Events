using System;
using System.Collections.Generic;
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
        public List<Exemplaar> Exemplaren;
        public Materiaalbeheer()
        {
            Exemplaren = new List<Exemplaar>();
            
        }

        public static bool MateriaalHuren(int id, DateTime uitleendatum, DateTime retourdatum, Gebruiker gebruiker)
        {
            try
            {
                retourdatum = uitleendatum.AddDays(3);
                string query = 
                    "INSERT INTO Uitlening (ID, Uitleendatum, Retourdatum, Gebruikersnaam) VALUES(" + id + "," + uitleendatum.ToShortDateString() + "," + retourdatum.ToShortDateString() + "," + gebruiker.
                    
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
    }
}
