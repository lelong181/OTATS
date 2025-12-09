using System.Collections.Generic;

namespace Model.ResponseModel{

public class BookingAccountStatusRes
{
	public List<BookingAccountStatusDetail> List { get; set; }

	public int TotalPage { get; set; }

	public BookingAccountStatusRes()
	{
		List = new List<BookingAccountStatusDetail>();
	}
}
}