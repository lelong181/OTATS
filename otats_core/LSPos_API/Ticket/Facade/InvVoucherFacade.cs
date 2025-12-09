namespace Ticket.Facade
{
	public class InvVoucherFacade : BaseFacade
	{
		protected static InvVoucherFacade instance = new InvVoucherFacade(new InvVoucherModel());

		public static InvVoucherFacade Instance => instance;

		protected InvVoucherFacade(InvVoucherModel model)
			: base(model)
		{
		}

		protected InvVoucherFacade()
		{
		}
	}
}
