using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CongTyDB
/// </summary>
public class KhoDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(KhoDB));
    public static SqlDataHelper db = new SqlDataHelper();
    public KhoDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public KhoOBJ GetById(int ID_Kho)
    {
        KhoOBJ rs = new KhoOBJ();

        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_Kho", ID_Kho)
        };

        DataSet ds = db.ExecuteDataSet("sp_Kho_GetByID", pars);
        DataRow dr = ds.Tables[0].Rows[0];

        try
        {

            rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            rs.ID_Kho = int.Parse(dr["ID_Kho"].ToString());
            rs.TenKho = dr["TenKho"].ToString();
            rs.DiaChi = dr["DiaChi"].ToString();
            rs.MaKho = dr["MaKho"].ToString();
            rs.NgayTao = dr["NgayTao"].ToString() != "" ? DateTime.Parse(dr["NgayTao"].ToString()) : rs.NgayTao;
            rs.TrangThai = dr["TrangThai"].ToString() != "" ? int.Parse(dr["TrangThai"].ToString()) : 0 ;
        }
        catch
        {
            return null;
        }
        return rs;
    }

    public KhoOBJ GetKhoByName(string TenKho, int ID_QLLH)
    {
        KhoOBJ rs = new KhoOBJ();

        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("TenKho", TenKho),
              new SqlParameter("ID_QLLH", ID_QLLH)
        };
        try
        {
            DataSet ds = db.ExecuteDataSet("sp_Kho_GetBy_Ten", pars);
            DataRow dr = ds.Tables[0].Rows[0];

       

            rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            rs.ID_Kho = int.Parse(dr["ID_Kho"].ToString());
            rs.TenKho = dr["TenKho"].ToString();
            rs.DiaChi = dr["DiaChi"].ToString();
            rs.MaKho = dr["MaKho"].ToString();
            rs.NgayTao = dr["NgayTao"].ToString() != "" ? DateTime.Parse(dr["NgayTao"].ToString()) : rs.NgayTao;
            rs.TrangThai = dr["TrangThai"].ToString() != "" ? int.Parse(dr["TrangThai"].ToString()) : 0;
        }
        catch(Exception ex)
        {
            return null;
        }
        return rs;
    }

    public KhoOBJ GetFromDataRow(DataRow dr)
    {
        KhoOBJ rs = new KhoOBJ();
 
      

        try
        {

            rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            rs.ID_Kho = int.Parse(dr["ID_Kho"].ToString());
            rs.TenKho = dr["TenKho"].ToString();
            rs.MaKho = dr["MaKho"].ToString();
            rs.DiaChi = dr["DiaChi"].ToString();
            rs.NgayTao = dr["NgayTao"].ToString() != "" ? DateTime.Parse(dr["NgayTao"].ToString()) : rs.NgayTao;
            rs.TrangThai = dr["TrangThai"].ToString() != "" ? int.Parse(dr["TrangThai"].ToString()) : 0;

        }
        catch
        {
            return null;
        }
        return rs;
    }
    public List<KhoOBJ> GetListDanhSach (int idct)
    {
        List<KhoOBJ> lst = new List<KhoOBJ>();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)
            };
           DataTable  dt = db.ExecuteDataSet("sp_Kho_GetAll", param).Tables[0];
            foreach(DataRow dr in dt.Rows)
            {
                KhoOBJ rs = GetFromDataRow(dr);
                lst.Add(rs);
            }
            
           
        }
        catch (Exception ex)
        {
            log.Error(ex);
            
        }
        return lst;
    }
 

    public DataTable GetDataDanhSach(int idct)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)
            };
            dt = db.ExecuteDataSet("sp_Kho_GetAll", param).Tables[0];


            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return dt;
        }
    }
    public bool Them(KhoOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                 
                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("TenKho", obj.TenKho),
                    new SqlParameter("MaKho", obj.MaKho),
                      new SqlParameter("DiaChi", obj.DiaChi),

                };
            int i = db.ExecuteNonQuery("sp_Kho_ThemMoi", pars);
            if (i > 0)
            {
                ret = true;
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return ret;
    }
    public bool Sua(KhoOBJ obj, int ID_QuanLy)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_Kho", obj.ID_Kho),
                     new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("TenKho", obj.TenKho),
                    new SqlParameter("MaKho", obj.MaKho),
                      new SqlParameter("DiaChi", obj.DiaChi),
                      new SqlParameter("ID_QuanLy", ID_QuanLy),
                         new SqlParameter("TrangThai", obj.TrangThai)

                };
            int i = db.ExecuteNonQuery("sp_Kho_Sua", pars);
            if (i > 0)
            {
                ret = true;
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return ret;
    }
    public bool Xoa(int ID_Kho, int ID_QuanLy)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_Kho", ID_Kho),
                    new SqlParameter("ID_QuanLy", ID_QuanLy)
                };
            int i = db.ExecuteNonQuery("sp_Kho_Xoa", pars);
            if (i > 0)
            {
                ret = true;
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return ret;
    }
    public DataTable GetNhanVien_QuanLyKho(int ID_Kho)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_Kho", ID_Kho)
            };
            dt = db.ExecuteDataSet("sp_Kho_GetNhanVienQuanLy", param).Tables[0];


            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return dt;
        }
    }
    /// <summary>
    /// Lấy danh sách kho hàng theo phân quyền nhân viên và kho 
    /// </summary>
    /// <param name="idmathang"></param>
    /// <param name="ID_NhanVien"></param>
    /// <returns></returns>
    public static DataTable DanhSachKho_MatHang(int idmathang, int ID_QLLLH)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_Hang", idmathang),
                  new SqlParameter("@ID_QLLH", ID_QLLLH),
            };
            dt = db.ExecuteDataSet("sp_Kho_HangHoa_GetByID_Hang_NhanVien", par).Tables[0];

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }

        return dt;
    }


    public DataTable GetKho_ByID_Hang_NVQL(int ID_Hang, int ID_NhanVien)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_Hang", ID_Hang),
                new SqlParameter("@ID_NhanVien", ID_NhanVien)
            };
            dt = db.ExecuteDataSet("usp_vuongtm_GetTonKhoTheoMatHang", param).Tables[0];
            if (dt.Rows.Count == 0)
            {

                DataRow dr = dt.NewRow();
                dr["ID_Hang"] = ID_Hang;
                dr["TonSoLuongCuoiKy"] = 0;
                dt.Rows.Add(dr);

            }

            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return dt;
        }
    }

    public static DataTable GetTonMatHang_TheoNhanVien(int ID_Hang, int ID_NhanVien)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_Hang", ID_Hang),
                new SqlParameter("@ID_NhanVien", ID_NhanVien)
            };
            dt = db.ExecuteDataSet("usp_TRUONGNM_GetTonKhoTheoMatHang_KhoPhanQuyen", param).Tables[0];
            if (dt.Rows.Count == 0)
            {

                DataRow dr = dt.NewRow();
                dr["ID_Hang"] = ID_Hang;
                dr["TonSoLuongCuoiKy"] = 0;
                dt.Rows.Add(dr);

            }
            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return dt;
        }
    }

    public DataTable GetSoLuongHangHoaChuaGiao(int ID_Hang, int ID_QLLH)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_Hang", ID_Hang),
                new SqlParameter("@ID_QLLH", ID_QLLH)
            };
            dt = db.ExecuteDataSet("sp_HangHoa_GetHangChuaGiao", param).Tables[0];


            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return dt;
        }
    }
}