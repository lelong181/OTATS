namespace Ticket
{
	public class PaymentModel : BaseModel
	{
		public int PaymentTypeID { get; set; }

		public string PaymentTypeName { get; set; }

		public int Money { get; set; }

		public int MoneyShowOnGrid { get; set; }
	}
}
