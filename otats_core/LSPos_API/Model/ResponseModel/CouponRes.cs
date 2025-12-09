using System;
using System.Collections.Generic;
//using Model.Master;

namespace Model.ResponseModel{

public class CouponRes
{
	public Guid CouponID { get; set; }

	public string Code { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public int? QuantityMax { get; set; }

	public int? QuantityUsed { get; set; }

	public DateTime? BeginDate { get; set; }

	public DateTime? EndDate { get; set; }

	public int? Types { get; set; }

	public decimal? TypeValues { get; set; }

	public int? DiscountTypes { get; set; }

	public decimal? DiscountTypeValues { get; set; }

	public decimal? DiscountTypeValueMax { get; set; }

	public int? Options { get; set; }

	public string DayOfWeek { get; set; }

	public string Status { get; set; }

	public decimal DiscountAmount { get; set; } = default(decimal);


	public int CouponType { get; set; }

	//public List<m_CouponTimes> ListCouponTimes { get; set; }

	public CouponRes()
	{
		//ListCouponTimes = new List<m_CouponTimes>();
	}
}
}