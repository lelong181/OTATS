using System.Collections.Generic;
using Business.Model;
using BusinessLayer.Model.API;
using RestSharp;
using Ticket.Utils;

namespace BusinessLayer.Repository
{
	public class SessionRepository
	{
		public Res CheckSession(string username, string computerId)
		{
			return ApiUtility.CallApiSimple("session/check-status", Method.POST, new
			{
				Username = username,
				ComputerId = computerId
			});
		}

		public SessionResponse GetDetail(string sessionId)
		{
			return ApiUtility.CallApiSimple<SessionResponse>("session/details", Method.POST, new
			{
				Value = sessionId
			});
		}

		public SessionResponse GetSessionMasterOpen()
		{
			return ApiUtility.CallApiSimple<SessionResponse>("session/get-session-master-open", Method.POST);
		}

		public bool CloseSession(string sessionId)
		{
			Res data = ApiUtility.CallApiSimple("session/close-session", Method.POST, new
			{
				Value = sessionId
			});
			if (data.Status == "SUCCESS")
			{
				return true;
			}
			return false;
		}

		public bool CloseAllSession()
		{
			Res data = ApiUtility.CallApiSimple("session/close-session-master", Method.POST);
			if (data.Status == "SUCCESS")
			{
				return true;
			}
			return false;
		}

		public Res OpenSession(string username, string computerId, string localApi = "")
		{
			return ApiUtility.CallApiSimple("session/open-session", Method.POST, new
			{
				Username = username,
				ComputerId = computerId
			}, localApi);
		}

		public List<SessionRes> GetData(SessionReq model)
		{
			return ApiUtility.CallApi<SessionRes>("session/get-data", Method.POST, model);
		}

		public Res AdminCloseSession(AdminSessionCloseReq model)
		{
			return ApiUtility.CallApiSimple("session/admin-close-session", Method.POST, model);
		}
	}
}
