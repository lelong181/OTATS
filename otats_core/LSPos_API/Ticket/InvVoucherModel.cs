using System;
using Ticket.Business;

namespace Ticket
{
	public class InvVoucherModel : BaseModel
	{
		public int ID { get; set; }

		public string VoucherNo { get; set; }

		public DateTime VoucherDate { get; set; }

		public int Type { get; set; }

		public string Description { get; set; }

		public int ToStoreID { get; set; }

		public int FromStoreID { get; set; }

		public string Receiver { get; set; }

		public int UserReceiveID { get; set; }

		public int Status { get; set; }

		public int ReasonID { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedDate { get; set; }

		public string UpdatedBy { get; set; }

		public DateTime UpdatedDate { get; set; }

		public int FromSessionID { get; set; }

		public int ToSessionID { get; set; }

		public int FromComputerID { get; set; }

		public int ToComputerID { get; set; }

		public bool IsTransferConfirmed { get; set; }

		public string TransferConfirmedBy { get; set; }

		public bool IsDeleted(ref string deletedBy)
		{
			if (ID == 0)
			{
				return false;
			}
			InvVoucherModel vM = (InvVoucherModel)InvVoucherBO.Instance.FindByPK(ID);
			if (vM.Status == 0)
			{
				deletedBy = vM.UpdatedBy;
				return true;
			}
			return false;
		}

		public bool IsUpdated(ref string updatedBy)
		{
			if (ID == 0)
			{
				return false;
			}
			InvVoucherModel vM = (InvVoucherModel)InvVoucherBO.Instance.FindByPK(ID);
			if (vM.UpdatedDate != UpdatedDate)
			{
				updatedBy = vM.UpdatedBy;
				return true;
			}
			return false;
		}
	}
}
