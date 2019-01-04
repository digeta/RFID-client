using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rfidClient.KartEvents
{
    class GecislerArgs : EventArgs
    {
        private String data = "";

        public GecislerArgs(String datas)
        {
            this.data = datas;
        }
        
        public string Data
        {
            get
            {
                return data;
            }
        }
    }

    class GecislerArgsArray : EventArgs
    {
        private String[] data = { "", "" };

        public GecislerArgsArray(String[] datas)
        {
            this.data = datas;
        }

        public string[] Data
        {
            get
            {
                return data;
            }
        }
    }

    class GecislerArgsBool : EventArgs
    {
        private Boolean aktifStream = false;

        public GecislerArgsBool(Boolean streamAktif)
        {
            this.aktifStream = streamAktif;
        }

        public Boolean StreamAktif
        {
            get
            {
                return aktifStream;
            }
        }
    }

    class ZamanlarArgsArray : EventArgs
    {
        private String[] zamanlar = { "", "", "" };

        public ZamanlarArgsArray(String[] zamans)
        {
            this.zamanlar = zamans;
        }

        public string[] Zamanlar
        {
            get
            {
                return zamanlar;
            }
        }
    }
}
