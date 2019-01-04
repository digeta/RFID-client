using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rfidClient
{
    public static class Durumlar
    {
        private static String hataStr;
        private static String msgStr;
        private static String durumStr;

        public static event EventHandler<DurumArgsHata> OnErrorChanged;
        public static event EventHandler<DurumArgsMessage> OnMessageChanged;
        public static event EventHandler<DurumArgsStatus> OnStatusChanged;

        public static String HataDetay
        {
            get
            {
                return hataStr;
            }
            set
            {
                hataStr = value;
                OnErrorChanged(null, new DurumArgsHata(hataStr));
            }
        }

        public static String Mesaj
        {
            get
            {
                return msgStr;
            }
            set
            {
                msgStr = value;
                OnMessageChanged(null, new DurumArgsMessage(msgStr));
            }
        }

        public static String Durum
        {
            get
            {
                return durumStr;
            }
            set
            {
                durumStr = value;
                OnStatusChanged(null, new DurumArgsStatus(durumStr));
            }
        }
    }
}
