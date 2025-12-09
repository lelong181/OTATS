namespace Ticket
{
	public class CloseShiftReportModel
	{
		public int STT { get; set; }

		public string ServiceName { get; set; }

		public int Price { get; set; }

		public int Quantity { get; set; }

		public int DiscountPercent { get; set; }

		public int DiscountAmount { get; set; }

		public int TotalAmount { get; set; }

		public string BookingName { get; set; }

		public string Temp { get; set; }

		public int RecoupAmount { get; set; }

		public int SellQuantity { get; set; }

		public int RefundQuantity { get; set; }

		public int SellAmount { get; set; }

		public int RefundAmount { get; set; }
	}
}
