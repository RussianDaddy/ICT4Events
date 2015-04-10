using System;
using System.Collections.Generic;
using System.Data;

namespace ICT4Events.ReserveringBeheer
{
    class ReserveringBeheer
    {
        static Database.Database database = new Database.Database();

        public bool Reserveren(string username, DateTime date, int paid, int campnumber)
        {
            string query = "INSERT INTO RESERVERING(Nummer, Datum, Betaald, GastGebruikersnaam) VALUES(reservering_seq," +
                           date + "," + paid + "," + Convert.ToString(campnumber) + ",)";
            throw new NotImplementedException();
        }

        public static List<string> AllePlaatsen()
        {
            string query = "SELECT * FROM KAMPEERPLAATS";
            DataTable kampeerplaatsen = database.voerQueryUit(query);
            List<String> stringlist= new List<string>();
            foreach (DataRow dr in kampeerplaatsen.Rows)
            {
                stringlist.Add("Nummer: " + dr[0] + " Soort: " + dr[1] + " Aantal Personen: " + dr[2]);
            }
            return stringlist;
        }

        public static List<string> AlleGebruikers()
        {
            string query = "SELECT g.gebruikersnaam, g.naam FROM GEBRUIKER g";
            DataTable gebruikers = database.voerQueryUit(query);
            List<String> stringlist = new List<string>();
            foreach (DataRow dr in gebruikers.Rows)
            {
                stringlist.Add("Gebruikersnaam: " + dr[0] + " Naam: " + dr[1]);
            }
            return stringlist;
        }

        public void VrijePlaatsen()
        {
            throw new NotImplementedException();
        }
    }
}
