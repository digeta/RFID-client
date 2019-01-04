using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace rfidClient
{
    public static class Settings
    {
        private static Boolean admin = false;
        private static String connStr = @"Server=localhost;Database=***;User Id=***;Password=***;";
        private static String connStrUnipa = @"Server=***;Database=***;User Id=***;Password=***;";
        private static String srvIp;
        private static Int32 listenPort;
        private static Int32 kontNok;
        private static String konum;
        private static DataTable ayarlar;
        private static DataTable turnikeler;

        public static Boolean AdminMod
        {
            get
            {
                return admin;
            }
            set
            {
                admin = value;
            }
        }

        public static String ConnectionStr
        {
            get
            {
                return connStr;
            }
            set
            {
                connStr = value;
            }
        }

        public static String ConnectionStrUnipa
        {
            get
            {
                return connStrUnipa;
            }
            set
            {
                connStrUnipa = value;
            }
        }

        public static String SunucuIP
        {
            get
            {
                return srvIp;
            }
            set
            {
                srvIp = value;
            }
        }

        public static Int32 OkuyucuPort
        {
            get
            {
                return listenPort;
            }
            set
            {
                listenPort = value;
            }
        }

        public static Int32 KontrolNokta
        {
            get
            {
                return kontNok;
            }
            set
            {
                kontNok = value;
            }
        }

        public static String Konum
        {
            get
            {
                return konum;
            }
            set
            {
                konum = value;
            }
        }

        public static DataTable Ayarlar
        {
            get
            {
                return ayarlar;
            }
            set
            {
                ayarlar = value;
            }
        }

        public static DataTable Turnikeler
        {
            get
            {
                return turnikeler;
            }
            set
            {
                turnikeler = value;
            }
        }
    }
}
