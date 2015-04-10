using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events
{
    abstract class Gebruiker
    {
        string Gebruikersnaam { get; set; }
        string Naam { get; set; }
        string Wachtwoord { get; set; }
        bool Aanwezig { get; set; }
        int RFID { get; set; }

        public Gebruiker(string gebruikersnaam, string naam, string wachtwoord, bool aanwezig, int rfid)
        {
            this.Gebruikersnaam = gebruikersnaam;
            this.Naam = naam;
            this.Wachtwoord = wachtwoord;
            this.Aanwezig = aanwezig;
            this.RFID = rfid;
        }
    }
}
