using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.Mediabeheer
{
    class Reactie
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

        public string ToString()
        {
            return "Id: " + ID + " - Reactie op bericht met id: " + Berichtid + " - Door: " + Gebruiker + " - Bericht: " + Bericht; 
        }



    }
}
