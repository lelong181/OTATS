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
    [RoutePrefix("api/rooms")]
    public class RoomsController : ApiController
    {
        [HttpGet]
        [Route("getall")]
        public HttpResponseMessage GetAll(int? hotelId = null, int? buildingId = null, int? floorId = null, string status = null)
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
                    RoomsData db = new RoomsData();
                    var data = db.GetAll(hotelId, buildingId, floorId, status, userinfo.ID_QLLH);
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
        [Route("insert")]
        public HttpResponseMessage Insert(RoomsModel model)
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
                    RoomsData db = new RoomsData();
                    model.ID_QLLH = userinfo.ID_QLLH;
                    long newId = db.Insert(model);
                    response = Request.CreateResponse(HttpStatusCode.OK, newId);
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
        [Route("updatestatus")]
        public HttpResponseMessage UpdateStatus(RoomsModel model)
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
                    RoomsData db = new RoomsData();
                    bool result = db.UpdateStatus(model.RoomId, model.Status, model.CleanStatus);
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
