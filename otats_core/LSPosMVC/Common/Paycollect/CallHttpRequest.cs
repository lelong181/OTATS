using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace LSPosMVC.Common.Paycollect
{
    public class CallHttpRequest
    {
        public string HttpGetRequest(string auth, string url, string isoDate, string content)
        {
            Console.WriteLine("========== start call http get =========");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add(Authorization.X_OP_AUTHORIZATION_HEADER, auth);
            request.Headers.Add(Authorization.X_OP_DATE_HEADER, isoDate);
            request.Headers.Add(Authorization.X_OP_EXPIRES_HEADER, "3600");
            request.Headers.Add("Content-Type", "application/json");
            request.Headers.Add("Content-Length", content.Length.ToString());

            try
            {
                // Get the response
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine(result);
                    return result;
                }
            }
            catch (WebException ex)
            {
                // Handle exceptions
                if (ex.Response is HttpWebResponse errorResponse)
                {
                    Console.WriteLine($"Error: {errorResponse.StatusCode} - {errorResponse.StatusDescription}");
                    // Handle error response
                }
                else
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
                return "";
            }
        }

        public string HttpPutRequest(string auth, string url, string isoDate, string content)
        {
            Console.WriteLine("========== start call http put =========");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.ContentType = "application/json";
            request.Headers.Add(Authorization.X_OP_AUTHORIZATION_HEADER, auth);
            request.Headers.Add(Authorization.X_OP_DATE_HEADER, isoDate);
            request.Headers.Add(Authorization.X_OP_EXPIRES_HEADER, "3600");
            request.Headers.Add("Content-Type", "application/json");
            request.Headers.Add("Content-Length", content.Length.ToString());

            // Write the JSON data to the request stream
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(content);
                writer.Flush();
            }

            try
            {
                // Get the response
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine(result);
                    return result;
                }
            }
            catch (WebException ex)
            {
                // Handle exceptions
                if (ex.Response is HttpWebResponse errorResponse)
                {
                    Console.WriteLine($"Error: {errorResponse.StatusCode} - {errorResponse.StatusDescription}");
                    // Handle error response
                }
                else
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
                return "";
            }
        }

        public string HttpPostRequest(string auth, string url, string isoDate, string content)
        {
            Console.WriteLine("========== start call http put =========");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = content.Length;
            request.Headers.Add(Authorization.X_OP_AUTHORIZATION_HEADER, auth);
            request.Headers.Add(Authorization.X_OP_DATE_HEADER, isoDate);
            request.Headers.Add(Authorization.X_OP_EXPIRES_HEADER, "3600");
            //request.Headers.Add("Content-Type", "application/json");
            //request.Headers.Add("Content-Length", content.Length.ToString());

            // Write the JSON data to the request stream
            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(content);
                writer.Flush();
            }

            try
            {
                // Get the response
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine(result);
                    return result;
                }
            }
            catch (WebException ex)
            {
                // Handle exceptions
                if (ex.Response is HttpWebResponse errorResponse)
                {
                    Console.WriteLine($"Error: {errorResponse.StatusCode} - {errorResponse.StatusDescription}");
                    // Handle error response
                }
                else
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
                return "";
            }
        }
    }
}