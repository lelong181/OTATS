using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Utilities
{
    public class Log
    {
        private static ILog log = LogManager.GetLogger(typeof(Log));

        public static void Error(Exception ex)
        {
            log.Error(ex);
        }

        public static void Info(string message)
        {
            log.Info(message);
        }

        public static void BeginMethod(string MethodName, Hashtable lstParameter)
        {
            log.Info("Begin Method ==> Parameters: ");
            int num = 0;
            foreach (DictionaryEntry item in lstParameter)
            {
                log.Info(string.Concat(" - ", num, " -> Parameter Name : ", item.Key, " - Parameter Value : ", item.Value));
            }
        }

        public static void EndMethod(string MethodName, string returnValue)
        {
            log.Info("End Method ==> Return: " + returnValue);
        }
    }

}
