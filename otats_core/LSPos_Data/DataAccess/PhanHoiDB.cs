using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PhanHoi
/// </summary>
public class PhanHoiDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(PhanHoiDB));
    public static SqlDataHelper db = new SqlDataHelper();
    public PhanHoiDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public PhanHoiOBJ GetPhanHoi(DataRow dr)
    {
        PhanHoiOBJ rs = new PhanHoiOBJ();



        try
        {

            rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            rs.ID_PhanHoi = int.Parse(dr["ID_PhanHoi"].ToString());
            rs.TenPhanHoi = dr["TenPhanHoi"].ToString();
            //srs.ID_CheckIn =  dr["ID_CheckIn"].ToString() != "" ? int.Parse(dr["ID_CheckIn"].ToString()) : 0;
            //rs.ThoiGian = dr["ThoiGian"].ToString() != "" ? DateTime.Parse(dr["ThoiGian"].ToString()) : rs.ThoiGian;

        }
        catch
        {
            return null;
        }
        return rs;
    }
    public DataTable GetDataList(int idct)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)

            };
            dt = db.ExecuteDataSet("sp_PhanHoi_GetAll", param).Tables[0];




        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return dt;
    }
    public List<PhanHoiOBJ> GetList (int idct)
    {
        List<PhanHoiOBJ> lst = new List<PhanHoiOBJ>();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)
            };
            DataTable dt = db.ExecuteDataSet("sp_PhanHoi_GetAll", param).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                PhanHoiOBJ rs = GetPhanHoi(dr);
                lst.Add(rs);
            }


        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return lst;
    }
    public static List<LichSuPhanHoi> LichSu(int idqllh, int idnhanvien, int idkhachhang, DateTime tungay, DateTime denngay, int loctatcanhanvien)
    {
        List<LichSuPhanHoi> rs = new List<LichSuPhanHoi>();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@idkhachhang", idkhachhang),
                  new SqlParameter("@idqllh", idqllh),
                new SqlParameter("@idnhanvien", idnhanvien),
                new SqlParameter("@tungay", tungay),
                new SqlParameter("@loctatcanhanvien", loctatcanhanvien),
                new SqlParameter("@denngay", denngay.ToString("yyyy-MM-dd 23:59:59"))
            };

            DataTable dt = db.ExecuteDataSet("sp_LichSuPhanHoi", param).Tables[0];
            DateTime d;
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    rs.Add(new LichSuPhanHoi
                    {
                        ChiTiet = !DBNull.Value.Equals(dr["ChiTiet"]) ? dr["ChiTiet"].ToString() : "",
                        idcheckin = !DBNull.Value.Equals(dr["ID_CheckIn"]) ? int.Parse(dr["ID_CheckIn"].ToString()) : 0,
                        ID_KhachHang = !DBNull.Value.Equals(dr["ID_KhachHang"]) ? int.Parse(dr["ID_KhachHang"].ToString()) : 0,
                        ID_NhanVien = !DBNull.Value.Equals(dr["ID_NhanVien"]) ? int.Parse(dr["ID_NhanVien"].ToString()) : 0,
                        ID_PhanHoi = !DBNull.Value.Equals(dr["ID_PhanHoi"]) ? int.Parse(dr["ID_PhanHoi"].ToString()) : 0,
                        //ID_QLLH = !DBNull.Value.Equals(dr["ID_QLLH"]) ? int.Parse(dr["ID_QLLH"].ToString()) : 0,
                        TenPhanHoi = !DBNull.Value.Equals(dr["TenPhanHoi"]) ? dr["TenPhanHoi"].ToString() : "",
                        TenKhachHang = !DBNull.Value.Equals(dr["TenKhachHang"]) ? dr["TenKhachHang"].ToString() : "",

                    });
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return rs;
    }

    public PhanHoiOBJ GetPhanHoiById(int ID_PhanHoi)
    {
        PhanHoiOBJ rs = new PhanHoiOBJ();

        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_PhanHoi", ID_PhanHoi)
        };

        DataSet ds = db.ExecuteDataSet("sp_PhanHoi_GetByID", pars);
        DataRow dr = ds.Tables[0].Rows[0];

        try
        {

            rs = GetPhanHoi(dr);

        }
        catch
        {
            return null;
        }
        return rs;
    }
    public DataTable LayDanhSachLoaiKhachHang(int idct)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)
            };
            dt = db.ExecuteDataSet("sp_LoaiKhachHang_GetAll", param).Tables[0];

            
            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return dt;
        }
    }

    public bool ThemLichSu(LichSuPhanHoi obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_PhanHoi", obj.ID_PhanHoi),
                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("ID_NhanVien", obj.ID_NhanVien),
                    new SqlParameter("ChiTiet", obj.ChiTiet),
                     new SqlParameter("ID_CheckIn", obj.idcheckin),
                     new SqlParameter("ID_KhachHang", obj.ID_KhachHang),
                    //new SqlParameter("ThoiGian", obj.ThoiGian),
                };
            int i = db.ExecuteNonQuery("sp_PhanHoiLichSu_ThemMoi", pars);
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

    public bool Xoa(int id)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                  new SqlParameter("ID_PhanHoi",  id),

                };
            int i = db.ExecuteNonQuery("sp_PhanHoi_Xoa", pars);
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
    public bool ThemMoi(PhanHoiOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                  new SqlParameter("TenPhanHoi",  obj.TenPhanHoi),
                    new SqlParameter("ID_QLLH",  obj.ID_QLLH),

                };
            int i = db.ExecuteNonQuery("sp_PhanHoi_Them", pars);
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
    public bool CapNhat(PhanHoiOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_PhanHoi",  obj.ID_PhanHoi),
                  new SqlParameter("TenPhanHoi",  obj.TenPhanHoi),
                    new SqlParameter("ID_QLLH",  obj.ID_QLLH),

                };
            int i = db.ExecuteNonQuery("sp_PhanHoi_CapNhat", pars);
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


}