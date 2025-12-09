
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

using LSPos_Data.Models;


using LSPos_Data.Data;
using Ksmart_DataSon.Models;
using LSPosMVC.Models;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/phanquyen")]
    [EnableCors(origins: "*", "*", "*")]
    public class PhanQuyenController : ApiController
    {

        #region phân quyền chức năng
        [HttpPost]
        [Route("capnhatphanquyennhom")]
        public HttpResponseMessage capnhatphanquyennhom([FromBody] QuyenNhomModels quyen)
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
                    try
                    {
                        PhanQuyenData phanQuyenData = new PhanQuyenData();
                        phanQuyenData.DeleteChucNang_Nhom(quyen.ID_Nhom);
                        foreach (QuyenModels quyenweb in quyen.ListChucNangWEB)
                        {
                            if (quyenweb.Xem == 1)
                                phanQuyenData.ThemChucNangChoNhom_Quyen(quyen.ID_Nhom,
                                    quyenweb.ID_ChucNang, quyenweb.Them, quyenweb.Sua, quyenweb.Xoa);
                        }

                        foreach (QuyenModels quyenapp in quyen.ListChucNangAPP)
                        {
                            if (quyenapp.Xem == 1)
                                phanQuyenData.ThemChucNangChoNhom(quyen.ID_Nhom, quyenapp.ID_ChucNang);
                        }

                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_phanquyenthanhcong" });
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_phanquyenkhongthanhcongvuilonglienhequantri" });
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
        [Route("getchucnangapp")]
        public HttpResponseMessage getchucnangapp([FromUri] int idnhom)
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
                    PhanQuyenData phanQuyenData = new PhanQuyenData();
                    DataTable list = phanQuyenData.chucnangapp_getlist(userinfo.ID_QLLH, idnhom);
                    response = Request.CreateResponse(HttpStatusCode.OK, list);
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
        [Route("getchucnangweb")]
        public HttpResponseMessage getchucnangweb([FromUri] int idnhom)
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
                    PhanQuyenData phanQuyenData = new PhanQuyenData();
                    DataTable list = phanQuyenData.chucnangweb_getlist(userinfo.ID_QLLH, idnhom);
                    response = Request.CreateResponse(HttpStatusCode.OK, list);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        #endregion

        #region phân quyền ngành hàng theo nhóm nhân vien
        [HttpGet]
        [Route("getdatanganhhangphanquyen")]
        public HttpResponseMessage getdatanganhhangphanquyen([FromUri] int idnhom)
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
                    PhanQuyenNganhHang_Nhom_dl PQCN_Nhom = new PhanQuyenNganhHang_Nhom_dl();
                    List<DanhMucOBJ> list = PQCN_Nhom.GetByIDNhom(idnhom);
                    response = Request.CreateResponse(HttpStatusCode.OK, list);
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
        [Route("getdatanganhhang")]
        public HttpResponseMessage getdatanganhhang()
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
                    List<DanhMucOBJ> lstDanhMuc = DanhMucDB.getDS_DanhMuc(userinfo.ID_QLLH);
                    response = Request.CreateResponse(HttpStatusCode.OK, lstDanhMuc);
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
        [Route("xoaphanquyennganhhangnhom")]
        public HttpResponseMessage xoaphanquyennganhhangnhom([FromUri] int idnhom, int idnganhhang)
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
                    PhanQuyenNganhHang_Nhom_dl PQCN_Nhom = new PhanQuyenNganhHang_Nhom_dl();
                    if (PQCN_Nhom.XoaNganhHang_Nhom(idnhom, idnganhhang))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoaphanquyenthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoaphanquyenkhongthanhcongvuilonglienhequantri" });
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

        [HttpPost]
        [Route("themnganhhangnhom")]
        public HttpResponseMessage themnganhhangnhom([FromUri] int idnhom, int idnganhhang)
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
                    PhanQuyenNganhHang_Nhom_dl PQCN_Nhom = new PhanQuyenNganhHang_Nhom_dl();
                    if (PQCN_Nhom.ThemNganhHangChoNhom(idnhom, idnganhhang))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_phanquyennganhhangchonhomthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_capnhatphanquyenkhongthanhcongvuilonglienhequantri" });
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

        #endregion

        #region phân quyền nhân viên theo khách hàng
        [HttpPost]
        [Route("phanquyen_nhanvienkhachhang")]
        public HttpResponseMessage phanquyen_nhanvienkhachhang([FromBody] PhanQuyen_KhachHanngNhanVienModel item)
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
                    KhachHang_dl khdl = new KhachHang_dl();
                    if (khdl.PhanQuyenKhachHang(item.ID_NhanVien, item.ID_KhachHang, item.ID_Quyen, userinfo.ID_QuanLy) > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_phanquyenthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_phanquyenkhongthanhcongvuilonglienhequantri" });
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
        [HttpPost]
        [Route("phanquyennhieunhanvienkhachhang")]
        public HttpResponseMessage phanquyennhieunhanvienkhachhang([FromBody] List<int> listnhanvien, [FromUri] int idkhachhang, string idquyen)
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
                    KhachHang_dl khdl = new KhachHang_dl();
                    int cnt = 0;
                    foreach (int idnhanvien in listnhanvien)
                    {
                        khdl.PhanQuyenKhachHang(idnhanvien, idkhachhang, idquyen, userinfo.ID_QuanLy);
                        cnt++;
                        try
                        {
                            string mess_push = "Bạn được phân công khách hàng mới, vui lòng vào mục khách hàng để kiểm tra";
                            String res = new System.Net.WebClient().DownloadString(Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + "/AppPush.aspx?idnhanvien=" + 
                                idnhanvien + "&type=phanquyenkhachhang&ngay=" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "&message=" + mess_push);
                        }
                        catch (Exception ex)
                        {
                            LSPos_Data.Utilities.Log.Error(ex);
                        }
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_phanquyenkhachhangchonhanvienthanhcong" });

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
        [Route("saochepphanquyen_nhieunhanvienkhachhang")]
        public HttpResponseMessage saochepphanquyen_nhieunhanvienkhachhang([FromBody] List<int> ListKhachHangDich, [FromUri] int ID_KhachHangNguon)
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
                    BaoCaoCommon baocao = new BaoCaoCommon();
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    string msg = "";
                    string msg_beg = "";
                    string msg_end = "";
                    if(lang == "vi")
                    {
                        msg_beg = "Đã gán thành công danh sách nhân viên phân quyền cho ";
                        msg_end = " khách hàng!";
                    }
                    else
                    {
                        msg_beg = "Applied employee(s) authorization to ";
                        msg_end = " customer(s) successfully!";
                    }

                    KhachHang_dl khdl = new KhachHang_dl();
                    int p = 0;
                    foreach (int idkhdich in ListKhachHangDich)
                    {

                        khdl.SaoChepPhanQuyen(ID_KhachHangNguon, idkhdich, ";", userinfo.ID_QuanLy);
                        p++;
                    }

                    msg = msg_beg + p.ToString() + msg_end;

                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = msg });
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
        [Route("saochepphanquyen_khachhangnhanvien")]
        public HttpResponseMessage saochepphanquyen_khachhangnhanvien([FromUri] int IDNVNguon, int IDNVDich)
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
                    KhachHangData khdl = new KhachHangData();
                    if (khdl.SaoChepPhanQuyenNhanVien(IDNVNguon, IDNVDich, ";", userinfo.ID_QuanLy) > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_saochepphanquyenkhachhangchonhanvienthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_saochepphanquyenkhachhangchonhanvienthatbaivuilongthuchienlai" });
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

        [HttpPost]
        [Route("removephanquyen_nhanvienkhachhang")]
        public HttpResponseMessage removephanquyen_nhanvienkhachhang([FromUri] int ID_KhachHang)
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
                    KhachHang_dl khdl = new KhachHang_dl();
                    if (khdl.XoaPhanQuyen(ID_KhachHang) > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoaphanquyenkhachhangchonhanvienthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoaphanquyenkhachhangchonhanvienthatbaivuilongthuchienlai" });
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

        [HttpPost]
        [Route("removephanquyen_khachhangnhanvien")]
        public HttpResponseMessage removephanquyen_khachhangnhanvien([FromUri] int ID_NhanVien)
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
                    KhachHangData khdl = new KhachHangData();
                    if (khdl.XoaPhanQuyenNhanVien(ID_NhanVien) > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoaphanquyenkhachhangchonhanvienthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoaphanquyenkhachhangchonhanvienthatbaivuilongthuchienlai" });
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
        [Route("getDSPhanQuyenNhanVienByKhachHang")]
        public HttpResponseMessage getDSPhanQuyenNhanVienByKhachHang([FromUri] int IdNhom, int ID_KhachHang)
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
                    //NhanVien_dl nv_dl = new NhanVien_dl();
                    PhanQuyen_KhachHangNhanVienData nvapp = new PhanQuyen_KhachHangNhanVienData();
                    List<PhanQuyen_KhachHanngNhanVienModel> dsnv = nvapp.GetDSPhanQuyenNhanVien_TheoNhomKhachHang(userinfo.ID_QLLH, IdNhom, ID_KhachHang);


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
        [Route("getnhanviendaphanchokhachhang")]
        public HttpResponseMessage getnhanviendaphanchokhachhang([FromUri] int idkhachhang)
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
                    PhanQuyen_KhachHangNhanVienData nvapp = new PhanQuyen_KhachHangNhanVienData();
                    DataTable dsnv = nvapp.getnhanviendaphanchokhachhang(userinfo.ID_QLLH, idkhachhang);

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

        #endregion

        #region phân quyền khách hàng theo nhóm nhân viên

        [HttpGet]
        [Route("getDSPhanQuyenNhomNhanVienByKhachHang")]
        public HttpResponseMessage getDSPhanQuyenNhomNhanVienByKhachHang([FromUri] int ID_KhachHang)
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
                    PhanQuyen_KhachHangNhomNhanVienData nvapp = new PhanQuyen_KhachHangNhomNhanVienData();
                    List<PhanQuyen_KhachHangNhomNhanVienModel> dsnv = nvapp.GetDSPhanQuyenNhanVien_TheoNhomKhachHang(userinfo.ID_QLLH, ID_KhachHang);
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

        [HttpPost]
        [Route("phanquyen_nhomnhanvienkhachhang")]
        public HttpResponseMessage phanquyen_nhomnhanvienkhachhang([FromBody] PhanQuyen_KhachHangNhomNhanVienModel item)
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
                    KhachHang_dl khdl = new KhachHang_dl();
                    if (khdl.PhanQuyenKhachHang_Nhom(item.ID_Nhom, item.ID_KhachHang, item.ID_Quyen) > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_phanquyenkhachhangchonhomnhanvienthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_phanquyenkhachhangchonhomnhanvienthatbaivuilongthuchienlai" });
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

        [HttpPost]
        [Route("phanquyennhieunhomnhanvienkhachhang")]
        public HttpResponseMessage phanquyennhieunhomnhanvienkhachhang([FromBody] List<int> listnhomnhanvien, [FromUri] int idkhachhang, string idquyen)
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
                    int n = 0;
                    KhachHang_dl khdl = new KhachHang_dl();
                    foreach (int idnhom in listnhomnhanvien)
                    {
                        if (khdl.PhanQuyenKhachHang_Nhom(idnhom, idkhachhang, idquyen) > 0)
                            n += 1;
                    }

                    if (n > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_phanquyenkhachhangchonhomnhanvienthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_phanquyenkhachhangchonhomnhanvienthatbaivuilongthuchienlai" });
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

        [HttpPost]
        [Route("saochepphanquyen_nhomnhanvienkhachhang")]
        public HttpResponseMessage saochepphanquyen_nhomnhanvienkhachhang([FromUri] int ID_NhomNguon, int ID_NhomDich)
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
                    KhachHang_dl khdl = new KhachHang_dl();
                    if (khdl.SaoChepPhanQuyen_Nhom(ID_NhomNguon, ID_NhomDich, ";") > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_saochepphanquyenkhachhangchonhomnhanvienthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_saochepphanquyenkhachhangchonhomnhanvienthatbaivuilongthuchienlai" });
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

        #endregion

        #region phân quyền khách hàng theo nhân viên
        [HttpGet]
        [Route("getlistnhanvienphanquyen")]
        public HttpResponseMessage getlistnhanvienphanquyen()
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
        [Route("getlistkhachhangdaphanquyen")]
        public HttpResponseMessage getlistkhachhangdaphanquyen([FromUri] int idnhanvien)
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
                    DataTable data = kh_dl.GetKhachHangDaCapQuyen(idnhanvien, -1, 0, 0, 0, 0);

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

        [HttpGet]
        [Route("getlistkhachhangchuaphanquyen")]
        public HttpResponseMessage getlistkhachhangchuaphanquyen([FromUri] int idnhanvien)
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
                    DataTable data = kh_dl.GetKhachHangChuaCapQuyen(idnhanvien, userinfo.ID_QLLH, userinfo.ID_QuanLy, 0, 0, 0, 0, 0);

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
        [Route("phanquyenkhachhangchonhanvien")]
        public HttpResponseMessage phanquyenkhachhangchonhanvien([FromBody] List<int> listkhachhang, [FromUri] int idnhanvien, string idquyen)
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
                    int n = 0;
                    KhachHang_dl khdl = new KhachHang_dl();
                    foreach (int idkhachhang in listkhachhang)
                    {
                        if (khdl.PhanQuyenKhachHang(idnhanvien, idkhachhang, idquyen, userinfo.ID_QuanLy) > 0)
                            n += 1;
                    }

                    if (n > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_phanquyenthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_phanquyenkhongthanhcongvuilonglienhequantri" });
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

        [HttpPost]
        [Route("bophanquyenkhachhangchonhanvien")]
        public HttpResponseMessage bophanquyenkhachhangchonhanvien([FromBody] List<int> listkhachhang, [FromUri] int idnhanvien)
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
                    int n = 0;
                    KhachHang_dl khdl = new KhachHang_dl();
                    foreach (int idkhachhang in listkhachhang)
                    {
                        if (khdl.PhanQuyenKhachHang(idnhanvien, idkhachhang, ";", userinfo.ID_QuanLy) > 0)
                            n += 1;
                    }

                    if (n > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_boquyenthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_boquyenthatbaivuilongthuchienlai" });
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

        #endregion

        #region excel phân quyền
        [HttpGet]
        [Route("ExportExcelPhanQuyen")]
        public HttpResponseMessage ExportExcelPhanQuyen()
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
                    PhanQuyen_KhachHangNhanVienData bc_dl = new PhanQuyen_KhachHangNhanVienData();
                    DataSet ds = bc_dl.getdataexcelphanquyen(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    ds.Tables[0].TableName = "DATA";

                    BaoCaoCommon baocao = new BaoCaoCommon();
                    DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    dt2.TableName = "DATA2";
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    DataTable dt1 = new DataTable();
                    dt1.TableName = "DATA1";
                    dt1.Columns.Add("TITLE", typeof(String));
                    DataRow row = dt1.NewRow();
                    row["TITLE"] = "";
                    dt1.Rows.Add(row);

                    DataSet dataSet = new DataSet();
                    dataSet.Tables.Add(ds.Tables[0].Copy());
                    dataSet.Tables.Add(dt1.Copy());
                    dataSet.Tables.Add(dt2.Copy());

                    string filename = "";
                    var stream = new MemoryStream();
                    ExportExcel excel = new ExportExcel();

                    if (lang == "vi")
                    {
                        filename = "BM077_phanquyennhanvientheokhachhang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("ExcelPhanQuyen.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM077_DelegateEmployeeBasedOnCustomer_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("ExcelPhanQuyen_en.xls", dataSet, null, ref stream);
                    }

                    response.Content = new ByteArrayContent(stream.ToArray());
                    response.Content.Headers.Add("x-filename", filename);
                    response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = filename;
                    response.StatusCode = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;

        }

        [HttpGet]
        [Route("GetTemplatePhanQuyen")]
        public HttpResponseMessage GetTemplatePhanQuyen()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                User_dl userDL = new User_dl();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    ds.Tables.Add(dt.Copy());

                    BaoCaoCommon baocao = new BaoCaoCommon();
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    string filename = "";
                    var stream = new MemoryStream();
                    ExportExcel excel = new ExportExcel();

                    if (lang == "vi")
                    {
                        filename = "BM078_FileMauPhanQuyenNhanVienTheoKhachHang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("File_Mau_Phan_Quyen.xls", ds, null, ref stream);
                    }
                    else
                    {
                        filename = "BM078_DelegateEmployeeBasedOnCustomerTemplate_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("File_Mau_Phan_Quyen_en.xls", ds, null, ref stream);
                    }

                    response.Content = new ByteArrayContent(stream.ToArray());
                    response.Content.Headers.Add("x-filename", filename);
                    response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = filename;
                    response.StatusCode = HttpStatusCode.OK;
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
        [Route("importphanquyen")]
        public HttpResponseMessage importphanquyen([FromBody] FileUploadModelFilter file)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            MemoryStream tempPath = new MemoryStream();

            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("MaKH", typeof(String));
                    dt.Columns.Add("TenKhachHang", typeof(String));
                    dt.Columns.Add("SoDienThoai", typeof(String));
                    dt.Columns.Add("TenDangNhap", typeof(String));

                    Aspose.Cells.Workbook workbook;
                    Aspose.Cells.Worksheet worksheet;
                    DataTable dtDataHeard = new DataTable();
                    string templatefolder = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUpload"];
                    string fileName = AppDomain.CurrentDomain.BaseDirectory + templatefolder + @"\" + file.filename;
                    workbook = new Aspose.Cells.Workbook(fileName);
                    if (workbook.Worksheets.GetSheetByCodeName("DATA") == null)
                    {
                        return response = Request.CreateResponse(HttpStatusCode.NoContent, new { success = false, msg = "label_filemaukhongdungdinhdang" });
                    }

                    worksheet = workbook.Worksheets[0];
                    dt = worksheet.Cells.ExportDataTableAsString(2, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, true);
                    if (System.IO.File.Exists(fileName))
                        System.IO.File.Delete(fileName);
                    dtDataHeard = dt.Clone();
                    foreach (var row in dt.Rows)
                    {
                        DataRow _datarow = row as DataRow;
                        var isRow = ImportValidate.TrimRow(ref _datarow, false);
                        if (!isRow)
                            continue;
                        dtDataHeard.ImportRow(_datarow);
                    }
                    if (dtDataHeard.Rows.Count == 0)
                    {
                        return response = Request.CreateResponse(HttpStatusCode.NoContent, new { success = false, msg = "label_khongtontaibanghi" });
                    }
                    if (dtDataHeard.Rows.Count > 2000)
                    {
                        return response = Request.CreateResponse(HttpStatusCode.LengthRequired, new { success = false, msg = "label_dulieutrongfileimportkhongduocqua2000dongvuilongkiemtralai" });
                    }

                    if (validatedataphanquyen(dtDataHeard, userinfo, ref tempPath))
                    {
                        try
                        {
                            KhachHang_dl kh_dl = new KhachHang_dl();

                            foreach (DataRow row in dtDataHeard.Rows)
                            {
                                string makh = row["MaKH"].ToString();
                                string tenkh = row["TenKhachHang"].ToString();
                                string sodienthoai = row["SoDienThoai"].ToString();
                                string tendangnhap = row["TenDangNhap"].ToString();

                                KhachHang khachhangobj = kh_dl.GetKhachHangTheoTenSDT(userinfo.ID_QLLH, makh, tenkh, sodienthoai);

                                if (khachhangobj != null && khachhangobj.IDKhachHang > 0)
                                {
                                    NhanVien nvobj = NhanVien_dl.ChiTietNhanVienTheoTenDangNhap(tendangnhap, userinfo.ID_QLLH);

                                    kh_dl.PhanQuyenKhachHang(nvobj.IDNV, khachhangobj.IDKhachHang, ";1;2;3;4;", userinfo.ID_QuanLy);
                                }
                            }

                            return response = Request.CreateResponse(HttpStatusCode.OK, true);
                        }
                        catch (Exception ex)
                        {
                            LSPos_Data.Utilities.Log.Error(ex);
                            return response = Request.CreateResponse(HttpStatusCode.NotModified);
                        }
                    }
                    else
                    {
                        BaoCaoCommon baocao = new BaoCaoCommon();
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string filename = "";
                        if (lang == "vi")
                        {
                            filename = "BM079_FileMauPhanQuyenNhanVienTheoKhachHangError_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        }
                        else
                        {
                            filename = "BM079_DelegateEmployeeBasedOnCustomerTemplateError_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        }

                        response.Content = new ByteArrayContent(tempPath.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.Created;
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        private bool validatedataphanquyen(DataTable dtDataHeard, UserInfo userinfo, ref MemoryStream fileStream)
        {
            try
            {
                BaoCaoCommon baocao = new BaoCaoCommon();
                string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                KhachHang_dl kh_dl = new KhachHang_dl();

                DataTable dtError = dtDataHeard.Clone();
                dtError.TableName = "DATA";

                List<string> lstEmp = new List<string>();

                int iRow = 4;
                bool IsError = false;
                foreach (DataRow row in dtDataHeard.Rows)
                {
                    string sError = "";
                    DataRow _datarow = row as DataRow;
                    var rowError = dtError.NewRow();
                    sError = (lang == "vi") ? "Tên khách hàng hàng chưa nhập" : "Missing customer's name";
                    ImportValidate.EmptyValue("TenKhachHang", ref _datarow, ref rowError, ref IsError, sError);
                    sError = (lang == "vi") ? "Số điện thoại khách hàng chưa nhập" : "Missing customer's phone number";
                    ImportValidate.EmptyValue("SoDienThoai", ref _datarow, ref rowError, ref IsError, sError);
                    sError = (lang == "vi") ? "Tên đăng nhập nhân viên quản lý chưa nhập" : "Missing manager's username";
                    ImportValidate.EmptyValue("TenDangNhap", ref _datarow, ref rowError, ref IsError, sError);
                    
                    if (!IsError)
                    {
                        KhachHang khachhangobj = kh_dl.GetKhachHangTheoTenSDT(userinfo.ID_QLLH
                            , row["MaKH"].ToString(), row["TenKhachHang"].ToString(), row["SoDienThoai"].ToString());

                        if (khachhangobj == null || khachhangobj.IDKhachHang <= 0)
                        {
                            rowError["MaKH"] = (lang == "vi") ? "Thông tin khách hàng không tồn tại" : "Customer does not exist";
                            IsError = true;
                        }
                    }

                    if (!IsError)
                    {
                        NhanVien nvobj = NhanVien_dl.ChiTietNhanVienTheoTenDangNhap(row["TenDangNhap"].ToString(), userinfo.ID_QLLH);

                        if (nvobj == null || nvobj.IDNV <= 0)
                        {
                            rowError["TenDangNhap"] = (lang == "vi") ? "Thông tin nhân viên không tồn tại" : "Employee does not exist";
                            IsError = true;
                        }
                    }

                    if (IsError)
                    {
                        rowError["STT"] = iRow;
                        dtError.Rows.Add(rowError);
                    }

                    iRow = (iRow + 1);
                    IsError = false;
                }
                if ((dtError.Rows.Count > 0))
                {
                    dtError.TableName = "DATA";
                    
                    ExportExcel ExportExcel = new ExportExcel();
                    if (lang == "vi")
                    {
                        ExportExcel.ExportTemplateToStream("File_Mau_Phan_Quyen_Error.xls", dtError, null, ref fileStream);
                    }
                    else
                    {
                        ExportExcel.ExportTemplateToStream("File_Mau_Phan_Quyen_Error_en.xls", dtError, null, ref fileStream);
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return true;
        }
        #endregion

    }
}
