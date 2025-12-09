using LSPosMVC.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using LSPos_Data.Models;
using LSPos_Data.Data;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/thaotacnguoidung")]
    public class LichSuThaoTacNguoiDungController : ApiController
    {
        public class Results
        {
            public int id { set; get; }
            public string thaoTac { set; get; }
            public string noiDung { set; get; }
            public string anhDaiDien { set; get; }
            public string anhDaiDien_thumbnail_small { set; get; }
            public string thaoTacHienThi { set; get; }
            public string noiDungHienThi { set; get; }
            public string nhanVien { set; get; }
            public string thoiGian { set; get; }
            public string diaChi { set; get; }
        }
        [HttpGet]
        [Route("get")]
        public HttpResponseMessage get([FromUri] int idKhachHang, int idNhanVien, int thaoTac, DateTime from, DateTime to)
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

                    BaoCaoCommon baocao = new BaoCaoCommon();
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    DataTable dt = baocao.BaoCaoLichSuThaoTac(userinfo.ID_QLLH, userinfo.ID_QuanLy, idKhachHang, idNhanVien, from, to, thaoTac);

                    if (lang == "vi")
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            Results results = new Results();
                            int loai = Convert.ToInt32(dr["loai"].ToString());
                            switch (loai)
                            {
                                case 1:
                                    results.thaoTacHienThi = "<span class=\"label label-warning box\">" + dr["tenloai"].ToString() + "</span>";
                                    break;
                                case 2:
                                    results.thaoTacHienThi = "<span class=\"label label-danger box\">" + dr["tenloai"].ToString() + "</span>";
                                    break;
                                case 3:
                                    results.thaoTacHienThi = "<a href='' ng-click='openlichsuavaoradiem()'><span class=\"label label-primary box\">" + dr["tenloai"].ToString() + "</span></a>";
                                    break;
                                case 4:
                                    results.thaoTacHienThi = "<a href='' ng-click='openlichsuavaoradiem()'><span class=\"label label-purple box\">" + dr["tenloai"].ToString() + "</span></a>";
                                    break;
                                case 5:
                                    results.thaoTacHienThi = "<a href='' ng-click='openalbumanh()'><span class=\"label label-info box\">" + dr["tenloai"].ToString() + "</span></a>";
                                    break;
                                case 6:
                                    results.thaoTacHienThi = "<a href='' ng-click='openchitietdonhang()'><span  class=\"label label-yellow box\">" + dr["tenloai"].ToString() + "</span></a>";
                                    break;
                                default:
                                    results.thaoTacHienThi = "";
                                    break;
                            }

                            string noiDungHienThi = "";
                            string noiDung = "";
                            noiDungHienThi += "Nhân viên: <strong>" + dr["tennhanvien"].ToString() + "</strong><br />";
                            noiDung += "Nhân viên: " + dr["tennhanvien"].ToString();

                            if (loai != 1 && loai != 2)
                            {
                                noiDungHienThi += " Khách hàng: <strong>" + dr["tenkhachhang"].ToString() + "</strong> <br />";
                                noiDung += " Khách hàng: " + dr["tenkhachhang"].ToString();

                                noiDungHienThi += " Địa chỉ : <strong>" + dr["DiaChi"].ToString() + "</strong> <br />";
                                noiDung += " Địa chỉ: " + dr["DiaChi"].ToString();
                            }

                            if (loai == 5)
                            {
                                noiDungHienThi += " Số lượng ảnh: <strong><a href='' ng-click='openalbumanh()'>" + dr["soluonganh"].ToString() + " </a></strong> <br />";
                                noiDung += " Số lượng ảnh: " + dr["soluonganh"].ToString();
                            }

                            if (loai == 6)
                            {
                                string format = userinfo.DinhDangTienSoThapPhan == 1 ? "n2" : "n0";
                                double tt = Convert.ToDouble(dr["TongTien"].ToString());
                                noiDungHienThi += " Giá trị: <strong><a href='' ng-click='openchitietdonhang()'>" + tt.ToString(format) + "</a></strong> <br />";
                                noiDung += " Giá trị: " + tt.ToString(format);
                            }

                            noiDungHienThi += "Thời gian: <strong>" + GetThoiGianHienThi(dr["thoigian"].ToString(), lang) + "</strong>";
                            noiDung += "Thời gian: " + GetThoiGianHienThi(dr["thoigian"].ToString(), lang);

                            results.noiDungHienThi = noiDungHienThi;
                            results.anhDaiDien = dr["AnhDaiDien"].ToString();
                            results.anhDaiDien_thumbnail_small = dr["AnhDaiDien_thumbnail_small"].ToString();

                            results.thaoTac = dr["tenloai"].ToString();
                            results.noiDung = noiDung;

                            results.nhanVien = dr["tennhanvien"].ToString();

                            results.diaChi = dr["DiaChi"].ToString();

                            DateTime thoigian = Convert.ToDateTime(dr["thoigian"].ToString());
                            results.thoiGian = thoigian.ToString("dd/MM/yyyy HH:mm");
                            results.id = int.Parse(dr["id"].ToString());

                            list.Add(results);
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            Results results = new Results();
                            int loai = Convert.ToInt32(dr["loai"].ToString());
                            switch (loai)
                            {
                                case 1:
                                    results.thaoTacHienThi = "<span class=\"label label-warning box\">" + dr["tenloai"].ToString() + "</span>";
                                    break;
                                case 2:
                                    results.thaoTacHienThi = "<span class=\"label label-danger box\">" + dr["tenloai"].ToString() + "</span>";
                                    break;
                                case 3:
                                    results.thaoTacHienThi = "<a href='' ng-click='openlichsuavaoradiem()'><span class=\"label label-primary box\">" + dr["tenloai"].ToString() + "</span></a>";
                                    break;
                                case 4:
                                    results.thaoTacHienThi = "<a href='' ng-click='openlichsuavaoradiem()'><span class=\"label label-purple box\">" + dr["tenloai"].ToString() + "</span></a>";
                                    break;
                                case 5:
                                    results.thaoTacHienThi = "<a href='' ng-click='openalbumanh()'><span class=\"label label-info box\">" + dr["tenloai"].ToString() + "</span></a>";
                                    break;
                                case 6:
                                    results.thaoTacHienThi = "<a href='' ng-click='openchitietdonhang()'><span  class=\"label label-yellow box\">" + dr["tenloai"].ToString() + "</span></a>";
                                    break;
                                default:
                                    results.thaoTacHienThi = "";
                                    break;
                            }

                            string noiDungHienThi = "";
                            string noiDung = "";
                            noiDungHienThi += "Employee: <strong>" + dr["tennhanvien"].ToString() + "</strong><br />";
                            noiDung += "Employee: " + dr["tennhanvien"].ToString();

                            if (loai != 1 && loai != 2)
                            {
                                noiDungHienThi += " Customer: <strong>" + dr["tenkhachhang"].ToString() + "</strong> <br />";
                                noiDung += " Customer: " + dr["tenkhachhang"].ToString();

                                noiDungHienThi += " Address : <strong>" + dr["DiaChi"].ToString() + "</strong> <br />";
                                noiDung += " Address: " + dr["DiaChi"].ToString();
                            }

                            if (loai == 5)
                            {
                                noiDungHienThi += " Photos: <strong><a href='' ng-click='openalbumanh()'>" + dr["soluonganh"].ToString() + " </a></strong> <br />";
                                noiDung += " Photos: " + dr["soluonganh"].ToString();
                            }

                            if (loai == 6)
                            {
                                string format = userinfo.DinhDangTienSoThapPhan == 1 ? "n2" : "n0";
                                double tt = Convert.ToDouble(dr["TongTien"].ToString());
                                noiDungHienThi += " Total Amount: <strong><a href='' ng-click='openchitietdonhang()'>" + tt.ToString(format) + "</a></strong> <br />";
                                noiDung += " Total Amount: " + tt.ToString(format);
                            }

                            noiDungHienThi += "Time: <strong>" + GetThoiGianHienThi(dr["thoigian"].ToString(), lang) + "</strong>";
                            noiDung += "Time: " + GetThoiGianHienThi(dr["thoigian"].ToString(), lang);

                            results.noiDungHienThi = noiDungHienThi;
                            results.anhDaiDien = dr["AnhDaiDien"].ToString();
                            results.anhDaiDien_thumbnail_small = dr["AnhDaiDien_thumbnail_small"].ToString();

                            results.thaoTac = dr["tenloai"].ToString();
                            results.noiDung = noiDung;

                            results.nhanVien = dr["tennhanvien"].ToString();

                            results.diaChi = dr["DiaChi"].ToString();

                            DateTime thoigian = Convert.ToDateTime(dr["thoigian"].ToString());
                            results.thoiGian = thoigian.ToString("dd/MM/yyyy HH:mm");
                            results.id = int.Parse(dr["id"].ToString());

                            list.Add(results);
                        }
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

        [HttpGet]
        [Route("ExportExcellichSuThaoTac")]
        public HttpResponseMessage ExportExcellichSuThaoTac([FromUri] int idKhachHang, int idNhanVien, int thaoTac, DateTime from, DateTime to)
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

                    BaoCaoCommon baocao = new BaoCaoCommon();
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    DataTable dt = baocao.BaoCaoLichSuThaoTac(userinfo.ID_QLLH, userinfo.ID_QuanLy, idKhachHang, idNhanVien, from, to, thaoTac);

                    if (lang == "vi")
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            Results results = new Results();

                            int loai = Convert.ToInt32(dr["loai"].ToString());
                            results.thaoTacHienThi = dr["tenloai"].ToString();
                            string noiDungHienThi = "";
                            string noiDung = "";
                            noiDungHienThi += "Nhân viên: " + dr["tennhanvien"].ToString() + "";
                            noiDung += "Nhân viên: " + dr["tennhanvien"].ToString();

                            if (loai != 1 && loai != 2)
                            {
                                noiDungHienThi += " Khách hàng: <strong>" + dr["tenkhachhang"].ToString() + "</strong> <br />";
                                noiDung += " Khách hàng: " + dr["tenkhachhang"].ToString();

                                noiDungHienThi += " Địa chỉ : <strong>" + dr["DiaChi"].ToString() + "</strong> <br />";
                                noiDung += " Địa chỉ: " + dr["DiaChi"].ToString();
                            }

                            if (loai == 5)
                            {
                                noiDungHienThi += " Số lượng ảnh: <strong><a href='../DSAlbum_ChiTiet.aspx?idalbum=" + dr["id"].ToString() + "'>" + dr["soluonganh"].ToString() + " </a></strong> <br />";
                                noiDung += " Số lượng ảnh: " + dr["soluonganh"].ToString();
                            }

                            if (loai == 6)
                            {
                                string format = userinfo.DinhDangTienSoThapPhan == 1 ? "n2" : "n0";
                                double tt = Convert.ToDouble(dr["TongTien"].ToString());
                                noiDungHienThi += " Giá trị: " + tt.ToString(format);
                                noiDung += " Giá trị: " + tt.ToString(format);
                            }

                            noiDungHienThi += "Thời gian:" + GetThoiGianHienThi(dr["thoigian"].ToString(), lang);
                            noiDung += "Thời gian: " + GetThoiGianHienThi(dr["thoigian"].ToString(), lang);

                            results.noiDungHienThi = noiDungHienThi;
                            results.anhDaiDien = dr["AnhDaiDien"].ToString();
                            results.anhDaiDien_thumbnail_small = dr["AnhDaiDien_thumbnail_small"].ToString();

                            results.thaoTac = dr["tenloai"].ToString();
                            results.noiDung = noiDung;

                            results.nhanVien = dr["tennhanvien"].ToString();

                            results.diaChi = dr["DiaChi"].ToString();

                            DateTime thoigian = Convert.ToDateTime(dr["thoigian"].ToString());
                            results.thoiGian = thoigian.ToString("dd/MM/yyyy HH:mm");


                            list.Add(results);
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            Results results = new Results();

                            int loai = Convert.ToInt32(dr["loai"].ToString());
                            results.thaoTacHienThi = dr["tenloai"].ToString();
                            string noiDungHienThi = "";
                            string noiDung = "";
                            noiDungHienThi += "Employee: " + dr["tennhanvien"].ToString() + "";
                            noiDung += "Employee: " + dr["tennhanvien"].ToString();

                            if (loai != 1 && loai != 2)
                            {
                                noiDungHienThi += " Customer: <strong>" + dr["tenkhachhang"].ToString() + "</strong> <br />";
                                noiDung += " Customer: " + dr["tenkhachhang"].ToString();

                                noiDungHienThi += " Address : <strong>" + dr["DiaChi"].ToString() + "</strong> <br />";
                                noiDung += " Address: " + dr["DiaChi"].ToString();
                            }

                            if (loai == 5)
                            {
                                noiDungHienThi += " Photos: <strong><a href='../DSAlbum_ChiTiet.aspx?idalbum=" + dr["id"].ToString() + "'>" + dr["soluonganh"].ToString() + " </a></strong> <br />";
                                noiDung += " Photos: " + dr["soluonganh"].ToString();
                            }

                            if (loai == 6)
                            {
                                string format = userinfo.DinhDangTienSoThapPhan == 1 ? "n2" : "n0";
                                double tt = Convert.ToDouble(dr["TongTien"].ToString());
                                noiDungHienThi += " Total Amount: " + tt.ToString(format);
                                noiDung += " Total Amount: " + tt.ToString(format);
                            }

                            noiDungHienThi += "Time:" + GetThoiGianHienThi(dr["thoigian"].ToString(), lang);
                            noiDung += "Time: " + GetThoiGianHienThi(dr["thoigian"].ToString(), lang);

                            results.noiDungHienThi = noiDungHienThi;
                            results.anhDaiDien = dr["AnhDaiDien"].ToString();
                            results.anhDaiDien_thumbnail_small = dr["AnhDaiDien_thumbnail_small"].ToString();

                            results.thaoTac = dr["tenloai"].ToString();
                            results.noiDung = noiDung;

                            results.nhanVien = dr["tennhanvien"].ToString();

                            results.diaChi = dr["DiaChi"].ToString();

                            DateTime thoigian = Convert.ToDateTime(dr["thoigian"].ToString());
                            results.thoiGian = thoigian.ToString("dd/MM/yyyy HH:mm");


                            list.Add(results);
                        }
                    }

                    ExportExcel excel = new ExportExcel();
                    DataTable dt0 = new DataTable();
                    dt0 = excel.ConvertToDataTable(list);
                    dt0.TableName = "DATA";

                    DataTable dt1 = new DataTable();
                    dt1.TableName = "DATA1";
                    dt1.Columns.Add("TITLE", typeof(String));
                    DataRow row = dt1.NewRow();
                    row["TITLE"] = "";
                    dt1.Rows.Add(row);

                    
                    DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    dt2.TableName = "DATA2";
                    

                    DataSet dataSet = new DataSet();
                    dataSet.Tables.Add(dt0.Copy());
                    dataSet.Tables.Add(dt1.Copy());
                    dataSet.Tables.Add(dt2.Copy());

                    string filename = "";
                    var stream = new MemoryStream();
                    if (lang == "vi")
                    {
                        filename = "BM007_LichSuThaoTac_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("LichSuThaoTac.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM007_ActionHistory_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("LichSuThaoTac_en.xls", dataSet, null, ref stream);
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

        public string GetThoiGianHienThi(object dt, string lang)
        {
            DateTime thoigian = Convert.ToDateTime(dt);

            string hienthi = "";
            TimeSpan ts = DateTime.Now - thoigian;
            if(lang == "vi")
            {
                if (thoigian.Year > 1900 && ts.TotalSeconds < 60)
                {
                    //< 1 phut
                    hienthi = ts.Seconds + " giây trước (" + thoigian.ToString("HH:mm:ss") + ")";
                }
                else if (thoigian.Year > 1900 && ts.TotalSeconds < 60 * 60)
                {
                    //< 1 tieng
                    hienthi = "Khoảng " + ts.Minutes + " phút trước (" + thoigian.ToString("HH:mm") + ")";
                }
                else if (thoigian.Year > 1900 && ts.TotalSeconds < 60 * 60 * 24)
                {
                    hienthi = "Khoảng " + Math.Round(ts.TotalHours, 0) + " giờ trước (" + thoigian.ToString("HH:mm") + ")";
                    //< 1 ngay
                }
                else
                {
                    hienthi = thoigian.Day + " tháng " + thoigian.Month + " lúc " + thoigian.ToString("HH:mm");
                }
            }
            else
            {
                if (thoigian.Year > 1900 && ts.TotalSeconds < 60)
                {
                    //< 1 phut
                    hienthi = ts.Seconds + " seconds ago (" + thoigian.ToString("HH:mm:ss") + ")";
                }
                else if (thoigian.Year > 1900 && ts.TotalSeconds < 60 * 60)
                {
                    //< 1 tieng
                    hienthi = "About " + ts.Minutes + " minutes ago (" + thoigian.ToString("HH:mm") + ")";
                }
                else if (thoigian.Year > 1900 && ts.TotalSeconds < 60 * 60 * 24)
                {
                    hienthi = "About " + Math.Round(ts.TotalHours, 0) + " hour ago (" + thoigian.ToString("HH:mm") + ")";
                    //< 1 ngay
                }
                else
                {
                    hienthi = thoigian.ToString("dd/MM") + " at " + thoigian.ToString("HH:mm");
                    //hienthi = thoigian.Day + " month " + thoigian.Month + " at " + thoigian.ToString("HH:mm");
                }
            }
            
            return hienthi;
        }
    }
}
