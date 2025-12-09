using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CongTyDB
/// </summary>
public class NhaCungCapDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(NhaCungCapDB));
    public static SqlDataHelper db = new SqlDataHelper();
    public NhaCungCapDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public NhaCungCapOBJ GetById(int ID)
    {
        NhaCungCapOBJ rs = new NhaCungCapOBJ();

        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_NhaCungCap", ID)
        };

        DataSet ds = db.ExecuteDataSet("sp_NhaCungCap_GetByID", pars);
        DataRow dr = ds.Tables[0].Rows[0];

        try
        {
            rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            rs.ID_NhaCungCap = int.Parse(dr["ID_NhaCungCap"].ToString());
            rs.TenNhaCungCap = dr["TenNhaCungCap"].ToString();
            rs.DiaChi = dr["DiaChi"].ToString();
            rs.NguoiLienHe = dr["NguoiLienHe"].ToString();
            rs.DienThoaiLienHe = dr["DienThoaiLienHe"].ToString();
            rs.TrangThai = dr["TrangThai"].ToString() != "" ? int.Parse(dr["TrangThai"].ToString()) : 0;
            rs.NgayTao = dr["NgayTao"].ToString() != "" ? DateTime.Parse(dr["NgayTao"].ToString()) : rs.NgayTao;
            rs.ProfileCode = dr["ProfileCode"].ToString();
            rs.ProfileID = dr["ProfileID"].ToString();
            rs.PaymentTypeID = dr["PaymentTypeID"].ToString();
            rs.AccountReceivableNo = dr["AccountReceivableNo"].ToString();
            rs.AccountReceivableBalance = decimal.Parse(dr["AccountReceivableBalance"].ToString());
            rs.SiteCode = dr["SiteCode"].ToString();
        }
        catch
        {
            return null;
        }
        return rs;
    }
    public NhaCungCapOBJ GetFromDataRow(DataRow dr)
    {
        NhaCungCapOBJ rs = new NhaCungCapOBJ();



        try
        {

            rs.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            rs.ID_NhaCungCap = int.Parse(dr["ID_NhaCungCap"].ToString());
            rs.TenNhaCungCap = dr["TenNhaCungCap"].ToString();
            rs.DiaChi = dr["DiaChi"].ToString();
            rs.NguoiLienHe = dr["NguoiLienHe"].ToString();
            rs.DienThoaiLienHe = dr["DienThoaiLienHe"].ToString();
            rs.TrangThai = dr["TrangThai"].ToString() != "" ? int.Parse(dr["TrangThai"].ToString()) : 0;
            rs.NgayTao = dr["NgayTao"].ToString() != "" ? DateTime.Parse(dr["NgayTao"].ToString()) : rs.NgayTao;
            rs.TenTrangThai = dr["TenTrangThai"].ToString();
            rs.ProfileCode = dr["ProfileCode"].ToString();
            rs.ProfileID = dr["ProfileID"].ToString();
            rs.PaymentTypeID = dr["PaymentTypeID"].ToString();
            rs.AccountReceivableNo = dr["AccountReceivableNo"].ToString();
            rs.AccountReceivableBalance = decimal.Parse(dr["AccountReceivableBalance"].ToString());
            rs.SiteCode = dr["SiteCode"].ToString();

        }
        catch
        {
            return null;
        }
        return rs;
    }
    public List<NhaCungCapOBJ> GetListDanhSach(int idct)
    {
        List<NhaCungCapOBJ> lst = new List<NhaCungCapOBJ>();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)
            };
            DataTable dt = db.ExecuteDataSet("sp_NhaCungCap_GetAll", param).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                NhaCungCapOBJ rs = GetFromDataRow(dr);
                lst.Add(rs);
            }


        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return lst;
    }


    public DataTable GetDanhSach(int idct)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)
            };
            dt = db.ExecuteDataSet("sp_NhaCungCap_GetAll", param).Tables[0];


            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return dt;
        }
    }
    public bool Them(NhaCungCapOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {

                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("TenNhaCungCap", obj.TenNhaCungCap),
                    new SqlParameter("NguoiLienHe", obj.NguoiLienHe),
                    new SqlParameter("DienThoaiLienHe", obj.DienThoaiLienHe),
                    new SqlParameter("DiaChi", obj.DiaChi),



                };
            int i = db.ExecuteNonQuery("sp_NhaCungCap_ThemMoi", pars);
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
    public bool Sua(NhaCungCapOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_NhaCungCap", obj.ID_NhaCungCap),
                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("TenNhaCungCap", obj.TenNhaCungCap),
                    new SqlParameter("NguoiLienHe", obj.NguoiLienHe),
                    new SqlParameter("DienThoaiLienHe", obj.DienThoaiLienHe),
                    new SqlParameter("DiaChi", obj.DiaChi),

                };
            int i = db.ExecuteNonQuery("sp_NhaCungCap_Sua", pars);
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
    public bool Xoa(int ID_NhaCungCap)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_NhaCungCap", ID_NhaCungCap)

                };
            int i = db.ExecuteNonQuery("sp_NhaCungCap_Xoa", pars);
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

    public bool ThemLichSuNapVi(LichSuNapVi_NhomTaiKhoanModel obj)
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
            int i = db.ExecuteNonQuery("sp_LichSuNapVi_Insert", pars);
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