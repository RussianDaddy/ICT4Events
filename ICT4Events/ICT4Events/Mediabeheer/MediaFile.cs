using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.Mediabeheer
{
    public class Mediafile
    {
        private string stringding;
        public Mediafile(int Id, String Naam, String Bericht, String Type, Categorie categorie, String Path, int Like, int Report, String Gebruikersnaam)
        {
            this.Id = Id;
            this.Naam = Naam;
            this.Bericht = Bericht;
            this.Type = Type;
            this.categorie = categorie;
            this.Path = Path;
            this.Like = Like;
            this.Report = Report;
            this.Gebruikersnaam = Gebruikersnaam;
        }

        public int Id { get; set; }
        public String Naam { get; set; }
        public String Bericht { get; set; }
        public String Type { get; set; }
        public Categorie categorie { get; set; }
        public String Path { get; set; }
        public int Like { get; set; }
        public int Report { get; set; }
        public String Gebruikersnaam { get; set; }

        public override string ToString()
        {
            return Id + " - " + Gebruikersnaam + " - Titel: " + Naam + " - Bericht:" + Bericht /*+" - Soort: " + Type + " - Categorie " + categorie.Naam + " - URL: " + Path +*/ + " - Aantal Likes: " + Like /*+ " - Aantal malen gereport: " + Report*/;
        }
        public string WholeString()
        {
            string output = string.Format(Id + " - " + Gebruikersnaam + "{0}{0}Titel: " + Naam + "{0}{0}Bericht: " + Bericht /*+" - Soort: " + Type + " - Categorie " + categorie.Naam + " - URL: " + Path +*/ + "{0}{0}Aantal Likes: " + Like /*+ " - Aantal malen gereport: " + Report*/, Environment.NewLine);
            return output;
            }

    }
}
