namespace Ticket.Facade
{
	public class ServiCDPcade : BaseFacade
	{
		protected static ServiCDPcade instance = new ServiCDPcade(new ServiceModel());

		public static ServiCDPcade Instance => instance;

		protected ServiCDPcade(ServiceModel model)
			: base(model)
		{
		}

		protected ServiCDPcade()
		{
		}
	}
}
