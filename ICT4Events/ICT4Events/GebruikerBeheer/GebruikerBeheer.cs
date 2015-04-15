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

        public void Update(string email, string naam, string wachtwoord, string admin)
        {
            string sqlGebruiker = "UPDATE GEBRUIKER SET GEBRUIKERSNAAM = '" + email + "', NAAM = '" + naam + "', WACHTWOORD = '" + wachtwoord + "', ADMINCHECK = '" + admin + "' WHERE GEBRUIKERSNAAM = '" + email + "'";
            if(admin == "Ja")
            {
                string sqlAdmin = "UPDATE ADMIN SET GEBRUIKERGEBRUIKERSNAAM = '" + email + "' WHERE GEBRUIKERGEBRUIKERSNAAM = '" + email + "'";
                if (Database.Insert(sqlGebruiker) == true)
                {
                    Database.Insert(sqlAdmin);
                    MessageBox.Show("Admin is aangepast.");
                }
            }
            else
            {
                string sqlGast = "UPDATE GAST SET GEBRUIKERGEBRUIKERSNAAM = '" + email + "' WHERE GEBRUIKERGEBRUIKERSNAAM = '" + email + "'";
                if (Database.Insert(sqlGebruiker) == true)
                {
                    Database.Insert(sqlGast);
                    MessageBox.Show("Gast is aangepast.");
                }
            }
        }
        
        //Hier wordt een gebruiker geregistreerd.
        //Zijn gegegevens worden toegevoegd aan de Database.
        public void Registreren(string email, string naam, string wachtwoord, int aanwezig, string admin)
        {
            string sqlGebruiker = "INSERT INTO GEBRUIKER(Gebruikersnaam, Naam, Wachtwoord,  Aanwezig, AdminCheck) VALUES('" + email + "','" + naam + "','" + wachtwoord + "','" + Convert.ToString(aanwezig) + "','" + admin + "')";
            if (admin == "Ja")
            {  
                string sqlAdmin = "INSERT INTO ADMIN(Gebruikergebruikersnaam) VALUES('" + email + "')";
                if (Database.Insert(sqlGebruiker) == true)
                {
                    Database.Insert(sqlAdmin);
                    MessageBox.Show("Admin is toegevoegd.");
                }
            }
            else
            {
                string sqlGast = "INSERT INTO GAST(Gebruikergebruikersnaam) VALUES('" + email + "')";
                if (Database.Insert(sqlGebruiker) == true)
                {
                    Database.Insert(sqlGast);
                    MessageBox.Show("Gast is toegevoegd.");
                }
            }
        }

        public List<Gebruiker> LijstAanwezigen(bool aanwezig)
        {
            string sql;
            if(aanwezig == true)
            {
                sql = "SELECT * FROM GEBRUIKER WHERE AANWEZIG = 1";
            }
            else
            {
                sql = "SELECT * FROM GEBRUIKER WHERE AANWEZIG = 0";
            }
            List<Gebruiker> Gebruikers = new List<Gebruiker>();
            Gebruikers = Database.GetGebruikerList(sql);
            return Gebruikers;
        }

        public Gebruiker GebruikerOpvragen(string email)
        {
            string sql = "SELECT * FROM GEBRUIKER WHERE GEBRUIKERSNAAM = '" + email + "'";
            List<Gebruiker> Gebruikers = new List<Gebruiker>();
            Gebruiker Eengebruiker;
            Gebruikers = Database.GetGebruikerList(sql);
            foreach(Gebruiker Temp in Gebruikers)
            {
                Eengebruiker = Temp;
                return Eengebruiker;
            }
            return null;
        }

        public List<ReserveringBeheer.Reservering> GebruikerReservering(string email)
        {
            string sqlReservering = "SELECT * FROM RESERVERING WHERE GEBRUIKERGEBRUIKERSNAAM = '" + email + "'";
            List<ReserveringBeheer.Reservering> Reserveringen = new List<ReserveringBeheer.Reservering>();
            Reserveringen = Database.GetReserveringen(sqlReservering);
            return Reserveringen;
        }

        public bool Betaalstatus(string Email, int ReserveringNummer)
        {
            string sqlBetaalstatus = "SELECT * FROM RESERVERING WHERE GEBRUIKERGEBRUIKERSNAAM = '" + Email + "' AND NUMMER = " + ReserveringNummer;
            bool ReturnStatus = false;
            List<ReserveringBeheer.Reservering> Reservering = new List<ReserveringBeheer.Reservering>();
            Reservering = Database.GetReserveringen(sqlBetaalstatus);
            foreach(ReserveringBeheer.Reservering Temp in Reservering)
            {
                if(Temp.Betaald == false)
                {
                    ReturnStatus = false;
                }
                else
                {
                    ReturnStatus = true;
                }
            }
            return ReturnStatus;
        }
    }
}
