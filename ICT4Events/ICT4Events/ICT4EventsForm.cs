using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace ICT4Events
{
    public partial class ICT4EventsForm : Form
    {
        //private Enum e = new Enum;
        private List<Mediabeheer.Mediafile> tempSoortList;
        private string searchstring;
        Mediabeheer.Mediabeheer mediabeheer;

        public ICT4EventsForm()
        {
            InitializeComponent();
            dtpDatumAankomst.MinDate = DateTime.Today;
            dtpDatumVertrek.MinDate = DateTime.Today;
            var oneYearAgoToday = DateTime.Now.AddYears(-18);

            /*
            dtpGeboorteDatum.MaxDate = oneYearAgoToday;
            for (var i = 1; i < 101; i++)
            {
                cbAantalPersonen.Items.Add(i);
            }

            cbVoorkeursplek.Items.AddRange(new[]
            {"Lawaai", "Schaduw", "Noodafstand vanaf facaliteiten*", "Rookvrij", "Adults Only Area"});
            //*Noodafstand houdt in dat het binnen 2 minuten te lopen is vanaf je staplek
            cbKampeerplaats.Items.AddRange(new[]
            {
                "Comfortplaatsen", "Huurtentjes", "Plaatsen voor eigen tenten", "Stacaravans", "Invalidenaccomodaties",
                "Bungalows", "Blokhutten", "Bungalinos"
            });*/
        }

        private void dtpDatumVan_ValueChanged(object sender, EventArgs e)
        {
            var reservatievan = new DateTime();
            reservatievan = dtpDatumAankomst.Value;
            dtpDatumVertrek.MinDate = reservatievan;
            dtpDatumVertrek.Refresh();
        }

        /*
        private void btReserveer_Click(object sender, EventArgs e)
        {
            if (tbVoornaam.Text == "" || tbAchternaam.Text == "" || tbWoonplaats.Text == "" ||
                tbPostcodegetal.Text == "" || tbPostcodeletter.Text == "" || tbTelefoonnummer.Text == "" ||
                tbEmail.Text == "")
            {
                MessageBox.Show("Vul alle velden in!");
            }
        }

        private void btvoegpersoontoe_Click(object sender, EventArgs e)
        {
            if (tbVoornaam.Text == "" || tbAchternaam.Text == "" || tbWoonplaats.Text == "" ||
                tbPostcodegetal.Text == "" || tbPostcodeletter.Text == "" || tbTelefoonnummer.Text == "" ||
                tbEmail.Text == "")
            {
                MessageBox.Show("Vul alle velden in!");
            }
        }
         */

        private void btnAanmakenBeheer_Click(object sender, EventArgs e)
        {
            GebruikerBeheer.GebruikerBeheer Gebruikerbeheer = new GebruikerBeheer.GebruikerBeheer();
            if (cbAdminBeheer.Checked == true)
            {
                Gebruikerbeheer.Registreren(tbGebruikersnaamBeheer.Text, tbNaamBeheer.Text, tbWachtwoordBeheer.Text, 0,  "Ja");
            }
            else
            {
                Gebruikerbeheer.Registreren(tbGebruikersnaamBeheer.Text, tbNaamBeheer.Text, tbWachtwoordBeheer.Text, 0,"Nee");
            }
        }

        private void btFilter_Click(object sender, EventArgs e)
        {

            searchstring = "";
            LbFeed.Items.Clear();
            if (chbBericht.Checked)
            {
                searchstring += " Bericht";
                //tempSoortListBericht = mediabeheer.GetSearchedSoort("Bericht");
            }
            if (chbBestand.Checked)
            {
                searchstring += " Bestand";
                //tempSoortListBestand = mediabeheer.GetSearchedSoort("Bestand");
            }
            if (chbEvent.Checked)
            {
                searchstring += " Event";
                //tempSoortListEvent = mediabeheer.GetSearchedSoort("Event");
            }
            if (chbFoto.Checked)
            {
                searchstring += " Foto";
                //tempSoortListFoto = mediabeheer.GetSearchedSoort("Foto");
            }
            if (chbVideo.Checked)
            {
                searchstring += " Video";
                //tempSoortListVideo = mediabeheer.GetSearchedSoort("Video");
            }

            tempSoortList = mediabeheer.GetSearchedSoort(searchstring);

            //Test van methode (later een foreach die alle categoriën waarop gesorteerd is uitleest en vervolgens alle berichten met categorie.categorie die in de lijst staan in een lijst zet en vervolgens al deze berichten in de lbFeed zet
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
    }
}