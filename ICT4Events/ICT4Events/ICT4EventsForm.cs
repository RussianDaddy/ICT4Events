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
        private Mediabeheer.Mediabeheer mediabeheer;
        RFID rfid;

        string CheckisMediafile;
        private string stringId;
        private string sstringId;
        private string loggedinuser;
        private int LastID;
        private string GetMediaId;

        public ICT4EventsForm()
        {
            InitializeComponent();
            rbtnAllePlaasten.Checked = true;
            clbReserveringKampeerplaatsen.DataSource = reserveringBeheer.AllePlaatsen();
            clbReserveringGebruikers.DataSource = reserveringBeheer.AlleGebruikers();
            listboxReserveringen.DataSource = reserveringBeheer.AlleReserveringen();
            clbExemplaren.DataSource = materiaalbeheer.AlleExemplaren();

            //RFID
            rfid = new RFID();
            rfid.open();//Opent de verbinding
            rfid.Attach += new AttachEventHandler(rfid_Attach);//kijkt of de Reader verbonden is
            rfid.Detach += new DetachEventHandler(rfid_Detach);
            rfid.Tag += new TagEventHandler(rfid_Tag);//scant de rfid tag
            lbRfidStatus.Text = "RFID not Connected!";
            lbRfidStatus.ForeColor = System.Drawing.Color.Red;
            tmRFIDTextboxClear.Stop();

            dtpDatumAankomst.MinDate = DateTime.Today;
            dtpDatumVertrek.MinDate = DateTime.Today;
            lbBetaalstatus.Visible = false;
            tabICT4Events.TabPages.Remove(TabReserveren);
            tabICT4Events.TabPages.Remove(TabFeed);
            tabICT4Events.TabPages.Remove(TabHuren);
            tabICT4Events.TabPages.Remove(TabBeheren);
            tabICT4Events.TabPages.Remove(TabUitloggen);

            mediabeheer = new Mediabeheer.Mediabeheer();
            RefreshAll();
        }

        //RFID Methods
        //Controleert of de RFID scanner is gekoppeld
        private void rfid_Attach(object sender, AttachEventArgs e)
        {
            RFID attached = (RFID)sender;
            rfid.Antenna = true;
            lbRfidStatus.Text = "RFID Connected!";
            lbRfidStatus.ForeColor = System.Drawing.Color.Black;
        }

        //Leest het nummer van de RFID tag uit en vult deze in, in een textbox
        private void rfid_Tag(object sender, TagEventArgs e)
        {
            if (tabICT4Events.SelectedTab == tabICT4Events.TabPages["TabBeheren"])
            {
                tbRFIDnummBeheer.Text = e.Tag;
            }
            else if (tabICT4Events.SelectedTab == tabICT4Events.TabPages["TabHuren"])
            {
                tbRFIDMBeheer.Text = e.Tag;
            }
            rfid.LED = true;
            Thread.Sleep(100);
            rfid.LED = false;
        }

        //Controleert of de RFID scanner wordt losgekoppeld
        private void rfid_Detach(object sender, DetachEventArgs e)
        {
            RFID detached = (RFID)sender;
            lbRfidStatus.Text = "RFID not Connected!";
            lbRfidStatus.ForeColor = System.Drawing.Color.Red;
        }

        //Een timer de verschillende lijsten in het systeem om de 3 minuten ververst. Hierdoor wordt nieuwe data ingeladen
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
        //Met de ingevulde gegevens in het form wordt een nieuwe gebruiker aangemaakt
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

        //Zodra er in de textbox voor het RFID nummer text wordt gewijzigd, wordt er meteen gecontroleerd of deze gebruiker toegang heeft tot het terrein
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

        //Een timer die de textbox voor het RFID na een bepaalde tijd leeghaald
        private void tmRFIDTextboxClear_Tick(object sender, EventArgs e)
        {
            tbRFIDnummBeheer.Text = "";
            pnIncheckBeheer.BackColor = System.Drawing.Color.LightGray;
            tmRFIDTextboxClear.Stop();
        }

        //Als er op de knop wijzigen geklikt wordt, worden de nieuwe gegevens van de gebruiker uit de textboxen gehaald en gewijzigd in de databse
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

        //Methode die de huidige gegevens van de selecteerde gebruiker in de lijst ophaald uit de database
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

        //Als het op de button Laat Zien wordt geklikt wordt de methode FilllbGebruikerBeheer aangeroepen
        private void btnLaatZienBeheren_Click_1(object sender, EventArgs e)
        {
            FillLbGebruikerBeheer();
        }

        //Vult de lijst met de gebruikersgegevens uit de database of met gegevens van alle aanwezige op het terrein
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

        //Controleert of de index verandert van de lijst met gebruikers
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

        //Controleerd de betaling van de geselecteerde gebruiker in de lijst. Als de gebruiker betaald heeft verschijnt er BETAALD en wordt deze teskt groen
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
        /// <summary>
        /// Als er op de knop Post wordt geklikt, wordt er gekeken of er sprake is van een Mediafile (een originele post) of een reactie (een post op een mediafile).
        /// vervolgens worden er in beide gevallen de juiste data uitgelezen uit de velden en meegegeven aan de methodes in MediaBeheer. Het id van de post wordt automatisch bepaald zodat er geen conflicten  kunnen komen.
        /// Als er niet voldaan wordt aan de juiste requirements worden er duidelijke messages gegevens die instructies geven wat te doen om de actie succesvol af te ronden.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPost_Click(object sender, EventArgs e)
        {
            if (rbReply.Checked)
            {
                string replyItem = Convert.ToString(LbFeed.SelectedItem);
                if (LbFeed.SelectedItem != null)
                {

                    CheckisMediafile = replyItem.Substring(0, 3);
                }
                else
                {
                    MessageBox.Show("selecteer een bericht!");
                }

                if (LbFeed.SelectedItem != null && CheckisMediafile != "Rea" && replyItem.Substring(0, 3) != "ID:")
                {
                    for (int i = 10; i > 0; i--)
                    {
                        stringId = replyItem.Substring(0, i);
                        if (stringId.IndexOf("-") == -1)
                        {
                            stringId = stringId.Substring(0, (i));
                            i = -1;
                        }
                    }
                    int maxreactieid = 0;
                    foreach (Mediabeheer.Reactie r in mediabeheer.GetReactieLijst)
                    {
                        if (r.ID > maxreactieid)
                        {
                            maxreactieid = r.ID;
                        }
                    }
                    int MediafileID = Convert.ToInt32(stringId);
                    int ReactieId = maxreactieid + 1;
                    if (!mediabeheer.ReactiePlaatsen(ReactieId, MediafileID, tbBericht.Text, loggedinuser))
                    {
                        MessageBox.Show("Er is iets fout gegaan bij het plaatsen van je reactie, probeer het opnieuw!");
                    }
                    else
                    {
                        MessageBox.Show("Je reactie is succesvol gepost!");
                        RefreshAll();
                    }
                }
                else
                {
                    MessageBox.Show("Selecteer eerst een origineel bericht om op te reageren");
                }
                mediabeheer.Update();
                RefreshAll();
            }
            else
            {
                string soort;
                if (rbBerichtAanmaken.Checked)
                {
                    soort = " Bericht";
                }
                else if (rbBestandAanmaken.Checked)
                {
                    soort = " Bestand";
                }
                else if (rbEventAanmaken.Checked)
                {
                    soort = " Event";
                }
                else if (rbFotoAanmaken.Checked)
                {
                    soort = " Foto";
                }
                else if (rbVideoAanmaken.Checked)
                {
                    soort = " Video";
                }
                else
                {
                    soort = "1";
                }
                if (soort == "1")
                {
                    MessageBox.Show("Selecteer een soort post!");
                }
                else if (cbCategorieAanmaken.Text == "")
                {
                    MessageBox.Show("Selecteer eerst een categorie!");
                }
                else
                {
                    LastID = GetlatestID();
                    string categorie = cbCategorieAanmaken.Text;
                    if (mediabeheer.BerichtPlaatsen(LastID + 1, loggedinuser, Convert.ToString(tbTitel.Text), Convert.ToString(tbBericht.Text), Convert.ToString(soort), categorie, tbPath.Text, 0, 0) == true)
                    {
                        MessageBox.Show("Het bericht is succesvol gepost!");
                    }
                    else
                    {
                        MessageBox.Show("Er is iets misgegaan bij het posten van uw bericht, probeer het opnieuw!");
                    }
                }
                mediabeheer.Update();
                RefreshAll();
            }
        }

        /// <summary>
        /// De knop view post maakt het mogelijk om de post die geselcteerd is in de listbox te bekijken in een overzichtelijke messagebox. hier wordt alle informatie in gezet zoals wie het heeft geschreven, hoeveel likes het heeft en wat de titel is. 
        /// Dit kan ook met reacties op posts. hierbij krijg je dan het originele bericht te zien met al zijn informatie, maar je krijgt ook de reactie te zien. Id's worden bepaald door middel van substrings ( vandaar de vele if's en de for loops)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btviewpost_Click(object sender, EventArgs e)
        {
            string Selectedtem = Convert.ToString(LbFeed.SelectedItem);
            if (LbFeed.SelectedItem != null && Selectedtem.Substring(0, 3) != "Rea")
            {
                for (int i = 10; i > 0; i--)
                {
                    stringId = Selectedtem.Substring(0, i);
                    if (stringId.IndexOf("-") == -1)
                    {
                        stringId = stringId.Substring(0, (i));
                        if (Selectedtem.Substring(0, 4) == "ID: ")
                        {
                            int RID = Convert.ToInt32(stringId.Substring(4, stringId.Length - 4));
                            foreach (Mediabeheer.Reactie r in mediabeheer.GetReactieLijst)
                            {
                                if (r.ID == Convert.ToInt32(RID))
                                {
                                    for (int y = 10; y > 0; y--)
                                    {
                                        sstringId = Selectedtem.Substring(Selectedtem.IndexOf("d"), y);
                                        if (sstringId.IndexOf(",") == -1)
                                        {
                                            string grg = Selectedtem.Substring(3, y - 3);
                                            GetMediaId = grg.Substring(1, grg.Length - 1);
                                            y = -1;
                                        }
                                    }
                                    MessageBox.Show(mediabeheer.GetWholeReactieString(RID, Convert.ToInt32(GetMediaId)));
                                }
                            }
                        }
                        else
                        {
                            foreach (Mediabeheer.Mediafile m in mediabeheer.GetMediafileLijst)
                            {
                                if (m.Id == Convert.ToInt32(stringId))
                                {
                                    MessageBox.Show(m.WholeString());
                                }
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

        /// <summary>
        /// wanneer er op de knop like wordt geklikt wordt er eerst bepaald welk bericht is geselecteerd. Als het een origineel Mediafile is dan is het mogelijk om deze te liken. 
        /// het desbetreffende mediaid wordt meegestuurd naar MediaBeheer en hier wordt de likecounte voor dit object opgehoogd.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btlike_Click(object sender, EventArgs e)
        {
            string Selectedtems = Convert.ToString(LbFeed.SelectedItem);
            if (LbFeed.SelectedItem != null && Selectedtems.Substring(0, 3) != "Rea" && Selectedtems.Substring(0, 3) != "ID:")
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
                MessageBox.Show("Selecteer eerst een bericht om te liken!");
            }

        }

        /// <summary>
        /// Wanneer je op report klikt wordt er bepaald welk item er is geselecteerd en vervolgens wordt de id doorgestuurd naar MediaBeheer.
        /// Hier wordt de report counter opgehoogt.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btreport_Click(object sender, EventArgs e)
        {
            string Selectedtem = Convert.ToString(LbFeed.SelectedItem);

            if (LbFeed.SelectedItem != null && Selectedtem.Substring(0, 3) != "Rea" && Selectedtem.Substring(0, 3) != "ID:")
            {
                for (int i = 10; i > 0; i--)
                {
                    stringId = Selectedtem.Substring(0, i);
                    if (stringId.IndexOf("-") == -1)
                    {
                        stringId = stringId.Substring(0, (i));
                        if (mediabeheer.MediafileRapporteren(Convert.ToInt32(stringId)))
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
                MessageBox.Show("Selecteer een origineel mediafile om te reporten!");
            }
        }

        /// <summary>
        /// Als er op de filter knop wordt gedrukt wordt er gecontroleerd welke checkboxen met soorten berichten zijn aangevinkt, en vervolgens word er een string gestuurd naar MediaBeheer.
        /// In deze string staan achter elkaar alle Soorten in tekst. Als er berichten zijn met dit soort als attribuut eworden deze berichten weer gegeven.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btFilter_Click(object sender, EventArgs e)
        {
            if (chbBericht.Checked == false && chbBestand.Checked == false && chbEvent.Checked == false &&
                chbFoto.Checked == false && chbVideo.Checked == false)
            {
                LbFeed.Items.Clear();
                foreach (Mediabeheer.Mediafile m in mediabeheer.GetMediafileLijst)
                {
                    if (m.Report == 0)
                    {
                        LbFeed.Items.Add(m.ToString());
                    }
                }
            }
            else
            {
                LbFeed.Items.Clear();
                searchstring = "";
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
                tempSoortList = mediabeheer.Filteren(searchstring);
                foreach (Mediabeheer.Mediafile m in tempSoortList)
                {
                    if (m.Report == 0)
                    {
                        LbFeed.Items.Add(m.ToString());
                    }
                }
                tempSoortList = null;
                mediabeheer.SearchedSoortLijst = null;
                mediabeheer.SearchedSoortLijst = new List<Mediabeheer.Mediafile>();
            }
        }

        /// <summary>
        /// Deze knop heft alle toegepaste filters weer op zodat alle berichten worden laten zien.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btShowAll_Click(object sender, EventArgs e)
        {
            RefreshAll();
            chbBericht.Checked = false;
            chbBestand.Checked = false;
            chbEvent.Checked = false;
            chbFoto.Checked = false;
            chbVideo.Checked = false;
        }

        /// <summary>
        /// Deze knop laat je een bestand op je computer selecteren en haalt het pad op zodat de file uiteindelijk gestuurd kan worden naar de server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter =
                "jpg image (*.jpg)|*.jpg|jpeg image (*.jpeg)|*.jpeg|gif image (*.gif)|*.gif|png image (*.png)|*.png|wmv video (*.wmv)|*.wmv|mp4 video (*.mp4)|*.mp4|txt files (*.txt)|*.txt";
            openFileDialog.Title = "Upload een mediafile";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK & openFileDialog.FileName != "")
            {
                string bestandPath = openFileDialog.FileName;
                string extenion = Path.GetExtension(openFileDialog.FileName);
                string bestandsnaam = openFileDialog.SafeFileName;
                if (extenion.Equals(".jpg") || extenion.Equals(".JPG") || extenion.Equals(".jpeg") || extenion.Equals(".gif") || extenion.Equals(".png"))
                {
                    mediabeheer.UploadenMedia(bestandPath, bestandsnaam, 1);
                    MessageBox.Show("Uw Afbeelding is succesvol geupload naar de server!");
                }
                if (extenion.Equals(".wmv") || extenion.Equals(".mp4"))
                {
                    mediabeheer.UploadenMedia(bestandPath, bestandsnaam, 2);
                    MessageBox.Show("Uw Video is succesvol geupload naar de server!");
                }
                if (extenion.Equals(".txt"))
                {
                    mediabeheer.UploadenMedia(bestandPath, bestandsnaam, 3);
                    MessageBox.Show("Uw Bestand is succesvol geupload naar de server!");
                }
            }
        }

        /// <summary>
        /// Maakt het mogelijk om een bestand wat op de servers is geupload te downloaden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btdownloadfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Download een mediafile";
            openFileDialog.Filter =
                "jpg image (*.jpg)|*.jpg|jpeg image (*.jpeg)|*.jpeg|gif image (*.gif)|*.gif|png image (*.png)|*.png|wmv video (*.wmv)|*.wmv|mp4 video (*.mp4)|*.mp4|txt files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = @"\\SRV2\";
            if (openFileDialog.ShowDialog() == DialogResult.OK & openFileDialog.FileName != "")
            {
                string bestandPath = openFileDialog.FileName;
                string bestandsnaam = openFileDialog.SafeFileName;
                if (mediabeheer.DownloadenMedia(bestandPath, bestandsnaam))
                {
                    MessageBox.Show("Bestand is opgeslagen in: " + @"C:\Users\Public\ICT4Events");
                }
            }
        }

        /// <summary>
        /// Deze methode haalt de laatste Id op van de mediafiles zodat er bij het posten een object aangemaakt kan worden met 1 hoger (auto increment)
        /// </summary>
        /// <returns></returns>
        private int GetlatestID()
        {
            tempSoortList = mediabeheer.GetMediafileLijst;
            int aantalberichten = tempSoortList.Count;
            Mediabeheer.Mediafile lastmediafile = tempSoortList[aantalberichten - 1];
            int lastid = lastmediafile.Id;
            return lastid;
        }

        /// <summary>
        /// Deze methode haalt de feed en comboboxen leeg en vult ze opnieuw met de meest recente gegevens uit de lijsten (die zijn bijgewerkt uit de database)
        /// </summary>
        public void RefreshAll()
        {
            mediabeheer.Update();
            LbFeed.Items.Clear();
            foreach (Mediabeheer.Mediafile m in mediabeheer.GetMediafileLijst)
            {
                LbFeed.Items.Add(m.ToString());
            }
            LbFeed.Items.Add("Reacties op posts...");
            foreach (Mediabeheer.Reactie r in mediabeheer.GetReactieLijst)
            {
                LbFeed.Items.Add(r.ToString());
            }
            cbCategorieAanmaken.Items.Clear();
            foreach (Mediabeheer.Categorie c in mediabeheer.GetCategorieLijst)
            {
                cbCategorieAanmaken.Items.Add(c.ToString());
            }
        }

        /// <summary>
        /// Disabled Titel textbox pad textbox en de combobox voor het selecteren van een categorie wanneer dit niet van toepassing is bij het geselecteerde soort.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbReply_CheckedChanged(object sender, EventArgs e)
        {
            if (rbReply.Checked)
            {
                tbTitel.Enabled = false;
                tbPath.Enabled = false;
                cbCategorieAanmaken.Enabled = false;
                btBrowse.Enabled = false;
            }
            else
            {
                tbTitel.Enabled = true;
                tbPath.Enabled = true;
                cbCategorieAanmaken.Enabled = true;
                btBrowse.Enabled = true;
            }
        }

        /// <summary>
        /// Disabled pad textbox wanneer dit niet van toepassing is bij het geselecteerde soort.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbEventAanmaken_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEventAanmaken.Checked)
            {
                tbPath.Enabled = false;
                btBrowse.Enabled = false;
            }
            else
            {
                tbTitel.Enabled = true;
                tbPath.Enabled = true;
                cbCategorieAanmaken.Enabled = true;
                btBrowse.Enabled = true;
            }
        }

        /// <summary>
        /// Disabled pad textbox wanneer dit niet van toepassing is bij het geselecteerde soort.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbBerichtAanmaken_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBerichtAanmaken.Checked)
            {
                tbPath.Enabled = false;
                btBrowse.Enabled = false;
            }
            else
            {
                tbTitel.Enabled = true;
                tbPath.Enabled = true;
                cbCategorieAanmaken.Enabled = true;
                btBrowse.Enabled = true;
            }
        }

        //ReserveringBeheer
        /// <summary>
        /// Dit is de methode die aangeroepen wordt al er op de knop Reserveer wordt geklikt, de reservering wordt uiteindelijk in de database opgeslagen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btReserveer_Click(object sender, EventArgs e)
        {
            DateTime aankomstDatum = dtpDatumAankomst.Value.Date; //de aankomstdatum is de waarde uit de DateTimePicker
            DateTime vertrekdatum = dtpDatumVertrek.Value.Date; //de vertrekdatum is de waarde uit de DateTimePicker
            int betaald = 0;
            //controleert of de checkbox Betaald is aangevinkt
            if (cbBetaald.Checked)
            {
                betaald = 1;
            }
            //Alle kampeerplaatsnummers van de aangevinkte plaatsen worden in een lijst geplaatst. En het aantal personen kan kan verblijven in deze plaatsen wordt opgeslagen in aantalpersonen
            List<String> kampeerPlaatsen = new List<string>();
            int aantalPersonen = 0;
            foreach (String kampeerplaats in clbReserveringKampeerplaatsen.CheckedItems)
            {
                aantalPersonen = aantalPersonen + Convert.ToInt32(kampeerplaats.Substring(kampeerplaats.Length - 1, 1));
                string kampeerplaatsNummer = kampeerplaats.Substring(8);
                kampeerplaatsNummer = kampeerplaatsNummer.Substring(0, kampeerplaatsNummer.IndexOf(" Soort:"));
                kampeerPlaatsen.Add(kampeerplaatsNummer);
            }
            //Alle gebruikersnamen van de aangevinkte gebruikers worden in een lijst geplaatst.
            List<String> gebruikersnamen = new List<string>();
            foreach (String gegevens in clbReserveringGebruikers.CheckedItems)
            {
                string gebruikersnaam = gegevens.Substring(11);
                gebruikersnaam = gebruikersnaam.Substring(0, gebruikersnaam.IndexOf(" Naam:"));
                gebruikersnamen.Add(gebruikersnaam);
            }
            string hoofdboeker = gebruikersnamen.First(); //De eerste gebruikersnaam in de lijst wordt de hoofdboeker
            if (aantalPersonen > gebruikersnamen.Count)
            {
                if (reserveringBeheer.Reserveren(hoofdboeker, aankomstDatum, vertrekdatum, betaald))
                {
                    gebruikersnamen.Remove(hoofdboeker);
                    MessageBox.Show("Reservering Toegevoegd");
                    string reserveringsNummer = reserveringBeheer.VindReserveringNummer(hoofdboeker,
                        aankomstDatum, vertrekdatum);
                    //Als er na de hoofdboeker nog steeds gebruikersnamen is de lijst voorkomen, worden deze toegevoegd als medereiziger
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
                    //Alle kampeerplaatsen worden toegevoegd aan de reservering
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

        //Als de radiobutton Alle Kampeerplaatsen wordt geselecteerd worden alle kampeerplaatsen opgevraagd
        private void rbtnAllePlaasten_CheckedChanged(object sender, EventArgs e)
        {
            clbReserveringKampeerplaatsen.DataSource = reserveringBeheer.AllePlaatsen();
        }

        //Als de radiobutton Specifieke Kampeerplaatsen wordt geselecteerd worden alle kampeerplaatsen met eigenschappen opgevraagd
        private void rbtnSpecifiekePlaatsen_CheckedChanged(object sender, EventArgs e)
        {
            clbReserveringKampeerplaatsen.DataSource = reserveringBeheer.AlleSpecifiekePlaatsen();
        }

        //Als de radiobutton Vrije Kampeerplaasten wordt geselecteerd wordt geselecteerd worden alle kamperplaatsen opgevraagd waar de vertrekdatum in het verleden ligt
        private void rbtnVrijePlaatsen_CheckedChanged(object sender, EventArgs e)
        {
            clbReserveringKampeerplaatsen.DataSource = reserveringBeheer.AlleVrijePlaatsen();
        }

        //Deze methode wordt aangeroepen als er op de button Betaald wordt geklikt
        private void btnReserveringBetaald_Click(object sender, EventArgs e)
        {
            string reservering = listboxReserveringen.SelectedItem.ToString(); //De selecteerde reservering uit de lijst wordt opgeslagen
            reservering = reservering.Substring(8);
            reservering = reservering.Substring(0, reservering.IndexOf(" Naam:"));
            //In de database wordt de betaling bijgewerkt aan de hand van het reserveringsnummer
            if(reserveringBeheer.UpdateBetaling(reservering))
            {
                MessageBox.Show("Reservering: " + reservering + " is betaald");
            }
            else
            {
                MessageBox.Show("Betaling kan niet worden gewijzigd");
            }
        }

        //Als de datum in de DateTimePicker vna de aankomstdatum veranderd wordt dat de minimum datum van de vertrekdatum
        private void dtpDatumVan_ValueChanged(object sender, EventArgs e)
        {
            DateTime reservatievan;
            reservatievan = dtpDatumAankomst.Value;
            dtpDatumVertrek.MinDate = reservatievan;
            dtpDatumVertrek.Refresh();
        }

        //MateriaalBheer
        /// <summary>
        /// Kijkt of het ingevuld ID gelijk is aan het ID van het materiaal in de database.
        /// </summary>
        private void btnZoekExemplaar_Click(object sender, EventArgs e)
        {
            
                int maxId = Convert.ToInt32(Materiaalbeheer.GetMaxExemplaar().Max());
                if (tbExemplaarId.Text == "")
                {
                    clbExemplaren.DataSource = materiaalbeheer.AlleExemplaren();
                }
                else if (Convert.ToInt32(tbExemplaarId.Text) <=  maxId)
                {
                    clbExemplaren.DataSource = null;
                    clbExemplaren.DataSource = Materiaalbeheer.ZoekMateriaal(tbExemplaarId.Text);
                }
                else
                {
                    MessageBox.Show("Het ID wat je hebt ingevuld bestaat niet");
                    clbExemplaren.DataSource = materiaalbeheer.AlleExemplaren();
                }
        }

        /// <summary>
        /// Berekent de totale borg in de list box met exemplaren die verhuurd gaan worden.
        /// </summary>
        private void btnTotaleBorg_Click(object sender, EventArgs e)
        {
            int totaleBorg = 0;
            foreach (string s in clbExemplaarHuren.Items)
            {
                string id = s.Substring(3, s.Length - 4);
                id = id.Substring(0, id.IndexOf(" - Borg:"));
                totaleBorg += Materiaalbeheer.GetTotaleBorg(id).Sum(x => Convert.ToInt32(x));
            }
            lblBorg.Text = "\u20AC" + totaleBorg;
        }

        private void btnKoppelMateriaalBeheer_Click(object sender, EventArgs e)
        {
            if(tbRFIDMBeheer.Text == "" || tbGebruikersnaamMBeheer.Text == "")
            {
                MessageBox.Show("Vul geldige informatie in.");
            }
            else if(materiaalbeheer.UitgevenRFID(tbGebruikersnaamMBeheer.Text, tbRFIDMBeheer.Text))
            {
                MessageBox.Show("RFID is gekoppeld.");
            }
            else
            {
                MessageBox.Show("Koppelen is niet gelukt.");
            }
        }

        /// <summary>
        /// Gebruikt het volgende uitleningId en update voor elk gecheckte item in de 
        /// checked list box. De datum is het moment dat er op de knop wordt gedrukt. 
        /// De retourdatum is drie dagen daarna.
        /// </summary>
        private void btnUitlenen_Click(object sender, EventArgs e)
        {
            DateTime uitleenDatum = DateTime.Now;
            DateTime retourDatum = uitleenDatum.AddDays(3);

            int maxId = Convert.ToInt32(Materiaalbeheer.AlleUitleningen().Max()) + 1;
            
            if (Materiaalbeheer.MateriaalHuren(maxId, DateTime.Now, retourDatum, tbGebruikersnaamMBeheer.Text))
            {
                foreach (string s in clbExemplaarHuren.CheckedItems)
                {
                    string id = s.Substring(3, s.Length - 4);
                    id = id.Substring(0, id.IndexOf(" - Borg:"));
                    materiaalbeheer.UpdateUitleningId(Convert.ToInt32(id), maxId);
                }
                MessageBox.Show("Uitlening toegevoegd.");
            }
        }

        /// <summary>
        /// Elk aangevinkt exemplaar in de list box met exemplaren wordt ook in de lijst
        /// met exemplaren die verhuurd worden gezet. Als het exemplaar al in de lijst staat
        /// krijgt de gebruiker een error.
        /// </summary>
        private void btnVerplaatsExemplaren_Click(object sender, EventArgs e)
        {
            List<string> stringList = new List<string>();
            foreach (string exemplaarH in clbExemplaarHuren.Items)
            {
                stringList.Add(exemplaarH);
            }

            foreach (string exemplaar in clbExemplaren.CheckedItems)
            {
                    if (!stringList.Contains(exemplaar))
                    {   
                        clbExemplaarHuren.Items.Add(exemplaar);
                    }
                    else
                    {
                        MessageBox.Show("Dit exemplaar staat al in de lijst van de exemplaren die verhuurd worden.");
                    }
            }

            foreach (int i in clbExemplaren.CheckedIndices)
            {
                clbExemplaren.SetItemCheckState(i, CheckState.Unchecked);
            }
        }

        /// <summary>
        /// Verwijdert elk aangevinkt exemplaar uit de lijst.
        /// </summary>
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
                loggedinuser = tbGebruikersnaamInloggen.Text;
            }
            else if (Check == "Gast")
            {
                tabICT4Events.TabPages.Add(TabFeed);
                tabICT4Events.TabPages.Add(TabUitloggen);
                tabICT4Events.TabPages.Remove(TabInloggen);
                loggedinuser = tbGebruikersnaamInloggen.Text;
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
    }
}
 
