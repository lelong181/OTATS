namespace BusinessLayer.Model.API
{
	public class PaymentTypeReq
	{
		public string PaymentID { get; set; }

		public string Name { get; set; }

		public decimal Amount { get; set; }
	}
}
