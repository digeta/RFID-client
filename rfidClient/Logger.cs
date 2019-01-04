using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rfidClient
{
    public static class Logger
    {
        private static String logStr = "";
        private static String methStr = "";
        private static Int32 kayitId = 0;
        private static Boolean isErr = false;

        public static event EventHandler<LogArgs> OnMessageChanged;
        
        public static String Message
        {
            get
            {
                return logStr;
            }
            set
            {
                logStr = value;
                OnMessageChanged(null, new LogArgs(logStr, methStr, kayitId, isErr));
            }
        }

        public static String Method
        {
            get
            {
                return methStr;
            }
            set
            {
                methStr = value;
                //OnMessageChanged(null, new LogArgs(logStr, methStr, isErr));
            }
        }

        public static Int32 KayıtID
        {
            get
            {
                return kayitId;
            }
            set
            {
                kayitId = value;
            }
        }

        public static Boolean isError
        {
            get
            {
                return isErr;
            }
            set
            {
                isErr = value;
                //OnMessageChanged(null, new LogArgs(logStr, methStr, isErr));
            }
        }
    }
}
