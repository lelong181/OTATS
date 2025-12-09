using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KeHoachBaoDuongDB
/// </summary>
public class HangHoa_LoiNhanData
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(SiteDAO));
    public static SqlDataHelper db = new SqlDataHelper();
    public HangHoa_LoiNhanData()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public List<HangHoa_LoiNhuanModel> GetAllByHangHoa(int ID_HangHoa)
    {
        List<HangHoa_LoiNhuanModel> rs = new List<HangHoa_LoiNhuanModel>();

        try
        {
            SqlParameter[] param = new SqlParameter[]
        {
                new SqlParameter("@ID_HangHoa", ID_HangHoa),

        };
            DataTable dt = db.ExecuteDataSet("sp_HangHoaLoiNhuan_GetByHangHoa", param).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                HangHoa_LoiNhuanModel item = GetObjectFromDataRowUtil<HangHoa_LoiNhuanModel>.ToOject(dr);
                rs.Add(item);
            }
            return rs;

        }
        catch (Exception ex)
        {
            log.Error(ex);
            return rs;
        }
    }

    public bool InsertOrUpdate(HangHoa_LoiNhuanModel item)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {

                    new SqlParameter("ID", item.ID),
                    new SqlParameter("ID_HangHoa", item.ID_HangHoa),
                    new SqlParameter("ID_NhomTaiKhoan", item.ID_NhomTaiKhoan),
                    new SqlParameter("SoLuongToiDa", item.SoLuongToiDa),
                    new SqlParameter("SoLuongToiThieu", item.SoLuongToiThieu),
                    new SqlParameter("TyLeHoaHong", item.TyLeHoaHong),
                    new SqlParameter("TienHoaHong", item.TienHoaHong)
                };
            int i = db.ExecuteNonQuery("sp_HangHoaLoiNhuan_InsertOrUpdate", pars);
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

    public bool UpdateTrangThai(int ID, int TrangThai)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {

                    new SqlParameter("@ID", ID),
                    new SqlParameter("@TrangThai", TrangThai)
                };
            int i = db.ExecuteNonQuery("sp_HangHoaLoiNhuan_UpdateTrangThai", pars);
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