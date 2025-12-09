using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.API
{
	public class PaymentMethodResponse
	{
		public Guid ID { get; set; }

		public List<CommonI18n> I18N { get; set; }
	}
}
