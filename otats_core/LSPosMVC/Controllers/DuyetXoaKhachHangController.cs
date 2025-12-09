using LSPosMVC.Common;
using LSPos_Data.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/duyetkhachhang")]
    public class DuyetXoaKhachHangController : ApiController
    {
        public class Results
        {
            public int idKhachHang { set; get; }
            public string maKhachHang { set; get; }
            public string tenKhachHang { set; get; }
            public string diaChi { set; get; }
            public string nhanVienQuanLy { set; get; }
            public string ghiChu { set; get; }
            public string yeuCau { set; get; }
            public int trangThai { set; get; }
        }
        [HttpGet]
        [Route("getdanhsach")]
        public HttpResponseMessage get()
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
                    KhachHangData kh_dl = new KhachHangData();
                    DataTable dskh = kh_dl.GetDataKhachHangDuyetXoa(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    List<Results> list = new List<Results>();

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

        [HttpGet]
        [Route("huyyeucau")]
        public HttpResponseMessage huyyeucau([FromUri] int idKhachHang, int trangThai)
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

                    if (trangThai == 1)
                    {
                        if (kh_dl.DeleteKhachHang(userinfo.ID_QLLH, idKhachHang))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, true);
                        }
                    }
                    if (trangThai == 3)
                    {
                        if (kh_dl.DuyetKhachHang(userinfo.ID_QLLH, idKhachHang, 4))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, true);
                        }
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
        [Route("duyetyeucau")]
        public HttpResponseMessage duyetyeucau([FromUri] int idKhachHang, int trangThai)
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

                    if (trangThai == 1)
                    {
                        if (kh_dl.DuyetKhachHang(userinfo.ID_QLLH, idKhachHang, 2))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, true);
                        }
                    }
                    if (trangThai == 3)
                    {
                        if (kh_dl.DeleteKhachHang(userinfo.ID_QLLH, idKhachHang))
                            response = Request.CreateResponse(HttpStatusCode.OK, true);
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
        [Route("chuyenquyen")]
        public HttpResponseMessage chuyenquyen([FromUri] int idKhachHang, string listid)
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
                    string quyen = ";1;2;3;4;";

                    KhachHang_dl kh_dl = new KhachHang_dl();
                    //remove all quyen
                    kh_dl.XoaPhanQuyen(idKhachHang);
                    //huy bo duyet xoa
                    kh_dl.DuyetKhachHang(userinfo.ID_QLLH, idKhachHang, 4);

                    string[] words = listid.Split(',');
                    foreach (var word in words)
                    {
                        int id = Convert.ToInt32(word);

                        if (kh_dl.PhanQuyenKhachHang(id, idKhachHang, quyen,userinfo.ID_QuanLy) > 0)
                        {
                            try
                            {
                                string mess_push = "Bạn được phân công khách hàng mới, vui lòng vào mục khách hàng để kiểm tra";
                                String respon = new System.Net.WebClient().DownloadString(Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + "/AppPush.aspx?idnhanvien=" + id + "&type=phanquyenkhachhang&ngay=" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "&message=" + mess_push);
                            }
                            catch (Exception ex)
                            {
                                LSPos_Data.Utilities.Log.Error(ex);
                            }
                        }
                    }
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
