using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace ICT4Events.ReserveringBeheer
{
    /// <summary>
    /// In deze klasse vindt de communicatie plaats tussen de tab Reserveren en de database. In de klasse worden de gegevens opgevraagd en toegevoegd aan de database
    /// </summary>
    class ReserveringBeheer
    {
        static Database.Database database = new Database.Database();

        /// <summary>
        /// Er wordt m.b.v. een inert query een nieuwe reservering aan de database toegevoegd
        /// </summary>
        /// <param name="gebruikersnaam"> Dit is de gebruikersnaam waarop de reservering komt te staan </param>
        /// <param name="aankomstDatum"> Dit is de datum waarop de persoon/personen aankomen op het terrein</param>
        /// <param name="vertrekDatum"> Dit is de datum waarop de persoon/personen vertrekken van het terrein</param>
        /// <param name="betaald"> Aan de hand van dit nummer (0 of 1) wordt in de database opgeslagen of de reservering al betaald is </param>
        /// <returns> Een bool om te controleren dat het toevoegen daadwerkelijk is gelukt </returns>
        public bool Reserveren(string gebruikersnaam, DateTime aankomstDatum, DateTime vertrekDatum,  int betaald)
        {
            try
            {
                string query =
                    "INSERT INTO RESERVERING (Nummer, Aankomstdatum, Vertrekdatum, Betaald, Gebruikergebruikersnaam) VALUES(SEQ_RESERVERING.NEXTVAL, TO_DATE('" +
                    aankomstDatum + "','DD/MM/YYYY HH24:MI:SS'),TO_DATE('" + vertrekDatum +
                    "','DD/MM/YYYY HH24:MI:SS'),'" + betaald + "','" + gebruikersnaam + "')";
                database.Insert(query);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Aan de hand van de gebruikersnama, aankomstdatum en vertrekdatum wordt het reserveringsnummer gezocht.
        /// Met dit reserveringsnummer wordt de kampeerplaats gekoppeld door deze gegevens in de database op te slaan.
        /// </summary>
        /// <param name="gebruikersnaam"> Dit is de gebruikersnaam waarop de reservering komt te staan </param>
        /// <param name="aankomstDatum"> Dit is de datum waarop de persoon/personen aankomen op het terrein</param>
        /// <param name="vertrekDatum"> Dit is de datum waarop de persoon/personen vertrekken van het terrein</param>
        /// <param name="kampeerplaatsnummer"> Dit is het nummer van de kampeerplaats die gekoppeld moet worden aan het reserveringsnummer </param>
        /// <returns> Een bool om te controleren dat het toevoegen daadwerkelijk is gelukt </returns>
        public bool KoppelKampeerplaats(string gebruikersnaam, DateTime aankomstDatum, DateTime vertrekDatum, string kampeerplaatsnummer)
        {
            try
            {
                string reserveringNummer = VindReserveringNummer(gebruikersnaam, aankomstDatum, vertrekDatum);
                string queryInsert =
                    "INSERT INTO Reservering_Kampeerplaats (KampeerplaatsNummer, ReserveringNummer) VALUES('" +
                    kampeerplaatsnummer + "','" + reserveringNummer + "')";
                database.Insert(queryInsert);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Als er meerdere gebruikers zijn aangevinkt, wordt iedere gebruiker (met uitzondering van de hoofdboeker) opgeslagen in de database met zijn/haar gebruikersnaam en reserveringsnummer
        /// </summary>
        /// <param name="medereizigerGebruikersnaam"> De gebruikersnaam van de medereiziger </param>
        /// <param name="reserveringsNummer"> Het reserveringsnummer van de reservering van de hoofdboeker </param>
        /// <returns> Een bool om te controleren dat het toevoegen daadwerkelijk is gelukt </returns>
        public bool VoegMedereizigerToe(string medereizigerGebruikersnaam, string reserveringsNummer)
        {
            try
            {
                string queryInsert = "INSERT INTO Medereiziger (GebruikerGebruikersnaam, ReserveringNummer) VALUES('" +
                                     medereizigerGebruikersnaam + "','" + reserveringsNummer + "')";
                database.Insert(queryInsert);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Met behulp van de reserveringsnummer wordt de betaalstatus van deze reservering bijgewerkt naar betaald
        /// </summary>
        /// <param name="reserveringsnummer"> Het reserveringsnummer van de reservering die betaald is </param>
        /// <returns> Een bool om te controleren dat het updaten daadwerkelijk is gelukt </returns>
        public bool UpdateBetaling(string reserveringsnummer)
        {
            try
            {
                string queryUpdate = "UPDATE Reservering SET Betaald = '1' WHERE Nummer= '" + reserveringsnummer + "'";
                database.Insert(queryUpdate);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Met behulp van de gebruikersnaam, aankomstdatum en vertrekdatum wordt het reserveringsnummer opgezocht
        /// </summary>
        /// <param name="gebruikersnaam"> Dit is de gebruikersnaam waarop de reservering komt te staan </param>
        /// <param name="aankomstDatum"> Dit is de datum waarop de persoon/personen aankomen op het terrein</param>
        /// <param name="vertrekDatum"> Dit is de datum waarop de persoon/personen vertrekken van het terrein</param>
        /// <returns> Het reserveringnummer of als deze niet gevonden kan worden een messageBox </returns>
        public string VindReserveringNummer(string gebruikersnaam, DateTime aankomstDatum, DateTime vertrekDatum)
        {
            try
            {
                string query = "SELECT r.nummer FROM Reservering r WHERE r.GEBRUIKERGEBRUIKERSNAAM = '" + gebruikersnaam +
                               "' AND r.AANKOMSTDATUM = TO_DATE('" + aankomstDatum +
                               "', 'DD/MM/YYYY HH24:MI:SS') AND r.VERTREKDATUM = TO_DATE('" + vertrekDatum +
                               "', 'DD/MM/YYYY HH24:MI:SS')";
                DataTable reserveringen = database.voerQueryUit(query);
                string[] array = new string[1];
                foreach (DataRow dr in reserveringen.Rows)
                {
                    array[0] = dr[0].ToString();
                }
                string reserveringNummer = array.GetValue(0).ToString();
                return reserveringNummer;

            }
            catch (Exception)
            {
                MessageBox.Show("Reserveringsnummer kan niet worden gevonden.");
                return null;
            }
        }

        /// <summary>
        /// Deze methode maakt een lijst van alle kampeerplaatsen uit de database
        /// </summary>
        /// <returns> De lijst van alle kampeerplaatsen </returns>
        public List<string> AllePlaatsen()
        {
            string query = "SELECT * FROM KAMPEERPLAATS";
            DataTable kampeerplaatsen = database.voerQueryUit(query);
            List<string> stringlist= new List<string>();
            foreach (DataRow dr in kampeerplaatsen.Rows)
            {
                stringlist.Add("Nummer: " + dr[0] + " Soort: " + dr[1] + " Aantal Personen: " + dr[2]);
            }
            return stringlist;
        }

        /// <summary>
        /// Deze methode maakt een lijst van alle kampeerplaatsen uit de database waar de eigenschappen van zijn ingevuld
        /// </summary>
        /// <returns> De lijst van kampeerplaatsen met eigenschappen </returns>
        public List<string> AlleSpecifiekePlaatsen()
        {
            string query = "SELECT * FROM Kampeerplaats k WHERE k.eigenschappen IS NOT NULL";
            DataTable specifiekeKampeerplaatsen = database.voerQueryUit(query);
            List<string> stringlist = new List<string>();
            foreach (DataRow dr in specifiekeKampeerplaatsen.Rows)
            {
                stringlist.Add("Nummer: " + dr[0] + " Soort: " + dr[1] + " Eigenschappen: " + dr[3] + " Aantal Personen: " + dr[2]);
            }
            return stringlist;
        }

        /// <summary>
        /// Deze methode maakt een lijst van alle kampeerplaatsen waarvan de vertrekdatum in het verleden ligt
        /// </summary>
        /// <returns> De lijst van kampeerplaatsen die vrij zijn gekomen </returns>
        public List<string> AlleVrijePlaatsen()
        {
            string query =
                "SELECT k.NUMMER,k.SOORT, k.AANTALPERSONEN FROM KAMPEERPLAATS k LEFT JOIN RESERVERING_KAMPEERPLAATS rk ON k.NUMMER = rk.KAMPEERPLAATSNUMMER JOIN RESERVERING r ON r.NUMMER = rk.RESERVERINGNUMMER AND (SELECT TO_DATE(TO_CHAR (SYSDATE, 'dd-mm-yyyy HH24:MI:SS'), 'dd-mm-yyyy HH24:MI:SS') FROM DUAL) NOT BETWEEN r.AANKOMSTDATUM AND r.VERTREKDATUM";
            DataTable vrijeKampeerplaatsen = database.voerQueryUit(query);
            List<string> stringlist = new List<string>();
            foreach (DataRow dr in vrijeKampeerplaatsen.Rows)
            {
                stringlist.Add("Nummer: " + dr[0] + " Soort: " + dr[1] + " Eigenschappen: " + dr[3] + " Aantal Personen: " + dr[2]);
            }
            return stringlist;
        }

        /// <summary>
        /// Deze methode maakt een lijst van alle gebruikers uit de database
        /// </summary>
        /// <returns> De lijst met alle gebruikers </returns>
        public List<string> AlleGebruikers()
        {
            string query = "SELECT g.gebruikersnaam, g.naam FROM GEBRUIKER g";
            DataTable gebruikers = database.voerQueryUit(query);
            List<string> stringlist = new List<string>();
            foreach (DataRow dr in gebruikers.Rows)
            {
                stringlist.Add("Gebruiker: " + dr[0] + " Naam: " + dr[1]);
            }
            return stringlist;
        }

        /// <summary>
        /// Deze methode maakt een lijst van alle reserveringen uit de database
        /// </summary>
        /// <returns> De lijst met alle reserveringen </returns>
        public List<string> AlleReserveringen()
        {
            string query = "SELECT r.nummer, r.gebruikergebruikersnaam FROM Reservering r";
            DataTable reserveringen = database.voerQueryUit(query);
            List<string> stringlist = new List<string>();
            foreach (DataRow dr in reserveringen.Rows)
            {
                stringlist.Add("Nummer: " + dr[0] + " Naam: " + dr[1]);
            }
            return stringlist;
        }
    }
}
