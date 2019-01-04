namespace rfidClient
{
    partial class LoginFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGiris = new System.Windows.Forms.Button();
            this.grpLogin = new System.Windows.Forms.GroupBox();
            this.lblWarn2 = new System.Windows.Forms.Label();
            this.lblWarn1 = new System.Windows.Forms.Label();
            this.txtParola = new System.Windows.Forms.TextBox();
            this.lblParola = new System.Windows.Forms.Label();
            this.txtKulad = new System.Windows.Forms.TextBox();
            this.lblKulad = new System.Windows.Forms.Label();
            this.btnShutdown = new System.Windows.Forms.Button();
            this.imgKart = new System.Windows.Forms.PictureBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblUyari = new System.Windows.Forms.Label();
            this.grpLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgKart)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGiris
            // 
            this.btnGiris.BackColor = System.Drawing.Color.Snow;
            this.btnGiris.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGiris.ForeColor = System.Drawing.Color.Black;
            this.btnGiris.Location = new System.Drawing.Point(393, 589);
            this.btnGiris.Name = "btnGiris";
            this.btnGiris.Size = new System.Drawing.Size(245, 107);
            this.btnGiris.TabIndex = 6;
            this.btnGiris.Text = "Giriş Yap";
            this.btnGiris.UseVisualStyleBackColor = false;
            this.btnGiris.Click += new System.EventHandler(this.btnGiris_Click);
            // 
            // grpLogin
            // 
            this.grpLogin.BackColor = System.Drawing.Color.Transparent;
            this.grpLogin.Controls.Add(this.lblWarn2);
            this.grpLogin.Controls.Add(this.lblWarn1);
            this.grpLogin.Controls.Add(this.txtParola);
            this.grpLogin.Controls.Add(this.lblParola);
            this.grpLogin.Controls.Add(this.txtKulad);
            this.grpLogin.Controls.Add(this.lblKulad);
            this.grpLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.grpLogin.ForeColor = System.Drawing.Color.White;
            this.grpLogin.Location = new System.Drawing.Point(395, 399);
            this.grpLogin.Name = "grpLogin";
            this.grpLogin.Size = new System.Drawing.Size(490, 184);
            this.grpLogin.TabIndex = 7;
            this.grpLogin.TabStop = false;
            this.grpLogin.Text = "Giriş";
            // 
            // lblWarn2
            // 
            this.lblWarn2.AutoSize = true;
            this.lblWarn2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblWarn2.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblWarn2.Location = new System.Drawing.Point(464, 106);
            this.lblWarn2.Name = "lblWarn2";
            this.lblWarn2.Size = new System.Drawing.Size(23, 31);
            this.lblWarn2.TabIndex = 12;
            this.lblWarn2.Text = "!";
            this.lblWarn2.Visible = false;
            // 
            // lblWarn1
            // 
            this.lblWarn1.AutoSize = true;
            this.lblWarn1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblWarn1.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblWarn1.Location = new System.Drawing.Point(464, 50);
            this.lblWarn1.Name = "lblWarn1";
            this.lblWarn1.Size = new System.Drawing.Size(23, 31);
            this.lblWarn1.TabIndex = 10;
            this.lblWarn1.Text = "!";
            this.lblWarn1.Visible = false;
            // 
            // txtParola
            // 
            this.txtParola.Location = new System.Drawing.Point(198, 103);
            this.txtParola.MaxLength = 20;
            this.txtParola.Name = "txtParola";
            this.txtParola.PasswordChar = '*';
            this.txtParola.Size = new System.Drawing.Size(264, 35);
            this.txtParola.TabIndex = 11;
            // 
            // lblParola
            // 
            this.lblParola.AutoSize = true;
            this.lblParola.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblParola.Location = new System.Drawing.Point(18, 106);
            this.lblParola.Name = "lblParola";
            this.lblParola.Size = new System.Drawing.Size(110, 29);
            this.lblParola.TabIndex = 10;
            this.lblParola.Text = "Parola : ";
            // 
            // txtKulad
            // 
            this.txtKulad.Location = new System.Drawing.Point(198, 47);
            this.txtKulad.MaxLength = 20;
            this.txtKulad.Name = "txtKulad";
            this.txtKulad.Size = new System.Drawing.Size(264, 35);
            this.txtKulad.TabIndex = 9;
            // 
            // lblKulad
            // 
            this.lblKulad.AutoSize = true;
            this.lblKulad.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblKulad.Location = new System.Drawing.Point(18, 50);
            this.lblKulad.Name = "lblKulad";
            this.lblKulad.Size = new System.Drawing.Size(179, 29);
            this.lblKulad.TabIndex = 8;
            this.lblKulad.Text = "Kullanıcı Adı : ";
            // 
            // btnShutdown
            // 
            this.btnShutdown.BackColor = System.Drawing.Color.Crimson;
            this.btnShutdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnShutdown.ForeColor = System.Drawing.Color.Black;
            this.btnShutdown.Location = new System.Drawing.Point(642, 589);
            this.btnShutdown.Name = "btnShutdown";
            this.btnShutdown.Size = new System.Drawing.Size(245, 107);
            this.btnShutdown.TabIndex = 8;
            this.btnShutdown.Text = "İptal";
            this.btnShutdown.UseVisualStyleBackColor = false;
            this.btnShutdown.Click += new System.EventHandler(this.btnShutdown_Click);
            // 
            // imgKart
            // 
            this.imgKart.BackColor = System.Drawing.Color.Transparent;
            this.imgKart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imgKart.Image = global::rfidClient.Properties.Resources.logo;
            this.imgKart.InitialImage = null;
            this.imgKart.Location = new System.Drawing.Point(527, 76);
            this.imgKart.Name = "imgKart";
            this.imgKart.Size = new System.Drawing.Size(237, 242);
            this.imgKart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgKart.TabIndex = 9;
            this.imgKart.TabStop = false;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(527, 324);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(237, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 10;
            // 
            // lblUyari
            // 
            this.lblUyari.AutoSize = true;
            this.lblUyari.BackColor = System.Drawing.Color.Transparent;
            this.lblUyari.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblUyari.ForeColor = System.Drawing.Color.DarkRed;
            this.lblUyari.Location = new System.Drawing.Point(65, 350);
            this.lblUyari.Name = "lblUyari";
            this.lblUyari.Size = new System.Drawing.Size(1170, 46);
            this.lblUyari.TabIndex = 11;
            this.lblUyari.Text = "Lütfen müdahale etmeyin. Teknik personel erişimi sağlanıyor..";
            this.lblUyari.Visible = false;
            // 
            // LoginFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::rfidClient.Properties.Resources.step01;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1366, 768);
            this.ControlBox = false;
            this.Controls.Add(this.lblUyari);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.imgKart);
            this.Controls.Add(this.btnShutdown);
            this.Controls.Add(this.grpLogin);
            this.Controls.Add(this.btnGiris);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1366, 768);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1278, 768);
            this.Name = "LoginFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginFrm";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.LoginFrm_Load);
            this.grpLogin.ResumeLayout(false);
            this.grpLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgKart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGiris;
        private System.Windows.Forms.GroupBox grpLogin;
        private System.Windows.Forms.TextBox txtParola;
        private System.Windows.Forms.Label lblParola;
        private System.Windows.Forms.TextBox txtKulad;
        private System.Windows.Forms.Label lblKulad;
        private System.Windows.Forms.Button btnShutdown;
        private System.Windows.Forms.PictureBox imgKart;
        private System.Windows.Forms.Label lblWarn2;
        private System.Windows.Forms.Label lblWarn1;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblUyari;
    }
}