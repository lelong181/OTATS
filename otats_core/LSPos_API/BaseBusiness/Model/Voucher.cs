namespace BaseBusiness.Model
{
	public class Voucher
	{
		public int VoucherID { get; set; }

		public int VoucherDetailID { get; set; }

		public string VoucherName { get; set; }

		public string VoucherCode { get; set; }

		public string Status { get; set; }

		public int DiscountRate { get; set; }

		public int DiscountAmount { get; set; }

		public string ServiceID { get; set; }
	}
}
