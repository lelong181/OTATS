
using LSPosMVC.Common;
using LSPos_Data.Data;
using LSPosMVC.Hubs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/noti")]
    [EnableCors(origins: "*", "*", "*")]
    public class NotifycationsController : ApiController
    {
        #region getnewfeed
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("canhbaonewfeed")]
        [EnableCors(origins: "*", "*", "*")]
        public HttpResponseMessage getnewfeed([FromUri] string type)
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
                    NotifyData _messageRepository = new NotifyData();
                    _messageRepository.GetAllHoatDong(userinfo.ID_QLLH);
                    System.Threading.Thread.Sleep(2000);
                    _messageRepository.GetAllChupAnh(userinfo.ID_QLLH);
                    System.Threading.Thread.Sleep(2000);
                    _messageRepository.GetAllDangNhap(userinfo.ID_QLLH);
                    RsCanhBaoNewFeed OBJ = new RsCanhBaoNewFeed();
                    OBJ.status = false;
                    OBJ.msg = "false";
                    OBJ.data = null;
                    OBJ.notify = false;
                    OBJ.notify_message = "";

                    switch (type)
                    {
                        case "newfeed":

                            BaoCaoCommon baocao = new BaoCaoCommon();
                            string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                            DataTable dt = baocao.BaoCaoLichSuThaoTac(userinfo.ID_QLLH, userinfo.ID_QuanLy, 0, 0, DateTime.Now.Date, DateTime.Now.Date.AddDays(1).AddTicks(-1), 0);

                            if (dt.Rows.Count > 0)
                            {
                                List<newfeed> data = new List<newfeed>();

                                if (lang == "vi")
                                {
                                    foreach (System.Data.DataRow dr in dt.Rows)
                                    {
                                        newfeed nf = new newfeed();
                                        nf.id = (dr["id"] != DBNull.Value) ? int.Parse(dr["id"].ToString()) : 0;
                                        nf.loai = (dr["loai"] != DBNull.Value) ? int.Parse(dr["loai"].ToString()) : 0;
                                        nf.idkehoach = (dr["idkehoach"] != DBNull.Value) ? int.Parse(dr["idkehoach"].ToString()) : 0;
                                        nf.idkhachhang = (dr["idkhachhang"] != DBNull.Value) ? int.Parse(dr["idkhachhang"].ToString()) : 0;
                                        nf.idnhanvien = (dr["idnhanvien"] != DBNull.Value) ? int.Parse(dr["idnhanvien"].ToString()) : 0;
                                        nf.soluonganh = (dr["soluonganh"] != DBNull.Value) ? int.Parse(dr["soluonganh"].ToString()) : 0;
                                        nf.TongTien = (dr["TongTien"] != DBNull.Value) ? double.Parse(dr["TongTien"].ToString()) : 0;

                                        nf.tenkhachhang = (dr["TenKhachHang"] != DBNull.Value) ? dr["TenKhachHang"].ToString() : "";
                                        nf.diachi = (dr["diachi"] != DBNull.Value) ? dr["diachi"].ToString() : "";
                                        nf.tennhanvien = (dr["TenNhanVien"] != DBNull.Value) ? dr["TenNhanVien"].ToString() : "";
                                        nf.tenloai = (dr["tenloai"] != DBNull.Value) ? dr["tenloai"].ToString() : "";
                                        nf.anhdaidien = (dr["AnhDaiDien"] != DBNull.Value) ? Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + dr["AnhDaiDien"] : "assets/img/noimage.png";
                                        nf.anhdaidien_small = (dr["AnhDaiDien_thumbnail_small"] != DBNull.Value) ? Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + dr["AnhDaiDien_thumbnail_small"] : "assets/img/noimage.png";

                                        nf.thoigian = Convert.ToDateTime(dr["thoigian"].ToString() != "" ? dr["thoigian"] : "1900-1-1");
                                        nf.thoigiandukien = Convert.ToDateTime(dr["thoigiandukien"].ToString() != "" ? dr["thoigiandukien"] : "1900-1-1");


                                        string hienthi = "";
                                        TimeSpan ts = DateTime.Now - nf.thoigian;
                                        if (nf.thoigian.Year > 1900 && ts.TotalSeconds < 60)
                                        {
                                            //< 1 phut
                                            hienthi = ts.Seconds + " giây trước (" + nf.thoigian.ToString("HH:mm:ss") + ")";
                                        }
                                        else if (nf.thoigian.Year > 1900 && ts.TotalSeconds < 60 * 60)
                                        {
                                            //< 1 tieng
                                            hienthi = ts.Minutes + " phút trước (" + nf.thoigian.ToString("HH: mm: ss") + ")";
                                        }
                                        else if (nf.thoigian.Year > 1900 && ts.TotalSeconds < 60 * 60 * 24)
                                        {
                                            hienthi = "Khoảng " + Math.Round(ts.TotalHours, 0) + " giờ trước (" + nf.thoigian.ToString("HH: mm: ss") + ")";
                                            //< 1 ngay
                                        }
                                        else
                                        {
                                            hienthi = nf.thoigian.Day + " tháng " + nf.thoigian.Month + " lúc " + nf.thoigian.ToString("HH:mm");
                                        }
                                        nf.thoigian_hienthi = hienthi;
                                        data.Add(nf);

                                    }
                                }
                                else
                                {
                                    foreach (System.Data.DataRow dr in dt.Rows)
                                    {
                                        newfeed nf = new newfeed();
                                        nf.id = (dr["id"] != DBNull.Value) ? int.Parse(dr["id"].ToString()) : 0;
                                        nf.loai = (dr["loai"] != DBNull.Value) ? int.Parse(dr["loai"].ToString()) : 0;
                                        nf.idkehoach = (dr["idkehoach"] != DBNull.Value) ? int.Parse(dr["idkehoach"].ToString()) : 0;
                                        nf.idkhachhang = (dr["idkhachhang"] != DBNull.Value) ? int.Parse(dr["idkhachhang"].ToString()) : 0;
                                        nf.idnhanvien = (dr["idnhanvien"] != DBNull.Value) ? int.Parse(dr["idnhanvien"].ToString()) : 0;
                                        nf.soluonganh = (dr["soluonganh"] != DBNull.Value) ? int.Parse(dr["soluonganh"].ToString()) : 0;
                                        nf.TongTien = (dr["TongTien"] != DBNull.Value) ? double.Parse(dr["TongTien"].ToString()) : 0;

                                        nf.tenkhachhang = (dr["TenKhachHang"] != DBNull.Value) ? dr["TenKhachHang"].ToString() : "";
                                        nf.diachi = (dr["diachi"] != DBNull.Value) ? dr["diachi"].ToString() : "";
                                        nf.tennhanvien = (dr["TenNhanVien"] != DBNull.Value) ? dr["TenNhanVien"].ToString() : "";
                                        nf.tenloai = (dr["tenloai"] != DBNull.Value) ? dr["tenloai"].ToString() : "";
                                        nf.anhdaidien = (dr["AnhDaiDien"] != DBNull.Value) ? Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + dr["AnhDaiDien"] : "assets/img/noimage.png";
                                        nf.anhdaidien_small = (dr["AnhDaiDien_thumbnail_small"] != DBNull.Value) ? Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + dr["AnhDaiDien_thumbnail_small"] : "assets/img/noimage.png";

                                        nf.thoigian = Convert.ToDateTime(dr["thoigian"].ToString() != "" ? dr["thoigian"] : "1900-1-1");
                                        nf.thoigiandukien = Convert.ToDateTime(dr["thoigiandukien"].ToString() != "" ? dr["thoigiandukien"] : "1900-1-1");


                                        string hienthi = "";
                                        TimeSpan ts = DateTime.Now - nf.thoigian;
                                        if (nf.thoigian.Year > 1900 && ts.TotalSeconds < 60)
                                        {
                                            //< 1 phut
                                            hienthi = ts.Seconds + " seconds ago (" + nf.thoigian.ToString("HH:mm:ss") + ")";
                                        }
                                        else if (nf.thoigian.Year > 1900 && ts.TotalSeconds < 60 * 60)
                                        {
                                            //< 1 tieng
                                            hienthi = ts.Minutes + " minutes ago (" + nf.thoigian.ToString("HH: mm: ss") + ")";
                                        }
                                        else if (nf.thoigian.Year > 1900 && ts.TotalSeconds < 60 * 60 * 24)
                                        {
                                            hienthi = "About " + Math.Round(ts.TotalHours, 0) + " hour ago (" + nf.thoigian.ToString("HH: mm: ss") + ")";
                                            //< 1 ngay
                                        }
                                        else
                                        {
                                            hienthi = nf.thoigian.ToString("dd/MM") + " at " + nf.thoigian.ToString("HH:mm");
                                            //hienthi = nf.thoigian.Day + " tháng " + nf.thoigian.Month + " lúc " + nf.thoigian.ToString("HH:mm");
                                        }
                                        nf.thoigian_hienthi = hienthi;
                                        data.Add(nf);

                                    }
                                }

                                OBJ.data = data;

                            }
                            OBJ.status = dt == null ? false : dt.Rows.Count > 0;
                            OBJ.msg = (dt == null ? false : dt.Rows.Count > 0).ToString();

                            break;

                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, OBJ);
                }

            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        public class RsCanhBaoNewFeed
        {
            public RsCanhBaoNewFeed() { }

            public bool status { get; set; }

            public string msg { get; set; }
            public List<newfeed> data { get; set; }
            public bool notify { get; set; }
            public int id { get; set; }
            public string notify_message { get; set; }

        }
        public class newfeed
        {
            public newfeed() { }

            public int id { get; set; }
            public int idnhanvien { get; set; }
            public DateTime thoigian { get; set; }
            public string thoigian_hienthi { get; set; }
            public string tennhanvien { get; set; }
            public string tenkhachhang { get; set; }
            public string diachi { get; set; }
            public string anhdaidien { get; set; }
            public string anhdaidien_small { get; set; }
            public int idkhachhang { get; set; }
            public int loai { get; set; }
            public double TongTien { get; set; }
            public string tenloai { get; set; }
            public int idkehoach { get; set; }
            public DateTime thoigiandukien { get; set; }
            public int soluonganh { get; set; }


        }

        #endregion

        #region canhbaocheckin
        [HttpGet]
        [Route("canhbaocheckin")]
        [EnableCors(origins: "*", "*", "*")]
        public HttpResponseMessage canhbaocheckin([FromUri] string type)
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
                    RsCanhBaoCheckIn OBJ = new RsCanhBaoCheckIn();
                    OBJ.status = false;
                    OBJ.msg = "false";
                    OBJ.data = null;
                    OBJ.notify = false;
                    OBJ.notify_message = "";

                    switch (type)
                    {
                        case "checkin":

                            List<CheckIn> listDH = KiemTraCheckInMoi(userinfo);
                            //if (listDH.Count > 0)
                            //{
                            //    if (Session["LastNotification_CheckIn"] == null)
                            //    {
                            //        Session["LastNotification_CheckIn"] = listDH[0];
                            //        OBJ.notify = false;
                            //        OBJ.notify_message = "Tổng số check in : " + listDH.Count;
                            //    }
                            //    else
                            //    {
                            //        CheckIn dhcu = Session["LastNotification_CheckIn"] as CheckIn;
                            //        if (dhcu != null)
                            //        {
                            //            if (listDH[0].ThoiGian > dhcu.ThoiGian)
                            //            {
                            //                Session["LastNotification_CheckIn"] = listDH[0];
                            //                OBJ.notify = true;
                            //                OBJ.id = listDH[0].ID_CheckIn;
                            //                if (listDH[0].ID_KeHoachDiChuyen > 0)
                            //                {
                            //                    if (listDH[0].ID_CheckIn > 0)
                            //                    {
                            //                        TimeSpan ts = listDH[0].CheckInTime - listDH[0].ThoiGianCheckInDuKien;
                            //                        if (ts.TotalMinutes < 0)
                            //                        {
                            //                            //som
                            //                            double x = -(ts.TotalMinutes);

                            //                            OBJ.notify_message = "Nhân viên: " + listDH[0].TenNhanVien + ", vào điểm sớm hơn kế hoạch " + Math.Round(x, 0) + " phút, khách hàng : " + listDH[0].TenKhachHang + ", Địa chỉ: " + listDH[0].DiaChiKhachHang + "(" + listDH[0].CheckInTime.ToString("HH:mm dd/MM/yyyy") + ")";

                            //                        }
                            //                        else
                            //                        {
                            //                            //qua
                            //                            OBJ.notify_message = "Nhân viên: " + listDH[0].TenNhanVien + ", vào điểm quá thời gian so với kế hoạch " + Math.Round(ts.TotalMinutes, 0) + " phút, khách hàng : " + listDH[0].TenKhachHang + ", Địa chỉ: " + listDH[0].DiaChiKhachHang + "(" + listDH[0].CheckInTime.ToString("HH:mm dd/MM/yyyy") + ")";

                            //                        }
                            //                    }
                            //                    else
                            //                    {
                            //                        OBJ.notify_message = "Nhân viên: " + listDH[0].TenNhanVien + " đến giờ vào điểm, khách hàng : " + listDH[0].TenKhachHang + ", Địa chỉ: " + listDH[0].DiaChiKhachHang + "(" + listDH[0].ThoiGian.ToString("HH:mm dd/MM/yyyy") + ")";
                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    OBJ.notify_message = "Nhân viên: " + listDH[0].TenNhanVien + ", vào điểm không có kế hoạch khách hàng : " + listDH[0].TenKhachHang + ", Địa chỉ: " + listDH[0].DiaChiKhachHang + "(" + listDH[0].CheckInTime.ToString("HH:mm dd/MM/yyyy") + ")";
                            //                }
                            //            }
                            //            else
                            //            {
                            //                OBJ.notify = false;
                            //                OBJ.notify_message = "";
                            //            }
                            //        }
                            //    }
                            //}
                            //Session["SoCheckIn"] = listDH.Count;

                            OBJ.status = listDH == null ? false : listDH.Count > 0;
                            OBJ.msg = (listDH == null ? false : listDH.Count > 0).ToString();
                            OBJ.data = listDH;
                            break;


                        case "sapdengiocheckin":

                            List<CheckIn> listsapdengio = KiemTraCheckInSapDenGio(userinfo);

                            //if (listsapdengio != null && listsapdengio.Count > 0)
                            //{
                            //    if (Session["LastNotification_SapDenGioCheckIn"] == null)
                            //    {

                            //        Session["LastNotification_SapDenGioCheckIn"] = listsapdengio[0];
                            //        OBJ.notify = true;
                            //        OBJ.id = listsapdengio[0].ID_CheckIn;
                            //        if (listsapdengio[0].ID_KeHoachDiChuyen > 0)
                            //        {
                            //            OBJ.notify_message = "Nhân viên: " + listsapdengio[0].TenNhanVien + " sắp đến giờ vào điểm, khách hàng : " + listsapdengio[0].TenKhachHang + ", Địa chỉ: " + listsapdengio[0].DiaChiKhachHang + "(" + listsapdengio[0].ThoiGianCheckInDuKien.ToString("HH:mm dd/MM/yyyy") + ")";
                            //        }

                            //    }
                            //    else
                            //    {
                            //        CheckIn dhcu = Session["LastNotification_SapDenGioCheckIn"] as CheckIn;
                            //        if (dhcu != null)
                            //        {
                            //            if (listsapdengio[0].ID_KeHoachDiChuyen > dhcu.ID_KeHoachDiChuyen)
                            //            {
                            //                Session["LastNotification_SapDenGioCheckIn"] = listsapdengio[0];
                            //                OBJ.notify = true;
                            //                OBJ.id = listsapdengio[0].ID_CheckIn;
                            //                if (listsapdengio[0].ID_KeHoachDiChuyen > 0)
                            //                {
                            //                    OBJ.notify_message = "Nhân viên: " + listsapdengio[0].TenNhanVien + " sắp đến giờ vào điểm, khách hàng : " + listsapdengio[0].TenKhachHang + ", Địa chỉ: " + listsapdengio[0].DiaChiKhachHang + "(" + listsapdengio[0].ThoiGianCheckInDuKien.ToString("HH:mm dd/MM/yyyy") + ")";
                            //                }

                            //            }
                            //            else
                            //            {
                            //                OBJ.notify = false;
                            //                OBJ.notify_message = "";
                            //            }
                            //        }
                            //    }
                            //    Session["SoSapCheckIn"] = listsapdengio.Count;
                            //}


                            OBJ.status = listsapdengio == null || listsapdengio.Count == 0 ? false : listsapdengio.Count > 0;
                            OBJ.msg = (listsapdengio == null ? false : listsapdengio.Count > 0).ToString();
                            OBJ.data = listsapdengio;
                            break;
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, OBJ);
                }

            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        public List<CheckIn> KiemTraCheckInMoi(UserInfo userinfo)
        {
            try
            {
                CheckInOut_dl c = new CheckInOut_dl();
                return c.GetCheckInTheoIDNV_Noti(userinfo.ID_QLLH, userinfo.ID_QuanLy);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<CheckIn> KiemTraCheckInSapDenGio(UserInfo userinfo)
        {
            try
            {
                CheckInOut_dl c = new CheckInOut_dl();
                return c.GetCheckIn_SapToiGio_TheoIDNV(userinfo.ID_QLLH, userinfo.ID_QuanLy);


            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<CheckIn> KiemTraCheckInDenGio(UserInfo userinfo)
        {
            try
            {
                CheckInOut_dl c = new CheckInOut_dl();
                return c.GetCheckInDenGio_TheoIDNV(userinfo.ID_QLLH, userinfo.ID_QuanLy);


            }
            catch (Exception)
            {
                return null;
            }
        }

        public class RsCanhBaoCheckIn
        {
            public RsCanhBaoCheckIn() { }

            public bool status { get; set; }

            public string msg { get; set; }

            public List<CheckIn> data { get; set; }

            public bool notify { get; set; }
            public int id { get; set; }
            public string notify_message { get; set; }

        }

        #endregion

        #region canhbaodonhangmoi

        private int idqllh = 0;

        [HttpGet]
        [Route("canhbaodonhangmoi")]
        [EnableCors(origins: "*", "*", "*")]
        public HttpResponseMessage canhbaodonhangmoi([FromUri] string type)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();
                idqllh = userinfo.ID_QLLH;
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    NotifyData _messageRepository = new NotifyData();

                    RsCanhBaoDonHang OBJ = new RsCanhBaoDonHang();
                    OBJ.status = false;
                    OBJ.msg = "false";
                    OBJ.data = null;
                    OBJ.notify = false;
                    OBJ.notify_message = "";
                    OBJ.soluongdonhangmoi = 0;
                    switch (type)
                    {
                        case "donhangmoi":
                            List<DonHang> listDH = _messageRepository.GetAllDonHang(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                            int sl = SoLuongDonHangMoi(userinfo);
                            OBJ.status = listDH == null ? false : sl > 0;
                            OBJ.msg = (listDH == null ? false : sl > 0).ToString();
                            OBJ.data = listDH;
                            OBJ.soluongdonhangmoi = sl;
                            break;
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, OBJ);
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
        [Route("canhbaodonhangmoicount")]
        [EnableCors(origins: "*", "*", "*")]
        public HttpResponseMessage canhbaodonhangmoicount()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();
                idqllh = userinfo.ID_QLLH;
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    NotifyData _messageRepository = new NotifyData();

                    RsCanhBaoDonHang OBJ = new RsCanhBaoDonHang();
                    OBJ.status = false;
                    OBJ.msg = "false";
                    OBJ.data = null;
                    OBJ.notify = false;
                    OBJ.notify_message = "";
                    int sl = SoLuongDonHangMoi(userinfo);
                    OBJ.soluongdonhangmoi = sl;

                    response = Request.CreateResponse(HttpStatusCode.OK, OBJ);
                }

            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        public List<DonHang> KiemTraDonHangMoi(UserInfo userinfo)
        {
            try
            {
                DonHangData dh = new DonHangData();
                return dh.GetDSDonHangChuaDoc(userinfo.ID_QLLH, userinfo.ID_QuanLy);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public int SoLuongDonHangMoi(UserInfo userinfo)
        {
            try
            {
                DonHang_dl dh = new DonHang_dl();
                return dh.GetSoLuongDonHangChuaXem(userinfo.ID_QLLH, userinfo.ID_QuanLy);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public class RsCanhBaoDonHang
        {
            public RsCanhBaoDonHang() { }
            public bool notify { get; set; }
            public int id { get; set; }
            public string notify_message { get; set; }
            public bool status { get; set; }
            public string msg { get; set; }
            public List<DonHang> data { get; set; }
            public int soluongdonhangmoi { get; set; }
        }

        #endregion

        #region canhbaotinnhanmoi
        [HttpGet]
        [Route("canhbaotinnhanmoi")]
        [EnableCors(origins: "*", "*", "*")]
        public HttpResponseMessage canhbaotinnhanmoi([FromUri] string type)
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
                    NotifyData _messageRepository = new NotifyData();
                    _messageRepository.GetAllTinNhan(userinfo.ID_QLLH);
                    RsCanhBaoTinNhan OBJ = new RsCanhBaoTinNhan();
                    OBJ.status = false;
                    OBJ.msg = "false";
                    OBJ.data = null;

                    switch (type)
                    {
                        case "tinnhanmoi":
                            List<TinNhanOBJ> listDH = KiemTraTinNhanMoi(userinfo);
                            //Session["NoNo"] = listDH.Count;

                            OBJ.status = listDH == null ? false : listDH.Count > 0;
                            OBJ.msg = listDH.Count == 0 ? "" : listDH[0].TenNhanVien + ": " + listDH[0].NoiDung;
                            OBJ.data = listDH;
                            break;
                    }
                    //System.Data.DataTable listNhanVien = GetDanhSachNhanVien(userinfo);
                    //if (listNhanVien != null && listNhanVien.Rows.Count > 0)
                    //{
                    //    if (Session["LastNotification_Online"] == null)
                    //    {
                    //        Session["LastNotification_Online"] = listNhanVien.Rows[0];
                    //        OBJ.notify_onoff = true;

                    //    }
                    //    else
                    //    {
                    //        System.Data.DataRow dhcu = Session["LastNotification_Online"] as System.Data.DataRow;
                    //        if (dhcu != null)
                    //        {
                    //            if (int.Parse(listNhanVien.Rows[0]["id"].ToString()) > int.Parse(dhcu["id"].ToString()))
                    //            {
                    //                Session["LastNotification_Online"] = listNhanVien.Rows[0];
                    //                OBJ.notify_onoff = true;
                    //                if (listNhanVien.Rows[0]["loai"].ToString() == "0")//dang nhap
                    //                {
                    //                    OBJ.notify_onoff_trangthai = 0;
                    //                    OBJ.notify_message_onoff = "Nhân viên : " + listNhanVien.Rows[0]["TenNhanVien"] + ", đăng nhập ( thời gian : " + Convert.ToDateTime(listNhanVien.Rows[0]["thoigiandangnhap"]) + " )";
                    //                }
                    //                else
                    //                {
                    //                    OBJ.notify_onoff_trangthai = 1;
                    //                    //dang xuat
                    //                    OBJ.notify_message_onoff = "Nhân viên : " + listNhanVien.Rows[0]["TenNhanVien"] + ", đăng xuất ( thời gian : " + Convert.ToDateTime(listNhanVien.Rows[0]["thoigiandangnhap"]) + ")";
                    //                }


                    //            }
                    //            else
                    //            {
                    //                OBJ.notify_onoff_trangthai = 0;
                    //                OBJ.notify_onoff = false;
                    //                OBJ.notify_message_onoff = "";
                    //            }
                    //        }

                    //    }
                    //}

                    response = Request.CreateResponse(HttpStatusCode.OK, OBJ);
                }

            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }
        public List<TinNhanOBJ> KiemTraTinNhanMoi(UserInfo userinfo)
        {
            try
            {
                return TinNhanDB.getDSTinNhanTheoID_ChuaDoc(userinfo.ID_QLLH, userinfo.ID_QuanLy);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public System.Data.DataTable GetDanhSachNhanVien(UserInfo userinfo)
        {
            try
            {
                System.Data.DataSet dsnv = NhanVien_dl.LichSuOnlineOffine(userinfo.ID_QLLH, 0, DateTime.Now.Date, DateTime.Now.AddDays(1).Date, userinfo.ID_QuanLy);
                return dsnv.Tables[0];
            }
            catch (Exception)
            {
                return null;
            }
        }
        public class RsCanhBaoTinNhan
        {
            public RsCanhBaoTinNhan() { }

            public bool status { get; set; }

            public string msg { get; set; }
            public bool notify_onoff { get; set; }
            public int notify_onoff_trangthai { get; set; }
            public string notify_message_onoff { get; set; }
            public List<TinNhanOBJ> data { get; set; }
        }
        #endregion

        [HttpGet]
        [Route("canhbaohoatdong")]
        [EnableCors(origins: "*", "*", "*")]
        public HttpResponseMessage CanhBaoHoatDong([FromUri] string type)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    NotifyData _messageRepository = new NotifyData();
                    _messageRepository.GetAllHoatDong(userinfo.ID_QLLH);
                    response = Request.CreateResponse(HttpStatusCode.Created, new { success = true });
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        [HttpGet]
        [Route("canhbaodonhang")]
        [EnableCors(origins: "*", "*", "*")]
        public HttpResponseMessage CanhBaoDonHang()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    NotifyData _messageRepository = new NotifyData();
                    //_messageRepository.StartNoty(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    response = Request.CreateResponse(HttpStatusCode.Created, new { success = true });
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        [HttpGet]
        [EnableCors(origins: "*", "*", "*")]
        [Route("canhbaotinnhan")]
        public HttpResponseMessage CanhBaoTinNhan([FromUri] string type)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    NotifyData _messageRepository = new NotifyData();
                    _messageRepository.GetAllTinNhan(userinfo.ID_QLLH);
                    response = Request.CreateResponse(HttpStatusCode.Created, new { success = true });
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        [HttpGet]
        [Route("clearnoti")]
        [EnableCors(origins: "*", "*", "*")]
        public HttpResponseMessage clearnoti([FromUri] int type)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    //type: 1:ClearNotification, 2:ClearMessage
                    if (type == 1)
                    {
                        SqlDataHelper helper = new SqlDataHelper();
                        helper.ExecuteNonQuery("sp_QL_UpdateDonHangDaXem", new SqlParameter("ID_QLLH", userinfo.ID_QLLH));

                        List<DonHang> list = new List<DonHang>();
                        RsCanhBaoDonHang OBJ = new RsCanhBaoDonHang();
                        OBJ.status = true;
                        OBJ.msg = "Xóa thông báo đơn hàng thành công";
                        OBJ.data = list;
                        OBJ.notify = true;
                        OBJ.notify_message = "";
                        OBJ.soluongdonhangmoi = 0;
                        return response = Request.CreateResponse(HttpStatusCode.OK, OBJ);
                    }
                    else if (type == 2)
                    {
                        SqlDataHelper helper = new SqlDataHelper();
                        SqlParameter[] par = new SqlParameter[] {
                            new SqlParameter("ID_QLLH", userinfo.ID_QLLH),
                            new SqlParameter("ID_QuanLy", userinfo.ID_QuanLy)
                            };

                        helper.ExecuteNonQuery("sp_QL_UpdateTinNhanDaXem", par);

                        List<TinNhanOBJ> list = new List<TinNhanOBJ>();
                        RsCanhBaoTinNhan OBJ = new RsCanhBaoTinNhan();
                        OBJ.status = true;
                        OBJ.msg = "Xóa thông báo đơn hàng thành công";
                        OBJ.data = list;

                        return response = Request.CreateResponse(HttpStatusCode.OK, OBJ);
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
