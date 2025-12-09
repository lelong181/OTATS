using System;
using System.Collections.Generic;

namespace Model.ResponseModel{

public class Data
{
	public decimal Id { get; set; }

	public string Status { get; set; }

	public string FolioNum { get; set; }

	public decimal ResId { get; set; }

	public string ResCode { get; set; }

	public string ResName { get; set; }

	public string ResNameEn { get; set; }

	public string AgentName { get; set; }

	public string AgentBooker { get; set; }

	public decimal Discount { get; set; }

	public decimal TaxRate { get; set; }

	public decimal TotalBeforeTax { get; set; }

	public decimal FeeRate { get; set; }

	public decimal Total { get; set; }

	public decimal Prepaid { get; set; }

	public decimal Deposit { get; set; }

	public decimal PaymentAmount { get; set; }

	public string Note { get; set; }

	public decimal DiscountAll { get; set; }

	public decimal DiscountAllRate { get; set; }

	public string LocationName { get; set; }

	public DateTime? CreateDate { get; set; }

	public string ResServiceCode { get; set; }

	public List<Payment> ListPayments { get; set; }

	public List<ResProduct> ListResProduct { get; set; }
}
}