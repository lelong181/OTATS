using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for HangHoa
/// </summary>
public class NhomDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(NhomDB));

    public static SqlDataHelper db = new SqlDataHelper();

    public NhomDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static List<NhomOBJ> getDS_Nhom(int Id_QLLH)
    {
        try
        {
            List<NhomOBJ> lstDanhMuc = new List<NhomOBJ>();
            SqlDataHelper sql = new SqlDataHelper();
            DataSet ds = sql.ExecuteDataSet("getDSNhomTheoID_QLLH",
                new SqlParameter("@ID_QLLH", Id_QLLH)
              );

            if (ds == null)
            {
                return null;
            }

            DataTable dtbl = null;
            dtbl = ds.Tables[0];
            NhomOBJ dmTatCa = new NhomOBJ();
            User_dl u = new User_dl();
            //SELECT SUM(SoLuongNhanVien) as SoLuongNhanVien, SUM(SoLuongTaiKhoan) as SoLuongTaiKhoan
            // FROM (
            //select count(NhanVien.ID_NhanVien) as 'SoLuongNhanVien','0' as SoLuongTaiKhoan  FROM NhanVien where ID_QLLH = 1
            //UNION
            //select 0 as SoLuongNhanVien, count(*) as 'SoLuongTaiKhoan' from TaiKhoan  where TaiKhoan.idcongty = 1
            //) as a

            dmTatCa.ID_Nhom = -2;
            dmTatCa.ID_PARENT = 0;
            dmTatCa.TenNhom = "Tất cả";
            int soLuongTatcaNV = 0;
            int soLuongTatcaQL = 0;
            int soLuongNVKhac = 0;
            int soLuongQLKhac = 0;

            try
            {
                DataSet dsThongKe = sql.ExecuteDataSet("sp_Nhom_LaySoLuongNhanVien_QuanLy",
                        new SqlParameter("@ID_QLLH", Id_QLLH)
                      );
                DataTable tbThongKe = null;
                tbThongKe = dsThongKe.Tables[0];
                soLuongTatcaNV = int.Parse(tbThongKe.Rows[0]["SoLuongNhanVien"].ToString());
                soLuongTatcaQL = int.Parse(tbThongKe.Rows[0]["SoLuongTaiKhoan"].ToString());
                soLuongNVKhac = int.Parse(tbThongKe.Rows[0]["SoLuongNhanVienKhac"].ToString());
                soLuongQLKhac = int.Parse(tbThongKe.Rows[0]["SoLuongQuanLyKhac"].ToString());


                //CongTyOBJ cty = CongTyDB.ThongTinCongTyByID(Id_QLLH);

                //dmTatCa.TenHienThi_NhanVien = "Tất cả" + " (" + soLuongTatcaNV + " / " + cty.soluongnhanvien_duocap + ")";
                dmTatCa.TenHienThi_QuanLy = "Tất cả" + " (" + soLuongTatcaQL + ")";
            }
            catch (Exception ex)
            {

                dmTatCa.TenHienThi_NhanVien = "Tất cả";
                dmTatCa.TenHienThi_QuanLy = "Tất cả";
            }
            lstDanhMuc.Add(dmTatCa);
            int cntNV = 0;
            int cntQL = 0;
            int IDGocCongTy = 0;
            int SoLuongIDGoc = 0;
            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                DataRow dr = dtbl.Rows[i];
                NhomOBJ dm = new NhomOBJ();
                dm.ID_Nhom = (dr["ID_Nhom"].ToString() != "") ? int.Parse(dr["ID_Nhom"].ToString()) : 0;
                dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
                if (dm.ID_PARENT == 0)
                {
                    SoLuongIDGoc++;
                    IDGocCongTy = dm.ID_Nhom;
                }
                dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
                dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
                dm.SoLuongNhanVien = (dr["SoLuongNhanVien"].ToString() != "") ? int.Parse(dr["SoLuongNhanVien"].ToString()) : 0;
                dm.SoLuongQuanLy = (dr["SoLuongQuanLy"].ToString() != "") ? int.Parse(dr["SoLuongQuanLy"].ToString()) : 0;
                dm.TenNhom = dr["TenNhom"].ToString();

                dm.MaNhom = dr["MaNhom"].ToString();
                //dm.SoLuongMatHang = (dr["SoLuongMatHang"].ToString() != "") ? int.Parse(dr["SoLuongMatHang"].ToString()) : 0;

                dm.TenHienThi_NhanVien = dr["TenNhom"].ToString() + " (" + dm.SoLuongNhanVien + ")";
                dm.TenHienThi_QuanLy = dr["TenNhom"].ToString() + " (" + dm.SoLuongQuanLy + ")";
                lstDanhMuc.Add(dm);
                // cntQL += dm.SoLuongQuanLy;
                cntNV += dm.SoLuongNhanVien;

            }
            //int cntNVKhac = soLuongTatcaNV - cntNV;
            // int cntQLKhac = soLuongTatcaQL - cntQL;

            NhomOBJ dmKhac = new NhomOBJ();


            dmKhac.ID_Nhom = -1;
            dmKhac.ID_PARENT = SoLuongIDGoc == 1 ? IDGocCongTy : 0;
            dmKhac.TenHienThi_QuanLy = "Khác (" + soLuongQLKhac + ")";
            //dmKhac.TenHienThi_QuanLy = "Khác";
            dmKhac.TenHienThi_NhanVien = "Khác (" + soLuongNVKhac + ")";
            dmKhac.TenNhom = "Khác";

            if (lstDanhMuc.Count > 0)
            {
                lstDanhMuc.Insert(lstDanhMuc.Count, dmKhac);
            }
            return lstDanhMuc;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }


    public static List<NhomOBJ> getDS_Nhom_Active(int Id_QLLH)
    {
        try
        {
            List<NhomOBJ> lstDanhMuc = new List<NhomOBJ>();
            SqlDataHelper sql = new SqlDataHelper();
            DataSet ds = sql.ExecuteDataSet("getDSNhomTheoID_QLLH",
                new SqlParameter("@ID_QLLH", Id_QLLH)
              );

            if (ds == null)
            {
                return null;
            }

            DataTable dtbl = null;
            dtbl = ds.Tables[0];
            NhomOBJ dmTatCa = new NhomOBJ();

            //dmTatCa.SoLuongMatHang = DemSoLuongMatHang(-1,Id_QLLH);

            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                DataRow dr = dtbl.Rows[i];
                NhomOBJ dm = new NhomOBJ();
                dm.ID_Nhom = (dr["ID_Nhom"].ToString() != "") ? int.Parse(dr["ID_Nhom"].ToString()) : 0;
                dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
                dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
                dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
                dm.TenNhom = dr["TenNhom"].ToString();
                dm.MaNhom = dr["MaNhom"].ToString();
                dm.SoLuongNhanVien = (dr["SoLuongNhanVien"].ToString() != "") ? int.Parse(dr["SoLuongNhanVien"].ToString()) : 0;
                dm.SoLuongQuanLy = (dr["SoLuongQuanLy"].ToString() != "") ? int.Parse(dr["SoLuongQuanLy"].ToString()) : 0;

                dm.TenHienThi_NhanVien = dr["TenNhom"].ToString() + " (" + dm.SoLuongNhanVien + ")";
                dm.TenHienThi_QuanLy = dr["TenNhom"].ToString() + " (" + dm.SoLuongQuanLy + ")";

                lstDanhMuc.Add(dm);
            }

            return lstDanhMuc;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }
    public static List<NhomOBJ> getDS_NhomCapCha(int Id_QLLH)
    {
        try
        {
            List<NhomOBJ> lstDanhMuc = new List<NhomOBJ>();
            SqlDataHelper sql = new SqlDataHelper();
            DataSet ds = sql.ExecuteDataSet("getDSNhomTheoID_QLLH",
                new SqlParameter("@ID_QLLH", Id_QLLH)
              );

            if (ds == null)
            {
                return null;
            }

            DataTable dtbl = null;
            dtbl = ds.Tables[0];

            for (int i = 0; i < dtbl.Rows.Count; i++)
            {

                DataRow dr = dtbl.Rows[i];
                if (dr["ID_PARENT"].ToString() == "")
                {
                    NhomOBJ dm = new NhomOBJ();
                    dm.ID_Nhom = (dr["ID_Nhom"].ToString() != "") ? int.Parse(dr["ID_Nhom"].ToString()) : 0;
                    dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
                    dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                    dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
                    dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
                    dm.TenNhom = dr["TenNhom"].ToString();
                    dm.MaNhom = dr["MaNhom"].ToString();
                    dm.SoLuongNhanVien = (dr["SoLuongNhanVien"].ToString() != "") ? int.Parse(dr["SoLuongNhanVien"].ToString()) : 0;
                    dm.SoLuongQuanLy = (dr["SoLuongQuanLy"].ToString() != "") ? int.Parse(dr["SoLuongQuanLy"].ToString()) : 0;

                    dm.TenHienThi_NhanVien = dr["TenNhom"].ToString() + " (" + dm.SoLuongNhanVien + ")";
                    dm.TenHienThi_QuanLy = dr["TenNhom"].ToString() + " (" + dm.SoLuongQuanLy + ")";
                    lstDanhMuc.Add(dm);
                }
            }

            return lstDanhMuc;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public static List<NhomOBJ> getDS_NhomCon_ById(int ID_Nhom)
    {
        try
        {
            List<NhomOBJ> lstDanhMuc = new List<NhomOBJ>();
            SqlDataHelper sql = new SqlDataHelper();
            DataSet ds = sql.ExecuteDataSet("getDS_NhomCon_ById",
                new SqlParameter("@ID_Nhom", ID_Nhom)
              );

            if (ds == null)
            {
                return null;
            }

            DataTable dtbl = null;
            dtbl = ds.Tables[0];
            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                DataRow dr = dtbl.Rows[i];
                NhomOBJ dm = new NhomOBJ();
                dm.ID_Nhom = (dr["ID_Nhom"].ToString() != "") ? int.Parse(dr["ID_Nhom"].ToString()) : 0;
                dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
                dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
                dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
                dm.TenNhom = dr["TenNhom"].ToString(); dm.MaNhom = dr["MaNhom"].ToString();
                dm.SoLuongNhanVien = (dr["SoLuongNhanVien"].ToString() != "") ? int.Parse(dr["SoLuongNhanVien"].ToString()) : 0;
                dm.SoLuongQuanLy = (dr["SoLuongQuanLy"].ToString() != "") ? int.Parse(dr["SoLuongQuanLy"].ToString()) : 0;

                dm.TenHienThi_NhanVien = dr["TenNhom"].ToString() + " (" + dm.SoLuongNhanVien + ")";
                dm.TenHienThi_QuanLy = dr["TenNhom"].ToString() + " (" + dm.SoLuongQuanLy + ")";


                lstDanhMuc.Add(dm);
            }

            return lstDanhMuc;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public static bool Them(NhomOBJ dm)
    {
        try
        {
            SqlDataHelper helper = new SqlDataHelper();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", dm.ID_QLLH),
                new SqlParameter("ID_PARENT", dm.ID_PARENT),
                new SqlParameter("TenNhom", dm.TenNhom),
                  new SqlParameter("MaNhom", dm.MaNhom),
                  new SqlParameter("SiteCode", dm.SiteCode)
            };
            int i = helper.ExecuteNonQuery("sp_Nhom_Insert", pars);
            if (i > 0)
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

    public static bool Sua(NhomOBJ dm)
    {
        try
        {
            SqlDataHelper helper = new SqlDataHelper();
            SqlParameter[] pars = new SqlParameter[] {
               new SqlParameter("ID_Nhom", dm.ID_Nhom),
               new SqlParameter("ID_QLLH", dm.ID_QLLH),
                new SqlParameter("ID_PARENT", dm.ID_PARENT),
                new SqlParameter("TenNhom", dm.TenNhom),
                 new SqlParameter("SiteCode", dm.SiteCode),
                 new SqlParameter("MaNhom", dm.MaNhom),
                new SqlParameter("TrangThai", dm.TrangThai),

            };

            if (helper.ExecuteNonQuery("sp_Nhom_Update", pars) != 0)
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

    public static bool Xoa(NhomOBJ dm)
    {
        try
        {
            SqlDataHelper helper = new SqlDataHelper();
            SqlParameter[] pars = new SqlParameter[] {
               new SqlParameter("ID_Nhom", dm.ID_Nhom),


            };

            if (helper.ExecuteNonQuery("sp_Nhom_Delete", pars) != 0)
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

    public static NhomOBJ get_NhomById(int ID_Nhom)
    {
        try
        {
            NhomOBJ dm = new NhomOBJ();
            SqlDataHelper sql = new SqlDataHelper();
            DataSet ds = sql.ExecuteDataSet("getDSNhomTheoID",
                new SqlParameter("@ID_Nhom", ID_Nhom)
              );

            if (ds == null)
            {
                return null;
            }

            DataTable dtbl = null;
            dtbl = ds.Tables[0];

            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                DataRow dr = dtbl.Rows[i];

                dm.ID_Nhom = (dr["ID_Nhom"].ToString() != "") ? int.Parse(dr["ID_Nhom"].ToString()) : 0;
                dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
                dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
                dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
                dm.TenNhom = dr["TenNhom"].ToString(); dm.MaNhom = dr["MaNhom"].ToString();
                dm.SoLuongNhanVien = (dr["SoLuongNhanVien"].ToString() != "") ? int.Parse(dr["SoLuongNhanVien"].ToString()) : 0;
                dm.SoLuongQuanLy = (dr["SoLuongQuanLy"].ToString() != "") ? int.Parse(dr["SoLuongQuanLy"].ToString()) : 0;

                dm.TenHienThi_NhanVien = dr["TenNhom"].ToString() + " (" + dm.SoLuongNhanVien + ")";
                dm.TenHienThi_QuanLy = dr["TenNhom"].ToString() + " (" + dm.SoLuongQuanLy + ")";
            }

            return dm;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }
    public static NhomOBJ get_NhomByTenNhom(string TenNhom)
    {
        try
        {
            NhomOBJ dm = new NhomOBJ();
            SqlDataHelper sql = new SqlDataHelper();
            DataSet ds = sql.ExecuteDataSet("getDSNhomTheoTenNhom",
                new SqlParameter("@TenNhom", TenNhom)
              );

            if (ds == null)
            {
                return null;
            }

            DataTable dtbl = null;
            dtbl = ds.Tables[0];

            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                DataRow dr = dtbl.Rows[i];

                dm.ID_Nhom = (dr["ID_Nhom"].ToString() != "") ? int.Parse(dr["ID_Nhom"].ToString()) : 0;
                dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
                dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
                dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
                dm.TenNhom = dr["TenNhom"].ToString(); dm.MaNhom = dr["MaNhom"].ToString();
                dm.SoLuongNhanVien = (dr["SoLuongNhanVien"].ToString() != "") ? int.Parse(dr["SoLuongNhanVien"].ToString()) : 0;
                dm.SoLuongQuanLy = (dr["SoLuongQuanLy"].ToString() != "") ? int.Parse(dr["SoLuongQuanLy"].ToString()) : 0;

                dm.TenHienThi_NhanVien = dr["TenNhom"].ToString() + " (" + dm.SoLuongNhanVien + ")";
                dm.TenHienThi_QuanLy = dr["TenNhom"].ToString() + " (" + dm.SoLuongQuanLy + ")";
            }

            return dm;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public static List<NhomOBJ> getDS_Nhom_ByIdTaiKhoan(int idtaikhoan)
    {
        try
        {
            List<NhomOBJ> lstDanhMuc = new List<NhomOBJ>();
            SqlDataHelper sql = new SqlDataHelper();
            DataSet ds = sql.ExecuteDataSet("getDSNhomTheo_IDTaiKhoan",
                new SqlParameter("@idtaikhoan", idtaikhoan)
              );

            if (ds == null)
            {
                return null;
            }

            DataTable dtbl = null;
            dtbl = ds.Tables[0];
            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                DataRow dr = dtbl.Rows[i];
                NhomOBJ dm = new NhomOBJ();
                dm.ID_Nhom = (dr["ID_Nhom"].ToString() != "") ? int.Parse(dr["ID_Nhom"].ToString()) : 0;
                dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
                dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
                dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
                dm.TenNhom = dr["TenNhom"].ToString();
                dm.MaNhom = dr["MaNhom"].ToString();
                dm.SiteCode = dr["SiteCode"].ToString();
                dm.SoLuongNhanVien = (dr["SoLuongNhanVien"].ToString() != "") ? int.Parse(dr["SoLuongNhanVien"].ToString()) : 0;
                dm.SoLuongQuanLy = (dr["SoLuongQuanLy"].ToString() != "") ? int.Parse(dr["SoLuongQuanLy"].ToString()) : 0;

                dm.TenHienThi_NhanVien = dr["TenNhom"].ToString() + " (" + dm.SoLuongNhanVien + ")";
                dm.TenHienThi_QuanLy = dr["TenNhom"].ToString() + " (" + dm.SoLuongQuanLy + ")";

                //lstDanhMuc.Add(dm);
                bool tontai = false;
                foreach (NhomOBJ n2 in lstDanhMuc)
                {
                    if (n2.ID_Nhom == dm.ID_Nhom)
                    {
                        tontai = true;
                        break;
                    }
                }
                if (!tontai)
                    lstDanhMuc.Add(dm);

                List<NhomOBJ> lstCon = getDS_NhomCon_ById(dm.ID_Nhom);
                foreach (NhomOBJ n in lstCon)
                {
                    tontai = false;
                    foreach (NhomOBJ n2 in lstDanhMuc)
                    {
                        if (n2.ID_Nhom == n.ID_Nhom)
                        {
                            tontai = true;
                            break;
                        }
                    }
                    if (!tontai)
                        lstDanhMuc.Add(n);
                }
            }

            return lstDanhMuc;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public static List<NhomOBJ> getDSNhomDaGan_TheoIdTaiKhoan(int idtaikhoan)
    {
        try
        {
            List<NhomOBJ> lstDanhMuc = new List<NhomOBJ>();
            SqlDataHelper sql = new SqlDataHelper();
            DataSet ds = sql.ExecuteDataSet("getDSNhomDaGan_TheoIdTaiKhoan",
                new SqlParameter("@idtaikhoan", idtaikhoan)
              );

            if (ds == null)
            {
                return null;
            }

            DataTable dtbl = null;
            dtbl = ds.Tables[0];
            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                DataRow dr = dtbl.Rows[i];
                NhomOBJ dm = new NhomOBJ();
                dm.ID_Nhom = (dr["ID_Nhom"].ToString() != "") ? int.Parse(dr["ID_Nhom"].ToString()) : 0;
                dm.ID_PARENT = (dr["ID_PARENT"].ToString() != "") ? int.Parse(dr["ID_PARENT"].ToString()) : 0;
                dm.ID_QLLH = (dr["ID_QLLH"].ToString() != "") ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
                dm.NgayTao = (dr["NgayTao"].ToString() != "") ? DateTime.Parse(dr["NgayTao"].ToString()) : dm.NgayTao;
                dm.TrangThai = (dr["TrangThai"].ToString() != "") ? int.Parse(dr["TrangThai"].ToString()) : 0;
                dm.TenNhom = dr["TenNhom"].ToString(); dm.MaNhom = dr["MaNhom"].ToString();
                dm.SoLuongNhanVien = (dr["SoLuongNhanVien"].ToString() != "") ? int.Parse(dr["SoLuongNhanVien"].ToString()) : 0;
                dm.SoLuongQuanLy = (dr["SoLuongQuanLy"].ToString() != "") ? int.Parse(dr["SoLuongQuanLy"].ToString()) : 0;

                dm.TenHienThi_NhanVien = dr["TenNhom"].ToString() + " (" + dm.SoLuongNhanVien + ")";
                dm.TenHienThi_QuanLy = dr["TenNhom"].ToString() + " (" + dm.SoLuongQuanLy + ")";

                lstDanhMuc.Add(dm);

            }

            return lstDanhMuc;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }
    public static bool PhanNhom(int ID_Nhom, int ID_TaiKhoan)
    {
        try
        {
            SqlDataHelper helper = new SqlDataHelper();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_Nhom", ID_Nhom),
                new SqlParameter("idtaikhoan", ID_TaiKhoan)

            };
            int i = helper.ExecuteNonQuery("sp_PhanNhom_Insert", pars);
            if (i > 0)
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

    public static bool XoaPhanNhom(int ID_TaiKhoan)
    {
        try
        {
            SqlDataHelper helper = new SqlDataHelper();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("idtaikhoan", ID_TaiKhoan)

            };
            int i = helper.ExecuteNonQuery("sp_PhanNhom_Delete", pars);
            if (i > 0)
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

}