using System;

namespace Model.ResponseModel{

public class PaymentParameter
{
	public Guid PaymentGatewayID { get; set; }

	public string KeyName { get; set; }

	public string KeyValue { get; set; }

	public string Description { get; set; }
}
}