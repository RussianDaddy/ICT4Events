using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using System.Data;
using System.IO;

namespace ICT4Events.Mediabeheer
{
    public class Mediabeheer
    {
        private List<Categorie> CategorieLijst;
        private List<Mediafile> searchedSoortLijst = new List<Mediafile>();
        private List<Mediafile> MediafileLijst;
        private List<Reactie> reactielijst;
        private Database.Database database = new Database.Database();
        string WholeString;
        public List<Mediafile> SearchedSoortLijst { get { return searchedSoortLijst; } set { searchedSoortLijst = value; } }
        public List<Mediafile> GetMediafileLijst { get { return MediafileLijst; } set { MediafileLijst = value; } }

        public List<Categorie> GetCategorieLijst { get { return CategorieLijst; } set { CategorieLijst = value; } }
        public List<Reactie> GetReactieLijst { get{ return reactielijst; } set{reactielijst = value; }  }

        /// <summary>
        /// Constructor
        /// </summary>
        public Mediabeheer()
        {
            Update();
        }

        /// <summary>
        /// Maakt een MediaFile object aan en voegt deze via de Databaseklasse toe aan de database
        /// </summary>
        /// <param name="MediafileId">Het id van het aan te maken mediafile</param>
        /// <param name="Gebruiker">De naam van de gebruiker die het bericht aan wilt maken</param>
        /// <param name="Titel">De titel van het aan te maken bericht</param>
        /// <param name="Bericht">Het Bericht dat de gebruiker wilt posten</param>
        /// <param name="soort">Het geselecteerde type van de post</param>
        /// <param name="categories">De categorie waartoe het mediafile toe behoort</param>
        /// <param name="Path">Het pad naar het bestand</param>
        /// <param name="Likes">Het aantal like dat het mediafile heeft (0 bij het aanmaken)</param>
        /// <param name="Report">Het aantal reports dat het mediafile heeft ( 0 bij het aanmaken) </param>
        /// <returns>true wanneer het mediafile succevolis toegevoeg aan de database/ false wanneer het niet gelukt is om het mediafile toe te voegen aan de database</returns>
        public bool BerichtPlaatsen(int MediafileId, string Gebruiker, string Titel, string Bericht, string soort, string categories, string Path, int Likes, int Report)
        {
            try
            {
                Categorie categorie = ReturnCategorie(categories);
                Mediafile mediaffile = new Mediafile(MediafileId, Titel, Bericht, soort, categorie, Path, Likes, Report, Gebruiker);
                string queryInsert =
                    "INSERT INTO MEDIAFILE (ID, Name, Bericht, Type, Path, VindIkLeuk, Report, GebruikerGebruikersnaam, CategorieID) VALUES('" +
                    MediafileId + "','" + Titel + "','" + Bericht + "','" + soort + "','" + Path + "','" + Likes + "','" + Report + "','" + Gebruiker + "', " + categorie.Id + ")";
                if (database.Insert(queryInsert) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Maakt een Reactie object aan met de meegekregen parameters
        /// </summary>
        /// <param name="ID">Het id van de aan te maken reactie</param>
        /// <param name="MediafileId">Het id van de mediafile waarop gereageerd wordt</param>
        /// <param name="Bericht">De tekst van de reactie</param>
        /// <param name="Gebruiker">De gebruiker die reageerd</param>
        /// <returns></returns>
        public bool ReactiePlaatsen(int ID, int MediafileId, string Bericht, string Gebruiker)
        {
            try
            {
                Reactie Reply = new Reactie(ID, Bericht, Gebruiker, MediafileId);
                string queryInsert =
                    "INSERT INTO Reactie (ID, MediafileID, GebruikerGebruikersnaam, Bericht, Datum) VALUES('" + ID + "','" + MediafileId + "','" + Gebruiker + "','" + Bericht + "', null)";
                if (database.Insert(queryInsert) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>
        /// zoekt in de lijsten naar het object met een id dat overeenkomt met het meegekregen id en hoogt de like counter van dit object op met 1, roept vervolgens de UpdateLRNaarDb methode aan
        /// </summary>
        /// <param name="MediafileId">id van het geselecteerde mediafile</param>
        public void Liken(int MediafileId)
        {
            Mediafile f = null;
            foreach (Mediafile m in MediafileLijst)
            {
                if (m.Id == MediafileId)
                {
                    m.Like = m.Like + 1;
                    f = m;
                }
            }
            UpdateLRNaarDb(f);
        }

        /// <summary>
        /// zoekt in de lijsten naar het object met een id dat overeenkomt met het meegekregen id en hoogt de report counter van dit object op met 1, roept vervolgens de UpdateLRNaarDb methode aan
        /// </summary>
        /// <param name="stringId">id van het geselecteerde mediafile</param>
        /// <returns>true wanneer het ophogen gelukt is/ false wanneer dit niet gelukt is</returns>
        public bool MediafileRapporteren(int stringId)
        {
            Mediafile f = null;
            foreach (Mediafile m in GetMediafileLijst)
            {
                if (m.Id == Convert.ToInt32(stringId))
                {
                    m.Report++;
                    f = m;
                    UpdateLRNaarDb(f);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// download de bestanden naar uw computer
        /// </summary>
        /// <param name="Id"></param>
        public bool DownloadenMedia(string bronFile, string bestandsnaam)
        {
            //Code voor het downloaden van media vanaf de server
            string doelPath = @"C:\Users\Public\ICT4Events";
            string doelBestand = Path.Combine(doelPath, bestandsnaam);
            if (!Directory.Exists(doelPath))
            {
                Directory.CreateDirectory(doelPath);
            }
            try
            {
                File.Copy(bronFile, doelBestand, true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// upload het bestand vanaf uw computer naar de server
        /// </summary>
        /// <param name="Naam"></param>
        /// <param name="Type"></param>
        /// <param name="Path"></param>
        /// <param name="categorie"></param>
        public void UploadenMedia(string bronPath, string bestandsnaam, int type)
        {
            //Code voor het uploaden van media naar de server
            switch (type)
            {
                case 1:
                    File.Copy(bronPath, @"\\SRV2\Foto\" + bestandsnaam);
                    break;
                case 2:
                    File.Copy(bronPath, @"\\SRV2\Video\" + bestandsnaam);
                    break;
                case 3:
                    File.Copy(bronPath, @"\\SRV2\Bestanden\" + bestandsnaam);
                    break;
            }
        }

        /// <summary>
        /// Filters alle berichten die voldoen aan de toegepaste filters 
        /// </summary>
        /// <param name="searchstring">string met daarin alle namen van de toegepaste filters</param>
        /// <returns>lijst met berichten die voldoen aan de filters</returns>
        public List<Mediafile> Filteren(String searchstring)
        {
            foreach (Mediafile m in MediafileLijst)
            {
                if (searchstring.IndexOf(m.Type) != -1)
                {
                    SearchedSoortLijst.Add(m);
                }
            }
            return SearchedSoortLijst;
        }

        /// <summary>
        /// returned de categorie die overeenkomt met de meegestuurde naam
        /// </summary>
        /// <param name="cat">naam van de te returnen categorie</param>
        /// <returns>het object dat overeenkomt met de naam die meegestuurd is</returns>
        public Categorie ReturnCategorie(string cat)
        {
            foreach (Categorie c in GetCategorieLijst)
            {
                if (c.Naam == cat)
                {
                    return c;
                }
            }
            return null;
        }

        /// <summary>
        /// stuurt een update naar de database waardoor het aantal likes en aantal reports ook in de database wordt vastgelegd
        /// </summary>
        /// <param name="f">het object waarvan de gegevens moeten worden geupdate</param>
        public void UpdateLRNaarDb(Mediafile f)
        {
            foreach (Mediafile m in GetMediafileLijst)
            {
                if (m.Id == f.Id)
                {
                    String updateSql = "UPDATE MEDIAFILE SET VindIkLeuk  = '" + f.Like + "', Report = '" + f.Report + "'WHERE ID = '" + f.Id + "'";
                    database.Insert(updateSql);
                }
            }
        }

        /// <summary>
        /// returned de gehele string van een reactie, deze string wordt vervolgens getoont in de messagebox
        /// </summary>
        /// <param name="ID">het id van de geselecteerde reactie</param>
        /// <param name="MediaID">het id van het bericht waar de reactie bij hoort</param>
        /// <returns></returns>
        public string GetWholeReactieString(int ID, int MediaID)
        {
            Mediafile Media = null;
            foreach (Mediafile m in GetMediafileLijst)
            {
                if (m.Id == MediaID)
                {
                    Media = m;
                }
            }
            foreach (Reactie R in GetReactieLijst)
            {
                if (R.ID == ID)
                {
                    WholeString = R.WholeString(Media);
                }
            }
            return WholeString;
        }

        /// <summary>
        /// zet alle gegevens uit de database opnieuw in de lijsten
        /// </summary>
        public void Update()
        {
            string sqlGetMediafile = "SELECT * FROM CATEGORIE";
            GetCategorieLijst = null;
            GetCategorieLijst = database.GetCategorieLijst(sqlGetMediafile, 0);

            string sqlGetCategorie = "SELECT * FROM MEDIAFILE";
            GetMediafileLijst = null;
            GetMediafileLijst = database.GetBerichtenList(sqlGetCategorie, CategorieLijst);

            string sqlGetReactie = "SELECT * FROM REACTIE";
            GetReactieLijst = null;
            GetReactieLijst = database.GetReactieLijst(sqlGetReactie);
        }
    }
}
