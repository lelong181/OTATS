namespace Model.ResponseModel{

public class ResProduct
{
	public decimal Id { get; set; }

	public string ItemName { get; set; }

	public string ItemNameEn { get; set; }

	public string UnitName { get; set; }

	public string UnitNameEn { get; set; }

	public decimal Price { get; set; }

	public decimal DiscountQty { get; set; }

	public decimal DiscountRate { get; set; }

	public decimal Qty { get; set; }

	public string Note { get; set; }
}
}