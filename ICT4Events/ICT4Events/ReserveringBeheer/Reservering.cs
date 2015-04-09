using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.ReserveringBeheer
{
    class Reservering
    {
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public bool Paid { get; set; }



        public Reservering(int number, DateTime date, bool paid)
        {
            Number = number;
            Date = date;
            Paid = paid;
        }
    }
}
