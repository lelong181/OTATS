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
    [RoutePrefix("api/dailyrates")]
    public class DailyRatesController : ApiController
    {
        [HttpGet]
        [Route("getbydaterange")]
        public HttpResponseMessage GetByDateRange(int roomTypeId, int ratePlanId, DateTime fromDate, DateTime toDate)
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
                    DailyRatesData db = new DailyRatesData();
                    var data = db.GetByDateRange(roomTypeId, ratePlanId, fromDate, toDate, userinfo.ID_QLLH);
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
        [Route("InsertOrUpdate")]
        public HttpResponseMessage InsertOrUpdate(DailyRatesModel model)
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
                    DailyRatesData db = new DailyRatesData();
                    model.ID_QLLH = userinfo.ID_QLLH;
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
        [Route("bulkupdate")]
        public HttpResponseMessage BulkUpdate(int roomTypeId, int ratePlanId, DateTime fromDate, DateTime toDate, decimal price, bool isClosed)
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
                    DailyRatesData db = new DailyRatesData();
                    db.BulkUpdate(roomTypeId, ratePlanId, fromDate, toDate, price, isClosed, userinfo.ID_QLLH);
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
    }
}
