using System;

namespace Ticket
{
	public class SendEmailTask
	{
		public Guid Id { get; set; }

		public string ToList { get; set; }

		public string Cclist { get; set; }

		public string Bcclist { get; set; }

		public string EmailSubject { get; set; }

		public string EmailContent { get; set; }

		public string FromEmail { get; set; }

		public string FromName { get; set; }

		public bool? IsHtml { get; set; }

		public int? RetryCount { get; set; }

		public bool? IsSent { get; set; }

		public string SentErrMsg { get; set; }

		public DateTime? SentDate { get; set; }

		public DateTime? CreatedDate { get; set; }

		public DateTime? UpdatedDate { get; set; }

		public string BookingCode { get; set; }
	}
}
