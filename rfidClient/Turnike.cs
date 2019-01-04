using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rfidClient
{
    class Turnike
    {
        private System.Net.Sockets.Socket socket;
        private String cihazID = "";
        private String startKomut = "";
        private String komut = "";
        private String failKomut = "";
        private String ip = "";
        private Int32 port = 0;
        private Int32 turnikeNum = 0;
        private Boolean aktif = false;
        private Boolean baglaniyor = false;
        private DateTime aktifTar;

        public System.Net.Sockets.Socket TurnikeSocket
        {
            get
            {
                return socket;
            }
            set
            {
                socket = value;
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

        public String StartKomut
        {
            get
            {
                return startKomut;
            }
            set
            {
                startKomut = value;
            }
        }

        public String Komut
        {
            get
            {
                return komut;
            }
            set
            {
                komut = value;
            }
        }

        public String FailKomut
        {
            get
            {
                return failKomut;
            }
            set
            {
                failKomut = value;
            }
        }

        public String IP
        {
            get
            {
                return ip;
            }
            set
            {
                ip = value;
            }
        }

        public Int32 TurnikeNo
        {
            get
            {
                return turnikeNum;
            }
            set
            {
                turnikeNum = value;
            }
        }

        public Int32 Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
            }
        }

        public Boolean Aktif
        {
            get
            {
                return aktif;
            }
            set
            {
                aktif = value;
            }
        }

        public Boolean Baglaniyor
        {
            get
            {
                return baglaniyor;
            }
            set
            {
                baglaniyor = value;
            }
        }

        public DateTime AktifTarih
        {
            get
            {
                return aktifTar;
            }
            set
            {
                aktifTar = value;
            }
        }
    }
}
