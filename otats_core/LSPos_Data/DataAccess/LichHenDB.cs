using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LichHenDB
/// </summary>
public class LichHenDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(LichHenDB));
    public static SqlDataHelper db = new SqlDataHelper();
    public LichHenDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public LichHenOBJ GetDuLieuFromDataRow(DataRow dr)
    {
        LichHenOBJ rs = new LichHenOBJ();

        try
        {
            rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            rs.ID_KhachHang = int.Parse(dr["ID_KhachHang"].ToString());
            rs.ID_NhanVien = int.Parse(dr["ID_NhanVien"].ToString());
            rs.ID_LichHen = int.Parse(dr["ID_LichHen"].ToString());
            rs.TrangThai = int.Parse(dr["TrangThai"].ToString());
            rs.NoiDung = dr["NoiDung"].ToString();
            rs.KetQua = dr["KetQua"].ToString();
            rs.TenNhanVien = dr["TenNhanVien"].ToString();
            rs.TenKhachHang = dr["TenKhachHang"].ToString();
            rs.DiaChi = dr["DiaChi"].ToString();
            rs.ThoiGian = dr["ThoiGian"].ToString() != "" ? DateTime.Parse(dr["ThoiGian"].ToString()) : rs.ThoiGian;
            rs.ThoiGian_HienThi = rs.ThoiGian.ToString("dd/MM/yyyy HH:mm:ss");

        }
        catch
        {
            return null;
        }
        return rs;
    }
    public List<LichHenOBJ> GetList( int ID_NhanVien)
    {
        List<LichHenOBJ> lst = new List<LichHenOBJ>();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_NhanVien", ID_NhanVien)
            };
            DataTable dt = db.ExecuteDataSet("sp_LichHen_GetAll_ByIDNhanVien", param).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                LichHenOBJ rs = GetDuLieuFromDataRow(dr);
                lst.Add(rs);
            }


        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return lst;
    }
    public LichHenOBJ GetLichHen_ById(int ID_LichHen)
    {
        LichHenOBJ rs = new LichHenOBJ();

        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_LichHen", ID_LichHen)
        };

        DataSet ds = db.ExecuteDataSet("sp_LichHen_GetByID", pars);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            try
            {

                rs = GetDuLieuFromDataRow(dr);

            }
            catch
            {
                return null;
            }
        }
        return rs;
    }
     

    public int Them(LichHenOBJ obj)
    {
        int ret = 0;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_KhachHang", obj.ID_KhachHang),
                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("ID_NhanVien", obj.ID_NhanVien),
                    new SqlParameter("NoiDung", obj.NoiDung),
                      new SqlParameter("KetQua", obj.KetQua),
                    new SqlParameter("ThoiGian", obj.ThoiGian),
                };
            object objID = db.ExecuteScalar("sp_LichHen_ThemMoi", pars);
            if(objID != null)
            {
                //int i = db.ExecuteNonQuery("sp_LichHen_ThemMoi", pars);

                ret = Convert.ToInt32(objID);
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return ret;
    }
    public bool CapNhat(LichHenOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_LichHen", obj.ID_LichHen),
                    new SqlParameter("ID_KhachHang", obj.ID_KhachHang),
                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("ID_NhanVien", obj.ID_NhanVien),
                    new SqlParameter("NoiDung", obj.NoiDung),
                    new SqlParameter("KetQua", obj.KetQua),
                    new SqlParameter("ThoiGian", obj.ThoiGian),
                    new SqlParameter("TrangThai", obj.TrangThai),
                };
            int i = db.ExecuteNonQuery("sp_LichHen_Sua", pars);
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


    public DataTable GetDanhSachLichHen(int ID_NhanVien,int ID_KhachHang, DateTime TuNgay, DateTime DenNgay, int ID_QLLH, int ID_QuanLy)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_NhanVien", ID_NhanVien),
                new SqlParameter("@ID_KhachHang", ID_KhachHang),
                new SqlParameter("@TuNgay", TuNgay),
                new SqlParameter("@DenNgay", DenNgay),
                new SqlParameter("@ID_QLLH", ID_QLLH),
                new SqlParameter("@ID_QuanLy", ID_QuanLy)
            };
             dt = db.ExecuteDataSet("sp_LichHen_GetDanhSachLichHen", param).Tables[0];
             


        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return dt;
    }

}