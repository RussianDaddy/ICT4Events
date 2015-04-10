using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using ICT4Events.Database;

namespace ICT4Events.ReserveringBeheer
{
    class ReserveringBeheer
    {
        Database.Database database = new Database.Database();

        public bool Reserveren(int number, DateTime date, bool paid, int campnumber)
        {
            throw new NotImplementedException();
        }

        public void AllePlaatsen()
        {
            string query = "SELECT * FROM KAMPEERPLAATS";
            database.voerQueryUit(query);
        }

        public void VrijePlaatsen()
        {
            throw new NotImplementedException();
        }
    }
}
