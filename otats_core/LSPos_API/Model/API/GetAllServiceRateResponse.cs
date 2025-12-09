using BusinessLayer.Model.API;
using BusinessLayer.Model.Sell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket;

namespace LSPos_API.Model.API
{
    public class GetAllServiceRateResponse
    {
        public string CurrencyCode { get; set; }

        public List<ServiceRateModel> ServiceRates { get; set; }

        public List<ServiceSubGroupModel> SubGroups { get; set; }

        public List<PaymentTypeModel> Payments { get; set; }

        public List<ServiceRateGroup> ServiceRateGroups { get; set; }

        public GetAllServiceRateResponse()
        {
            ServiceRates = new List<ServiceRateModel>();
            SubGroups = new List<ServiceSubGroupModel>();
            Payments = new List<PaymentTypeModel>();
            ServiceRateGroups = new List<ServiceRateGroup>();
        }
    }
}
