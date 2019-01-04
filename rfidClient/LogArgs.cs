using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rfidClient
{
    public class LogArgs : EventArgs
    {
        private String msg;
        private String meth;
        private Int32 kaytid;
        private Boolean isErr;

        public LogArgs(String message, String method, Int32 kayitID, Boolean isError)
        {
            this.msg = message;
            this.meth = method;
            this.kaytid = kayitID;
            this.isErr = isError;
        }

        public string Message
        {
            get
            {
                return msg;
            }
        }

        public string Method
        {
            get
            {
                return meth;
            }
        }

        public Int32 KayıtID
        {
            get
            {
                return kaytid;
            }
        }

        public Boolean isError
        {
            get
            {
                return isErr;
            }
        }
    }
}
