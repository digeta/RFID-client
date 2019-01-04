using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rfidClient
{
    public class KartOkunduArgs : EventArgs
    {
        private Int64 kartid = 0;
        private Int64 tckimlik = 0;
        private String ad = "";
        private String soyad = "";

        public KartOkunduArgs(Int64 kartId, Int64 TCkimlik, String aD, String soyaD)
        {
            this.kartid = kartId;
            this.tckimlik = TCkimlik;
            this.ad = aD;
            this.soyad = soyaD;
        }

        public Int64 KartID
        {
            get
            {
                return kartid;
            }
        }

        public Int64 TCKimlik
        {
            get
            {
                return tckimlik;
            }
        }

        public String Ad
        {
            get
            {
                return ad;
            }
        }

        public String Soyad
        {
            get
            {
                return ad;
            }
        }
    }
}
