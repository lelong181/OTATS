using LSPos_Data.Data;
using LSPos_Data.Models;
using LSPosMVC.Common;
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/roomtyperateplans")]
    public class RoomTypeRatePlansController : ApiController
    {
        [HttpGet]
        [Route("getbyroomtype")]
        public HttpResponseMessage GetByRoomType(int roomTypeId)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    RoomTypeRatePlansData db = new RoomTypeRatePlansData();
                    var data = db.GetByRoomType(roomTypeId,userinfo.ID_QLLH);
                    response = Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        [HttpPost]
        [Route("insertorupdate")]
        public HttpResponseMessage InsertOrUpdate(RoomTypeRatePlansModel model)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    RoomTypeRatePlansData db = new RoomTypeRatePlansData();
                    model.ID_QLH = userinfo.ID_QLLH;
                    db.InsertOrUpdate(model);
                    response = Request.CreateResponse(HttpStatusCode.OK, true);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        [HttpPost]
        [Route("delete")]
        public HttpResponseMessage Delete(int roomTypeId, int ratePlanId)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    RoomTypeRatePlansData db = new RoomTypeRatePlansData();
                    bool result = db.Delete(roomTypeId, ratePlanId);
                    response = Request.CreateResponse(HttpStatusCode.OK, result);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }
    }
}
