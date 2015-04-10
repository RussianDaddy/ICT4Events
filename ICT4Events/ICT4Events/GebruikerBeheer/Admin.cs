using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.GebruikerBeheer
{
    class Admin : Gebruiker
    {
        bool Admin { get; set; }
        public Admin (string gebruikersnaam, string naam, string wachtwoord, bool aanwezig, int rfid, bool admin)
            : base (gebruikersnaam, naam, wachtwoord, aanwezig, rfid)
        {
            this.Admin = admin;
        }
    }
}
