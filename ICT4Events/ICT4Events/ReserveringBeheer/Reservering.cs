using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT4Events.ReserveringBeheer
{
    /// <summary>
    /// In deze klasse worden de objecten van de reserveringen gemaakt
    /// </summary>
    class Reservering
    {
        public int Nummer { get; set; }
        public DateTime AankomstDatum { get; set; }
        public DateTime VertrekDatum { get; set; }
        public bool Betaald { get; set; }


        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="number"> Het reserveringsnummer </param>
        /// <param name="arrivalDate"> De aankomstdatum van de gasten </param>
        /// <param name="departureDate"> De vertrekdatum van de gasten </param>
        /// <param name="paid"> De check om te zien of de reservering betaald is </param>
        public Reservering(int number, DateTime arrivalDate, DateTime departureDate, bool paid)
        {
            Nummer = number;
            AankomstDatum = arrivalDate;
            VertrekDatum = departureDate;
            Betaald = paid;
        }
    }
}
