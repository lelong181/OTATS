using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CongTyDB
/// </summary>
public class CheckListDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(CheckListDB));
    public static SqlDataHelper db = new SqlDataHelper();
    public CheckListDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public CheckListOBJ GetCheckList(DataRow dr)
    {
        CheckListOBJ rs = new CheckListOBJ();



        try
        {

            rs.ID_QLLH = dr["ID_QLLH"].ToString() != "" ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
            rs.ID_CheckList = int.Parse(dr["ID_CheckList"].ToString());
            rs.TenCheckList = dr["TenCheckList"].ToString();
            try
            {
                rs.TrangThai = dr["TrangThai"].ToString() != "" ? int.Parse(dr["TrangThai"].ToString()) : 0;
                rs.TenTrangThai = rs.TrangThai == 0 ? "Không tốt" : "Tốt";
            }
            catch (Exception ex)
            {

                
            }
            //rs.ThoiGian = dr["ThoiGian"].ToString() != "" ? DateTime.Parse(dr["ThoiGian"].ToString()) : rs.ThoiGian;

        }
        catch
        {
            return null;
        }
        return rs;
    }
    public List<CheckListOBJ> GetListCheckList(int idct, int idkhachhang, int idcheckin)
    {
        List<CheckListOBJ> lst = new List<CheckListOBJ>();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct),
                new SqlParameter("@idkhachhang", idkhachhang)
            };
            DataTable dt = db.ExecuteDataSet("sp_CheckList_GetAll", param).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                CheckListOBJ rs = GetCheckList(dr);
                lst.Add(rs);
            }
            if (idcheckin > 0)
            {
                SqlParameter[] param_CheckIn = new SqlParameter[]
                {
                new SqlParameter("@ID_QLLH", idct),
                new SqlParameter("@idcheckin", idcheckin)
                };
                DataTable dtCheckIn = db.ExecuteDataSet("sp_CheckList_GetBy_ID_CheckIn", param_CheckIn).Tables[0];
                foreach (DataRow dr in dtCheckIn.Rows)
                {
                    foreach(CheckListOBJ ck in lst)
                    {
                        if (dr["ID_CheckList"].ToString() == ck.ID_CheckList.ToString())
                        {
                            ck.DaCheck = 1;
                            break;
                        }
                    }
                   
                }
            }

            

        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return lst;
    }

   

    public CheckListOBJ GetCheckListById(int ID_CheckList)
    {
        CheckListOBJ rs = new CheckListOBJ();

        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_CheckList", ID_CheckList)
        };

        DataSet ds = db.ExecuteDataSet("sp_CheckList_GetByID", pars);
        DataRow dr = ds.Tables[0].Rows[0];

        try
        {

            rs = GetCheckList(dr);

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

    public bool ThemLichSu(LichSuCheckList obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("ID_CheckList", obj.ID_CheckList),
                    new SqlParameter("ID_QLLH", obj.ID_QLLH),
                    new SqlParameter("ID_NhanVien", obj.ID_NhanVien),
                    new SqlParameter("ChiTiet", obj.ChiTiet != null ? obj.ChiTiet : ""),
                     new SqlParameter("ID_CheckIn", obj.idcheckin),
                    //new SqlParameter("ThoiGian", obj.ThoiGian),
                };
            int i = db.ExecuteNonQuery("sp_CheckListLichSu_ThemMoi", pars);
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
    public bool XoaLichSuCheckList_TheoCheckIn(int idcheckin,int idnhanvien)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                  new SqlParameter("ID_NhanVien",  idnhanvien),
                     new SqlParameter("ID_CheckIn",  idcheckin),
                    
                };
            int i = db.ExecuteNonQuery("sp_CheckListLichSu_Xoa_Theo_ID_CheckIn", pars);
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
                  new SqlParameter("ID_CheckList",  id), 

                };
            int i = db.ExecuteNonQuery("sp_CheckList_Xoa", pars);
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
    public bool ThemMoi(CheckListOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                  new SqlParameter("TenCheckList",  obj.TenCheckList),
                    new SqlParameter("ID_QLLH",  obj.ID_QLLH),
                   
                };
            int i = db.ExecuteNonQuery("sp_CheckList_Them", pars);
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
    public bool CapNhat(CheckListOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_CheckList",  obj.ID_CheckList),
                  new SqlParameter("TenCheckList",  obj.TenCheckList),
                    new SqlParameter("ID_QLLH",  obj.ID_QLLH),

                };
            int i = db.ExecuteNonQuery("sp_CheckList_CapNhat", pars);
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
    public static List<LichSuCheckList> LichSu( int idqllh, int idnhanvien, int idkhachhang, DateTime tungay, DateTime denngay)
    {
        List<LichSuCheckList> rs = new List<LichSuCheckList>();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@idkhachhang", idkhachhang),
                  new SqlParameter("@idqllh", idqllh),
                new SqlParameter("@idnhanvien", idnhanvien),
                new SqlParameter("@tungay", tungay),
                new SqlParameter("@denngay", denngay)
            };

            DataTable dt = db.ExecuteDataSet("sp_LichSuCheckList", param).Tables[0];
            DateTime d;
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    rs.Add(new LichSuCheckList
                    {
                        ChiTiet = !DBNull.Value.Equals(dr["ChiTiet"]) ? dr["ChiTiet"].ToString() : "",
                        idcheckin = !DBNull.Value.Equals(dr["idcheckin"]) ? int.Parse(dr["idcheckin"].ToString()) : 0,
                        ID_KhachHang = !DBNull.Value.Equals(dr["ID_KhachHang"]) ? int.Parse(dr["ID_KhachHang"].ToString()) : 0,
                        ID_NhanVien = !DBNull.Value.Equals(dr["ID_NhanVien"]) ? int.Parse(dr["ID_NhanVien"].ToString()) : 0,
                        ID_CheckList = !DBNull.Value.Equals(dr["ID_CheckList"]) ? int.Parse(dr["ID_CheckList"].ToString()) : 0,
                        ID_QLLH = !DBNull.Value.Equals(dr["ID_QLLH"]) ? int.Parse(dr["ID_QLLH"].ToString()) : 0,
                        TenCheckList = !DBNull.Value.Equals(dr["TenCheckList"]) ? dr["TenCheckList"].ToString() : "",
                        TenKhachHang = !DBNull.Value.Equals(dr["TenKhachHang"]) ? dr["TenKhachHang"].ToString() : "",

                    });
                }
                catch
                {
                }
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return rs;
    }

    public List<CheckListOBJ> GetListCheckList_ByKhachHang(int idct, int idkhachhang, int idcheckin)
    {
        List<CheckListOBJ> lst = new List<CheckListOBJ>();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct),
                new SqlParameter("@idkhachhang", idkhachhang)
            };
            DataTable dt = db.ExecuteDataSet("sp_CheckList_GetAll_ByIDKhachHang", param).Tables[0];
            DataTable dtCheckIn = new DataTable();
            if (idcheckin > 0)
            {
                SqlParameter[] param_CheckIn = new SqlParameter[]
                {
                new SqlParameter("@ID_QLLH", idct),
                new SqlParameter("@idcheckin", idcheckin)
                };
                dtCheckIn = db.ExecuteDataSet("sp_CheckList_GetBy_ID_CheckIn", param_CheckIn).Tables[0];
                
            }
            foreach (DataRow dr in dt.Rows)
            {
                CheckListOBJ rs = GetCheckList(dr);

                foreach (DataRow drDaCheck in dtCheckIn.Rows)
                {

                    if (dr["ID_CheckList"].ToString() == drDaCheck["ID_CheckList"].ToString())
                    {
                        rs.DaCheck = 1;
                        rs.TrangThai = drDaCheck["TrangThai"].ToString() != "" ? int.Parse(drDaCheck["TrangThai"].ToString()) : 0;
                        rs.TenTrangThai = rs.TrangThai == 0 ? "Không tốt" : "Tốt";
                        rs.ThoiGian = drDaCheck["ThoiGian"].ToString() != "" ? DateTime.Parse(drDaCheck["ThoiGian"].ToString()) : rs.ThoiGian;
                        break;
                    }

                }

                lst.Add(rs);
            }
             



        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return lst;
    }

    public DataTable GetList(int idct)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_QLLH", idct)
               
            };
              dt = db.ExecuteDataSet("sp_CheckList_GetAll", param).Tables[0];
             



        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return dt;
    }

    public bool ThemCheckListVaoKhachHang(CheckListOBJ obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                  new SqlParameter("ID_CheckList",  obj.ID_CheckList),
                  new SqlParameter("ID_KhachHang",  obj.ID_KhachHang),
                  new SqlParameter("STT",  obj.STT),
                    new SqlParameter("ID_QLLH",  obj.ID_QLLH),

                };
            int i = db.ExecuteNonQuery("sp_CheckList_Them_ByIDKhachHang", pars);
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

    public bool XoaCheckList_ByKhachHang(int ID_KhachHang)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                   
                  new SqlParameter("ID_KhachHang",   ID_KhachHang),
                  

                };
            int i = db.ExecuteNonQuery("sp_CheckList_Xoa_ByIDKhachHang", pars);
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


    public  DataTable GetLichSu_TheoKhachHang(int ID_QLLH, int ID_NhanVien, int ID_KhachHang, int ID_CheckIn, DateTime tungay, DateTime denngay)
    {
        DataTable dt = new DataTable();
         
        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                 new SqlParameter("@ID_CheckIn", ID_CheckIn),
                new SqlParameter("@ID_KhachHang", ID_KhachHang),
                  new SqlParameter("@ID_QLLH", ID_QLLH),
                new SqlParameter("@ID_NhanVien", ID_NhanVien),
                new SqlParameter("@dtFrom", tungay),
                new SqlParameter("@dtTo", denngay)
            };

             dt = db.ExecuteDataSet("sp_CheckList_LichSuTheoKhachHang", param).Tables[0];
             
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return dt;
    }
}