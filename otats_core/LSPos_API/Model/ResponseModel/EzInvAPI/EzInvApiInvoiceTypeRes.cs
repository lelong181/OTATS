using System.Collections.Generic;

namespace Model.ResponseModel.EzInvAPI{

public class EzInvApiInvoiceTypeRes
{
	public string Status { get; set; }

	public string Message { get; set; }

	public List<EzInvApiInvoiceType> Data { get; set; }
}
}