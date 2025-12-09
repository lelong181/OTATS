using LSPosMVC.Common;
using LSPos_Data.Data;
using LSPos_Data.Models;
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
    [RoutePrefix("api/kpi")]
    public class ChiTieuKPIController : ApiController
    {
        [HttpGet]
        [Route("getbyidnhom")]
        public HttpResponseMessage get([FromUri] int idnhom)
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
                    List<Results> list = new List<Results>();

                    ChiTieuKPIData chitieu = new ChiTieuKPIData();
                    DataTable data = chitieu.GetChiTieuTheoNhanVienKPI(userinfo.ID_QLLH, idnhom, userinfo.ID_QuanLy);
                    foreach (DataRow r in data.Rows)
                    {
                        Results results = new Results();
                        results.ID_ChiTieuKPI = Convert.ToInt32(r["ID_ChiTieuKPI"].ToString());
                        results.ID_NhanVien = Convert.ToInt32(r["ID_NhanVien"].ToString());
                        results.ApDung_TuNgay = Convert.ToDateTime(r["ApDung_TuNgay"].ToString());
                        results.ID_NhanVien = Convert.ToInt32(r["ID_NhanVien"].ToString());
                        results.TenDangNhap = r["TenDangNhap"].ToString();
                        results.TenNhanVien = r["TenNhanVien"].ToString();
                        results.DoanhSo = Convert.ToDouble(r["DoanhSo"].ToString());
                        results.SoDonHang = Convert.ToInt32(r["SoDonHang"].ToString());
                        results.NgayCong = Convert.ToInt32(r["NgayCong"].ToString());
                        results.LuotViengTham = Convert.ToInt32(r["LuotViengTham"].ToString());
                        results.TenTrangThai = r["TenTrangThai"].ToString();
                        results.TrangThai = Convert.ToInt32(r["TrangThai"].ToString());
                        results.NgayTaoKPI = Convert.ToDateTime(r["NgayTaoKPI"].ToString());

                        list.Add(results);
                    }

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

        [HttpPost]
        [Route("addbynhom")]
        public HttpResponseMessage addbynhom([FromBody] ChiTieuKPIByNhomModels ct)
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
                    bool validate = true;
                    DateTime ngay = new DateTime(1900, 1, 1);
                    if (ct.ID_Nhom <= 0 && validate)
                    {
                        validate = false;
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_khongthedetrongnhomnhanvien" });
                    }
                    if (ct.ApDung_TuNgay <= ngay && validate)
                    {
                        validate = false;
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_khongthedetrongthangapdung" });
                    }
                    if (ct.ApDung_TuNgay.Month < DateTime.Now.Month && ct.ApDung_TuNgay.Year <= DateTime.Now.Year && validate)
                    {
                        validate = false;
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_khongtheapdungchitieuthangdaqua" });
                    }

                    if (validate)
                    {
                        ct.IDQLLH = userinfo.ID_QLLH;
                        ct.ApDung_TuNgay = new DateTime(ct.ApDung_TuNgay.Year, ct.ApDung_TuNgay.Month, 1);

                        ChiTieu_KPI_dl chiTieu_KPI = new ChiTieu_KPI_dl();
                        int count_nv = 0;
                        int count_sucsess = 0;
                        int count_fail = 0;

                        NhanVienApp nv_dl = new NhanVienApp();
                        List<NhanVien> dsnv = nv_dl.GetDataNhanVien_TheoNhomQuanLy(userinfo.ID_QLLH, ct.ID_Nhom);
                        foreach (NhanVien nv in dsnv)
                        {
                            count_nv += 1;

                            ChiTieuKPIOBJ obj = new ChiTieuKPIOBJ();
                            obj.IDQLLH = userinfo.ID_QLLH;
                            obj.ApDung_TuNgay = ct.ApDung_TuNgay;
                            obj.ID_ChiTieuKPI = ct.ID_ChiTieuKPI;
                            obj.ID_NhanVien = nv.IDNV;
                            obj.DoanhSo = ct.DoanhSo;
                            obj.LuotViengTham = ct.LuotViengTham;
                            obj.NgayCong = ct.NgayCong;
                            obj.SoDonHang = ct.SoDonHang;
                            obj.TrangThai = ct.TrangThai;

                            if (chiTieu_KPI.Them(obj))
                                count_sucsess += 1;
                            else
                                count_fail += 1;
                        }

                        if (count_nv > 0)
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luuchitieuthanhcong" });
                        else
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_khongconhanviendephanchitieu" });
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
        [Route("add")]
        public HttpResponseMessage add([FromBody] ChiTieuKPIOBJ ct)
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
                    bool validate = true;
                    DateTime ngay = new DateTime(1900, 1, 1);
                    if (ct.ID_NhanVien <= 0 && validate)
                    {
                        validate = false;
                        response = Request.CreateResponse(HttpStatusCode.NotModified, "label_khongthedetrongnhanvien");
                    }
                    if (ct.ApDung_TuNgay <= ngay && validate)
                    {
                        validate = false;
                        response = Request.CreateResponse(HttpStatusCode.NotModified, "label_khongthedetrongthangapdung");
                    }
                    if (ct.ApDung_TuNgay.Month < DateTime.Now.Month && ct.ApDung_TuNgay.Year <= DateTime.Now.Year && validate)
                    {
                        validate = false;
                        response = Request.CreateResponse(HttpStatusCode.NotModified, "label_khongtheapdungchitieuthangdaqua");
                    }

                    if (validate)
                    {
                        ct.IDQLLH = userinfo.ID_QLLH;
                        ct.ApDung_TuNgay = new DateTime(ct.ApDung_TuNgay.Year, ct.ApDung_TuNgay.Month, 1);

                        ChiTieu_KPI_dl chiTieu_KPI = new ChiTieu_KPI_dl();
                        if (chiTieu_KPI.Them(ct))
                            response = Request.CreateResponse(HttpStatusCode.OK, "label_luuchitieuthanhcong");
                        else
                            response = Request.CreateResponse(HttpStatusCode.NotModified, "label_luuchitieukhongthanhcong");
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
        [Route("update")]
        public HttpResponseMessage update([FromBody] ChiTieuKPIOBJ ct)
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
                    bool validate = true;
                    DateTime ngay = new DateTime(1900, 1, 1);
                    if (ct.ID_NhanVien <= 0 && validate)
                    {
                        validate = false;
                        response = Request.CreateResponse(HttpStatusCode.NotModified, "label_khongthedetrongnhanvien");
                    }
                    if (ct.ApDung_TuNgay <= ngay && validate)
                    {
                        validate = false;
                        response = Request.CreateResponse(HttpStatusCode.NotModified, "label_khongthedetrongthangapdung");
                    }
                    if (ct.ApDung_TuNgay.Month < DateTime.Now.Month && ct.ApDung_TuNgay.Year <= DateTime.Now.Year && validate)
                    {
                        validate = false;
                        response = Request.CreateResponse(HttpStatusCode.NotModified, "label_khongtheapdungchitieuthangdaqua");
                    }

                    if (validate)
                    {
                        ct.IDQLLH = userinfo.ID_QLLH;
                        ct.ApDung_TuNgay = new DateTime(ct.ApDung_TuNgay.Year, ct.ApDung_TuNgay.Month, 1);

                        ChiTieu_KPI_dl chiTieu_KPI = new ChiTieu_KPI_dl();
                        if (chiTieu_KPI.CapNhat(ct))
                            response = Request.CreateResponse(HttpStatusCode.OK, "label_luuchitieuthanhcong");
                        else
                            response = Request.CreateResponse(HttpStatusCode.NotModified, "label_luuchitieukhongthanhcong");
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
        [Route("delete")]
        public HttpResponseMessage delete([FromBody] int id)
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
                    ChiTieu_KPI_dl chiTieu_KPI = new ChiTieu_KPI_dl();
                    if (chiTieu_KPI.Delete(id))
                        response = Request.CreateResponse(HttpStatusCode.OK, "label_xoachitieuthanhcong");
                    else
                        response = Request.CreateResponse(HttpStatusCode.NotModified, "label_xoachitieukhongthanhcong");
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
        [Route("deletemulti")]
        public HttpResponseMessage deleteMulti([FromUri] string id)
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
                    ChiTieuKPIData chiTieu = new ChiTieuKPIData();
                    if (chiTieu.Delete(id))
                        response = Request.CreateResponse(HttpStatusCode.OK, "label_xoachitieuthanhcong");
                    else
                        response = Request.CreateResponse(HttpStatusCode.NotModified, "label_xoachitieukhongthanhcong");
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        public class Results
        {
            public int ID_ChiTieuKPI { set; get; }
            public DateTime ApDung_TuNgay { set; get; }
            public int ID_NhanVien { set; get; }
            public string TenDangNhap { set; get; }
            public string TenNhanVien { set; get; }
            public double DoanhSo { set; get; }
            public int SoDonHang { set; get; }
            public int NgayCong { set; get; }
            public int LuotViengTham { set; get; }
            public string TenTrangThai { set; get; }
            public int TrangThai { set; get; }
            public DateTime NgayTaoKPI { set; get; }

        }
    }
}
