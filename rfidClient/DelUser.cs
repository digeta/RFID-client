using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rfidClient
{
    public partial class DelUser : Form
    {
        Kart kart;
        private Timer timer;

        public DelUser()
        {
            InitializeComponent();
        }

        private void btnAra_Click(object sender, EventArgs e)
        {
            if (txtKimlik.Text != "")
            {
                Int64 tckimlik = 0;
                Int64.TryParse(txtKimlik.Text, out tckimlik);
                if (tckimlik > 0)
                {
                    Kart karte = new Kart();
                    karte = DB.MisafirGetir(Convert.ToInt64(txtKimlik.Text));
                    txtAd.Text = karte.Ad;
                    txtSoyad.Text = karte.Soyad;
                }
                else
                {
                    lblBilgi.Text = "Hatalı T.C. Kimlik";
                }
            }
            else
            {
                lblBilgi.Text = "T.C. Kimlik alanını doldurun";
            }
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (DB.MisafirSil(kart.Kartno))
            {
                lblBilgi.Text = "Misafir kaydı silindi";
            }
            else
            {
                lblBilgi.Text = "Misafir kaydı silinemedi";
            }
        }

        private void DelUser_Load(object sender, EventArgs e)
        {
            this.FormClosing += new FormClosingEventHandler(OnClose);

            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            kart = new Kart();

            KartOkundu.DelUser = true;            
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            kart.Kartno = KartOkundu.KartID;
            lblKartNo2.Text = Convert.ToString(kart.Kartno);
            txtAd.Text = KartOkundu.Ad;
            txtSoyad.Text = KartOkundu.Soyad;
            txtKimlik.Text = Convert.ToString(kart.Tckimlik);
        }

        private void OnClose(object sender, EventArgs e)
        {
            KartOkundu.DelUser = false;
            KartOkundu.TCKimlik = 0;
            KartOkundu.Ad = "";
            KartOkundu.Soyad = "";
            KartOkundu.KartID = 0;
        }
    }
}
