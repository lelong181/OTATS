using System;

namespace BusinessLayer.Model.API
{
	public class VoucherRes
	{
		public Guid? VoucherID { get; set; }

		public Guid? VoucherDetailID { get; set; }

		public string Name { get; set; }

		public string VoucherName { get; set; }

		public string VoucherCode { get; set; }

		public decimal? DiscountRate { get; set; }

		public decimal? DiscountAmount { get; set; }

		public string Description { get; set; }

		public DateTime? IssuedDate { get; set; }

		public DateTime? ExpirationDate { get; set; }
	}
}
