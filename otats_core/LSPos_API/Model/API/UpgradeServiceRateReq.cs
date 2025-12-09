using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class UpgradeServiceRateReq
	{
		public DateTime CheckInDate { get; set; }

		public Guid OriginInvoiceID { get; set; }

		public string OriginInvoiceCode { get; set; }

		public Guid OriginBookingID { get; set; }

		public string SessionID { get; set; }

		public string Username { get; set; }

		public string ComputerID { get; set; }

		public string ZoneID { get; set; }

		public string SiteID { get; set; }

		public string SiteCode { get; set; }

		public string Note { get; set; }

		public Guid ProfileID { get; set; }

		public List<PaymentTypeReq> PaymentTypes { get; set; }

		public List<PaymentTypeReq> PaymentTypesRepay { get; set; }

		public List<UgradeServiceRateDetailReq> UpgradeDetails { get; set; }

		public bool IsExportTicket { get; set; } = false;


		public List<SellCardInput> listSellCard { get; set; }

		public UpgradeServiceRateReq()
		{
			PaymentTypes = new List<PaymentTypeReq>();
			PaymentTypesRepay = new List<PaymentTypeReq>();
			UpgradeDetails = new List<UgradeServiceRateDetailReq>();
			listSellCard = new List<SellCardInput>();
		}
	}
}
