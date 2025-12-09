using System.Collections.Generic;
using BusinessLayer.Model.API;
using RestSharp;
using Ticket;
using Ticket.Utils;

namespace BusinessLayer.Repository
{
	public class LoginRepository
	{
		public PermissionLoginResponse ProcessLogin(string username, string password, string siteId, string localApi = "")
		{
			return ApiUtility.CallApiLogin<PermissionLoginResponse>(new
			{
				Username = username,
				Password = password,
				SiteId = siteId,
				SiteCode = siteId				
			}, localApi);
		}

		public ComputerModel GetComputer(string name, string localApi = "")
		{
			return ApiUtility.CallApiSimple<ComputerModel>("common/get-computer-pos?computerName=" + name, Method.GET, localApi);
		}

		public List<PermissionRole> GetRolePOS(string username, string localApi = "")
		{
			return ApiUtility.CallApi<PermissionRole>("session/get-roles-pos?username=" + username, Method.GET, localApi);
		}
	}
}
