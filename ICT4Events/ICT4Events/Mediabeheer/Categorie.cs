using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.Mediabeheer
{
    public class Categorie
    {
        public Categorie(int Id, String Naam)
        {
            this.Id = Id;
            this.Naam = Naam;
        }

        public int Id { get; set; }
        public String Naam { get; set; }
    }
}
