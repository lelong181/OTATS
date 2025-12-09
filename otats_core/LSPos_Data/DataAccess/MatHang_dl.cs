using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MatHang_dl
/// </summary>
public class MatHang_dl
{
    private SqlDataHelper helper;
    private log4net.ILog log = log4net.LogManager.GetLogger(typeof(MatHang_dl));
    public MatHang_dl()
    {
        //
        // TODO: Add constructor logic here
        //
        helper = new SqlDataHelper();
    }

    public MatHang GetMatHangFromDataRow(DataRow dr)
    {
        try
        {
            MatHang mh = new MatHang();
            mh.IDMatHang = int.Parse(dr["ID_Hang"].ToString());
            mh.IDQLLH = int.Parse(dr["ID_QLLH"].ToString());
            mh.MaHang = dr["MaHang"].ToString();
            mh.TenHang = dr["TenHang"].ToString();
            mh.IDDonVi = int.Parse(dr["ID_DonVi"].ToString());
            mh.TenDonVi = dr["TenDonVi"].ToString();
            mh.KhuyenMai = dr["KhuyenMai"].ToString();
            mh.SoLuong = double.Parse(dr["SoLuong"].ToString());
            //mh.GiaBuon = Convert.ToUInt32(double.Parse(dr["GiaBanBuon"].ToString()));
            //mh.GiaLe = Convert.ToUInt32(double.Parse(dr["GiaBanLe"].ToString()));
            mh.GiaBuon = (dr["GiaBanBuon"].ToString() == "") ? mh.GiaBuon = -1 : double.Parse(dr["GiaBanBuon"].ToString());
            mh.GiaLe = (dr["GiaBanLe"].ToString() == "") ? mh.GiaLe = -1 : double.Parse(dr["GiaBanLe"].ToString());

            mh.ID_DANHMUC = (dr["ID_DANHMUC"].ToString() != "") ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0;
            mh.IsDichVu = (dr["IsDichVu"].ToString() != "") ? int.Parse(dr["IsDichVu"].ToString()) : 0;
            mh.TenDanhMuc = dr["TenDanhMuc"].ToString();
            mh.GhiChuGia = dr["GhiChuGia"].ToString();
           
            try
            {
                mh.AnhDaiDien = dr["AnhDaiDien"].ToString();
                mh.DanhSachAnh = HangHoaAlbumDB.LayDanhSachAnh_TheoMatHang(mh.IDQLLH, mh.IDMatHang);

              
                mh.ID_NhanHieu = (dr["ID_NhanHieu"].ToString() != "") ? int.Parse(dr["ID_NhanHieu"].ToString()) : 0;
                mh.ID_NhaCungCap = (dr["ID_NhaCungCap"].ToString() != "") ? int.Parse(dr["ID_NhaCungCap"].ToString()) : 0;
                mh.LinkGioiThieu = dr["LinkGioiThieu"].ToString();
                mh.MoTa = dr["MoTa"].ToString();
                mh.MoTaNgan = dr["MoTaNgan"].ToString();
                mh.SoLuongTon = dr.Table.Columns.Contains("SoLuongTon") ? ((dr["SoLuongTon"].ToString() != "") ? double.Parse(dr["SoLuongTon"].ToString()) : 0 ) : ((dr["SoLuong"].ToString() != "") ? double.Parse(dr["SoLuong"].ToString()) : 0);

            }
            catch (Exception)
            {

                 
            }
            return mh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public List<MatHang> GetMatHangAll(int IDQLLH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("id", IDQLLH)
        };

        DataSet ds = helper.ExecuteDataSet("sp_web_getDSHangHoaTheoID_QLLH", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<MatHang> dsmh = new List<MatHang>();
            foreach (DataRow dr in dt.Rows)
            {
                MatHang mh = GetMatHangFromDataRow(dr);
                //MatHang mh = new GetObjectFromDataRowUtil<MatHang>().ToOject(dr);
                dsmh.Add(mh);
            }

            return dsmh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }
    public DataTable Get_DS_MatHangAll(int IDQLLH)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("id", IDQLLH)
                };

            DataSet ds = helper.ExecuteDataSet("sp_web_getDSHangHoaTheoID_QLLH", pars);
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            log.Error(ex);
              return null;
        }

         
          

         
    }
    public DataTable Get_DS_MatHangAll_LoaiBoTheoCTKM(int IDQLLH,int ID_CTKM)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("id", IDQLLH),
             new SqlParameter("ID_CTKM", ID_CTKM)
                };

            DataSet ds = helper.ExecuteDataSet("sp_web_getDSHangHoaTheoID_QLLH_LoaiBoTheoCTKM", pars);
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }





    }
    public int DemTongMatHangKhongCoDanhMuc(int IDQLLH)
    {
        try
        {
            int tong = 0;
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("id", IDQLLH)
                };

            object ds = helper.ExecuteScalar("sp_HangHoa_CountMatHangKhongCoDanhMuc", pars);
            if (ds != null)
            {
                tong = int.Parse(ds.ToString());
            }
            return tong;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return 0;
        }






    }
    public MatHang GetMatHangTheoID(int ID_MatHang, int ID_Kho)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_Hang", ID_MatHang),
            new SqlParameter("ID_Kho", ID_Kho)
        };

        DataSet ds = helper.ExecuteDataSet("sp_web_getDSHangHoaTheo_ID_Kho", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            DataRow dr = dt.Rows[0];
            MatHang mh = GetMatHangFromDataRow(dr);
            return mh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public MatHang GetMatHangTheoID(int ID_MatHang)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_Hang", ID_MatHang)
        };

        DataSet ds = helper.ExecuteDataSet("sp_web_getHangHoaTheoID", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            DataRow dr = dt.Rows[0];
            MatHang mh = GetMatHangFromDataRow(dr);
            mh.lstDichVu = new HangHoa_DichVuDAO().GetAllByHangHoa(ID_MatHang);
            return mh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }
    public MatHang GetMatHangTheoTenHang(string TenHang, string MaHang, int ID_QLLH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("TenHang", TenHang),
            new SqlParameter("MaHang", MaHang),
             new SqlParameter("ID_QLLH", ID_QLLH)
        };

        DataSet ds = helper.ExecuteDataSet("sp_HangHoa_GetHangHoaTheoTenHang", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            DataRow dr = dt.Rows[0];
            MatHang mh = GetMatHangFromDataRow(dr);
            return mh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }
    public int ThemMatHang(MatHang mh)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_Hang", int.Parse("0")),
                new SqlParameter("ID_QLLH", mh.IDQLLH),
                new SqlParameter("MaHang", mh.MaHang),
                new SqlParameter("TenHang", mh.TenHang),
                mh.GiaBuon != null ? new SqlParameter("GiaBanBuon",(Convert.ToDouble(mh.GiaBuon))) : new SqlParameter("GiaBanBuon",DBNull.Value),
                mh.GiaLe != null ? new SqlParameter("GiaBanLe",(Convert.ToDouble(mh.GiaLe))) : new SqlParameter("GiaBanLe",DBNull.Value),
                new SqlParameter("ID_DonVi", mh.IDDonVi),
                new SqlParameter("SoLuong", mh.SoLuong), 
                new SqlParameter("KhuyenMai", mh.KhuyenMai),
                new SqlParameter("GhiChuGia", mh.GhiChuGia),
                new SqlParameter("IsDichVu", mh.IsDichVu),
                new SqlParameter("ID_DANHMUC", mh.ID_DANHMUC),
                   new SqlParameter("ID_NhaCungCap", mh.ID_NhaCungCap),
                      
                      new SqlParameter("ID_NhanHieu", mh.ID_NhanHieu),
                   new SqlParameter("MoTa", mh.MoTa),
                   new SqlParameter("MoTaNgan", mh.MoTaNgan),
                   new SqlParameter("LinkGioiThieu", mh.LinkGioiThieu),
                   new SqlParameter("AnhDaiDien", mh.AnhDaiDien),
            };
            object obj = helper.ExecuteScalar("sp_web_inserthanghoa", pars);
            if(obj != null)
            {
                return Convert.ToInt32(obj);
            }
            else
            {
                return 0;
            }
            
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return 0;
        }
    }

    public bool CapNhatMatHang(MatHang mh)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_Hang", mh.IDMatHang),
                new SqlParameter("ID_QLLH", mh.IDQLLH),
                new SqlParameter("MaHang", mh.MaHang),
                new SqlParameter("ID_DANHMUC", mh.ID_DANHMUC),
                new SqlParameter("IsDichVu", mh.IsDichVu),
                new SqlParameter("TenHang", mh.TenHang),
                //new SqlParameter("GiaBanBuon", Convert.ToDouble(mh.GiaBuon)),
                //new SqlParameter("GiaBanLe", Convert.ToDouble(mh.GiaLe)),
                mh.GiaBuon != null ? new SqlParameter("GiaBanBuon",(Convert.ToDouble(mh.GiaBuon))) : new SqlParameter("GiaBanBuon",DBNull.Value),
                mh.GiaLe != null ? new SqlParameter("GiaBanLe",(Convert.ToDouble(mh.GiaLe))) : new SqlParameter("GiaBanLe",DBNull.Value),

                new SqlParameter("ID_DonVi", mh.IDDonVi),
                new SqlParameter("SoLuong", mh.SoLuong),
                new SqlParameter("KhuyenMai", mh.KhuyenMai),
                 new SqlParameter("GhiChuGia", mh.GhiChuGia),
                   new SqlParameter("ID_NhaCungCap", mh.ID_NhaCungCap),
                      new SqlParameter("ID_NhanHieu", mh.ID_NhanHieu),
                       new SqlParameter("MoTa", mh.MoTa),
                       new SqlParameter("MoTaNgan", mh.MoTaNgan),
                   new SqlParameter("LinkGioiThieu", mh.LinkGioiThieu),
                   new SqlParameter("AnhDaiDien", mh.AnhDaiDien)
            };

            if (helper.ExecuteNonQuery("sp_web_inserthanghoa", pars) != 0)
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
        catch (Exception ex)
        {
            log.Error(ex);
            return false;
        }
    }

    public bool DeleteMatHang(MatHang mh)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_Hang", mh.IDMatHang),
                new SqlParameter("ID_QLLH", mh.IDQLLH)
            };

            if (helper.ExecuteNonQuery("sp_web_deletehanghoa", pars) != 0)
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
        catch (Exception ex)
        {
            log.Error(ex);
            return false;
        }
    }
    public bool DeleteMatHang(int ID_QLLH, int ID_Hang)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_Hang", ID_Hang),
                new SqlParameter("ID_QLLH", ID_QLLH)
            };

            if (helper.ExecuteNonQuery("sp_web_deletehanghoa", pars) != 0)
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
        catch (Exception ex)
        {
            log.Error(ex);
            return false;
        }
    }
    public List<MatHang> GetMatHangAll(int IDQLLH, int ID_DANHMUC)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("id", IDQLLH)
        };

        DataSet ds = helper.ExecuteDataSet("sp_web_getDSHangHoaTheoID_QLLH", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<MatHang> dsmh = new List<MatHang>();
            foreach (DataRow dr in dt.Rows)
            {
                MatHang mh = GetMatHangFromDataRow(dr);
                dsmh.Add(mh);
            }

            return dsmh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }
    public DataTable get_DS_HangHoa_ByIdDanhMuc(int ID_DANHMUC, int ID_QLLH)
    {
        try
        {

            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_DANHMUC", ID_DANHMUC),
                new SqlParameter("@ID_QLLH", ID_QLLH)
            };

            DataTable dt = helper.ExecuteDataSet("getDS_HangHoa_ByIdDanhMuc", pars).Tables[0];
             

            //lay cac mat hang cua danh muc con
            List<DanhMucOBJ> lstDMCon = DanhMucDB.getDS_DanhMucCon_ByIdDanhMuc(ID_DANHMUC);
            foreach (DanhMucOBJ dm in lstDMCon)
            {
                DataTable lstHangHoaCon = get_DS_HangHoa_ByIdDanhMuc(dm.ID_DANHMUC, dm.ID_QLLH);
                foreach (DataRow h in lstHangHoaCon.Rows)
                {
                    dt.ImportRow(h);
                }
            }

            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }
    public DataTable get_DS_HangHoa_ByIdDanhMuc_LoaiBoTheoCTKM(int ID_DANHMUC, int ID_QLLH, int ID_CTKM)
    {
        try
        {

            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_DANHMUC", ID_DANHMUC),
                new SqlParameter("@ID_QLLH", ID_QLLH),
                 new SqlParameter("@ID_CTKM", ID_CTKM)
            };

            DataTable dt = helper.ExecuteDataSet("getDS_HangHoa_ByIdDanhMuc_TheoCTKM", pars).Tables[0];


            //lay cac mat hang cua danh muc con
            List<DanhMucOBJ> lstDMCon = DanhMucDB.getDS_DanhMucCon_ByIdDanhMuc(ID_DANHMUC);
            foreach (DanhMucOBJ dm in lstDMCon)
            {
                DataTable lstHangHoaCon = get_DS_HangHoa_ByIdDanhMuc_LoaiBoTheoCTKM(dm.ID_DANHMUC, dm.ID_QLLH, ID_CTKM);
                foreach (DataRow h in lstHangHoaCon.Rows)
                {
                    dt.ImportRow(h);
                }
            }

            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public List<MatHang> getDS_HangHoa_ByIdDanhMuc(int ID_DANHMUC, int ID_QLLH)
    {
        List<MatHang> rs = new List<MatHang>();

        try
        {

            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_DANHMUC", ID_DANHMUC),
                new SqlParameter("@ID_QLLH", ID_QLLH)
            };

            DataTable dt = helper.ExecuteDataSet("getDS_HangHoa_ByIdDanhMuc", pars).Tables[0];
            int a = dt.Rows.Count;
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    //MatHang matHang = new GetObjectFromDataRowUtil<MatHang>().ToOject(dr);
                    rs.Add(GetMatHangFromDataRow(dr));
                    //rs.Add(matHang);

                    //rs.Add(new MatHang
                    //{
                    //    IDMatHang = int.Parse(dr["ID_Hang"].ToString()),
                    //    MaHang = dr["MaHang"].ToString(),
                    //    TenHang = dr["TenHang"].ToString(),
                    //    GiaBuon = (dr["GiaBanBuon"].ToString() != "") ? Convert.ToDouble(dr["GiaBanBuon"]) : 0,
                    //    GiaLe = (dr["GiaBanLe"].ToString() != "") ? Convert.ToDouble(dr["GiaBanLe"]) : 0,
                    //    SoLuong = (dr["SoLuong"].ToString() != "") ? double.Parse(dr["SoLuong"].ToString()) : 0,
                    //    TenDonVi = dr["TenDonVi"].ToString(),
                    //    KhuyenMai = dr["KhuyenMai"].ToString(),
                    //    ID_DANHMUC = (dr["ID_DANHMUC"].ToString() != "") ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0,
                    //    TenDanhMuc = dr["TenDanhMuc"].ToString(),
                    //    GhiChuGia = dr["GhiChuGia"].ToString()

                    //});
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            //lay cac mat hang cua danh muc con
            List<DanhMucOBJ> lstDMCon = DanhMucDB.getDS_DanhMucCon_ByIdDanhMuc(ID_DANHMUC);
            foreach (DanhMucOBJ dm in lstDMCon)
            {
                List<MatHang> lstHangHoaCon = getDS_HangHoa_ByIdDanhMuc(dm.ID_DANHMUC, dm.ID_QLLH);
                foreach (MatHang h in lstHangHoaCon)
                {
                    rs.Add(h);
                }
            }

            return rs;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return rs;
        }
    }

    public List<MatHang> GetMatHangAll_TheoKho(int ID_Kho)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_Kho", ID_Kho)
        };

        DataSet ds = helper.ExecuteDataSet("sp_web_getDSHangHoaTheo_ID_Kho", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<MatHang> dsmh = new List<MatHang>();
            foreach (DataRow dr in dt.Rows)
            {
                MatHang mh = GetMatHangFromDataRow(dr);
                dsmh.Add(mh);
            }

            return dsmh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }
    /// <summary>
    /// Hàm trả về danh sách mặt hàng có trong KHO
    /// </summary>
    /// <param name="ID_Kho">Mã Kho</param>
    /// <param name="ID_Hang">Mã Hàng</param>
    /// <returns></returns>
    public DataTable GetMatHangAll_TheoKho(int ID_Kho, int ID_Hang)
    {
        DataTable dt = new DataTable();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_Kho", ID_Kho),
            new SqlParameter("ID_Hang", ID_Hang)
        };
        try
        {
            DataSet ds = helper.ExecuteDataSet("sp_web_getDSHangHoaTheo_ID_Kho", pars);
            dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return null;




        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }

        return dt;
    }

}