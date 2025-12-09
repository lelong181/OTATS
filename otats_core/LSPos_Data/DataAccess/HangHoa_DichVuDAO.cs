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
public class HangHoa_DichVuDAO
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(SiteDAO));
    public static SqlDataHelper db = new SqlDataHelper();
    public HangHoa_DichVuDAO()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public List<HangHoa_DichVuModel> GetAllByHangHoa(int ID_HangHoa)
    {
        List<HangHoa_DichVuModel> rs = new List<HangHoa_DichVuModel>();
        SqlParameter[] param = new SqlParameter[]
        {
                new SqlParameter("@ID_HangHoa", ID_HangHoa),

        };
        try
        {
            DataTable dt = db.ExecuteDataSet("sp_HangHoa_DichVu_GetByHangHoa", param).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                HangHoa_DichVuModel item = GetObjectFromDataRowUtil<HangHoa_DichVuModel>.ToOject(dr);
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

    public bool InsertOrUpdate(HangHoa_DichVuModel item)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {

                    new SqlParameter("ID", item.ID),
                    new SqlParameter("TenHienThi", item.TenHienThi),
                    new SqlParameter("ID_HangHoa", item.ID_HangHoa),
                    new SqlParameter("ID_DichVu", item.ID_DichVu),
                    new SqlParameter("Loai", item.Loai),
                    new SqlParameter("SoLuong", item.SoLuong),
                    new SqlParameter("GiaBan", item.GiaBan),
                    new SqlParameter("HanSuDung", item.HanSuDung),
                    new SqlParameter("ID_NhaCungCap", item.ID_NhaCungCap),
                    new SqlParameter("TrangThai", item.TrangThai)
                };
            int i = db.ExecuteNonQuery("sp_HangHoa_DichVu_InsertOrUpdate", pars);
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

    public bool Delete(int ID)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {

                    new SqlParameter("ID", ID)                   
                };
            int i = db.ExecuteNonQuery("sp_HangHoa_DichVu_Delete", pars);
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