namespace Ticket.Facade
{
	public class VersionsFacade : BaseFacade
	{
		protected static VersionsFacade instance = new VersionsFacade(new VersionsModel());

		public static VersionsFacade Instance => instance;

		protected VersionsFacade(VersionsModel model)
			: base(model)
		{
		}

		protected VersionsFacade()
		{
		}
	}
}
