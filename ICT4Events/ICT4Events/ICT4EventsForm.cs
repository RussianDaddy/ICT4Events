using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Phidgets.Events;
using System.Threading;
using Phidgets;
using ICT4Events.GebruikerBeheer;
using ICT4Events.MateriaalBeheer;
using System.IO;

namespace ICT4Events
{
    public partial class ICT4EventsForm : Form
    {
        ReserveringBeheer.ReserveringBeheer reserveringBeheer = new ReserveringBeheer.ReserveringBeheer();
        GebruikerBeheer.GebruikerBeheer Gebruikerbeheer = new GebruikerBeheer.GebruikerBeheer();
        private Materiaalbeheer materiaalbeheer = new Materiaalbeheer();
        private List<Mediabeheer.Mediafile> tempSoortList;
        private string searchstring;
        private List<Exemplaar> exemplaren; 
        private Mediabeheer.Mediabeheer mediabeheer;
        RFID rfid;
        
        private string stringId;
        private int idTeller = 2;
        private string path;
        private string loggedinuser;

        public ICT4EventsForm()
        {
            InitializeComponent();
            rbtnAllePlaasten.Checked = true;
            clbReserveringKampeerplaatsen.DataSource = reserveringBeheer.AllePlaatsen();
            clbReserveringGebruikers.DataSource = reserveringBeheer.AlleGebruikers();
            listboxReserveringen.DataSource = reserveringBeheer.AlleReserveringen();
            clbExemplaren.DataSource = materiaalbeheer.AlleExemplaren();
            lbGasten.DataSource = materiaalbeheer.AlleGasten();

            //RFID
            rfid = new RFID();
            rfid.open();//Opent de verbinding
            rfid.Attach += new AttachEventHandler(rfid_Attach);//kijkt of de Reader verbonden is
            rfid.Detach += new DetachEventHandler(rfid_Detach);
            rfid.Tag += new TagEventHandler(rfid_Tag);//scant de rfid tag
            lbRfidStatus.Text = "RFID not Connected!";
            lbRfidStatus.ForeColor = System.Drawing.Color.Red;
            tmRFIDTextboxClear.Stop();
            //

            dtpDatumAankomst.MinDate = DateTime.Today;
            dtpDatumVertrek.MinDate = DateTime.Today;
            lbBetaalstatus.Visible = false;
            tabICT4Events.TabPages.Remove(TabReserveren);
            tabICT4Events.TabPages.Remove(TabFeed);
            tabICT4Events.TabPages.Remove(TabHuren);
            tabICT4Events.TabPages.Remove(TabBeheren);
            tabICT4Events.TabPages.Remove(TabUitloggen);
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

        //RFID Methods
        private void rfid_Attach(object sender, AttachEventArgs e)
        {
            RFID attached = (RFID)sender;
            rfid.Antenna = true;
            lbRfidStatus.Text = "RFID Connected!";
            lbRfidStatus.ForeColor = System.Drawing.Color.Black;
        }
        private void rfid_Tag(object sender, TagEventArgs e)
        {
            tbRFIDnummBeheer.Text = e.Tag;
            rfid.LED = true;
            Thread.Sleep(100);
            rfid.LED = false;
        }
        private void rfid_Detach(object sender, DetachEventArgs e)
        {
            RFID detached = (RFID)sender;
            lbRfidStatus.Text = "RFID not Connected!";
            lbRfidStatus.ForeColor = System.Drawing.Color.Red;
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

        private void tbRFIDnummBeheer_TextChanged(object sender, EventArgs e)
        {
            if (tbRFIDnummBeheer.Text != "")
            {
                if (Gebruikerbeheer.Aanwezig(tbRFIDnummBeheer.Text) == true)
                {
                    pnIncheckBeheer.BackColor = System.Drawing.Color.Green;
                    tmRFIDTextboxClear.Start();
                }
                else
                {
                    pnIncheckBeheer.BackColor = System.Drawing.Color.Red;
                    tmRFIDTextboxClear.Start();
                }
            }
            FillLbGebruikerBeheer();
            
        }

        private void tmRFIDTextboxClear_Tick(object sender, EventArgs e)
        {
            tbRFIDnummBeheer.Text = "";
            pnIncheckBeheer.BackColor = System.Drawing.Color.LightGray;
            tmRFIDTextboxClear.Stop();
        }

        private void btnWijzigenBeheer_Click(object sender, EventArgs e)
        {
            if (tbGebruikersnaamBeheer.Text == "" || tbNaamBeheer.Text == "" || tbWachtwoordBeheer.Text == "")
            {
                MessageBox.Show("Vul alle informatie in.");
            }
            else if (cbAdminBeheer.Checked == true)
            {
                Gebruikerbeheer.Update(tbGebruikersnaamBeheer.Text, tbNaamBeheer.Text, tbWachtwoordBeheer.Text,
                    "Ja");
                FillLbGebruikerBeheer();
            }
            else
            {
                Gebruikerbeheer.Update(tbGebruikersnaamBeheer.Text, tbNaamBeheer.Text, tbWachtwoordBeheer.Text,
                    "Nee");
                FillLbGebruikerBeheer();
            }
        }

        private void btnAanpassenBeheer_Click(object sender, EventArgs e)
        {
            if (lbGebruikerBeheer.SelectedItem != null)
            {
                string gebruiker = lbGebruikerBeheer.SelectedItem.ToString();
                string email = gebruiker.Split(',')[0];
                GebruikerBeheer.Gebruiker TempGebruiker = Gebruikerbeheer.GebruikerOpvragen(email);
                tbGebruikersnaamBeheer.Text = TempGebruiker.Gebruikersnaam;
                tbNaamBeheer.Text = TempGebruiker.Naam;
                tbWachtwoordBeheer.Text = TempGebruiker.Wachtwoord;
                if (TempGebruiker.Admin == true)
                {
                    cbAdminBeheer.Checked = true;
                }
                else
                {
                    cbAdminBeheer.Checked = false;
                }
            }
            else
            {
                MessageBox.Show("Geen gebruiker geselecteerd.");
            }
        }

        private void btnLaatZienBeheren_Click_1(object sender, EventArgs e)
        {
            FillLbGebruikerBeheer();
        }

        public void FillLbGebruikerBeheer()
        {
            lbGebruikerBeheer.Items.Clear();
            List<GebruikerBeheer.Gebruiker> Gebruiker = new List<GebruikerBeheer.Gebruiker>();
            if (cbAanwezigBeheer.Checked == true)
            {
                Gebruiker = Gebruikerbeheer.LijstAanwezigen(true);
            }
            else
            {
                Gebruiker = Gebruikerbeheer.LijstAanwezigen(false);
            }
            if (Gebruiker.Count == 0)
            {
                lbGebruikerBeheer.Items.Add("Geen gebruikers aanwezig");
            }
            else
            {
                foreach (GebruikerBeheer.Gebruiker Temp in Gebruiker)
                {
                    lbGebruikerBeheer.Items.Add(Temp.ToString());
                }
            }
        }

        private void lbGebruikerBeheer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string email = "";
            string gebruiker = "";
            lbBetaalstatus.Visible = false;
            try
            {
                lbGebruikerBeheer.SelectedItem.ToString();
                email = gebruiker.Split(',')[0];
            }
            catch(NullReferenceException)
            {

            }
            cbBetaaldBeheer.Items.Clear();
            cbBetaaldBeheer.Text = "";
            List<ReserveringBeheer.Reservering> Reserveringen = new List<ReserveringBeheer.Reservering>();
            Reserveringen = Gebruikerbeheer.GebruikerReservering(email);
            foreach(ReserveringBeheer.Reservering Temp in Reserveringen)
            {
                cbBetaaldBeheer.Items.Add(Temp.Nummer);
            }
        }

        private void btnCheckBetaalstatus_Click(object sender, EventArgs e)
        {
            if(lbGebruikerBeheer.SelectedItem == null)
            {
                MessageBox.Show("Geen gebruiker geselecteerd.");
            }
            else if (cbBetaaldBeheer.SelectedItem == null)
            {
                MessageBox.Show("Geen reservering geselecteerd.");
            }
            else
            {
                string gebruiker = lbGebruikerBeheer.SelectedItem.ToString();
                string email = gebruiker.Split(',')[0];
                string nummerString = cbBetaaldBeheer.SelectedItem.ToString();
                int nummerInt = Convert.ToInt32(nummerString);
                if(Gebruikerbeheer.Betaalstatus(email, nummerInt) == true)
                {
                    lbBetaalstatus.Text = "BETAALD";
                    lbBetaalstatus.ForeColor = System.Drawing.Color.Green;
                    lbBetaalstatus.Visible = true;
                }
                else
                {
                    lbBetaalstatus.Visible = true;
                    lbBetaalstatus.Text = "NIET BETAALD";
                    lbBetaalstatus.ForeColor = System.Drawing.Color.Red;
                }
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

        private void btlike_Click(object sender, EventArgs e)
        {
            string Selectedtems = Convert.ToString(LbFeed.SelectedItem);
            if(LbFeed.SelectedItem != null)
            {
                for (int i = 10; i > 0; i--)
                {
                    stringId = Selectedtems.Substring(0, i);
                    if (stringId.IndexOf("-") == -1)
                    {
                        stringId = stringId.Substring(0, (i));
                        MessageBox.Show("U heeft de post met ID " + stringId + " geliked");
                        i = -1;
                    }
                }
                int MediafileID = Convert.ToInt32(stringId);
                mediabeheer.Liken(MediafileID);
                RefreshAll();
            }
            else
            {
                MessageBox.Show("Selecteer eerste een bericht om te liken!");
            }
            
        }

        private void btviewpost_Click(object sender, EventArgs e)
        {
            string Selectedtem = Convert.ToString(LbFeed.SelectedItem);
            
            if (LbFeed.SelectedItem != null)
            {
                for (int i = 10; i > 0; i--)
                {
                    stringId = Selectedtem.Substring(0, i);
                    if (stringId.IndexOf("-") == -1)
                    {
                        stringId = stringId.Substring(0, (i));
                        
                            foreach(Mediabeheer.Mediafile m in mediabeheer.GetMediafileLijst)
                            {
                                if(m.Id == Convert.ToInt32(stringId))
                                {
                                    MessageBox.Show(m.WholeString());
                                }
                            }
                        
                        
                        i = -1;
                    }
                }
                
            }
                        else
            {
                MessageBox.Show("Selecteer een Mediafile om te bekijken");
            }
            
        }

        private void btreport_Click(object sender, EventArgs e)
        {
        string Selectedtem = Convert.ToString(LbFeed.SelectedItem);
            
            if (LbFeed.SelectedItem != null)
            {
                for (int i = 10; i > 0; i--)
                {
                    stringId = Selectedtem.Substring(0, i);
                    if (stringId.IndexOf("-") == -1)
                    {
                        stringId = stringId.Substring(0, (i));
                        if(mediabeheer.MediafileRapporteren(Convert.ToInt32(stringId)))
                        {
                            MessageBox.Show("U heeft bericht met ID " + stringId + " Gereport");
                        }
                        else
                        {
                            MessageBox.Show("Er is iets fout gegaan bij het rapporteren van het mediafile! Probeer het opnieuw!");
                        }                        
                        i = -1;
                    }
                }
                
                
            }
            else
            {
                MessageBox.Show("Selecteer een Mediafile om te reporten!");
            }
            
        }

        private void btreply_Click(object sender, EventArgs e)
        {

        }

        private void btPost_Click(object sender, EventArgs e)
        {
            string username;
            int lastid = GetlatestID();
            /*if(chbBericht.Checked)
                {
                    Mediabeheer.Mediafile = new Mediabeheer.Mediafile(lastid + 1, Convert.ToString(tbTitel.Text), Convert.ToString(tbBericht.Text), "Bericht", Convert.ToString(cbCategorieAanmaken.Text), Convert.ToString(tbPath.Text), 0, 0, username);
                }*/
        }

        private int GetlatestID()
        {
            tempSoortList = mediabeheer.GetMediafileLijst;
            int aantalberichten = tempSoortList.Count;
            Mediabeheer.Mediafile lastmediafile = tempSoortList[aantalberichten - 1];
            int lastid = lastmediafile.Id;
            return lastid;

        }

        private void btBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.ShowDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                path = openFileDialog1.FileName;
                tbPath.Text = path;
            }


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

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void btnUitlenen_Click(object sender, EventArgs e)
        {
            string checkedGebruikersnaam = lbGasten.SelectedItem.ToString();
            checkedGebruikersnaam = checkedGebruikersnaam.Substring(0, checkedGebruikersnaam.IndexOf(" - Naam:"));
            DateTime uitleenDatum = DateTime.Now;
            DateTime retourDatum = uitleenDatum.AddDays(3);
            //string materiaalId = clbExemplaarHuren.SelectedItem.ToString();
            //materiaalId = materiaalId.Substring(3, materiaalId.Length - 4);
            //materiaalId = materiaalId.Substring(0, materiaalId.IndexOf(" - Borg:"));
            
            int maxId = Convert.ToInt32(Materiaalbeheer.AlleUitleningen().Max()) + 1;

                if (Materiaalbeheer.MateriaalHuren(maxId, DateTime.Now, retourDatum, checkedGebruikersnaam))
                    {
                        foreach (string s in clbExemplaarHuren.SelectedItems)
                        {
                            string id = s.Substring(3, s.Length - 4);
                            id = id.Substring(0, id.IndexOf(" - Borg:"));
                            materiaalbeheer.UpdateUitleningId(Convert.ToInt32(id), maxId);
                        }
                        MessageBox.Show("Uitlening toegevoegd.");
                    }

            List<string> exemplaren = new List<string>();
            
            
        }

        private void btnVerplaatsExemplaren_Click(object sender, EventArgs e)
        {
            foreach (string exemplaar in clbExemplaren.CheckedItems)
            {
                clbExemplaarHuren.Items.Add(exemplaar);
            }
        }

        private void btnTerugplaatsenExemplaar_Click(object sender, EventArgs e)
        {
            foreach (string item in clbExemplaarHuren.CheckedItems.OfType<string>().ToList())
            {
                clbExemplaarHuren.Items.Remove(item);
            }
        }

        private void btInloggen_Click(object sender, EventArgs e)
        {
            string Check = Gebruikerbeheer.Inloggen(tbGebruikersnaamInloggen.Text, tbWachtwoordInloggen.Text);
            if (Check == "Admin")
            {
                tabICT4Events.TabPages.Add(TabFeed);
                tabICT4Events.TabPages.Add(TabReserveren);
                tabICT4Events.TabPages.Add(TabHuren);
                tabICT4Events.TabPages.Add(TabBeheren);
                tabICT4Events.TabPages.Remove(TabInloggen);
                tabICT4Events.TabPages.Add(TabUitloggen);
            }
            else if (Check == "Gast")
            {
                tabICT4Events.TabPages.Add(TabFeed);
                tabICT4Events.TabPages.Add(TabHuren);
                tabICT4Events.TabPages.Add(TabUitloggen);
                tabICT4Events.TabPages.Remove(TabInloggen);
            }
            else if (Check == "Error")
            {

            }
        }
        private void btnUitloggen_Click(object sender, EventArgs e)
        {
            tabICT4Events.TabPages.Remove(TabFeed);
            tabICT4Events.TabPages.Remove(TabReserveren);
            tabICT4Events.TabPages.Remove(TabHuren);
            tabICT4Events.TabPages.Remove(TabUitloggen);
            tabICT4Events.TabPages.Remove(TabBeheren);
            tabICT4Events.TabPages.Add(TabInloggen);
        }

        //EventBeheer
    }
}
 
