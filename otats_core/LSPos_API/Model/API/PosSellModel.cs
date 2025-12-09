using System.Collections.Generic;
using BusinessLayer.Model.Sell;
using Ticket;

namespace BusinessLayer.Model.API
{
	public class PosSellModel
	{
		public List<ServiceSubGroupModel> SubGroups { get; set; }

		public List<ServiceRateModel> ServiceRates { get; set; }

		public List<PaymentTypeModel> PaymentTypes { get; set; }

		public List<ServiceRateGroup> ServiceRateGroups { get; set; }

		public List<ServiceTicket> ServiceTickets { get; set; }

		public PosSellModel()
		{
			SubGroups = new List<ServiceSubGroupModel>();
			ServiceRates = new List<ServiceRateModel>();
			PaymentTypes = new List<PaymentTypeModel>();
			ServiceRateGroups = new List<ServiceRateGroup>();
			ServiceTickets = new List<ServiceTicket>();
		}
	}
}
