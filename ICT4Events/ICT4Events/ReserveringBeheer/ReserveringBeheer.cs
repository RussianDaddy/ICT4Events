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
        static Database.Database database = new Database.Database();

        public bool Reserveren(int number, DateTime date, bool paid, int campnumber)
        {
            throw new NotImplementedException();
        }

        public static List<string> AllePlaatsen()
        {
            string query = "SELECT * FROM KAMPEERPLAATS";
            DataTable kampeerplaatsen = database.voerQueryUit(query);
            List<String> stringlist= new List<string>();
            foreach (DataRow dr in kampeerplaatsen.Rows)
            {
                foreach (var item in dr.ItemArray)
                {
                    stringlist.Add(item.ToString());
                }
            }
            return stringlist;
        }

        public void VrijePlaatsen()
        {
            throw new NotImplementedException();
        }
    }
}
