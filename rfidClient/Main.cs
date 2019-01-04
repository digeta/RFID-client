using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using rfidClient.KartEvents;

namespace rfidClient
{
    public partial class Main : Form
    {
        private Thread DBThread;
        private Boolean DB_OK = false;


        private Char _ayrac = Convert.ToChar(",");

        String _komut = ",BUZZER;0;1"; // Uyuma la

        private System.Timers.Timer _timer;
        private System.Timers.Timer _uptimer;
        private Int32 _elapsedUp = 0;

        public delegate void deleg(String data);
        public delegate void delegex(Exception data);
        public delegate void delegTarih(DateTime tarih);
        public delegate void delegKart(Kart kart);
        public delegate void delegGecisler(DataTable gecisler);
        public delegate void delegLights(Int32 trnum, String clr);

        public Int64 misafirKart = 0;

        Boolean _dataOku = true;
        Boolean _dinlemeDevam = true;        
        Boolean _running = false;

        IPEndPoint ipserv;

        DataTable _table;
        DataTable _threadsRunning;
        Dictionary<String, Thread> _threadsReconnect = new Dictionary<String, Thread>();

        ArrayList _gecisListesi = new ArrayList();
        Thread _gecislerKaydet;
        public static Boolean GecisKaydetDevam = false;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            WinAPI.KioskMode();

            DBThread = new Thread(new ThreadStart(DBconnect));
            DBThread.Start();

            while (!DB_OK)
            {
                Application.DoEvents();
            }

            try
            {
                Int32 rowCount = Settings.Turnikeler.Rows.Count;
                for (Int32 i = 0; i < rowCount - 1; i++)
                {
                    GroupBox grpBox = (GroupBox)grpKart.Controls.Find("grp_t" + Convert.ToString(Settings.Turnikeler.Rows[i]["TURNIKE_NO"]), false)[0];
                    grpBox.Text = Convert.ToString(Settings.Turnikeler.Rows[i]["HAREKET_TUR"]);
                }                
            }
            catch (Exception ex)
            {
            }

            _uptimer = new System.Timers.Timer();
            _uptimer.Interval = 1000;
            _uptimer.Elapsed += new System.Timers.ElapsedEventHandler(Uptimer_Elapsed);
            _uptimer.Start();

            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);
            _timer.Start();
            
            Durumlar.OnErrorChanged += new EventHandler<DurumArgsHata>(Main_HataVar);
            Durumlar.OnMessageChanged += new EventHandler<DurumArgsMessage>(Main_MesajVar);
            Durumlar.OnStatusChanged += new EventHandler<DurumArgsStatus>(Main_DurumVar);
                                  
            Logger.OnMessageChanged += new EventHandler<LogArgs>(OnMessageChanged);
            KartOkundu.OnKartOkundu +=new EventHandler<KartOkunduArgs>(OnKartOkundu);

            this.FormClosing += new FormClosingEventHandler(OnClose);
            //CihazTest();
            run();
        }


        #region "Events"
        private void Main_MesajVar(Object obj, DurumArgsMessage msgArgs)
        {
            lblMesaj.Text = msgArgs.Message;
        }

        private void Main_HataVar(Object obj, DurumArgsHata hataArgs)
        {
            lblHata.Text = hataArgs.Hata;
        }

        private void Main_DurumVar(Object obj, DurumArgsStatus statusArgs)
        {
            lblDurum2.Text = statusArgs.Durum;
        }

        private void OnMessageChanged(Object obj, LogArgs logArgs)
        {
            DB.LogKaydiYap();
        }

        private void OnKartOkundu(Object obj, KartOkunduArgs kartOkunduArgs)
        {
            misafirKart = KartOkundu.KartID;
        }

        private void Mesaj(String msgData)
        {
            Durumlar.Mesaj = msgData;
        }

        private void SistemDurum(String durumData)
        {
            Durumlar.Durum = durumData;
        }

        private void Hata(String hataData)
        {
            Durumlar.HataDetay = hataData;
        }

        private void Lights(Int32 turnikeNo, String color)
        {
            PictureBox imgStatus;
            GroupBox grpBox = (GroupBox)grpKart.Controls.Find("grp_t" + turnikeNo, false)[0];
            imgStatus = (PictureBox)grpBox.Controls.Find("imgStatus_t" + turnikeNo, false)[0];
            if (color == "Green")
            {
                imgStatus.Image = Properties.Resources.stGreen;
            }
            if (color == "Red")
            {
                imgStatus.Image = Properties.Resources.stRed;
            }
            if (color == "Yellow")
            {
                imgStatus.Image = Properties.Resources.stYellow;
            }
        }
        #endregion



        private void DBconnect()
        {
            DB_OK = DB.InitDB();
        }


        #region "Sektör 1"
        private void run()
        {            
            try
            {
                _running = true; // Çalışıyore

                /*
                   - Geçerli IP adresi ve Port numarası olan turnikeleri, turnikeler Datarow' una eklenir
                   - Turnikelere sonradan sıra numarasıyla erişebilmek için gerekli olan tablo düzenlenir
                   - Aktif olacak her turnike için thread tanımlanır
                   - Her thread, OkuyucuBaglan methodunu kendine alarak çalıştırılır
                */
                DataRow[] turnikeler = Settings.Turnikeler.Select("LEN(IP) > 0 AND PORT > 0");

                _table = new DataTable();
                _table.Columns.Add("Socket", typeof(Socket));
                _table.Columns.Add("TurnikeNo", typeof(Int32));
                _table.Columns.Add("TurnikeClass", typeof(Turnike));

                _threadsRunning = new DataTable();
                _threadsRunning.Columns.Add("Thread", typeof(Thread));
                _threadsRunning.Columns.Add("TurnikeNo", typeof(Int32));
                _threadsRunning.Columns.Add("TurnikeRow", typeof(DataRow));
               
                Thread[] threads = new Thread[turnikeler.Length];

                for (Int32 i = 0; i < threads.Length; i++)
                {
                    threads[i] = new Thread(new ParameterizedThreadStart(OkuyucuVeriAl));
                    threads[i].Start(turnikeler[i]);
                    _threadsRunning.Rows.Add(threads[i], Convert.ToInt32(turnikeler[i]["TURNIKE_NO"]), turnikeler[i]);
                }
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
                {
                    Logger.isError = true;
                    Logger.Method = "Main, run";
                    Logger.Message = ex.Message;
                    this.BeginInvoke(new deleg(Hata), "Sektör 1' de hata!");
                }
            }
        }

        private void GecisKaydet()
        {
            GecisKaydetDevam = true;
            Int32 result = 0;
            while (GecisKaydetDevam)
            {
                for (Int32 i = 0; i < _gecisListesi.Count - 1; i++)
                {
                    result = DB.GecisKaydet((Kart)_gecisListesi[i]);
                    if (result > 0)
                    {
                        _gecisListesi.RemoveAt(i);
                    }
                }
            }
        }
        #endregion


        #region "Sektör 2           Turnike Kart Okuyucuları ile Haberleş"
        private void OkuyucuVeriAl(object turnikeObj)
        {
            /* 
               - Listeden gönderilen turnike bilgileri
               - TurnikeNo: Turnikenin veritabanında ki numarası (ID değil, elle atanır)
               - StartKomut: kart okuyucular ile bağlantı sağlanabildiğine dair sesli uyarı komutu (BUZZER)
               - Komut: Geçerli kart okunduğunda gönderilen komutlar (röle aç, buzzer vs.)
               - FailKomut: Geçersiz kart okunduğunda gönderilen sesli uyarı komutu (BUZZER uzun süreli)
            */
            DataRow turnikeRow = (DataRow)turnikeObj;
            Turnike turnike = new Turnike();

            turnike.TurnikeNo = Convert.ToInt32(turnikeRow["TURNIKE_NO"]);
            turnike.CihazID = Convert.ToString(turnikeRow["CIHAZ_ID"]);
            turnike.IP = Convert.ToString(turnikeRow["IP"]);
            turnike.Port = Convert.ToInt32(turnikeRow["PORT"]);
            turnike.StartKomut = Convert.ToString(turnikeRow["START_KOMUT"]);
            turnike.Komut = Convert.ToString(turnikeRow["KOMUT"]);
            turnike.FailKomut = Convert.ToString(turnikeRow["FAIL_KOMUT"]);
            turnike.Aktif = true;

            try
            {
                ASCIIEncoding encoder = new ASCIIEncoding();
                
                Byte[] gelenBytes = new Byte[4096];
                Byte[] gidenBytes = encoder.GetBytes(turnike.CihazID + turnike.Komut);
                Byte[] gidenBytesFail = encoder.GetBytes(turnike.CihazID + turnike.FailKomut);
                
                /*
                NetworkStream onOffStream = client.GetStream();
                Byte[] onOffBytes = encoder.GetBytes(CihazID + StartKomut);
                onOffStream.Write(onOffBytes, 0, onOffBytes.Length);
                onOffStream.Flush();
                */

                Int32 connectionAttempt = 0;
                Socket dataSocket = null;
                while (dataSocket == null)
                {
                    connectionAttempt++;
                    dataSocket = Baglan(turnike);
                    this.BeginInvoke(new delegLights(Lights), turnike.TurnikeNo, "Yellow");
                }

                if (dataSocket != null) this.BeginInvoke(new deleg(SistemDurum), "");

                _table.Rows.Add(dataSocket, turnike.TurnikeNo, turnike);

                Int32 hataSayac = 0;

                while (turnike.Aktif)
                {
                    int gelenVeri = 0;

                    DataRow[] socketRow = _table.Select("TurnikeNo = " + turnike.TurnikeNo);
                    dataSocket = (Socket)socketRow[0]["Socket"];
                    
                    try
                    {
                        gelenVeri = dataSocket.Receive(gelenBytes, 0, gelenBytes.Length, SocketFlags.None);
                        hataSayac = 0;                        
                    }
                    catch (Exception ex)
                    {
                        if (hataSayac >= 50)
                        {
                            if (!this.IsDisposed)
                            {
                                Logger.isError = true;
                                Logger.Method = "Main, OkuyucuBaglan, Bağlantı yok";
                                Logger.Message = ex.Message;
                            }
                            hataSayac = 0;
                        }
                    }
                    
                    String gelenStr = "";
                    if (gelenVeri > 0)
                    {
                        gelenStr = encoder.GetString(gelenBytes, 0, gelenVeri);
                    }
                    this.BeginInvoke(new deleg(Hata), gelenStr);
                    if (gelenStr != "")
                    {                    
                        //"MCR02-3EAAF7"
                        //Int32 cihaz1 = 0;
                        //cihaz1 = gelenStr.IndexOf("MCR");
                        //this.BeginInvoke(new deleg(Mesaj), Convert.ToString(cihaz1));
                                                        
                        string pattern = @"(MCR)([A-F0-9]*)(-)([A-F0-9]*)(,UID=)([0-9]*)";
                        Regex myRegex = new Regex(pattern, RegexOptions.IgnoreCase);

                        Match m = myRegex.Match(gelenStr);   // m is the first match

                        string pattern2 = @"(MCR)([A-F0-9]*)(-)([A-F0-9]*)";
                        Regex myRegex2 = new Regex(pattern2, RegexOptions.IgnoreCase);

                        Match m2 = myRegex2.Match(m.Value);   // m is the first match

                        string m3 = m.Value.Substring(m2.Value.Length + 5);

                            //String[] gelenBilgi = gelenStr.Split(_ayrac); // Okuyucudan gelen veriler "," ile ayrılmış
                        
                            Int64 okunanKartno = 0;
                            String kartBilgisi = "";

                            if (m.Value.Length >= 2)
                            {
                                try
                                {
                                    //this.BeginInvoke(new deleg(Mesaj), Convert.ToString(gelenBilgi[1]) + " - " + Convert.ToString(kartBilgisi.Substring(4)));
                                    //kartBilgisi = Convert.ToString(gelenBilgi[1]); // Dizinin 2. elemanı kart ID bilgisi
                                    //this.BeginInvoke(new deleg(Mesaj), kartBilgisi);
                                    //okunanKartno = Convert.ToInt64(kartBilgisi.Substring(4)); // "UID=" den sonra kart IDsi başlıyor
                                    okunanKartno = Convert.ToInt64(m3);
                                    this.BeginInvoke(new deleg(Mesaj), Convert.ToString(okunanKartno));
                                }
                                catch (Exception ex)
                                {
                                    //this.BeginInvoke(new deleg(Mesaj), "3");
                                }
                            }

                            if (okunanKartno > 0)
                            {
                                Kart kart = new Kart();

                                // Okuyucudan gelen kart IDsi ile veritabanındakiler ters, önce bu düzeltilir ve
                                // daha sonra veritabanından kart ID kontrolü yapılır
                                Int64 kartid = 0;
                                try
                                {
                                    kartid = tersineCevir(okunanKartno);
                                    this.BeginInvoke(new deleg(Mesaj), Convert.ToString(kartid));
                                }
                                catch (Exception ex)
                                {
                                    Logger.isError = true;
                                    Logger.Method = "Main, Tersine Çevir";
                                    Logger.Message = ex.Message;
                                }

                                DBThread dbThread = new DBThread();
                                kart = dbThread.KartKontrol(kartid);

                                kart.Birim = "";

                                if (kart.KartTipid == 2 || kart.KartTipid == 3)
                                {
                                    Kart picard = new Kart();
                                    picard = DB.ResimGetir(kart.Tckimlik);
                                    kart.Resim = picard.Resim;
                                    kart.Birim = picard.Birim;
                                }

                                //kart.CihazID = gelenBilgi[0]; // Okuyucudan gelen veri dizisinin ilk elemanı cihaz IDsi
                                kart.CihazID = m2.Value;

                                kart.TurnikeNo = turnike.TurnikeNo;
                                kart.GecisTarihi = DateTime.Now;

                                // Okunan karta ait bilgileri ekrana yazdır
                                if (!this.IsDisposed)
                                {
                                    this.BeginInvoke(new delegKart(KartBilgiBas), kart);
									
                                    if (kart.GecisOnay)
                                    {
                                        dataSocket.Send(gidenBytes);
                                    }
                                    else
                                    {
                                        dataSocket.Send(gidenBytesFail);
                                    }
                                }
                        }
                    }
                    
                    DataRow[] aktifRow = _table.Select("TurnikeNo = " + turnike.TurnikeNo);
                    if (aktifRow.Length > 0)
                    {
                        Turnike aktifTurnike = (Turnike)aktifRow[0]["TurnikeClass"];
                        turnike.Aktif = aktifTurnike.Aktif;
                    }
                }
            }
            catch (Exception ex)
            {               
                if (!this.IsDisposed)
                {
                    Logger.isError = true;
                    Logger.Method = "Main, OkuyucuVeriAl";
                    Logger.Message = ex.Message;
                    this.BeginInvoke(new deleg(Hata), "Sektör 2' de hata!");
                    // Hata oluştuğunda ilgili soket ve turnike bilgilerinin tablodan çıkarılması
                    _table.Rows.RemoveAt(_table.Rows.IndexOf(_table.Select("TurnikeNo = " + turnike.TurnikeNo)[0]));
                }  
            }
        }

        /* Okuyucu Client modda ise
        private void OkuyucuDinle(object clientSocket)
        {
            try
            {
                // 
                   - Listeden gönderilen turnike bilgileri
                   - turnikeNum: Turnikenin veritabanında ki numarası (ID değil, elle atanır)
                   - startKomut: kart okuyucular ile bağlantı sağlanabildiğine dair sesli uyarı komutu (BUZZER)
                   - komut: Geçerli kart okunduğunda gönderilen komutlar (röle aç, buzzer vs.)
                   - failKomut: Geçersiz kart okunduğunda gönderilen sesli uyarı komutu (BUZZER uzun süreli)
                //
                Socket client = (Socket)clientSocket;
                IPEndPoint ipClient = (IPEndPoint)client.RemoteEndPoint;
                String clientIP = ipClient.Address.ToString();
                DataRow[] turnikeler = Settings.Turnikeler.Select("IP ='" + clientIP + "'");                

                DataRow turnikeRow = turnikeler[0];
                Turnike turnike = new Turnike();

                turnike.TurnikeNo = Convert.ToInt32(turnikeRow["TURNIKE_NO"]);
                turnike.CihazID = Convert.ToString(turnikeRow["CIHAZ_ID"]);
                turnike.IP = Convert.ToString(turnikeRow["IP"]);
                turnike.Port = Convert.ToInt32(turnikeRow["PORT"]);
                turnike.StartKomut = Convert.ToString(turnikeRow["START_KOMUT"]);
                turnike.Komut = Convert.ToString(turnikeRow["KOMUT"]);
                turnike.FailKomut = Convert.ToString(turnikeRow["FAIL_KOMUT"]);
                turnike.Aktif = true;

                ASCIIEncoding encoder = new ASCIIEncoding();
                //Data data = new Data(4096);

                Byte[] gelenBytes = new Byte[4096];
                Byte[] gidenBytes = encoder.GetBytes(turnike.CihazID + turnike.Komut);
                Byte[] gidenBytesFail = encoder.GetBytes(turnike.CihazID + turnike.FailKomut);             

                _table.Rows.Add(client, turnike.TurnikeNo, turnike);
                Int32 hataSayac = 0;

                while (turnike.Aktif)
                {
                    int gelenVeri = 0;

                    DataRow[] socketRow = _table.Select("TurnikeNo = " + turnike.TurnikeNo);
                    client = (Socket)socketRow[0]["Socket"];

                    try
                    {
                        gelenVeri = client.Receive(gelenBytes, 0, gelenBytes.Length, SocketFlags.None);
                        hataSayac = 0;
                    }
                    catch (Exception ex)
                    {
                        if (hataSayac >= 30)
                        {
                            if (!this.IsDisposed)
                            {
                                Logger.isError = true;
                                Logger.Method = "Main, OkuyucuBaglan, Bağlantı yok";
                                Logger.Message = ex.Message;
                                this.BeginInvoke(new deleg(SistemDurum), Convert.ToString(turnike.TurnikeNo) + ". Turnike ile bağlantı koptu!");
                            }
                            hataSayac = 0;
                        }
                    }

                    String gelenStr = "";
                    if (gelenVeri > 0)
                    {
                        gelenStr = encoder.GetString(gelenBytes, 0, gelenVeri);
                    }

                    if (gelenStr != "")
                    {
                        //String gelenStr = encoder.GetString(gelenBytes, 0, gelenVeri);

                        Char ayrac = Convert.ToChar(",");
                        String[] gelenBilgi = gelenStr.Split(ayrac); // Okuyucudan gelen veriler "," ile ayrılmış

                        String kartBilgisi = Convert.ToString(gelenBilgi[1]); // Dizinin 2. elemanı kart ID bilgisi
                        Int64 okunanKartno = Convert.ToInt64(kartBilgisi.Substring(4)); // "UID=" den sonra kart IDsi başlıyor

                        Kart kart = new Kart();

                        // Okuyucudan gelen kart IDsi ile veritabanındakiler ters, önce bu düzeltilir ve
                        // daha sonra veritabanından kart ID kontrolü yapılır
                        Int64 kartid = tersineCevir(okunanKartno);

                        kart = DB.KartKontrol(kartid);

                        if (KartOkundu.NewUser | KartOkundu.DelUser)
                        {
                            KartOkundu.TCKimlik = kart.Tckimlik;
                            KartOkundu.Ad = kart.Ad;
                            KartOkundu.Soyad = kart.Soyad;
                            KartOkundu.KartID = kartid;
                        }

                        kart.CihazID = gelenBilgi[0]; // Okuyucudan gelen veri dizisinin ilk elemanı cihaz IDsi

                        kart.TurnikeNo = turnike.TurnikeNo;

                        // Okunan karta ait bilgileri ekrana yazdır
                        if (!this.IsDisposed)
                        {
                            this.BeginInvoke(new delegKart(KartBilgiBas), kart);

                            if (kart.GecisOnay)
                            {
                                client.Send(gidenBytes);
                                this.BeginInvoke(new deleg(Mesaj), "");
                            }
                            else
                            {
                                client.Send(gidenBytesFail);
                                this.BeginInvoke(new deleg(Mesaj), "Geçersiz kart veya geçiş izni yok!");
                            }
                            DB.GecisKaydiYap(kart);
                        }
                    }

                    if (!client.Connected)
                    {
                        if (!this.IsDisposed)
                        {
                            Logger.isError = true;
                            Logger.Method = "Main, OkuyucuDinle, Bağlantı koptu";
                            Logger.Message = ((IPEndPoint)client.RemoteEndPoint).Address.ToString() + " yeniden bağlanılacak";
                            this.BeginInvoke(new deleg(SistemDurum), Convert.ToString(turnike.TurnikeNo) + ". Turnike ile bağlantı koptu!");
                        }
                        _table.Rows.RemoveAt(_table.Rows.IndexOf(_table.Select("TurnikeNo = " + turnike.TurnikeNo)[0]));
                        turnike.Aktif = false;
                    }
                }

                //if (outerSocket != null) outerSocket.Close(1);
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
                {
                    Logger.isError = true;
                    Logger.Method = "Main, OkuyucuVeriAl";
                    Logger.Message = ex.Message;
                    this.BeginInvoke(new deleg(Hata), "Sektör 2' de hata!");
                    //reconnectTimer++;
                }
            }
        }
        */
        #endregion



        #region "Sektör 3           Turnike Kart Okuyucuları ile Bağlantı Kur"
        private Socket Baglan(Turnike turnike)
        {            
            Socket socket = null;

            try
            {
                Socket srvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ipserv = new IPEndPoint(IPAddress.Parse(turnike.IP), turnike.Port);
                srvSocket.Connect(ipserv);
                socket = srvSocket;

            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
                {
                    Logger.isError = true;
                    Logger.Method = "Main, Baglan";
                    Logger.Message = ex.Message;
                }
            }
            return socket;
        }

        private void TekrarBaglan(object turnikeObj)
        {
            Turnike turnike = (Turnike)turnikeObj;

            try
            {                
                turnike.Baglaniyor = true;

                Int32 connectionAttempt = 0;
                Socket dataSocketNew = null;
                while (dataSocketNew == null)
                {
                    connectionAttempt++;
                    dataSocketNew = Baglan(turnike);
                }

                _table.Rows.RemoveAt(_table.Rows.IndexOf(_table.Select("TurnikeNo = " + turnike.TurnikeNo)[0]));
                _table.Rows.Add(dataSocketNew, turnike.TurnikeNo, turnike);                
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
                {
                    Logger.isError = true;
                    Logger.Method = "Main, TekrarBaglan";
                    Logger.Message = ex.Message;
                }
            }
            finally
            {
                _threadsReconnect.Remove("thread_" + turnike.TurnikeNo);
                turnike.Baglaniyor = false;
            }
        }

        private void Dinle()
        {
            try
            {
                Socket srvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ipserv = new IPEndPoint(IPAddress.Any, Settings.OkuyucuPort);
                srvSocket.Bind(ipserv);
                srvSocket.Listen(100);

                while (_dinlemeDevam)
                {
                    Socket client = srvSocket.Accept();
                }
            }
            catch (Exception ex)
            {
                if (ex is SocketException)
                {
                    Logger.isError = true;
                    Logger.Method = "Main, Dinle";
                    Logger.Message = ex.Message;
                    this.BeginInvoke(new deleg(Hata), "Bağlantı noktası kullanımda, çalışan diğer programları kapatın");
                }
            }
        }

        private void CihazTest()
        {
            Thread receiveThread = new Thread(new ThreadStart(this.ReceiveData));                   
            receiveThread.Start();

            string str = "255.255.255.255";
            IPEndPoint pEndPoint = new IPEndPoint(IPAddress.Parse(str), 65535);
            UdpClient udpClient = new UdpClient();
            try
            {
                try
                {
                    string str1 = "$GRTC,#";
                    //string str1 = "$BRD,#";
                    if (str1.Length != 0)
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(str1);
                        udpClient.Send(bytes, (int)bytes.Length, pEndPoint);
                        udpClient.Close();
                    }
                }
                catch (Exception exception)
                {
                    this.BeginInvoke(new deleg(Hata), exception.Message);
                }
            }
            finally
            {
                udpClient.Close();
            }
        }

        private void ReceiveData()
        {
            string str = "";
            int ınt32 = 1000;
            UdpClient udpClient = null;
            this._dataOku = true;
            try
            {
                try
                {
                    udpClient = new UdpClient(65535);
                    while (this._dataOku)
                    {
                        try
                        {
                            IPEndPoint pEndPoint = new IPEndPoint(IPAddress.Any, 0);
                            udpClient.Client.ReceiveTimeout = ınt32;
                            if (this._dataOku)
                            {
                                byte[] numArray = udpClient.Receive(ref pEndPoint);
                                str = Encoding.UTF8.GetString(numArray);
                                if (!str.Equals("$BRD,#"))
                                {
                                    //this.AddToListBox(str);
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch (SocketException socketException1)
                        {
                            SocketException socketException = socketException1;
                            if (socketException.SocketErrorCode == SocketError.TimedOut)
                            {
                                string message = socketException.Message;
                            }
                        }
                    }
                }
                catch (Exception exception1)
                {
                    Exception exception = exception1;
                    string[] strArrays = new string[] { "Error: ", exception.Message, "\n\n Please close UDP Port (65535) and try again." };
                    MessageBox.Show(string.Concat(strArrays), "Port Error");
                }
            }
            finally
            {
                udpClient.Close();
            }
        }
        #endregion

        #region "Sektör 4           Okunan Kart Numarasını hex'e ve tersine çevir sonra long' a"
        private long tersineCevir(Int64 kartNo)
        {
            Int64 result = 0;
            try
            {
                String kartHex = kartNo.ToString("X");
                String kartKimlik = "";

                for (int i = 6; i > -1; i -= 2)
                {
                    kartKimlik += kartHex.Substring(i, 2);
                }

                result = Convert.ToInt64(kartKimlik, 16);
            }
            catch (Exception ex)
            {
                Logger.isError = true;
                Logger.Method = "Main, tersineCevir";
                Logger.Message = ex.Message;
                this.BeginInvoke(new deleg(Hata), "Sektör 4' de hata");
            }
            return result;
        }
        #endregion



        #region "Sektör 5           Kart Bilgilerini Ekrana Bas"
        private void KartBilgiBas(Kart kart)
        {
            try
            {
                // gprKart groupbox' ı içindeki alanlara, turnike numarasına göre bilgi yazdırma
                GroupBox grpBox = (GroupBox)grpKart.Controls.Find("grp_t" + Convert.ToString(kart.TurnikeNo), false)[0];

                Label labelAd = (Label)grpBox.Controls.Find("lblAd_t" + Convert.ToString(kart.TurnikeNo), false)[0];
                Label labelSoyad = (Label)grpBox.Controls.Find("lblSoyad_t" + Convert.ToString(kart.TurnikeNo), false)[0];
                Label labelBirim = (Label)grpBox.Controls.Find("lblBirim_t" + Convert.ToString(kart.TurnikeNo), false)[0];
                Label labelKartTip = (Label)grpBox.Controls.Find("lblKartTip_t" + Convert.ToString(kart.TurnikeNo), false)[0];
                Label labelGecisOnay = (Label)grpBox.Controls.Find("lblGecisOnay_t" + Convert.ToString(kart.TurnikeNo), false)[0];
                PictureBox imgKart = (PictureBox)grpKart.Controls.Find("imgKart_t" + Convert.ToString(kart.TurnikeNo), false)[0];

                labelAd.Text = kart.Ad;
                labelSoyad.Text = kart.Soyad;
                labelBirim.Text = kart.Birim;
                imgKart.Image = kart.Resim;
                labelGecisOnay.Text = (kart.Ad == "" | kart.Ad == null) ? "" : (kart.GecisOnay) ? "Onaylı" : "Onaysız";
                labelKartTip.Text = kart.KartTip;
             }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
                {
                    Logger.isError = true;
                    Logger.Method = "Main, KartBilgiBas";
                    Logger.Message = ex.Message;
                    this.BeginInvoke(new deleg(Hata), "Sektör 5-1' de hata");
                }
            }
        }

        private void EkranTemizle(Int32 turnikeNo)
        {
            try
            {
                // gprKart groupbox' ı içindeki alanlara, turnike numarasına göre bilgi yazdırma
                GroupBox grpBox = (GroupBox)grpKart.Controls.Find("grp_t" + turnikeNo, false)[0];

                Label labelAd = (Label)grpBox.Controls.Find("lblAd_t" + turnikeNo, false)[0];
                Label labelSoyad = (Label)grpBox.Controls.Find("lblSoyad_t" + turnikeNo, false)[0];
                Label labelBirim = (Label)grpBox.Controls.Find("lblBirim_t" + turnikeNo, false)[0];
                Label labelKartTip = (Label)grpBox.Controls.Find("lblKartTip_t" + turnikeNo, false)[0];
                Label labelGecisOnay = (Label)grpBox.Controls.Find("lblGecisOnay_t" + turnikeNo, false)[0];
                PictureBox imgKart = (PictureBox)grpKart.Controls.Find("imgKart_t" + turnikeNo, false)[0];

                labelAd.Text = "";
                labelSoyad.Text = "";
                labelBirim.Text = "";
                imgKart.Image = Properties.Resources.nopicture;
                labelGecisOnay.Text = "";
                labelKartTip.Text = "";
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
                {
                    Logger.isError = true;
                    Logger.Method = "Main, EkranTemizle";
                    Logger.Message = ex.Message;
                    this.BeginInvoke(new deleg(Hata), "Sektör 5-2' de hata");
                }
            }
        }

        private void GecisListesi(DataTable dtGecisler)
        {
            try
            {
                //gridTurnike_1.DataSource = dtGecisler;
                //gridTurnike_1.Refresh();
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
                {
                    Logger.isError = true;
                    Logger.Method = "Main, GecisListesi";
                    Logger.Message = ex.Message;
                    this.BeginInvoke(new deleg(Hata), "Sektör 5-1' de hata");
                }
            }
        }
        #endregion



        #region "Sektör 7           Uptimer, Okuyucu Bağlantı Kontrol"
        private void Timer_Elapsed(object sender, EventArgs e)
        {
            this.BeginInvoke(new delegTarih(Tarih), DateTime.Now);
        }

        private void Tarih(DateTime tarih)
        {
            lblTarih.Text = tarih.ToString();
        }

        private void Uptimer_Elapsed(object sender, EventArgs e)
        {
            _elapsedUp++;

            if (_running && _elapsedUp >= 5)
            {
                _uptimer.Stop();
                _elapsedUp = 0;
                                
                ASCIIEncoding encoder = new ASCIIEncoding();                

                for (Int32 i = 0; i < _table.Rows.Count; i++)
                {
                    Socket dataSocket = (Socket)_table.Rows[i]["Socket"];
                    Turnike turnike = (Turnike)_table.Rows[i]["TurnikeClass"];
                    Byte[] wakeUpBytes = encoder.GetBytes(turnike.CihazID + _komut);

                    try
                    {
                        dataSocket.Send(wakeUpBytes);
                        this.BeginInvoke(new delegLights(Lights), turnike.TurnikeNo, "Green");
                    }
                    catch(Exception ex)
                    {
                        this.BeginInvoke(new delegLights(Lights), turnike.TurnikeNo, "Red");

                        Logger.isError = true;
                        Logger.Method = "Main, Uptimer";
                        Logger.Message = ex.Message;

                        if (!turnike.Baglaniyor)
                        {                            
                            _threadsReconnect.Add("thread_" + turnike.TurnikeNo, new Thread(new ParameterizedThreadStart(TekrarBaglan)));
                            ((Thread)_threadsReconnect["thread_" + turnike.TurnikeNo]).Start(turnike);
                        }
                    }
                }
                
                for (Int32 i = 0; i < _threadsRunning.Rows.Count; i++)
                {
                    Thread thread = (Thread)_threadsRunning.Rows[i]["Thread"];
                    DataRow turnikeRow = (DataRow)_threadsRunning.Rows[i]["TurnikeRow"];
                    if (!thread.IsAlive)
                    {
                        String turnikeNo = Convert.ToString(_threadsRunning.Rows[i]["TurnikeNo"]);
                        Int32 rowIndex = Convert.ToInt32(_table.Rows.IndexOf(_table.Select("TurnikeNo = " + turnikeNo)[0]));

                        Socket dataSocket = (Socket)_table.Rows[rowIndex]["Socket"];

                        try
                        {
                            dataSocket.Shutdown(SocketShutdown.Both);                            
                            while (dataSocket.Connected)
                            {
                            }
                            dataSocket.Close();
                        }
                        catch (Exception ex)
                        {
                        }

                        _table.Rows.RemoveAt(rowIndex);
                        _threadsRunning.Rows.RemoveAt(i);

                        Thread newThread = new Thread(new ParameterizedThreadStart(OkuyucuVeriAl));
                        newThread.Start(turnikeRow);

                        _threadsRunning.Rows.Add(newThread, Convert.ToInt32(turnikeNo), turnikeRow);
                    }
                }
            }
            _uptimer.Start();
        }
        #endregion



        #region "Sektör 8           Turnike Aç Butonlar"
        private void btn_t1_Click(object sender, EventArgs e)
        {
            DataRow[] clientRow = _table.Select("TurnikeNo = 1");
            if (clientRow.Length > 0) TurnikeAc(clientRow[0]);
        }

        private void btn_t2_Click(object sender, EventArgs e)
        {
            DataRow[] clientRow = _table.Select("TurnikeNo = 2");
            if (clientRow.Length > 0) TurnikeAc(clientRow[0]);
        }

        private void btn_t3_Click(object sender, EventArgs e)
        {
            DataRow[] clientRow = _table.Select("TurnikeNo = 3");
            if (clientRow.Length > 0) TurnikeAc(clientRow[0]);
        }

        private void TurnikeAc(DataRow clientRow)
        {
            try
            {
                Socket dataSocket = (Socket)clientRow["Socket"];
                Turnike turnike = (Turnike)clientRow["TurnikeClass"];

                ASCIIEncoding encoder = new ASCIIEncoding();
                Byte[] turnikeAcBytes = encoder.GetBytes(turnike.CihazID + turnike.Komut);

                try
                {
                    dataSocket.Send(turnikeAcBytes);
                }
                catch (Exception ex)
                {
                    if (ex is SocketException || ex is ObjectDisposedException)
                    {
                        if (!this.IsDisposed)
                        {
                            Logger.isError = true;
                            Logger.Method = "Main, TurnikeAc, Baglanti yok";
                            Logger.Message = ex.Message;
                        }
                    }
                }              
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
                {
                    Logger.isError = true;
                    Logger.Method = "Main, TurnikeAc";
                    Logger.Message = ex.Message;
                }
            }
        }
        #endregion


        private void OnClose(object sender, EventArgs e)
        {
            try
            {
                _uptimer.Stop();
                _timer.Stop();

                if (_table != null)
                {
                    for (Int32 i = 0; i < _table.Rows.Count; i++)
                    {
                        Boolean turnikeAktif = (Boolean)_table.Rows[i]["Aktif"];
                        turnikeAktif = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            Environment.Exit(0);
        }

        private void Logex(Exception logData)
        {
            //txtLog.Text += logData.Message + "\r\n";
        }

        private void btnMisafir_Click(object sender, EventArgs e)
        {
            LoginFrm loginfrm = new LoginFrm();
            Login.MisafirEkle = true;
            loginfrm.Show();
        }

        private void btnAyarlar_Click(object sender, EventArgs e)
        {
            LoginFrm loginfrm = new LoginFrm();
            Login.Settings = true;
            loginfrm.Show();
        }

        private void btnMisafirSil_Click(object sender, EventArgs e)
        {
            LoginFrm loginfrm = new LoginFrm();
            Login.MisafirSil = true;
            loginfrm.Show();
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            LoginFrm loginfrm = new LoginFrm();
            Login.Cikis = true;
            loginfrm.Show();
        }
    }
}
