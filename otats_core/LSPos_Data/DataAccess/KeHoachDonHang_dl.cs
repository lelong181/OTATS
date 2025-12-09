using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KeHoachDiChuyen_dl
/// </summary>
public class KeHoachDonHang_dl
{
    log4net.ILog log = log4net.LogManager.GetLogger(typeof(KeHoachDonHang_dl));
    private SqlDataHelper helper;
    public KeHoachDonHang_dl()
    {
        helper = new SqlDataHelper();
    }

    public KeHoachDonHangOBJ GetKeHoachFromDataRow(DataRow dr)
    {
        try
        {
            DateTime d;
            KeHoachDonHangOBJ kh = new KeHoachDonHangOBJ
            {
                IDKeHoach = int.Parse(dr["ID"].ToString()),
                ID_DonHang = int.Parse(dr["ID_DonHang"].ToString()),
                ID_KhachHang = int.Parse(dr["ID_KhachHang"].ToString()),
                ID_NhanVien = int.Parse(dr["ID_NhanVien"].ToString()),
                ThoiGianDuKien = dr["ThoiGianDuKien"].ToString() != "" ? DateTime.Parse(dr["ThoiGianDuKien"].ToString()) : new DateTime(1900, 01, 01),
                ThoiGianThucTe = dr["ThoiGianThucTe"].ToString() != "" ? DateTime.Parse(dr["ThoiGianThucTe"].ToString()) : new DateTime(1900, 01, 01),
                NgayTaoDonHang = dr["CreateDate"].ToString() != "" ? DateTime.Parse(dr["CreateDate"].ToString()) : new DateTime(1900, 01, 01),
                
                ThoiGianDuKien_Text = (DateTime.TryParse(dr["ThoiGianDuKien"].ToString(), out d)) ? DateTime.Parse(dr["ThoiGianDuKien"].ToString()).ToString("dd-MM-yyyy HH:mm:ss") : "",
                TrangThai = int.Parse(dr["TrangThai"].ToString()),
                STT = int.Parse(dr["ThuTuCheckIn"].ToString()),
                TenKhachHang = dr["TenKhachHang"].ToString(),
                TenNhanVien = dr["TenNhanVien"].ToString(),
                GhiChu = dr["GhiChu"].ToString(),
                MaThamChieu = dr["MaThamChieu"].ToString(),
                KinhDo = dr.Table.Columns.Contains("KinhDo") ? double.Parse(dr["KinhDo"].ToString()) : 0,
                ViDo =  dr.Table.Columns.Contains("ViDo") ? double.Parse(dr["ViDo"].ToString()) : 0 
                
            };




            try
            {
                if (kh.ThoiGianDuKien.Year > 1900 && (kh.ThoiGianDuKien < DateTime.Now && kh.ThoiGianThucTe.Year < 2000 ))
                {
                    kh.text_color = "#DD4B39";
                    kh.text_color_mota = "Kế hoạch đã quá giờ mà chưa giao";
                    //- Màu đỏ: kế hoạch đã quá giờ mà chưa vào điểm
                    // e.Row.Cells[8].CssClass = "label label-danger";

                }
                else if (kh.ThoiGianDuKien < kh.ThoiGianThucTe && kh.ThoiGianThucTe.Year > 2000)
                {
                    kh.text_color = "#F39C12";
                    kh.text_color_mota = "Kế hoạch đã giao hàng nhưng muộn hơn so với thời gian dự kiến";
                    //- Màu vàng: kế hoạch đã vào điểm nhưng muộn hơn so với thời gian dự kiến
                    //e.Row.Cells[8].CssClass = "label label-warning";

                }
                else if (kh.ThoiGianDuKien >= kh.ThoiGianThucTe && kh.ThoiGianThucTe.Year > 2000)
                {
                    kh.text_color = "#3C8DBC";
                    kh.text_color_mota = "Kế hoạch đã giao hàng trước/ đúng giờ theo dự kiến";
                    //-   //-Màu xanh blue: kế hoạch đã vào điểm trước/ đúng giờ theo dự kiến
                    //e.Row.Cells[8].CssClass = "label label-primary";

                }

                else if (kh.ThoiGianDuKien > DateTime.Now)
                {
                    kh.text_color = "#00A65A";
                    kh.text_color_mota = "Kế hoạch chưa đến giờ";
                    //-Màu xanh: kế hoạch chưa đến giờ
                    // e.Row.Cells[8].CssClass = "label label-success";

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return kh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public List<KeHoachDonHangOBJ> GetKeHoachTheoNhanVien(int ID_NhanVien, DateTime Ngay)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@Ngay", Ngay)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetKeHoachTheoNhanVien", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<KeHoachDonHangOBJ> dskhdc = new List<KeHoachDonHangOBJ>();
            foreach (DataRow dr in dt.Rows)
            {
                KeHoachDonHangOBJ khdc = GetKeHoachFromDataRow(dr);
                dskhdc.Add(khdc);
            }

            return dskhdc;
        }
        catch
        {
            return null;
        }
    }
    //public List<KeHoachDiChuyenObj> GetKeHoachTheoNhanVien(int ID_NhanVien, DateTime Ngay, DateTime denNgay)
    //{
    //    SqlParameter[] pars = new SqlParameter[] {
    //        new SqlParameter("@ID_NhanVien", ID_NhanVien),
    //        new SqlParameter("@Ngay", Ngay),
    //         new SqlParameter("@DenNgay", denNgay)
    //    };

    //    DataSet ds = helper.ExecuteDataSet("sp_QL_GetKeHoachTheoNhanVien", pars);
    //    DataTable dt = ds.Tables[0];

    //    if (dt.Rows.Count == 0)
    //        return null;

    //    try
    //    {
    //        List<KeHoachDiChuyenObj> dskhdc = new List<KeHoachDiChuyenObj>();
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            KeHoachDiChuyenObj khdc = GetKeHoachFromDataRow(dr);
    //            dskhdc.Add(khdc);
    //        }

    //        return dskhdc;
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}
    public List<KeHoachDonHangOBJ> GetKeHoachById(int ID_KeHoach)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_KeHoach", ID_KeHoach)
        };

        DataSet ds = helper.ExecuteDataSet("sp_KeHoachGiaoHang_GetBy_ID", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<KeHoachDonHangOBJ> dskhdc = new List<KeHoachDonHangOBJ>();
            foreach (DataRow dr in dt.Rows)
            {
                KeHoachDonHangOBJ khdc = GetKeHoachFromDataRow(dr);
                dskhdc.Add(khdc);
            }

            return dskhdc;
        }
        catch
        {
            return null;
        }
    }

    //public List<KeHoachDiChuyenObj> GetKeHoachTheoNhanVien(int IDKhachHang,int ID_NhanVien, DateTime Ngay)
    //{
    //    SqlParameter[] pars = new SqlParameter[] {
    //        new SqlParameter("@ID_KhachHang", IDKhachHang),
    //        new SqlParameter("@ID_NhanVien", ID_NhanVien),
    //        new SqlParameter("@Ngay", Ngay)
    //    };

    //    DataSet ds = helper.ExecuteDataSet("sp_QL_GetKeHoachTheoNhanVien", pars);
    //    DataTable dt = ds.Tables[0];

    //    if (dt.Rows.Count == 0)
    //        return null;

    //    try
    //    {
    //        List<KeHoachDiChuyenObj> dskhdc = new List<KeHoachDiChuyenObj>();
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            KeHoachDiChuyenObj khdc = GetKeHoachFromDataRow(dr);
    //            dskhdc.Add(khdc);
    //        }

    //        return dskhdc;
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}
    //public DataTable GetKeHoachLastEdit(int ID_NhanVien, DateTime Ngay)
    //{
    //    SqlParameter[] pars = new SqlParameter[] {
    //        new SqlParameter("@ID_NhanVien", ID_NhanVien),
    //        new SqlParameter("@Ngay", Ngay)
    //    };

    //    DataSet ds = helper.ExecuteDataSet("sp_QL_KeHoachGetLastEdit", pars);
    //    try
    //    {
    //        return ds.Tables[0];
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}

    //public List<KeHoachDiChuyenObj> CheckKeHoachTheoNhanVien(int ID_NhanVien, DateTime Ngay,int ID_Tinh,int ID_Quan)
    //{
    //    SqlParameter[] pars = new SqlParameter[] {
    //        new SqlParameter("@ID_NhanVien", ID_NhanVien),
    //        new SqlParameter("@Ngay", Ngay),
    //        new SqlParameter("@ID_Tinh", ID_Tinh),
    //        new SqlParameter("@ID_Quan", ID_Quan),
    //    };

    //    DataSet ds = helper.ExecuteDataSet("sp_QL_CheckKeHoach", pars);
    //    DataTable dt = ds.Tables[0];

    //    if (dt.Rows.Count == 0)
    //        return null;

    //    try
    //    {
    //        List<KeHoachDiChuyenObj> dskhdc = new List<KeHoachDiChuyenObj>();
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            KeHoachDiChuyenObj khdc = GetKeHoachFromDataRow(dr);
    //            dskhdc.Add(khdc);
    //        }

    //        return dskhdc;
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}
    //public int XoaKeHoach(int ID_NhanVien, int ID_QuanLy, DateTime Ngay)
    //{
    //    SqlParameter[] pars = new SqlParameter[] {
    //        new SqlParameter("@ID_NhanVien", ID_NhanVien),
    //        new SqlParameter("@Ngay", Ngay),
    //        new SqlParameter("@ID_QuanLy", ID_QuanLy)
    //    };
    //    try
    //    {
    //        return helper.ExecuteNonQuery("sp_QL_XoaKeHoach", pars);
    //    }
    //    catch
    //    {
    //        return 0;
    //    }
    //}

    //public int XoaKeHoach(int ID_NhanVien, int ID_QuanLy, DateTime Ngay, int ID_Tinh, int ID_Quan)
    //{
    //    SqlParameter[] pars = new SqlParameter[] {
    //        new SqlParameter("@ID_QuanLy", ID_QuanLy),
    //        new SqlParameter("@ID_NhanVien", ID_NhanVien),
    //        new SqlParameter("@Ngay", Ngay),
    //        new SqlParameter("@ID_Tinh", ID_Tinh),
    //        new SqlParameter("@ID_Quan", ID_Quan)
    //    };
    //    try
    //    {
    //        return helper.ExecuteNonQuery("sp_QL_XoaKeHoach_Theo_Tinh_Quan", pars);
    //    }
    //    catch(Exception ex)
    //    {

    //        log.Error(ex);
    //        return 0;
    //    }
    //}
    public int ThemKeHoach(KeHoachDonHangOBJ  khdc)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_NhanVien", khdc.ID_NhanVien),
            new SqlParameter("@ID_KhachHang", khdc.ID_KhachHang),
          
            new SqlParameter("@ID_DonHang", khdc.ID_DonHang),
              new SqlParameter("@ThoiGianDuKien", khdc.ThoiGianDuKien),
            new SqlParameter("@GhiChu", khdc.GhiChu)
        };
        try
        {
            if (khdc.ThoiGianDuKien < DateTime.Now)
            {
                return -1;
            }
            else
                return helper.ExecuteNonQuery("sp_QL_ThemKeHoachDonHang", pars);
        }
        catch
        {
            return 0;
        }
    }
    public int SuaKeHoach(KeHoachDonHangOBJ khdc)
    {
        SqlParameter[] pars = new SqlParameter[] {
              new SqlParameter("@IDKeHoach", khdc.IDKeHoach),
            new SqlParameter("@ID_NhanVien", khdc.ID_NhanVien),
            new SqlParameter("@ID_KhachHang", khdc.ID_KhachHang),
        new SqlParameter("@ID_DonHang", khdc.ID_DonHang),
              new SqlParameter("@ThoiGianDuKien", khdc.ThoiGianDuKien),
            new SqlParameter("@GhiChu", khdc.GhiChu)
        };
        try
        {
            if (khdc.ThoiGianDuKien < DateTime.Now)
            {
                return -1;
            }
            else
                return helper.ExecuteNonQuery("sp_KeHoachGiaoHang_Sua", pars);
        }
        catch
        {
            return 0;
        }
    }

    public int XoaKeHoach(int id)
    {
        SqlParameter[] pars = new SqlParameter[] {
              new SqlParameter("@IDKeHoach", id),
            
        };
         
                return helper.ExecuteNonQuery("sp_KeHoachGiaoHang_Xoa", pars);
        
    }
    //public int DeleteKeHoach(KeHoachDiChuyenObj khdc)
    //{
    //    SqlParameter[] pars = new SqlParameter[] {
    //        new SqlParameter("@ID_NhanVien", khdc.IDNhanVien),
    //        new SqlParameter("@ID_KhachHang", khdc.IDKhachHang),
    //        new SqlParameter("@ThoiGianCheckInDuKien", khdc.ThoiGianCheckInDuKien.Date)
    //    };
    //    try
    //    {
    //        return helper.ExecuteNonQuery("sp_QL_DeleteKeHoachDiChuyen", pars);
    //    }
    //    catch
    //    {
    //        return 0;
    //    }
    //}

    //public int DeleteKeHoachTheoNgay(int ID_NhanVien, DateTime ngay)
    //{
    //    SqlParameter[] pars = new SqlParameter[] {
    //        new SqlParameter("@ID_NhanVien", ID_NhanVien),
    //        new SqlParameter("@ngay", ngay)
    //    };
    //    try
    //    {
    //        return helper.ExecuteNonQuery("sp_QL_DeleteKeHoachTheoNgay", pars);
    //    }
    //    catch
    //    {
    //        return 0;
    //    }
    //}
    //public int DeleteKeHoachTheoNgay(int ID_KhachHang, int ID_NhanVien, DateTime ngay)
    //{
    //    SqlParameter[] pars = new SqlParameter[] {
    //        new SqlParameter("@ID_KhachHang", ID_KhachHang),
    //        new SqlParameter("@ID_NhanVien", ID_NhanVien),
    //        new SqlParameter("@ngay", ngay)
    //    };
    //    try
    //    {
    //        return helper.ExecuteNonQuery("sp_QL_DeleteKeHoachTheoNgay", pars);
    //    }
    //    catch
    //    {
    //        return 0;
    //    }
    //}
    public List<KeHoachDonHangOBJ> GetKeHoachTheoNhanVien_Moi(int ID_NhanVien, DateTime TuNgay, DateTime DenNgay, int ID_QLLH, int ID_QuanLy)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@TuNgay", TuNgay),
            new SqlParameter("@DenNgay", DenNgay),
            new SqlParameter("@ID_QLLH", ID_QLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetKeHoachGiaoHangTheoNhanVien", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<KeHoachDonHangOBJ> dskhdc = new List<KeHoachDonHangOBJ>();
            foreach (DataRow dr in dt.Rows)
            {
                KeHoachDonHangOBJ khdc = GetKeHoachFromDataRow(dr);
                dskhdc.Add(khdc);
            }

            return dskhdc;
        }
        catch (Exception ex)
        {
            log.Error(ex);

            return null;
        }
    }

    //public DataTable LayDanhSachKeHoach_TheoNhanVien(int ID_NhanVien, DateTime Ngay, int ID_Tinh, int ID_Quan)
    //{
    //    DataTable dt = new DataTable();
    //    try
    //    {
    //        SqlParameter[] pars = new SqlParameter[] {
    //        new SqlParameter("@ID_NhanVien", ID_NhanVien),
    //        new SqlParameter("@Ngay", Ngay),
    //        new SqlParameter("@ID_Tinh", ID_Tinh),
    //        new SqlParameter("@ID_Quan", ID_Quan),
    //    };

    //        DataSet ds = helper.ExecuteDataSet("sp_QL_CheckKeHoach", pars);
    //        dt = ds.Tables[0];

    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);

    //    }

    //    return dt;
    //}


    //public KeHoachDiChuyenObj GetKeHoach(int ID_QLLH, int ID_KhachHang, int ID_NhanVien, DateTime Ngay )
    //{
    //    try
    //    {
    //        KeHoachDiChuyenObj keHoach = new KeHoachDiChuyenObj();
    //        SqlParameter[] pars = new SqlParameter[] {
    //        new SqlParameter("@ID_NhanVien", ID_NhanVien),
    //        new SqlParameter("@ID_KhachHang", ID_KhachHang),
    //        new SqlParameter("@Ngay", Ngay),
    //        new SqlParameter("@ID_QLLH", ID_QLLH) 
    //    };

    //        DataSet ds = helper.ExecuteDataSet("sp_QL_GetKeHoachTheoNhanVien_KhachHang", pars);
    //        DataTable dt = ds.Tables[0];
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            keHoach = GetKeHoachFromDataRow(dr);
    //        }
    //        return keHoach;
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}

}