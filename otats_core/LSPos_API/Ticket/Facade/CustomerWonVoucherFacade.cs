namespace Ticket.Facade
{
	public class CustomerWonVoucherFacade : BaseFacade
	{
		protected static CustomerWonVoucherFacade instance = new CustomerWonVoucherFacade(new CustomerWonVoucherModel());

		public static CustomerWonVoucherFacade Instance => instance;

		protected CustomerWonVoucherFacade(CustomerWonVoucherModel model)
			: base(model)
		{
		}

		protected CustomerWonVoucherFacade()
		{
		}
	}
}
