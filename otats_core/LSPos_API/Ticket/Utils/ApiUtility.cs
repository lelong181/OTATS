using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Business.Model;
using log4net;
using LSPos_API.Model.RequestModel;
using LSPos_API.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Ticket.Utils
{
    public static class ApiUtility
    {
        public static string ApiUrl = "";
        private static ILog log = LogManager.GetLogger(typeof(ApiUtility));

        public static List<T> CallApi<T>(string endpoint, Method method, object dataObject = null, string localApi = "")
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true;
                RestClient client = new RestClient
                {
                    BaseUrl = string.IsNullOrEmpty(localApi) ? ApiUrl : localApi
                };
                RestRequest request = new RestRequest("pos/" + endpoint, method)
                {
                    RequestFormat = DataFormat.Json
                };
                client.Timeout = 500000;
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", GenTokenApi());
                if (dataObject != null)
                {
                    request.AddBody(dataObject);
                }
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Có lỗi trong quá trình gửi Request API, Vui lòng kiểm tra lại!");
                }
                Res result = JsonConvert.DeserializeObject<Res>(response.Content, new JsonSerializerSettings());
                if (result.Status == "SUCCESS")
                {
                    if (result.Value == null || string.IsNullOrEmpty(result.Value.ToString()))
                    {
                        return new List<T>();
                    }
                    return JsonConvert.DeserializeObject<List<T>>(result.Value.ToString());
                }
                //MessageBox.Show();
                log.Error("Có lỗi trong quá tình gửi Request: pos/" + endpoint + ", Nội dung: " + result.Value);
                return new List<T>();
            }
            catch (Exception)
            {
                throw new Exception("Có lỗi trong quá trình gửi Request API, Vui lòng kiểm tra lại!");
            }
        }

        public static List<T> CallApiBulkData<T>(string endpoint, Method method, object dataObject = null, string localApi = "")
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true;
                RestClient client = new RestClient
                {
                    BaseUrl = string.IsNullOrEmpty(localApi) ? ApiUrl : localApi
                };
                RestRequest request = new RestRequest("pos/" + endpoint, method)
                {
                    RequestFormat = DataFormat.Json
                };
                client.Timeout = 500000;
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", GenTokenApi());
                if (dataObject != null)
                {
                    request.AddBody(dataObject);
                }
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Có lỗi trong quá trình gửi Request API, Vui lòng kiểm tra lại!");
                }
                JsonSerializerSettings options = new JsonSerializerSettings
                {
                    MaxDepth = 3,
                    Formatting = Formatting.None
                };
                object temp = JsonConvert.DeserializeObject(response.Content, options);
                Res result = JsonConvert.DeserializeObject<Res>(response.Content, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                });
                if (result.Status == "SUCCESS")
                {
                    JObject s = JObject.Parse(response.Content);
                }
                else
                {
                    log.Error("Có lỗi trong quá tình gửi Request: pos/" + endpoint + ", Nội dung: " + result.Value);
                }
                return new List<T>();
            }
            catch (Exception)
            {
                throw new Exception("Có lỗi trong quá trình gửi Request API, Vui lòng kiểm tra lại!");
            }
        }

        public static T CallApiSimple<T>(string endpoint, Method method, object dataObject = null, string localApi = "")
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true;
                RestClient client = new RestClient
                {
                    BaseUrl = string.IsNullOrEmpty(localApi) ? ApiUrl : localApi
                };
                RestRequest request = new RestRequest("pos/" + endpoint, method)
                {
                    RequestFormat = DataFormat.Json
                };
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", GenTokenApi());
                client.Timeout = 500000;
                if (dataObject != null)
                {
                    request.AddBody(dataObject);
                }
                string param = JsonConvert.SerializeObject(dataObject);

                LSPos_API.Utils.Log.Info(client.BaseUrl + "/" + endpoint + " start");
                LSPos_API.Utils.Log.Info(param);
                IRestResponse response = client.Execute(request);
                LSPos_API.Utils.Log.Info(client.BaseUrl + "/" + endpoint + " end");
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    LSPos_API.Utils.Log.Info(response.Content);
                    throw new Exception();
                }
                Res result = JsonConvert.DeserializeObject<Res>(response.Content);
                if (result.Status == "SUCCESS")
                {
                    if (result.Value == null || string.IsNullOrEmpty(result.Value.ToString()))
                    {
                        return default(T);
                    }
                    return JsonConvert.DeserializeObject<T>(result.Value.ToString());
                }
                log.Error("Có lỗi trong quá tình gửi Request: pos/" + endpoint + ", Nội dung: " + result.Status + ", " + result.Value);
                return default(T);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình gửi Request API, Vui lòng kiểm tra lại. Message: " + ex.Message);
            }
        }

        public static Res CallApiSimple(string endpoint, Method method, object dataObject = null, string localApi = "")
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true;
                RestClient client = new RestClient
                {
                    BaseUrl = string.IsNullOrEmpty(localApi) ? ApiUrl : localApi
                };
                client.Timeout = 500000;
                RestRequest request = new RestRequest("pos/" + endpoint, method)
                {
                    RequestFormat = DataFormat.Json
                };
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", GenTokenApi());
                if (dataObject != null)
                {
                    request.AddBody(dataObject);
                }
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Có lỗi trong quá trình gửi Request API, Vui lòng kiểm tra lại!");
                }
                return JsonConvert.DeserializeObject<Res>(response.Content, new JsonSerializerSettings());
            }
            catch (Exception)
            {
                throw new Exception("Có lỗi trong quá trình gửi Request API, Vui lòng kiểm tra lại!");
            }
        }

        private static Res Res()
        {
            throw new NotImplementedException();
        }

        public static T CallApiLogin<T>(object dataObject = null, string localApi = "")
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true;
                RestClient client = new RestClient
                {
                    BaseUrl = string.IsNullOrEmpty(localApi) ? ApiUrl : localApi
                };
                RestRequest request = new RestRequest("pos/session/login", Method.POST)
                {
                    RequestFormat = DataFormat.Json
                };
                client.Timeout = 500000;
                request.AddHeader("Accept", "application/json");
                if (dataObject != null)
                {
                    request.AddBody(dataObject);
                }
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Có lỗi trong quá trình gửi Request API, Vui lòng kiểm tra lại!");
                }
                Res result = JsonConvert.DeserializeObject<Res>(response.Content, new JsonSerializerSettings());
                if (result.Status == "SUCCESS")
                {
                    if (result.Value == null || string.IsNullOrEmpty(result.Value.ToString()))
                    {
                        return (T)new object();
                    }
                    return JsonConvert.DeserializeObject<T>(result.Value.ToString());
                }
                if (result.Value.ToString() == "NotUsedSiteException" || result.Value.ToString() == "Site not found")
                {
                    log.Error("Người dùng chưa đăng ký Site, Vui lòng kiểm tra lại!");
                }
                else if (result.Value.ToString() == "USER_INVALID" || result.Value.ToString() == "Employee invalid")
                {
                    log.Error("Thông tin đăng nhập chưa đúng, Vui lòng kiểm tra lại!");
                }
                else
                {
                    log.Error("Có lỗi trong quá trình đăng nhập,  Vui lòng kiểm tra lại: " + result.Value.ToString());
                }
                return default(T);
            }
            catch (NotUsedSiteException)
            {
                throw new Exception("Người dùng chưa đăng ký Site này, Vui lòng kiểm tra lại!");
            }
            catch (Exception)
            {
                throw new Exception("Có lỗi trong quá trình đăng nhập, Vui lòng liên hệ IT!!!");
            }
        }

        public static T CallB2BApiSimple<T>(string endpoint, Method method, object dataObject = null, string localApi = "")
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true;
                RestClient client = new RestClient
                {
                    BaseUrl = string.IsNullOrEmpty(localApi) ? ApiUrl : localApi
                };
                RestRequest request = new RestRequest(endpoint, method)
                {
                    RequestFormat = DataFormat.Json
                };
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Authorization", GenTokenApi());
                client.Timeout = 500000;
                if (dataObject != null)
                {
                    request.AddBody(dataObject);
                }
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception();
                }
                Res result = JsonConvert.DeserializeObject<Res>(response.Content);
                if (result.Status == "SUCCESS")
                {
                    if (result.Value == null || string.IsNullOrEmpty(result.Value.ToString()))
                    {
                        return default(T);
                    }
                    return JsonConvert.DeserializeObject<T>(result.Value.ToString());
                }
                log.Error("Có lỗi trong quá tình gửi Request: pos/" + endpoint + ", Nội dung: " + result.Status + ", " + result.Value);
                return default(T);
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình gửi Request API, Vui lòng kiểm tra lại. Message: " + ex.Message);
            }
        }

        public static string CallOnepayQuery(string querydata = null)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true;
                RestClient client = new RestClient
                {
                    BaseUrl = "https://onepay.vn/"
                };
                RestRequest request = new RestRequest("msp/api/v1/vpc/invoices/queries", Method.POST)
                {
                    RequestFormat = DataFormat.Json
                };
                request.AddHeader("Accept", "*/*");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded", querydata, ParameterType.RequestBody);
                client.Timeout = 500000;
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception();
                }

                return response.Content;
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi trong quá trình gửi Request API, Vui lòng kiểm tra lại. Message: " + ex.Message);
            }
        }

        public static string CreateQRVnpay(string data)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true;
                RestClient client = new RestClient
                {
                    //BaseUrl = "http://14.160.87.123:18080"
                    BaseUrl = "https://createqr.vnpay.vn"
                };
                RestRequest request = new RestRequest("/QRCreateAPIRestV2/rest/CreateQrcodeApi/createQrcode", Method.POST)
                {
                    RequestFormat = DataFormat.Json
                };
                request.AddHeader("Accept", "*/*");
                request.AddHeader("Content-Type", "text/plain");
                if (!string.IsNullOrWhiteSpace(data))
                {
                    request.AddParameter("text/plain", data, ParameterType.RequestBody);

                }
                client.Timeout = 500000;
                IRestResponse response = client.Execute(request);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception();
                }

                return response.Content;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw new Exception("Có lỗi trong quá trình gửi Request API, Vui lòng kiểm tra lại. Message: " + ex.Message);
            }
        }

        public static string GenTokenApi()
        {
            string connectID = ((Global.ApiConnect != null) ? Global.ApiConnect.ConnectID : "");
            string connectKey = ((Global.ApiConnect != null) ? Global.ApiConnect.ConnectKey : "");
            string token = "ConnectID={0},Timestamp={1},Signature={2}";
            token = token.Replace("{0}", connectID);
            string timeStamp = ConvertToTimestamp(DateTime.UtcNow).ToString();
            token = token.Replace("{1}", timeStamp);
            return token.Replace("{2}", sha256(connectID + timeStamp + connectKey));
        }

        private static string sha256(string randomString)
        {
            SHA256Managed crypt = new SHA256Managed();
            StringBuilder hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            byte[] array = crypto;
            foreach (byte theByte in array)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        private static long ConvertToTimestamp(DateTime value)
        {
            return (long)(value - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }
    }
}
