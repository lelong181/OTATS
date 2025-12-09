namespace Ticket.Facade
{
	public class LicenseFacade : BaseFacade
	{
		protected static LicenseFacade instance = new LicenseFacade(new LicenseModel());

		public static LicenseFacade Instance => instance;

		protected LicenseFacade(LicenseModel model)
			: base(model)
		{
		}

		protected LicenseFacade()
		{
		}
	}
}
