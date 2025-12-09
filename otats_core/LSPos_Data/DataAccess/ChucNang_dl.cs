using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ChucNang_dl
/// </summary>
public class ChucNang_dl
{
    private SqlDataHelper helper;

    public ChucNangOBJ GetDataFromDataRow(DataRow dr)
    {
        try
        {
            ChucNangOBJ dv = new ChucNangOBJ();
            dv.ID_ChucNang = int.Parse(dr["ID_ChucNang"].ToString());
            dv.TenChucNang = dr["TenChucNang"].ToString();
            try
            {
                dv.TenNhomChucNang = dr["TenNhomChucNang"].ToString();
            }
            catch (Exception)
            {

                 
            }
            dv.MaChucNang = dr["MaChucNang"].ToString();
            dv.URL = dr["URL"].ToString();
            dv.InsertedTime = DateTime.Parse( dr["InsertedTime"].ToString());
            try
            {
                dv.Xoa = dr["Xoa"].ToString() != "" ? int.Parse(dr["Xoa"].ToString()) : 0;
                dv.Them = dr["Them"].ToString() != "" ? int.Parse(dr["Them"].ToString()) : 0;
                dv.Sua = dr["Sua"].ToString() != "" ? int.Parse(dr["Sua"].ToString()) : 0;
            }
            catch (Exception)
            {
 
            }

            return dv;
        }
        catch
        {
            return null;
        }
    }

    public ChucNang_dl()
    {
        //
        // TODO: Add constructor logic here
        //
        helper = new SqlDataHelper();
    }

    public DataTable GetDanhSachGoiChucNang()
    {
        DataTable dt = new DataTable();

        try
        {

            DataSet ds = helper.ExecuteDataSet("sp_GoiChucNang_GetDanhSach", null);
             dt = ds.Tables[0];
            return dt;
        }
        catch
        {
            return null;
        }
    }

    public List<ChucNangOBJ> GetAllChucNang()
    {
       
       

        DataSet ds = helper.ExecuteDataSet("sp_ChucNang_GetDanhSachChucNang_Web", null);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<ChucNangOBJ> dsdv = new List<ChucNangOBJ>();
            foreach (DataRow dr in dt.Rows)
            {
                ChucNangOBJ dv = new ChucNangOBJ();
                dv = GetDataFromDataRow(dr);
                dsdv.Add(dv);
            }

            return dsdv;
        }
        catch
        {
            return null;
        }
    }

    public DataTable GetAllChucNangTheoGoi()
    {


        DataTable dt = new DataTable();
        DataSet ds = helper.ExecuteDataSet("sp_ChucNang_GetDanhSachChucNangTheoGoi", null);
          dt = ds.Tables[0];

        

        try
        {
             

            return dt;
        }
        catch
        {
            return null;
        }
    }

    public List<ChucNangOBJ> getDanhSachQuyen_WEB(int IDQLLH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH)
        };

        DataSet ds = helper.ExecuteDataSet("sp_ChucNang_GetDanhSachChucNang_Web_TheoCongTy", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<ChucNangOBJ> dsdv = new List<ChucNangOBJ>();
            foreach (DataRow dr in dt.Rows)
            {
                ChucNangOBJ dv = new ChucNangOBJ();
                dv = GetDataFromDataRow(dr);
                dsdv.Add(dv);
            }

            return dsdv;
        }
        catch
        {
            return null;
        }
    }
    public DataTable getData_DanhSachQuyen_WEB(int IDQLLH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH)
        };

        DataSet ds = helper.ExecuteDataSet("sp_ChucNang_GetDanhSachChucNang_Web_TheoCongTy", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            

            return dt;
        }
        catch
        {
            return null;
        }
    }
    public DataTable getData_DanhSachQuyen_WEB_PhanQuyen(int IDQLLH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH)
        };

        DataSet ds = helper.ExecuteDataSet("sp_ChucNang_GetDanhSachChucNang_Web_TheoCongTy_PhanQuyen", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {


            return dt;
        }
        catch
        {
            return null;
        }
    }

    public List<ChucNangOBJ> getDanhSachQuyen_App(int IDQLLH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH)
        };

        DataSet ds = helper.ExecuteDataSet("sp_ChucNang_GetDanhSachChucNang_App_TheoCongTy", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<ChucNangOBJ> dsdv = new List<ChucNangOBJ>();
            foreach (DataRow dr in dt.Rows)
            {
                ChucNangOBJ dv = new ChucNangOBJ();
                dv = GetDataFromDataRow(dr);
                dsdv.Add(dv);
            }

            return dsdv;
        }
        catch
        {
            return null;
        }
    }
    public DataTable getData_DanhSachQuyen_App(int IDQLLH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH)
        };

        DataSet ds = helper.ExecuteDataSet("sp_ChucNang_GetDanhSachChucNang_App_TheoCongTy", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            return dt;
        }
        catch
        {
            return null;
        }
    }
    public ChucNangOBJ GetChucNangById(int ID_ChucNang)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_ChucNang", ID_ChucNang)
        };

        DataSet ds = helper.ExecuteDataSet("sp_ChucNang_getByID", pars);
        DataRow dr = ds.Tables[0].Rows[0];

        try
        {
            ChucNangOBJ dv = new ChucNangOBJ();
             dv = GetDataFromDataRow(dr);

            return dv;
        }
        catch
        {
            return null;
        }
    }
    public bool DeleteBy_ID_Goi(int ID_GoiChucNang)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_GoiChucNang", ID_GoiChucNang) 
            };

            if (helper.ExecuteNonQuery("sp_GoiChucNang_ChiTiet_Delete", pars) != 0)
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
    public bool AddQuyen(int ID_GoiChucNang, int ID_ChucNang)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_GoiChucNang", ID_GoiChucNang),
                 new SqlParameter("ID_ChucNang", ID_ChucNang)
            };

            if (helper.ExecuteNonQuery("sp_GoiChucNang_ChiTiet_Add", pars) != 0)
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
    public bool XoaQuyen(int ID_GoiChucNang, int ID_ChucNang)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_GoiChucNang", ID_GoiChucNang),
                 new SqlParameter("ID_ChucNang", ID_ChucNang)
            };

            if (helper.ExecuteNonQuery("sp_GoiChucNang_ChiTiet_Delete", pars) != 0)
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
    public static bool ThemNhomChucNang(NhomChucNangOBJ dm)
    {
        try
        {
            SqlDataHelper helper = new SqlDataHelper();
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("TenNhomChucNang", dm.TenNhomChucNang),
                new SqlParameter("STT", dm.STT) 
            };
            int i = helper.ExecuteNonQuery("sp_NhomChucNang_ThemMoi", pars);
            if (i > 0)
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

    public List<NhomChucNangOBJ> LayDSKHD()
    {
        try
        {
            List<NhomChucNangOBJ> list = new List<NhomChucNangOBJ>();
            DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllNhomChucNang");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new NhomChucNangOBJ
                {
                    ID_NhomChucNang = int.Parse(dr["ID_NhomChucNang"].ToString()),
                    TenNhomChucNang = "- " + dr["TenNhomChucNang"].ToString()
                });
            }
            return list;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public List<ChucNangOBJ> GetChucNang_IDNhomChucNang(int ID_NhomChucNang)
    {
        try
        {
            List<ChucNangOBJ> list = new List<ChucNangOBJ>();
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_NhomChucNang", ID_NhomChucNang)
            };

            DataTable dt = helper.ExecuteDataSet_INCLUDE_AutoIncrement("sp_GetChucNang_ID_NhomChucNang", par);
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new ChucNangOBJ
                {
                    STT = int.Parse(dr["STT"].ToString()),
                    ID_ChucNang = int.Parse(dr["ID_ChucNang"].ToString()),
                    URL = dr["URL"].ToString(),
                    TrangThai = int.Parse(dr["TrangThai"].ToString()),
                    TenTrangThai = dr["TrangThai"].ToString() == "1" ? "Hoạt động" : "Chưa hoạt động",
                    TenLoaiThietBi = dr["LoaiThietBi"].ToString(),
                    ID_NhomChucNang = int.Parse(dr["ID_NhomChucNang"].ToString()),
                    TenChucNang = dr["TenChucNang"].ToString(),
                    ViTriChucNang = dr["Priority"].ToString()
                });
            }
            return list;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public bool ThemChucNang(string TenChucNang, string URL, int TrangThai, int LoaiThietBi, int ID_NhomChucNang, int ViTri)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@TenChucNang", TenChucNang),
            new SqlParameter("@URL",URL),
            new SqlParameter("@TrangThai", TrangThai),
            new SqlParameter("@LoaiThietBi",LoaiThietBi),
            new SqlParameter("@ID_NhomChucNang",ID_NhomChucNang),
            new SqlParameter("@ViTri",ViTri == -1 ? (object)DBNull.Value : ViTri)
        };

            if (helper.ExecuteNonQuery("Insert_ChucNang", pars) != 0)
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
    public List<ChucNangOBJ> GetChucNang_IDChucNang(int ID_ChucNang)
    {
        try
        {
            List<ChucNangOBJ> list = new List<ChucNangOBJ>();
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_ChucNang", ID_ChucNang)
            };

            DataSet ds = helper.ExecuteDataSet("sp_web_GetID_ChucNang", par);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                list.Add(new ChucNangOBJ
                {
                    ID_ChucNang = int.Parse(dr["ID_ChucNang"].ToString()),
                    URL = dr["URL"].ToString(),
                    TrangThai = int.Parse(dr["TrangThai"].ToString()),
                    TenTrangThai = dr["TrangThai"].ToString() == "1" ? "Hoạt động" : "Chưa hoạt động",
                    TenLoaiThietBi = dr["LoaiThietBi"].ToString(),
                    ID_NhomChucNang = int.Parse(dr["ID_NhomChucNang"].ToString()),
                    TenChucNang = dr["TenChucNang"].ToString(),
                    ViTriChucNang = dr["Priority"] == DBNull.Value ? "" : dr["Priority"].ToString()
                });
            }
            return list;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public bool CapNhatChucNang(ChucNangOBJ cn)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_ChucNang", cn.ID_ChucNang),
                new SqlParameter("@TenChucNang", cn.TenChucNang),
                new SqlParameter("@URL", cn.URL),
                new SqlParameter("@TrangThai", cn.TrangThai),
                new SqlParameter("@LoaiThietBi", cn.LoaiThietBi),
                new SqlParameter("@ID_NhomChucNang", cn.ID_NhomChucNang),
                new SqlParameter("@ViTriChucNang", cn.ViTriChucNang == "" ? (object)DBNull.Value : int.Parse(cn.ViTriChucNang))
            };

            if (helper.ExecuteNonQuery("sp_QL_UpdateChucNang", pars) != 0)
            {
                // cap nhat thanh cong
                return true;
            }
            else
            {
                // cap nhat that bai
                return false;
            }
        }
        catch
        {
            return false;
        }
    }
    public bool DeleteChucNang(int ID_ChucNang)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_ChucNang", ID_ChucNang)
        };

            if (helper.ExecuteNonQuery("sp_QL_DeleteChucNang", pars) != 0)
            {
                // xoa thanh cong
                return true;
            }
            else
            {
                // xoa that bai
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    public ChucNangOBJ CheckQuyen(string MaChucNang, int ID_QuanLy)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("MaChucNang", MaChucNang),
             new SqlParameter("ID_QuanLy", ID_QuanLy)
        };

        DataSet ds = helper.ExecuteDataSet("sp_ChucNang_CheckQuyen", pars);
        DataRow dr = ds.Tables[0].Rows[0];

        try
        {
            ChucNangOBJ dv = new ChucNangOBJ();
            dv = GetDataFromDataRow(dr);

            return dv;
        }
        catch
        {
            return null;
        }
    }

}