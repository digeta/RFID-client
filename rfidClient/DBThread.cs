using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;

namespace rfidClient
{
    class DBThread
    {
        public Kart KartKontrol(Int64 kartNo)
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
                    " LEFT OUTER JOIN BIRIMLER AS B WITH(NOLOCK)" +
                    " ON K.BIRIM = B.BIRIMID WHERE K.IPTAL = 0 AND K.KART_ID = @KART_ID";

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
                    kart.GecisOnay = Convert.ToBoolean(dt.Rows[0]["GECIS_ONAY"]);

                    try
                    {
                        String url = "";

                        if (kart.KartTipid == 1)
                        {
                            url = @"http://ekampus.beun.edu.tr/Content/Ogrenci/" + Convert.ToString(kart.Tckimlik) + ".jpg";
                        }
                        if (kart.KartTipid == 5)
                        {
                            url = @"http://10.1.16.30/report/5/" + Convert.ToString(kart.Tckimlik) + ".jpg";
                        }

                        WebRequest request = WebRequest.Create(url);
                        //request.Timeout = 2000;
                        WebResponse response = request.GetResponse();
                        kart.Resim = Bitmap.FromStream(response.GetResponseStream());
                    }
                    catch (Exception ex)
                    {
                        Logger.Message = ex.Message + " - resim";
                    }                    
                }
            }
            catch (Exception ex)
            {
                Logger.Method = "DBThread, Kart Kontrol";
                Logger.isError = true;
                Logger.Message = ex.Message;
            }
            finally
            {
                if (sqlConn.State != System.Data.ConnectionState.Closed) sqlConn.Close();
            }
            return kart;
        }

        public Kart ResimGetir(Int64 tckimlik)
        {
            Kart kart = new Kart();
            SqlConnection sqlConn = new SqlConnection();

            try
            {
                sqlConn.ConnectionString = Settings.ConnectionStrUnipa;

                String sqlStr = "SELECT O.Adi AS BirimAd, NI.Foto FROM Personel AS P WITH(NOLOCK)" +
                " INNER JOIN Memur AS M WITH(NOLOCK) ON M.Personel = P.PersonelID " +
                " INNER JOIN Kadro AS K WITH(NOLOCK) ON K.KadroID = M.Kadro " +
                " INNER JOIN Organizasyon AS O WITH(NOLOCK) ON O.OrganizasyonID = K.Organizasyon " +
                " INNER JOIN Nufus AS N WITH(NOLOCK) ON N.Personel = P.PersonelID " +
                " INNER JOIN NufusImage AS NF WITH(NOLOCK) ON NF.Nufus = N.NufusID " +
                " WHERE P.KimlikNo = @TCKIMLIK";

                SqlCommand sqlComm = new SqlCommand(sqlStr, sqlConn);
                sqlComm.Parameters.AddWithValue("TCKIMLIK", tckimlik);

                if (sqlConn.State != System.Data.ConnectionState.Open) sqlConn.Open();
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
                    }
                    catch (Exception ex)
                    {
                        //Logger.Message = ex.Message + " - resimGetir";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Method = "DBThread, Resim Getir";
                Logger.isError = true;
                Logger.Message = ex.Message;
            }
            finally
            {
                if (sqlConn.State != System.Data.ConnectionState.Closed) sqlConn.Close();
            }
            return kart;
        }

        public Int32 GecisKaydet(Kart kart)
        {
            Int32 inserted = 0;
            SqlConnection sqlConn = new SqlConnection();

            try
            {
                sqlConn.ConnectionString = Settings.ConnectionStr;

                String sqlStr = "INSERT INTO GECISLER (KART_ID, TURNIKE_NO, BASARILI)" +
                    " VALUES(@KART_ID, @TURNIKE_NO, @BASARILI)" +
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
                    Logger.Message = "Geçersiz kart. Kart No : " + Convert.ToString(kart.Kartno);
                }
            }
            catch (Exception ex)
            {
                Logger.isError = true;
                Logger.Method = "DBThread, Geçiş Kaydı";
                Logger.Message = ex.Message;
            }
            finally
            {
                if (sqlConn.State != System.Data.ConnectionState.Closed) sqlConn.Close();
            }
            return inserted;
        }
    }
}
