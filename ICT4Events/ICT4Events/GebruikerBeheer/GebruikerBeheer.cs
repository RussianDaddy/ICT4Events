using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.GebruikerBeheer
{
    class GebruikerBeheer
    {
        Database.Database Database = new Database.Database();
        public void Inloggen(string gebruikersnaam, string wachtwoord)
        {

        }

        public void Registreren(string email, string naam, string wachtwoord, int aanwezig, string admin)
        {
            string sql = "INSERT INTO GEBRUIKER(Gebruikersnaam, Naam, Wachtwoord,  Aanwezig, AdminCheck) VALUES('"+ email +"','" + naam + "','" + wachtwoord + "','" + Convert.ToString(aanwezig) + "','" + Convert.ToString(admin) + "')";
            Database.Insert(sql);
        }

    }
}
