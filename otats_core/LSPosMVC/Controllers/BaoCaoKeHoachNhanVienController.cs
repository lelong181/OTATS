using LSPosMVC.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Aspose.Cells;
using System.Reflection;
using System.Configuration;
using System.Web.UI;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using LSPos_Data.Models;
using LSPos_Data.Data;
using LSPosMVC.Models.Models_Filter;

namespace LSPosMVC.Controllers
{
    /// <summary>
    /// Dongnn update 2019-07-11
    /// </summary>

    [Authorize]
    [RoutePrefix("api/baocaokehoachnhanvien")]
    public class BaoCaoKeHoachNhanVienController : ApiController
    {

        #region Báo cáo kế hoạch nhân viên

        [HttpGet]
        [Route("getallKendo")]
        public DataSourceResult GetOrders(HttpRequestMessage requestMessage)
        {
            DataSourceResult s = new DataSourceResult();
            try
            {

                RequestGridParam param = JsonConvert.DeserializeObject<RequestGridParam>(
                // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
                requestMessage.RequestUri.ParseQueryString().GetKey(0));
                //get token string from Headers Request
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];

                //decode token string
                var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                string username = token.Claims.First(c => c.Type == "Username").Value;
                string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;

                User_dl userDL = new User_dl();

                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);
                KhachHang_dl kh_dl = new KhachHang_dl();
                KhachHangData khd = new KhachHangData();

                //DataTable dskh = kh_dl.GetDataKhachHangAll(userinfo.ID_QLLH, userinfo.ID_QuanLy, param.tieuchiloc.IdTinh, param.tieuchiloc.IdQuan, param.tieuchiloc.IdLoaiKhachHang, 0);
                //dskh.Rows.RemoveAt(0);
                List<KhachHangDTO> lstKhachHang = new List<KhachHangDTO>();
                FilterGrid filter = new FilterGrid();
                int tongso = 0;
                if (param.request.Filter != null)
                {
                    foreach (Filter f in param.request.Filter.Filters)
                    {
                        switch (f.Field)
                        {
                            case "tenKhachHang":
                                filter.TenKhachHang = f.Value.ToString(); ;
                                break;
                        }
                    }
                }
                KeHoachDiChuyen_dl khdc_dl = new KeHoachDiChuyen_dl();
                List<KeHoachDiChuyenObj> dskhdc = khdc_dl.GetKeHoachTheoNhanVien_Moi(param.tieuchiloc.ID_NhanVien, param.tieuchiloc.TuNgay, param.tieuchiloc.DenNgay, userinfo.ID_QLLH, userinfo.ID_QuanLy);
                s.Data = lstKhachHang;
                s.Total = tongso;
                s.Aggregates = null;

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return s;
        }

        [HttpPost]
        [Route("getdatabaocao")]
        public HttpResponseMessage GetDataBaoCao([FromBody] TieuChiLoc param)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];

                var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                string username = token.Claims.First(c => c.Type == "Username").Value;
                string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;

                User_dl userDL = new User_dl();
                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    BaoCaoCommon bc = new BaoCaoCommon();
                    DataTable dskhdc = bc.GetKeHoachTheoNhanVien_Moi(param.ID_NhanVien, param.TuNgay, param.DenNgay, userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    response = Request.CreateResponse(HttpStatusCode.OK, dskhdc);
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
        [Route("xoakehoach")]
        public HttpResponseMessage XoaKeHoach([FromBody] List<int> Ids)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];

                var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                string username = token.Claims.First(c => c.Type == "Username").Value;
                string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;

                User_dl userDL = new User_dl();
                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    int i = 0;
                    KeHoachDiChuyen_dl khdc_dl = new KeHoachDiChuyen_dl();
                    foreach (int id in Ids)
                    {
                        List<KeHoachDiChuyenObj> k = khdc_dl.GetKeHoachById(id);
                        foreach (KeHoachDiChuyenObj kh in k)
                        {
                            if (DateTime.Compare(kh.ThoiGianCheckInDuKien, DateTime.Now) > 0)
                            {
                                ;
                                if (khdc_dl.DeleteKeHoach(kh) > 0)
                                {
                                    i++;
                                };
                                //if (khdc_dl.XoaKeHoach(kh.IDNhanVien, userinfo.ID_QLLH, kh.ThoiGianCheckInDuKien) > 0) {
                                //    i++;
                                //};
                            }
                        }
                    }
                    if (i == 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, count = i });

                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, count = i });

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
        [Route("xoakehoach_v1")]
        public HttpResponseMessage XoaKeHoach_v1([FromBody] List<int> Ids)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];

                var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                string username = token.Claims.First(c => c.Type == "Username").Value;
                string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;

                User_dl userDL = new User_dl();
                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    int i = 0;
                    TuyenKhachHangData tuyenKhachHangData = new TuyenKhachHangData();
                    KeHoachDiChuyen_dl khdc_dl = new KeHoachDiChuyen_dl();
                    foreach (int id in Ids)
                    {
                        List<KeHoachDiChuyenObj> k = khdc_dl.GetKeHoachById(id);
                        foreach (KeHoachDiChuyenObj kh in k)
                        {
                            if (DateTime.Compare(kh.ThoiGianCheckInDuKien, DateTime.Now) > 0)
                            {
                                tuyenKhachHangData.XoaKeHoachDiChuyen(userinfo.ID_QuanLy, id);
                                i++;

                                try
                                {
                                    if (kh.ThoiGianCheckInDuKien.DayOfYear == DateTime.Now.DayOfYear)
                                    {
                                        string mess_push = "Kế hoạch thay đổi " + kh.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + ", vui lòng vào mục kế hoạch để kiểm tra";
                                        String res = new System.Net.WebClient().DownloadString(Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"])
                                            + "/AppPush.aspx?type=kehoach&idnhanvien=" + kh.IDNhanVien + "&ngay=" + kh.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + "&message=" + mess_push);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }
                            }
                        }
                    }
                    if (i == 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, count = i });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, count = i });
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
        [Route("getdatagridthemnhanh")]
        public HttpResponseMessage GetDataGridThemNhanh([FromBody] TieuChiLoc param)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];

                var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                string username = token.Claims.First(c => c.Type == "Username").Value;
                string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;

                User_dl userDL = new User_dl();
                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    KeHoachDiChuyen_dl khdc_dl = new KeHoachDiChuyen_dl();
                    DataTable dt = khdc_dl.LayDanhSachKeHoach_TheoNhanVien(param.ID_NhanVien, param.TuNgay, 0, 0);
                    List<KeHoachModel> result = new List<KeHoachModel>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        KeHoachModel i = new KeHoachModel();
                        i.IDKeHoach = (dr["ID"] != null) ? int.Parse(dr["ID"].ToString()) : 0;
                        i.IDKhachHang = (dr["ID_KhachHang"] != null) ? int.Parse(dr["ID_KhachHang"].ToString()) : 0;
                        i.TenKhachHang = (dr["TenKhachHang"] != null) ? dr["TenKhachHang"].ToString() : "";
                        i.IDNhanVien = param.ID_NhanVien;
                        i.DiaChi = (dr["DiaChi"] != null) ? dr["DiaChi"].ToString() : "";
                        i.GhiChu = (dr["GhiChu"] != null) ? dr["GhiChu"].ToString() : "";
                        i.TenTrangThai = (dr["TenTrangThai"] != null) ? dr["TenTrangThai"].ToString() : "";
                        i.ThoiGianCheckInDuKien = dr["ThoiGianCheckInDuKien"].ToString() == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianCheckInDuKien"]);
                        i.ThoiGianCheckOutDuKien = dr["ThoiGianCheckOutDuKien"].ToString() == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianCheckOutDuKien"]);
                        i.ThuTuCheckIn = (dr["ThuTuCheckIn"] != null) ? int.Parse(dr["ThuTuCheckIn"].ToString()) : 0;
                        result.Add(i);
                    }
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


        [HttpPost]
        [Route("luukehoachnhanh")]
        public HttpResponseMessage LuuKeHoachNhanh([FromBody] List<KeHoachModel> param)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];

                var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                string username = token.Claims.First(c => c.Type == "Username").Value;
                string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;

                User_dl userDL = new User_dl();
                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {

                    KeHoachDiChuyen_dl khdc_dl = new KeHoachDiChuyen_dl();
                    foreach (KeHoachModel kh in param)
                    {
                        string err = "";
                        if (kh.IDKeHoach > 0)
                        {
                            KeHoachDiChuyenObj item = khdc_dl.GetKeHoachById(kh.IDKeHoach).First();
                            item.ThoiGianCheckInDuKien = kh.ThoiGianCheckInDuKien;
                            item.ThoiGianCheckOutDuKien = kh.ThoiGianCheckOutDuKien;
                            item.GhiChu = kh.GhiChu;
                            if (item.ThoiGianCheckInDuKien > item.ThoiGianCheckOutDuKien)
                            {

                                err = "Thời gian vào điểm dự kiến không thể lớn hơn thời gian ra điểm dự kiến, mời bạn vui lòng nhập lại";
                            }
                            if (item.ThoiGianCheckInDuKien == item.ThoiGianCheckOutDuKien)
                            {

                                err = "Thời gian vào điểm dự kiến không thể bằng thời gian ra điểm dự kiến, mời bạn vui lòng nhập lại";
                            }
                            if (item.ThoiGianCheckInDuKien < DateTime.Now)
                            {

                                err = "Không thể lập kế hoạch cho thời điểm ở quá khứ, mời bạn vui lòng chọn lại.";
                            }
                            if (khdc_dl.SuaKeHoach(item) > 0)
                            {
                                try
                                {
                                    string mess_push = "Bạn có kế hoạch mới ngày " + item.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + ", vui lòng vào mục kế hoạch để kiểm tra";
                                    String res = new System.Net.WebClient().DownloadString(Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + "/AppPush.aspx?idnhanvien=" + item.IDNhanVien + "&ngay=" + item.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + "&message=" + mess_push);
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }

                            }
                        }
                        else
                        {
                            //string err = "";
                            if (kh.IDKhachHang > 0)
                            {
                                KeHoachDiChuyenObj khdc_new = new KeHoachDiChuyenObj();
                                khdc_new.IDKeHoach = 0;
                                khdc_new.IDKhachHang = kh.IDKhachHang;
                                khdc_new.IDNhanVien = kh.IDNhanVien;
                                khdc_new.ThoiGianCheckInDuKien = kh.ThoiGianCheckInDuKien;
                                khdc_new.ThoiGianCheckOutDuKien = kh.ThoiGianCheckOutDuKien;

                                if (khdc_new.ThoiGianCheckInDuKien > khdc_new.ThoiGianCheckOutDuKien)
                                {

                                    err = "Thời gian vào điểm dự kiến không thể lớn hơn thời gian ra điểm dự kiến, mời bạn vui lòng nhập lại";
                                }
                                if (khdc_new.ThoiGianCheckInDuKien == khdc_new.ThoiGianCheckOutDuKien)
                                {

                                    err = "Thời gian vào điểm dự kiến không thể bằng thời gian ra điểm dự kiến, mời bạn vui lòng nhập lại";
                                }
                                if (khdc_new.ThoiGianCheckInDuKien < DateTime.Now)
                                {

                                    err = "Không thể lập kế hoạch cho thời điểm ở quá khứ, mời bạn vui lòng chọn lại.";

                                }
                                khdc_new.GhiChu = kh.GhiChu;
                                khdc_new.ViecCanLam = kh.ViecCanLam;
                                if (DateTime.Compare(khdc_new.ThoiGianCheckInDuKien, DateTime.Now) > 0)
                                {
                                    if (khdc_dl.ThemKeHoach(khdc_new) > 0)
                                    {
                                        err = "Lập kế hoạch thành công";
                                        try
                                        {
                                            string mess_push = "Bạn có kế hoạch mới ngày " + khdc_new.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + ", vui lòng vào mục kế hoạch để kiểm tra";
                                            String res = new System.Net.WebClient().DownloadString(Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + "/AppPush.aspx?idnhanvien=" + khdc_new.IDNhanVien + "&ngay=" + khdc_new.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + "&message=" + mess_push);
                                        }
                                        catch (Exception ex)
                                        {
                                            LSPos_Data.Utilities.Log.Error(ex);

                                        }
                                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = err });

                                    }
                                    else
                                    {
                                        err = "Lập kế hoạch thất bại, vui lòng thử lại";
                                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = err });
                                    }
                                }
                                else
                                {
                                    err = "Lập kế hoạch thất bại, vui lòng kiểm tra ngày";
                                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = err });
                                }
                            }
                            else
                            {
                                KhachHang_dl kh_dl = new KhachHang_dl();
                                DataTable dsKH = kh_dl.GetKhachHangDaCapQuyen(kh.IDNhanVien, -1, 0, 0, 0, 0);
                                foreach (DataRow dr in dsKH.Rows)
                                {
                                    KeHoachDiChuyenObj khdc_new = new KeHoachDiChuyenObj();
                                    khdc_new.IDKeHoach = 0;
                                    khdc_new.IDKhachHang = int.Parse(dr["ID_KhachHang"].ToString());
                                    khdc_new.IDNhanVien = kh.IDNhanVien;
                                    khdc_new.ThoiGianCheckInDuKien = kh.ThoiGianCheckInDuKien;
                                    khdc_new.ThoiGianCheckOutDuKien = kh.ThoiGianCheckOutDuKien;
                                    khdc_new.GhiChu = kh.GhiChu;
                                    khdc_new.ViecCanLam = kh.ViecCanLam;
                                    if (khdc_dl.ThemKeHoach(khdc_new) > 0)
                                    {
                                        err = "Lập kế hoạch thành công";
                                        try
                                        {
                                            string mess_push = "Bạn có kế hoạch mới ngày " + khdc_new.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + ", vui lòng vào mục kế hoạch để kiểm tra";
                                            String res = new System.Net.WebClient().DownloadString(Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + "/AppPush.aspx?idnhanvien=" + khdc_new.IDNhanVien + "&ngay=" + khdc_new.ThoiGianCheckInDuKien.ToString("dd/MM/yyyy") + "&message=" + mess_push);
                                        }
                                        catch (Exception ex)
                                        {
                                            LSPos_Data.Utilities.Log.Error(ex);
                                        }
                                    }
                                    else
                                    {
                                        err = "Lập kế hoạch thất bại, vui lòng thử lại";
                                    }
                                }
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = err });
                            }
                        }
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = true });
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

        #region Tiêu chí lọc + Class
        public class FilterGrid
        {
            public DateTime Ngay { get; set; }
            public string TenNhanVien { get; set; }
            public string TenKhachHang { get; set; }
            public DateTime DuKienVao { get; set; }
            public DateTime DuKienRa { get; set; }
            public DateTime ThucTeVao { get; set; }
            public DateTime ThucTeRa { get; set; }
            public string TrangThai { get; set; }
        }

        public DataSourceRequest request { get; set; }

        private class RequestGridParam
        {
            public DataSourceRequest request { get; set; }
            public TieuChiLoc tieuchiloc { get; set; }
        }
        public class KeHoachModel
        {
            public string DiaChi { get; set; }
            public string DuongPho { get; set; }
            public string GhiChu { get; set; }
            public int IDKeHoach { get; set; }
            public int IDKhachHang { get; set; }
            public int IDNhanVien { get; set; }
            public double KinhDo { get; set; }
            public string TenKhachHang { get; set; }
            public string TenNhanVien { get; set; }
            public string text_color { get; set; }
            public string text_color_mota { get; set; }
            public DateTime ThoiGianCheckInDuKien { get; set; }
            public DateTime ThoiGianCheckInThucTe { get; set; }
            public DateTime ThoiGianCheckOutDuKien { get; set; }
            public DateTime ThoiGianCheckOutThucTe { get; set; }
            public int ThuTuCheckIn { get; set; }
            public int TrangThai { get; set; }
            public double ViDo { get; set; }
            public string ViecCanLam { get; set; }
            public string TenTrangThai { get; set; }
        }
        #endregion
    }
}
