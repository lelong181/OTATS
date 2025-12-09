using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LSPosMVC.Common
{
    public class Common
    {
        public static byte[] GetBytesFromFileImage(string fullFilePath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)
            FileStream fs = File.OpenRead(fullFilePath);
            try
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                return bytes;
            }
            finally
            {
                fs.Close();

            }
        }
    }
}