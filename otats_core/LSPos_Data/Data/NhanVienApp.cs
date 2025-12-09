using LSPos_API.Utils;
using LSPos_Data.Models;
using LSPos_Data.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class NhanVienApp
    {
        private SqlDataHelper helper;
        public NhanVienApp()
        {
            helper = new SqlDataHelper();
        }

        public class FilterGridNV
        {
            public int IDNV { get; set; }
            public string TenDangNhap { get; set; }
            public string TenDayDu { get; set; }
            public string DienThoai { get; set; }
            public string TrangThaiTrucTuyen { get; set; }
            public double KinhDo { get; set; }
            public double ViDo { get; set; }
            public string PhienBan { get; set; }
            public string ThoiGianGuiBanTinCuoiCung { get; set; }
            public string TenNhom { get; set; }
            public string AnhDaiDien { get; set; }
            public string AnhDaiDien_thumbnail_medium { get; set; }
            public string AnhDaiDien_thumbnail_small { get; set; }


        }

        public int ThemNhanVien_v1(NhanVienModels nv)
        {
            int re = 0;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_NhanVien", 0),
                    new SqlParameter("@ID_QLLH", nv.IDQLLH),
                    new SqlParameter("@TenNhanVien", nv.TenDayDu),
                    new SqlParameter("@TenDangNhap", nv.TenDangNhap),
                    new SqlParameter("@MatKhau", Utils.md5(nv.MatKhau)),
                    new SqlParameter("@DiaChi", nv.DiaChi),
                    new SqlParameter("@QueQuan", nv.QueQuan),
                    new SqlParameter("@NgaySinh", nv.NgaySinh),
                    new SqlParameter("@Email", nv.Email),
                    new SqlParameter("@DienThoai", nv.DienThoai),
                    new SqlParameter("@ID_QuanLy", nv.ID_QuanLy),
                    new SqlParameter("@ID_Nhom", nv.ID_Nhom),
                    new SqlParameter("@ID_NhomKhachHang_MacDinh", nv.ID_NhomKhachHang_MacDinh),
                    new SqlParameter("@GioiTinh", nv.GioiTinh),
                    new SqlParameter("@ID_ChucVu", nv.ID_ChucVu),
                    new SqlParameter("@ChucVu", nv.ChucVu),
                    new SqlParameter("@Loai", nv.Loai),
                    new SqlParameter("@TruongNhom", nv.TruongNhom)
                    };

                DataSet ds = helper.ExecuteDataSet("VuongTM_NhanVien_ThemNhanVienApp", pars);
                DataTable dt = ds.Tables[0];
                re = int.Parse(dt.Rows[0]["re"].ToString());
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return re;
        }
        public int ThemNhanVienKhongDangNhap(NhanVienModels nhanVienModels)
        {
            int re = 0;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_NhanVien", nhanVienModels.IDNV),
                    new SqlParameter("@ID_QLLH", nhanVienModels.IDQLLH),
                    new SqlParameter("@ID_QuanLy", nhanVienModels.ID_QuanLy),
                    new SqlParameter("@TenNhanVien", nhanVienModels.TenDayDu),
                    new SqlParameter("@ID_Nhom", nhanVienModels.ID_Nhom),
                    new SqlParameter("@DienThoai", nhanVienModels.DienThoai)
                    };

                DataSet ds = helper.ExecuteDataSet("VuongTM_NhanVien_ThemNhanVienKhongDangNhap", pars);
                DataTable dt = ds.Tables[0];
                re = int.Parse(dt.Rows[0]["re"].ToString());
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return re;
        }

        public DataTable GetDSNhanVien_DaDangNhapApp_TheoNhom(int ID_QLLH, int ID_Nhom)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_QLLH", ID_QLLH),
                new SqlParameter("@ID_Nhom", ID_Nhom)
                };

                DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSNhanVien_DaDangNhapApp_TheoNhom_v1", pars);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }

        public List<NhanVienModels> GetDSNhanVien_TheoNhomQuanLy(int ID_QLLH, int ID_QuanLy, int ID_Nhom)
        {
            List<NhanVienModels> dsnv = new List<NhanVienModels>();
            try
            {
                BaoCaoCommon baocao = new BaoCaoCommon();
                string lang = baocao.GetCurrentLanguages(ID_QLLH, ID_QuanLy);

                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_Nhom", ID_Nhom)
                    };

                DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSNhanVien_TheoNhom_V4", pars);
                DataTable dt = ds.Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    NhanVienModels nv = GetNhanVienFromDataRowNew(dr, lang);
                    dsnv.Add(nv);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dsnv;
        }
        
        
        


        public DataSet ExportExcelNhanVien(int IDQLLH, int ID_QuanLy, ExportNhanVienDTO filter)
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_QLLH", IDQLLH),
                new SqlParameter("@ID_QuanLy", ID_QuanLy),
                new SqlParameter("@ID_Nhom", filter.idNhom),
                new SqlParameter("@TenDangNhap", filter.TenDangNhap) ,
                new SqlParameter("@TenDayDu", filter.TenDayDu) ,
                new SqlParameter("@DienThoai", filter.DienThoai) ,
                new SqlParameter("@TrangThaiTrucTuyen", filter.TrangThaiTrucTuyen) ,
                new SqlParameter("@ThoiGianGuiBanTinCuoiCung", filter.ThoiGianGuiBanTinCuoiCung) ,
                new SqlParameter("@PhienBan", filter.PhienBan) ,
                new SqlParameter("@TenNhom", filter.TenNhom) ,
                };
            DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSNhanVien_Export_v1", pars);
            try
            {
                return ds;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public NhanVienModels GetNhanVienFromDataRowNew(DataRow dr, string lang)
        {
            NhanVienModels nv = new NhanVienModels();
            try
            {
                nv.IDNV = int.Parse(dr["ID_NhanVien"].ToString());
                nv.IDQLLH = int.Parse(dr["ID_QLLH"].ToString());
                nv.TenDangNhap = dr["TenDangNhap"].ToString();
                nv.TenDayDu = dr["TenNhanVien"].ToString();
                nv.MatKhau = dr["MatKhau"].ToString();
                nv.DiaChi = dr["DiaChi"].ToString();
                nv.DienThoai = dr["DienThoai"].ToString();

                try
                {
                    nv.QueQuan = dr["QueQuan"].ToString();
                    nv.TruongNhom = String.IsNullOrEmpty(dr["TruongNhom"].ToString()) ? 0 : Convert.ToInt32(dr["TruongNhom"]);
                    nv.NgaySinh = String.IsNullOrEmpty(dr["NgaySinh"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["NgaySinh"]);
                    nv.Email = String.IsNullOrEmpty(dr["Email"].ToString()) ? "" : dr["Email"].ToString();
                    nv.ID_QuanLy = String.IsNullOrEmpty(dr["ID_QuanLy"].ToString()) ? 0 : Convert.ToInt32(dr["ID_QuanLy"]);
                    nv.ID_Nhom = String.IsNullOrEmpty(dr["ID_Nhom"].ToString()) ? 0 : Convert.ToInt32(dr["ID_Nhom"]);
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                }

                try
                {
                    nv.TenNhom = dr.Table.Columns.Contains("TenNhom") ? dr["TenNhom"].ToString() : "";
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                    nv.TenNhom = "";
                }

                try
                {
                    nv.AnhDaiDien = dr.Table.Columns.Contains("AnhDaiDien") ? dr["AnhDaiDien"].ToString() : "";
                    nv.AnhDaiDien_thumbnail_medium = dr.Table.Columns.Contains("AnhDaiDien_thumbnail_medium") ? dr["AnhDaiDien_thumbnail_medium"].ToString() : "";
                    nv.AnhDaiDien_thumbnail_small = dr.Table.Columns.Contains("AnhDaiDien_thumbnail_small") ? dr["AnhDaiDien_thumbnail_small"].ToString() : "";
                }
                catch
                {
                    //LSPos_Data.Utilities.Log.Error(ex);
                }

                try
                {
                    nv.Loai = int.Parse(dr["Loai"].ToString());
                }
                catch
                {
                    //LSPos_Data.Utilities.Log.Error(ex);
                    nv.Loai = 0;
                }

                try
                {
                    nv.PhienBan = dr["versionId"].ToString();
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                    nv.PhienBan = "";
                }

                try
                {
                    if (Convert.ToInt32(dr["hinhthucdangxuat"].ToString()) == 1)
                    {
                        nv.HinhThucDangXuat = (lang == "vi") ? "Tự động đăng xuất" : "Automatically logout";
                    }
                    if (Convert.ToInt32(dr["hinhthucdangxuat"].ToString()) == 0)
                    {
                        nv.HinhThucDangXuat = (lang == "vi") ? "Người dùng đăng xuất" : "User self-logout";
                    }
                    if (Convert.ToInt32(dr["hinhthucdangxuat"].ToString()) == -1)
                    {
                        nv.HinhThucDangXuat = "   ";
                    }
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                    nv.HinhThucDangXuat = "   ";
                }

                try
                {
                    if (dr["TinhTrangPin"].ToString() == "")
                        nv.TinhTrangPin = "--%";
                    else
                        nv.TinhTrangPin = dr["TinhTrangPin"] + "%";
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                    nv.TinhTrangPin = "--%";
                }

                try
                {
                    nv.TrangThai = int.Parse(dr["TrangThaiNhanVien"].ToString());
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                }

                try
                {
                    nv.TrucTuyen = String.IsNullOrEmpty(dr["dangtructuyen"].ToString()) ? 0 : Convert.ToInt32(dr["dangtructuyen"]);
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                    nv.TrucTuyen = 0;
                }

                int hinhthucdangxuat = -1;
                try
                {
                    hinhthucdangxuat = Convert.ToInt32(dr["hinhthucdangxuat"].ToString());
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                }

                if (nv.TrucTuyen == 0)
                {
                    if (hinhthucdangxuat == 1)
                    {
                        nv.TrangThaiTrucTuyen = (lang == "vi") ? "Tự động đăng xuất" : "Automatically logout";
                        nv.ThoiGianCapNhat = String.IsNullOrEmpty(dr["ThoiGianDangXuat"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianDangXuat"]);
                    }
                    else if (hinhthucdangxuat == 0)
                    {
                        nv.TrangThaiTrucTuyen = (lang == "vi") ? "Người dùng đăng xuất" : "User self-logout";
                        nv.ThoiGianCapNhat = String.IsNullOrEmpty(dr["ThoiGianDangXuat"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianDangXuat"]);
                    }
                    else if (hinhthucdangxuat == -1)
                    {
                        nv.TrangThaiTrucTuyen = (lang == "vi") ? "Ngoại tuyến" : "Offline";
                        nv.ThoiGianCapNhat = String.IsNullOrEmpty(dr["ThoiGianBanTinCuoi"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianBanTinCuoi"]);
                    }
                }
                else if (nv.TrucTuyen == 1)
                {
                    nv.TrangThaiTrucTuyen = (lang == "vi") ? "Trực tuyến" : "Online";
                    nv.ThoiGianCapNhat = String.IsNullOrEmpty(dr["ThoiGianBanTinCuoi"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianBanTinCuoi"]);
                }
                else if (nv.TrucTuyen == 2)
                {
                    nv.TrangThaiTrucTuyen = (lang == "vi") ? "Mất kết nối" : "Disconnect";
                    nv.ThoiGianCapNhat = String.IsNullOrEmpty(dr["ThoiGianBanTinCuoi"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianBanTinCuoi"]);
                }

                try
                {
                    nv.ThoiGianDangXuat = String.IsNullOrEmpty(dr["ThoiGianDangXuat"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianDangXuat"]);
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                    nv.ThoiGianDangXuat = new DateTime(1900, 1, 1);
                }
                try
                {
                    nv.ThoiGianGuiBanTinCuoiCung = String.IsNullOrEmpty(dr["ThoiGianBanTinCuoi"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianBanTinCuoi"]);
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                    nv.ThoiGianDangXuat = new DateTime(1900, 1, 1);
                }

                try
                {
                    nv.KinhDo = String.IsNullOrEmpty(dr["KinhDo"].ToString()) ? 0 : Convert.ToDouble(dr["KinhDo"]);
                    nv.ViDo = String.IsNullOrEmpty(dr["ViDo"].ToString()) ? 0 : Convert.ToDouble(dr["ViDo"]);
                }
                catch
                {
                    //LSPos_Data.Utilities.Log.Error(ex);
                }

                try
                {
                    nv.DongMay = dr.Table.Columns.Contains("dongmay") ? dr["dongmay"].ToString() : "";
                    nv.DoiMay = dr.Table.Columns.Contains("doimay") ? dr["doimay"].ToString() : "";
                    nv.TenMay = dr.Table.Columns.Contains("devicename") ? dr["devicename"].ToString() : "";
                    nv.OSVersion = dr.Table.Columns.Contains("osversion") ? dr["osversion"].ToString() : "";
                    nv.Imei = dr.Table.Columns.Contains("imei") ? dr["imei"].ToString() : "";
                    nv.ChucVu = dr.Table.Columns.Contains("ChucVu") ? dr["ChucVu"].ToString() : "";
                    if (dr["OS"].ToString() == "1")
                        nv.OS = "IOS";
                    else if (dr["OS"].ToString() == "2")
                        nv.OS = "Android";
                    else if (dr["OS"].ToString() == "2")
                        nv.OS = (lang == "vi") ? "Không xác định" : "Undefined";

                    nv.IsFakeGPS = (dr["isFakeGPS"].ToString() == "1");
                    nv.isCheDoTietKiemPin = (dr["isCheDoTietKiemPin"].ToString() == "1");
                }
                catch
                {
                    //LSPos_Data.Utilities.Log.Error(ex);
                }

                try
                {
                    nv.AppFakeGPS = dr.Table.Columns.Contains("appfakegps") ? dr["appfakegps"].ToString() : "";
                    nv.ID_ChucVu = dr.Table.Columns.Contains("ID_ChucVu") ? dr["ID_ChucVu"].ToString() != "" ? Convert.ToInt32(dr["ID_ChucVu"].ToString()) : 0 : 0;
                    nv.GioiTinh = dr.Table.Columns.Contains("GioiTinh") ? dr["GioiTinh"].ToString() != "" ? Convert.ToInt32(dr["GioiTinh"].ToString()) : 0 : 0;

                    nv.LastUpdate_ID_NhanVien = dr["LastUpdate_ID_NhanVien"].ToString() != "" ? Convert.ToInt32(dr["LastUpdate_ID_NhanVien"]) : 0;
                    nv.LastUpdate_ID_QuanLy = dr["LastUpdate_ID_QuanLy"].ToString() != "" ? Convert.ToInt32(dr["LastUpdate_ID_QuanLy"]) : 0;
                    nv.LastUpdate_ThoiGian_NhanVien = dr["LastUpdate_ThoiGian_NhanVien"].ToString() != "" ? Convert.ToDateTime(dr["LastUpdate_ThoiGian_NhanVien"]) : nv.LastUpdate_ThoiGian_NhanVien;
                    nv.LastUpdate_ThoiGian_QuanLy = dr["LastUpdate_ThoiGian_QuanLy"].ToString() != "" ? Convert.ToDateTime(dr["LastUpdate_ThoiGian_QuanLy"]) : nv.LastUpdate_ThoiGian_QuanLy;
                    //nv.LastUpdate_Ten_NhanVien = dr["LastUpdate_Ten_NhanVien"].ToString();
                    //nv.LastUpdate_Ten_QuanLy = dr["LastUpdate_Ten_QuanLy"].ToString();
                }
                catch
                {
                    //LSPos_Data.Utilities.Log.Error(ex);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return nv;
        }

        public DataTable ChiTietNhanVienTheoIDNV(int IDNV)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("IDNV", IDNV)
                };

                DataSet ds = helper.ExecuteDataSet("sp_QL_ChiTietNhanVienTheoIDNV", pars);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public static DataTable getDanhSachEmailGuiThongBao(int ID_QLLH, int ID_NhanVien)
        {
            DataTable dt = new DataTable();
            try
            {

                SqlDataHelper sql = new SqlDataHelper();
                DataSet ds = sql.ExecuteDataSet("sp_TaiKhoan_GetDanhSachGuiMailThongBaoDonHang",
                    new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_NhanVien", ID_NhanVien)
                    );
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }

        public bool XoaNhanvien(string listidnhanvien)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@IDNV", listidnhanvien)
                    };

                return (helper.ExecuteNonQuery("usp_vuongtm_web_deletenhanvienbanhang", pars) != 0);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }

        public bool ResetImei(int ID_NhanVien)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_NhanVien", ID_NhanVien)
                    };

                return (helper.ExecuteNonQuery("sp_NhanVien_ResetImei", pars) != 0);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }

        public bool UpdateIDPush(int ID_TaiKhoan, int ID_NhanVien, string IDPUSH)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID_TaiKhoan", ID_TaiKhoan),
                    new SqlParameter("@ID_NhanVien", ID_NhanVien),
                    new SqlParameter("@IDPUSH", IDPUSH)
                    };

                return (helper.ExecuteNonQuery("sp_NhanVien_UpdateIDPush", pars) != 0);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }

        public static implicit operator NhanVien_dl(NhanVienApp v)
        {
            throw new NotImplementedException();
        }

        public NhanVienAppModels ThongTinNhanVienTheoID(int idnhanvien)
        {
            NhanVienAppModels nvobj = null;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("IDNV", idnhanvien)
            };

                DataSet ds = helper.ExecuteDataSet("getNhanVienTheoID", pars);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    nvobj = GetNhanVienFromDataRow(dr);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return nvobj;
        }

        public NhanVienAppModels ThongTinNhanVienTheoTenDangNhap(string TenDangNhap, int ID_QLLH)
        {
            NhanVienAppModels nvobj = null;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("TenDangNhap", TenDangNhap),
                new SqlParameter("ID_QLLH", ID_QLLH)
            };

                DataSet ds = helper.ExecuteDataSet("getNhanVienTheoUsername", pars);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    nvobj = GetNhanVienFromDataRow(dr);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return nvobj;
        }


        public NhanVienAppModels GetNhanVienFromDataRow(DataRow dr)
        {
            NhanVienAppModels rs = new NhanVienAppModels();
            try
            {

                rs.idct = int.Parse(dr["ID_QLLH"].ToString());
                rs.idnhanvien = int.Parse(dr["ID_NhanVien"].ToString());
                rs.ID_NhomKhachHang_MacDinh = (dr["ID_NhomKhachHang_MacDinh"].ToString() != "") ? int.Parse(dr["ID_NhomKhachHang_MacDinh"].ToString()) : 0;

                rs.tennhanvien = dr["TenNhanVien"].ToString();
                rs.tendangnhap = dr["TenDangNhap"].ToString();
                rs.thoigiandangnhapcuoicung = (dr["ThoiGianHoatDong"] != null) ? DateTime.Parse(dr["ThoiGianHoatDong"].ToString()) : (DateTime?)null;
                rs.dangtructuyen = int.Parse(dr["dangtructuyen"].ToString());
                rs.device = dr["device"].ToString();
                rs.TGCanhBaoMatKetNoi = (dr["TGCanhBaoMatKetNoi"].ToString() != "") ? int.Parse(dr["TGCanhBaoMatKetNoi"].ToString()) : 5;
                rs.TGCanhBaoLaiMatKetNoi = (dr["TGCanhBaoLaiMatKetNoi"].ToString() != "") ? int.Parse(dr["TGCanhBaoLaiMatKetNoi"].ToString()) : 30;
                rs.sotinnhanchuadoc = (dr["SoTinNhanChuaDoc"].ToString() != "") ? int.Parse(dr["SoTinNhanChuaDoc"].ToString()) : 0;
                rs.idnhom = (dr["ID_Nhom"].ToString() != "") ? int.Parse(dr["ID_Nhom"].ToString()) : 0;
                rs.os = (dr["OS"].ToString() != "") ? int.Parse(dr["OS"].ToString()) : 0;
                rs.idpush = dr["IDPUSH"].ToString();
                rs.TypeApp = (dr["TypeApp"].ToString() != "") ? int.Parse(dr["TypeApp"].ToString()) : 0;
                rs.bankinhchophep = (dr["BanKinhChoPhep"].ToString() != "") ? int.Parse(dr["BanKinhChoPhep"].ToString()) : 500;
                rs.chophepvaodiemkhongkehoach = (dr["ChoPhepVaoDiemKhongKeHoach"].ToString() != "") ? int.Parse(dr["ChoPhepVaoDiemKhongKeHoach"].ToString()) : 1;
                try
                {
                    rs.AnhDaiDien = dr.Table.Columns.Contains("AnhDaiDien") ? dr["AnhDaiDien"].ToString() : "";
                    rs.AnhDaiDien_thumbnail_medium = dr.Table.Columns.Contains("AnhDaiDien_thumbnail_medium") ? dr["AnhDaiDien_thumbnail_medium"].ToString() : "";
                    rs.AnhDaiDien_thumbnail_small = dr.Table.Columns.Contains("AnhDaiDien_thumbnail_small") ? dr["AnhDaiDien_thumbnail_small"].ToString() : "";
                    rs.matkhau = dr["MatKhau"].ToString();
                    rs.TGLayViTri = (dr["TGLayViTri"].ToString() != "") ? int.Parse(dr["TGLayViTri"].ToString()) : 15000;
                    rs.ThongBaoPhienBanMoi = (dr["ThongBaoPhienBanMoi"].ToString() != "") ? int.Parse(dr["ThongBaoPhienBanMoi"].ToString()) : 0;
                    rs.Min_Version = dr["Min_Version"].ToString();
                    rs.Max_Version = dr["Max_Version"].ToString();
                    rs.icon_path = dr["icon_path"].ToString();
                    rs.MaDonHang_CauTruc = dr["MaDonHang_CauTruc"].ToString();
                    rs.MaDonHang_TuSinh = (dr["MaDonHang_TuSinh"].ToString() != "") ? int.Parse(dr["MaDonHang_TuSinh"].ToString()) : 1;
                    rs.MaDonHang_TuSinh_TheoKy = (dr["MaDonHang_TuSinh_TheoKy"].ToString() != "") ? int.Parse(dr["MaDonHang_TuSinh_TheoKy"].ToString()) : 1;
                    rs.ChiCaiDatPhanMem1Lan = (dr["ChiCaiDatPhanMem1Lan"].ToString() != "") ? int.Parse(dr["ChiCaiDatPhanMem1Lan"].ToString()) : 0;
                    rs.ngaycaidat = (dr["NgayCaiDat"] != null && dr["NgayCaiDat"].ToString() != "") ? DateTime.Parse(dr["NgayCaiDat"].ToString()) : new DateTime(1900, 01, 01);
                    rs.TGThongBaoDenKeHoach = (dr["TGThongBaoDenKeHoach"].ToString() != "") ? int.Parse(dr["TGThongBaoDenKeHoach"].ToString()) : 10;
                    rs.ChanDangNhapFakeGPS = (dr["ChanDangNhapFakeGPS"].ToString() != "") ? int.Parse(dr["ChanDangNhapFakeGPS"].ToString()) : 0;
                    rs.ChiLapDonHangKhiVaoDiem = (dr["ChiLapDonHangKhiVaoDiem"].ToString() != "") ? int.Parse(dr["ChiLapDonHangKhiVaoDiem"].ToString()) : 0;
                    rs.GioiTinh = (dr["GioiTinh"].ToString() != "") ? int.Parse(dr["GioiTinh"].ToString()) : 0;
                    rs.DiaChi = dr["DiaChi"].ToString();
                    rs.QueQuan = dr["QueQuan"].ToString();
                    rs.NgaySinh = (dr["NgaySinh"] != null && dr["NgaySinh"].ToString() != "") ? DateTime.Parse(dr["NgaySinh"].ToString()) : new DateTime(1900, 01, 01);
                    rs.Email = dr["Email"].ToString();
                    rs.kinhdo = dr.Table.Columns.Contains("KinhDo") ? ((dr["KinhDo"].ToString() != "") ? float.Parse(dr["KinhDo"].ToString()) : 0) : 0;
                    rs.vido = dr.Table.Columns.Contains("ViDo") ? ((dr["ViDo"].ToString() != "") ? float.Parse(dr["ViDo"].ToString()) : 0) : 0;
                }
                catch (Exception)
                {
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return rs;
        }
        public DataTable GetKhachHangDaCapQuyen(string ID_NhanVien, int ID_LoaiKhachHang, int ID_KenhBanHang, int ID_Tinh, int ID_Quan, int ID_Phuong)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_NhanVien", ID_NhanVien),
                    new SqlParameter("ID_LoaiKhachHang", ID_LoaiKhachHang),
                    new SqlParameter("ID_KenhBanHang", ID_KenhBanHang),
                    new SqlParameter("@ID_Tinh", ID_Tinh),
                    new SqlParameter("@ID_Quan", ID_Quan),
                    new SqlParameter("@ID_Phuong", ID_Phuong),
                    };

                DataSet ds = helper.ExecuteDataSet("sp_vuongtm_QL_getDSKhachHangDaCapQuyen", pars);
                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count == 0)
                    return new DataTable();
                else
                    return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
        public DataTable CountSoLuong(int IDQLLH, int ID_Nhom, int startRecord, int maxRecords, FilterGridNV filter)
        {
            try
            {

                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", IDQLLH),
                new SqlParameter("ID_Nhom", ID_Nhom),
                new SqlParameter("@startRecord", startRecord),
                new SqlParameter("@maxRecords", maxRecords) ,
                new SqlParameter("@TenDangNhap", filter.TenDangNhap) ,
                new SqlParameter("@TenDayDu", filter.TenDayDu) ,
                new SqlParameter("@DienThoai", filter.DienThoai) ,
                new SqlParameter("@TrangThaiTrucTuyen", filter.TrangThaiTrucTuyen) ,
                new SqlParameter("@ThoiGianGuiBanTinCuoiCung", filter.ThoiGianGuiBanTinCuoiCung) ,
                new SqlParameter("@PhienBan", filter.PhienBan) ,
                new SqlParameter("@TenNhom", filter.TenNhom) ,
                                };
                DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSNhanVien_TheoNhom_Kendo_Count", pars);
                DataTable dt = ds.Tables[0];
                return dt;
            }

            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public List<NhanVienDTO> GetDataNhanVien_Kendo(int IDQLLH, int ID_Nhom, int startRecord, int maxRecords, FilterGridNV filter, ref int tongso)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_QLLH", IDQLLH),
                    new SqlParameter("ID_Nhom", ID_Nhom),
                    new SqlParameter("@startRecord", startRecord),
                    new SqlParameter("@maxRecords", maxRecords) ,
                    new SqlParameter("@TenDangNhap", filter.TenDangNhap) ,
                    new SqlParameter("@TenDayDu", filter.TenDayDu) ,
                    new SqlParameter("@DienThoai", filter.DienThoai) ,
                    new SqlParameter("@TrangThaiTrucTuyen", filter.TrangThaiTrucTuyen) ,
                    new SqlParameter("@ThoiGianGuiBanTinCuoiCung", filter.ThoiGianGuiBanTinCuoiCung) ,
                    new SqlParameter("@PhienBan", filter.PhienBan) ,
                    new SqlParameter("@TenNhom", filter.TenNhom) ,
                     };

                DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSNhanVien_TheoNhom_Kendo", pars);
                DataTable dt = ds.Tables[0];
                DataTable dt2 = ds.Tables[1];
                if (dt2.Rows.Count > 0)
                {
                    tongso += int.Parse(dt2.Rows[0]["soluong"].ToString());
                }
                else
                {
                    tongso += 0;
                }
                List<NhanVienDTO> lnv = new List<NhanVienDTO>();
                foreach (DataRow dr in dt.Rows)
                {
                    NhanVienDTO nv = GetNhanVienFromDataRow1(dr);
                    lnv.Add(nv);

                }
                List<NhomOBJ> lstNhomCon = NhomDB.getDS_NhomCon_ById(ID_Nhom);
                if (lstNhomCon != null)
                {
                    foreach (NhomOBJ nhom in lstNhomCon)
                    {
                        List<NhanVienDTO> lstNV = GetDataNhanVien_Kendo(IDQLLH, nhom.ID_Nhom, startRecord, maxRecords, filter, ref tongso);
                        if (lstNV != null)
                        {
                            foreach (NhanVienDTO n in lstNV)
                            {
                                lnv.Add(n);
                            }
                        }
                    }
                }
                return lnv;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
        public NhanVienDTO GetNhanVienFromDataRow1(DataRow dr)
        {
            NhanVienDTO nv = new NhanVienDTO();
            try
            {
                nv.IDNV = int.Parse(dr["ID_NhanVien"].ToString());
                nv.TenDangNhap = dr["TenDangNhap"].ToString();
                nv.TenDayDu = dr["TenNhanVien"].ToString();
                nv.KinhDo = dr["KinhDo"].ToString() != "" ? double.Parse(dr["KinhDo"].ToString()) : 0;
                nv.ViDo = dr["ViDo"].ToString() != "" ? double.Parse(dr["ViDo"].ToString()) : 0;
                nv.AnhDaiDien = dr.Table.Columns.Contains("AnhDaiDien") ? dr["AnhDaiDien"].ToString() : "";
                nv.AnhDaiDien_thumbnail_medium = dr.Table.Columns.Contains("AnhDaiDien_thumbnail_medium") ? dr["AnhDaiDien_thumbnail_medium"].ToString() : "";
                nv.AnhDaiDien_thumbnail_small = dr.Table.Columns.Contains("AnhDaiDien_thumbnail_small") ? dr["AnhDaiDien_thumbnail_small"].ToString() : "";
                nv.TrangThaiTrucTuyen = dr["TrangThaiTrucTuyen"].ToString();
                nv.ThoiGianGuiBanTinCuoiCung = dr["ThoiGianDangXuat"].ToString();
                nv.TenNhom = dr["TenNhom"].ToString();
                nv.DienThoai = dr["DienThoai"].ToString();
                nv.TinhTrangPin = dr["tinhTrangPin"].ToString();
                nv.PhienBan = dr["versionId"].ToString();
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return nv;
        }

        public bool BoKhachHangChoNhanVien(int ID_NhanVien)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_NhanVien", ID_NhanVien)
        };

                return (helper.ExecuteNonQuery("sp_QL_DeleteAllKhachHangTheoNhanVien_Kendo", pars) != 0);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }

        public bool CapNhatKhachHangChoNhanVien(int ID_NhanVien, int ID_KhachHang)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_NhanVien", ID_NhanVien),
                new SqlParameter("ID_KhachHang", ID_KhachHang)
                };

                return (helper.ExecuteNonQuery("sp_QL_PhanQuyenKhachHang_Kendo", pars) != 0);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }

        public bool XoaPhanQuyenKhachHangChoNhanVien(int ID_NhanVien, int ID_KhachHang)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_NhanVien", ID_NhanVien),
                new SqlParameter("ID_KhachHang", ID_KhachHang)
                };

                return (helper.ExecuteNonQuery("sp_QL_XoaPhanQuyenKhachHang_Kendo", pars) != 0);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }

        public bool CapNhatNhanVien(NhanVien nv, int ID_QuanLy)
        {
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_NhanVien", nv.IDNV),
                new SqlParameter("ID_QLLH", nv.IDQLLH),
                new SqlParameter("TenNhanVien", nv.TenDayDu),
                new SqlParameter("TenDangNhap", nv.TenDangNhap),
                new SqlParameter("MatKhau", ( nv.MatKhau != null && nv.MatKhau != "" )? Utils.md5(nv.MatKhau) : ""),
                new SqlParameter("TrangThai", nv.TrangThai),
                new SqlParameter("DiaChi", nv.DiaChi),
                new SqlParameter("QueQuan", nv.QueQuan),
                new SqlParameter("NgaySinh", nv.NgaySinh),
                new SqlParameter("Email", nv.Email),
                new SqlParameter("DienThoai", nv.DienThoai),
                new SqlParameter("ID_QuanLy", nv.ID_QuanLy),
                new SqlParameter("ID_Nhom", nv.ID_Nhom),
                new SqlParameter("ID_NhomKhachHang_MacDinh", nv.ID_NhomKhachHang_MacDinh),
                new SqlParameter("TruongNhom", nv.TruongNhom),
                new SqlParameter("GioiTinh", nv.GioiTinh),
                new SqlParameter("ID_ChucVu", nv.ID_ChucVu),
                new SqlParameter("ChucVu", nv.ChucVu),
                new SqlParameter("ChkDoiMatKhau", nv.TrucTuyen),
                new SqlParameter("LastUpdate_ID_QuanLy", ID_QuanLy)
            };

                if (helper.ExecuteNonQuery("sp_QL_UpdateNhanVien_New_v1", pars) != 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
        }

        public List<NhanVien> GetDSNhanVien(int IDQLLH, int ID_QuanLy)
        {
            List<NhanVien> dsnv = new List<NhanVien>();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", IDQLLH),
                new SqlParameter("ID_QuanLy", ID_QuanLy)
            };

            DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSNhanVien_Kendo", pars);
            DataTable dt = ds.Tables[0];
            try
            {

                foreach (DataRow dr in dt.Rows)
                {
                    NhanVien nhanvien = new NhanVien();
                    nhanvien.IDNV = int.Parse(dr["ID_NhanVien"].ToString());
                    nhanvien.TenDayDu = dr["TenNhanVien"].ToString();
                    nhanvien.QueQuan = dr["QueQuan"].ToString();
                    nhanvien.ChucVu = dr["ChucVu"].ToString();
                    dsnv.Add(nhanvien);
                }


            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);


            }
            return dsnv;
        }

        public List<NhanVien> GetDataNhanVien_TheoNhomQuanLy(int IDQLLH, int idNhom)
        {
            List<NhanVien> dsnv = new List<NhanVien>();

            NhanVien_dl NVDB = new NhanVien_dl();
            DataTable dt = NVDB.GetDataNhanVien_TheoNhomQuanLy(IDQLLH, idNhom);
            try
            {

                foreach (DataRow dr in dt.Rows)
                {
                    NhanVien nhanvien = new NhanVien();
                    nhanvien.IDNV = int.Parse(dr["ID_NhanVien"].ToString());
                    nhanvien.TenDayDu = dr["TenNhanVien"].ToString();
                    dsnv.Add(nhanvien);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);


            }
            return dsnv;
        }


        public int DangNhap(string MaCongTy, string tendangnhap, string matkhau, out int ID_QLLH, out int ID_NhanVien)
        {
            int rs = 0;
            ID_QLLH = 0;
            ID_NhanVien = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@macongty", MaCongTy),
                new SqlParameter("@taikhoan",  tendangnhap),
                new SqlParameter("@matkhau", md5(matkhau))
                };

                DataTable dtU = helper.ExecuteDataSet("sp_NhanVien_DangNhap_From_WEB", param).Tables[0];
                if (dtU.Rows.Count > 0)
                {
                    DataRow dr = dtU.Rows[0];
                    int returncode = int.Parse(dr["returncode"].ToString()); //1 : ok, 2: sai ma cong ty ,3 : het han hop dong, 4 : so nhan vien khai bao vuot qua
                    ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
                    ID_NhanVien = int.Parse(dr["ID_NhanVien"].ToString());
                    rs = returncode;//ok
                }
                else
                {
                    rs = 0;// sai mat khau / username
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return rs;
        }

        public static byte[] encryptData(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }
        public static string md5(string data)
        {
            return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();
        }

        public int DangNhap_QuanLy(string MaCongTy, string tendangnhap, string matkhau, out int ID_QLLH, out int ID_QuanLy)
        {
            int rs = 0;
            ID_QLLH = 0;
            ID_QuanLy = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@macongty", MaCongTy),
                new SqlParameter("@taikhoan",  tendangnhap),
                new SqlParameter("@matkhau", md5(matkhau))
                //new SqlParameter("@matkhau", matkhau)
                };

                DataTable dtU = helper.ExecuteDataSet("sp_TaiKhoan_DangNhap", param).Tables[0];
                if (dtU.Rows.Count > 0)
                {
                    DataRow dr = dtU.Rows[0];
                    int returncode = int.Parse(dr["returncode"].ToString()); //1 : ok, 2: sai ma cong ty ,3 : het han hop dong, 4 : so nhan vien khai bao vuot qua
                    ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
                    ID_QuanLy = int.Parse(dr["ID_QuanLy"].ToString());
                    rs = returncode;//ok
                }
                else
                {
                    rs = 0;// sai mat khau / username
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return rs;
        }

        public bool LuuHinhThucTT_NhanVien(NhanVienHinhThucTTModels obj)
        {
            bool ret = false;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@IDNV",  obj.ID_NhanVien),
                    new SqlParameter("@IDHTTT",  obj.ID_HTTT),
                };
                int i = helper.ExecuteNonQuery("sp_NhanVien_HinhThucThanhToan", pars);
                if (i > 0)
                {
                    ret = true;
                }

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return ret;
        }

        public bool XoaHinhThucTT_NhanVien(int IDNV)
        {
            bool ret = false;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@IDNV",  IDNV),
                };
                int i = helper.ExecuteNonQuery("sp_NhanVien_HinhThucThanhToan_DeleteAll", pars);
                if (i > 0)
                {
                    ret = true;
                }

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return ret;
        }

        public List<HinhThucThanhToanModel> getListHinhThucThanhToan(int IDNV)
        {
            List<HinhThucThanhToanModel> httt = new List<HinhThucThanhToanModel>();

            try
            {
                SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@IDNV",  IDNV)
                };

                DataTable dt = helper.ExecuteDataSet("sp_NhanVien_HinhThucThanhToan_List", pars).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    HinhThucThanhToanModel item = GetObjectFromDataRowUtil<HinhThucThanhToanModel>.ToOject(dr);
                    httt.Add(item);
                }
                return httt;

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return httt;
            }
        }
    }
}