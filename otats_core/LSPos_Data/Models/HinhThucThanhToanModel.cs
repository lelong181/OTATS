using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Models
{
    public class HinhThucThanhToanModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string VirtualPaymentClientURL { get; set; }
        public string Version { get; set; }
        public string MerchantCode { get; set; }
        public string MerchantName { get; set; }
        public string ServiceCode { get; set; }
        public string CountryCode { get; set; }
        public string PayType { get; set; }
        public string Ccy { get; set; }
        public string MasterMerCode { get; set; }
        public string MerchantType { get; set; }
        public string TerminalId { get; set; }
        public string TerminalName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string AccessCode { get; set; }
        public string Hascode { get; set; }
        public string Currency { get; set; }
        public string ReturnURL { get; set; }

    }
}
