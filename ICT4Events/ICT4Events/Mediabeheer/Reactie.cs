using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.Mediabeheer
{
    class Reactie
    {
        public Reactie(String Bericht, DateTime Datum )
        {
            this.Bericht = Bericht;
            this.Datum = Datum;
        }

        public String Bericht { get; set; }
        public DateTime Datum { get; set; }





    }
}
