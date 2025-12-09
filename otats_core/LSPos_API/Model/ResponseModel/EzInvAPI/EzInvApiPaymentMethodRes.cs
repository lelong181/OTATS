using System.Collections.Generic;

namespace Model.ResponseModel.EzInvAPI{

public class EzInvApiPaymentMethodRes
{
	public string Status { get; set; }

	public string Message { get; set; }

	public List<EzInvApiPaymentMethod> Data { get; set; }
}
}