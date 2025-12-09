namespace Ticket.Facade
{
	public class ProfileFacade : BaseFacade
	{
		protected static ProfileFacade instance = new ProfileFacade(new ProfileModel());

		public static ProfileFacade Instance => instance;

		protected ProfileFacade(ProfileModel model)
		{
		}

		protected ProfileFacade()
		{
		}
	}
}
