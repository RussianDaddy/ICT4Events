using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.GebruikerBeheer
{
    class Admin : Gebruiker
    {
       
        public Admin (string gebruikersnaam, string naam, string wachtwoord, bool aanwezig, string rfid, bool admin)
            : base (gebruikersnaam, naam, wachtwoord, aanwezig, rfid, admin)
        {
           
        }
    }
}
