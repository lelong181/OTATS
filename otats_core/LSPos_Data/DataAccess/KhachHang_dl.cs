using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

/// <summary>
/// Summary description for KhachHang_dl
/// </summary>
public class KhachHang_dl
{
    private SqlDataHelper helper;
    public static SqlDataHelper db = new SqlDataHelper();
    public KhachHang_dl()
    {
        helper = new SqlDataHelper();
    }
    log4net.ILog log = log4net.LogManager.GetLogger(typeof(KhachHang_dl));
    public KhachHang GetKhachHangFromDataRow(DataRow dr)
    {
        try
        {
            KhachHang kh = new KhachHang();
            //
            kh.IDKhachHang = (dr["ID_KhachHang"] != null) ? int.Parse(dr["ID_KhachHang"].ToString()) : 0;
            kh.IDQLLH = (dr["ID_QLLH"] != null) ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
            kh.Ten = (dr["TenKhachHang"] != null) ? dr["TenKhachHang"].ToString() : "";
            kh.KinhDo = double.Parse(dr["KinhDo"].ToString());
            kh.Email = (dr["Email"] != null) ? dr["Email"].ToString() : "";
            kh.ViDo = double.Parse(dr["ViDo"].ToString());
            kh.MaKH = (dr["MaKH"] != null) ? dr["MaKH"].ToString() : "";
            kh.MaSoThue = dr["MaSoThue"].ToString();
            kh.DiaChi = (dr["DiaChi"] != null) ? dr["DiaChi"].ToString() : "";
            kh.ID_Tinh = (dr["ID_Tinh"].ToString() != "") ? int.Parse(dr["ID_Tinh"].ToString()) : 0;
            kh.ID_Quan = (dr["ID_Quan"].ToString() != "") ? int.Parse(dr["ID_Quan"].ToString()) : 0;
            kh.ID_Phuong = (dr["ID_Phuong"].ToString() != "") ? int.Parse(dr["ID_Phuong"].ToString()) : 0;
            kh.SoDienThoai = (dr["DienThoai"] != null) ? dr["DienThoai"].ToString() : "";
            kh.SoDienThoai2 = (dr["DienThoai2"] != null) ? dr["DienThoai2"].ToString() : "";
            kh.SoDienThoai3 = (dr["DienThoai3"] != null) ? dr["DienThoai3"].ToString() : "";
            kh.SoDienThoaiMacDinh = (dr["DienThoaiMacDinh"] != null) ? dr["DienThoaiMacDinh"].ToString() : "";
            kh.TenTinh = (dr["TenTinh"].ToString() != "") ? dr["TenTinh"].ToString() : "";
            kh.TenQuan = (dr["TenQuan"].ToString() != "") ? dr["TenQuan"].ToString() : "";
            kh.TenPhuong = (dr["TenPhuong"].ToString() != "") ? dr["TenPhuong"].ToString() : "";
            kh.NguoiLienHe = (dr["NguoiLienHe"] != DBNull.Value) ? dr["NguoiLienHe"].ToString() : "";
            kh.ID_NhanVien = (dr["ID_NhanVien"] != DBNull.Value) ? Convert.ToInt16(dr["ID_NhanVien"]) : 0;
            kh.TenNhanVien = (dr["TenNhanVien"] != DBNull.Value) ? dr["TenNhanVien"].ToString() : "";
            kh.SoTKNganHang = dr["SoTKNganHang"].ToString();
            kh.Fax = dr["Fax"].ToString();
            kh.Website = dr["Website"].ToString();
            kh.GhiChu = dr["GhiChu"].ToString();
            kh.TrangThai = Convert.ToInt32(dr["TrangThai"]);
            kh.NgayTao = dr["insertedtime"].ToString() == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["insertedtime"]);
            kh.ID_QuanLy = (dr["ID_QuanLy"] != DBNull.Value) ? int.Parse(dr["ID_QuanLy"].ToString()) : 0;
            kh.ID_LoaiKhachHang = (dr["ID_LoaiKhachHang"] != DBNull.Value) ? int.Parse(dr["ID_LoaiKhachHang"].ToString()) : 0;
            kh.DuongPho = (dr["DuongPho"] != null) ? dr["DuongPho"].ToString() : "";
            kh.danhsachanh = new List<ImageOBJ>();
            kh.danhsachanh = DanhSachAnh_ByIDKhachHang(int.Parse(dr["ID_KhachHang"].ToString()));
            kh.ID_NhomKH = (dr["ID_NhomKH"] != DBNull.Value) ? int.Parse(dr["ID_NhomKH"].ToString()) : 0;
            kh.ID_Cha = (dr["ID_Cha"] != DBNull.Value) ? int.Parse(dr["ID_Cha"].ToString()) : 0;
            kh.ID_KhuVuc = (dr["ID_KhuVuc"] != DBNull.Value ) ? int.Parse(dr["ID_KhuVuc"].ToString()) : 0;
            kh.GhiChuKhiXoa = dr["GhiChuKhiXoa"].ToString();
            kh.DiaChiXuatHoaDon = dr["DiaChiXuatHoaDon"].ToString();
            try
            {
                kh.LastUpdate_ID_NhanVien = dr["LastUpdate_ID_NhanVien"].ToString() != "" ? Convert.ToInt32(dr["LastUpdate_ID_NhanVien"]) : 0;
                kh.LastUpdate_ID_QuanLy = dr["LastUpdate_ID_QuanLy"].ToString() != "" ? Convert.ToInt32(dr["LastUpdate_ID_QuanLy"]) : 0;
                kh.LastUpdate_ThoiGian_NhanVien = dr["LastUpdate_ThoiGian_NhanVien"].ToString() != "" ? Convert.ToDateTime(dr["LastUpdate_ThoiGian_NhanVien"]) : kh.LastUpdate_ThoiGian_NhanVien;
                kh.LastUpdate_ThoiGian_QuanLy = dr["LastUpdate_ThoiGian_QuanLy"].ToString() != "" ? Convert.ToDateTime(dr["LastUpdate_ThoiGian_QuanLy"]) : kh.LastUpdate_ThoiGian_QuanLy;
                kh.LastUpdate_Ten_NhanVien = dr["LastUpdate_Ten_NhanVien"].ToString();
                kh.LastUpdate_Ten_QuanLy = dr["LastUpdate_Ten_QuanLy"].ToString();

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return kh;
        }
        catch
        {
            return null;
        }
    }
    public DataTable GetDataKhachHangAll(int IDQLLH, int ID_QuanLy, int ID_Tinh, int ID_Quan, int ID_LoaiKhachHang, int ID_KenhBanHang)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@id_qllh", IDQLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
              new SqlParameter("@ID_Tinh", ID_Tinh),
                new SqlParameter("@ID_Quan", ID_Quan),
                new SqlParameter("@ID_LoaiKhachHang", ID_LoaiKhachHang),
                new SqlParameter("@ID_KenhBanHang", ID_KenhBanHang)
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangTheoIDQL", pars);
            dt = ds.Tables[0];
            DataRow dr = dt.NewRow();
            dr["ID_KhachHang"] = 0;
            dr["TenKhachHang"] = "Tất cả";
            dt.Rows.InsertAt(dr, 0);

        }
        catch (Exception ex)
        {
            log.Error(ex);

        }


        return dt;
    }
    public DataTable GetDataKhachHangAll_Combo(int IDQLLH, int ID_QuanLy)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@id_qllh", IDQLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
           
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangTheoIDQL_ComBo", pars);
            dt = ds.Tables[0];
            DataRow dr = dt.NewRow();
            dr["ID_KhachHang"] = 0;
            dr["TenKhachHang"] = "Tất cả";
            dt.Rows.InsertAt(dr, 0);

        }
        catch (Exception ex)
        {
            log.Error(ex);

        }


        return dt;
    }
    public DataTable GetDataKhachHangAll_Grid(int IDQLLH, int ID_QuanLy, int ID_Tinh, int ID_Quan, int ID_LoaiKhachHang, int ID_KenhBanHang)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@id_qllh", IDQLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
              new SqlParameter("@ID_Tinh", ID_Tinh),
                new SqlParameter("@ID_Quan", ID_Quan),
                new SqlParameter("@ID_LoaiKhachHang", ID_LoaiKhachHang),
                new SqlParameter("@ID_KenhBanHang", ID_KenhBanHang)
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangTheoIDQL_Grid", pars);
            dt = ds.Tables[0];

        }
        catch (Exception ex)
        {
            log.Error(ex);

        }


        return dt;
    }

    //public List<KhachHang> GetKhachHangAll(int IDQLLH,int ID_QuanLy)
    //{
    //    SqlParameter[] pars = new SqlParameter[] {
    //        new SqlParameter("@id_qllh", IDQLLH),
    //        new SqlParameter("@ID_QuanLy", ID_QuanLy)
    //    };

    //    DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangTheoIDQL", pars);
    //    DataTable dt = ds.Tables[0];

    //    if (dt.Rows.Count == 0)
    //        return null;

    //    try
    //    {
    //        List<KhachHang> dskh = new List<KhachHang>();
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            KhachHang kh = GetKhachHangFromDataRow(dr);
    //            dskh.Add(kh);
    //        }

    //        return dskh;
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}

    public DataTable GetKhachHangDaCapQuyen(int ID_NhanVien, int ID_LoaiKhachHang, int ID_KenhBanHang, int ID_Tinh, int ID_Quan, int ID_Phuong)
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

            DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangDaCapQuyen", pars);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return new DataTable();
            else
                return dt;
        }
        catch
        {
            return null;
        }
    }
    public DataTable GetKhachHangDaCapQuyen_NhanVienTrucThuocNhom(int ID_NhanVien, int ID_LoaiKhachHang, int ID_KenhBanHang)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_NhanVien", ID_NhanVien),
                 new SqlParameter("ID_LoaiKhachHang", ID_LoaiKhachHang),
              new SqlParameter("ID_KenhBanHang", ID_KenhBanHang)

        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangDaCapQuyen_NhanVienTrucThuocNhomQuanLy", pars);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return null;
            else
                return dt;
        }
        catch
        {
            return null;
        }
    }
    public DataTable GetNhanVienQuanLyKhachHang(int ID_KhachHang)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_KhachHang", ID_KhachHang)
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_getDSNhanVienDaCapQuyen_TheoKhachHang", pars);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return null;
            else
                return dt;
        }
        catch
        {
            return null;
        }
    }

    public DataTable GetNhomQuanLyKhachHang(int ID_KhachHang)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_KhachHang", ID_KhachHang)
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_getDSNhomDaCapQuyen_TheoKhachHang", pars);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return null;
            else
                return dt;
        }
        catch
        {
            return null;
        }
    }

    public DataTable GetNhanVienChuaQuanLyKhachHang(int ID_KhachHang, int ID_QLLH, int ID_QuanLy)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_KhachHang", ID_KhachHang),
              new SqlParameter("ID_QLLH", ID_QLLH),
                new SqlParameter("ID_QuanLy", ID_QuanLy),
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_getDSNhanVienChuaCapQuyen_TheoKhachHang", pars);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return null;
            else
                return dt;
        }
        catch
        {
            return null;
        }
    }
    public DataTable GetKhachHangQuanLy(int ID_NhanVien)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_NhanVien", ID_NhanVien)
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangQuanLy", pars);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return null;
            else
                return dt;
        }
        catch
        {
            return null;
        }
    }

    public DataTable GetKhachHangChuaCapQuyen(int ID_NhanVien, int ID_QLLH, int ID_QuanLy, int ID_LoaiKhachHang, int ID_KenhBanHang, int ID_Tinh, int ID_Quan,int ID_Phuong)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_QLLH", ID_QLLH),
             new SqlParameter("ID_LoaiKhachHang", ID_LoaiKhachHang),
              new SqlParameter("ID_KenhBanHang", ID_KenhBanHang),
                        new SqlParameter("@ID_Tinh", ID_Tinh),
                new SqlParameter("@ID_Quan", ID_Quan),
                new SqlParameter("@ID_Phuong", ID_Phuong),


        };
            //sua ngay 25/11 : lay da danh sach khach hang chua phan quyen, bao gom ca cac khach hang duoc phan cho nhan vien trong nhóm nếu nhân viên này là trưởng nhóm
            //DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangChuaCapQuyen", pars);
            DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangChuaCapQuyen_BaoGomLoaiKhachHangCuaNhanVien", pars);

            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return null;
            else
                return dt;
        }
        catch
        {
            return null;
        }
    }

    public List<KhachHang> GetKhachHangChuaCapQuyen(int IDQLLH, int ID_QuanLy)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@id_qllh", IDQLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangTheoIDQL", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<KhachHang> dskh = new List<KhachHang>();
            foreach (DataRow dr in dt.Rows)
            {
                KhachHang kh = GetKhachHangFromDataRow(dr);
                dskh.Add(kh);
            }

            return dskh;
        }
        catch
        {
            return null;
        }
    }

    public List<KhachHang> GetKhachHangDuyet(int IDQLLH,int ID_QuanLy)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@id_qllh", IDQLLH),
             new SqlParameter("@ID_QuanLy", ID_QuanLy)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangDuyetTheoIDQL", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<KhachHang> dskh = new List<KhachHang>();
            foreach (DataRow dr in dt.Rows)
            {
                KhachHang kh = GetKhachHangFromDataRow(dr);
                dskh.Add(kh);
            }

            return dskh;
        }
        catch
        {
            return null;
        }
    }

    public static KhachHang GetKhachHangTheoID(int IDKH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@idkhachhang", IDKH)
        };

        DataSet ds = db.ExecuteDataSet("sp_QL_ChiTietKhachHangTheoID", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            DataRow dr = dt.Rows[0];
            KhachHang kh = new KhachHang();
            kh.IDKhachHang = (dr["ID_KhachHang"] != null) ? int.Parse(dr["ID_KhachHang"].ToString()) : 0;
            kh.IDQLLH = (dr["ID_QLLH"] != null) ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
            kh.Ten = (dr["TenKhachHang"] != null) ? dr["TenKhachHang"].ToString() : "";
            kh.KinhDo = double.Parse(dr["KinhDo"].ToString());
            kh.ViDo = double.Parse(dr["ViDo"].ToString());
            kh.TenDayDu = (dr["TenDayDu"] != null) ? dr["TenDayDu"].ToString() : "";
            kh.MaSoThue = dr["MaSoThue"].ToString();
            kh.DiaChi = (dr["DiaChi"] != null) ? dr["DiaChi"].ToString() : "";
            kh.ID_Tinh = (dr["ID_Tinh"].ToString() != "") ? int.Parse(dr["ID_Tinh"].ToString()) : 0;
            kh.ID_Quan = (dr["ID_Quan"].ToString() != "") ? int.Parse(dr["ID_Quan"].ToString()) : 0;
            kh.ID_Phuong = (dr["ID_Phuong"].ToString() != "") ? int.Parse(dr["ID_Phuong"].ToString()) : 0;
            kh.SoDienThoai = (dr["DienThoai"] != null) ? dr["DienThoai"].ToString() : "";
            kh.TenTinh = (dr["TenTinh"].ToString() != "") ? dr["TenTinh"].ToString() : "";
            kh.TenQuan = (dr["TenQuan"].ToString() != "") ? dr["TenQuan"].ToString() : "";
            kh.TenPhuong = (dr["TenPhuong"].ToString() != "") ? dr["TenPhuong"].ToString() : "";
            kh.NguoiLienHe = (dr["NguoiLienHe"] != null) ? dr["NguoiLienHe"].ToString() : "";
            kh.SoDienThoai = (dr["DienThoai"] != null) ? dr["DienThoai"].ToString() : "";
            kh.SoDienThoai2 = (dr["DienThoai2"] != null) ? dr["DienThoai2"].ToString() : "";
            kh.SoDienThoai3 = (dr["DienThoai3"] != null) ? dr["DienThoai3"].ToString() : "";
            kh.SoTKNganHang = dr["SoTKNganHang"].ToString();
            kh.Email = (dr["Email"] != null) ? dr["Email"].ToString() : "";
            kh.DuongPho = (dr["DuongPho"] != null) ? dr["DuongPho"].ToString() : "";
            kh.Fax = dr["Fax"].ToString();
            kh.Website = dr["Website"].ToString();
            kh.GhiChu = dr["GhiChu"].ToString();
            try
            {
                kh.TenNhanVien = NhanVien_dl.TenNhanVien((dr["ID_NhanVien"] != null) ? int.Parse(dr["ID_NhanVien"].ToString()) : 0);
            }
            catch
            {
                kh.TenNhanVien = "Không xác định";
            }
            kh.danhsachanh = DanhSachAnh_ByIDKhachHang(int.Parse(dr["ID_KhachHang"].ToString()));
            kh.Imgurl = (dr["Imgurl"].ToString() == null || dr["Imgurl"].ToString() == "") ? "images/noimg.png" : Utils.GiaiMa(WebConfigurationManager.AppSettings["SERVERIMAGE"]) + dr["Imgurl"].ToString();
            kh.Imgurl2 = (dr["Imgurl2"].ToString() == null || dr["Imgurl2"].ToString() == "") ? "images/noimg.png" : Utils.GiaiMa(WebConfigurationManager.AppSettings["SERVERIMAGE"]) + dr["Imgurl2"].ToString();
            kh.Imgurl3 = (dr["Imgurl3"].ToString() == null || dr["Imgurl3"].ToString() == "") ? "images/noimg.png" :Utils.GiaiMa( WebConfigurationManager.AppSettings["SERVERIMAGE"]) + dr["Imgurl3"].ToString();
            kh.Imgurl4 = (dr["Imgurl4"].ToString() == null || dr["Imgurl4"].ToString() == "") ? "images/noimg.png" : Utils.GiaiMa(WebConfigurationManager.AppSettings["SERVERIMAGE"]) + dr["Imgurl4"].ToString();
            kh.ID_QuanLy = (dr["ID_QuanLy"] != DBNull.Value) ? int.Parse(dr["ID_QuanLy"].ToString()) : 0;
            kh.ID_LoaiKhachHang = (dr["ID_LoaiKhachHang"] != DBNull.Value) ? int.Parse(dr["ID_LoaiKhachHang"].ToString()) : 0;
            kh.TenLoaiKhachHang = (dr["TenLoaiKhachHang"] != DBNull.Value) ?  dr["TenLoaiKhachHang"].ToString()  : "";
            try
            {
                kh.TenKenhBanHang = (dr["TenKenhBanHang"] != DBNull.Value) ? dr["TenKenhBanHang"].ToString() : "";
            }
            catch (Exception)
            {

                 
            }
            return kh;
        }
        catch
        {
            return null;
        }
    }

    public KhachHang GetKhachHangID(int IDKH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("idkh", IDKH)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_getKhachHangTheoID", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            DataRow dr = dt.Rows[0];
            KhachHang kh = GetKhachHangFromDataRow(dr);
            return kh;
        }
        catch
        {
            return null;
        }
    }
    public KhachHang GetKhachHangTheoMa(int ID_QLLH, string Ma, int ID_KhachHang)
    {
        SqlParameter[] par = new SqlParameter[]{

                new SqlParameter("@ID_QLLH", ID_QLLH),
                 new SqlParameter("@Ma", Ma),
                 new SqlParameter("@ID_KhachHang", ID_KhachHang)
            };

        DataSet ds = helper.ExecuteDataSet("sp_App_CuaHang_ByMaCuaHang", par);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            DataRow dr = dt.Rows[0];
            KhachHang kh = GetKhachHangFromDataRow(dr);
            return kh;
        }
        catch
        {
            return null;
        }
    }
    public bool ThemKhachHang(KhachHang kh)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_KhachHang", int.Parse("0")),
            new SqlParameter("ID_QLLH", kh.IDQLLH),
            new SqlParameter("TenKhachHang", kh.Ten),
            new SqlParameter("MaKH", kh.MaKH),
            new SqlParameter("KinhDo", kh.KinhDo),
            new SqlParameter("ViDo", kh.ViDo),
            new SqlParameter("ID_NhanVien", kh.ID_NhanVien),
            new SqlParameter("MaSoThue", kh.MaSoThue),
            new SqlParameter("DiaChi", kh.DiaChi),
            new SqlParameter("ID_Tinh", kh.ID_Tinh),
            new SqlParameter("ID_Quan", kh.ID_Quan),
            new SqlParameter("ID_Phuong", kh.ID_Phuong),
            new SqlParameter("DienThoai", kh.SoDienThoai),
            new SqlParameter("DienThoai2", kh.SoDienThoai2),
            new SqlParameter("DienThoai3", kh.SoDienThoai3),
            new SqlParameter("DienThoaiMacDinh", kh.SoDienThoaiMacDinh),
            new SqlParameter("NguoiLienHe", kh.NguoiLienHe),
            new SqlParameter("SoTKNganHang", kh.SoTKNganHang),
            new SqlParameter("Email", kh.Email),
            new SqlParameter("Fax", kh.Fax),
            new SqlParameter("Website", kh.Website),
            new SqlParameter("GhiChu", kh.GhiChu),
             new SqlParameter("DuongPho", kh.DuongPho),
             new SqlParameter("ID_QuanLy", kh.ID_QuanLy),
             new SqlParameter("ID_LoaiKhachHang", kh.ID_LoaiKhachHang),
              new SqlParameter("ID_NhomKH", kh.ID_NhomKH),
        };

            if (helper.ExecuteNonQuery("sp_QL_InsertKhachHang", pars) != 0)
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
        catch
        {
            return false;
        }
    }
    public int ThemKhachHangv2(KhachHang kh, int LastUpdate_ID_QuanLy)
    {
        int idKH = 0;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_KhachHang", int.Parse("0")),
            new SqlParameter("ID_QLLH", kh.IDQLLH),
            new SqlParameter("TenKhachHang", kh.Ten),
            new SqlParameter("MaKH", kh.MaKH),
            new SqlParameter("KinhDo", kh.KinhDo),
            new SqlParameter("ViDo", kh.ViDo),
            new SqlParameter("ID_NhanVien", kh.ID_NhanVien),
            new SqlParameter("MaSoThue", kh.MaSoThue),
            new SqlParameter("DiaChi", kh.DiaChi),
            new SqlParameter("ID_Tinh", kh.ID_Tinh),
            new SqlParameter("ID_Quan", kh.ID_Quan),
            new SqlParameter("ID_Phuong", kh.ID_Phuong),
            new SqlParameter("DienThoai", kh.SoDienThoai),
            new SqlParameter("DienThoai2", kh.SoDienThoai2),
            new SqlParameter("DienThoai3", kh.SoDienThoai3),
            new SqlParameter("DienThoaiMacDinh", kh.SoDienThoaiMacDinh),
            new SqlParameter("NguoiLienHe", kh.NguoiLienHe),
            new SqlParameter("SoTKNganHang", kh.SoTKNganHang),
            new SqlParameter("Email", kh.Email),
            new SqlParameter("Fax", kh.Fax),
            new SqlParameter("Website", kh.Website),
            new SqlParameter("GhiChu", kh.GhiChu),
             new SqlParameter("DuongPho", kh.DuongPho),
             new SqlParameter("ID_QuanLy", kh.ID_QuanLy),
             new SqlParameter("ID_LoaiKhachHang", kh.ID_LoaiKhachHang),
              new SqlParameter("ID_NhomKH", kh.ID_NhomKH),
                new SqlParameter("ID_Cha", kh.ID_Cha),
                new SqlParameter("ID_KhuVuc", kh.ID_KhuVuc),
                  new SqlParameter("LastUpdate_ID_QuanLy", LastUpdate_ID_QuanLy),
                  new SqlParameter("DiaChiXuatHoaDon", kh.DiaChiXuatHoaDon),
        };
            object obj = helper.ExecuteScalar("sp_QL_InsertKhachHang", pars);
            if (obj != null)
            {
                int x = int.Parse(obj.ToString());
                if(x > 1)
                {
                    idKH = x;
                }
                
            }
             
        }
        catch(Exception ex)
        {
            log.Error(ex);
        }
        return idKH;
    }
    public bool UpdateKhachHang(KhachHang kh, int LastUpdate_ID_QuanLy)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_KhachHang", kh.IDKhachHang),
            new SqlParameter("ID_QLLH", kh.IDQLLH),
            new SqlParameter("TenKhachHang", kh.Ten),
            new SqlParameter("MaKH", kh.MaKH),
            new SqlParameter("KinhDo", kh.KinhDo),
            new SqlParameter("ViDo", kh.ViDo),
            new SqlParameter("ID_NhanVien", kh.ID_NhanVien),
            new SqlParameter("MaSoThue", kh.MaSoThue),
            new SqlParameter("DiaChi", kh.DiaChi),
            new SqlParameter("ID_Tinh", kh.ID_Tinh),
            new SqlParameter("ID_Quan", kh.ID_Quan),
            new SqlParameter("ID_Phuong", kh.ID_Phuong),
            new SqlParameter("DienThoai", kh.SoDienThoai),
            new SqlParameter("DienThoai2", kh.SoDienThoai2),
            new SqlParameter("DienThoai3", kh.SoDienThoai3),
            new SqlParameter("DienThoaiMacDinh", kh.SoDienThoaiMacDinh),
            new SqlParameter("NguoiLienHe", kh.NguoiLienHe),
            new SqlParameter("SoTKNganHang", kh.SoTKNganHang),
            new SqlParameter("Email", kh.Email),
            new SqlParameter("Fax", kh.Fax),
            new SqlParameter("Website", kh.Website),
            new SqlParameter("GhiChu", kh.GhiChu),
            new SqlParameter("DuongPho", kh.DuongPho),
             new SqlParameter("ID_QuanLy", kh.ID_QuanLy),
             new SqlParameter("ID_LoaiKhachHang", kh.ID_LoaiKhachHang),
              new SqlParameter("ID_NhomKH", kh.ID_NhomKH),
              new SqlParameter("ID_KhuVuc", kh.ID_KhuVuc),
                 new SqlParameter("ID_Cha", kh.ID_Cha),
                 new SqlParameter("DiaChiXuatHoaDon", kh.DiaChiXuatHoaDon),
                 new SqlParameter("LastUpdate_ID_QuanLy", LastUpdate_ID_QuanLy),
                 
        };

            if (helper.ExecuteNonQuery("sp_QL_InsertKhachHang", pars) != 0)
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
        catch
        {
            return false;
        }
    }

    public bool DeleteKhachHang(int IDQL, int IDKH)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("IDKH", IDKH),
            new SqlParameter("ID_QLLH", IDQL)
        };

            if (helper.ExecuteNonQuery("sp_QL_DeleteKhachHang", pars) != 0)
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

    public bool DuyetKhachHang(int IDQL, int IDKH, int TrangThai)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("IDKH", IDKH),
            new SqlParameter("ID_QLLH", IDQL),
            new SqlParameter("TrangThai",TrangThai)
        };

            if (helper.ExecuteNonQuery("sp_QL_DuyetKhachHang", pars) != 0)
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




    public static DataTable DanhSachDaiLyTheoTinh(int tinhid)
    {
        DataTable dt = null;
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@tinhid", tinhid)
            };

            dt = db.ExecuteDataSet("sp_QL_DanhSachDaiLyTheoTinh", par).Tables[0];

        }
        catch { }
        return dt;
    }


    public int PhanQuyenKhachHang(int ID_NhanVien, int ID_KhachHang, string Quyen,int ID_QuanLy)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("Quyen", Quyen),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
        };
        try
        {
            log.Info("PhanQuyenKhachHang : ID_NhanVien = " + ID_NhanVien + " - ID_KhachHang : " + ID_KhachHang + " - Quyen : " + Quyen + " - ID_QuanLy : " + ID_QuanLy);
            return helper.ExecuteNonQuery("sp_QL_PhanQuyenKhachHang", pars);
           
        }
        catch
        {
            return 0;
        }
    }
    public int XoaPhanQuyen( int ID_KhachHang)
    {
        SqlParameter[] pars = new SqlParameter[] {
           
            new SqlParameter("ID_KhachHang", ID_KhachHang),
             
        };
        try
        {
            return helper.ExecuteNonQuery("sp_PhanQuyen_XoaByKhachHang", pars);
        }
        catch
        {
            return 0;
        }
    }

    public int PhanQuyenKhachHang_Nhom(int ID_Nhom, int ID_KhachHang, string Quyen)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_Nhom", ID_Nhom),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("Quyen", Quyen),
        };
        try
        {
            return helper.ExecuteNonQuery("sp_QL_PhanQuyenKhachHang_Nhom", pars);
        }
        catch
        {
            return 0;
        }
    }


    public string GetPhanQuyen(int ID_NhanVien, int ID_KhachHang)
    {
        string Quyen;
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_KhachHang", ID_KhachHang)
        };
        try
        {
            return helper.ExecuteScalar("sp_QL_GetPhanQuyen", pars).ToString();
        }
        catch
        {
            return "";
        }
    }

    public static List<KhachHang> DSKhachHangTheoIDCT(int idct)
    {
        List<KhachHang> rs = new List<KhachHang>();

        SqlParameter[] par = new SqlParameter[]{
            new SqlParameter("@idct", idct)
        };

        DataTable dt = db.ExecuteDataSet("sp_QL_KhachHangTheoIDCT", par).Tables[0];

        try
        {
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    rs.Add(new KhachHang
                    {
                        IDKhachHang = int.Parse(dr["ID_KhachHang"].ToString()),
                        Ten = dr["TenKhachHang"].ToString()
                    });
                }
                catch
                {
                    rs.Add(new KhachHang
                    {
                        IDKhachHang = 0,
                        Ten = ""
                    });
                }
            }
            return rs;
        }
        catch
        {
            return rs;
        }


    }

    public static List<KhachHang> TimKiemKhachHangTheoMaVaTen(int idct, int idtinh, string input,int ID_QuanLy)
    {
        List<KhachHang> rs = new List<KhachHang>();

        SqlParameter[] par = new SqlParameter[]{
            new SqlParameter("@idct", idct),
            new SqlParameter("@idtinh", idtinh),
            new SqlParameter("@input", input),
            new SqlParameter("@ID_QuanLy", ID_QuanLy)
        };

        DataTable dt = db.ExecuteDataSet("sp_QL_TimKiemDaiLy", par).Tables[0];

        try
        {
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    rs.Add(new KhachHang
                    {
                        IDKhachHang = int.Parse(dr["ID_KhachHang"].ToString()),
                        Ten = dr["TenKhachHang"].ToString()
                    });
                }
                catch
                {
                    rs.Add(new KhachHang
                    {
                        IDKhachHang = 0,
                        Ten = "Không xác định"
                    });
                }
            }
            return rs;
        }
        catch
        {
            return rs;
        }
    }





    public static ToaDoOBJ ViTriKhachHang(int idkh)
    {
        ToaDoOBJ rs = new ToaDoOBJ();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idkh", idkh)
            };


            DataRow dr = db.ExecuteDataSet("getKhachHangTheoID", par).Tables[0].Rows[0];
            try
            {

                rs.id = (dr["ID_KhachHang"] != null) ? int.Parse(dr["ID_KhachHang"].ToString()) : 0;
                rs.ten = (dr["TenKhachHang"] != null) ? dr["TenKhachHang"].ToString() : "";
                rs.kinhdo = double.Parse(dr["KinhDo"].ToString());
                rs.vido = double.Parse(dr["ViDo"].ToString());

            }
            catch
            {
                rs.id = 0;
                rs.ten = "Không xác định";
                rs.kinhdo = 0;
                rs.vido = 0;
            }

        }
        catch { }
        return rs;
    }

    public bool CheckTrungKhachHangTheoSDT(int ID_QLLH, string DienThoai, int ID_KhachHang)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", ID_QLLH),
                new SqlParameter("DienThoai", DienThoai),
                new SqlParameter("ID_KhachHang", ID_KhachHang)
            };
            DataSet ds = helper.ExecuteDataSet("sp_QL_CheckTrungKHTheoSDT", pars);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //đã tồn tại
                return false;
            }
            else
            {
                //chưa tồn tại
                return true;
            }
        }
        catch
        {
            return true;
        }
    }

    public bool CheckTrungKhachHangTheoMST(int ID_QLLH, string MaSoThue, int ID_KhachHang)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", ID_QLLH),
                new SqlParameter("MaSoThue", MaSoThue),
                new SqlParameter("ID_KhachHang", ID_KhachHang)
            };
            DataSet ds =helper.ExecuteDataSet("sp_QL_CheckTrungKHTheoMST", pars);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count>0)
            {
                //đã tồn tại
                return false;
            }
            else
            {
                //chưa tồn tại
                return true;
            }
        }
        catch
        {
            return true;
        }
    }


    public static List<ToaDoOBJ> TatCaKhachHangTheoIDCT(int idct,int ID_QuanLy,int ID_Tinh, int ID_Quan, int ID_LoaiKhachHang)
    {
        List<ToaDoOBJ> rs = new List<ToaDoOBJ>();
        SqlParameter[] par = new SqlParameter[]{
            new SqlParameter("@idct", idct),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
             new SqlParameter("@ID_Tinh", ID_Tinh),
              new SqlParameter("@ID_Quan", ID_Quan),
               new SqlParameter("@ID_LoaiKhachHang", ID_LoaiKhachHang)
        };

        DataTable dt = db.ExecuteDataSet("sp_QL_DanhSachKhachHangTheoIDCT", par).Tables[0];

        foreach (DataRow dr in dt.Rows)
        {
            ToaDoOBJ kh = new ToaDoOBJ();
            try
            {

                kh.id = (dr["ID_KhachHang"] != null) ? int.Parse(dr["ID_KhachHang"].ToString()) : 0;
                kh.ten = (dr["TenKhachHang"] != null) ? dr["TenKhachHang"].ToString() : "";
                kh.kinhdo = double.Parse(dr["KinhDo"].ToString());
                kh.vido = double.Parse(dr["ViDo"].ToString());
                kh.icon_path = (dr["IconHienThi"] != null) ? dr["IconHienThi"].ToString() : "";
            }
            catch
            {
                kh.id = 0;
                kh.ten = "Không xác định";
                kh.kinhdo = 0;
                kh.vido = 0;
            }
            rs.Add(kh);
        }

        return rs;
    }

    public static List<ToaDoOBJ> TatCaKhachHangTheoIDCT_ToaDo(int idct, int ID_QuanLy, double kinhdo, double vido,int ID_Tinh, int ID_Quan, int ID_LoaiKhachHang)
    {
        List<ToaDoOBJ> rs = new List<ToaDoOBJ>();
        SqlParameter[] par = new SqlParameter[]{
            new SqlParameter("@idct", idct),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
            new SqlParameter("@kinhdo", kinhdo),
            new SqlParameter("@vido", vido),
             new SqlParameter("@ID_Tinh", ID_Tinh),
              new SqlParameter("@ID_Quan", ID_Quan),
               new SqlParameter("@ID_LoaiKhachHang", ID_LoaiKhachHang)
        };

        DataTable dt = db.ExecuteDataSet("sp_QL_DanhSachKhachHangTheoIDCT_ToaDo", par).Tables[0];

        foreach (DataRow dr in dt.Rows)
        {
            ToaDoOBJ kh = new ToaDoOBJ();
            try
            {

                kh.id = (dr["ID_KhachHang"] != null) ? int.Parse(dr["ID_KhachHang"].ToString()) : 0;
                kh.ten = (dr["TenKhachHang"] != null) ? dr["TenKhachHang"].ToString() : "";
                kh.kinhdo = double.Parse(dr["KinhDo"].ToString());
                kh.vido = double.Parse(dr["ViDo"].ToString());
                kh.icon_path = (dr["IconHienThi"] != null) ? dr["IconHienThi"].ToString() : "";
            }
            catch
            {
                kh.id = 0;
                kh.ten = "Không xác định";
                kh.kinhdo = 0;
                kh.vido = 0;
            }
            rs.Add(kh);
        }

        return rs;
    }
    public bool CheckTrungKhachHangTheoSDT_V1(int ID_QLLH, string DienThoai, int ID_KhachHang)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", ID_QLLH),
                new SqlParameter("DienThoai", DienThoai),
                new SqlParameter("ID_KhachHang", ID_KhachHang)
            };
            DataSet ds = helper.ExecuteDataSet("sp_QL_CheckTrungKHTheoSDT_V1", pars);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //đã tồn tại
                return false;
            }
            else
            {
                //chưa tồn tại
                return true;
            }
        }
        catch
        {
            return true;
        }
    }


    public static List<ImageOBJ> DanhSachAnh_ByIDKhachHang(int ID_KhachHang)
    {
        List<ImageOBJ> rs = new List<ImageOBJ>();
        try
        {
            //SqlParameter[] par = new SqlParameter[]{
            //    new SqlParameter("@ID_KhachHang", ID_KhachHang)
            //};
            //DataTable dt = db.ExecuteDataSet("sp_Album_GetImage_ByIDKhachHang", par).Tables[0];
            //DateTime d;
            //foreach (DataRow dr in dt.Rows)
            //{
            //    string small_path = dr["path_thumbnail_small"].ToString();
            //    string medium_path = dr["path_thumbnail_medium"].ToString();


            //    rs.Add(new ImageOBJ
            //    {
            //        imageid = int.Parse(dr["imageid"].ToString()),
            //        path = dr["path"].ToString(),
            //        path_thumbnail_medium = medium_path,
            //        path_thumbnail_small = small_path,
            //        thoigian =  DateTime.Parse(dr["insertedtime"].ToString()) ,
            //        kinhdo = double.Parse(dr["kinhdo"].ToString()),
            //        vido = double.Parse(dr["vido"].ToString()),
            //        idkhachhang = int.Parse(dr["idkhachhang"].ToString()),
            //        idnhanvien = int.Parse(dr["idnhanvien"].ToString()),
            //        idcongty = int.Parse(dr["idcongty"].ToString()),
            //        ghichu = dr["ghichu"].ToString(),
            //        tendaily = dr["TenKhachHang"].ToString(),
            //        diachi = dr["diachi"].ToString(),

            //    });
            //}
            return rs;
        }
        catch (Exception ex)
        {
            //log.Error(ex);
            return rs;
        }

    }


    public KhachHang GetKhachHangTheoTenSDT(int ID_QLLH, string Ma, string TenKhachHang, string SoDienThoai)
    {
        SqlParameter[] par = new SqlParameter[]{

                new SqlParameter("@ID_QLLH", ID_QLLH),
                 new SqlParameter("@Ma", Ma),
                 new SqlParameter("@SoDienThoai", SoDienThoai),
                 new SqlParameter("@TenKhachHang", TenKhachHang),
                 new SqlParameter("@ID_KhachHang", 0)
            };

        DataSet ds = helper.ExecuteDataSet("sp_App_CuaHang_ByMaCuaHang", par);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            DataRow dr = dt.Rows[0];
            KhachHang kh = GetKhachHangFromDataRow(dr);
            return kh;
        }
        catch
        {
            return null;
        }
    }

    public DataTable GetDataKhachHangByIdCha(int idcha)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
           new SqlParameter("@idcha", idcha) 
        };

            DataSet ds = helper.ExecuteDataSet("sp_App_DanhSachCuaHangTheoIDCha", pars);
            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            log.Error(ex);

        }


        return dt;
    }

    public DataTable GetDataKhachHangBy_IDNhomKH(int IDNhomKH)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
           new SqlParameter("@idnhomkh", IDNhomKH),
            new SqlParameter("@laykhachhangnhomcha", 1)
        };

            DataSet ds = helper.ExecuteDataSet("sp_App_DanhSachCuaHangTheoIDCha", pars);
            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            log.Error(ex);

        }


        return dt;
    }


    public int SaoChepPhanQuyen( int ID_KhachHang_Nguon, int ID_KhachHang_Dich,string ID_Quyen, int ID_QuanLy)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_KhachHang_Nguon", ID_KhachHang_Nguon),
            new SqlParameter("ID_KhachHang_Dich", ID_KhachHang_Dich) ,
            new SqlParameter("ID_Quyen", ID_Quyen),
               new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            log.Info("SaoChepPhanQuyen : ID_KhachHang_Nguon = " + ID_KhachHang_Nguon + " - ID_KhachHang_Dich : " + ID_KhachHang_Dich + " - ID_Quyen : " + ID_KhachHang_Dich + " - ID_QuanLy : " + ID_QuanLy);
            return helper.ExecuteNonQuery("sp_QL_PhanQuyenKhachHang_Copy", pars);
          
        }
        catch
        {
            return 0;
        }
    }

    public int SaoChepPhanQuyen_Nhom(int ID_KhachHang_Nguon, int ID_KhachHang_Dich, string ID_Quyen)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_KhachHang_Nguon", ID_KhachHang_Nguon),
            new SqlParameter("ID_KhachHang_Dich", ID_KhachHang_Dich),
            new SqlParameter("ID_Quyen", ID_Quyen)
        };
        try
        {
            return helper.ExecuteNonQuery("sp_QL_PhanQuyenKhachHang_Nhom_Copy", pars);
        }
        catch
        {
            return 0;
        }
    }
    public bool CheckTrungMST(string MST, int ID_QLLH, int ID_KhachHang)
    {
        bool tontai = false;
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@MST", MST),
                  new SqlParameter("@ID_QLLH", ID_QLLH),
                    new SqlParameter("@ID_KhachHang", ID_KhachHang),
            };

            DataTable dt = db.ExecuteDataSet("sp_KhachHang_CheckTrungMST", param).Tables[0];
            if (dt.Rows.Count > 0)
            {
                tontai = true;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return tontai;

    }

    [DataObjectMethod(DataObjectMethodType.Select, true)]
    public DataTable GetDataKhachHangAll_Grid_Paging(int IDQLLH, int ID_QuanLy, int ID_Tinh, int ID_Quan, int ID_LoaiKhachHang, int ID_KenhBanHang, int startRecord, int maxRecords)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@id_qllh", IDQLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
              new SqlParameter("@ID_Tinh", ID_Tinh),
                new SqlParameter("@ID_Quan", ID_Quan),
                new SqlParameter("@ID_LoaiKhachHang", ID_LoaiKhachHang),
                new SqlParameter("@ID_KenhBanHang", ID_KenhBanHang),
                 new SqlParameter("@startRecord", startRecord),
                  new SqlParameter("@maxRecords", maxRecords)
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangTheoIDQL_Paging", pars);
            dt = ds.Tables[0];

        }
        catch (Exception ex)
        {
            log.Error(ex);

        }


        return dt;
    }


    public Int32 GetDataKhachHangAll_Grid_Count(int IDQLLH, int ID_QuanLy, int ID_Tinh, int ID_Quan, int ID_LoaiKhachHang, int ID_KenhBanHang)
    {
        Int32  dt = 0;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@id_qllh", IDQLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
              new SqlParameter("@ID_Tinh", ID_Tinh),
                new SqlParameter("@ID_Quan", ID_Quan),
                new SqlParameter("@ID_LoaiKhachHang", ID_LoaiKhachHang),
                new SqlParameter("@ID_KenhBanHang", ID_KenhBanHang),
                 
        };

            object ds = helper.ExecuteScalar("sp_QL_getDSKhachHangTheoIDQL_Paging_Count", pars);
            if(ds != null)
            {
                dt = Convert.ToInt32(ds);
            }
             

        }
        catch (Exception ex)
        {
            log.Error(ex);

        }


        return dt;
    }

     
}