using System;

namespace Model.ResponseModel.HotelInterface
{

	public class GuestInHouseCheckRes
{
	public string Resv_PropertyID { get; set; }

	public string AccountCode { get; set; }

	public DateTime IssuedDate { get; set; }

	public DateTime ExpirationDate { get; set; }

	public string Status { get; set; }

	public string Resv_ID { get; set; }

	public string Resv_Confirmation { get; set; }

	public string Resv_ProfileID { get; set; }

	public string Resv_GuestName { get; set; }

	public int Rsv_NumberOfUse { get; set; }

	public string Resv_RoomNo { get; set; }
}
}