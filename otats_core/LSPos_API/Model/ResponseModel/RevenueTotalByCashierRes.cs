namespace Model.ResponseModel{

public class RevenueTotalByCashierRes
{
	public string ServiceRateName { get; set; }

	public string ServiceRateGroupName { get; set; }

	public int? Quantity { get; set; }

	public decimal? TotalMoney { get; set; }

	public string Status { get; set; }

	public string ID { get; set; }

	public int? SessionNo { get; set; }

	public decimal Price { get; set; }
}
}