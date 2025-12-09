using System.Collections.Generic;

namespace Model.ResponseModel{

public class RptARListDetailModel
{
	public IEnumerable<RptARListDetail> list { get; set; }

	public IEnumerable<RptARListDetailAllocated> details { get; set; }
}
}