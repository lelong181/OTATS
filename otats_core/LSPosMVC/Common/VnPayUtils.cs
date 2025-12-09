using LSPos_Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace LSPosMVC.Common
{
    public class VnPayUtils
    {
        [JsonObject(IsReference = false)]
        public class VnPayIPNResponse
        {
            public string code { get; set; }
            public string message { get; set; }
            public string data { get; set; }
        }
        public class VnPayIPNResquestAddData
        {
            public string merchantType { get; set; }
            public string serviceCode { get; set; }
            public string masterMerCode { get; set; }
            public string merchantCode { get; set; }
            public string terminalId { get; set; }
            public string productId { get; set; }
            public string amount { get; set; }
            public string ccy { get; set; }
            public string qty { get; set; }
            public string note { get; set; }
        }

        public class VnPayOrder
        {
            public string appId { get; set; }
            public string merchantName { get; set; }
            public string serviceCode { get; set; }
            public string countryCode { get; set; }
            public string masterMerCode { get; set; }
            public string merchantType { get; set; }
            public string merchantCode { get; set; }
            public string payloadFormat { get; set; }
            public string terminalId { get; set; }
            public string terminalName { get; set; }
            public string payType { get; set; }
            public string productId { get; set; }
            public string txnId { get; set; }
            public string amount { get; set; }
            public string tipAndFee { get; set; }
            public string ccy { get; set; }
            public string expDate { get; set; }
            public string billNumber { get; set; }
            public string checksum { get; set; }
        }
        public class VnPayIPNRequest
        {
            public string code { get; set; }
            public string message { get; set; }
            public string msgType { get; set; }
            public string txnId { get; set; }
            public string qrTrace { get; set; }
            public string bankCode { get; set; }
            public string mobile { get; set; }
            public string accountNo { get; set; }
            public string amount { get; set; }
            public string payDate { get; set; }
            public string merchantCode { get; set; }
            public string terminalId { get; set; }
            public string name { get; set; }
            public string phone { get; set; }
            public string province_id { get; set; }
            public string district_id { get; set; }
            public string address { get; set; }
            public string email { get; set; }
            public VnPayIPNResquestAddData addData { get; set; }
            public string checksum { get; set; }

            public string CreateChecksum(string secretKey)
            {
                return code + "|" + msgType + "|" +
                txnId + "|" + qrTrace + "|" +
                bankCode + "|" + mobile + "|" +
                accountNo + "|" + amount + "|"
                + payDate + "|" +
                merchantCode + "|" +
                secretKey;
            }
        }

        public string Md5(string sInput)
        {
            HashAlgorithm algorithmType = null;
            ASCIIEncoding enCoder = new ASCIIEncoding();
            byte[] valueByteArr = enCoder.GetBytes(sInput);
            byte[] hashArray = null;
            algorithmType = new MD5CryptoServiceProvider();
            hashArray = algorithmType.ComputeHash(valueByteArr);
            StringBuilder sb = new StringBuilder();
            byte[] array = hashArray;
            foreach (byte b in array)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        private string Sha256(string data)
        {
            SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));
            StringBuilder builder = new StringBuilder();
            byte[] array = bytes;
            foreach (byte t in array)
            {
                builder.Append(t.ToString("x2"));
            }
            return builder.ToString();
        }

        public string CreateChecksum(DonHangModels donhang)
        {
            string appId = "";
            string merchantName = "";
            string serviceCode = "";
            string countryCode = "";
            string masterMerCode = "";
            string merchantType = "";
            string merchantCode = "";
            string terminalId = "";
            string payType = "";
            string productId = "";
            string txnId = donhang.iddonhang.ToString();
            string amount = donhang.tongtien.ToString();
            string tipAndFee = "";
            string ccy = "";
            string expDate = DateTime.Now.ToString("yyMMddHHmm");
            string secretKey = "";

            appId = ConfigurationManager.AppSettings["vnp_appId"];
            merchantName = ConfigurationManager.AppSettings["vnp_merchantName"];
            serviceCode = ConfigurationManager.AppSettings["vnp_serviceCode"];
            countryCode = ConfigurationManager.AppSettings["vnp_countryCode"];
            masterMerCode = ConfigurationManager.AppSettings["vnp_masterMerCode"];
            merchantType = ConfigurationManager.AppSettings["vnp_merchantType"];
            merchantCode = ConfigurationManager.AppSettings["vnp_merchantCode"];
            terminalId = ConfigurationManager.AppSettings["vnp_terminalId"];
            payType = ConfigurationManager.AppSettings["vnp_payType"];
            ccy = ConfigurationManager.AppSettings["vnp_ccy"];
            secretKey = ConfigurationManager.AppSettings["vnp_secretKey"];

            string d = appId + "|" + merchantName + "|" + serviceCode + "|" + countryCode + "|" + masterMerCode + "|" +
                merchantType + "|" + merchantCode + "|" + terminalId + "|" + payType + "|" + productId + "|" + txnId + "|" + amount + "|" +
                tipAndFee + "|" + ccy + "|" + expDate + "|" + secretKey;

            string checksum = new VnPayUtils().Md5(d);

            return checksum;
        }

        public string CreateChecksum_KheCoc(DonHangModels donhang)
        {
            string appId = "";
            string merchantName = "";
            string serviceCode = "";
            string countryCode = "";
            string masterMerCode = "";
            string merchantType = "";
            string merchantCode = "";
            string terminalId = "";
            string payType = "";
            string productId = "";
            string txnId = donhang.iddonhang.ToString();
            string amount = donhang.tongtien.ToString();
            string tipAndFee = "";
            string ccy = "";
            string expDate = DateTime.Now.ToString("yyMMddHHmm");
            string secretKey = "";

            appId = ConfigurationManager.AppSettings["vnp_appId_KC"];
            merchantName = ConfigurationManager.AppSettings["vnp_merchantName_KC"];
            serviceCode = ConfigurationManager.AppSettings["vnp_serviceCode"];
            countryCode = ConfigurationManager.AppSettings["vnp_countryCode"];
            masterMerCode = ConfigurationManager.AppSettings["vnp_masterMerCode"];
            merchantType = ConfigurationManager.AppSettings["vnp_merchantType_KC"];
            merchantCode = ConfigurationManager.AppSettings["vnp_merchantCode_KC"];
            terminalId = ConfigurationManager.AppSettings["vnp_terminalId_KC"];
            payType = ConfigurationManager.AppSettings["vnp_payType"];
            ccy = ConfigurationManager.AppSettings["vnp_ccy"];
            secretKey = ConfigurationManager.AppSettings["vnp_secretKey_KC"];

            string d = appId + "|" + merchantName + "|" + serviceCode + "|" + countryCode + "|" + masterMerCode + "|" +
                merchantType + "|" + merchantCode + "|" + terminalId + "|" + payType + "|" + productId + "|" + txnId + "|" + amount + "|" +
                tipAndFee + "|" + ccy + "|" + expDate + "|" + secretKey;

            string checksum = new VnPayUtils().Md5(d);

            return checksum;
        }

        public VnPayOrder CreateOrder(DonHangModels donhang)
        {
            string appId = "";
            string merchantName = "";
            string serviceCode = "";
            string countryCode = "";
            string masterMerCode = "";
            string merchantType = "";
            string merchantCode = "";
            string terminalId = "";
            string terminalName = "";
            string payType = "";
            string productId = "";
            string txnId = donhang.iddonhang.ToString();
            string amount = donhang.tongtien.ToString();
            string tipAndFee = "";
            string ccy = "";
            string expDate = DateTime.Now.ToString("yyMMddHHmm");
            string billNumber = donhang.mathamchieu;
            string secretKey = "";

            appId = ConfigurationManager.AppSettings["vnp_appId"];
            merchantName = ConfigurationManager.AppSettings["vnp_merchantName"];
            serviceCode = ConfigurationManager.AppSettings["vnp_serviceCode"];
            countryCode = ConfigurationManager.AppSettings["vnp_countryCode"];

            masterMerCode = ConfigurationManager.AppSettings["vnp_masterMerCode"];
            merchantType = ConfigurationManager.AppSettings["vnp_merchantType"];
            merchantCode = ConfigurationManager.AppSettings["vnp_merchantCode"];
            terminalId = ConfigurationManager.AppSettings["vnp_terminalId"];
            terminalName = ConfigurationManager.AppSettings["vnp_terminalName"];
            payType = ConfigurationManager.AppSettings["vnp_payType"];
            ccy = ConfigurationManager.AppSettings["vnp_ccy"];

            string checksum = CreateChecksum(donhang);

            VnPayOrder order = new VnPayOrder();
            order.appId = appId;
            order.merchantName = merchantName;
            order.serviceCode = serviceCode;
            order.countryCode = countryCode;
            order.masterMerCode = masterMerCode;
            order.merchantType = merchantType;
            order.merchantCode = merchantCode;
            order.terminalId = terminalId;
            order.terminalName = terminalName;
            order.payType = payType;
            order.productId = productId;
            order.txnId = txnId;
            order.amount = amount;
            order.tipAndFee = tipAndFee;
            order.ccy = ccy;
            order.expDate = expDate;
            order.billNumber = billNumber;
            order.checksum = checksum;

            return order;
        }

        public VnPayOrder CreateOrder_KheCoc(DonHangModels donhang)
        {
            string appId = "";
            string merchantName = "";
            string serviceCode = "";
            string countryCode = "";
            string masterMerCode = "";
            string merchantType = "";
            string merchantCode = "";
            string terminalId = "";
            string terminalName = "";
            string payType = "";
            string productId = "";
            string txnId = donhang.iddonhang.ToString();
            string amount = donhang.tongtien.ToString();
            string tipAndFee = "";
            string ccy = "";
            string expDate = DateTime.Now.ToString("yyMMddHHmm");
            string billNumber = donhang.mathamchieu;
            string secretKey = "";

            appId = ConfigurationManager.AppSettings["vnp_appId_KC"];
            merchantName = ConfigurationManager.AppSettings["vnp_merchantName_KC"];
            serviceCode = ConfigurationManager.AppSettings["vnp_serviceCode"];
            countryCode = ConfigurationManager.AppSettings["vnp_countryCode"];

            masterMerCode = ConfigurationManager.AppSettings["vnp_masterMerCode"];
            merchantType = ConfigurationManager.AppSettings["vnp_merchantType_KC"];
            merchantCode = ConfigurationManager.AppSettings["vnp_merchantCode_KC"];
            terminalId = ConfigurationManager.AppSettings["vnp_terminalId_KC"];
            terminalName = ConfigurationManager.AppSettings["vnp_terminalName_KC"];
            payType = ConfigurationManager.AppSettings["vnp_payType"];
            ccy = ConfigurationManager.AppSettings["vnp_ccy"];

            string checksum = CreateChecksum_KheCoc(donhang);

            VnPayOrder order = new VnPayOrder();
            order.appId = appId;
            order.merchantName = merchantName;
            order.serviceCode = serviceCode;
            order.countryCode = countryCode;
            order.masterMerCode = masterMerCode;
            order.merchantType = merchantType;
            order.merchantCode = merchantCode;
            order.terminalId = terminalId;
            order.terminalName = terminalName;
            order.payType = payType;
            order.productId = productId;
            order.txnId = txnId;
            order.amount = amount;
            order.tipAndFee = tipAndFee;
            order.ccy = ccy;
            order.expDate = expDate;
            order.billNumber = billNumber;
            order.checksum = checksum;

            return order;
        }
    }
}
