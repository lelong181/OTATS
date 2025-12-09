namespace Model.ResponseModel.EzInvAPI{

public class EzInvApiAddInvoiceRes
{
	public string Status { get; set; }

	public string Message { get; set; }

	public EzInvApiAddInvoice Data { get; set; }
}
}