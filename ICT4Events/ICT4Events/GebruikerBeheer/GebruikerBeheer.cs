using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICT4Events.GebruikerBeheer
{
    //Deze klasse voert de actie tussen het tabblad beheren in het form en de database
    class GebruikerBeheer
    {
        Database.Database Database = new Database.Database();

        //De methode regelt het inloggen in het systeem. Het wachtwoord wordt opgezocht m.b.v. de gebruikersnaam en wordt vergeleken met het ingevoerde wachtwoord
        //Als het wachtwoord correct is wordt er gekeken of de gebruiker een gast of een admin is
        public string Inloggen(string gebruikersnaam, string wachtwoord)
        {
            string sqlGegevens = "SELECT * FROM GEBRUIKER WHERE GEBRUIKERSNAAM = '" + gebruikersnaam + "'";
            string returns = "";
            List<Gebruiker> Gebruiker = new List<Gebruiker>();
            Gebruiker = Database.GetGebruikerList(sqlGegevens);
            if(Gebruiker.Count != 0)
            {
                foreach(Gebruiker Temp in Gebruiker)
                {
                    if (Temp.Wachtwoord == wachtwoord)
                    {
                        if (Temp.Admin == true)
                        {
                            returns = "Admin";
                        }
                        else
                        {
                            returns = "Gast";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Wachtwoord is incorrect.");
                        returns = "Error";
                    }
                }
            }
            else
            {
                MessageBox.Show("Gebruikersnaam is incorrect");
                returns = "Error";
            }
            return returns;

        }

        //Met deze methode kunnen de gegevens van de gebruiker gewijzigd worden in de database
        public void Update(string email, string naam, string wachtwoord, string admin)
        {
            string sqlGebruiker = "";
            if(admin == "Ja")
            {
                sqlGebruiker = "UPDATE GEBRUIKER SET GEBRUIKERSNAAM = '" + email + "', NAAM = '" + naam + "', WACHTWOORD = '" + wachtwoord + "', ADMINCHECK = 1 WHERE GEBRUIKERSNAAM = '" + email + "'";
                string sqlAdmin = "UPDATE ADMIN SET GEBRUIKERGEBRUIKERSNAAM = '" + email + "' WHERE GEBRUIKERGEBRUIKERSNAAM = '" + email + "'";
                string sqlRemove = "DELETE FROM GAST WHERE GEBRUIKERGEBRUIKERSNAAM = '" + email + "'";
                string sqlInsert = "INSERT INTO ADMIN(GEBRUIKERGEBRUIKERSNAAM) VALUES('" + email + "')";
                if (Database.Insert(sqlGebruiker) == true)
                {
                    Database.Insert(sqlAdmin);
                    Database.Insert(sqlRemove);
                    Database.Insert(sqlInsert);
                    MessageBox.Show("Admin is aangepast.");
                }
            }
            else
            {
                sqlGebruiker = "UPDATE GEBRUIKER SET GEBRUIKERSNAAM = '" + email + "', NAAM = '" + naam + "', WACHTWOORD = '" + wachtwoord + "', ADMINCHECK = 0 WHERE GEBRUIKERSNAAM = '" + email + "'";
                string sqlGast = "UPDATE GAST SET GEBRUIKERGEBRUIKERSNAAM = '" + email + "' WHERE GEBRUIKERGEBRUIKERSNAAM = '" + email + "'";
                string sqlRemove = "DELETE FROM ADMIN WHERE GEBRUIKERGEBRUIKERSNAAM = '" + email + "'";
                string sqlInsert = "INSERT INTO GAST(GEBRUIKERGEBRUIKERSNAAM) VALUES('" + email + "')";
                if (Database.Insert(sqlGebruiker) == true)
                {
                    Database.Insert(sqlGast);
                    Database.Insert(sqlRemove);
                    Database.Insert(sqlInsert);
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
                else
                {
                    MessageBox.Show("Gebruiker bestaat al.");
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
                else
                {
                    MessageBox.Show("Gebruiker bestaat al.");
                }
            }
        }

        //Er wordt een lijst gemaakt met alle gebruiker die als aanwezig staan geregistreerd in de database
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

        //Aan de hand van het emailadres worden de gebruikergegevens opgevraagd uit de database
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

        //Aan de hand van het emailadres van de gebruiker wordt de reservering van de gebruiker opgevraagd
        public List<ReserveringBeheer.Reservering> GebruikerReservering(string email)
        {
            string sqlReservering = "SELECT * FROM RESERVERING WHERE GEBRUIKERGEBRUIKERSNAAM = '" + email + "'";
            List<ReserveringBeheer.Reservering> Reserveringen = new List<ReserveringBeheer.Reservering>();
            Reserveringen = Database.GetReserveringen(sqlReservering);
            return Reserveringen;
        }

        //De status van de betaling worden opgevraagd uit de database m.b.v. het emailadres en het reserveringnummer van de gebruiker
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

        //Deze methode wordt gebruikt als toegangsysteem. Aan de hand van het RFID van de gebruiker, wordt bij deze gebruiker de aanwezigheid gewijzigd
        public bool Aanwezig(string RFID)
        {
            bool Check = false;
            List<Gebruiker> gebruiker = new List<Gebruiker>();
            string sql = ("SELECT * FROM GEBRUIKER WHERE RFID = '" + RFID + "'");
            gebruiker = Database.GetGebruikerList(sql);
            if(gebruiker.Count == 0)
            {
                Check = false;
            }
            else
            {
                foreach(Gebruiker TempGebruiker in gebruiker)
                {
                    if (TempGebruiker.RFID == RFID)
                    {
                        if (TempGebruiker.Aanwezig == false)
                        {
                            string sqlUpdate = ("UPDATE GEBRUIKER SET AANWEZIG = 1 WHERE RFID = '" + RFID + "'");
                            Database.Insert(sqlUpdate);
                            Check = true;
                        }
                        else
                        {
                            string sqlUpdate = ("UPDATE GEBRUIKER SET AANWEZIG = 0 WHERE RFID = '" + RFID + "'");
                            Database.Insert(sqlUpdate);
                            Check = true;
                        }
                    }
                }
            }
            return Check;
        }
    }
}
