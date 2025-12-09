namespace BusinessLayer.Model.API
{
	public class ChangePaymentType
	{
		public string InvoicePaymentTypeID { get; set; }

		public string OldPaymentTypeID { get; set; }

		public string NewPaymentTypeID { get; set; }

		public string UpdatedBy { get; set; }
	}
}
