namespace Model.ResponseModel.BiInterface{

public class BiRes
{
	public const int Success = 1;

	public const int Error = 0;

	public int code { get; set; }

	public string messages { get; set; }

	public object data { get; set; }

	public BiRes()
	{
	}

	public BiRes(int _code, string _messages, object _data = null)
	{
		code = _code;
		messages = _messages;
		data = _data;
	}
}
}