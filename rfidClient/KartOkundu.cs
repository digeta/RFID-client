using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rfidClient
{
    public static class KartOkundu
    {
        private static Int64 kartid = 0;
        private static Int64 tckimlik = 0;
        private static String ad = "";
        private static String soyad = "";
        private static Boolean newUser = false;
        private static Boolean delUser = false;

        public static event EventHandler<KartOkunduArgs> OnKartOkundu;

        public static Int64 KartID
        {
            get
            {
                return kartid;
            }
            set
            {
                kartid = value;
                OnKartOkundu(null, new KartOkunduArgs(kartid, tckimlik, ad, soyad));
            }
        }

        public static Int64 TCKimlik
        {
            get
            {
                return tckimlik;
            }
            set
            {
                tckimlik = value;                
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

        public static Boolean NewUser
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

        public static Boolean DelUser
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
    }
}
