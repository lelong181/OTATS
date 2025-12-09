using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for NhanVien_dl
/// </summary>
public class NhanVien_dl
{
    private SqlDataHelper helper;
    log4net.ILog log = log4net.LogManager.GetLogger(typeof(NhanVien_dl));
    private static SqlDataHelper db = new SqlDataHelper();

    public NhanVien_dl()
    {
        //
        // TODO: Add constructor logic here
        //
        helper = new SqlDataHelper();
    }

    public NhanVien GetNhanVienFromDataRow(DataRow dr)
    {
        NhanVien nv = new NhanVien();
        try
        {

            nv.IDNV = int.Parse(dr["ID_NhanVien"].ToString());
            nv.IDQLLH = int.Parse(dr["ID_QLLH"].ToString());
            nv.TenDangNhap = dr["TenDangNhap"].ToString();
            nv.TenDayDu = dr["TenNhanVien"].ToString();
            nv.MatKhau = dr["MatKhau"].ToString();
            nv.DiaChi = dr["DiaChi"].ToString();
            nv.QueQuan = dr["QueQuan"].ToString();
            nv.ChucVu = dr.Table.Columns.Contains("ChucVu") ? dr["ChucVu"].ToString() : "";
            nv.DienThoai = dr["DienThoai"].ToString();
            //try
            //{
            //    nv.PhienBan = dr["versionId"].ToString();
            //}
            //catch (Exception ex)
            //{
            //    nv.PhienBan = "";
            //}
            //try
            //{
            //    if (Convert.ToInt32(dr["hinhthucdangxuat"].ToString()) == 1)
            //    {
            //        nv.HinhThucDangXuat = "Tự động đăng xuất";
            //    }
            //    if (Convert.ToInt32(dr["hinhthucdangxuat"].ToString()) == 0)
            //    {
            //        nv.HinhThucDangXuat = "Người dùng đăng xuất";
            //    }
            //    if (Convert.ToInt32(dr["hinhthucdangxuat"].ToString()) == -1)
            //    {
            //        nv.HinhThucDangXuat = "   ";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    nv.HinhThucDangXuat = "   ";
            //}

            //try
            //{
            //    if (dr["TinhTrangPin"].ToString() == "")
            //    {
            //        nv.TinhTrangPin = "--%";
            //    }
            //    else
            //        nv.TinhTrangPin = dr["TinhTrangPin"] + "%";
            //}
            //catch (Exception ex)
            //{
            //    nv.TinhTrangPin = "--%";
            //}
            nv.TruongNhom = String.IsNullOrEmpty(dr["TruongNhom"].ToString()) ? 0 : Convert.ToInt32(dr["TruongNhom"]);
            nv.NgaySinh = String.IsNullOrEmpty(dr["NgaySinh"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["NgaySinh"]);
            //nv.NgaySinh = String.IsNullOrEmpty(dr["NgaySinh"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(dr["NgaySinh"]);
            nv.Email = dr["Email"].ToString();
            nv.TrangThai = int.Parse(dr["TrangThaiNhanVien"].ToString());
            nv.ID_QuanLy = String.IsNullOrEmpty(dr["ID_QuanLy"].ToString()) ? 0 : Convert.ToInt32(dr["ID_QuanLy"]);
            nv.ID_Nhom = String.IsNullOrEmpty(dr["ID_Nhom"].ToString()) ? 0 : Convert.ToInt32(dr["ID_Nhom"]);
            nv.ID_NhomKhachHang_MacDinh = String.IsNullOrEmpty(dr["ID_NhomKhachHang_MacDinh"].ToString()) ? 0 : Convert.ToInt32(dr["ID_NhomKhachHang_MacDinh"]);
            nv.TrucTuyen = String.IsNullOrEmpty(dr["dangtructuyen"].ToString()) ? 0 : Convert.ToInt32(dr["dangtructuyen"]);
            //if (nv.TrucTuyen == 0)
            //{
            //    if (Convert.ToInt32(dr["hinhthucdangxuat"].ToString()) == 1)
            //    {
            //        nv.TrangThaiTrucTuyen = "Tự động đăng xuất";
            //        nv.ThoiGianCapNhat = String.IsNullOrEmpty(dr["ThoiGianDangXuat"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianDangXuat"]);
            //    }
            //    else if (Convert.ToInt32(dr["hinhthucdangxuat"].ToString()) == 0)
            //    {
            //        nv.TrangThaiTrucTuyen = "Người dùng đăng xuất";
            //        nv.ThoiGianCapNhat = String.IsNullOrEmpty(dr["ThoiGianDangXuat"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianDangXuat"]);
            //    }
            //    else if (Convert.ToInt32(dr["hinhthucdangxuat"].ToString()) == -1)
            //    {
            //        nv.TrangThaiTrucTuyen = "Ngoại tuyến";
            //        nv.ThoiGianCapNhat = String.IsNullOrEmpty(dr["ThoiGianBanTinCuoi"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianBanTinCuoi"]);
            //    }


            //}
            //else if (nv.TrucTuyen == 1)
            //{
            //    nv.TrangThaiTrucTuyen = "Trực tuyến";
            //    nv.ThoiGianCapNhat = String.IsNullOrEmpty(dr["ThoiGianBanTinCuoi"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianBanTinCuoi"]);
            //}
            //else if (nv.TrucTuyen == 2)
            //{
            //    nv.TrangThaiTrucTuyen = "Mất kết nối";
            //    nv.ThoiGianCapNhat = String.IsNullOrEmpty(dr["ThoiGianBanTinCuoi"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianBanTinCuoi"]);
            //}
            //try
            //{
            //    nv.ThoiGianDangXuat = String.IsNullOrEmpty(dr["ThoiGianDangXuat"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianDangXuat"]);
            //}
            //catch (Exception ex)
            //{
            //    nv.ThoiGianDangXuat = new DateTime(1900, 1, 1);
            //}
            //try
            //{
            //    nv.ThoiGianGuiBanTinCuoiCung = String.IsNullOrEmpty(dr["ThoiGianBanTinCuoi"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianBanTinCuoi"]);
            //}
            //catch (Exception ex)
            //{
            //    nv.ThoiGianDangXuat = new DateTime(1900, 1, 1);
            //}

            try
            {

                //nv.KinhDo = String.IsNullOrEmpty(dr["KinhDo"].ToString()) ? 0 : Convert.ToDouble(dr["KinhDo"]);
                //nv.ViDo = String.IsNullOrEmpty(dr["ViDo"].ToString()) ? 0 : Convert.ToDouble(dr["ViDo"]);
                nv.TenNhom = dr.Table.Columns.Contains("TenNhom") ? dr["TenNhom"].ToString() : "";
            }
            catch (Exception ex)
            {
                log.Error(ex);


            }

            nv.DongMay = dr.Table.Columns.Contains("dongmay") ? dr["dongmay"].ToString() : "";
            nv.DoiMay = dr.Table.Columns.Contains("doimay") ? dr["doimay"].ToString() : "";
            nv.TenMay = dr.Table.Columns.Contains("devicename") ? dr["devicename"].ToString() : "";
            nv.OSVersion = dr.Table.Columns.Contains("osversion") ? dr["osversion"].ToString() : "";
            nv.Imei = dr.Table.Columns.Contains("imei") ? dr["imei"].ToString() : "";
            //if (dr["OS"].ToString() == "1")
            //{
            //    nv.OS = "IOS";
            //}
            //else if (dr["OS"].ToString() == "2")
            //{
            //    nv.OS = "Android";
            //}
            //else if (dr["OS"].ToString() == "2")
            //{
            //    nv.OS = "Không xác định";
            //}

            //if (dr["isFakeGPS"].ToString() == "1")
            //{
            //    nv.IsFakeGPS = true;
            //}
            //else
            //{
            //    nv.IsFakeGPS = false;
            //}

            //if (dr["isCheDoTietKiemPin"].ToString() == "1")
            //{
            //    nv.isCheDoTietKiemPin = true;
            //}
            //else
            //{
            //    nv.isCheDoTietKiemPin = false;
            //}

            nv.AnhDaiDien = dr.Table.Columns.Contains("AnhDaiDien") ? dr["AnhDaiDien"].ToString() : "";
            nv.AnhDaiDien_thumbnail_medium = dr.Table.Columns.Contains("AnhDaiDien_thumbnail_medium") ? dr["AnhDaiDien_thumbnail_medium"].ToString() : "";
            nv.AnhDaiDien_thumbnail_small = dr.Table.Columns.Contains("AnhDaiDien_thumbnail_small") ? dr["AnhDaiDien_thumbnail_small"].ToString() : "";

            //nv.AppFakeGPS = dr.Table.Columns.Contains("appfakegps") ? dr["appfakegps"].ToString() : "";
            nv.ID_ChucVu = dr.Table.Columns.Contains("ID_ChucVu") ? dr["ID_ChucVu"].ToString() != "" ? Convert.ToInt32(dr["ID_ChucVu"].ToString()) : 0 : 0;
            nv.GioiTinh = dr.Table.Columns.Contains("GioiTinh") ? dr["GioiTinh"].ToString() != "" ? Convert.ToInt32(dr["GioiTinh"].ToString()) : 0 : 0;
            try
            {
                nv.LastUpdate_ID_NhanVien = dr["LastUpdate_ID_NhanVien"].ToString() != "" ? Convert.ToInt32(dr["LastUpdate_ID_NhanVien"]) : 0;
                nv.LastUpdate_ID_QuanLy = dr["LastUpdate_ID_QuanLy"].ToString() != "" ? Convert.ToInt32(dr["LastUpdate_ID_QuanLy"]) : 0;
                nv.LastUpdate_ThoiGian_NhanVien = dr["LastUpdate_ThoiGian_NhanVien"].ToString() != "" ? Convert.ToDateTime(dr["LastUpdate_ThoiGian_NhanVien"]) : nv.LastUpdate_ThoiGian_NhanVien;
                nv.LastUpdate_ThoiGian_QuanLy = dr["LastUpdate_ThoiGian_QuanLy"].ToString() != "" ? Convert.ToDateTime(dr["LastUpdate_ThoiGian_QuanLy"]) : nv.LastUpdate_ThoiGian_QuanLy;
                nv.LastUpdate_Ten_NhanVien = dr["LastUpdate_Ten_NhanVien"].ToString();
                nv.LastUpdate_Ten_QuanLy = dr["LastUpdate_Ten_QuanLy"].ToString();

            }
            catch (Exception)
            {

            }
        }
        catch (Exception ex)
        {
            log.Error(ex);


        }
        return nv;
    }

    /// <summary>
    /// v1.9.9.3 : Lấy danh sách nhân viên : Trạng thái trực tuyến theo trạng thai kết nôi
    /// </summary>
    /// <param name="dr"></param>
    /// <returns></returns>
    public NhanVien GetNhanVienFromDataRow_v3(DataRow dr)
    {
        NhanVien nv = new NhanVien();
        try
        {

            nv.IDNV = int.Parse(dr["ID_NhanVien"].ToString());
            nv.IDQLLH = int.Parse(dr["ID_QLLH"].ToString());
            nv.TenDangNhap = dr["TenDangNhap"].ToString();
            nv.TenDayDu = dr["TenNhanVien"].ToString();
            nv.MatKhau = dr["MatKhau"].ToString();
            nv.DiaChi = dr["DiaChi"].ToString();
            nv.QueQuan = dr["QueQuan"].ToString();
            nv.DienThoai = dr["DienThoai"].ToString();
            try
            {
                nv.PhienBan = dr["versionId"].ToString();
            }
            catch (Exception ex)
            {
                nv.PhienBan = "";
            }
            try
            {
                if (Convert.ToInt32(dr["hinhthucdangxuat"].ToString()) == 1)
                {
                    nv.HinhThucDangXuat = "Tự động đăng xuất";
                }
                if (Convert.ToInt32(dr["hinhthucdangxuat"].ToString()) == 0)
                {
                    nv.HinhThucDangXuat = "Người dùng đăng xuất";
                }
                if (Convert.ToInt32(dr["hinhthucdangxuat"].ToString()) == -1)
                {
                    nv.HinhThucDangXuat = "   ";
                }
            }
            catch (Exception ex)
            {
                nv.HinhThucDangXuat = "   ";
            }

            try
            {
                if (dr["TinhTrangPin"].ToString() == "")
                {
                    nv.TinhTrangPin = "--%";
                }
                else
                    nv.TinhTrangPin = dr["TinhTrangPin"] + "%";
            }
            catch (Exception ex)
            {
                nv.TinhTrangPin = "--%";
            }
            nv.TruongNhom = String.IsNullOrEmpty(dr["TruongNhom"].ToString()) ? 0 : Convert.ToInt32(dr["TruongNhom"]);
            nv.NgaySinh = String.IsNullOrEmpty(dr["NgaySinh"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["NgaySinh"]);
            //nv.NgaySinh = String.IsNullOrEmpty(dr["NgaySinh"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(dr["NgaySinh"]);
            nv.Email = dr["Email"].ToString();
            nv.TrangThai = int.Parse(dr["TrangThaiNhanVien"].ToString());
            nv.ID_QuanLy = String.IsNullOrEmpty(dr["ID_QuanLy"].ToString()) ? 0 : Convert.ToInt32(dr["ID_QuanLy"]);
            nv.ID_Nhom = String.IsNullOrEmpty(dr["ID_Nhom"].ToString()) ? 0 : Convert.ToInt32(dr["ID_Nhom"]);
            nv.TrucTuyen = String.IsNullOrEmpty(dr["dangtructuyen"].ToString()) ? 0 : Convert.ToInt32(dr["dangtructuyen"]);//trang thai GPS
            nv.TrangThaiKetNoi = String.IsNullOrEmpty(dr["trangthaiketnoi"].ToString()) ? 0 : Convert.ToInt32(dr["trangthaiketnoi"]);//trang thai kết nối
            if (nv.TrangThaiKetNoi == 1)
            {
                nv.TrangThaiKetNoi_Text = "Trực tuyến";
            }
            else
            {
                nv.TrangThaiKetNoi_Text = "Ngoại tuyến";
            }
            if (nv.TrucTuyen == 0)
            {
                if (Convert.ToInt32(dr["hinhthucdangxuat"].ToString()) == 1)
                {
                    nv.TrangThaiTrucTuyen = "Tự động đăng xuất";
                    nv.ThoiGianCapNhat = String.IsNullOrEmpty(dr["ThoiGianDangXuat"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianDangXuat"]);
                }
                else if (Convert.ToInt32(dr["hinhthucdangxuat"].ToString()) == 0)
                {
                    nv.TrangThaiTrucTuyen = "Người dùng đăng xuất";
                    nv.ThoiGianCapNhat = String.IsNullOrEmpty(dr["ThoiGianDangXuat"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianDangXuat"]);
                }
                else if (Convert.ToInt32(dr["hinhthucdangxuat"].ToString()) == -1)
                {
                    nv.TrangThaiTrucTuyen = "Ngoại tuyến";
                    nv.ThoiGianCapNhat = String.IsNullOrEmpty(dr["ThoiGianBanTinCuoi"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianBanTinCuoi"]);
                }


            }
            else if (nv.TrucTuyen == 1)
            {
                nv.TrangThaiTrucTuyen = "Định vị ổn định";
                nv.ThoiGianCapNhat = String.IsNullOrEmpty(dr["ThoiGianBanTinCuoi"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianBanTinCuoi"]);
            }
            else if (nv.TrucTuyen == 2)
            {
                nv.TrangThaiTrucTuyen = "Định vị mất kết nối";
                nv.ThoiGianCapNhat = String.IsNullOrEmpty(dr["ThoiGianBanTinCuoi"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianBanTinCuoi"]);
            }
            try
            {
                nv.ThoiGianDangXuat = String.IsNullOrEmpty(dr["ThoiGianDangXuat"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianDangXuat"]);
            }
            catch (Exception ex)
            {
                nv.ThoiGianDangXuat = new DateTime(1900, 1, 1);
            }
            try
            {
                nv.ThoiGianGuiBanTinCuoiCung = String.IsNullOrEmpty(dr["ThoiGianBanTinCuoi"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianBanTinCuoi"]);
            }
            catch (Exception ex)
            {
                nv.ThoiGianDangXuat = new DateTime(1900, 1, 1);
            }

            try
            {
                nv.KinhDo = String.IsNullOrEmpty(dr["KinhDo"].ToString()) ? 0 : Convert.ToDouble(dr["KinhDo"]);
                nv.ViDo = String.IsNullOrEmpty(dr["ViDo"].ToString()) ? 0 : Convert.ToDouble(dr["ViDo"]);
                nv.TenNhom = dr.Table.Columns.Contains("TenNhom") ? dr["TenNhom"].ToString() : "";
            }
            catch (Exception ex)
            {
                log.Error(ex);


            }

            nv.DongMay = dr.Table.Columns.Contains("dongmay") ? dr["dongmay"].ToString() : "";
            nv.DoiMay = dr.Table.Columns.Contains("doimay") ? dr["doimay"].ToString() : "";
            nv.TenMay = dr.Table.Columns.Contains("devicename") ? dr["devicename"].ToString() : "";
            nv.OSVersion = dr.Table.Columns.Contains("osversion") ? dr["osversion"].ToString() : "";
            nv.Imei = dr.Table.Columns.Contains("imei") ? dr["imei"].ToString() : "";
            nv.ChucVu = dr.Table.Columns.Contains("ChucVu") ? dr["ChucVu"].ToString() : "";
            if (dr["OS"].ToString() == "1")
            {
                nv.OS = "IOS";
            }
            else if (dr["OS"].ToString() == "2")
            {
                nv.OS = "Android";
            }
            else if (dr["OS"].ToString() == "2")
            {
                nv.OS = "Không xác định";
            }

            if (dr["isFakeGPS"].ToString() == "1")
            {
                nv.IsFakeGPS = true;
            }
            else
            {
                nv.IsFakeGPS = false;
            }

            if (dr["isCheDoTietKiemPin"].ToString() == "1")
            {
                nv.isCheDoTietKiemPin = true;
            }
            else
            {
                nv.isCheDoTietKiemPin = false;
            }

            nv.AnhDaiDien = dr.Table.Columns.Contains("AnhDaiDien") ? dr["AnhDaiDien"].ToString() : "";
            nv.AnhDaiDien_thumbnail_medium = dr.Table.Columns.Contains("AnhDaiDien_thumbnail_medium") ? dr["AnhDaiDien_thumbnail_medium"].ToString() : "";
            nv.AnhDaiDien_thumbnail_small = dr.Table.Columns.Contains("AnhDaiDien_thumbnail_small") ? dr["AnhDaiDien_thumbnail_small"].ToString() : "";

            nv.AppFakeGPS = dr.Table.Columns.Contains("appfakegps") ? dr["appfakegps"].ToString() : "";
            nv.ID_ChucVu = dr.Table.Columns.Contains("ID_ChucVu") ? dr["ID_ChucVu"].ToString() != "" ? Convert.ToInt32(dr["ID_ChucVu"].ToString()) : 0 : 0;
            nv.GioiTinh = dr.Table.Columns.Contains("GioiTinh") ? dr["GioiTinh"].ToString() != "" ? Convert.ToInt32(dr["GioiTinh"].ToString()) : 0 : 0;
            try
            {
                nv.LastUpdate_ID_NhanVien = dr["LastUpdate_ID_NhanVien"].ToString() != "" ? Convert.ToInt32(dr["LastUpdate_ID_NhanVien"]) : 0;
                nv.LastUpdate_ID_QuanLy = dr["LastUpdate_ID_QuanLy"].ToString() != "" ? Convert.ToInt32(dr["LastUpdate_ID_QuanLy"]) : 0;
                nv.LastUpdate_ThoiGian_NhanVien = dr["LastUpdate_ThoiGian_NhanVien"].ToString() != "" ? Convert.ToDateTime(dr["LastUpdate_ThoiGian_NhanVien"]) : nv.LastUpdate_ThoiGian_NhanVien;
                nv.LastUpdate_ThoiGian_QuanLy = dr["LastUpdate_ThoiGian_QuanLy"].ToString() != "" ? Convert.ToDateTime(dr["LastUpdate_ThoiGian_QuanLy"]) : nv.LastUpdate_ThoiGian_QuanLy;
                nv.LastUpdate_Ten_NhanVien = dr["LastUpdate_Ten_NhanVien"].ToString();
                nv.LastUpdate_Ten_QuanLy = dr["LastUpdate_Ten_QuanLy"].ToString();

            }
            catch (Exception)
            {

            }
        }
        catch (Exception ex)
        {
            log.Error(ex);


        }
        return nv;
    }

    public List<NhanVien> GetDSNhanVien(int IDQLLH, int ID_QuanLy)
    {
        List<NhanVien> dsnv = new List<NhanVien>();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSNhanVien", pars);
        DataTable dt = ds.Tables[0];


        try
        {

            foreach (DataRow dr in dt.Rows)
            {
                NhanVien nv = GetNhanVienFromDataRow(dr);
                dsnv.Add(nv);
            }


        }
        catch (Exception ex)
        {
            log.Error(ex);


        }
        return dsnv;
    }


    public List<NhanVien> GetDSNhanVien_TheoNhomQuanLy(int IDQLLH, int ID_Nhom)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_Nhom", ID_Nhom)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSNhanVien_TheoNhom", pars);
        DataTable dt = ds.Tables[0];



        try
        {
            List<NhanVien> dsnv = new List<NhanVien>();
            foreach (DataRow dr in dt.Rows)
            {
                NhanVien nv = GetNhanVienFromDataRow(dr);
                dsnv.Add(nv);
            }
            List<NhomOBJ> lstNhomCon = NhomDB.getDS_NhomCon_ById(ID_Nhom);
            if (lstNhomCon != null)
            {
                foreach (NhomOBJ nhom in lstNhomCon)
                {
                    List<NhanVien> lstNV = GetDSNhanVien_TheoNhomQuanLy(IDQLLH, nhom.ID_Nhom);
                    if (lstNV != null)
                    {
                        foreach (NhanVien n in lstNV)
                        {
                            dsnv.Add(n);
                        }
                    }
                }
            }
            return dsnv;
        }
        catch
        {
            return null;
        }
    }
    /// <summary>
    /// v.1.9.9.2 : Lấy danh sách trực tuyến theo bản tin kết nối
    /// </summary>
    /// <param name="IDQLLH"></param>
    /// <param name="ID_Nhom"></param>
    /// <returns></returns>
    public List<NhanVien> GetDSNhanVien_TheoNhomQuanLy_v3(int IDQLLH, int ID_Nhom)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_Nhom", ID_Nhom)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSNhanVien_TheoNhom_v3", pars);
        DataTable dt = ds.Tables[0];



        try
        {
            List<NhanVien> dsnv = new List<NhanVien>();
            foreach (DataRow dr in dt.Rows)
            {
                NhanVien nv = GetNhanVienFromDataRow_v3(dr);
                dsnv.Add(nv);
            }
            List<NhomOBJ> lstNhomCon = NhomDB.getDS_NhomCon_ById(ID_Nhom);
            if (lstNhomCon != null)
            {
                foreach (NhomOBJ nhom in lstNhomCon)
                {
                    List<NhanVien> lstNV = GetDSNhanVien_TheoNhomQuanLy_v3(IDQLLH, nhom.ID_Nhom);
                    if (lstNV != null)
                    {
                        foreach (NhanVien n in lstNV)
                        {
                            dsnv.Add(n);
                        }
                    }
                }
            }
            return dsnv;
        }
        catch
        {
            return null;
        }
    }
    public int GetIDNV(string Username, int ID_QLLH)
    {
        SqlParameter idnvOut = new SqlParameter();
        idnvOut.Direction = ParameterDirection.Output;
        idnvOut.DbType = DbType.Int32;
        idnvOut.ParameterName = "idnv";

        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("usr", Username),
            new SqlParameter("ID_QLLH", ID_QLLH),
            idnvOut
        };

        if (helper.ExecuteNonQuery("getIdNhanVien", pars) != 0)
        {
            int idnv = int.Parse(idnvOut.Value.ToString());
            return idnv;
        }
        else
        {
            return 0;
        }
    }

    public int CheckChuyenKH(int IDNV)
    {
        SqlParameter soluong = new SqlParameter();
        soluong.Direction = ParameterDirection.Output;
        soluong.DbType = DbType.Int32;
        soluong.ParameterName = "soluong";
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("IDNV", IDNV),
            soluong
        };

        int sl = (int)helper.ExecuteScalar("sp_QL_CheckChuyenKH", pars);

        try
        {
            return sl;
        }
        catch
        {
            return 0;
        }
    }

    public NhanVien GetNVTheoID(int IDNV)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("IDNV", IDNV)
        };

        DataSet ds = helper.ExecuteDataSet("getNhanVienTheoID", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            DataRow dr = dt.Rows[0];
            NhanVien nv = GetNhanVienFromDataRow(dr);
            return nv;
        }
        catch
        {
            return null;
        }
    }

    public NhanVien GetNVTheoUserName(string UserName, int ID_QLLH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("TenDangNhap", UserName),
            new SqlParameter("ID_QLLH", ID_QLLH)
        };
        try
        {
            DataSet ds = helper.ExecuteDataSet("getNhanVienTheoUsername", pars);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return null;


            DataRow dr = dt.Rows[0];
            NhanVien nv = GetNhanVienFromDataRow(dr);
            return nv;
        }
        catch
        {
            return null;
        }
    }

    public int GetLastNhanVienID(int ID_QLLH)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH)
        };
            return Convert.ToInt32(helper.ExecuteScalar("sp_QL_GetLastNhanVienID", pars));
        }
        catch
        {
            return 0;
        }
    }


    public bool ThemNhanVien(NhanVien nv)
    {
        try
        {
            SqlParameter IDNV = new SqlParameter();
            IDNV.Direction = ParameterDirection.Output;
            IDNV.DbType = DbType.Int32;
            IDNV.ParameterName = "IDNV";
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_NhanVien", int.Parse("0")),
            new SqlParameter("ID_QLLH", nv.IDQLLH),
            new SqlParameter("TenNhanVien", nv.TenDayDu),
            new SqlParameter("TenDangNhap", nv.TenDangNhap),
            new SqlParameter("MatKhau", Utils.md5(nv.MatKhau)),
            new SqlParameter("DiaChi", nv.DiaChi),
            new SqlParameter("QueQuan", nv.QueQuan),
            new SqlParameter("NgaySinh", nv.NgaySinh),
            new SqlParameter("Email", nv.Email),
            new SqlParameter("DienThoai", nv.DienThoai),
            new SqlParameter("ID_QuanLy", nv.ID_QuanLy),
               new SqlParameter("ID_Nhom", nv.ID_Nhom),
               new SqlParameter("ID_NhomKhachHang_MacDinh", nv.ID_NhomKhachHang_MacDinh),
                new SqlParameter("GioiTinh", nv.GioiTinh),
                 new SqlParameter("ID_ChucVu", nv.ID_ChucVu),
                 new SqlParameter("ChucVu", nv.ChucVu),
                new SqlParameter("TruongNhom", nv.TruongNhom),
            IDNV

            };

            //if (helper.ExecuteNonQuery("insertnhanvienbanhang", pars) != 0)
            if (helper.ExecuteNonQuery("sp_QL_InsertNhanVien", pars) > 0)
            {
                // them thanh cong
                return true;
            }
            else
            {
                // them that bai
                return false;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
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
                new SqlParameter("LastUpdate_ID_QuanLy", ID_QuanLy),
            };

            if (helper.ExecuteNonQuery("sp_QL_UpdateNhanVien", pars) != 0)
            {
                // cap nhat thanh cong
                return true;
            }
            else
            {
                // cap nhat that bai
                return false;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
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

            if (helper.ExecuteNonQuery("sp_QL_CapNhatNhanVienChoKhachHang", pars) != 0)
            {
                // cap nhat thanh cong
                return true;
            }
            else
            {
                // cap nhat that bai
                return false;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return false;
        }
    }

    public bool BoKhachHangChoNhanVien(int ID_NhanVien)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_NhanVien", ID_NhanVien)
        };

            if (helper.ExecuteNonQuery("sp_QL_BoNhanVienChoKhachHang", pars) != 0)
            {
                // cap nhat thanh cong
                return true;
            }
            else
            {
                // cap nhat that bai
                return false;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return false;
        }
    }




    public void LuuLichSuChuyenGiaoKH(int ID_QLLH, int ID_NhanVien, int ID_KhachHang)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_KhachHang", ID_KhachHang)
        };

            helper.ExecuteNonQuery("sp_QL_LuuLichSuChuyenGiaoKhachHang", pars);
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
    }


    public bool DatLaiMatKhau(int IDNV, string matKhauMoi, int ID_QuanLy)
    {
        NhanVien nv = this.GetNVTheoID(IDNV);
        if (nv == null)
        {
            return false;
        }
        nv.MatKhau = matKhauMoi;//.ToUpper() ;

        if (this.CapNhatNhanVien(nv, ID_QuanLy))
        {
            return true;
        }

        return false;
    }

    public string ResetMatKhauNV(int IDNV, int IDQLLH, int ID_QuanLy)
    {
        NhanVien nv = this.GetNVTheoID(IDNV);
        if (nv == null)
        {
            return "";
        }
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string mkmoi = "";
        for (int i = 0; i < 6; i++)
        {
            mkmoi += chars[random.Next(chars.Length)];
        }
        //mkmoi = Util.GetRandomString().Substring(0, 6);

        nv.MatKhau = mkmoi;//.ToUpper() ;

        if (this.CapNhatNhanVien(nv, ID_QuanLy))
        {
            return mkmoi;
        }
        return "";
    }

    public bool DeleteNV(int IDNV)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("IDNV", IDNV)
        };

            if (helper.ExecuteNonQuery("deletenhanvienbanhang", pars) != 0)
            {
                // xoa thanh cong
                return true;
            }
            else
            {
                // xoa that bai
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
    public bool KhoiPhucNhanVienDaXoa(int IDNV)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_NhanVien", IDNV)
        };

            if (helper.ExecuteNonQuery("sp_NhanVien_KhoiPhucNhanVienDaXoa", pars) != 0)
            {
                // xoa thanh cong
                return true;
            }
            else
            {
                // xoa that bai
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
    public static string TenNhanVien(int idnv)
    {

        SqlParameter[] param = new SqlParameter[] {
            new SqlParameter("@idnv", idnv),
        };

        return db.ExecuteScalar("sp_GetTenNhanVien", param).ToString();

    }
    public static List<NhanVien> TimTheoTenNhanVien(string TenNhanVien, int idct, int ID_QuanLy)
    {
        List<NhanVien> rs = new List<NhanVien>();
        if (TenNhanVien != "")
        {
            SqlParameter[] param = new SqlParameter[] {
            new SqlParameter("@idct", idct),
            new SqlParameter("@ten", TenNhanVien),
            new SqlParameter("@ID_QuanLy", ID_QuanLy)
        };

            DataTable dt = db.ExecuteDataSet("sp_QL_TimKiemNhanVienTheoTen", param).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                NhanVien nv = new NhanVien();
                try
                {
                    nv.IDNV = int.Parse(dr["ID_NhanVien"].ToString());
                    nv.IDQLLH = int.Parse(dr["ID_QLLH"].ToString());
                    nv.TenDangNhap = dr["TenDangNhap"].ToString();
                    nv.TenDayDu = dr["TenNhanVien"].ToString();
                    nv.MatKhau = dr["MatKhau"].ToString();
                    nv.DiaChi = dr["DiaChi"].ToString();
                    nv.QueQuan = dr["QueQuan"].ToString();
                    nv.NgaySinh = String.IsNullOrEmpty(dr["NgaySinh"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["NgaySinh"]);
                    //nv.NgaySinh = String.IsNullOrEmpty(dr["NgaySinh"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(dr["NgaySinh"]);
                    nv.Email = dr["Email"].ToString();
                    nv.ThoiGianHoatDong = DateTime.Parse(dr["ThoiGianHoatDong"].ToString());
                    nv.TrucTuyen = int.Parse(dr["dangtructuyen"].ToString());
                }
                catch
                {
                    nv.IDNV = 0;
                    nv.IDQLLH = 0;
                    nv.TenDangNhap = "Không xác định";
                    nv.TenDayDu = "Không xác định";
                }
                rs.Add(nv);
            }
        }
        return rs;
    }
    public static List<NhanVien> TimKiemNhanVien(string input, int idct, int ID_QuanLy)
    {
        List<NhanVien> rs = new List<NhanVien>();

        SqlParameter[] param = new SqlParameter[] {
            new SqlParameter("@idct", idct),
            new SqlParameter("@ten", input),
            new SqlParameter("@ID_QuanLy", ID_QuanLy)
        };

        DataTable dt = db.ExecuteDataSet("sp_QL_TimKiemNhanVienTheoTen", param).Tables[0];
        foreach (DataRow dr in dt.Rows)
        {
            NhanVien nv = new NhanVien();
            try
            {
                nv.IDNV = int.Parse(dr["ID_NhanVien"].ToString());
                nv.IDQLLH = int.Parse(dr["ID_QLLH"].ToString());
                nv.TenDangNhap = dr["TenDangNhap"].ToString();
                nv.TenDayDu = dr["TenNhanVien"].ToString();
                nv.MatKhau = dr["MatKhau"].ToString();
                nv.DiaChi = dr["DiaChi"].ToString();
                nv.QueQuan = dr["QueQuan"].ToString();
                nv.NgaySinh = String.IsNullOrEmpty(dr["NgaySinh"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["NgaySinh"]);
                //nv.NgaySinh = String.IsNullOrEmpty(dr["NgaySinh"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(dr["NgaySinh"]);
                nv.Email = dr["Email"].ToString();
                nv.ThoiGianHoatDong = DateTime.Parse(dr["ThoiGianHoatDong"].ToString());
                nv.TrucTuyen = int.Parse(dr["dangtructuyen"].ToString());
            }
            catch
            {
                nv.IDNV = 0;
                nv.IDQLLH = 0;
                nv.TenDangNhap = "Không xác định";
                nv.TenDayDu = "Không xác định";
            }
            rs.Add(nv);
        }
        return rs;
    }

    public static List<NhanVien> DSNhanVienTheoTrangThai(int idct, string input, int ID_QuanLy)
    {
        List<NhanVien> rs = new List<NhanVien>();

        SqlParameter[] param = new SqlParameter[] {
            new SqlParameter("@idct", idct),
            new SqlParameter("@trangthai", input),
            new SqlParameter("@ID_QuanLy", ID_QuanLy)
        };

        DataTable dt = db.ExecuteDataSet("sp_QL_DanhSachNhanVienTheoTrangThai", param).Tables[0];
        foreach (DataRow dr in dt.Rows)
        {
            NhanVien nv = new NhanVien();
            try
            {
                nv.IDNV = int.Parse(dr["ID_NhanVien"].ToString());
                nv.IDQLLH = int.Parse(dr["ID_QLLH"].ToString());
                nv.TenDangNhap = dr["TenDangNhap"].ToString();
                nv.TenDayDu = dr["TenNhanVien"].ToString();
                nv.MatKhau = dr["MatKhau"].ToString();
                nv.DiaChi = dr["DiaChi"].ToString();
                nv.QueQuan = dr["QueQuan"].ToString();
                nv.NgaySinh = String.IsNullOrEmpty(dr["NgaySinh"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["NgaySinh"]);
                //nv.NgaySinh = String.IsNullOrEmpty(dr["NgaySinh"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(dr["NgaySinh"]);
                nv.Email = dr["Email"].ToString();
                nv.ThoiGianHoatDong = DateTime.Parse(dr["ThoiGianHoatDong"].ToString());
                nv.TrucTuyen = int.Parse(dr["dangtructuyen"].ToString());
                nv.KinhDo = dr["KinhDo"].ToString() != "" ? double.Parse(dr["KinhDo"].ToString()) : 0;
                nv.ViDo = dr["ViDo"].ToString() != "" ? double.Parse(dr["ViDo"].ToString()) : 0;
            }
            catch
            {
                nv.IDNV = 0;
                nv.IDQLLH = 0;
                nv.TenDangNhap = "Không xác định";
                nv.TenDayDu = "Không xác định";
            }
            rs.Add(nv);
        }
        return rs;
    }
    public bool ThoatDevice(int IDNV)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("IDNV", IDNV)
        };

            if (helper.ExecuteNonQuery("sp_QL_ThoatDevice", pars) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    public static NhanVien ChiTietNhanVienTheoIDNV(int IDNV)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("IDNV", IDNV)
        };

        DataSet ds = db.ExecuteDataSet("sp_QL_ChiTietNhanVienTheoIDNV", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        NhanVien nv = new NhanVien();
        try
        {
            DataRow dr = dt.Rows[0];
            nv.IDNV = int.Parse(dr["ID_NhanVien"].ToString());
            nv.IDQLLH = int.Parse(dr["ID_QLLH"].ToString());
            nv.TenDangNhap = dr["TenDangNhap"].ToString();
            nv.TenDayDu = dr["TenNhanVien"].ToString();
            nv.MatKhau = dr["MatKhau"].ToString();
            nv.DiaChi = dr["DiaChi"].ToString();
            nv.QueQuan = dr["QueQuan"].ToString();
            nv.NgaySinh = String.IsNullOrEmpty(dr["NgaySinh"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["NgaySinh"]);

            //nv.NgaySinh = String.IsNullOrEmpty(dr["NgaySinh"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(dr["NgaySinh"]);
            nv.Email = dr["Email"].ToString();
            nv.ThoiGianHoatDong = DateTime.Parse(dr["ThoiGianHoatDong"].ToString());
            nv.ThoiGianGuiBanTinCuoiCung = String.IsNullOrEmpty(dr["ThoiGianGuiBanTin"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianGuiBanTin"]);
            nv.TrucTuyen = int.Parse(dr["dangtructuyen"].ToString());
            try
            {
                nv.AnhDaiDien = dr["AnhDaiDien"].ToString();
                nv.AnhDaiDien_thumbnail_medium = dr["AnhDaiDien_thumbnail_medium"].ToString();
                nv.AnhDaiDien_thumbnail_small = dr["AnhDaiDien_thumbnail_small"].ToString();
                nv.TinhTrangPin = dr["TinhTrangPin"] + "%";
            }
            catch (Exception ex)
            {
                nv.TinhTrangPin = "";
            }
            nv.TrangThaiXoa = dr["TrangThaiXoa"].ToString() != "" ? int.Parse(dr["TrangThaiXoa"].ToString()) : 0;
            nv.NgayXoa = String.IsNullOrEmpty(dr["NgayXoa"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["NgayXoa"]);
            return nv;
        }
        catch
        {
            return null;
        }
    }

    public static NhanVien ChiTietNhanVienTheoTenDangNhap(string TenDangNhap, int ID_QLLH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("TenDangNhap", TenDangNhap),
            new SqlParameter("ID_QLLH", ID_QLLH)
        };

        DataSet ds = db.ExecuteDataSet("sp_QL_ChiTietNhanVienTheoTenDangNhap", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        NhanVien nv = new NhanVien();
        try
        {
            DataRow dr = dt.Rows[0];
            nv.IDNV = int.Parse(dr["ID_NhanVien"].ToString());
            nv.IDQLLH = int.Parse(dr["ID_QLLH"].ToString());
            nv.TenDangNhap = dr["TenDangNhap"].ToString();
            nv.TenDayDu = dr["TenNhanVien"].ToString();
            nv.MatKhau = dr["MatKhau"].ToString();
            nv.DiaChi = dr["DiaChi"].ToString();
            nv.QueQuan = dr["QueQuan"].ToString();
            nv.NgaySinh = String.IsNullOrEmpty(dr["NgaySinh"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["NgaySinh"]);

            //nv.NgaySinh = String.IsNullOrEmpty(dr["NgaySinh"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(dr["NgaySinh"]);
            nv.Email = dr["Email"].ToString();
            nv.ThoiGianHoatDong = DateTime.Parse(dr["ThoiGianHoatDong"].ToString());
            nv.ThoiGianGuiBanTinCuoiCung = String.IsNullOrEmpty(dr["ThoiGianGuiBanTin"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["ThoiGianGuiBanTin"]);
            nv.TrucTuyen = int.Parse(dr["dangtructuyen"].ToString());
            try
            {
                nv.TinhTrangPin = dr["TinhTrangPin"] + "%";
            }
            catch (Exception ex)
            {
                nv.TinhTrangPin = "";
            }
            nv.TrangThaiXoa = dr["TrangThaiXoa"].ToString() != "" ? int.Parse(dr["TrangThaiXoa"].ToString()) : 0;
            nv.NgayXoa = String.IsNullOrEmpty(dr["NgayXoa"].ToString()) ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["NgayXoa"]);
            return nv;
        }
        catch
        {
            return null;
        }
    }
    //public static byte[] encryptData(string data)
    //{
    //    System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
    //    byte[] hashedBytes;
    //    System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
    //    hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
    //    return hashedBytes;
    //}
    //public static string md5(string data)
    //{
    //    //return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();
    //    return data;
    //}

    public static DataSet PhienLamViec(int IDQLLH, int ID_NhanVien, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            ds = db.ExecuteDataSet("sp_NhanVien_GetPhienLamViec", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }
    public static DataSet LichSuOnlineOffine(int IDQLLH, int ID_NhanVien, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            ds = db.ExecuteDataSet("sp_NhanVien_LichSuOnlineOffline", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }
    public static List<PhienLamViecOBJ> PhienLamViec(int ID_NhanVien, DateTime dtFrom, DateTime dtTo)
    {
        List<PhienLamViecOBJ> lst = new List<PhienLamViecOBJ>();
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {

            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom.ToString("yyyy-MM-dd")),
            new SqlParameter("dtTo", dtTo.ToString("yyyy-MM-dd 23:59:59")),

        };
        try
        {
            ds = db.ExecuteDataSet("sp_NhanVien_GetPhienLamViec_TheoNhanVien", pars);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(GetPhienLamViecFromDataRow(dr));
            }
        }
        catch (Exception)
        {
            return null;
        }
        return lst;
    }

    public static PhienLamViecOBJ GetPhienLamViecFromDataRow(DataRow dr)
    {
        PhienLamViecOBJ rs = new PhienLamViecOBJ();
        try
        {
            rs.idnhanvien = int.Parse(dr["ID_NhanVien"].ToString());
            rs.thoigianbatdau = dr["thoigiandangnhap"].ToString() != "" ? DateTime.Parse(dr["thoigiandangnhap"].ToString()).ToString("yyyy-MM-ddTHH:mm:ss") : "";
            rs.thoigianketthuc = dr["thoigiandangxuatphien"].ToString() != "" ? DateTime.Parse(dr["thoigiandangxuatphien"].ToString()).ToString("yyyy-MM-ddTHH:mm:ss") : "";
            if (rs.thoigianketthuc == "")
            {
                rs.thoigianketthuc = dr["thoigiandangnhaptieptheo"].ToString() != "" ? DateTime.Parse(dr["thoigiandangnhaptieptheo"].ToString()).ToString("yyyy-MM-ddTHH:mm:ss") : "";
            }

        }
        catch (Exception ex)
        {

            rs.idnhanvien = 0;

        }
        return rs;
    }


    public static List<ChiTietPhienLamViecOBJ> PhienLamViecChiTiet(int ID_NhanVien, DateTime dtFrom, DateTime dtTo)
    {
        List<ChiTietPhienLamViecOBJ> lst = new List<ChiTietPhienLamViecOBJ>();
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {

            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom.ToString("yyyy-MM-dd HH:mm:ss")),
            new SqlParameter("dtTo", dtTo.ToString("yyyy-MM-dd HH:mm:ss")),

        };
        try
        {
            // ds = db.ExecuteDataSet("sp_NhanVien_GetPhienLamViec_By_ThoiGian", pars);
            ds = db.ExecuteDataSet("sp_NhanVien_GetPhienLamViec_ChiTiet", pars);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                lst.Add(GetPhienLamViecChiTietFromDataRow(dr));
            }
        }
        catch (Exception)
        {
            return null;
        }
        return lst;
    }
    public List<NhanVien> GetDSNhanVien_LoaiBoDanhDauXoa(int IDQLLH, int ID_QuanLy)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSNhanVien_LoaiDanhDauXoa", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<NhanVien> dsnv = new List<NhanVien>();
            foreach (DataRow dr in dt.Rows)
            {
                NhanVien nv = GetNhanVienFromDataRow(dr);
                dsnv.Add(nv);
            }

            return dsnv;
        }
        catch
        {
            return null;
        }
    }
    public static ChiTietPhienLamViecOBJ GetPhienLamViecChiTietFromDataRow(DataRow dr)
    {
        ChiTietPhienLamViecOBJ rs = new ChiTietPhienLamViecOBJ();
        try
        {
            rs.idnhanvien = int.Parse(dr["ID_NhanVien"].ToString());
            rs.thoigianbatdau = dr["ThoiGian"].ToString() != "" ? DateTime.Parse(dr["ThoiGian"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") : "";
            //rs.thoigianketthuc = dr["ThoiGianKetThuc"].ToString() != "" ? DateTime.Parse(dr["ThoiGianKetThuc"].ToString()).ToString("yyyy-MM-ddTHH:mm:ss") : "";
            rs.loai = dr["Loai"].ToString() != "" ? int.Parse(dr["Loai"].ToString()) : 0;
            rs.tenloai = dr["TenLoai"].ToString();
            rs.kinhdo = dr["KinhDo"].ToString() != "" ? double.Parse(dr["KinhDo"].ToString()) : 0;
            rs.vido = dr["ViDo"].ToString() != "" ? double.Parse(dr["ViDo"].ToString()) : 0;
            rs.accuracy = dr["accuracy"].ToString() != "" ? double.Parse(dr["accuracy"].ToString()) : 0;
        }
        catch (Exception ex)
        {

            rs.idnhanvien = 0;

        }
        return rs;
    }
    public List<NhanVien> GetDSNhanVien_TheoID_QLLH(int IDQLLH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", IDQLLH)
        };

        DataSet ds = helper.ExecuteDataSet("getDSNhanVien", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<NhanVien> dsnv = new List<NhanVien>();
            foreach (DataRow dr in dt.Rows)
            {
                NhanVien nv = new NhanVien();
                nv.TenDangNhap = dr["TenDangNhap"].ToString();
                nv.TenDayDu = dr["TenNhanVien"].ToString();
                nv.IDNV = int.Parse(dr["ID_NhanVien"].ToString());
                dsnv.Add(nv);
            }

            return dsnv;
        }
        catch
        {
            return null;
        }
    }
    public DataTable GetDataNhanVien_LoaiBoDanhDauXoa(int IDQLLH, int ID_QuanLy)
    {
        DataTable dt = null;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSNhanVien_LoaiDanhDauXoa", pars);
            dt = ds.Tables[0];





        }
        catch (Exception ex)
        {
            return null;
        }

        return dt;
    }

    public DataTable GetDataNhanVien_DangNhapApp(int IDQLLH, int ID_QuanLy)
    {
        DataTable dt = null;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSNhanVien_DangNhapApp", pars);
            dt = ds.Tables[0];





        }
        catch (Exception ex)
        {
            return null;
        }

        return dt;
    }

    public DataTable GetDataNhanVien_TheoNhomQuanLy(int IDQLLH, int ID_Nhom)
    {
        DataTable dt = null;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_Nhom", ID_Nhom)
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSNhanVien_TheoNhom_v2", pars);
            dt = ds.Tables[0];





        }
        catch (Exception ex)
        {
            return null;
        }

        return dt;
    }
    public DataTable GetNhanVien_DangNhapPhanMem(int IDQLLH, int ID_Nhom)
    {
        DataTable dt = null;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_Nhom", ID_Nhom)
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSNhanVien_DaDangNhapApp_TheoNhom", pars);
            dt = ds.Tables[0];





        }
        catch (Exception ex)
        {
            return null;
        }

        return dt;
    }
    public static DataSet PhienLamViec_Offline(int IDQLLH, int ID_NhanVien, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            ds = db.ExecuteDataSet("sp_NhanVien_GetPhienLamViec_v2", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }

    public static DataSet LichSuMatTinHieu(int IDQLLH, int ID_NhanVien, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            ds = db.ExecuteDataSet("sp_LichSuMatTinHieu_GetAll", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }

    public DataTable GetDataNhanVien_SuDungGanNhat(int IDQLLH)
    {
        DataTable dt = null;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH)
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSNhanVien_SuDungGanNhat", pars);
            dt = ds.Tables[0];





        }
        catch (Exception ex)
        {
            return null;
        }

        return dt;
    }
    public List<NhanVien> GetDSNhanVien_TheoKho(int IDQLLH, int ID_QuanLy, int ID_Kho)
    {
        List<NhanVien> dsnv = new List<NhanVien>();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_Kho", ID_Kho),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };

        DataSet ds = helper.ExecuteDataSet("sp_Kho_Get_NhanVienQL", pars);
        DataTable dt = ds.Tables[0];


        try
        {

            foreach (DataRow dr in dt.Rows)
            {
                NhanVien nv = GetNhanVienFromDataRow(dr);
                dsnv.Add(nv);
            }


        }
        catch (Exception ex)
        {
            log.Error(ex);


        }
        return dsnv;
    }






}