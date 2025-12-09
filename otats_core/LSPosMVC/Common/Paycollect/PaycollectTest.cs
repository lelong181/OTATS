using System;
using System.Globalization;
using System.Text;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Net.PeerToPeer;
using NPOI.SS.Formula.Functions;
using System.Web;
using System.Configuration;
using Microsoft.SqlServer.Server;

namespace LSPosMVC.Common.Paycollect
{
    public class CreateUserInvoiceResponse
    {
        public UserOP user { get; set; }
        public InvoiceOP invoice { get; set; }
    }
    public class PaycollectTest
    {
        public readonly string yyyyMMdd = "yyyyMMdd";
        public readonly string yyyyMMddTHHmmssZ = "yyyyMMddTHHmmssZ";
        public string partnerHashCode = ConfigurationManager.AppSettings["partnerHashCode_TRANGANPC"];
        public string partner = ConfigurationManager.AppSettings["partner_TRANGANPC"];

        public PaycollectTest(string ID)
        {
            this.partner = ConfigurationManager.AppSettings["partner_" + ID];
            this.partnerHashCode = ConfigurationManager.AppSettings["partnerHashCode_" + ID]; 
        }

        public Authorization CreateSign(string timestamp, string exptime, string uri, byte[] payload)
        {
            //DateTime currentDate = DateTime.ParseExact(timestamp, yyyyMMddTHHmmssZ, CultureInfo.InvariantCulture);
            DateTime currentDate = DateTime.ParseExact(timestamp, yyyyMMddTHHmmssZ, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
            IDictionary<string, string> queryParameter = new Dictionary<string, string>();
            IDictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add(Authorization.X_OP_DATE_HEADER, timestamp);
            headers.Add(Authorization.X_OP_EXPIRES_HEADER, exptime);

            Authorization auth = new Authorization(partner, partnerHashCode, "onepay", "paycollect", "PUT",
            uri, queryParameter, headers, payload, currentDate, int.Parse(exptime));
            return auth;
        }

        public string CallCreateUserAsync(string userref)
        {
            Console.WriteLine("========= START CALL CREATE USER =========");
            //string reference = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            DateTime currentDate = DateTime.UtcNow;
            string isoTime = currentDate.ToString(yyyyMMddTHHmmssZ, CultureInfo.InvariantCulture);
            IDictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add(Authorization.X_OP_DATE_HEADER, isoTime);
            headers.Add(Authorization.X_OP_EXPIRES_HEADER, "3600");

            //JsonObject data = new JsonObject();
            JObject data = new JObject(
                new JProperty("name", "SonTQ"),
                new JProperty("gender", "SonTQ"),
                new JProperty("address", "SonTQ"),
                new JProperty("mobile_number", "SonTQ"),
                new JProperty("email", "SonTQ"),
                new JProperty("id_card", "SonTQ"),
                new JProperty("issue_date", "SonTQ"),
                new JProperty("issue_by", "SonTQ"),
                new JProperty("bank_id", "SonTQ"),
                new JProperty("description", "SonTQ")
                );
            //data.Add("name", "SonTQ");
            //data.Add("gender", "male");
            //data.Add("address", "Trang An, Ninh Binh");
            //data.Add("mobile_number", "0944876541");
            //data.Add("email", "sontq@lscloud.vn");
            //data.Add("id_card", "034097997463");
            //data.Add("issue_date", "05/09/2021 12:00:00 AM");
            //data.Add("issue_by", "CA Ha Noi");
            //data.Add("bank_id", "BIDVVNVX");
            //data.Add("description", "test 1");

            IDictionary<string, string> queryParameter = new Dictionary<string, string>();

            string strContent = data.ToString();
            byte[] payload = Encoding.UTF8.GetBytes(strContent);

            string uri = "/paycollect/api/v1/partners/" + partner + "/users/" + userref;
            Authorization auth = new Authorization(partner, partnerHashCode, "onepay", "paycollect", "PUT",
        uri, queryParameter, headers, payload, currentDate, 3600);

            string url = "https://onepay.vn" + uri;
            string result = new CallHttpRequest().HttpPutRequest(auth.ToString(), url, isoTime, strContent);
            return result;
        }

        public CreateUserInvoiceResponse CallCreateUserInvoice(RequestUserOP user, RequestInvoiceOP inv)
        {
            DateTime currentDate = DateTime.UtcNow;
            string isoTime = currentDate.ToString(yyyyMMddTHHmmssZ, CultureInfo.InvariantCulture);
            IDictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add(Authorization.X_OP_DATE_HEADER, isoTime);
            headers.Add(Authorization.X_OP_EXPIRES_HEADER, "3600");


            BatchOP userdata = new BatchOP();
            userdata.name = "user";
            userdata.do_on = "always";
            userdata.method = "PUT";
            string user_reference = user.reference + "_" + isoTime;
            userdata.href = "/partners/" + partner + "/users/" + user_reference;

            userdata.body = user;

            BatchOP invoicedata = new BatchOP();
            invoicedata.name = "invoice";
            invoicedata.do_on = "$user.response.status == 201";
            invoicedata.method = "PUT";
            invoicedata.href = "/partners/" + partner + "/users/" + user_reference + "/invoices/dh" + DateTime.Now.ToString("yyyyMMddhhmmss");

            invoicedata.body = inv;

            IDictionary<string, string> queryParameter = new Dictionary<string, string>();
            BatchOP[] arr = new BatchOP[] { userdata, invoicedata };
            string strContent = JsonSerializer.Serialize(arr);
            byte[] payload = Encoding.UTF8.GetBytes(strContent);

            string uri = "/paycollect/api/v1/batchs";
            Authorization auth = new Authorization(partner, partnerHashCode, "onepay", "paycollect", "POST",
        uri, queryParameter, headers, payload, currentDate, 3600);

            string url = "https://onepay.vn" + uri;
            string result = new CallHttpRequest().HttpPostRequest(auth.ToString(), url, isoTime, strContent);
            LSPos_Data.Utilities.Log.Info(url);
            LSPos_Data.Utilities.Log.Info(auth.ToString());
            LSPos_Data.Utilities.Log.Info(isoTime);
            LSPos_Data.Utilities.Log.Info(strContent);
            BatchOPResponse[] response = JsonSerializer.Deserialize<BatchOPResponse[]>(result);
            UserOP u = JsonSerializer.Deserialize<UserOP>(response[0].body.ToString());
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"FileUpload\user_qr\" + u.id + ".jpg";
            File.WriteAllBytes(filePath, Convert.FromBase64String(u.accounts.qr.image));
            InvoiceOP i = JsonSerializer.Deserialize<InvoiceOP>(response[1].body.ToString());
            filePath = AppDomain.CurrentDomain.BaseDirectory + @"FileUpload\invoice_qr\" + i.id + ".jpg";
            File.WriteAllBytes(filePath, Convert.FromBase64String(i.qr.image));
            CreateUserInvoiceResponse res = new CreateUserInvoiceResponse();
            res.user = u;
            res.invoice = i;
            return res;
        }





        public void CallGetPartner()
        {
            Console.WriteLine("========= START CALL GET USER =========");
            //string reference = "1700466043731";
            string reference = "USR37F5482A1880A40D";
            DateTime currentDate = DateTime.UtcNow;
            string isoTime = currentDate.ToString(yyyyMMddTHHmmssZ, CultureInfo.InvariantCulture);
            IDictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add(Authorization.X_OP_DATE_HEADER, isoTime);
            headers.Add(Authorization.X_OP_EXPIRES_HEADER, "3600");

            IDictionary<string, string> queryParameter = new Dictionary<string, string>();
            string strContent = "";
            byte[] payload = Encoding.UTF8.GetBytes(strContent);

            string uri = "/paycollect/api/v1/partners/" + partner + "/users/" + reference;
            Authorization auth = new Authorization(partner, partnerHashCode, "onepay", "paycollect", "GET",
        uri, queryParameter, headers, payload, currentDate, 3600);

            string url = "https://onepay.vn" + uri;
            _ = new CallHttpRequest().HttpGetRequest(auth.ToString(), url, isoTime, strContent);
        }

        public void CallGetUser()
        {
            Console.WriteLine("========= START CALL GET USER =========");
            //string reference = "1700466043731";
            string reference = "USR37F5482A1880A40D";
            DateTime currentDate = DateTime.UtcNow;
            string isoTime = currentDate.ToString(yyyyMMddTHHmmssZ, CultureInfo.InvariantCulture);
            IDictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add(Authorization.X_OP_DATE_HEADER, isoTime);
            headers.Add(Authorization.X_OP_EXPIRES_HEADER, "3600");

            IDictionary<string, string> queryParameter = new Dictionary<string, string>();
            string strContent = "";
            byte[] payload = Encoding.UTF8.GetBytes(strContent);

            string uri = "/paycollect/api/v1/users/" + reference;
            Authorization auth = new Authorization(partner, partnerHashCode, "onepay", "paycollect", "GET",
        uri, queryParameter, headers, payload, currentDate, 3600);

            string url = "https://onepay.vn" + uri;
            _ = new CallHttpRequest().HttpGetRequest(auth.ToString(), url, isoTime, strContent);
        }

        public InvoiceOP CallGetInvoice(string user_reference, string invoice_reference)
        {
            DateTime currentDate = DateTime.UtcNow;
            string isoTime = currentDate.ToString(yyyyMMddTHHmmssZ, CultureInfo.InvariantCulture);
            IDictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add(Authorization.X_OP_DATE_HEADER, isoTime);
            headers.Add(Authorization.X_OP_EXPIRES_HEADER, "3600");

            IDictionary<string, string> queryParameter = new Dictionary<string, string>();
            string strContent = "";
            byte[] payload = Encoding.UTF8.GetBytes(strContent);

            string uri = "/paycollect/api/v1/partners/" + partner + "/users/" + user_reference + "/invoices/" + invoice_reference;
            Authorization auth = new Authorization(partner, partnerHashCode, "onepay", "paycollect", "GET",
        uri, queryParameter, headers, payload, currentDate, 3600);

            string url = "https://onepay.vn" + uri;
            string result = new CallHttpRequest().HttpGetRequest(auth.ToString(), url, isoTime, strContent);
            InvoiceOP invoice = JsonSerializer.Deserialize<InvoiceOP>(result);
            return invoice;
        }
    }
}