using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CongTyDB
/// </summary>
public class PhieuNhap_dl
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(PhieuNhap_dl));
    public static SqlDataHelper db = new SqlDataHelper();
    public PhieuNhap_dl()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static PhieuNhapOBJ TaoPhieuNhap(PhieuNhapOBJ dh)
    {
        PhieuNhapOBJ rs = new PhieuNhapOBJ();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_QLLH", dh.ID_QLLH),
                new SqlParameter("@ID_QuanLy", dh.ID_QuanLy),
                new SqlParameter("@NoiDung", dh.NoiDung) ,
                 new SqlParameter("@ID_Kho", dh.ID_Kho),
            };
            rs.ID_PhieuNhap = int.Parse(db.ExecuteScalar("sp_PhieuNhap_Insert", par).ToString());
            return rs;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return rs;
        }
    }


    public static int ThemChiTietPhieuNhap(PhieuNhapChiTietOBJ dh)
    {
        int re = 0;
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_PhieuNhap", dh.ID_PhieuNhap),
                new SqlParameter("@ID_HangHoa", dh.ID_HangHoa),
                new SqlParameter("@SoLuong", dh.SoLuong),
                 new SqlParameter("@ID_Kho", dh.ID_Kho),
                    new SqlParameter("@DonGia", dh.DonGia),
                       new SqlParameter("@ThanhTien", dh.ThanhTien)
            };
           re= int.Parse(db.ExecuteScalar("sp_PhieuNhap_ChiTiet_Insert", par).ToString());
             
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


    public static int UpdateThongTinKho(int ID_PhieuNhap, int ID_Kho)
    {
        int re = 0;
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_PhieuNhap", ID_PhieuNhap),
                  new SqlParameter("@ID_Kho", ID_Kho)
            };
            re = int.Parse(db.ExecuteNonQuery("sp_PhieuNhap_UpdateThongTinKho", par).ToString());

        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return re;
    }


    public DataTable GetDanhSachPhieuNhap(int ID_QLLH, DateTime dtTuNgay, DateTime dtDenNgay)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", ID_QLLH),
            new SqlParameter("@TuNgay", dtTuNgay),
                new SqlParameter("@DenNgay", dtDenNgay)
        };

        DataSet ds = db.ExecuteDataSet("sp_PhieuNhap_GetByIDQLLH", pars);
         dt = ds.Tables[0];
        }
        catch(Exception ex)
        {
            log.Error(ex);
        }
        return dt;
    }


    public DataTable GetPhieuNhapById(int ID_PhieuNhap)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_PhieuNhap", ID_PhieuNhap)
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
    
    public DataTable GetChiTietPhieuNhapById(int ID_PhieuNhap)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_PhieuNhap", ID_PhieuNhap)
        };

            DataSet ds = db.ExecuteDataSet("sp_ChiTietPhieuNhap_GetByID", pars);
            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return dt;
    }
    /// <summary>
    /// Hàm lấy số liệu chi tiết phiếu nhập có kèm theo só lượng tồn kho để loaddata phiếu điều chỉnh
    /// </summary>
    /// <param name="ID_PhieuNhap"></param>
    /// <returns></returns>
    public DataTable GetChiTietPhieuNhapById_LaySoLieuTonKho(int ID_PhieuNhap)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_PhieuNhap", ID_PhieuNhap)
        };

            DataSet ds = db.ExecuteDataSet("sp_ChiTietPhieuNhap_GetByID_GomTonKho", pars);
            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return dt;
    }

    public static PhieuDieuChinhOBJ TaoPhieuDieuChinh(PhieuDieuChinhOBJ dh)
    {
        PhieuDieuChinhOBJ rs = new PhieuDieuChinhOBJ();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_QLLH", dh.ID_QLLH),
                new SqlParameter("@ID_QuanLy", dh.ID_QuanLy),
                new SqlParameter("@ID_PhieuNhap", dh.ID_PhieuNhap) ,
                 new SqlParameter("@LyDoDieuChinh", dh.LyDoDieuChinh),
            };
            rs.ID_PhieuNhap = int.Parse(db.ExecuteScalar("sp_PhieuDieuChinh_Insert", par).ToString());
            return rs;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return rs;
        }
    }

    public static int TaoPhieuDieuChinhChiTiet(PhieuDieuChinh_ChiTietOBJ dh)
    {
        int rs = 0;
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@DonGia", dh.DonGia),
                new SqlParameter("@ID_HangHoa", dh.ID_HangHoa),
                new SqlParameter("@ID_PhieuDieuChinh", dh.ID_PhieuDieuChinh) ,
                 new SqlParameter("@SoLuong", dh.SoLuong),
                   new SqlParameter("@ThanhTien", dh.ThanhTien) 
                   
            };
            rs = int.Parse(db.ExecuteNonQuery("sp_PhieuDieuChinh_ChiTiet_Insert", par).ToString());
          
        }
        catch (Exception ex)
        {
            log.Error(ex);
            
        }
        return rs;
    }

    public static int XoaPhieuDieuChinh(int ID_PhieuDieuChinh)
    {
        int rs = 0;
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_PhieuDieuChinh", ID_PhieuDieuChinh) 

            };
            rs = int.Parse(db.ExecuteNonQuery("sp_PhieuDieuChinh_ChiTiet_Insert", par).ToString());

        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return rs;
    }
}