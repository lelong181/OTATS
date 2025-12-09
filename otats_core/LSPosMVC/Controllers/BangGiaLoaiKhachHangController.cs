using LSPosMVC.Common;
using LSPos_Data.Data;
using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LSPosMVC.Controllers
{

    /// <summary>
    /// Author edit: Dongnn
    /// Date: 2019-07-11
    /// </summary>

    [Authorize]
    [RoutePrefix("api/banggialoaikhachhang")]
    public class BangGiaLoaiKhachHangController : ApiController
    {
        /// <summary>
        /// Thêm bảng giá
        /// </summary>
        /// <param name="bangGia"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        public HttpResponseMessage add([FromBody] BangGiaLoaiKhachHangModel bangGia)
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
                    BangGiaLoaiKhachHangData bangGiaData = new BangGiaLoaiKhachHangData();

                    if (bangGiaData.add(bangGia) > 0)
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

        /// <summary>
        /// update bảng giá
        /// </summary>
        /// <param name="bangGia"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        public HttpResponseMessage update([FromBody] BangGiaLoaiKhachHangModel bangGia)
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
                    BangGiaLoaiKhachHangData bangGiaData = new BangGiaLoaiKhachHangData();

                    if (bangGiaData.update(bangGia))
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
        [Route("deletemark")]
        public HttpResponseMessage deletemark([FromBody] int id, string updatedBy, bool deleteMark)
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
                    BangGiaLoaiKhachHangData bangGiaData = new BangGiaLoaiKhachHangData();

                    if (bangGiaData.deletemark(id, updatedBy, deleteMark))
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

        /// <summary>
        /// Get thông tin bảng giá theo ID mặt hàng
        /// </summary>
        /// <param name="idMatHang"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getbanggia")]
        public HttpResponseMessage getbanggia([FromUri] int idMatHang)
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
                    BangGiaLoaiKhachHangData bangGiaData = new BangGiaLoaiKhachHangData();
                    List<BangGiaLoaiKhachHangModel> li = new List<BangGiaLoaiKhachHangModel>();
                    li = bangGiaData.getbanggia(idMatHang);
                    response = Request.CreateResponse(HttpStatusCode.OK, li);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
      
        /// <summary>
        /// Get loại khách hàng thiếu theo ID mặt hàng
        /// </summary>
        /// <param name="idMatHang"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getloaikhachhangthieu")]
        public HttpResponseMessage getloaikhachhangthieu([FromUri] int idMatHang)
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
                    BangGiaLoaiKhachHangData bangGiaData = new BangGiaLoaiKhachHangData();
                    List<BangGiaLoaiKhachHangModel> li = new List<BangGiaLoaiKhachHangModel>();
                    li = bangGiaData.getloaikhachhangthieu(userinfo.ID_QLLH, idMatHang);
                    response = Request.CreateResponse(HttpStatusCode.OK, li);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        /// <summary>
        /// Get loại khách hàng theo ID mặt hàng
        /// </summary>
        /// <param name="idMatHang"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getloaikhachhang")]
        public HttpResponseMessage get([FromUri] int idMatHang)
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
                    BangGiaLoaiKhachHangData bangGiaData = new BangGiaLoaiKhachHangData();
                    List<LoaiKhachHangOBJ> lst = bangGiaData.getloaikhachhangbyidmathang(userinfo.ID_QLLH, idMatHang);

                    response = Request.CreateResponse(HttpStatusCode.OK, lst);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        /// <summary>
        /// Cập nhật bảng giá cho mặt hàng
        /// </summary>
        /// <param name="listBangGia"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("hoantat")]
        public HttpResponseMessage capnhatbanggiachomathang([FromBody] List<BangGiaLoaiKhachHangModel> listBangGia)
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
                    BangGiaLoaiKhachHangData bangGiaData = new BangGiaLoaiKhachHangData();
                    string listId = "";
                    int i = 1;
                    int idMatHang = 0;
                    foreach (BangGiaLoaiKhachHangModel bg in listBangGia)
                    {
                        idMatHang = bg.IDMatHang;

                        bg.CreatedBy = userinfo.Username;
                        bg.UpdateBy = userinfo.Username;
                        bg.DeleteMark = false;

                        if (bg.ID > 0)
                        {
                            if (i == 1)
                            {
                                listId = listId + bg.ID.ToString();
                                i = 2;
                            }
                            else
                                listId = listId + "," + bg.ID.ToString();

                            bangGiaData.update(bg);
                        }
                        else
                        {
                            BangGiaLoaiKhachHangModel bgc = bangGiaData.getbanggiabyidmathangidloaikhachhang(bg.IDMatHang, bg.IDLoaiKhachHang);
                            if (bgc.ID > 0)
                            {
                                if (i == 1)
                                {
                                    listId = listId + bgc.ID.ToString();
                                    i = 2;
                                }
                                else
                                    listId = listId + "," + bgc.ID.ToString();

                                bg.ID = bgc.ID;
                                bangGiaData.update(bg);
                            }
                            else
                            {
                                int id = bangGiaData.add(bg);
                                if (i == 1)
                                {
                                    listId = listId + id.ToString();
                                    i = 2;
                                }
                                else
                                    listId = listId + "," + id.ToString();
                            }
                        }
                    }

                    bangGiaData.deleteMarkofmathang(idMatHang, listId, userinfo.Username);

                    response = Request.CreateResponse(HttpStatusCode.OK, listId);
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
