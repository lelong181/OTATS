using LSPosMVC.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LSPos_Data.Data;
using Ksmart_DataSon.Models;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/hanmuc")]
    public class HanMucCongNoController : ApiController
    {
        [HttpGet]
        [Route("getdatakhachhang")]
        public HttpResponseMessage getdatakhachhang([FromUri] int idtinh, int idquan, int idloai)
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
                    KhachHang_dl kh_dl = new KhachHang_dl();
                    DataTable dskh = kh_dl.GetDataKhachHangAll_Grid(userinfo.ID_QLLH, userinfo.ID_QuanLy, idtinh, idquan, idloai, 0);

                    response = Request.CreateResponse(HttpStatusCode.OK, dskh);
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
        [Route("setnguongkhachhang")]
        public HttpResponseMessage setnguongkhachhang([FromUri] int idkhachhang, double nguong)
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
                    GioiHanCongNo_Data cndt = new GioiHanCongNo_Data();
                    if (cndt.SetNguongCongNoKhach(nguong, idkhachhang))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_thietlapnguongcongnothanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_thietlapnguongcongnothatbaivuilongthuchienlai" });
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("getdatanhanvien")]
        public HttpResponseMessage getdatanhanvien([FromUri] int idnhom)
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
                    HanMucData hanMucData = new HanMucData();
                    DataTable dt = hanMucData.GetDSNhanVien_TheoNhomQuanLy(userinfo.ID_QLLH, idnhom);

                    response = Request.CreateResponse(HttpStatusCode.OK, dt);
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
        [Route("setnguongnhanvien")]
        public HttpResponseMessage setnguongnhanvien([FromUri] int idnhanvien, double nguong)
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
                    HanMucData cndt = new HanMucData();
                    if (cndt.SetNguongCongNoNV(nguong, idnhanvien))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_thietlapnguongcongnothanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_thietlapnguongcongnothatbaivuilongthuchienlai" });
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("getdataloaikhachhang")]
        public HttpResponseMessage getdataloaikhachhang()
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
                    LoaiKhachHangDB loaiKhachHangDB = new LoaiKhachHangDB();
                    DataTable data = loaiKhachHangDB.LayDanhSachLoaiKhachHang(userinfo.ID_QLLH);

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
        [Route("setnguongloaikhachhang")]
        public HttpResponseMessage setnguongloaikhachhang([FromUri] int idloaikhachhang, double nguong)
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
                    HanMucData cndt = new HanMucData();
                    if (cndt.SetNguongCongNoNhomKhachHang(nguong, idloaikhachhang))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_thietlapnguongcongnothanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_thietlapnguongcongnothatbaivuilongthuchienlai" });
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("getdatanhomnhanvien")]
        public HttpResponseMessage getdatanhomnhanvien()
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
                    HanMucData hanMucData = new HanMucData();
                    DataTable data = hanMucData.GetListNguongCongNo_NV(userinfo.ID_QLLH);

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
        [Route("setnguongnhomnhanvien")]
        public HttpResponseMessage setnguongnhomnhanvien([FromUri] int idnhom, double nguong)
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
                    HanMucData cndt = new HanMucData();
                    if (cndt.SetNguongCongNoNhomNV(nguong, idnhom))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_thietlapnguongcongnothanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_thietlapnguongcongnothatbaivuilongthuchienlai" });
                    }
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
