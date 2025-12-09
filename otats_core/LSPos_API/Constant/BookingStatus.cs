namespace BusinessLayer.Constant
{
	public class BookingStatus
	{
		public const string New = "NEW";

		public const string AwaiPayment = "AWAIT_PAYMENT";

		public const string AwaiFullyPayment = "AWAIT_FULLY_PAYMENT";

		public const string FullPayment = "FULL_PAYMENT";

		public const string Confirm = "CONFIRM";

		public const string Void = "VOID";

		public const string Cancel = "CANCEL";

		public const string Inservice = "IN_SERVICE";

		public const string Close = "CLOSES";

		public const string Finished = "FINISHED";

		public const string PaymentFailed = "PAYMENT_FAILED";

		public const string NewValue = "Mới";

		public const string AwaiPaymentValue = "Chờ thanh toán";

		public const string AwaiFullyPaymentValue = "Đặt cọc";

		public const string ConfirmValue = "Được xác nhận";

		public const string VoidValue = "Bị hủy bởi khu vui chơi";

		public const string InserviceValue = "Trong thời gian sử dụng";

		public const string CloseValue = "Đã hết hạn sử dụng";

		public const string PaymentFailedValue = "Thanh toán thất bại";

		public const string FullPaymentValue = "Đã thanh toán hết";
	}
}
