using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.Mediabeheer
{
    class MediaFile
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
        public bool Like { get; set; }
        public bool Report { get; set; }

        /*
        public MediaFile(int id, string type, Categorie categorie, string path, Gebruiker gebruiker)
        {
            
        }
         */
    }
}
