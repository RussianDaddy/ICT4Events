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
        public List<Mediafile> SearchedSoortLijst { get { return searchedSoortLijst; } set { searchedSoortLijst = value; } }
        public List<Mediafile> GetMediafileLijst { get { return MediafileLijst; } set { MediafileLijst = value; } }

        public List<Categorie> GetCategorieLijst { get { return CategorieLijst; } set { CategorieLijst = value; } }
        public Mediabeheer()
        {
            /*GetCategorieLijst = new List<Categorie>{
            new Categorie(1, "Sport"),
            new Categorie(2, "Eten"),
            new Categorie(3, "Spelen"),
            new Categorie(4, "Feesten"),
            new Categorie(5, "Gamen")
            };
            Categorie Sport = CategorieLijst[0];
            Categorie Eten = CategorieLijst[1];
            Categorie Spelen = CategorieLijst[2];
            Categorie Feesten = CategorieLijst[3];
            Categorie Gamen = CategorieLijst[4];*/

            //testdata mediafiles
            /*MediafileLijst = new List<Mediafile> {
            new Mediafile(1, "beleg", "We gaan lekker broodjes met.... Beleg eten! kom gezellig een broodje met... Beleg eten", "Foto", Eten, @"C:\...", 8564947, 0, "wout_kamp@hotmail.com"),
            new Mediafile(2, "Glijbaan", " ", "Bericht", Spelen, @"C:\...", 8, 11, "wout_kamp@hotmail.com"),
            new Mediafile(3, "Basketbal!", " ", "Event", Sport, @"C:\...", 23, 5, "wout_kamp@hotmail.com"),
            new Mediafile(4, "Disco!!", " ", "Video", Feesten, @"C:\...", 23, 3, "wout_kamp@hotmail.com") ,
            new Mediafile(5, "Weer", " ", "Foto", Gamen, @"C:\...", 48, 8, "wout_kamp@hotmail.com"),
            new Mediafile(6, "Yu-Gi-Oh TCG"," ", "Bestand", Gamen, @"C:\...", 1223, 1, "wout_kamp@hotmail.com")
            };*/

            Update();


        }

        public void Update()
        {
            string sqlGetMediafile = "SELECT * FROM CATEGORIE";
            GetCategorieLijst = database.GetCategorieLijst(sqlGetMediafile, 0);

            //Uncommenten zodra database.Reader.read() != null  (Reader leest geen rijen uit Mediafile tabel)
            string sqlGetCategorie = "SELECT * FROM MEDIAFILE";
            GetMediafileLijst = database.GetBerichtenList(sqlGetCategorie, CategorieLijst);
        }
        public void DownloadenMedia(int Id)
        {
            //Code voor het downloaden van media vanag de server
        }

        public void UploadenMedia(String Naam, String Type, String Path, Categorie categorie/*Gebruiker gebuiker*/)
        {
            //Code voor het uploaden van media naar de server
        }

        public List<Mediafile> GetSearchedSoort(String searchstring)
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
        public bool BerichtPlaatsen(int MediafileId, string Gebruiker, string Titel, string Bericht, string soort, string categories, string Path, int Likes, int Report)
        {

            try
            {
                Categorie categorie = ReturnCategorie(categories);
                Mediafile mediaffile = new Mediafile(MediafileId, Titel, Bericht, soort, categorie, Path, Likes, Report, Gebruiker);
                int unicode = 34;
                char character = (char)unicode;
                string text = character.ToString();

                string queryInsert =
                    "INSERT INTO MEDIAFILE (ID, Name, Bericht, Type, Categorie, Path, VindIkLeuk, Report, GebruikerGebruikersnaam) VALUES('" +
                    MediafileId + "','" + Titel + "','" + Bericht + "','" + soort + "','" + categorie.Naam + "','" + Path + "','" + Likes + "','" + Report + "','" + Gebruiker + "')";
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

        public bool MediafileRapporteren(int stringId)
        {
            foreach (Mediafile m in GetMediafileLijst)
            {
                if (m.Id == Convert.ToInt32(stringId))
                {
                    m.Report++;
                    return true;
                }
            }
            return false;
            //berichten hebben een berichtenid nodig. zo kunnne we ze binnen de listbox identificeren. pas dan kun je bepaalde berichten liken en reageren op de desbetreffende berichten

        }

        public void BerichtRapporteren()
        {
            //berichten hebben een berichtenid nodig. zo kunnne we ze binnen de listbox identificeren. pas dan kun je bepaalde berichten liken en reageren op de desbetreffende berichten
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

        public List<Mediafile> Filteren()
        {
            throw new NotImplementedException();
        }

        public void Liken(int MediafileId)
        {
            foreach (Mediafile m in MediafileLijst)
            {
                if (m.Id == MediafileId)
                {
                    m.Like = m.Like + 1;
                }
            }
        }

        public int VraagLikesOp(int MediafileId)
        {
            foreach (Mediafile m in MediafileLijst)
            {
                if (MediafileId == m.Id)
                {
                    return m.Like;
                }

            }
            return -1;
        }







    }
}
