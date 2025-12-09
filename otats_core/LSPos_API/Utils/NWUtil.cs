using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Security.Cryptography;
using BusinessLayer.Model.Sell;

namespace LSPosMVC.App_Start
{
    public class NWUtil
    {
        public string CallAPI(string endpoint, Method method, string token, object dataObject = null)
        {
            RestClient client = new RestClient
            {
                //BaseUrl = "https://upms.hotelaas.com" //Test
                BaseUrl = "http://trangangroup.com.vn:8711"
            };
            RestRequest request = new RestRequest("ticketcentermarchantapi/" + endpoint, method)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddHeader("Accept", "application/json");
            //request.AddHeader("secretkey", "9a8fa234b2520newway9d8178545a62");//Test
            request.AddHeader("secretkey", "7976399a0c504c20acc692892fc45e16");//Test
            request.AddHeader("accesstoken", token);
            client.Timeout = 500000;
            if (dataObject != null)
            {
                request.AddBody(dataObject);
            }
            string param = JsonConvert.SerializeObject(dataObject);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                LSPos_API.Utils.Log.Info(response.Content);
            }
            return response.Content;
        }

        public Metadata_addticket addTicket(string token, NWRequestBooking requestbody)
        {
            try
            {
                LSPos_API.Utils.Log.Info(JsonConvert.SerializeObject(requestbody));
                string res = CallAPI("addticket", Method.POST, token, requestbody);
                LSPos_API.Utils.Log.Info(res);
                NWResponse_addticket result = JsonConvert.DeserializeObject<NWResponse_addticket>(res);
                if (result.statuscode == "OK")
                {
                    return result.metadata;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LSPos_API.Utils.Log.Error(ex);
                return null;
            }
        }

        public Profile login(string token, LoginModel requestbody)
        {
            try
            {
                LSPos_API.Utils.Log.Info(JsonConvert.SerializeObject(requestbody));
                string res = CallAPI("login", Method.POST, token, requestbody);
                LSPos_API.Utils.Log.Info(res);
                NWResponse_login result = JsonConvert.DeserializeObject<NWResponse_login>(res);
                if (result.statuscode == "OK")
                {
                    return result.metadata;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LSPos_API.Utils.Log.Error(ex);
                return null;
            }
        }
    }

    public class LoginModel
    {
        public string username { get; set; } = "merchant";
        public string password { get; set; } = "Newway@123";
    }

    public class Profile
    {
        public string id { get; set; }
        public string full_name { get; set; }
        public string birth_day { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string accesstoken { get; set; }
    }

    public class NWResponse_login
    {
        public string statuscode { get; set; }
        public string message { get; set; }
        public Profile metadata { get; set; }
    }

    public class Ticket
    {
        public string qrcode { get; set; }
        public string ticket_number { get; set; }
    }
    public class TicketAccount
    {
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string ticket_code { get; set; }
        public string name { get; set; }
        public Dictionary<int, Ticket> listqrcode { get; set; }
    }
    public class Metadata_addticket
    {
        public Dictionary<int, TicketAccount> listticket { get; set; }
        public string invoicecode { get; set; }
    }
    public class NWResponse_addticket
    {
        public string statuscode { get; set; }
        public string message { get; set; }
        public Metadata_addticket metadata { get; set; }
    }


    public class Service
    {
        public string ticket_code { get; set; }
        public int quantity { get; set; }
        public int num_people { get; set; }
        public float price { get; set; }
    }
    public class People
    {
        public string name { get; set; }
        public string phone_number { get; set; }
        public string license_plates { get; set; }
    }

    public class NWRequestBooking
    {
        public List<Service> listticket { get; set; }
        public string invoice_code { get; set; }
        public string create_date { get; set; }
        public string in_date { get; set; }
        public string booker { get; set; }
        public string phone_booker { get; set; }
        public string email_booker { get; set; }
        public string site_code { get; set; }
        public People leader { get; set; }
        public People guider { get; set; }
        public People driver { get; set; }
    }
}
