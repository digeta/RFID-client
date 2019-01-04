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
    public partial class NewUser : Form
    {
        public delegate void deleg(String data);
        Kart kart;
        private Timer timer;

        public NewUser()
        {
            InitializeComponent();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {                
                if ( kart.Kartno != 0 )
                {
                    if (txtAd.Text != "" && txtSoyad.Text != "")
                    {
                        kart.Ad = txtAd.Text;
                        kart.Soyad = txtSoyad.Text;
                        kart.Tckimlik = Convert.ToInt64(txtKimlik.Text);
                        kart.Kaydeden = Login.ID;

                        if (Login.Basarili)
                        {
                            if (DB.MisafirEkle(kart))
                            {
                                lblBilgi.Text = "Misafir kaydı yapıldı";
                            }
                            else
                            {
                                lblBilgi.Text = "Kart kullanımda veya geçersiz kart";
                            }
                        }
                    }
                    else
                    {
                        lblBilgi.Text = "Bilgi alanlarını lütfen doldurun";
                    }                    
                }
                else
                {
                    lblBilgi.Text = "Kart numarası geçersiz. Tekrar okutun";
                }
            }
            catch (Exception ex)
            {
                if (Settings.AdminMod)
                {
                    lblBilgi.Text = "Hatalı bilgi girdiniz! | " + ex.Message;
                }
                else
                {
                    lblBilgi.Text = "Hatalı bilgi girdiniz!";
                }                
            }            
        }

        private void NewUser_Load(object sender, EventArgs e)
        {
            Durumlar.OnErrorChanged += new EventHandler<DurumArgsHata>(OnErrorChanged);
            Durumlar.OnMessageChanged += new EventHandler<DurumArgsMessage>(OnMessageChanged);
            
            this.FormClosing += new FormClosingEventHandler(OnClose);
                        
            timer = new Timer();
            timer.Interval = 100;
            timer.Tick +=new EventHandler(timer_Tick);
            timer.Start();

            kart = new Kart();
            KartOkundu.NewUser = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            kart.Kartno = KartOkundu.KartID;
        }

        private void OnClose(object sender, EventArgs e)
        {
            KartOkundu.NewUser = false;
            KartOkundu.TCKimlik = 0;
            KartOkundu.Ad = "";
            KartOkundu.Soyad = "";
            KartOkundu.KartID = 0;
        }

        private void OnMessageChanged(Object obj, DurumArgsMessage infoArgs)
        {
        }

        private void OnErrorChanged(Object obj, DurumArgsHata infoArgs)
        {
        }

        private void OnKartOkundu(Object obj, KartOkunduArgs kartOkunduArgs)
        {
        }

        private void Yaz(String yazData)
        {
        }
    }
}
