using System;
using System.Collections.Generic;
using System.Linq;
using Business.Model;
using BusinessLayer.Model;
using BusinessLayer.Model.Locker;
using RestSharp;
using Ticket.Utils;

namespace BusinessLayer.Repository
{
	public class LockerRepository
	{
		public Res AutoLogin()
		{
			return ApiUtility.CallApiSimple("locker/login", Method.GET);
		}

		public List<LockerZone> GetAllZone()
		{
			return ApiUtility.CallApiSimple<List<LockerZone>>("locker/getAllZones", Method.GET);
		}

		public KeyValueObject GetKeyCard()
		{
			return ApiUtility.CallApiSimple<KeyValueObject>("locker/get-keycard", Method.GET);
		}

		public Locker AutoAssignLocker(AutoAssignReq model)
		{
			return ApiUtility.CallApiSimple<Locker>("locker/autoAssign", Method.POST, model);
		}

		public AssignLockerRes AssignLocker(AssignLockerReq model)
		{
			return ApiUtility.CallApiSimple<AssignLockerRes>("locker/assignLocker", Method.POST, model);
		}

		public ReAssignLockerRes ReassignLocker(ReAssignLockerReq model)
		{
			return ApiUtility.CallApiSimple<ReAssignLockerRes>("locker/reassign", Method.POST, model);
		}

		public ReleaseLockerRes ReleaseLocker(ReleaseLockerReq model)
		{
			return ApiUtility.CallApiSimple<ReleaseLockerRes>("locker/releaseLocker", Method.POST, model);
		}

		public CheckCardRes CheckCard(CheckCardReq model)
		{
			return ApiUtility.CallApiSimple<CheckCardRes>("locker/checkCard", Method.POST, model);
		}

		public GetListLockerRes GetAllLine(GetListLockerReq model)
		{
			return ApiUtility.CallApiSimple<GetListLockerRes>("locker/getAllLine", Method.POST, new
			{
				Value = model
			});
		}

		public LockerMapModel GetAllLocker(GetListLockerReq model)
		{
			LockerMapModel response = new LockerMapModel();
			List<Locker> lockers = ApiUtility.CallApi<Locker>("locker/getAllLocker", Method.POST, new
			{
				Value = model
			});
			if (lockers != null)
			{
				response = GetLockerMapModel(lockers);
			}
			return response;
		}

		public List<Locker> GetLocker(GetListLockerReq model)
		{
			LockerMapModel response = new LockerMapModel();
			return ApiUtility.CallApi<Locker>("locker/getAllLocker", Method.POST, model);
		}

		private LockerMapModel GetLockerMapModel(List<Locker> lockers)
		{
			try
			{
				LockerMapModel obj = new LockerMapModel();
				foreach (Locker item in lockers)
				{
					if (item.Transaction_status == LockerTransactionStatus.IN_USE)
					{
						item.StatusStr = LockerTransactionStatus.GetLockerStatus(LockerTransactionStatus.IN_USE);
					}
					else if (item.Transaction_status == LockerTransactionStatus.AVAI)
					{
						item.StatusStr = LockerTransactionStatus.GetLockerStatus(LockerTransactionStatus.AVAI);
					}
					else if (item.Transaction_status == LockerTransactionStatus.DISABLE)
					{
						item.StatusStr = LockerTransactionStatus.GetLockerStatus(LockerTransactionStatus.DISABLE);
					}
					else
					{
						item.StatusStr = LockerTransactionStatus.GetLockerStatus("");
					}
					if (obj.LockerZones.Count((LockerZone m) => m.Id == item.Zone_id) == 0)
					{
						LockerZone lockerZone = new LockerZone();
						lockerZone.Id = item.Zone_id;
						lockerZone.Name = item.ZoneName;
						LockerLine lockerLine2 = new LockerLine();
						lockerLine2.Id = item.Line_id;
						lockerLine2.Name = item.LineName;
						lockerLine2.Zone_id = item.Zone_id;
						lockerLine2.Lockers.Add(item);
						lockerZone.LockerLines.Add(lockerLine2);
						obj.LockerZones.Add(lockerZone);
						continue;
					}
					LockerZone Lockerzone = obj.LockerZones.FirstOrDefault((LockerZone m) => m.Id == item.Zone_id);
					LockerLine lockerLine = Lockerzone.LockerLines.FirstOrDefault((LockerLine l) => l.Id == item.Line_id);
					if (lockerLine == null)
					{
						lockerLine = new LockerLine();
						lockerLine.Id = item.Line_id;
						lockerLine.Name = item.LineName;
						lockerLine.Zone_id = item.Zone_id;
						lockerLine.Lockers = new List<Locker>();
						lockerLine.Lockers.Add(item);
						Lockerzone.LockerLines.Add(lockerLine);
					}
					else
					{
						lockerLine.Lockers.Add(item);
					}
				}
				return obj;
			}
			catch (Exception)
			{
				return new LockerMapModel();
			}
		}
	}
}
