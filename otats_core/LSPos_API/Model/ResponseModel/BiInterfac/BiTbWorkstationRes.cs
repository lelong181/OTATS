using System;

namespace Model.ResponseModel.BiInterface{

public class BiTbWorkstationRes
{
	public Guid WorkstationId { get; set; }

	public Guid CategoryId { get; set; }

	public Guid LocationAccountId { get; set; }

	public Guid OpAreaAccountId { get; set; }

	public int WorkstationType { get; set; }

	public string WorkstationCode { get; set; }

	public string WorkstationName { get; set; }

	public int StationSerial { get; set; }

	public Guid? SaleChannelId { get; set; }

	public Guid? LastUserAccountId { get; set; }

	public DateTime? LastClientActivity { get; set; }

	public string LastClientVersion { get; set; }

	public string LastClientIPAddress { get; set; }

	public Guid? LoggedUserAccountId { get; set; }

	public string WorkstationURI { get; set; }
}
}