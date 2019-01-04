using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rfidClient
{
    public class DurumArgsHata : EventArgs
    {
        private String err;

        public DurumArgsHata(String hata)
        {
            this.err = hata;
        }

        public string Hata
        {
            get
            {
                return err;
            }
        }
    }

    public class DurumArgsMessage : EventArgs
    {
        private String msg;

        public DurumArgsMessage(String mesaj)
        {
            this.msg = mesaj;
        }

        public string Message
        {
            get
            {
                return msg;
            }
        }
    }

    public class DurumArgsStatus : EventArgs
    {
        private String status;

        public DurumArgsStatus(String durum)
        {
            this.status = durum;
        }

        public string Durum
        {
            get
            {
                return status;
            }
        }
    }
}
