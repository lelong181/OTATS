namespace Ticket
{
	public class InvoiceDetail : BaseModel
	{
		public string ServiceID { get; set; }

		public string Price { get; set; }

		public string ServiceTypeID { get; set; }

		public string ServiceTypeName { get; set; }

		public string AccountID { get; set; }

		public string CardID { get; set; }

		public string CardNo { get; set; }

		public string Quantity { get; set; }

		public string SaleDate { get; set; }

		public string CurrentAccAmount { get; set; }

		public string Status { get; set; }

		public string DiscountPercent { get; set; }

		public string DiscountAmount { get; set; }

		public string TotalMoney { get; set; }

		public string ServiceName { get; set; }

		public string Description { get; set; }

		public string UsingDate { get; set; }

		public int PrintCount { get; set; }

		public string InvoiceCode { get; set; }
	}
}
