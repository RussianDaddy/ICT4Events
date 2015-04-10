using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.Mediabeheer
{
    public class Mediabeheer
    {
        private List<Categorie> CategorieLijst;
        private List<Mediafile> MediafileLijst;
        public Mediabeheer()
        {
            CategorieLijst = new List<Categorie>();
        }

        public void DownloadenMedia(int Id)
        {
            //Code voor het downloaden van media vanag de server
        }

        public void UploadenMedia(String Naam, String Type, String Path, Categorie categorie/*Gebruiker gebuiker*/)
        {
            //Code voor het uploaden van media naar de server
        }

        public List<Categorie> GetSearchedCategorie(String Naam)
        {
            List<Categorie> SearchedCategorieLijst = new List<Categorie>();
            foreach (Categorie c in CategorieLijst)
            {
                if (c.Naam == Naam)
                {
                    SearchedCategorieLijst.Add(c);
                }
            }
            return SearchedCategorieLijst;
            //Code voor het returnen van een list met categorie
        }
        public bool BerichtPlaatsen(Mediafile mediafile, String Bericht, DateTime Datum/*,Gebruiker gebruiker*/)
        {
            throw new NotImplementedException();
            //Code voor het apart uploaden naar database ( format: "bericht-Datum-Mediafile-gebruiker")
        }

        public void MediafileRapporteren()
        {
            //berichten hebben een berichtenid nodig. zo kunnne we ze binnen de listbox identificeren. pas dan kun je bepaalde berichten liken en reageren op de desbetreffende berichten
        }

        public void BerichtRapporteren()
        {
            //berichten hebben een berichtenid nodig. zo kunnne we ze binnen de listbox identificeren. pas dan kun je bepaalde berichten liken en reageren op de desbetreffende berichten
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
    }
}
