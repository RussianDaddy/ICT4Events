using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICT4Events.GebruikerBeheer;

namespace ICT4Events.MateriaalBeheer
{
    class Materiaalbeheer
    {
    //private Gebruiker Harold = new Gast("RussianDaddy", "Harold", "Egelhoorntje96", false, 1, false);
        public List<Exemplaar> Exemplaren;
        public Materiaalbeheer()
        {
            Exemplaren = new List<Exemplaar>();
            
        }

        public void HurenMateriaal(int id, DateTime uitleenDatum, DateTime retourDatum, Exemplaar exemplaar, Gebruiker gebruiker)
        {
            Uitlening uitlening = new Uitlening(id, uitleenDatum, retourDatum, exemplaar, gebruiker);
            
        }

        public void UitgevenRFID(Gebruiker gebruiker, int rfid)
        {
            
        }
    }
}
