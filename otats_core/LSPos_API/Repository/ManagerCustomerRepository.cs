using System.Collections.Generic;
using Business.Model;
using BusinessLayer.Model;
using RestSharp;
using Ticket.Utils;

namespace BusinessLayer.Repository
{
	public class ManagerCustomerRepository
	{
		public List<CustomerModel> GetData(string keyword = "", int CurrentPage = 1, int PageSize = 150)
		{
			return ApiUtility.CallApi<CustomerModel>("customer/get-all", Method.POST, new
			{
				keyword = keyword,
				currentPage = CurrentPage,
				pageSize = PageSize
			});
		}

		public Res AddNewCustomer(CustomerModel model)
		{
			return ApiUtility.CallApiSimple("customer/create", Method.POST, model);
		}

		public Res UpdateCustomer(CustomerModel model)
		{
			return ApiUtility.CallApiSimple("customer/update", Method.POST, model);
		}
	}
}
