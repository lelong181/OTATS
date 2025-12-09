using System;

namespace Ticket
{
	public class ViewException : Exception
	{
		public ViewException(string message)
			: base(message)
		{
		}

		public ViewException(Exception e)
		{
		}
	}
}
