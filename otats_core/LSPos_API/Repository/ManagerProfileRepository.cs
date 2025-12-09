using System.Collections.Generic;
using RestSharp;
using Ticket;
using Ticket.Utils;

namespace BusinessLayer.Repository
{
	public class ManagerProfileRepository
	{
		public List<ProfileModel> GetData(string keyword = "", int CurrentPage = 1, int PageSize = 150, string localApi = "")
		{
			return ApiUtility.CallApi<ProfileModel>("profile/get-data", Method.POST, new
			{
				keyword = keyword,
				currentPage = CurrentPage,
				pageSize = PageSize
			},localApi);
		}
    }
}
