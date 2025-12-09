namespace Ticket.Facade
{
	public class SystemInfoFacade : BaseFacade
	{
		protected static SystemInfoFacade instance = new SystemInfoFacade(new SystemInfoModel());

		public static SystemInfoFacade Instance => instance;

		protected SystemInfoFacade(SystemInfoModel model)
			: base(model)
		{
		}

		protected SystemInfoFacade()
		{
		}
	}
}
