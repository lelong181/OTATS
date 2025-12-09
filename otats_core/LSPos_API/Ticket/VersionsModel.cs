namespace Ticket
{
	public class VersionsModel : BaseModel
	{
		public int ID { get; set; }

		public string VersionNo { get; set; }

		public string ServerPath { get; set; }

		public int FileType { get; set; }

		public int NetType { get; set; }

		public string FtpServerIP { get; set; }

		public string FtpUserName { get; set; }

		public string FtpPassword { get; set; }

		public string FtpDirectory { get; set; }
	}
}
