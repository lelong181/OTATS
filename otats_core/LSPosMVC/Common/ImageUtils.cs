using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LSPosMVC.Common
{
    public class ImageUtils
    {

        public string Capture(string imageParts)
        {
            using (var client = new WebClient())
            {
                Mainrequests Mainrequests = new Mainrequests()
                {
                    requests = new List<requests>()
                {
                     new requests()
                {
                     image = new image()
                     {
                     content = imageParts
                 },

                 features = new List<features>()
                 {
                     new features()
                     {
                         type = "DOCUMENT_TEXT_DETECTION",
                     }

                 }

             }

             }

                };

                string GG_APIKey = "AIzaSyA8nFWV2zIy2-zTlNNC1PeEBAB7O35eZcs";
                var uri = "https://vision.googleapis.com/v1/images:annotate?key=" + GG_APIKey;
                client.Headers.Add("Content-Type:application/json");
                client.Headers.Add("Accept:application/json");
                client.Encoding = Encoding.UTF8;
                var response = client.UploadString(uri, JsonConvert.SerializeObject(Mainrequests));
                string s = JObject.Parse(response)["responses"][0]["fullTextAnnotation"]["text"].ToString();
                return s;
            }
        }
    }
    public class Mainrequests
    {
        public List<requests> requests { get; set; }
    }

    public class requests
    {
        public image image { get; set; }
        public List<features> features { get; set; }
    }

    public class image
    {
        public string content { get; set; }
    }
    public class features
    {
        public string type { get; set; }
    }
}
