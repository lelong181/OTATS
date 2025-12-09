using System.Collections.Generic;

namespace Model.ResponseModel{

public class InvoiceDetailResponse
{
	public List<InvoicePaymentResponse> ListPaymentType { get; set; }

	public List<InvoiceAccountResponse> ListAccount { get; set; }

	public List<InvoiceServiceRateResponse> ListServiceRate { get; set; }

	public InvoiceDetailResponse()
	{
		ListPaymentType = new List<InvoicePaymentResponse>();
		ListAccount = new List<InvoiceAccountResponse>();
		ListServiceRate = new List<InvoiceServiceRateResponse>();
	}
}
}