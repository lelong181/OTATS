using Business.Model;
using BusinessLayer.Model.API;
using BusinessLayer.Repository;
using LSPosMVC.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;
using Ticket.Utils;
using Ticket;
using BusinessLayer.Model;
using LSPos_Data.Models;
using Aspose.Cells;
using Microsoft.Ajax.Utilities;
using Ticket.Business;

namespace LSPosMVC.App_Start
{
    public class GlobalUtilities
    {
        private LoginRepository _login = new LoginRepository();


        public void SetGlobal(string ApiUrl, string username, string password, string SiteCode, string ProfileID, string ProfileCode, string MemberID = "")
        {
            LSPos_Data.Utilities.Log.Info("SetGlobal Start");

            GlobalDAO gdao = new GlobalDAO();
            GlobalModel global = gdao.GetCurrentGlobalBySite(SiteCode, DateTime.Now.Date);
            ApiUtility.ApiUrl = ApiUrl;

            if (global == null || global.Status != "SESSION_GOING_ON")
            {
                LSPos_Data.Utilities.Log.Info("Create new session start");
                PermissionLoginResponse user = _login.ProcessLogin(username, password, SiteCode);
                user.ApiConnect.ConnectKey = MD5.Decrypt(user.ApiConnect.ConnectKey);
                Global.ApiConnect = user.ApiConnect;
                global = new GlobalModel();
                global.ConnectKey = user.ApiConnect.ConnectKey;
                global.ConnectID = user.ApiConnect.ConnectID;
                if (user != null)
                {
                    ComputerModel resComputer = _login.GetComputer(TextUtils.GetHostName());
                    if (resComputer != null)
                    {

                        global.SiteID = user.SiteID.ToString();
                        global.SiteCode = user.SiteCode.ToString();
                        global.ZoneID = (resComputer.ZoneID.HasValue ? resComputer.ZoneID.Value.ToString() : "");
                        global.ComputerID = resComputer.ID.ToString();
                        global.Fullname = !string.IsNullOrWhiteSpace(user.FullName) ? user.FullName : "";
                        global.Username = user.Username;
                        Res res = new SessionRepository().OpenSession(global.Username, global.ComputerID);
                        if (res.Status == "SUCCESS" && !string.IsNullOrEmpty(res.Value.ToString()))
                        {
                            Session session = JsonConvert.DeserializeObject<Session>(res.Value.ToString());
                            if (session != null)
                            {
                                global.SessionID = session.ID.ToString();
                                global.SessionNo = session.SessionNo.ToString();
                                gdao.InsertGlobal(global);
                            }
                        }
                    }

                }
                LSPos_Data.Utilities.Log.Info("Create new session end");
            }

            if (global != null)
            {

                Global.SiteID = global.SiteID.ToString();
                Global.SiteCode = global.SiteCode.ToString();
                Global.ProfileID = ProfileID;
                Global.ProfileCode = ProfileCode;
                Global.ZoneID = global.ZoneID;
                Global.ComputerID = global.ComputerID;
                Global.Fullname = global.Fullname;
                Global.UserID = global.Username;
                Global.UserName = global.Username;
                Global.SessionID = global.SessionID;
                Global.SessionNo = global.SessionNo;
                Global.ApiConnect = new m_ApiConnect();
                Global.ApiConnect.ConnectID = global.ConnectID;
                Global.ApiConnect.ConnectKey = global.ConnectKey;
                try
                {
                    //Global._Profile = new ManagerProfileRepository().GetData(Global.ProfileCode).FirstOrDefault();
                    Global._Profile = new ProfileModel();
                    Global._Profile.Name = "Khach le";
                    Global._Profile.ProfileId = ProfileID;
                    Global._Profile.Address = "";
                    Global._Profile.Phone = "";
                    Global._Profile.Email = "";
                    Global._Profile.AlternativeEmail = "";
                    Global._Profile.IdentityCard = "";
                    Global._Profile.MemberId = MemberID;
                    Global._Profile.ProfileCode = ProfileCode;
                }
                catch (Exception)
                {

                }
            }
            LSPos_Data.Utilities.Log.Info("SetGlobal End");
        }
    }
}