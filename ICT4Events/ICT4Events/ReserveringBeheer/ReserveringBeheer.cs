using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.ReserveringBeheer
{
    class ReserveringBeheer
    {
        private List<Reservering> reserveringen;

        public void Reserveren(Reservering reservering)
        {
            reserveringen.Add(reservering);
        }
    }
}
