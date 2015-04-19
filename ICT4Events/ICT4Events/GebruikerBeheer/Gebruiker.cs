using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.GebruikerBeheer
{
    //Met de klasse worden gebruikers aangemaakt in het systeem
    class Gebruiker
    {
        public string Gebruikersnaam { get; set; }
        public string Naam { get; set; }
        public string Wachtwoord { get; set; }
        public bool Aanwezig { get; set; }
        public bool Admin { get; set; }
        public string RFID { get; set; }

        //De constructor
        public Gebruiker(string gebruikersnaam, string naam, string wachtwoord, bool aanwezig, string rfid, bool admin)
        {
            this.Gebruikersnaam = gebruikersnaam;
            this.Naam = naam;
            this.Wachtwoord = wachtwoord;
            this.Aanwezig = aanwezig;
            this.RFID = rfid;
            this.Admin = admin;
        }

        //Getter van de gebruikersnaam
        public string GetGebruikersnaam()
        {
            return Gebruikersnaam;
        }

        public override string ToString()
        {
            return Gebruikersnaam + ", " + Naam + ", " + Wachtwoord + ", " + Convert.ToString(Aanwezig) + ", " + Convert.ToString(Admin) + ", " + RFID;
        }
    }
}
