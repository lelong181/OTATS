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
    [Authorize]
    [RoutePrefix("api/guitinnhan")]
    public class GuiTinNhanController : ApiController
    {
        public class ResultsNhanVien
        {
            public int dangtructuyen { get; set; }
            public int idnhanvien { get; set; }
            public int ID_Nhom { get; set; }
            public double KinhDo { get; set; }
            public string tennhanvien { get; set; }
            public string trangthai { get; set; }
            public string thoigiancapnhat { get; set; }
            public double ViDo { get; set; }
            public string thoigianhoatdong { get; set; }
            public string anhdaidien { get; set; }
            public string tendangnhap { get; set; }
            public string tinhtrangpin { get; set; }
        }

        public string GetTrangThai(object TrangThai, string lang)
        {
            string strangthai = "";
            if (TrangThai.ToString() == "0")
                strangthai = (lang == "vi") ? "Ngoại tuyến" : "Offline";
            else if (TrangThai.ToString() == "1")
                strangthai = (lang == "vi") ? "Trực truyến" : "Online";
            else if (TrangThai.ToString() == "2")
                strangthai = (lang == "vi") ? "Mất kết nối" : "Disconnect";
            return strangthai;
        }

        [HttpGet]
        [Route("getlistnhanvien")]
        public HttpResponseMessage get(float KinhDo, float ViDo, int BanKinh, int TrangThai, int idNhom)
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

                    List<ResultsNhanVien> results = new List<ResultsNhanVien>();
                    LoTrinhData ldt = new LoTrinhData();
                    List<ViTriNhanVienGPSModel> list = new List<ViTriNhanVienGPSModel>();
                    if (BanKinh > 0)
                    {
                        list = ldt.GetViTriTatCaNVTheoToaDo(userinfo.ID_QLLH, userinfo.ID_QuanLy, KinhDo, ViDo, BanKinh, TrangThai);
                    }
                    else
                    {
                        list = ldt.GetViTriTatCaNVOnline(userinfo.ID_QLLH, userinfo.ID_QuanLy, TrangThai, idNhom);
                    }


                    foreach (ViTriNhanVienGPSModel nv in list)
                    {
                        ResultsNhanVien result = new ResultsNhanVien();
                        result.dangtructuyen = nv.dangtructuyen;
                        result.idnhanvien = nv.idnhanvien;
                        result.ID_Nhom = nv.ID_Nhom;
                        result.KinhDo = nv.KinhDo;
                        result.ViDo = nv.ViDo;
                        result.tennhanvien = nv.tennhanvien;
                        result.thoigiancapnhat = nv.thoigiancapnhat;
                        result.trangthai = GetTrangThai(nv.dangtructuyen, lang);
                        result.thoigianhoatdong = nv.thoigianguitoado;
                        result.anhdaidien = nv.anhdaidien;
                        result.tendangnhap = nv.TenDangNhap;
                        result.tinhtrangpin = nv.TinhTrangPin;
                        results.Add(result);
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, results);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        public class GuiTinNhanModel
        {
            public List<int> ids { get; set; }
            public string noiDung { get; set; }
        }

        [HttpPost]
        [Route("guitinnhan")]
        public HttpResponseMessage GuiTinNhan([FromBody] GuiTinNhanModel model)
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

                    int i = 0;
                    if (model.ids.Count > 0)
                    {
                        DateTime dtGui = DateTime.Now;
                        foreach (int id in model.ids)
                        {
                            //NhanVien nv = NhanVien_dl.ChiTietNhanVienTheoIDNV(id);
                            //if (nv != null && nv.IDNV > 0)
                            //{


                            TinNhanOBJ tn = new TinNhanOBJ();
                            tn.ID_QLLH = userinfo.ID_QLLH;
                            tn.ID_NHANVIEN = id;
                            tn.ID_QUANLY = userinfo.ID_QuanLy;
                            tn.NoiDung = model.noiDung;
                            tn.NgayGui = dtGui;
                            TinNhanDB.ThemTinNhan(tn);
                            i++;
                            try
                            {
                                string url = Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + "/AppPush.aspx?idnhanvien=" + id + "&type=tinnhan&message=Bạn có tin nhắn mới, vui lòng kiểm tra.";
                                String r = new System.Net.WebClient().DownloadString(url);

                            }
                            catch (Exception ex)
                            {
                                LSPos_Data.Utilities.Log.Error(ex);
                            }

                        }
                        if (i > 0)
                        {

                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_guitinnhanthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_khongguiduoctinnhan" });
                        }
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_vuilongchonnhanvien" });
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
