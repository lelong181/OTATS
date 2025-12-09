using LSPosMVC.Common;
using LSPos_Data.Data;
using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LSPosMVC.Models;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/chuyenquyen")]
    public class ChuyenQuyenController : ApiController
    {
        [HttpGet]
        [Route("getlistnhanvien")]
        public HttpResponseMessage getlistnhanvien()
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
                    NhanVien_dl nv_dl = new NhanVien_dl();
                    List<NhanVien> dsnv = nv_dl.GetDSNhanVien_LoaiBoDanhDauXoa(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    response = Request.CreateResponse(HttpStatusCode.OK, dsnv);
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
        [Route("getlistkhachhangbyidnhanvien")]
        public HttpResponseMessage getlistkhachhangbyidnhanvien([FromUri] int idnhanvien)
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
                    LSPos_Data.Utilities.Log.Info("getlistkhachhangbyidnhanvien:" + idnhanvien.ToString());

                    KhachHangData kh_dl = new KhachHangData();
                    DataTable data = kh_dl.GetKhachHangQuanLy(idnhanvien);

                    response = Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
            }

            return response;
        }

        [HttpPost]
        [Route("capnhat")]
        public HttpResponseMessage ThemKhuyenMai([FromBody] ChuyenQuyenModelFilter chuyenquyen)
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
                    NhanVien_dl nv_dl = new NhanVien_dl();
                    KhachHang_dl khachHang_dl = new KhachHang_dl();

                    foreach ( int idnhanvien in chuyenquyen.List_ID_NhanVien)
                    {
                        foreach (int idkhachhang in chuyenquyen.List_ID_KhachHang)
                        {
                            if (nv_dl.CapNhatKhachHangChoNhanVien(idnhanvien, idkhachhang))
                            {
                                nv_dl.LuuLichSuChuyenGiaoKH(userinfo.ID_QLLH, idnhanvien, idkhachhang);
                            }
                        }
                    }

                    foreach (int idkhachhang in chuyenquyen.List_ID_KhachHang)
                    {
                        //set quyền nhan viên cũ = trống
                        khachHang_dl.PhanQuyenKhachHang(chuyenquyen.ID_NhanVien_Old, idkhachhang, ";", userinfo.ID_QuanLy);
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, data = 0, msg = "Chuyển giao khách hàng thành công" });
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