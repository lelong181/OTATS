using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CongTyDB
/// </summary>
public class PhieuDieuChuyen_dl
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(PhieuDieuChuyen_dl));
    public static SqlDataHelper db = new SqlDataHelper();
    public PhieuDieuChuyen_dl()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static PhieuDieuChuyenOBJ ThemMoi(PhieuDieuChuyenOBJ dh)
    {
        PhieuDieuChuyenOBJ rs = new PhieuDieuChuyenOBJ();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_QLLH", dh.ID_QLLH),
                new SqlParameter("@ID_QuanLy", dh.ID_QuanLy),
                new SqlParameter("@DienGiai", dh.DienGiai) ,
                 new SqlParameter("@NgayDieuChuyen", dh.NgayDieuChuyen),
                 new SqlParameter("@ID_KhoNhap", dh.ID_KhoNhap),
                 new SqlParameter("@ID_KhoXuat", dh.ID_KhoXuat)
            };
            rs.ID_PhieuDieuChuyen = int.Parse(db.ExecuteScalar("sp_PhieuDieuChuyenKho_Insert", par).ToString());
            return rs;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return rs;
        }
    }

    public static int ThemChiTiet(PhieuDieuChuyenChiTietOBJ dh)
    {
        int re = 0;
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_PhieuDieuChuyen", dh.ID_PhieuDieuChuyen),
                new SqlParameter("@ID_HangHoa", dh.ID_HangHoa),
                new SqlParameter("@SoLuong", dh.SoLuong),
                 new SqlParameter("@ID_KhoNhap", dh.ID_KhoNhap),
                 new SqlParameter("@ID_KhoXuat", dh.ID_KhoXuat)
            };
           re= int.Parse(db.ExecuteScalar("sp_PhieuDieuChuyenChiTiet_Insert", par).ToString());
             
        }
        catch (Exception ex)
        {
            log.Error(ex);
            
        }
        return re;
    }
    public static int XoaChiTietPhieuNhap(int ID_PhieuNhap)
    {
        int re = 0;
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_PhieuNhap", ID_PhieuNhap)
                
            };
            re = int.Parse(db.ExecuteScalar("sp_PhieuNhap_ChiTiet_Delete", par).ToString());

        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return re;
    }
     

    public DataTable GetDanhSachPhieuDieuChuyen(int ID_QLLH, DateTime dtTuNgay, DateTime dtDenNgay)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", ID_QLLH),
            new SqlParameter("@TuNgay", dtTuNgay),
                new SqlParameter("@DenNgay", dtDenNgay)
        };

        DataSet ds = db.ExecuteDataSet("sp_PhieuDieuChuyenKho_GetByIDQLLH", pars);
         dt = ds.Tables[0];
        }
        catch(Exception ex)
        {
            log.Error(ex);
        }
        return dt;
    }


    public DataTable GetPhieuNhapById(int ID_PhieuDieuChuyen)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_PhieuDieuChuyen", ID_PhieuDieuChuyen)
        };

            DataSet ds = db.ExecuteDataSet("sp_PhieuNhap_GetByID", pars);
            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return dt;
    }

    public DataTable GetChiTietPhieuById(int ID_PhieuDieuChuyen)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_PhieuDieuChuyen", ID_PhieuDieuChuyen)
        };

            DataSet ds = db.ExecuteDataSet("sp_ChiTietPhieuDieuChuyen_GetByID", pars);
            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return dt;
    }
}