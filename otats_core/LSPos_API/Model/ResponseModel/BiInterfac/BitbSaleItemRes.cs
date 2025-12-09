using System;

namespace Model.ResponseModel.BiInterface{

public class BitbSaleItemRes
{
	public Guid SaleItemId { get; set; }

	public Guid SaleId { get; set; }

	public Guid ProductId { get; set; }

	public Guid? PerformanceTypeId { get; set; }

	public Guid? PerformanceSetId { get; set; }

	public decimal UnitRawPrice { get; set; }

	public int TaxCalcType { get; set; }

	public decimal UnitAmount { get; set; }

	public decimal UnitTax { get; set; }

	public int Quantity { get; set; }

	public decimal TotalAmount { get; set; }

	public decimal TotalTax { get; set; }

	public Guid? PerformanceId { get; set; }

	public bool? MultiPerformance { get; set; }

	public Guid? OptionSetId { get; set; }

	public int? QuantityUnit { get; set; }

	public Guid? MainSaleItemId { get; set; }

	public decimal? StatAmount { get; set; }

	public bool? DistinctItem { get; set; }
}
}