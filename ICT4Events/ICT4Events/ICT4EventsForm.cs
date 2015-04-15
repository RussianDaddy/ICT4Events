using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using ICT4Events.MateriaalBeheer;


namespace ICT4Events
{
    public partial class ICT4EventsForm : Form
    {
        ReserveringBeheer.ReserveringBeheer reserveringBeheer = new ReserveringBeheer.ReserveringBeheer();
        GebruikerBeheer.GebruikerBeheer Gebruikerbeheer = new GebruikerBeheer.GebruikerBeheer();
        private MateriaalBeheer.Materiaalbeheer materiaalbeheer = new Materiaalbeheer();
        private List<Mediabeheer.Mediafile> tempSoortList;
        private string searchstring;
        private List<Exemplaar> exemplaren; 
        private Mediabeheer.Mediabeheer mediabeheer;
        int buttonZoekGeklikt = 0;

        public ICT4EventsForm()
        {
            InitializeComponent();
            rbtnAllePlaasten.Checked = true;
            clbReserveringKampeerplaatsen.DataSource = reserveringBeheer.AllePlaatsen();
            clbReserveringGebruikers.DataSource = reserveringBeheer.AlleGebruikers();
            listboxReserveringen.DataSource = reserveringBeheer.AlleReserveringen();
            clbExemplaren.DataSource = materiaalbeheer.AlleExemplaren();
            dtpDatumAankomst.MinDate = DateTime.Today;
            dtpDatumVertrek.MinDate = DateTime.Today;

            //Materiaal Beamer = new Materiaal("Beamer",50);
            //Materiaal Laptop = new Materiaal("Laptop",100);
            //Materiaal Hdmi = new Materiaal("HDMI kabel",30);
            //Materiaal Ethernet = new Materiaal("Ethernet kabel",30);

            //materiaalbeheer.Exemplaren = new List<Exemplaar>
            //{
            //    new Exemplaar(1, Beamer),
            //    new Exemplaar(2, Beamer),
            //    new Exemplaar(3, Beamer),
            //    new Exemplaar(4, Laptop),
            //    new Exemplaar(5, Laptop),
            //    new Exemplaar(6, Hdmi),
            //    new Exemplaar(7, Hdmi),
            //    new Exemplaar(8, Ethernet),
            //    new Exemplaar(9, Ethernet)
            //};

            mediabeheer = new Mediabeheer.Mediabeheer();
            exemplaren = new List<Exemplaar>();
            RefreshAll();
        }

        private void refreshLijstTimer_Tick(object sender, EventArgs e)
        {
            if (rbtnAllePlaasten.Checked)
            {
                clbReserveringKampeerplaatsen.DataSource = reserveringBeheer.AllePlaatsen();
            }
            else if (rbtnSpecifiekePlaatsen.Checked)
            {
                clbReserveringKampeerplaatsen.DataSource = reserveringBeheer.AlleSpecifiekePlaatsen();
            }
            else if (rbtnVrijePlaatsen.Checked)
            {
                clbReserveringKampeerplaatsen.DataSource = reserveringBeheer.AlleVrijePlaatsen();
            }
            clbReserveringGebruikers.DataSource = reserveringBeheer.AlleGebruikers();
            listboxReserveringen.DataSource = reserveringBeheer.AlleReserveringen();
        }

        //GebruikerBeheer
        private void btnAanmakenBeheer_Click(object sender, EventArgs e)
        {
            if(tbGebruikersnaamBeheer.Text == "" || tbNaamBeheer.Text == "" || tbWachtwoordBeheer.Text == "")
            {
                MessageBox.Show("Vul alle informatie in.");
            }
            else if (cbAdminBeheer.Checked == true)
            {
                Gebruikerbeheer.Registreren(tbGebruikersnaamBeheer.Text, tbNaamBeheer.Text, tbWachtwoordBeheer.Text, 0,
                    "Ja");
            }
            else
            {
                Gebruikerbeheer.Registreren(tbGebruikersnaamBeheer.Text, tbNaamBeheer.Text, tbWachtwoordBeheer.Text, 0,
                    "Nee");
            }
        }

        //MediaBeheer
        private void btFilter_Click(object sender, EventArgs e)
        {

            searchstring = "";
            LbFeed.Items.Clear();
            if (chbBericht.Checked)
            {
                searchstring += " Bericht";
            }
            if (chbBestand.Checked)
            {
                searchstring += " Bestand";
            }
            if (chbEvent.Checked)
            {
                searchstring += " Event";
            }
            if (chbFoto.Checked)
            {
                searchstring += " Foto";
            }
            if (chbVideo.Checked)
            {
                searchstring += " Video";
            }

            tempSoortList = mediabeheer.GetSearchedSoort(searchstring);

            foreach (Mediabeheer.Mediafile m in tempSoortList)
            {
                LbFeed.Items.Add(m.ToString());
            }

            tempSoortList = null;
            mediabeheer.SearchedSoortLijst = null;
            mediabeheer.SearchedSoortLijst = new List<Mediabeheer.Mediafile>();
        }

        public void RefreshAll()
        {
            LbFeed.Items.Clear();
            foreach (Mediabeheer.Mediafile m in mediabeheer.GetMediafileLijst)
            {
                LbFeed.Items.Add(m.ToString());
            }
        }

        private void btShowAll_Click(object sender, EventArgs e)
        {
            RefreshAll();
            chbBericht.Checked = false;
            chbBestand.Checked = false;
            chbEvent.Checked = false;
            chbFoto.Checked = false;
            chbVideo.Checked = false;
        }

        //ReserveringBeheer
        private void btReserveer_Click(object sender, EventArgs e)
        {
            DateTime aankomstDatum = dtpDatumAankomst.Value.Date;
            DateTime vertrekdatum = dtpDatumVertrek.Value.Date;
            int betaald = 0;
            if (cbBetaald.Checked)
            {
                betaald = 1;
            }
            List<String> kampeerPlaatsen = new List<string>();
            int aantalPersonen = 0;
            foreach (String kampeerplaats in clbReserveringKampeerplaatsen.CheckedItems)
            {
                aantalPersonen = aantalPersonen + Convert.ToInt32(kampeerplaats.Substring(kampeerplaats.Length - 1, 1));
                string kampeerplaatsNummer = kampeerplaats.Substring(8);
                kampeerplaatsNummer = kampeerplaatsNummer.Substring(0, kampeerplaatsNummer.IndexOf(" Soort:"));
                kampeerPlaatsen.Add(kampeerplaatsNummer);
            }
            List<String> gebruikersnamen = new List<string>();
            foreach (String gegevens in clbReserveringGebruikers.CheckedItems)
            {
                string gebruikersnaam = gegevens.Substring(11);
                gebruikersnaam = gebruikersnaam.Substring(0, gebruikersnaam.IndexOf(" Naam:"));
                gebruikersnamen.Add(gebruikersnaam);
            }
            string hoofdboeker = gebruikersnamen.First();
            if (aantalPersonen > gebruikersnamen.Count)
            {
                if (reserveringBeheer.Reserveren(hoofdboeker, aankomstDatum, vertrekdatum, betaald))
                {
                    gebruikersnamen.Remove(hoofdboeker);
                    MessageBox.Show("Reservering Toegevoegd");
                    string reserveringsNummer = reserveringBeheer.VindReserveringNummer(hoofdboeker,
                        aankomstDatum, vertrekdatum);
                    if (gebruikersnamen.Count > 0)
                    {
                        foreach (String medereiziger in gebruikersnamen)
                        {
                            if (reserveringBeheer.VoegMedereizigerToe(medereiziger, reserveringsNummer))
                            {
                                MessageBox.Show("Medereiziger: " + medereiziger + " toegevoegd aan reservering: " +
                                                reserveringsNummer);
                            }
                        }
                    }
                    foreach (var verblijfplaats in kampeerPlaatsen)
                    {
                        if (reserveringBeheer.KoppelKampeerplaats(hoofdboeker, aankomstDatum, vertrekdatum,
                            verblijfplaats))
                        {
                            MessageBox.Show("Verblijfplaats: " + verblijfplaats + " toegevoegd aan reservering: " +
                                            reserveringsNummer);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(
                    "Er zijn meer personen geselecteerd, dan er daadwerkelijk verblijven kan. Selecteer minder gebruikers of meer kampeerplaatsen.");
            }
            listboxReserveringen.DataSource = reserveringBeheer.AlleReserveringen();
        }

        private void rbtnAllePlaasten_CheckedChanged(object sender, EventArgs e)
        {
            clbReserveringKampeerplaatsen.DataSource = reserveringBeheer.AllePlaatsen();
        }

        private void rbtnSpecifiekePlaatsen_CheckedChanged(object sender, EventArgs e)
        {
            clbReserveringKampeerplaatsen.DataSource = reserveringBeheer.AlleSpecifiekePlaatsen();
        }

        private void rbtnVrijePlaatsen_CheckedChanged(object sender, EventArgs e)
        {
            clbReserveringKampeerplaatsen.DataSource = reserveringBeheer.AlleVrijePlaatsen();
        }

        private void btnReserveringBetaald_Click(object sender, EventArgs e)
        {
            string reservering = listboxReserveringen.SelectedItem.ToString();
            reservering = reservering.Substring(8);
            reservering = reservering.Substring(0, reservering.IndexOf(" Naam:"));
            if(reserveringBeheer.UpdateBetaling(reservering))
            {
                MessageBox.Show("Reservering: " + reservering + " is betaald");
            }
            else
            {
                MessageBox.Show("Betaling kan niet worden gewijzigd");
            }
        }

        private void btnLaatZienBeheren_Click(object sender, EventArgs e)
        {
            List<GebruikerBeheer.Gebruiker> Gebruiker = Gebruikerbeheer.LijstAanwezigen();
            foreach(GebruikerBeheer.Gebruiker Temp in Gebruiker)
            {
                clbGebruikersBeheer.Items.Add(Temp.ToString());
            }
        }
        private void dtpDatumVan_ValueChanged(object sender, EventArgs e)
        {
            var reservatievan = new DateTime();
            reservatievan = dtpDatumAankomst.Value;
            dtpDatumVertrek.MinDate = reservatievan;
            dtpDatumVertrek.Refresh();
        }

        //MateriaalBheer
        private void btnZoekExemplaar_Click(object sender, EventArgs e)
        {
            clbExemplaren.DataSource = null;
            clbExemplaren.DataSource = Materiaalbeheer.ZoekMateriaal(tbExemplaarId.Text);
        }

        private List<Exemplaar> GehuurdeExemplaren()
        {
            List<Exemplaar> tempExemplaren = new List<Exemplaar>();
            foreach (Exemplaar e in clbExemplaren.CheckedItems)
            {
                tempExemplaren.Add(e);
                clbExemplaarHuren.Items.Add(e);
            }
            return tempExemplaren;
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void btlike_Click(object sender, EventArgs e)
        {
            string Selectedtems = Convert.ToString(LbFeed.SelectedItem);
            string stringId = Selectedtems.Substring(0, 3);
            int MediafileID = Convert.ToInt32(stringId);

            if(stringId.IndexOf(",") != -1)
            {

            }
        }

        private void btnVerplaatsExemplaren_Click(object sender, EventArgs e)
        {
            foreach (string exemplaar in clbExemplaren.CheckedItems)
            {
                clbExemplaarHuren.Items.Add(exemplaar);
            }
        }

        private void btnTerugplaatsenExemplaren_Click(object sender, EventArgs e)
        {
            clbExemplaarHuren.DataSource = null;
            foreach (string item in clbExemplaarHuren.CheckedItems.OfType<string>().ToList())
            {
                clbExemplaarHuren.Items.Remove(item);
            }
        }

        //EventBeheer
    }
}
 
