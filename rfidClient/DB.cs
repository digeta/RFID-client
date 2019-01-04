using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace rfidClient
{
    public static class DB
    {        
        public static Boolean InitDB()
        {
            Boolean sonuc;
            try
            {
                AyarlarOku();
                CihazListesi();
                sonuc = true;
            }
            catch (Exception ex)
            {
                sonuc = false;
            }
            return sonuc;
        }

        public static void AyarlarOku()
        {
            SqlConnection sqlConn = new SqlConnection();

            try
            {
                sqlConn.ConnectionString = Settings.ConnectionStr;

                String sqlStr = "SELECT * FROM AYARLAR";
                SqlCommand sqlComm = new SqlCommand(sqlStr, sqlConn);

                if (sqlConn.State != System.Data.ConnectionState.Open) sqlConn.Open();

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlComm);
                DataTable dt = new DataTable();
                sqlAdapter.Fill(dt);

                Settings.Ayarlar = dt;
                
                Settings.SunucuIP = Convert.ToString(dt.Rows[0]["SUNUCU_IP"]);
                Settings.OkuyucuPort = Convert.ToInt32(dt.Rows[0]["OKUYUCU_PORT"]);            
            }
            catch (Exception ex)
            {
                if (sqlConn.State != System.Data.ConnectionState.Closed) sqlConn.Close();               
            }
        }

        public static void CihazListesi()
        {
            SqlConnection sqlConn = new SqlConnection();

            try
            {
                sqlConn.ConnectionString = Settings.ConnectionStr;

                String sqlStr = "SELECT * FROM TURNIKE";
                SqlCommand sqlComm = new SqlCommand(sqlStr, sqlConn);

                if (sqlConn.State != System.Data.ConnectionState.Open) sqlConn.Open();

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlComm);
                DataTable dt = new DataTable();
                sqlAdapter.Fill(dt);

                Settings.Turnikeler = dt;
            }
            catch (Exception ex)
            {
                if (sqlConn.State != System.Data.ConnectionState.Closed) sqlConn.Close();
            }
        }

        public static Boolean GirisYap()
        {
            Boolean result = false;
            SqlConnection sqlConn = new SqlConnection();

            try
            {
                sqlConn.ConnectionString = Settings.ConnectionStr;

                String sqlStr = "SELECT K.ID, ISNULL(K.ADI,'-') AS ADI, ISNULL(K.SOYADI,'-') AS SOYADI, "+
                    "ISNULL(B.BIRIMAD,'-') AS BIRIMI FROM KULLANICILAR AS K WITH(NOLOCK)" +
                    " LEFT OUTER JOIN BIRIMLER AS B WITH(NOLOCK) ON K.BIRIM = B.ID WHERE K.AKTIF = 1 AND K.KULLANICI = @KULLANICI AND PAROLA = @PAROLA";

                SqlCommand sqlComm = new SqlCommand(sqlStr, sqlConn);
                sqlComm.Parameters.AddWithValue("KULLANICI", Login.Kullanici);
                sqlComm.Parameters.AddWithValue("PAROLA", Login.Parola);
                
                if (sqlConn.State != System.Data.ConnectionState.Open) sqlConn.Open();

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlComm);
                DataTable dt = new DataTable();
                sqlAdapter.Fill(dt);

                Login.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                Login.Ad = Convert.ToString(dt.Rows[0]["ADI"]);
                Login.Soyad = Convert.ToString(dt.Rows[0]["SOYADI"]);
                Login.Birim = Convert.ToString(dt.Rows[0]["BIRIMI"]);
                
                if (dt.Rows.Count > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (sqlConn.State != System.Data.ConnectionState.Closed) sqlConn.Close();
                result = false;
            }
            return result;
        }

        public static Boolean MisafirEkle(Kart kart)
        {
            Boolean result = false;
            SqlConnection sqlConn = new SqlConnection();

            try
            {
                sqlConn.ConnectionString = Settings.ConnectionStr;

                String sqlStr = "IF NOT EXISTS(SELECT 1 FROM KARTLAR WHERE KART_ID = @KART_ID AND IPTAL = 0)"+
                    "INSERT INTO KARTLAR (TCKIMLIK, KART_ID, AD, SOYAD, KART_TIP, KAYIT_EDEN)" +
                    " VALUES(@TCKIMLIK, @KART_ID, @AD, @SOYAD, @KART_TIP, @KAYIT_EDEN)";

                SqlCommand sqlComm = new SqlCommand(sqlStr, sqlConn);
                sqlComm.Parameters.AddWithValue("TCKIMLIK", kart.Tckimlik);
                sqlComm.Parameters.AddWithValue("KART_ID", kart.Kartno);
                sqlComm.Parameters.AddWithValue("AD", kart.Ad);
                sqlComm.Parameters.AddWithValue("SOYAD", kart.Soyad);
                sqlComm.Parameters.AddWithValue("KART_TIP", 4);
                sqlComm.Parameters.AddWithValue("KAYIT_EDEN", kart.Kaydeden);

                if (sqlConn.State != System.Data.ConnectionState.Open) sqlConn.Open();

                Int32 inserted = sqlComm.ExecuteNonQuery();
                if (inserted > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (sqlConn.State != System.Data.ConnectionState.Closed) sqlConn.Close();
                result = false;
            }
            return result;
        }

        public static Kart MisafirGetir(Int64 tckimlik)
        {
            Kart kart = new Kart();
            SqlConnection sqlConn = new SqlConnection();

            try
            {
                sqlConn.ConnectionString = Settings.ConnectionStr;

                String sqlStr = "SELECT ISNULL(K.KART_ID, 0) AS KART_ID, ISNULL(K.TCKIMLIK, 0) AS TCKIMLIK, ISNULL(K.AD,'') AS ADI, " +
                    "ISNULL(K.SOYAD,'') AS SOYADI, ISNULL(B.BIRIMAD,'') AS BIRIMI FROM KARTLAR AS K WITH(NOLOCK)" +
                    " LEFT OUTER JOIN BIRIMLER AS B WITH(NOLOCK) ON K.BIRIM = B.ID WHERE K.IPTAL = 0 AND K.KART_TIP = 4 AND K.TCKIMLIK = @TCKIMLIK";

                SqlCommand sqlComm = new SqlCommand(sqlStr, sqlConn);
                sqlComm.Parameters.AddWithValue("TCKIMLIK", tckimlik);

                if (sqlConn.State != System.Data.ConnectionState.Open) sqlConn.Open();

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlComm);
                DataTable dt = new DataTable();
                sqlAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {                    
                    kart.Kartno = Convert.ToInt64(dt.Rows[0]["KART_ID"]);
                    kart.Ad = Convert.ToString(dt.Rows[0]["ADI"]);
                    kart.Soyad = Convert.ToString(dt.Rows[0]["SOYADI"]);
                    kart.Birim = Convert.ToString(dt.Rows[0]["BIRIMI"]);
                }
            }
            catch (Exception ex)
            {
                if (sqlConn.State != System.Data.ConnectionState.Closed) sqlConn.Close();
            }
            return kart;
        }

        public static Boolean MisafirSil(Int64 kartNo)
        {
            Boolean result = false;
            SqlConnection sqlConn = new SqlConnection();

            try
            {
                sqlConn.ConnectionString = Settings.ConnectionStr;    
          
                String sqlStr = "UPDATE KARTLAR SET IPTAL = 1, IPTAL_EDEN = @IPTAL_EDEN, IPTAL_TAR = @IPTAL_TAR" +
                    " WHERE KART_ID = @KART_ID";

                SqlCommand sqlComm = new SqlCommand(sqlStr, sqlConn);
                sqlComm.Parameters.AddWithValue("KART_ID", kartNo);
                sqlComm.Parameters.AddWithValue("IPTAL_EDEN", Login.ID);
                sqlComm.Parameters.AddWithValue("IPTAL_TAR", DateTime.Now);

                if (sqlConn.State != System.Data.ConnectionState.Open) sqlConn.Open();

                Int32 updated = sqlComm.ExecuteNonQuery();
                result = (updated > 0) ? true : false;
            }
            catch (Exception ex)
            {
                if (sqlConn.State != System.Data.ConnectionState.Closed) sqlConn.Close();
                result = false;
            }
            return result;
        }


        public static Kart KartKontrol(Int64 kartNo)
        {
            Kart kart = new Kart();
            SqlConnection sqlConn = new SqlConnection();

            try
            {
                sqlConn.ConnectionString = Settings.ConnectionStr;

                String sqlStr = "SELECT ISNULL(K.KART_ID, 0) AS KART_ID, ISNULL(K.TCKIMLIK, 0) AS TCKIMLIK," +
                    " UPPER(LEFT(K.AD, 1)) + LOWER(SUBSTRING(K.AD, 2, LEN(K.AD))) AS ADI," +
                    " ISNULL(K.SOYAD, '') AS SOYADI, ISNULL(B.BIRIMAD, '') AS BIRIMI," +
                    " K.KART_TIP AS KART_TIPID," +
                    " CASE K.KART_TIP" +
                    " WHEN 1 THEN 'Öğrenci'" +
                    " WHEN 2 THEN 'Akademik Personel'" +
                    " WHEN 3 THEN 'İdari Personel'" +
                    " WHEN 4 THEN 'Ziyaretçi'" +
                    " WHEN 5 THEN 'Hizmet Alım Personeli'" +
                    " END AS KART_TIP," +
                    " K.GECIS_ONAY " +
                    " FROM KARTLAR AS K WITH(NOLOCK)" +
                    " LEFT OUTER JOIN BIRIMLER AS B WITH(NOLOCK) ON K.BIRIM = B.BIRIMID WHERE K.IPTAL = 0 AND K.KART_ID = @KART_ID";

                SqlCommand sqlComm = new SqlCommand(sqlStr, sqlConn);
                sqlComm.Parameters.AddWithValue("KART_ID", kartNo);

                if (sqlConn.State != System.Data.ConnectionState.Open) sqlConn.Open();

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlComm);
                DataTable dt = new DataTable();
                sqlAdapter.Fill(dt);

                kart.Kartno = kartNo;
                kart.Resim = Properties.Resources.nopicture;

                if (dt.Rows.Count > 0)
                {
                    kart.Tckimlik = Convert.ToInt64(dt.Rows[0]["TCKIMLIK"]);
                    kart.Ad = Convert.ToString(dt.Rows[0]["ADI"]);
                    kart.Soyad = Convert.ToString(dt.Rows[0]["SOYADI"]);
                    kart.Birim = Convert.ToString(dt.Rows[0]["BIRIMI"]);
                    kart.KartTip = Convert.ToString(dt.Rows[0]["KART_TIP"]);
                    kart.KartTipid = Convert.ToInt32(dt.Rows[0]["KART_TIPID"]);
                    try
                    {
                        if (kart.KartTipid == 1)
                        {
                            kart.Resim = Bitmap.FromStream(WebRequest.Create("http://ekampus.beun.edu.tr/Content/Ogrenci/" + kart.Tckimlik + ".jpg").GetResponse().GetResponseStream());
                        }
                        if (kart.KartTipid == 5)
                        {
                            kart.Resim = Bitmap.FromStream(WebRequest.Create("http://10.1.16.30/report/5/" + kart.Tckimlik + ".jpg").GetResponse().GetResponseStream());
                        }
                    }
                    catch (Exception ex)
                    {
                        //Logger.Message = ex.Message + " - resim";
                    }
                    kart.GecisOnay = Convert.ToBoolean(dt.Rows[0]["GECIS_ONAY"]);
                }
            }
            catch (Exception ex)
            {
                if (sqlConn.State != System.Data.ConnectionState.Closed) sqlConn.Close();
                Logger.Message = ex.Message + " - kartKontrol";
            }
            return kart;
        }

        public static Kart ResimGetir(Int64 tckimlik)
        {
            Kart kart = new Kart();

            SqlConnection sqlkonn = new SqlConnection();

            try
            {
                sqlkonn.ConnectionString = "Server=***;Database=***;User Id=***;Password=***;";
                String sqlStr = "SELECT O.Adi AS BirimAd, NI.Foto FROM Personel AS P " +
                "INNER JOIN Memur AS M ON M.Personel = P.PersonelID " +
                "INNER JOIN Kadro AS K ON K.KadroID = M.Kadro " +
                "INNER JOIN Organizasyon AS O ON O.OrganizasyonID = K.Organizasyon " +
                "INNER JOIN Nufus AS N ON N.Personel = P.PersonelID " +
                "INNER JOIN NufusImage AS NI ON NI.Nufus = N.NufusID " +
                "WHERE P.KimlikNo = @TCKIMLIK";

                SqlCommand sqlComm = new SqlCommand(sqlStr, sqlkonn);
                sqlComm.Parameters.AddWithValue("TCKIMLIK", tckimlik);

                if (sqlkonn.State != System.Data.ConnectionState.Open) sqlkonn.Open();
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlComm);
                DataTable dt = new DataTable();
                sqlAdapter.Fill(dt);

                kart.Resim = Properties.Resources.nopicture;

                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        kart.Birim = Convert.ToString(dt.Rows[0]["BirimAd"]);
                        Byte[] resim = (Byte[])(dt.Rows[0]["Foto"]);
                        MemoryStream mem = new MemoryStream(resim);
                        kart.Resim = Image.FromStream(mem);
                        //kart.Resim = Bitmap.FromStream(WebRequest.Create("http://ekampus.beun.edu.tr/Content/Ogrenci/" + kart.Tckimlik + ".jpg").GetResponse().GetResponseStream());
                    }
                    catch (Exception ex)
                    {
                        Logger.Message = ex.Message + " - resimGetir";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Message = ex.Message + " - resimGetirMain";
                if (Settings.AdminMod)
                {
                    //Info.ErrorDesc = "Kart kaydı bulunamadı! | " + ex.Message;
                }
                else
                {
                    //Info.ErrorDesc = "Kart kaydı bulunamadı!";
                }
            }
            finally
            {
                if (sqlkonn.State != System.Data.ConnectionState.Closed) sqlkonn.Close();
            }
            return kart;
        }

        public static Int32 GecisKaydet(Kart kart)
        {
            Int32 inserted = 0;
            SqlConnection sqlConn = new SqlConnection();
            try
            {
                sqlConn.ConnectionString = Settings.ConnectionStr;

                String sqlStr = "INSERT INTO GECISLER (KART_ID, TURNIKE_NO, BASARILI) VALUES(@KART_ID, @TURNIKE_NO, @BASARILI)" +
                    ";Select Scope_Identity()";

                SqlCommand sqlComm = new SqlCommand(sqlStr, sqlConn);
                sqlComm.Parameters.AddWithValue("KART_ID", kart.Kartno);
                sqlComm.Parameters.AddWithValue("TURNIKE_NO", kart.TurnikeNo);
                sqlComm.Parameters.AddWithValue("BASARILI", (kart.GecisOnay) ? 1 : 0);

                if (sqlConn.State != System.Data.ConnectionState.Open) sqlConn.Open();

                inserted = Convert.ToInt32(sqlComm.ExecuteScalar());

                if (!kart.GecisOnay)
                {
                    Logger.KayıtID = inserted;
                    Logger.Message = String.Format("Geçersiz kart. Kart No : {0}", kart.Kartno);
                }
            }
            catch (Exception ex)
            {
                Logger.Message = ex.Message + " - gecisKaydi";
                if (Settings.AdminMod)
                {
                    //Info.ErrorDesc = "Kart kaydı bulunamadı! | " + ex.Message;
                }
                else
                {
                    //Info.ErrorDesc = "Kart kaydı bulunamadı!";
                }
            }
            return inserted;
        }

        public static DataTable GecisListe(Int32 turnikeNo)
        {
            DataTable gecisler = new DataTable();

            SqlConnection sqlConn = new SqlConnection();
            try
            {
                sqlConn.ConnectionString = Settings.ConnectionStr;
                String sqlStr = "SELECT UPPER(LEFT(K.AD, 1)) + LOWER(SUBSTRING(K.AD, 2, LEN(K.AD))) AS ADI," +
                                " UPPER(K.SOYAD) AS SOYADI," +
                                " CASE K.KART_TIP" +
                                " WHEN 1 THEN 'Öğrenci'" +
                                " WHEN 2 THEN 'Akademisyen'" +
                                " WHEN 3 THEN 'İdari Personel' END AS KART_TIP," +
                                " CASE K.GECIS_ONAY" +
                                " WHEN 1 THEN 'Onaylı'" +
                                " WHEN 0 THEN 'Onaysız' END AS GECIS," +
                                " G.GECIS_TAR AS GECIS_TARIH" +
                                " FROM KARTLAR AS K WITH (NOLOCK)" +
                                " INNER JOIN GECISLER AS G WITH (NOLOCK)" +
                                " ON K.KART_ID = G.KART_ID" +
                                " WHERE K.IPTAL = 0 AND G.BASARILI = 1 AND TURNIKE_NO = @TURNIKE_NO" +
                                " ORDER BY G.ID DESC";

                SqlCommand sqlComm = new SqlCommand(sqlStr, sqlConn);
                sqlComm.Parameters.AddWithValue("TURNIKE_NO", turnikeNo);

                if (sqlConn.State != System.Data.ConnectionState.Open) sqlConn.Open();
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlComm);
                sqlAdapter.Fill(gecisler);
            }
            catch (Exception ex)
            {
                Logger.Message = ex.Message + " - gecisListe";
                if (Settings.AdminMod)
                {
                    //Info.ErrorDesc = "Kart kaydı bulunamadı! | " + ex.Message;
                }
                else
                {
                    //Info.ErrorDesc = "Kart kaydı bulunamadı!";
                }
            }
            return gecisler;
        }

        public static void LogKaydiYap()
        {
            Thread logThread = new Thread(new ThreadStart(Logla));
            logThread.Start();
        }

        private static void Logla()
        {
            SqlConnection sqlConn = new SqlConnection();
            try
            {
                sqlConn.ConnectionString = Settings.ConnectionStr;
                String sqlStr = "INSERT INTO LOG (ACIKLAMA, METHOD, KAYITID, HATA) VALUES(@ACIKLAMA, @METHOD, @KAYITID, @HATA)";
                SqlCommand sqlComm = new SqlCommand(sqlStr, sqlConn);
                sqlComm.Parameters.AddWithValue("ACIKLAMA", Logger.Message);
                sqlComm.Parameters.AddWithValue("METHOD", Logger.Method);
                sqlComm.Parameters.AddWithValue("KAYITID", Logger.KayıtID);
                sqlComm.Parameters.AddWithValue("HATA", Logger.isError);

                if (sqlConn.State != System.Data.ConnectionState.Open) sqlConn.Open();

                Int32 inserted = sqlComm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (Settings.AdminMod)
                {
                    //Info.ErrorDesc = "Ayarlar Veritabanından Okunamadı! | " + ex.Message;
                }
                else
                {
                    //Info.ErrorDesc = "Ayarlar Veritabanından Okunamadı!";
                }            
            }
        }
    }
}
