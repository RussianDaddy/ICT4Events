using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.GebruikerBeheer
{
    class Gebruiker
    {
        public string Gebruikersnaam { get; set; }
        public string Naam { get; set; }
        public string Wachtwoord { get; set; }
        public bool Aanwezig { get; set; }
        public bool Admin { get; set; }
        public int RFID { get; set; }

        public Gebruiker(string gebruikersnaam, string naam, string wachtwoord, bool aanwezig, int rfid, bool admin)
        {
            this.Gebruikersnaam = gebruikersnaam;
            this.Naam = naam;
            this.Wachtwoord = wachtwoord;
            this.Aanwezig = aanwezig;
            this.RFID = rfid;
            this.Admin = admin;
        }

        public string GetGebruikersnaam()
        {
            return Gebruikersnaam;
        }

        public override string ToString()
        {
            return Gebruikersnaam + ", " + Naam + ", " + Wachtwoord + ", " + Convert.ToString(Aanwezig) + ", " + Convert.ToString(Admin) + ", " + Convert.ToString(RFID);
        }
    }
}
