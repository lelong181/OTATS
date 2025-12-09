using System;

namespace Model.ResponseModel{

public class TCPLog
{
	public long ID { get; set; }

	public DateTime Device_Time { get; set; }

	public string Device_IP { get; set; }

	public string Device_ID { get; set; }

	public string Device_CommandContent { get; set; }

	public string Device_CommandID { get; set; }

	public string Device_CardID { get; set; }

	public DateTime Server_Time { get; set; }

	public string Server_CommandResultContent { get; set; }

	public string Server_CommandResultID { get; set; }

	public string Server_CommandResultMessage { get; set; }

	public DateTime Server_EndTime { get; set; }
}
}