namespace rfidClient
{
    partial class SettingsFrm
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
            this.grpGecisler = new System.Windows.Forms.GroupBox();
            this.gridGecisler = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCikis = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gridZamanlar = new System.Windows.Forms.DataGridView();
            this.grpConsole = new System.Windows.Forms.GroupBox();
            this.btnDisableClientRun = new System.Windows.Forms.Button();
            this.btnGetNewRecords = new System.Windows.Forms.Button();
            this.txtServisKomut = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnKioskOff = new System.Windows.Forms.Button();
            this.btnKioskOn = new System.Windows.Forms.Button();
            this.grpGecisler.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGecisler)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridZamanlar)).BeginInit();
            this.grpConsole.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpGecisler
            // 
            this.grpGecisler.BackColor = System.Drawing.Color.Transparent;
            this.grpGecisler.Controls.Add(this.gridGecisler);
            this.grpGecisler.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.grpGecisler.ForeColor = System.Drawing.Color.Black;
            this.grpGecisler.Location = new System.Drawing.Point(12, 12);
            this.grpGecisler.Name = "grpGecisler";
            this.grpGecisler.Size = new System.Drawing.Size(1240, 198);
            this.grpGecisler.TabIndex = 6;
            this.grpGecisler.TabStop = false;
            this.grpGecisler.Text = "Geçiş Listesi";
            // 
            // gridGecisler
            // 
            this.gridGecisler.AllowUserToAddRows = false;
            this.gridGecisler.AllowUserToDeleteRows = false;
            this.gridGecisler.AllowUserToOrderColumns = true;
            this.gridGecisler.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridGecisler.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridGecisler.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridGecisler.Location = new System.Drawing.Point(6, 29);
            this.gridGecisler.Name = "gridGecisler";
            this.gridGecisler.ReadOnly = true;
            this.gridGecisler.Size = new System.Drawing.Size(1228, 159);
            this.gridGecisler.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(12, 510);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 31);
            this.label1.TabIndex = 7;
            // 
            // btnCikis
            // 
            this.btnCikis.BackColor = System.Drawing.Color.Crimson;
            this.btnCikis.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnCikis.ForeColor = System.Drawing.Color.Black;
            this.btnCikis.Location = new System.Drawing.Point(870, 898);
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.Size = new System.Drawing.Size(382, 76);
            this.btnCikis.TabIndex = 8;
            this.btnCikis.Text = "Çıkış";
            this.btnCikis.UseVisualStyleBackColor = false;
            this.btnCikis.Click += new System.EventHandler(this.btnCikis_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.gridZamanlar);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox1.ForeColor = System.Drawing.Color.Black;
            this.groupBox1.Location = new System.Drawing.Point(6, 216);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1240, 239);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Zamanlama";
            // 
            // gridZamanlar
            // 
            this.gridZamanlar.AllowUserToAddRows = false;
            this.gridZamanlar.AllowUserToDeleteRows = false;
            this.gridZamanlar.AllowUserToOrderColumns = true;
            this.gridZamanlar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridZamanlar.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridZamanlar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridZamanlar.Location = new System.Drawing.Point(6, 29);
            this.gridZamanlar.Name = "gridZamanlar";
            this.gridZamanlar.ReadOnly = true;
            this.gridZamanlar.Size = new System.Drawing.Size(1228, 200);
            this.gridZamanlar.TabIndex = 0;
            // 
            // grpConsole
            // 
            this.grpConsole.BackColor = System.Drawing.Color.Transparent;
            this.grpConsole.Controls.Add(this.btnDisableClientRun);
            this.grpConsole.Controls.Add(this.btnGetNewRecords);
            this.grpConsole.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.grpConsole.Location = new System.Drawing.Point(6, 461);
            this.grpConsole.Name = "grpConsole";
            this.grpConsole.Size = new System.Drawing.Size(294, 139);
            this.grpConsole.TabIndex = 9;
            this.grpConsole.TabStop = false;
            this.grpConsole.Text = "Servis Komutları";
            // 
            // btnDisableClientRun
            // 
            this.btnDisableClientRun.Location = new System.Drawing.Point(12, 82);
            this.btnDisableClientRun.Name = "btnDisableClientRun";
            this.btnDisableClientRun.Size = new System.Drawing.Size(271, 46);
            this.btnDisableClientRun.TabIndex = 2;
            this.btnDisableClientRun.Text = "Client kontrol kapat";
            this.btnDisableClientRun.UseVisualStyleBackColor = true;
            this.btnDisableClientRun.Click += new System.EventHandler(this.btnDisableClientRun_Click);
            // 
            // btnGetNewRecords
            // 
            this.btnGetNewRecords.Location = new System.Drawing.Point(12, 30);
            this.btnGetNewRecords.Name = "btnGetNewRecords";
            this.btnGetNewRecords.Size = new System.Drawing.Size(271, 46);
            this.btnGetNewRecords.TabIndex = 1;
            this.btnGetNewRecords.Text = "Yeni kayıtları aldır";
            this.btnGetNewRecords.UseVisualStyleBackColor = true;
            // 
            // txtServisKomut
            // 
            this.txtServisKomut.Location = new System.Drawing.Point(458, 646);
            this.txtServisKomut.Multiline = true;
            this.txtServisKomut.Name = "txtServisKomut";
            this.txtServisKomut.Size = new System.Drawing.Size(279, 108);
            this.txtServisKomut.TabIndex = 0;
            this.txtServisKomut.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.btnKioskOff);
            this.groupBox2.Controls.Add(this.btnKioskOn);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox2.Location = new System.Drawing.Point(319, 461);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(294, 139);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "İstemci Komutları";
            // 
            // btnKioskOff
            // 
            this.btnKioskOff.Location = new System.Drawing.Point(12, 82);
            this.btnKioskOff.Name = "btnKioskOff";
            this.btnKioskOff.Size = new System.Drawing.Size(271, 46);
            this.btnKioskOff.TabIndex = 2;
            this.btnKioskOff.Text = "Kiosk modundan çık";
            this.btnKioskOff.UseVisualStyleBackColor = true;
            this.btnKioskOff.Click += new System.EventHandler(this.btnKioskOff_Click);
            // 
            // btnKioskOn
            // 
            this.btnKioskOn.Location = new System.Drawing.Point(12, 30);
            this.btnKioskOn.Name = "btnKioskOn";
            this.btnKioskOn.Size = new System.Drawing.Size(271, 46);
            this.btnKioskOn.TabIndex = 1;
            this.btnKioskOn.Text = "Kiosk moduna Gir";
            this.btnKioskOn.UseVisualStyleBackColor = true;
            this.btnKioskOn.Click += new System.EventHandler(this.btnKioskOn_Click);
            // 
            // SettingsFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::rfidClient.Properties.Resources.step01;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1264, 986);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpConsole);
            this.Controls.Add(this.txtServisKomut);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCikis);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grpGecisler);
            this.DoubleBuffered = true;
            this.Name = "SettingsFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SettingsFrm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SettingsFrm_Load);
            this.grpGecisler.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridGecisler)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridZamanlar)).EndInit();
            this.grpConsole.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpGecisler;
        private System.Windows.Forms.DataGridView gridGecisler;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCikis;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView gridZamanlar;
        private System.Windows.Forms.GroupBox grpConsole;
        private System.Windows.Forms.Button btnGetNewRecords;
        private System.Windows.Forms.TextBox txtServisKomut;
        private System.Windows.Forms.Button btnDisableClientRun;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnKioskOff;
        private System.Windows.Forms.Button btnKioskOn;
    }
}