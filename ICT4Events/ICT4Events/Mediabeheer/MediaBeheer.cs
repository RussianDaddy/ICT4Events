using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using System.Data;

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
        public Mediabeheer()
        {
            Update();
        }
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
        public List<Mediafile> ZoekFiles(String MediaFileNaam)
        {
            List<Mediafile> SearchedMediafileLijst = new List<Mediafile>();
            foreach (Mediafile m in MediafileLijst)
            {
                if (m.Naam == MediaFileNaam)
                {
                    SearchedMediafileLijst.Add(m);
                }
            }
            return SearchedMediafileLijst;
        }
        public void DownloadenMedia(int Id)
        {
            //Code voor het downloaden van media vanag de server
        }
        public void UploadenMedia(String Naam, String Type, String Path, Categorie categorie/*Gebruiker gebuiker*/)
        {
            //Code voor het uploaden van media naar de server
        }
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
        public void Update()
        {
            string sqlGetMediafile = "SELECT * FROM CATEGORIE";
            GetCategorieLijst = null;
            GetCategorieLijst = database.GetCategorieLijst(sqlGetMediafile, 0);

            //Uncommenten zodra database.Reader.read() != null  (Reader leest geen rijen uit Mediafile tabel)
            string sqlGetCategorie = "SELECT * FROM MEDIAFILE";
            GetMediafileLijst = null;
            GetMediafileLijst = database.GetBerichtenList(sqlGetCategorie, CategorieLijst);

            String sqlGetReactie = "SELECT * FROM REACTIE";
            GetReactieLijst = null;
            GetReactieLijst = database.GetReactieLijst(sqlGetReactie);
        }
        public void BerichtRapporteren()
        {
            //berichten hebben een berichtenid nodig. zo kunnen we ze binnen de listbox identificeren. pas dan kun je bepaalde berichten liken en reageren op de desbetreffende berichten
        }
        /*public int VraagLikesOp(int MediafileId)
        {
            foreach (Mediafile m in MediafileLijst)
            {
                if (MediafileId == m.Id)
                {
                    return m.Like;
                }

            }
            return -1;
        }*/

        /*public List<Mediafile> GetSearchedSoort(String searchstring)
        {

            foreach (Mediafile m in MediafileLijst)
            {
                if (searchstring.IndexOf(m.Type) != -1)
                {
                    SearchedSoortLijst.Add(m);
                }
            }
            return SearchedSoortLijst;
        }*/
    }
}
