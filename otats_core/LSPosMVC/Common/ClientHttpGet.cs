
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

    public class ClientHttpGet
    {
        private static ClientHttpGet INSTANCE = null;
        static HttpClient client;
        public ClientHttpGet()
        {
            client = new HttpClient();
        }

        public static ClientHttpGet getInstance()
        {
            if (INSTANCE == null)
            {
                INSTANCE = new ClientHttpGet();
            }
            return INSTANCE;
        }

        public string DatTourHttpGet(string path)
        {
            HttpWebRequest http = (HttpWebRequest)WebRequest.Create(path);
            http.Method = "GET";
            http.ContentType = "application/json; charset=utf-8";
            try
            {
                WebResponse webresponse = http.GetResponse();
                Stream stream = webresponse.GetResponseStream();
                //MemoryStream stream = new MemoryStream();
                //webresponse.GetResponse();
                StreamReader sr = new StreamReader(stream);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string DatTourHttpGetWebClient(string path)
        {
            try
            {
                WebClient client = new WebClient();
                client.Encoding = System.Text.Encoding.UTF8;
                var text = client.DownloadString(path);
                return text;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }