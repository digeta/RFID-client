using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace rfidClient
{
    public class Kart
    {
        private String ad = "";
        private String soyad = "";
        private Int64 tckimlik = 0;
        private Int64 kartno = 0;
        private String birimi = "";
        private Image resim;
        private Int32 kayiteden = 0;
        private String kartTip = "";
        private Int32 kartTipid = 0;
        private Boolean gecisOnay = false;
        private String cihazID = "";
        private Int32 turnikeNo = 0;
        private DateTime gecisTar;
         
        public String Ad
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

        public String Soyad
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

        public Int64 Tckimlik
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

        public Int64 Kartno
        {
            get
            {
                return kartno;
            }
            set
            {
                kartno = value;
            }
        }

        public String Birim
        {
            get
            {
                return birimi;
            }
            set
            {
                birimi = value;
            }
        }

        public Image Resim
        {
            get
            {
                return resim;
            }
            set
            {
                resim = value;
            }
        }

        public Int32 Kaydeden
        {
            get
            {
                return kayiteden;
            }
            set
            {
                kayiteden = value;
            }
        }

        public Int32 KartTipid
        {
            get
            {
                return kartTipid;
            }
            set
            {
                kartTipid = value;
            }
        }

        public String KartTip
        {
            get
            {
                return kartTip;
            }
            set
            {
                kartTip = value;
            }
        }

        public Boolean GecisOnay
        {
            get
            {
                return gecisOnay;
            }
            set
            {
                gecisOnay = value;
            }
        }

        public String CihazID
        {
            get
            {
                return cihazID;
            }
            set
            {
                cihazID = value;
            }
        }

        public Int32 TurnikeNo
        {
            get
            {
                return turnikeNo;
            }
            set
            {
                turnikeNo = value;
            }
        }

        public DateTime GecisTarihi
        {
            get
            {
                return gecisTar;
            }
            set
            {
                gecisTar = value;
            }
        }
    }
}
