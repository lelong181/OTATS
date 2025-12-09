namespace LSPosMVC.Common.Paycollect
{
    public class OPUser
    {
        public string id { get; set; }
        public string reference { get; set; }
        public string name { get; set; }
    }
    public class OPAccount
    {
        public string id { get; set; }
        public string account_number { get; set; }
        public string bank_name { get; set; }
        public string swift_code { get; set; }
    }
    public class OnePayIPNRequest
    {
        public string bank_txn_ref { get; set; }
        public string create_time { get; set; }
        public string state { get; set; }
        public string amount { get; set; }
        public string remark { get; set; }
        public string currency { get; set; }
        public OPUser user { get; set; }
        public OPAccount account { get; set; }
        public string merchant_id { get; set; }

    }
}
