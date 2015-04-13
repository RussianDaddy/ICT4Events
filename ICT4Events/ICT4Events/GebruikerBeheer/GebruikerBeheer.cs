using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICT4Events.GebruikerBeheer
{
    class GebruikerBeheer
    {
        Database.Database Database = new Database.Database();
        public void Inloggen(string gebruikersnaam, string wachtwoord)
        {

        }
        
        //Hier wordt een gebruiker geregistreerd.
        //Zijn gegegevens worden toegevoegd aan de Database.
        public void Registreren(string email, string naam, string wachtwoord, int aanwezig, string admin)
        {
            string sql = "INSERT INTO GEBRUIKER(Gebruikersnaam, Naam, Wachtwoord,  Aanwezig, AdminCheck) VALUES('"+ email +"','" + naam + "','" + wachtwoord + "','" + Convert.ToString(aanwezig) + "','" + Convert.ToString(admin) + "')";
            if (Database.Insert(sql) == true)
            {
                MessageBox.Show("Gebruiker is toegevoegd.");
            }
        }
    }
}
