using System.Collections.Generic;

namespace Ticket
{
	public class ReturnServiceModel : BaseModel
	{
		public int ServiceID { get; set; }

		public string ServiceName { get; set; }

		public string Description { get; set; }

		public string ServiceTypeName { get; set; }

		public int ChannelID { get; set; }

		public string InvoiceID { get; set; }

		public string ServiceType { get; set; }

		public int TotalMoney { get; set; }

		public int Quantity { get; set; }

		public int VATPercent { get; set; }

		public int Price { get; set; }

		public string Status { get; set; }

		public bool IsCard { get; set; }

		public bool IsBarcode { get; set; }

		public int ZoneID { get; set; }

		public string IsSold { get; set; }

		public bool HasDiscount { get; set; }

		public string Discount { get; set; }

		public List<AccountModel> Accounts { get; set; }

		public ReturnServiceModel()
		{
			Accounts = new List<AccountModel>();
		}
	}
}
