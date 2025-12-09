namespace Ticket.Facade
{
	public class PropertyFacade : BaseFacade
	{
		protected static PropertyFacade instance = new PropertyFacade(new PropertyModel());

		public static PropertyFacade Instance => instance;

		protected PropertyFacade(PropertyModel model)
			: base(model)
		{
		}

		protected PropertyFacade()
		{
		}
	}
}
