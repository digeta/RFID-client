using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rfidClient
{
    public static class Login
    {
        private static Int32 id;
        private static String ad;
        private static String soyad;
        private static String kullanici;
        private static String parola;
        private static String birim;
        private static Boolean basarili = false;
        private static Boolean delUser = false;
        private static Boolean newUser = false;
        private static Boolean settings = false;
        private static Boolean cikis = false;

        public static Int32 ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public static String Ad
        {
            get
            {
                return ad;
            }
            set
            {
                ad = value;
            }
        }

        public static String Soyad
        {
            get
            {
                return soyad;
            }
            set
            {
                soyad = value;
            }
        }

        public static String Birim
        {
            get
            {
                return birim;
            }
            set
            {
                birim = value;
            }
        }

        public static String Kullanici
        {
            get
            {
                return kullanici;
            }
            set
            {
                kullanici = value;
            }
        }

        public static String Parola
        {
            get
            {
                return parola;
            }
            set
            {
                parola = value;
            }
        }

        public static Boolean Basarili
        {
            get
            {
                return basarili;
            }
            set
            {
                basarili = value;
            }
        }

        public static Boolean MisafirSil
        {
            get
            {
                return delUser;
            }
            set
            {
                delUser = value;
            }
        }

        public static Boolean MisafirEkle
        {
            get
            {
                return newUser;
            }
            set
            {
                newUser = value;
            }
        }

        public static Boolean Settings
        {
            get
            {
                return settings;
            }
            set
            {
                settings = value;
            }
        }

        public static Boolean Cikis
        {
            get
            {
                return cikis;
            }
            set
            {
                cikis = value;
            }
        }
    }
}
