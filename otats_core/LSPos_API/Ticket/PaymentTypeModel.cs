using System;
using BusinessLayer.Model.API;
using Newtonsoft.Json;

namespace Ticket
{
	public class PaymentTypeModel
	{
		public Guid? BookingPaymentID { get; set; }

		public string PaymentTypeID { get; set; }

		[JsonProperty(PropertyName = "Title")]
		public string PaymentTypeName { get; set; }

		public long Amount { get; set; }

		public bool IsNewPayment { get; set; }

		public bool IsVoucher { get; set; }

		public int? Seq { get; set; }

		public VoucherRes voucher { get; set; }

		public bool? IsPaymentDeposit { get; set; } = false;


		public string Type { get; set; }

		public PaymentTypeModel()
		{
			Type = "Thanh toán mới";
		}
	}
}
