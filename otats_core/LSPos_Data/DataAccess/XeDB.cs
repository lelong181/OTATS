using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for XeDB
/// </summary>
public class XeDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(XeDB));
    public static SqlDataHelper db = new SqlDataHelper();
    public XeDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public XeOBJ GetById(int ID_Xe)
    {
        XeOBJ rs = new XeOBJ();

        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_Xe", ID_Xe)
        };

        DataSet ds = db.ExecuteDataSet("sp_Xe_GetByID", pars);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            try
            {

                rs = GetFromDataRow(dr);
            }
            catch
            {
                return null;
            }
        }
        return rs;
    }

     

    public XeOBJ GetFromDataRow(DataRow dr)
    {
        XeOBJ rs = new XeOBJ();
 
      

        try
        {

            rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            rs.BienKiemSoat = dr["BienKiemSoat"].ToString();
            rs.ID_Xe = dr["ID_Xe"].ToString() != "" ? int.Parse(dr["ID_Xe"].ToString()) : 0;
            rs.LoaiXe = dr["LoaiXe"].ToString();
            rs.MoTa = dr["MoTa"].ToString();
            rs.NamSanXuat = dr["NamSanXuat"].ToString();
            rs.SoCho = dr["SoCho"].ToString();
            rs.NgayTao = dr["NgayTao"].ToString() != "" ? DateTime.Parse(dr["NgayTao"].ToString()) : rs.NgayTao;
            rs.NgayBDGanNhat = dr["NgayBDGanNhat"].ToString() != "" ? DateTime.Parse(dr["NgayBDGanNhat"].ToString()) : rs.NgayBDGanNhat;
            rs.NgayBDTiepTheo = dr["NgayBDTiepTheo"].ToString() != "" ? DateTime.Parse(dr["NgayBDTiepTheo"].ToString()) : rs.NgayBDTiepTheo;
            rs.ChuKyBaoDuong = dr["ChuKyBaoDuong"].ToString() != "" ? int.Parse(dr["ChuKyBaoDuong"].ToString()) : 0;
            rs.ID_NhanVien = dr["ID_NhanVien"].ToString() != "" ? int.Parse(dr["ID_NhanVien"].ToString()) : 0;
             
        }
        catch
        {
            return null;
        }
        return rs;
    }
    public List<XeOBJ> GetListDanhSach (int idct)
    {
        List<XeOBJ> lst = new List<XeOBJ>();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)
            };
           DataTable  dt = db.ExecuteDataSet("sp_Xe_GetAll", param).Tables[0];
            foreach(DataRow dr in dt.Rows)
            {
                XeOBJ rs = GetFromDataRow(dr);
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
            dt = db.ExecuteDataSet("sp_Xe_GetAll", param).Tables[0];


            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return dt;
        }
    }

    public DataTable GetLichSuSuDung(int ID_Xe)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_Xe", ID_Xe)
            };
            dt = db.ExecuteDataSet("sp_Xe_NhanVien_GetAll", param).Tables[0];


            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return dt;
        }
    }
    public DataTable GetLichSuBaoDuong(int ID_Xe)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_Xe", ID_Xe)
            };
            dt = db.ExecuteDataSet("sp_Xe_LichSuBD_GetAll", param).Tables[0];


            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return dt;
        }
    }
    public bool Them(XeOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter parNgayBDGanNhat = new SqlParameter("NgayBDGanNhat", DBNull.Value);
            if(obj.NgayBDGanNhat.Year > 1900)
            {
                parNgayBDGanNhat = new SqlParameter("NgayBDGanNhat", obj.NgayBDGanNhat);
            }

            SqlParameter parNgayBDTiepTheo = new SqlParameter("NgayBDTiepTheo", DBNull.Value);
            if (obj.NgayBDTiepTheo.Year > 1900)
            {
                parNgayBDTiepTheo = new SqlParameter("NgayBDTiepTheo", obj.NgayBDTiepTheo);
            }
            SqlParameter[] pars = new SqlParameter[] {
                 
                    new SqlParameter("BienKiemSoat", obj.BienKiemSoat),
                    new SqlParameter("ChuKyBaoDuong", obj.ChuKyBaoDuong),
                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                      new SqlParameter("LoaiXe", obj.LoaiXe),
                      new SqlParameter("MoTa", obj.MoTa),
                      new SqlParameter("NamSanXuat", obj.NamSanXuat),
                      new SqlParameter("SoCho", obj.SoCho),
                       new SqlParameter("ID_NhanVien", obj.ID_NhanVien),
                        parNgayBDGanNhat,
                        parNgayBDTiepTheo

                };
            int i = db.ExecuteNonQuery("sp_Xe_ThemMoi", pars);
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
    public bool Sua(XeOBJ obj, int ID_QuanLy)
    {
        bool ret = false;
        try
        {
            SqlParameter parNgayBDGanNhat = new SqlParameter("NgayBDGanNhat", DBNull.Value);
            if (obj.NgayBDGanNhat.Year > 1900)
            {
                parNgayBDGanNhat = new SqlParameter("NgayBDGanNhat", obj.NgayBDGanNhat);
            }

            SqlParameter parNgayBDTiepTheo = new SqlParameter("NgayBDTiepTheo", DBNull.Value);
            if (obj.NgayBDTiepTheo.Year > 1900)
            {
                parNgayBDTiepTheo = new SqlParameter("NgayBDTiepTheo", obj.NgayBDTiepTheo);
            }

            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_Xe", obj.ID_Xe),
                new SqlParameter("BienKiemSoat", obj.BienKiemSoat),
                    new SqlParameter("ChuKyBaoDuong", obj.ChuKyBaoDuong),
                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                      new SqlParameter("LoaiXe", obj.LoaiXe),
                      new SqlParameter("MoTa", obj.MoTa),
                      new SqlParameter("NamSanXuat", obj.NamSanXuat),
                      new SqlParameter("SoCho", obj.SoCho),
                      new SqlParameter("ID_QuanLy", ID_QuanLy),
                      new SqlParameter("ID_NhanVien", obj.ID_NhanVien),
                       parNgayBDGanNhat,
                        parNgayBDTiepTheo
                };
            int i = db.ExecuteNonQuery("sp_Xe_CapNhat", pars);
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
    public bool Xoa(int ID_Xe, int ID_QuanLy)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_Xe", ID_Xe),
                    new SqlParameter("ID_QuanLy", ID_QuanLy)
                };
            int i = db.ExecuteNonQuery("sp_Xe_Xoa", pars);
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
    //public DataTable GetNhanVien_QuanLyXe(int ID_Xe)
    //{
    //    DataTable dt = new DataTable();
    //    try
    //    {
    //        SqlParameter[] param = new SqlParameter[]
    //        {
    //            new SqlParameter("@ID_Kho", ID_Kho)
    //        };
    //        dt = db.ExecuteDataSet("sp_Kho_GetNhanVienQuanLy", param).Tables[0];


    //        return dt;
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
    //        return dt;
    //    }
    //}
}