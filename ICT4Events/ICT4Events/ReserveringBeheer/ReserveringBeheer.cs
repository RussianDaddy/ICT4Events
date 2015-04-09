using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;

namespace ICT4Events.ReserveringBeheer
{
    class ReserveringBeheer
    {
        public void Reserveren(Reservering reservering)
        {
            throw new NotImplementedException();
        }

        public static String AllePlaatsen()
        {
            string query = "SELECT * FROM KAMPEERPLAATS";
            return query;
        }

        public void VrijePlaatsen()
        {
            throw new NotImplementedException();
        }
    }
}
