using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KeHoachBaoDuongDB
/// </summary>
public class KeHoachBaoDuongDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(KeHoachBaoDuongDB));
    public static SqlDataHelper db = new SqlDataHelper();
	public KeHoachBaoDuongDB()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static KeHoachBaoDuongOBJ  GetKeHoach(int ID_Xe_KeHoachBaoDuong)
    {
         KeHoachBaoDuongOBJ  rs = new KeHoachBaoDuongOBJ();

        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_Xe_KeHoachBaoDuong", ID_Xe_KeHoachBaoDuong),
             
            };
            DataTable dt = db.ExecuteDataSet("sp_Xe_KeHoachBD_GetKeHoach_ById", param).Tables[0];
            DateTime d;
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    rs = new KeHoachBaoDuongOBJ
                    {
                        ID_NhanVien = int.Parse(dr["ID_NhanVien"].ToString()),
                        ID_Xe_KeHoachBaoDuong = int.Parse(dr["ID_Xe_KeHoachBaoDuong"].ToString()),
                        ID_Xe = int.Parse(dr["ID_Xe"].ToString()),
                        NgayBaoDuong = dr["NgayBaoDuong"].ToString() != "" ? DateTime.Parse(dr["NgayBaoDuong"].ToString()) : new DateTime(1900, 01, 01),
                        NgayBaoDuongDuKien = dr["NgayBaoDuongDuKien"].ToString() != "" ? DateTime.Parse(dr["NgayBaoDuongDuKien"].ToString()) : new DateTime(1900, 01, 01),
                        NgayBDTiepTheo = dr["NgayBDTiepTheo"].ToString() != "" ? DateTime.Parse(dr["NgayBDTiepTheo"].ToString()) : new DateTime(1900, 01, 01),
                        TrangThai = int.Parse(dr["TrangThai"].ToString()),
                        SoCho = dr["SoCho"].ToString() != "" ? int.Parse(dr["SoCho"].ToString()) : 0,
                        BienKiemSoat = dr["BienKiemSoat"].ToString(),
                        LoaiXe = dr["LoaiXe"].ToString(),
                        NoiDung = dr["NoiDung"].ToString(),
                        DiaChiBaoDuong = dr["DiaChiBaoDuong"].ToString(),
                        DiaDiemBaoDuong = dr["DiaDiemBaoDuong"].ToString(),
                        ChiPhi = dr["ChiPhi"].ToString() != "" ? double.Parse(dr["ChiPhi"].ToString()) : 0,
                        KinhDo = dr["KinhDo"].ToString() != "" ? double.Parse(dr["KinhDo"].ToString()) : 0,
                        ViDo = dr["ViDo"].ToString() != "" ? double.Parse(dr["ViDo"].ToString()) : 0,
                    };

                     
                }
                catch (Exception ex)
                {
                    log.Error(ex);
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


    public static int XoaKeHoach(int ID_Xe_KeHoachBaoDuong)
    {
      

        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_Xe_KeHoachBaoDuong", ID_Xe_KeHoachBaoDuong),

            };
            return db.ExecuteNonQuery("sp_Xe_KeHoachBD_Xoa_ById", param);
             

        }
        catch (Exception ex)
        {
            log.Error(ex);
            return -1;
        }
    }
    public static List<KeHoachBaoDuongOBJ> KeHoachTuNgayDenNgay(int idct, int idquanly,int idnhanvien, string tungay, string denngay, int idXe)
    {
        List<KeHoachBaoDuongOBJ> rs = new List<KeHoachBaoDuongOBJ>();

        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@idnhanvien", idnhanvien),
                 new SqlParameter("@idquanly", idquanly),
                  new SqlParameter("@idct", idct),
                  new SqlParameter("@tungay", tungay),
                 new SqlParameter("@denngay", denngay),
                       new SqlParameter("@idXe", idXe),

            };
            DataTable dt = db.ExecuteDataSet("sp_Xe_KeHoachBD_GetKeHoach", param).Tables[0];
            DateTime d;
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    KeHoachBaoDuongOBJ kh =    new KeHoachBaoDuongOBJ
                    {
                        ID_NhanVien = int.Parse(dr["ID_NhanVien"].ToString()),
                        ID_Xe_KeHoachBaoDuong = int.Parse(dr["ID_Xe_KeHoachBaoDuong"].ToString()),
                        ID_Xe = int.Parse(dr["ID_Xe"].ToString()),
                        NgayBaoDuong = dr["NgayBaoDuong"].ToString() != "" ? DateTime.Parse(dr["NgayBaoDuong"].ToString()) : new DateTime(1900, 01, 01),
                        NgayBaoDuongDuKien = dr["NgayBaoDuongDuKien"].ToString() != "" ? DateTime.Parse(dr["NgayBaoDuongDuKien"].ToString()) : new DateTime(1900, 01, 01),
                        NgayBDTiepTheo = dr["NgayBDTiepTheo"].ToString() != "" ? DateTime.Parse(dr["NgayBDTiepTheo"].ToString()) : new DateTime(1900, 01, 01),
                         
                        SoCho = dr["SoCho"].ToString() != "" ? int.Parse(dr["SoCho"].ToString()) : 0,
                        BienKiemSoat = dr["BienKiemSoat"].ToString(),
                        LoaiXe = dr["LoaiXe"].ToString(),
                        NoiDung = dr["NoiDung"].ToString() ,
                        TenNhanVien = dr["TenNhanVien"].ToString(),
                        DiaChiBaoDuong = dr["DiaChiBaoDuong"].ToString(),
                        DiaDiemBaoDuong = dr["DiaDiemBaoDuong"].ToString(),
                        ChiPhi = dr["ChiPhi"].ToString() != "" ? double.Parse(dr["ChiPhi"].ToString()) : 0,
                        KinhDo = dr["KinhDo"].ToString() != "" ? double.Parse(dr["KinhDo"].ToString()) : 0,
                        ViDo = dr["ViDo"].ToString() != "" ? double.Parse(dr["ViDo"].ToString()) : 0,
                        TrangThai = dr["TrangThai"].ToString() != "" ? int.Parse(dr["TrangThai"].ToString()) : 0,
                    } ;
                    try
                    {
                        if (kh.NgayBaoDuongDuKien.Year > 1900 && (kh.NgayBaoDuongDuKien < DateTime.Now && kh.TrangThai == 0))
                        {
                            kh.text_color = "#DD4B39";
                            kh.text_color_mota = "Đã quá ngày kế hoạch mà chưa đi bảo dưỡng";
                            //- Màu đỏ: kế hoạch đã quá giờ mà chưa vào điểm
                            // e.Row.Cells[8].CssClass = "label label-danger";

                        }
                       
                        else if ( kh.TrangThai == 1)
                        {
                            kh.text_color = "#3C8DBC";
                            kh.text_color_mota = "Đã đi bảo dưỡng theo kế hoạch";
                            //-   //-Màu xanh blue: kế hoạch đã vào điểm trước/ đúng giờ theo dự kiến
                            //e.Row.Cells[8].CssClass = "label label-primary";

                        }

                        else if (kh.NgayBaoDuongDuKien > DateTime.Now && kh.TrangThai == 0)
                        {
                            kh.text_color = "#00A65A";
                            kh.text_color_mota = "Kế hoạch chưa đến";
                            //-Màu xanh: kế hoạch chưa đến giờ
                            // e.Row.Cells[8].CssClass = "label label-success";

                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    rs.Add(kh);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
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
    public static int CapNhat(KeHoachBaoDuongOBJ kh)
    {
        int id = 0;

        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_Xe", kh.ID_Xe),
                new SqlParameter("@ID_Xe_KeHoachBaoDuong", kh.ID_Xe_KeHoachBaoDuong),
                new SqlParameter("@ID_NhanVien", kh.ID_NhanVien),
                new SqlParameter("@NgayBaoDuongDuKien", kh.NgayBaoDuongDuKien),
              
            };
              id = db.ExecuteNonQuery("sp_Xe_KeHoachBD_CapNhatKeHoach", param);
             

        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return id;
    }
    public static int ThemMoi(KeHoachBaoDuongOBJ kh)
    {
        int id = 0;

        try
        {
            SqlParameter[] param = new SqlParameter[]
            {
                  new SqlParameter("@ID_Xe", kh.ID_Xe),
                new SqlParameter("@ID_NhanVien", kh.ID_NhanVien),
                new SqlParameter("@NgayBaoDuongDuKien", kh.NgayBaoDuongDuKien),
              
            };
            id = db.ExecuteNonQuery("sp_Xe_KeHoachBD_ThemMoi", param);


        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return id;
    }
}