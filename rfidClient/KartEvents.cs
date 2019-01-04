using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rfidClient.KartEvents
{
    class Gecisler
    {
        private static String dataStr = "";

        public static event EventHandler<GecislerArgs> OnKartOkuma;

        public static String OkunanData
        {
            get
            {
                return dataStr;
            }
            set
            {
                dataStr = value;
                OnKartOkuma(null, new GecislerArgs(dataStr));
            }
        }
    }

    class GecislerArray
    {
        private static String[] dataStr = { "", "" };

        public static event EventHandler<GecislerArgsArray> OnKartOkuma;

        public static String[] OkunanData
        {
            get
            {
                return dataStr;
            }
            set
            {
                dataStr = value;
                OnKartOkuma(null, new GecislerArgsArray(dataStr));
            }
        }
    }

    class GecislerBool
    {
        private static Boolean streamAktif = false;

        public static event EventHandler<GecislerArgsBool> OnKartStream;

        public static Boolean StreamAktif
        {
            get
            {
                return streamAktif;
            }
            set
            {
                streamAktif = value;
                OnKartStream(null, new GecislerArgsBool(streamAktif));
            }
        }
    }

    class ZamanlarArray
    {
        private static String[] zamanStr = { "", "", "" };

        public static event EventHandler<ZamanlarArgsArray> OnZaman;

        public static String[] OkunanZaman
        {
            get
            {
                return zamanStr;
            }
            set
            {
                zamanStr = value;
                OnZaman(null, new ZamanlarArgsArray(zamanStr));
            }
        }
    }
}
