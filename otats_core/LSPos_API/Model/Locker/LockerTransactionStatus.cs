namespace BusinessLayer.Model.Locker
{
	public static class LockerTransactionStatus
	{
		public static string ALL = "ALL";

		public static string AVAI = "AVAILABLE";

		public static string IN_USE = "IN_USE";

		public static string DISABLE = "DISABLE";

		public static string GetLockerStatus(string statusCode)
		{
			if (statusCode.Equals(AVAI))
			{
				return "Trống";
			}
			if (statusCode.Equals(IN_USE))
			{
				return "Đang sử dụng";
			}
			if (statusCode.Equals(DISABLE))
			{
				return "Khóa";
			}
			return "Không khả dụng";
		}
	}
}
