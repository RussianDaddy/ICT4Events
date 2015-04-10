namespace ICT4Events.GebruikerBeheer
{
    abstract class Gebruiker
    {
        string Gebruikersnaam { get; set; }
        string Naam { get; set; }
        string Wachtwoord { get; set; }
        bool Aanwezig { get; set; }
        bool Admin { get; set; }
        int RFID { get; set; }

        public Gebruiker(string gebruikersnaam, string naam, string wachtwoord, bool aanwezig, int rfid, bool admin)
        {
            this.Gebruikersnaam = gebruikersnaam;
            this.Naam = naam;
            this.Wachtwoord = wachtwoord;
            this.Aanwezig = aanwezig;
            this.RFID = rfid;
            this.Admin = admin;
        }
    }
}
