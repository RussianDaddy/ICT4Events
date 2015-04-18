using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.Mediabeheer
{
    public class Reactie
    {
        public Reactie(String Bericht, DateTime Datum, int Berichtid)
        {
            this.Bericht = Bericht;
            this.Datum = Datum;
            this.Berichtid = Berichtid;
        }
        public Reactie(int Id, String Bericht, String Gebruiker, int Berichtid)
        {
            this.Bericht = Bericht;
            //this.Datum = Datum;
            this.Berichtid = Berichtid;
            this.Gebruiker = Gebruiker;
            this.ID = Id;
        }
        public String Bericht { get; set; }
        public int ID { get; set; }
        public DateTime Datum { get; set; }
        public int Berichtid { get; set; }
        public String Gebruiker { get; set; }
        public override string ToString()
        {
            return "ID: " + ID + " - Reactie op bericht met id: " + Berichtid + " ,- Door: " + Gebruiker + " - Bericht: " + Bericht; 
        }
        public string WholeString(Mediafile M)
        {
            string output = string.Format(Gebruiker + " Reageerde op: {0}" + Berichtid + " - Titel: " + M.Naam + "{0}{0}Bericht: " + M.Bericht /*+" - Soort: " + Type + " - Categorie " + categorie.Naam + " - URL: " + Path + */+ "{0}{0}Aantal Likes: " + M.Like + /*" - Aantal malen gereport: " + Report*/ "{0}Origineel gepost door: " + M.Gebruikersnaam + "{0}{0}Reactie: {0}" + Bericht,  Environment.NewLine);
            return output;
        }
    }
}
