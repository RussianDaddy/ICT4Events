using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.ReserveringBeheer
{
    class Reservering
    {
        public int Nummer { get; set; }
        public DateTime Datum { get; set; }
        public bool Betaald { get; set; }



        public Reservering(int number, DateTime date, bool paid)
        {
            Nummer = number;
            Datum = date;
            Betaald = paid;
        }
    }
}
