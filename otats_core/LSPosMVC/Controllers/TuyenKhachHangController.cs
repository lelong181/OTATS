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
using LSPos_Data.Data;
using LSPos_Data.Models;
using Ksmart_DataSon.DataAccess;
using System.Web.Http.Cors;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/tuyenkhachhang")]
    [EnableCors(origins: "*", "*", "*")]

    public class TuyenKhachHangController : ApiController
    {
        private class RequestGridParam
        {
            public DataSourceRequest request { get; set; }
        }

        public class FilterTuyenGrid
        {
            public string TenTuyen { get; set; }
            public string MoTa { get; set; }
            public int SoLuongKhach { get; set; }
            public int SoLuongNhanVien { get; set; }
            public int SoLuongNhom { get; set; }
        }
        [HttpGet]
        [Route("getalltuyen")]
        public DataSourceResult GetAllTuyen(HttpRequestMessage requestMessage)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            DataSourceResult s = new DataSourceResult();
            try
            {
                RequestGridParam param = JsonConvert.DeserializeObject<RequestGridParam>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    TuyenKhachHangData tdt = new TuyenKhachHangData();
                    List<TuyenDTO> data = tdt.GetAll(userinfo.ID_QLLH);
                    int tongso = data.Count;

                    FilterTuyenGrid filter = new FilterTuyenGrid();
                    if (param.request.Filter != null)
                    {
                        foreach (Filter f in param.request.Filter.Filters)
                        {
                            switch (f.Field)
                            {
                                case "tenTuyen":
                                    filter.TenTuyen = f.Value.ToString(); ;
                                    data = data.Where(x => x.TenTuyen.ToLower().Contains(filter.TenTuyen.ToLower())).ToList();
                                    break;
                                case "moTa":
                                    filter.MoTa = f.Value.ToString(); ;
                                    data = data.Where(x => x.MoTa != null).ToList();
                                    data = data.Where(x => x.MoTa.ToLower().Contains(filter.MoTa.ToLower())).ToList();
                                    break;
                                case "soLuongKhachHang":
                                    filter.SoLuongKhach = int.Parse(f.Value.ToString());
                                    data = data.Where(x => x.SoLuongKhachHang == filter.SoLuongKhach).ToList();
                                    break;
                                case "soLuongNhanVien":
                                    filter.SoLuongNhanVien = int.Parse(f.Value.ToString());
                                    data = data.Where(x => x.SoLuongNhanVien == filter.SoLuongNhanVien).ToList();
                                    break;
                                case "soLuongNhomNhanVien":
                                    filter.SoLuongNhom = int.Parse(f.Value.ToString());
                                    data = data.Where(x => x.SoLuongNhomNhanVien == filter.SoLuongNhom).ToList();
                                    break;
                            }
                        }
                        tongso = data.Count;
                    }


                    //s = data.AsQueryable().ToDataSourceResult(param.request.Take, param.request.Skip, param.request.Sort, param.request.Filter);
                    s.Data = data.Skip(param.request.Skip).Take(param.request.Take);
                    s.Total = tongso;
                    s.Aggregates = null;
                    //response = Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return s;
        }

        public class LichViengThamTuyen
        {
            public List<DateTime> LichViengTham { get; set; }
            public List<int> CacNgayThucHien { get; set; }
            public int LoaiTanSuat { get; set; }
            public DateTime NgayKetThuc { get; set; }
        }


        [HttpGet]
        [Route("getbyid")]
        public HttpResponseMessage GetByID([FromUri] int ID)
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
                    TuyenKhachHangData tdt = new TuyenKhachHangData();
                    TuyenDTO item = tdt.GetByID(ID);
                    DataTable dt = tdt.GetDsKhachHangTheoTuyen(userinfo.ID_QLLH, userinfo.ID_QuanLy, ID);
                    DataTable dtnhom = tdt.GetNhomNhanVienTuyen(ID);
                    List<int> listnhom = new List<int>();
                    if (dtnhom != null)
                    {
                        foreach (DataRow dr in dtnhom.Rows)
                        {
                            int idnhom = (dr["ID_NhomNhanVien"] != null) ? int.Parse(dr["ID_NhomNhanVien"].ToString()) : 0;
                            listnhom.Add(idnhom);
                        }
                    }
                    DataTable dtnv = tdt.GetNhanVienTuyen(ID);
                    List<int> listnhanvien = new List<int>();
                    if (dtnv != null)
                    {
                        foreach (DataRow dr in dtnv.Rows)
                        {
                            int idnv = (dr["ID_NhanVien"] != null) ? int.Parse(dr["ID_NhanVien"].ToString()) : 0;
                            listnhanvien.Add(idnv);
                        }
                    }
                    List<KhachHangDTO> dskhachhang = new List<KhachHangDTO>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        KhachHangDTO kh = new KhachHangDTO();
                        kh.ID_KhachHang = (dr["ID_KhachHang"] != null) ? int.Parse(dr["ID_KhachHang"].ToString()) : 0;
                        kh.TenKhachHang = (dr["TenKhachHang"] != null) ? dr["TenKhachHang"].ToString() : "";
                        kh.KinhDo = double.Parse(dr["KinhDo"].ToString());
                        kh.ViDo = double.Parse(dr["ViDo"].ToString());
                        kh.MaKH = (dr["MaKH"] != null) ? dr["MaKH"].ToString() : "";
                        kh.DiaChi = (dr["DiaChi"] != null) ? dr["DiaChi"].ToString() : "";
                        kh.TenTinh = (dr["TenTinh"] != null) ? dr["TenTinh"].ToString() : "";
                        kh.TenQuan = (dr["TenQuan"] != null) ? dr["TenQuan"].ToString() : "";
                        kh.TenPhuong = (dr["TenPhuong"] != null) ? dr["TenPhuong"].ToString() : "";
                        kh.DienThoai = (dr["DienThoai"] != null) ? dr["DienThoai"].ToString() : "";
                        kh.Email = (dr["Email"] != null) ? dr["Email"].ToString() : "";
                        kh.TenLoaiKhachHang = (dr["TenLoaiKhachHang"] != null) ? dr["TenLoaiKhachHang"].ToString() : "";
                        kh.TenNhomKH = (dr["TenNhomKH"] != null) ? dr["TenNhomKH"].ToString() : "";
                        kh.NguoiLienHe = (dr["NguoiLienHe"] != null) ? dr["NguoiLienHe"].ToString() : "";
                        dskhachhang.Add(kh);
                    }
                    LichViengThamTuyen lvt = new LichViengThamTuyen();
                    LichViengThamTuyenData ltdt = new LichViengThamTuyenData();
                    lvt.LichViengTham = ltdt.GetAllLichForTuyen(userinfo.ID_QLLH, ID);
                    DataTable dtlich = ltdt.GetTanSuatTuyenByID(ID);
                    lvt.CacNgayThucHien = new List<int>();
                    if (dtlich != null)
                    {
                        lvt.LoaiTanSuat = int.Parse(dtlich.Rows[0]["ID_LoaiTanSuat"].ToString());
                        if (!string.IsNullOrWhiteSpace(dtlich.Rows[0]["CacNgayThucHien"].ToString()))
                        {
                            List<string> cacngaythuchiens = dtlich.Rows[0]["CacNgayThucHien"].ToString().Split(',').ToList();
                            foreach (string ngay in cacngaythuchiens)
                            {
                                lvt.CacNgayThucHien.Add(int.Parse(ngay));
                            }
                        }
                        lvt.NgayKetThuc = Convert.ToDateTime(dtlich.Rows[0]["NgayKetThuc"].ToString());
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, new { tuyen = item, lichviengtham = lvt, dsnhom = listnhom, dsnhanvien = listnhanvien, dskhachhang = dskhachhang });
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
        [Route("deleteTuyen")]
        public HttpResponseMessage DeleteTuyen([FromBody] List<int> Ids)
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
                    TuyenKhachHangData tdt = new TuyenKhachHangData();
                    foreach (int id in Ids)
                    {
                        tdt.DeleteTuyen(id);
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

        public class RequestCreateTuyen
        {
            public int Id { get; set; }
            public string TenTuyen { get; set; }
            public string MoTa { get; set; }
            public List<int> DsNhom { get; set; }
            public List<int> DsNhanVien { get; set; }
            public List<int> DsKhachHang { get; set; }
            public int KieuLich { get; set; }
            public int LoaiTanSuat { get; set; }
            public DateTime[] Ngay { get; set; }
            public int[] Thu { get; set; }
            public DateTime NgayKetThuc { get; set; }
        }
        [HttpPost]
        [Route("themsua")]
        public HttpResponseMessage CreateOrUpdate([FromBody] RequestCreateTuyen item)
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
                    TuyenKhachHangData tdt = new TuyenKhachHangData();
                    if (item.Id > 0)
                    {
                        TuyenDTO newitem = tdt.GetByID(item.Id);
                        newitem.TenTuyen = item.TenTuyen;
                        newitem.MoTa = item.MoTa;
                        tdt.UpdateTuyen(newitem);
                        tdt.ClearNhanVienTuyen(userinfo.ID_QLLH, item.Id);



                        foreach (int idnv in item.DsNhanVien)
                        {
                            tdt.Insert_TuyenNhanVien(userinfo.ID_QLLH, item.Id, idnv);
                        }
                        tdt.ClearNhomNhanVienTuyen(userinfo.ID_QLLH, item.Id);

                        foreach (int idnnv in item.DsNhom)
                        {
                            tdt.Insert_TuyenNhomNhanVien(userinfo.ID_QLLH, item.Id, idnnv);
                        }

                        KhachHangTuyen_Data khdt = new KhachHangTuyen_Data();
                        tdt.ClearKhachHangTuyen(item.Id);
                        foreach (int idkh in item.DsKhachHang)
                        {
                            khdt.ThemKhachHangTuyen(idkh, item.Id);
                        }
                        LichViengThamTuyenData ltdt = new LichViengThamTuyenData();



                        if (item.DsNhanVien.Count == 0)
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "Sửa tuyến thành công, không thể tạo lịch do danh sách nhân viên trống." });
                        }
                        else
                        {
                            List<DateTime> lstNgay = new List<DateTime>();
                            foreach (DateTime d in item.Ngay)
                            {
                                lstNgay.Add(d);
                                ltdt.InsertLich(userinfo.ID_QLLH, item.Id, d);
                            }
                            ltdt.TaoLich(userinfo.ID_QLLH, item.Id, item.LoaiTanSuat, 1, string.Join(",", item.Thu), "", item.NgayKetThuc);
                            List<DateTime> lngay = GetDanhSachNgayViengThamRow(DateTime.Now, DateTime.Now, item.NgayKetThuc, DateTime.Now, item.NgayKetThuc, item.LoaiTanSuat, 1, item.Thu);
                            foreach (DateTime d in lngay)
                            {
                                lstNgay.Add(d);
                                ltdt.InsertLich(userinfo.ID_QLLH, item.Id, d);
                            }

                            string strDate = "(";
                            int x = 0;
                            foreach (DateTime dr in lstNgay)
                            {
                                x++;

                                if (x == lstNgay.Count)
                                {
                                    strDate += "'" + dr.ToString("yyyy-MM-dd") + "'";
                                }
                                else
                                {
                                    strDate += "'" + dr.ToString("yyyy-MM-dd") + "'" + ",";
                                }



                            }

                            strDate += ")";
                            if (x > 0)
                            {
                                SqlDataHelper helper = new SqlDataHelper();
                                //TRUONGNM : xóa kế hoạch các nhân viên mà trên quản lý đã xóa, trước không có đoạn xóa :((
                                helper.ExecuteNonQuery("delete from KeHoachViengThamTuyen where ID_Tuyen = " + item.Id + " and CONVERT(varchar(10), NgayViengTham_KeHoach_BatDau, 111) NOT IN  '" + strDate + "'");
                            }

                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_suatuyenthanhcong" });
                        }

                    }
                    else
                    {
                        TuyenDTO newitem = new TuyenDTO();
                        newitem.TenTuyen = item.TenTuyen;
                        newitem.MoTa = item.MoTa;
                        newitem.ID_NhanVien = 0;
                        newitem.ID_QLLH = userinfo.ID_QLLH;
                        int idtuyen = tdt.CreateTuyen(newitem);
                        foreach (int idnv in item.DsNhanVien)
                        {
                            tdt.Insert_TuyenNhanVien(userinfo.ID_QLLH, idtuyen, idnv);
                        }
                        foreach (int idnnv in item.DsNhom)
                        {
                            tdt.Insert_TuyenNhomNhanVien(userinfo.ID_QLLH, idtuyen, idnnv);
                        }
                        KhachHangTuyen_Data khdt = new KhachHangTuyen_Data();
                        foreach (int idkh in item.DsKhachHang)
                        {
                            khdt.ThemKhachHangTuyen(idkh, idtuyen);
                        }

                        LichViengThamTuyenData ltdt = new LichViengThamTuyenData();
                        if (item.DsNhanVien.Count == 0)
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_themtuyenthanhcongkhongthetaolichdodanhsachnhanvientrong" });
                        }
                        else
                        {
                            foreach (DateTime d in item.Ngay)
                            {
                                ltdt.InsertLich(userinfo.ID_QLLH, idtuyen, d);
                            }
                            ltdt.TaoLich(userinfo.ID_QLLH, idtuyen, item.LoaiTanSuat, 1, string.Join(",", item.Thu), "", item.NgayKetThuc);
                            List<DateTime> lngay = GetDanhSachNgayViengThamRow(DateTime.Now, DateTime.Now, item.NgayKetThuc, DateTime.Now, item.NgayKetThuc, item.LoaiTanSuat, 1, item.Thu);
                            foreach (DateTime d in lngay)
                            {
                                ltdt.InsertLich(userinfo.ID_QLLH, idtuyen, d);
                            }
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_themtuyenthanhcong" });
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

        public class Point
        {
            public double lon { get; set; }
            public double lat { get; set; }
            public string diachi { get; set; }
            public string tenkhachhang { get; set; }
        }
        [HttpGet]
        [Route("getpoints")]
        public HttpResponseMessage GetPoints([FromUri] int ID)
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
                    TuyenKhachHangData tdt = new TuyenKhachHangData();
                    DataTable dt = tdt.GetDsKhachHangTheoTuyen(userinfo.ID_QLLH, userinfo.ID_QuanLy, ID);
                    List<Point> r = new List<Point>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        Point p = new Point();
                        p.lon = double.Parse(dr["KinhDo"].ToString());
                        p.lat = double.Parse(dr["ViDo"].ToString());
                        p.tenkhachhang = (dr["TenKhachHang"] != null) ? dr["TenKhachHang"].ToString() : "";
                        p.diachi = (dr["DiaChi"] != null) ? dr["DiaChi"].ToString() : "";
                        r.Add(p);
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, r);

                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        public static List<DateTime> GetDanhSachNgayViengThamRow(DateTime ngayhomnay, DateTime ngaybatdau, DateTime ngayketthuc, DateTime tungay, DateTime denngay, int loaitansuat, int tansuat, int[] listngaythuchien)
        {
            //type = 1 mặc định theo 1 tháng, type = 2 theo từ ngày đến ngày
            List<DateTime> ds = new List<DateTime>();


            //loại hằng tuần
            if (loaitansuat == 2)
            {
                //Khai báo 1 list ngày đi
                List<DateTime> listngaydi = new List<DateTime>();

                int thungaybatdau = 0;

                //Lấy ra thứ ngày bắt đầu
                thungaybatdau = (int)ngaybatdau.DayOfWeek;

                //lặp danh sách ngày thực hiện 
                for (int i = 0; i < listngaythuchien.Length; i++)
                {
                    //tính ngày lệch bao nhiêu so với thứ ngày bắt đầu
                    int ngaylech = listngaythuchien[i] - 1 - thungaybatdau;
                    if (DateTime.Compare(ngaybatdau.AddDays(ngaylech), DateTime.Now) <= 0)
                    {
                        listngaydi.Add(ngaybatdau.AddDays(ngaylech + 7));
                    }
                    else
                    {
                        listngaydi.Add(ngaybatdau.AddDays(ngaylech));
                    }
                    //Add ngày lệch đó vào tìm ra ngày bắt đầu gần nhất

                }

                //vòng lặp list ngày đi vừa tìm được
                foreach (DateTime ngaydi in listngaydi)
                {
                    DateTime ngay = ngaydi;
                    //thực hiện vòng lặp theo từng ngày đi trong tháng theo tần suất * 7 ngày
                    while ((DateTime.Compare(ngay, denngay) <= 0 && DateTime.Compare(ngay, tungay) >= 0) && DateTime.Compare(ngay, ngayketthuc) <= 0)
                    {
                        if (DateTime.Compare(ngay, tungay) >= 0)
                            ds.Add(ngay);

                        ngay = ngay.AddDays(tansuat * 7);
                    }
                }
            }

            //loại hằng tháng
            if (loaitansuat == 3)
            {
                //gán ngày đi = ngày bắtt đầu
                DateTime ngaydi = ngaybatdau;
                //Nếu như ngày đi cộng 1 ngày < tháng sau
                while (DateTime.Compare(ngaydi.AddDays(1), denngay) <= 0 && DateTime.Compare(ngaydi.AddDays(1), ngayketthuc) <= 0)
                {
                    //thực hiện cộng 1 ngày

                    for (int i = 0; i < listngaythuchien.Length; i++)
                    {
                        //lấy thứ của ngày đi
                        int ngayditrongthang = (int)ngaydi.Day;

                        //so sánh với ngày thực hiện tour
                        if (ngayditrongthang == listngaythuchien[i])
                            //Nếu bằng nhau thì lấy ra ngày đó
                            if (DateTime.Compare(ngaydi, tungay) >= 0)
                                ds.Add(ngaydi);
                    }

                    ngaydi = ngaydi.AddDays(1);
                }
            }

            return ds;
        }
        [HttpGet]
        [Route("getallcombotuyen")]
        public HttpResponseMessage getallcombotuyen()
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
                    TuyenKhachHangData tdt = new TuyenKhachHangData();
                    List<ComboboxDTO> tt = tdt.ComboboxTuyen(userinfo.ID_QLLH);
                    response = Request.CreateResponse(HttpStatusCode.OK, tt);
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
        [Route("ExcelTuyenKhachHang")]
        public HttpResponseMessage ExcelTuyenKhachHang()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                TuyenKhachHangData tdt = new TuyenKhachHangData();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = tdt.GetdsTuyenKhachHang(userinfo.ID_QLLH);
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
                        filename = "BM025_DanhSachTuyenKhachHang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("Excel_TuyenKhachHang.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM025_CustomerRoutePlan_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("Excel_TuyenKhachHang_en.xls", dataSet, null, ref stream);
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
    }
}
