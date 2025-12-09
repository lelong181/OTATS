namespace Ticket
{
	public class InvoicePaymentTypeModel : BaseModel
	{
		public string PaymentID { get; set; }

		public string PaymentName { get; set; }

		public string NewPaymentID { get; set; }

		public string Amount { get; set; }
	}
}
