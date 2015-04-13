using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.MateriaalBeheer
{
    class Uitlening
    {
        public int Id { get; set; }
        public DateTime Uitleendatum { get; set; }
        public DateTime Retourdatum { get; set; }

        public Uitlening(int id, DateTime uitleendatum, DateTime retourdatum, Exemplaar exemplaar, GebruikerBeheer.Gebruiker gebruiker)
        {
            Id = id;
            Uitleendatum = uitleendatum;
            Retourdatum = retourdatum;
        }
    }
}
