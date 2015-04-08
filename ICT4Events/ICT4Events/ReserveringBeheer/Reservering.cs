using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.ReserveringBeheer
{
    class Reservering
    {
        private int Nummer;
        private DateTime Datum;
        private bool Betaald;

        public Reservering(int nummer, DateTime datum, bool betaald)
        {
            this.Nummer = nummer;
            this.Datum = datum;
            this.Betaald = betaald;
        }
    }
}
