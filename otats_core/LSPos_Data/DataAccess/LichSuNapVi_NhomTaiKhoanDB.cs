using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

/// <summary>
/// Summary description for CongTyDB
/// </summary>
public class LichSuNapVi_NhomTaiKhoanDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(LichSuNapVi_NhomTaiKhoanDB));
    public static SqlDataHelper db = new SqlDataHelper();
    public LichSuNapVi_NhomTaiKhoanDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int ThemLichSuNapVi(LichSuNapVi_NhomTaiKhoanModel obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {

                    new SqlParameter("ID_NhomTaiKhoan", obj.ID_NhomTaiKhoan),
                    new SqlParameter("NgayTao", obj.NgayTao),
                    new SqlParameter("SoTien", obj.SoTien),
                    new SqlParameter("TrangThai", obj.TrangThai),
                    new SqlParameter("ImgUrl", obj.ImgUrl),
                    new SqlParameter("CongThanhToan", obj.CongThanhToan),
                    new SqlParameter("DuLieuThanhToan", obj.DuLieuThanhToan)
                                    };
            int id = int.Parse(db.ExecuteScalar("sp_LichSuNapVi_Insert", pars).ToString());
            return id;

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return 0;
    }

    public bool UpdateThanhCong_LichSuNapVi(LichSuNapVi_NhomTaiKhoanModel obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {

                    new SqlParameter("ID", obj.ID),
                    new SqlParameter("SoTien", obj.SoTien),
                    new SqlParameter("TrangThai", obj.TrangThai),
                    new SqlParameter("ImgUrl", obj.ImgUrl),
                    new SqlParameter("CongThanhToan", obj.CongThanhToan),
                    new SqlParameter("DuLieuThanhToan", obj.DuLieuThanhToan)
                                    };
            int i = db.ExecuteNonQuery("sp_LichSuNapVi_Update", pars);
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

    public LichSuNapVi_NhomTaiKhoanModel GetLichSuNapViByID(int id)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID", id)
            };
            dt = db.ExecuteDataSet("sp_LichSuNapVi_GetByID", param).Tables[0];
            LichSuNapVi_NhomTaiKhoanModel item = GetObjectFromDataRowUtil<LichSuNapVi_NhomTaiKhoanModel>.ToOject(dt.Rows[0]);

            return item;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }


    public DataTable GetLichSuNapVi(int id)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_NhomTaiKhoan", id)
            };
            dt = db.ExecuteDataSet("sp_LichSuNapVi_GetByIDNhom", param).Tables[0];


            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return dt;
        }
    }
}