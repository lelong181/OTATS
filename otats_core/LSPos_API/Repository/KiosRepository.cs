using System.Collections.Generic;
using BusinessLayer.Model.API;
using RestSharp;
using Ticket;
using Ticket.Utils;

namespace BusinessLayer.Repository
{
	public class KiosRepository
	{
		public List<KiosCheckinResponse> CheckIn(KiosCheckinRequest model)
		{
			model.SiteCode = Global.SiteCode;
			return ApiUtility.CallApi<KiosCheckinResponse>("kios/check-in", Method.POST, model);
		}
	}
}
