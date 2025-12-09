namespace BusinessLayer.Model.EzInv
{
	public class EzInvoiceExport : EzInvoiceReq
	{
		public string SignStatus { get; set; }

		public string strInvoiceStatus { get; set; }

		public string strEzInvoice_InvoiceID { get; set; }

		public int STT { get; set; }
	}
}
