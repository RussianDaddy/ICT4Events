using System.Collections.Generic;

namespace ICT4Events.MateriaalBeheer
{
    class Materiaal
    {
        public string Soort { get; set; }
        public int Borg { get; set; } 
        public Materiaal(string soort, int borg)
        {
            Soort = soort;
            Borg = borg;
        }

        public override string ToString()
        {
            return Soort + " - " + Borg;
        }
    }
}
