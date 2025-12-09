using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DonVi_dl
/// </summary>
public class ChiTieu_KPI_dl
{
    private SqlDataHelper helper;

     

    public ChiTieu_KPI_dl()
    {
        //
        // TODO: Add constructor logic here
        //
        helper = new SqlDataHelper();
    }

        


    public bool Them(ChiTieuKPIOBJ dv)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
             //   new SqlParameter("ID_ChiTieuKPI", int.Parse("0")),
                new SqlParameter("ID_QLLH", dv.IDQLLH),
                new SqlParameter("ApDung_TuNgay", dv.ApDung_TuNgay),
                new SqlParameter("DoanhSo", dv.DoanhSo),
                new SqlParameter("ID_NhanVien", dv.ID_NhanVien),
                new SqlParameter("LuotViengTham", dv.LuotViengTham),
                new SqlParameter("NgayCong", dv.NgayCong),
                 new SqlParameter("SoDonHang", dv.SoDonHang),
            };

            if (helper.ExecuteNonQuery("sp_ChiTieuKPI_ThemMoi", pars) != 0)
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

    

    public bool CapNhat(ChiTieuKPIOBJ dv)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_ChiTieuKPI", dv.ID_ChiTieuKPI),
                new SqlParameter("ID_QLLH", dv.IDQLLH),
                new SqlParameter("ApDung_TuNgay", dv.ApDung_TuNgay),
                new SqlParameter("DoanhSo", dv.DoanhSo),
                new SqlParameter("ID_NhanVien", dv.ID_NhanVien),
                new SqlParameter("LuotViengTham", dv.LuotViengTham),
                new SqlParameter("NgayCong", dv.NgayCong),
                 new SqlParameter("SoDonHang", dv.SoDonHang),
                  new SqlParameter("TrangThai", dv.TrangThai),
            };

            if (helper.ExecuteNonQuery("sp_ChiTieuKPI_Update", pars) != 0)
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
    public DataTable GetChiTieuTheoNhanVienKPI(int ID_QLLH, int ID_Nhom)
    {
        DataTable dt = null;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", ID_QLLH),
                new SqlParameter("ID_Nhom", ID_Nhom)
                };

            DataSet ds = helper.ExecuteDataSet("sp_ChiTieuKPI_GetDanhSach", pars);
            dt = ds.Tables[0];





        }
        catch (Exception ex)
        {
            return null;
        }

        return dt;
    }
    public ChiTieuKPIOBJ GetChiTieuKPI(int ID_ChiTieuKPI)
    {
        ChiTieuKPIOBJ dt = null;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_ChiTieuKPI", ID_ChiTieuKPI)
                };

            DataSet ds = helper.ExecuteDataSet("sp_ChiTieuKPI_GetById", pars);
            dt = GetFromDataRow(ds.Tables[0].Rows[0]);





        }
        catch (Exception ex)
        {
            return null;
        }

        return dt;
    }
    public DataTable GetChiTieuKPI_TheoID(int ID_ChiTieuKPI)
    {
        DataTable dt = null;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_ChiTieuKPI", ID_ChiTieuKPI)
                };

            DataSet ds = helper.ExecuteDataSet("sp_ChiTieuKPI_GetById", pars);
            dt = ds.Tables[0];





        }
        catch (Exception ex)
        {
            return null;
        }

        return dt;
    }
    public ChiTieuKPIOBJ GetFromDataRow(DataRow dr)
    {
        try
        {
            ChiTieuKPIOBJ dv = new ChiTieuKPIOBJ();
            dv.IDQLLH = int.Parse(dr["ID_QLLH"].ToString());
            dv.ID_NhanVien = int.Parse(dr["ID_NhanVien"].ToString());
            dv.TrangThai = int.Parse(dr["TrangThai"].ToString());
            dv.ID_ChiTieuKPI = int.Parse(dr["ID_ChiTieuKPI"].ToString());
            dv.LuotViengTham = dr["LuotViengTham"].ToString() != "" ?  double.Parse(dr["LuotViengTham"].ToString()) : 0;
            dv.NgayCong = dr["NgayCong"].ToString() != "" ? double.Parse(dr["NgayCong"].ToString()) : 0;
            dv.SoDonHang = dr["SoDonHang"].ToString() != "" ? double.Parse(dr["SoDonHang"].ToString()) : 0;
            dv.DoanhSo = dr["DoanhSo"].ToString() != "" ? double.Parse(dr["DoanhSo"].ToString()) : 0;
            dv.ApDung_TuNgay = dr["ApDung_TuNgay"].ToString() != "" ? Convert.ToDateTime(dr["ApDung_TuNgay"].ToString()) : dv.ApDung_TuNgay;
            return dv;
        }
        catch
        {
            return null;
        }
    }
    

    public bool Delete(int ID_ChiTieuKPI)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_ChiTieuKPI",ID_ChiTieuKPI),
               
            };

            if (helper.ExecuteNonQuery("sp_ChiTieuKPI_Delete", pars) != 0)
            {
                // delete thanh cong
                return true;
            }
            else
            {
                // delete that bai
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
}