using System;
using System.Collections.Generic;

namespace Model.ResponseModel{

public class SitePaymentResponse
{
	public Guid PaymentGatewayID { get; set; }

	public string PaymentName { get; set; }

	public Guid PaymentID { get; set; }

	public string PaymentCode { get; set; }

	public byte[] Image { get; set; }

	public string ImageType { get; set; }

	public string Type { get; set; }

	public string Description { get; set; }

	public List<PaymentParameter> Parameter { get; set; }

	public bool NeedApprove { get; set; }

	public SitePaymentResponse()
	{
		Parameter = new List<PaymentParameter>();
	}
}
}