namespace Ticket
{
	public class InvVoucherDetailModel : BaseModel
	{
		public int ID { get; set; }

		public int VoucherID { get; set; }

		public int CardTypeID { get; set; }

		public int Quantity { get; set; }

		public int Status { get; set; }
	}
}
