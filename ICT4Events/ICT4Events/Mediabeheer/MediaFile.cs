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
        public Mediafile(int Id, String Naam, String Type, Categorie categorie, String Path, int Like, bool Report)
        {
            this.Id = Id;
            this.Naam = Naam;
            this.Type = Type;
            this.categorie = categorie;
            this.Path = Path;
            this.Like = Like;
            this.Report = Report;
        }

        public int Id { get; set; }
        public String Naam { get; set; }
        public String Type { get; set; }
        public Categorie categorie { get; set; }
        public String Path { get; set; }
        public int Like { get; set; }
        public bool Report { get; set; }


        public string ToString()
        {
            return Id + " - " + Naam + " - " + Type + " - " + categorie.Naam + " - " + Path + " - " + Like + " - " + Report;
        }


    }
}
