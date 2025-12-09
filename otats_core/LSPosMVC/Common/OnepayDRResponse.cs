using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LSPosMVC.Common
{
    public class OnepayDRResponse
    {
        public int vpc_Amount { get; set; }
        public string vpc_CardNum { get; set; }
        public string vpc_Command { get; set; }
        public string vpc_MerchTxnRef { get; set; }
        public string vpc_Merchant { get; set; }
        public string vpc_Message { get; set; }
        public string vpc_OrderInfo { get; set; }
        public string vpc_PayChannel { get; set; }
        public string vpc_TransactionNo { get; set; }
        public string vpc_TxnResponseCode { get; set; }
        public string vpc_Version { get; set; }
        public string vpc_DRExists { get; set; }
        public string vpc_SecureHash { get; set; }

    }
}