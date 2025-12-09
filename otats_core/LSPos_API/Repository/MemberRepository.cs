using System;
using System.Collections.Generic;
using Business.Model;
using BusinessLayer.Model;
using BusinessLayer.Model.API;
using RestSharp;
using Ticket.Utils;

namespace BusinessLayer.Repository
{
	public class MemberRepository
	{
		public List<Member> GetList(GetListMemberReq model)
		{
			return ApiUtility.CallApi<Member>("member/get-list", Method.POST, model);
		}

		public Member GetByID(Guid memberID)
		{
			return ApiUtility.CallApiSimple<Member>("member/get-detail?memberID=" + memberID, Method.GET);
		}

		public Res CreateMember(Member model)
		{
			return ApiUtility.CallApiSimple("member/create", Method.POST, model);
		}

		public Res UpdateMember(Member model)
		{
			return ApiUtility.CallApiSimple("member/edit", Method.POST, model);
		}

		public List<MemberTicketRes> GetListTicket(string memberID)
		{
			return ApiUtility.CallApi<MemberTicketRes>("member/get-ticket?memberID=" + memberID, Method.POST);
		}
	}
}
