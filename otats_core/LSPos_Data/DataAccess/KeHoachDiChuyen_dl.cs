using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KeHoachDiChuyen_dl
/// </summary>
public class KeHoachDiChuyen_dl
{
    log4net.ILog log = log4net.LogManager.GetLogger(typeof(KeHoachDiChuyen_dl));
    private SqlDataHelper helper;
    public KeHoachDiChuyen_dl()
    {
        helper = new SqlDataHelper();
    }

    public KeHoachDiChuyenObj GetKeHoachFromDataRow(DataRow dr)
    {
        try
        {
            KeHoachDiChuyenObj keHoach = new KeHoachDiChuyenObj();
            //
            try
            {
                keHoach.IDKeHoach = (dr["ID"] != DBNull.Value) ? int.Parse(dr["ID"].ToString()) : 0;
            }
            catch (Exception)
            {
                
                 
            }
            keHoach.IDKhachHang = (dr["ID_KhachHang"] != DBNull.Value) ? int.Parse(dr["ID_KhachHang"].ToString()) : 0;
            keHoach.IDNhanVien = (dr["ID_NhanVien"] != DBNull.Value) ? int.Parse(dr["ID_NhanVien"].ToString()) : 0;
            keHoach.TenKhachHang = (dr["TenKhachHang"] != DBNull.Value) ? dr["TenKhachHang"].ToString() : "";
            keHoach.TenNhanVien = (dr["TenNhanVien"] != DBNull.Value) ? dr["TenNhanVien"].ToString() : "";
            keHoach.ThoiGianCheckInDuKien = Convert.ToDateTime(dr["ThoiGianCheckInDuKien"].ToString() != "" ? dr["ThoiGianCheckInDuKien"] : "1900-1-1");
            keHoach.ThoiGianCheckOutDuKien = Convert.ToDateTime(dr["ThoiGianCheckOutDuKien"].ToString() != "" ? dr["ThoiGianCheckOutDuKien"] : "1900-1-1");
            keHoach.ThoiGianCheckInThucTe = Convert.ToDateTime(dr["ThoiGianCheckInThucTe"].ToString() != "" ? dr["ThoiGianCheckInThucTe"] : "1900-1-1");
            keHoach.ThoiGianCheckOutThucTe = Convert.ToDateTime(dr["ThoiGianCheckOutThucTe"].ToString() != "" ? dr["ThoiGianCheckOutThucTe"] : "1900-1-1");
            keHoach.DiaChi = (dr["DiaChi"] != DBNull.Value) ? dr["DiaChi"].ToString() : "";
            keHoach.TrangThai = (dr["TrangThai"] != DBNull.Value) ? Convert.ToInt16(dr["TrangThai"]) : 0;
            keHoach.ThuTuCheckIn = (dr["ThuTuCheckIn"] != DBNull.Value) ? Convert.ToInt16(dr["ThuTuCheckIn"]) : 0;
            keHoach.GhiChu = (dr["GhiChu"] != DBNull.Value ? dr["GhiChu"] : "").ToString();
            keHoach.DuongPho = (dr["DuongPho"] != DBNull.Value ? dr["DuongPho"] : "").ToString();
            keHoach.ViecCanLam = (dr["ViecCanLam"] != DBNull.Value ? dr["ViecCanLam"] : "").ToString();
            keHoach.NguoiTao = (dr["NguoiTao"] != DBNull.Value) ? Convert.ToInt16(dr["NguoiTao"]) : 0;
            keHoach.NgayTao = Convert.ToDateTime(dr["NgayTao"].ToString() != "" ? dr["NgayTao"] : "1900-1-1");

            if (keHoach.ThoiGianCheckInDuKien.Year > 1900 && ( keHoach.ThoiGianCheckInDuKien < DateTime.Now && keHoach.TrangThai == 0))
            {
                keHoach.text_color = "#DD4B39";
                keHoach.text_color_mota = "Kế hoạch đã quá giờ mà chưa vào điểm";
                //- Màu đỏ: kế hoạch đã quá giờ mà chưa vào điểm
                // e.Row.Cells[8].CssClass = "label label-danger";
                 
            }
            else if (keHoach.ThoiGianCheckInDuKien < keHoach.ThoiGianCheckInThucTe && keHoach.TrangThai == 1)
            {
                keHoach.text_color = "#F39C12";
                //keHoach.text_color_mota = "Vào - ra thực tế:"+ keHoach.ThoiGianCheckInThucTe.ToString("HH:mm") + "-" + (keHoach.ThoiGianCheckOutThucTe.Year > 1900 ? keHoach.ThoiGianCheckOutThucTe.ToString("HH:mm")  : "") + " <br/>Kế hoạch đã vào điểm nhưng muộn hơn so với thời gian dự kiến";

                keHoach.text_color_mota = "Kế hoạch đã vào điểm nhưng muộn hơn so với thời gian dự kiến";


                //- Màu vàng: kế hoạch đã vào điểm nhưng muộn hơn so với thời gian dự kiến
                //e.Row.Cells[8].CssClass = "label label-warning";

            }
            else if ((keHoach.ThoiGianCheckInDuKien >= keHoach.ThoiGianCheckInThucTe || (keHoach.ThoiGianCheckInDuKien > DateTime.Now)) && keHoach.TrangThai == 1)
            {
                keHoach.text_color = "#3C8DBC";
                //keHoach.text_color_mota = "Vào - ra thực tế:" + keHoach.ThoiGianCheckInThucTe.ToString("HH:mm") + "-" + (keHoach.ThoiGianCheckOutThucTe.Year > 1900 ? keHoach.ThoiGianCheckOutThucTe.ToString("HH:mm") : "") + " <br/>Kế hoạch đã vào điểm trước/ đúng giờ theo dự kiến";

                keHoach.text_color_mota = "Kế hoạch đã vào điểm trước/ đúng giờ theo dự kiến";


                //-   //-Màu xanh blue: kế hoạch đã vào điểm trước/ đúng giờ theo dự kiến
                //e.Row.Cells[8].CssClass = "label label-primary";

            }

            else if (keHoach.ThoiGianCheckInDuKien > DateTime.Now)
            {
                keHoach.text_color = "#00A65A";
                keHoach.text_color_mota = "Kế hoạch chưa đến giờ";
                //-Màu xanh: kế hoạch chưa đến giờ
                // e.Row.Cells[8].CssClass = "label label-success";
                
            }
            if(dr.Table.Columns.Contains("KinhDo"))
            {
                keHoach.KinhDo = (dr["KinhDo"] != DBNull.Value) ? double.Parse(dr["KinhDo"].ToString()) : 0;
            }
            if (dr.Table.Columns.Contains("ViDo"))
            {
                keHoach.ViDo = (dr["ViDo"] != DBNull.Value) ? double.Parse(dr["ViDo"].ToString()) : 0;
            }
            return keHoach;
        }
        catch
        {
            return null;
        }
    }

    public List<KeHoachDiChuyenObj> GetKeHoachTheoNhanVien(int ID_NhanVien, DateTime Ngay)
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
            List<KeHoachDiChuyenObj> dskhdc = new List<KeHoachDiChuyenObj>();
            foreach (DataRow dr in dt.Rows)
            {
                KeHoachDiChuyenObj khdc = GetKeHoachFromDataRow(dr);
                dskhdc.Add(khdc);
            }

            return dskhdc;
        }
        catch
        {
            return null;
        }
    }
    public List<KeHoachDiChuyenObj> GetKeHoachTheoNhanVien(int ID_NhanVien, DateTime Ngay, DateTime denNgay)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@Ngay", Ngay),
             new SqlParameter("@DenNgay", denNgay)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetKeHoachTheoNhanVien", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<KeHoachDiChuyenObj> dskhdc = new List<KeHoachDiChuyenObj>();
            foreach (DataRow dr in dt.Rows)
            {
                KeHoachDiChuyenObj khdc = GetKeHoachFromDataRow(dr);
                dskhdc.Add(khdc);
            }

            return dskhdc;
        }
        catch
        {
            return null;
        }
    }
    public List<KeHoachDiChuyenObj> GetKeHoachById(int ID_KeHoach)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_KeHoach", ID_KeHoach) 
        };

        DataSet ds = helper.ExecuteDataSet("sp_KeHoachDiChuyen_GetBy_ID", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<KeHoachDiChuyenObj> dskhdc = new List<KeHoachDiChuyenObj>();
            foreach (DataRow dr in dt.Rows)
            {
                KeHoachDiChuyenObj khdc = GetKeHoachFromDataRow(dr);
                dskhdc.Add(khdc);
            }

            return dskhdc;
        }
        catch
        {
            return null;
        }
    }

    public List<KeHoachDiChuyenObj> GetKeHoachTheoNhanVien(int IDKhachHang,int ID_NhanVien, DateTime Ngay)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_KhachHang", IDKhachHang),
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@Ngay", Ngay)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetKeHoachTheoNhanVien", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<KeHoachDiChuyenObj> dskhdc = new List<KeHoachDiChuyenObj>();
            foreach (DataRow dr in dt.Rows)
            {
                KeHoachDiChuyenObj khdc = GetKeHoachFromDataRow(dr);
                dskhdc.Add(khdc);
            }

            return dskhdc;
        }
        catch
        {
            return null;
        }
    }
    public DataTable GetKeHoachLastEdit(int ID_NhanVien, DateTime Ngay)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@Ngay", Ngay)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_KeHoachGetLastEdit", pars);
        try
        {
            return ds.Tables[0];
        }
        catch
        {
            return null;
        }
    }
   
    public List<KeHoachDiChuyenObj> CheckKeHoachTheoNhanVien(int ID_NhanVien, DateTime Ngay,int ID_Tinh,int ID_Quan)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@Ngay", Ngay),
            new SqlParameter("@ID_Tinh", ID_Tinh),
            new SqlParameter("@ID_Quan", ID_Quan),
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_CheckKeHoach", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<KeHoachDiChuyenObj> dskhdc = new List<KeHoachDiChuyenObj>();
            foreach (DataRow dr in dt.Rows)
            {
                KeHoachDiChuyenObj khdc = GetKeHoachFromDataRow(dr);
                dskhdc.Add(khdc);
            }

            return dskhdc;
        }
        catch
        {
            return null;
        }
    }
    public int XoaKeHoach(int ID_NhanVien, int ID_QuanLy, DateTime Ngay)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@Ngay", Ngay),
            new SqlParameter("@ID_QuanLy", ID_QuanLy)
        };
        try
        {
            return helper.ExecuteNonQuery("sp_QL_XoaKeHoach", pars);
        }
        catch
        {
            return 0;
        }
    }

    public int XoaKeHoach(int ID_NhanVien, int ID_QuanLy, DateTime Ngay, int ID_Tinh, int ID_Quan)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@Ngay", Ngay),
            new SqlParameter("@ID_Tinh", ID_Tinh),
            new SqlParameter("@ID_Quan", ID_Quan)
        };
        try
        {
            return helper.ExecuteNonQuery("sp_QL_XoaKeHoach_Theo_Tinh_Quan", pars);
        }
        catch(Exception ex)
        {
            
            log.Error(ex);
            return 0;
        }
    }
    public int ThemKeHoach(KeHoachDiChuyenObj khdc)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_NhanVien", khdc.IDNhanVien),
            new SqlParameter("@ID_KhachHang", khdc.IDKhachHang),
            new SqlParameter("@ThoiGianCheckInDuKien", khdc.ThoiGianCheckInDuKien),
            new SqlParameter("@ThoiGianCheckOutDuKien", khdc.ThoiGianCheckOutDuKien),
            new SqlParameter("@NguoiTao", khdc.NguoiTao),
            new SqlParameter("@NgayTao", khdc.NgayTao),
            new SqlParameter("@GhiChu", khdc.GhiChu),
              new SqlParameter("@ViecCanLam", khdc.ViecCanLam)
        };
        try
        {
            if (khdc.ThoiGianCheckInDuKien < DateTime.Now)
            {
                return -1;
            }
            else
                return helper.ExecuteNonQuery("sp_QL_ThemKeHoachDiChuyen", pars);
        }
        catch
        {
            return 0;
        }
    }
    public int SuaKeHoach(KeHoachDiChuyenObj khdc)
    {
        SqlParameter[] pars = new SqlParameter[] {
              new SqlParameter("@IDKeHoach", khdc.IDKeHoach),
            new SqlParameter("@ID_NhanVien", khdc.IDNhanVien),
            new SqlParameter("@ID_KhachHang", khdc.IDKhachHang),
            new SqlParameter("@ThoiGianCheckInDuKien", khdc.ThoiGianCheckInDuKien),
            new SqlParameter("@ThoiGianCheckOutDuKien", khdc.ThoiGianCheckOutDuKien),
            new SqlParameter("@GhiChu", khdc.GhiChu),
             new SqlParameter("@ViecCanLam", khdc.ViecCanLam)
        };
        try
        {
            if (khdc.ThoiGianCheckInDuKien < DateTime.Now)
            {
                return -1;
            }
            else
                return helper.ExecuteNonQuery("sp_KeHoachDiChuyen_Sua", pars);
        }
        catch
        {
            return 0;
        }
    }
    public int DeleteKeHoach(KeHoachDiChuyenObj khdc)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_NhanVien", khdc.IDNhanVien),
            new SqlParameter("@ID_KhachHang", khdc.IDKhachHang),
            new SqlParameter("@ThoiGianCheckInDuKien", khdc.ThoiGianCheckInDuKien.Date)
        };
        try
        {
            return helper.ExecuteNonQuery("sp_QL_DeleteKeHoachDiChuyen", pars);
        }
        catch
        {
            return 0;
        }
    }

    public int DeleteKeHoachTheoNgay(int ID_NhanVien, DateTime ngay)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@ngay", ngay)
        };
        try
        {
            return helper.ExecuteNonQuery("sp_QL_DeleteKeHoachTheoNgay", pars);
        }
        catch
        {
            return 0;
        }
    }
    public int DeleteKeHoachTheoNgay(int ID_KhachHang, int ID_NhanVien, DateTime ngay)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_KhachHang", ID_KhachHang),
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@ngay", ngay)
        };
        try
        {
            return helper.ExecuteNonQuery("sp_QL_DeleteKeHoachTheoNgay", pars);
        }
        catch
        {
            return 0;
        }
    }
    public List<KeHoachDiChuyenObj> GetKeHoachTheoNhanVien_Moi(int ID_NhanVien, DateTime TuNgay, DateTime DenNgay, int ID_QLLH, int ID_QuanLy)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@TuNgay", TuNgay),
            new SqlParameter("@DenNgay", DenNgay),
            new SqlParameter("@ID_QLLH", ID_QLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetKeHoachTheoNhanVien_New", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<KeHoachDiChuyenObj> dskhdc = new List<KeHoachDiChuyenObj>();
            foreach (DataRow dr in dt.Rows)
            {
                KeHoachDiChuyenObj khdc = GetKeHoachFromDataRow(dr);
                dskhdc.Add(khdc);
            }

            return dskhdc;
        }
        catch(Exception ex)
        {
            log.Error(ex);

            return null;
        }
    }

    public List<KeHoachDiChuyenObj> LayDanhSachKeHoach_TheoNguoiTao(int NguoiTao, DateTime NgayTao)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@NguoiTao", NguoiTao),
            new SqlParameter("@NgayTao", NgayTao),
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_GetKeHoachTheoNguoiTao", pars);
            dt = ds.Tables[0];
            List<KeHoachDiChuyenObj> dskhdc = new List<KeHoachDiChuyenObj>();
            foreach (DataRow dr in dt.Rows)
            {
                KeHoachDiChuyenObj khdc = GetKeHoachFromDataRow(dr);
                dskhdc.Add(khdc);
            }

            return dskhdc;

        }
        catch (Exception ex)
        {
            log.Error(ex);

        }

        return null;
    }

    public DataTable LayDanhSachKeHoach_TheoNhanVien(int ID_NhanVien, DateTime Ngay, int ID_Tinh, int ID_Quan)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@Ngay", Ngay),
            new SqlParameter("@ID_Tinh", ID_Tinh),
            new SqlParameter("@ID_Quan", ID_Quan),
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_CheckKeHoach", pars);
            dt = ds.Tables[0];
             
        }
        catch (Exception ex)
        {
            log.Error(ex);
             
        }

        return dt;
    }


    public KeHoachDiChuyenObj GetKeHoach(int ID_QLLH, int ID_KhachHang, int ID_NhanVien, DateTime Ngay )
    {
        try
        {
            KeHoachDiChuyenObj keHoach = new KeHoachDiChuyenObj();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("@ID_KhachHang", ID_KhachHang),
            new SqlParameter("@Ngay", Ngay),
            new SqlParameter("@ID_QLLH", ID_QLLH) 
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_GetKeHoachTheoNhanVien_KhachHang", pars);
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                keHoach = GetKeHoachFromDataRow(dr);
            }
            return keHoach;
        }
        catch
        {
            return null;
        }
    }

}