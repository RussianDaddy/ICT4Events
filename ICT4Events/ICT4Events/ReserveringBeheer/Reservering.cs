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
        public DateTime AankomstDatum { get; set; }
        public DateTime VertrekDatum { get; set; }
        public bool Betaald { get; set; }



        public Reservering(int number, DateTime arrivalDate, DateTime departureDate, bool paid)
        {
            Nummer = number;
            AankomstDatum = arrivalDate;
            VertrekDatum = departureDate;
            Betaald = paid;
        }

        public override string ToString()
        {
            return "Nummer: " + Nummer + " Aankomstdatum: " + AankomstDatum.ToString() + " Vertrekdatum: " +
                   VertrekDatum.ToString() + " Betaald: " + Betaald;
        }
    }
}
