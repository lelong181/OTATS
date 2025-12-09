namespace BusinessLayer.Model.EzInv
{
	public class EzInvoiceList : EzInvoiceTransactionReq
	{
		public string SignStatus { get; set; }

		public string strInvoiceStatus { get; set; }

		public string strEzInvoice_InvoiceID { get; set; }
	}
}
