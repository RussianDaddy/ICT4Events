using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.GebruikerBeheer
{
    class Gebruiker
    {
        string Gebruikersnaam { get; set; }
        string Naam { get; set; }
        string Wachtwoord { get; set; }
        bool Aanwezig { get; set; }
        bool Admin { get; set; }
        int RFID { get; set; }

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
