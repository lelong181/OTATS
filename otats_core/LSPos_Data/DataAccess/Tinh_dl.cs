using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Tinh_dl
/// </summary>
public class Tinh_dl
{
    static log4net.ILog log = log4net.LogManager.GetLogger(typeof(Tinh_dl));
    private SqlDataHelper helper;

    //public Tinh GetTinhFromDataRow(DataRow dr)
    //{
    //    try
    //    {
    //        Tinh tinh = new Tinh();
    //        tinh.ID_Tinh = int.Parse(dr["ID_Tinh"].ToString());
    //        tinh.TenTinh = dr["TenTinh"].ToString();
    //        return tinh;
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}

    public Tinh_dl()
    {
        helper = new SqlDataHelper();
    }

    public List<Tinh> GetTinhAll(int ID_QLLH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH)
        };
        DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllTinh", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<Tinh> dsTinh = new List<Tinh>();
            foreach (DataRow dr in dt.Rows)
            {
                Tinh tinh = new Tinh();
                tinh.ID_Tinh = int.Parse(dr["ID_Tinh"].ToString());
                tinh.TenTinh = dr["TenTinh"].ToString();
                tinh.ID_KhuVuc = dr["ID_KhuVuc"].ToString() != "" ? int.Parse(dr["ID_KhuVuc"].ToString()) : 0;
                dsTinh.Add(tinh);
            }

            return dsTinh;
        }
        catch
        {
            return null;
        }
    }
    public List<Tinh> GetTinhAll()
    {

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllTinh");
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<Tinh> dsTinh = new List<Tinh>();
            foreach (DataRow dr in dt.Rows)
            {
                Tinh tinh = new Tinh();
                tinh.ID_Tinh = int.Parse(dr["ID_Tinh"].ToString());
                tinh.TenTinh = dr["TenTinh"].ToString();
                tinh.ID_KhuVuc = dr["ID_KhuVuc"].ToString() != "" ? int.Parse(dr["ID_KhuVuc"].ToString()) : 0;
                dsTinh.Add(tinh);
            }

            return dsTinh;
        }
        catch
        {
            return null;
        }
    }

    public Tinh GetTinhByID(int ID_Tinh)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_Tinh", ID_Tinh)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetTinhTheoID", pars);
        DataRow dr = ds.Tables[0].Rows[0];

        try
        {
            Tinh tinh = new Tinh();
            tinh.ID_Tinh = int.Parse(dr["ID_Tinh"].ToString());
            tinh.TenTinh = dr["TenTinh"].ToString();

            return tinh;
        }
        catch
        {
            return null;
        }
    }
    public static  Tinh GetTinhByTen(string  TenTinh)
    {
        Tinh tinh = new Tinh();
        tinh.ID_Tinh = 0;
        SqlDataHelper helper = new SqlDataHelper();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("Ten", TenTinh)
        };

        DataSet ds = helper.ExecuteDataSet("sp_Tinh_GetByName", pars);
       
        
        try
        {
            DataRow dr = ds.Tables[0].Rows[0];
            tinh.ID_Tinh = int.Parse(dr["ID_Tinh"].ToString());
            tinh.TenTinh = dr["TenTinh"].ToString();

           
        }
        catch (Exception ex)
        {
            log.Info("Error : TenTinh : " + TenTinh);
            log.Error(ex);

        }
        return tinh;
    }
    public string GetTenTinhTheoCongTy(int ID_QLLH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH)
        };
        try
        {
            return helper.ExecuteScalar("GetTenTinhTheoCongTy", pars).ToString();

        }
        catch
        {
            return "";
        }
    }
}



/// <summary>
/// Summary description for Quan_dl
/// </summary>
public class Quan_dl
{
    static log4net.ILog log = log4net.LogManager.GetLogger(typeof(Quan_dl));
    private SqlDataHelper helper;

    //public Quan GetQuanFromDataRow(DataRow dr)
    //{
    //    try
    //    {
    //        Quan quan = new Quan();
    //        quan.ID_Quan = int.Parse(dr["ID_Quan"].ToString());
    //        quan.TenQuan = dr["TenQuan"].ToString();
    //        quan.ID_Tinh = int.Parse(dr["ID_Tinh"].ToString());
    //        return quan;
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}

    public Quan_dl()
    {
        helper = new SqlDataHelper();
    }

    public List<Quan> GetQuanTheoTinh(int ID_Tinh)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_Tinh", ID_Tinh)
        };
        DataSet ds = helper.ExecuteDataSet("sp_QL_GetQuanTheoTinh", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<Quan> dsQuan = new List<Quan>();
            foreach (DataRow dr in dt.Rows)
            {
                Quan quan = new Quan();
                quan.ID_Quan = int.Parse(dr["ID_Quan"].ToString());
                quan.TenQuan = dr["TenQuan"].ToString();
                quan.ID_Tinh = int.Parse(dr["ID_Tinh"].ToString());
                dsQuan.Add(quan);
            }

            return dsQuan;
        }
        catch
        {
            return null;
        }
    }

    public Quan GetQuanByID(int ID_Quan)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_Quan", ID_Quan)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetQuanTheoID", pars);
        DataRow dr = ds.Tables[0].Rows[0];

        try
        {
            Quan quan = new Quan();
            quan.ID_Quan = int.Parse(dr["ID_Quan"].ToString());
            quan.TenQuan = dr["TenQuan"].ToString();
            quan.ID_Tinh = int.Parse(dr["ID_Tinh"].ToString());

            return quan;
        }
        catch
        {
            return null;
        }
    }



    public static Quan GetQuanByTen(string TenQuan, int ID_Tinh)
    {
        Quan quan = new Quan();
        quan.ID_Quan = 0;
        SqlDataHelper helper = new SqlDataHelper();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("Ten", TenQuan),
             new SqlParameter("ID_Tinh", ID_Tinh)
        };

        DataSet ds = helper.ExecuteDataSet("sp_Quan_GetByName", pars);
       

        try
        {
            DataRow dr = ds.Tables[0].Rows[0];
            quan.ID_Quan = int.Parse(dr["ID_Quan"].ToString());
            quan.TenQuan = dr["TenQuan"].ToString();
            quan.ID_Tinh = int.Parse(dr["ID_Tinh"].ToString());

            
        }
        catch (Exception ex)
        {
            log.Info("Error : TenQuan : " + TenQuan);
            log.Error(ex);
            
        }
        return quan;
    }
}



/// <summary>
/// Summary description for Phuong_dl
/// </summary>
public class Phuong_dl
{
    private SqlDataHelper helper;
    static log4net.ILog log = log4net.LogManager.GetLogger(typeof(Phuong_dl));
    //public Tinh GetPhuongFromDataRow(DataRow dr)
    //{
    //    try
    //    {
    //        Tinh tinh = new Tinh();
    //        tinh.ID_Tinh = int.Parse(dr["ID_Tinh"].ToString());
    //        tinh.TenTinh = dr["TenTinh"].ToString();
    //        return tinh;
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}

    public Phuong_dl()
    {
        helper = new SqlDataHelper();
    }

    public List<Phuong> GetPhuongTheoQuan(int ID_Quan)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_Quan", ID_Quan)
        };
        DataSet ds = helper.ExecuteDataSet("sp_QL_GetPhuongTheoQuan", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<Phuong> dsPhuong = new List<Phuong>();
            foreach (DataRow dr in dt.Rows)
            {
                Phuong phuong = new Phuong();
                phuong.ID_Phuong = int.Parse(dr["ID_Phuong"].ToString());
                phuong.TenPhuong = dr["TenPhuong"].ToString();
                phuong.ID_Quan = int.Parse(dr["ID_Quan"].ToString());
                dsPhuong.Add(phuong);
            }

            return dsPhuong;
        }
        catch
        {
            return null;
        }
    }

    public Phuong GetPhuongByID(int ID_Phuong)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_Phuong", ID_Phuong)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetPhuongTheoID", pars);
        DataRow dr = ds.Tables[0].Rows[0];

        try
        {
            Phuong phuong = new Phuong();
            phuong.ID_Phuong = int.Parse(dr["ID_Phuong"].ToString());
            phuong.TenPhuong = dr["TenPhuong"].ToString();
            phuong.ID_Quan = int.Parse(dr["ID_Quan"].ToString());
            return phuong;
        }
        catch
        {
            return null;
        }
    }

    public static Phuong GetPhuongByTen(string TenPhuong,int ID_Quan)
    {
        Phuong phuong = new Phuong();
        phuong.ID_Phuong = 0;
        SqlDataHelper helper = new SqlDataHelper();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("Ten", TenPhuong),
             new SqlParameter("ID_Quan", ID_Quan)
        };

        DataSet ds = helper.ExecuteDataSet("sp_Phuong_GetByName", pars);
       

        try
        {
            DataRow dr = ds.Tables[0].Rows[0];
            phuong.ID_Phuong = int.Parse(dr["ID_Phuong"].ToString());
            phuong.TenPhuong = dr["TenPhuong"].ToString();
            phuong.ID_Quan = int.Parse(dr["ID_Quan"].ToString());
            
        }
        catch (Exception ex)
        {
            log.Info("Error : TenPhuong : " + TenPhuong);
            log.Error(ex);
            
        }
        return phuong;
    }
}

public class DuongPho_dl
{
    private SqlDataHelper helper;

    //public Tinh GetPhuongFromDataRow(DataRow dr)
    //{
    //    try
    //    {
    //        Tinh tinh = new Tinh();
    //        tinh.ID_Tinh = int.Parse(dr["ID_Tinh"].ToString());
    //        tinh.TenTinh = dr["TenTinh"].ToString();
    //        return tinh;
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}

    public DuongPho_dl()
    {
        helper = new SqlDataHelper();
    }

    public List<DuongPho> GetAllDuong()
    {
         
        DataSet ds = helper.ExecuteDataSet("sp_DuongPho_GetAll", null);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<DuongPho> dsPhuong = new List<DuongPho>();
            foreach (DataRow dr in dt.Rows)
            {
                DuongPho phuong = new DuongPho();
                phuong.ID_DuongPho = int.Parse(dr["ID_DuongPho"].ToString());
                phuong.TenDuongPho = dr["TenDuongPho"].ToString();
                 
                dsPhuong.Add(phuong);
            }

            return dsPhuong;
        }
        catch
        {
            return null;
        }
    }

    


}