using System;

namespace Ticket
{
	public class FacadeException : Exception
	{
		public FacadeException(string message)
			: base(message)
		{
		}

		public FacadeException(Exception e)
		{
		}
	}
}
