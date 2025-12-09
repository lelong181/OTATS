using System;

namespace Model.ResponseModel.BiInterface{

public class BitbTicketUsageRes
{
	public Guid TicketUsageId { get; set; }

	public DateTime UsageDateTime { get; set; }

	public int UsageType { get; set; }

	public int ValidateResult { get; set; }

	public Guid AptWorkstationId { get; set; }

	public Guid TicketId { get; set; }

	public Guid? AccessAreaAccountId { get; set; }

	public Guid? PerformanceId { get; set; }

	public DateTime? ValidateDateTime { get; set; }

	public Guid? IncProductId { get; set; }

	public Guid? MediaId { get; set; }

	public DateTime UsageFiscalDate { get; set; }
}
}