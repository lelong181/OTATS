using System;

namespace BusinessLayer.Model.API
{
	public class FoundWalletInfoReq
	{
		public string Type { get; set; }

		public string Value { get; set; }

		public DateTime DateUsing { get; set; }
	}
}
