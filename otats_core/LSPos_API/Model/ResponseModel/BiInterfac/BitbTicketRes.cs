using System;

namespace Model.ResponseModel.BiInterface{

public class BitbTicketRes
{
	public Guid TicketId { get; set; }

	public Guid? TicketMediaMatchId { get; set; }

	public Guid SaleItemId { get; set; }

	public Guid ProductId { get; set; }

	public Guid? PerformanceSetId { get; set; }

	public int TicketStatus { get; set; }

	public int? StationSerial { get; set; }

	public string TicketSerial { get; set; }

	public DateTime EncodeDateTime { get; set; }

	public DateTime EncodeFiscalDate { get; set; }

	public DateTime? ValidateDateTime { get; set; }

	public DateTime? FirstUsageDateTime { get; set; }

	public Guid? SeatHoldId { get; set; }

	public int? GroupQuantity { get; set; }

	public int? GroupTicketOption { get; set; }

	public Guid? TransactionId { get; set; }

	public bool Settled { get; set; }

	public DateTime ValidDateFrom { get; set; }

	public DateTime ValidDateTo { get; set; }

	public int? PurgeLevel { get; set; }

	public bool? ManualExpiration { get; set; }

	public bool? BonusEntry { get; set; }

	public decimal? UnitAmount { get; set; }

	public decimal? UnitTax { get; set; }

	public decimal? ClearingLimit { get; set; }

	public decimal? ClearingUsed { get; set; }

	public string ExpiredOnDateTime { get; set; }

	public int? PriorityOrder { get; set; }
}
}