using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Windows.Forms;
//database
using Oracle.DataAccess.Client;

namespace ICT4Events
{
    public partial class ICT4EventsForm : Form
    {
        //private Enum e = new Enum;

        public ICT4EventsForm()
        {
            InitializeComponent();
            dtpDatumAankomst.MinDate = DateTime.Today;
            dtpDatumVertrek.MinDate = DateTime.Today;
            var oneYearAgoToday = DateTime.Now.AddYears(-18);
<<<<<<< HEAD
            dtpGeboorteDatum.MaxDate = oneYearAgoToday;
            /*
=======
>>>>>>> origin/master
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
<<<<<<< HEAD
         */
=======

        private void btnAanmakenBeheer_Click(object sender, EventArgs e)
        {

        }
>>>>>>> origin/master
    }
}