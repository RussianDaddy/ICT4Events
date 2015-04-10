using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.GebruikerBeheer
{
    class Gast : Gebruiker
    {
        public Gast(string gebruikersnaam, string naam, string wachtwoord, bool aanwezig, int rfid) 
            : base(gebruikersnaam, naam, wachtwoord, aanwezig, rfid)
        {

        }
    }
}
