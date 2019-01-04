using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace rfidClient
{
    public partial class LoginFrm : Form
    {
        public LoginFrm()
        {
            InitializeComponent();
        }

        private void LoginFrm_Load(object sender, EventArgs e)
        {
            this.KeyDown += new KeyEventHandler(LoginFrm_KeyDown);
			
            Int32 pr = 1;

            while (true)
            {
                pr += pr * 5;
                if (pr >= 100) pr = 0;
                progressBar.Value = pr;
                Application.DoEvents();
            }
        }

        private void Timer_Elapsed(object sender, EventArgs e)
        {
            if (lblUyari.BackColor == Color.White)
            {
                lblUyari.BackColor = Color.White;
            }
        }

        private void LoginFrm_KeyDown(object sender, KeyEventArgs e) 
        {

            if (e.KeyCode == Keys.Enter) {
                GirisYap();
            }

        }

        private void GirisYap() {
            Login.Kullanici = txtKulad.Text;
            Login.Parola = txtParola.Text;
			
            if (DB.GirisYap())
            {
                Login.Basarili = true;

                if (Login.MisafirEkle)
                {
                    Login.MisafirEkle = false;
                    NewUser newUserFrm = new NewUser();
                    newUserFrm.Show();
                }

                if (Login.MisafirSil)
                {
                    Login.MisafirSil = false;
                    DelUser delUser = new DelUser();
                    delUser.Show();
                }

                if (Login.Settings)
                {
                    Login.Settings = false;
                    Settings.AdminMod = true;
                    SettingsFrm settingsFrm = new SettingsFrm();
                    settingsFrm.Show();
                }

                if (Login.Cikis)
                {
                    Login.Cikis = false;
                    Application.Exit();
                }

                this.Hide();
            }
            else
            {
                Login.Basarili = false;
                MessageBox.Show("Giriş yetkiniz yok!");
            }
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            GirisYap();  
        }

        private void btnShutdown_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

    }
}
