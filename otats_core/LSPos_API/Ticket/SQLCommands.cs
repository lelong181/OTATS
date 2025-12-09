namespace Ticket
{
	public class SQLCommands
	{
		public const string FomatShortDate = "dd/MM/yyyy";

		public const string FormatLongDate = "dd/MM/yyyy hh/mm/ss";

		public const string ConStr = "Server=.;database=CSSERP;uid=sa;pwd=";

		public const string InsertSucessful = "Insert Successfully";

		public const string UpdateSucessful = "Update Successfully";

		public const string DeleteSucessful = "Delete Successfully";

		public const string Choice = "Are you sure delete this row";

		public const string Caption = "CSSOLUTION";

		public const string InsertFail = "Can't insert";

		public const string UpdateFail = "Can't update";

		public const string DeleteFail = "Can't Delete";

		public const string SelectRow = " You must select a row to delete";

		public const string Entity_List = "Select * from [EntityObj]";

		public const string Entity_Delete = "Delete from [EntityObj] where Field1=@Field1";

		public const string Entity_Add = "Delete from [EntityObj] where Field1=@Field1";

		public const string Users_Select = "Select UserID,UserName,Name,GroupName,[Read],Active,Change,FullControl,Address,StartDate,EndDate,Phone,CellPhone,Fax,Email,[Users].GroupID,BirthDay,Password from Users INNER JOIN UserGroup ON Users.GroupID=UserGroup.GroupID";
	}
}
