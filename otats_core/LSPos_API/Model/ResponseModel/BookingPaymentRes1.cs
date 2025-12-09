using System;

namespace Model.ResponseModel{

public class BookingPaymentRes1
{
	public Guid BookingID { get; set; }

	public Guid BookingPaymentID { get; set; }

	public Guid ServiceID { get; set; }

	public decimal Amount { get; set; }

	public string Explanation { get; set; }

	public DateTime? CreatedDate { get; set; }

	public string CreatedBy { get; set; }

	public string PaymentName { get; set; }

	public string ResponseCode { get; set; }

	public string BatchNo { get; set; }

	public string TransactionRef { get; set; }

	public DateTime? TransactionDate { get; set; }
}
}