using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using rfidClient.KartEvents;

namespace rfidClient
{
    public partial class SettingsFrm : Form
    {
        DataTable gecislerTable;
        DataTable zamanlarTable;
        Int64 tableIndexGecisler = 0;
        Int64 tableIndexZamanlar = 0;

        private delegate void delGecisList();

        public SettingsFrm()
        {
            InitializeComponent();
        }

        private void SettingsFrm_Load(object sender, EventArgs e)
        {
            gecislerTable = new DataTable();
            gecislerTable.Columns.Add("Index", typeof(Int64));
            gecislerTable.Columns.Add("Turnike No", typeof(Int32));
            gecislerTable.Columns.Add("Gelen Veri", typeof(String));
            gecislerTable.Columns.Add("Tarih", typeof(DateTime));

            zamanlarTable = new DataTable();
            zamanlarTable.Columns.Add("Index", typeof(Int32));
            zamanlarTable.Columns.Add("Turnike No", typeof(String));
            zamanlarTable.Columns.Add("Açıklama", typeof(String));
            zamanlarTable.Columns.Add("Okunan Zaman", typeof(String));
            zamanlarTable.Columns.Add("Tarih", typeof(String));

            gridGecisler.DataSource = gecislerTable;
            gridZamanlar.DataSource = zamanlarTable;

            GecislerArray.OnKartOkuma += new EventHandler<GecislerArgsArray>(GecislerArray_OnKartOkuma);
            GecislerBool.OnKartStream += new EventHandler<GecislerArgsBool>(GecislerBool_OnKartStream);

            ZamanlarArray.OnZaman += new EventHandler<ZamanlarArgsArray>(ZamanlarArray_OnZaman);

            if (WinAPI.ptrHook == IntPtr.Zero)
            {
                btnKioskOn.Enabled = true;
                btnKioskOff.Enabled = false;
            }
            else
            {
                btnKioskOn.Enabled = false;
                btnKioskOff.Enabled = true;
            }
        }

        private void Gecisler_OnKartOkuma(object sender, EventArgs e)
        {
            tableIndexGecisler++;
            this.BeginInvoke(new delGecisList(RefreshGridGecisler), null);
        }

        private void GecislerArray_OnKartOkuma(object sender, EventArgs e)
        {
            tableIndexGecisler++;
            gecislerTable.Rows.Add(tableIndexGecisler, GecislerArray.OkunanData[0], GecislerArray.OkunanData[1], GecislerArray.OkunanData[2], DateTime.Now.ToLongTimeString());
            this.BeginInvoke(new delGecisList(RefreshGridGecisler), null);
        }

        private void ZamanlarArray_OnZaman(object sender, EventArgs e)
        {
            tableIndexZamanlar++;
            zamanlarTable.Rows.Add(tableIndexZamanlar, ZamanlarArray.OkunanZaman[0], ZamanlarArray.OkunanZaman[1], ZamanlarArray.OkunanZaman[2], DateTime.Now.ToLongTimeString());
            this.BeginInvoke(new delGecisList(RefreshGridZamanlar), null);
        }

        private void GecislerBool_OnKartStream(object sender, EventArgs e)
        {

        }

        private void RefreshGridGecisler()
        {
            Application.DoEvents();
            gridGecisler.DataSource = gecislerTable;            
            gridGecisler.Refresh();
        }

        private void RefreshGridZamanlar()
        {
            Application.DoEvents();
            gridZamanlar.DataSource = zamanlarTable;
            gridZamanlar.Refresh();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGetNewRecords_Click(object sender, EventArgs e)
        {
            try
            {
                String fileName = @"C:\Users\Public\Documents\srvc.cmdx";
                /*
                File.Create(fileName);
                FileStream fileStream = File.Open(fileName, FileMode.Open);
                fileStream.SetLength(0);
                fileStream.Close();
                */
                StreamWriter sw = new StreamWriter(fileName);
                String command = txtServisKomut.Text.Trim();
                sw.WriteLine(String.Empty);
                sw.Close();
                StreamWriter sw2 = new StreamWriter(fileName);
                sw2.WriteLine("GetNewRecords");                
                sw2.Close();
            }
            catch (Exception ex)
            {
                Logger.Method = "Settings, GetNewRecords";
                Logger.Message = ex.Message;
            }  
        }

        private void btnDisableClientRun_Click(object sender, EventArgs e)
        {
            try
            {
                String fileName = @"C:\Users\Public\Documents\srvc.cmdx";
                /*
                File.Create(fileName);
                FileStream fileStream = File.Open(fileName, FileMode.Open);
                fileStream.SetLength(0);
                fileStream.Close();
                */
                StreamWriter sw = new StreamWriter(fileName);
                String command = txtServisKomut.Text.Trim();
                sw.WriteLine(String.Empty);
                sw.Close();
                StreamWriter sw2 = new StreamWriter(fileName);
                sw2.WriteLine("DisableClientRun");
                sw2.Close();
            }
            catch (Exception ex)
            {
                Logger.Method = "Settings, DisableClientRun";
                Logger.Message = ex.Message;
            }
        }

        private void btnKioskOn_Click(object sender, EventArgs e)
        {
            if (WinAPI.ptrHook == IntPtr.Zero)
            {
                WinAPI.KioskMode();
                //if (WinAPI.ptrHook == IntPtr.Zero) MessageBox.Show("Not Hooked");
                btnKioskOn.Enabled = false;
                btnKioskOff.Enabled = true;
            }
        }

        private void btnKioskOff_Click(object sender, EventArgs e)
        {
            if (WinAPI.ptrHook != IntPtr.Zero)
            {
                bool rv = WinAPI.UnKioskMode();
                //if (!rv) MessageBox.Show("Not Unhooked");
                btnKioskOn.Enabled = rv;
                btnKioskOff.Enabled = !rv;
            }
        }
    }
}
