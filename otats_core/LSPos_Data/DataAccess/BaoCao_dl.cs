using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BaoCao_dl
/// </summary>
public class BaoCao_dl
{
    private SqlDataHelper helper;
    private static SqlDataHelper db = new SqlDataHelper();
	public BaoCao_dl()
	{
		//
		// TODO: Add constructor logic here
        //
        helper = new SqlDataHelper();
	}

    public DataSet BaoCaoDoanhThu(int IDQLLH, int ID_NhanVien, int ID_KhachHang, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            ds =   helper.ExecuteDataSet("sp_web_BaoCaoDoanhThu", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }

    public DataSet BaoCaoDoanhThuTheoNgay(int IDQLLH, int ID_NhanVien, int ID_KhachHang, DateTime date, int ID_QuanLy)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("date", date),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_QL_BaoCaoDoanhThuTheoNgay", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }

    public DataSet BaoCaoDoanhThuNgay(int IDQLLH,DateTime Ngay, int ID_QuanLy, int ID_Nhom,int ID_NhanVien)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
             new SqlParameter("ID_Nhom", ID_Nhom),
            new SqlParameter("Ngay", Ngay),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
             new SqlParameter("ID_NhanVien", ID_NhanVien)
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_QL_BaoCaoDoanhThuTrongNgay", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }
    public DataSet BaoCaoDoanhThuNgay_V2(int IDQLLH, DateTime Ngay, int ID_QuanLy,int ID_Nhom, int ID_KhachHang, int ID_NhanVien)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("Ngay", Ngay),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
             new SqlParameter("ID_Nhom", ID_Nhom),
             new SqlParameter("ID_KhachHang", ID_KhachHang),
              new SqlParameter("ID_NhanVien", ID_NhanVien)
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_QL_BaoCaoDoanhThuTrongNgay_v2", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }
    public DataSet BaoCaoTrangChu(int IDQLLH, int ID_QuanLy)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_QL_GetBaoCaoTrangChu", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }
    public DataSet BaoCaoTrangChu_v3(int IDQLLH, int ID_QuanLy)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_QL_GetBaoCaoTrangChu_v3", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }
    /// <summary>
    /// Hàm mới cập nhật v1.9.9.3 : nhân viên trực tuyến lấy theo trangthaiketnoi thay vì trước đó lấy theo trạng thái bản tin gps gửi lên
    /// 
    /// </summary>
    /// <param name="IDQLLH"></param>
    /// <param name="ID_QuanLy"></param>
    /// <returns></returns>
    public DataSet BaoCaoTrangChu_v4(int IDQLLH, int ID_QuanLy)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_QL_GetBaoCaoTrangChu_v4", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }
    public DataSet BaoCaoDonHangTongQuan(int IDQLLH,DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("from", dtFrom),
            new SqlParameter("to", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_QL_BaoCaoDonHangTongQuan", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }
    
    public DataSet BaoCaoMH_KH(int IDQLLH, int ID_Hang, int ID_KhachHang, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_Hang", ID_Hang),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_QL_BaoCaoMHKH_v2", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }

    public DataSet BaoCaoMH_KH_DH(int IDQLLH, int ID_Hang, int ID_KhachHang, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_Hang", ID_Hang),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_QL_BaoCaoMH_KH_DH", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }
    public DataSet BaoCaoMH_KH_DH_NV(int IDQLLH, int ID_Hang, int ID_KhachHang, DateTime dtFrom, DateTime dtTo, int ID_QuanLy, int ID_NhanVien)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_Hang", ID_Hang),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_QL_BaoCaoMH_KH_DH_NV", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }

    public DataSet BaoCaoMH_NV(int IDQLLH, int ID_Hang, int ID_NhanVien, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_Hang", ID_Hang),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_QL_BaoCaoMHNV", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }


    public DataSet BaoCaoMH_NV_ChiTiet(int IDQLLH, int ID_Hang, int ID_NhanVien, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_Hang", ID_Hang),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_QL_BaoCaoMHNV_ChiTiet", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }

    public DataSet BaoCaoDonHang(int IDQLLH, int ID_NhanVien, DateTime dtFrom, DateTime dtTo,int ID_QuanLy)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom),
            new SqlParameter("dtTo", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_QL_BaoCaoDonHang", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }

    public DataSet BaoCaoLoTrinh(int IDQLLH, int ID_NhanVien, DateTime dtReceivedTime)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtReceivedTime", dtReceivedTime)
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_web_BaoCaoLoTrinh", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }

    public DataTable BaoCaoAnhChup(int idcty, int idnhanvien, int idkhachhang, DateTime tungay, DateTime denNgay, int ID_QuanLy)
    {
        DataTable dt = null;
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", idcty),
            new SqlParameter("ID_NhanVien", idnhanvien),
            new SqlParameter("ID_KhachHang", idkhachhang),
            new SqlParameter("dtFrom", tungay),
            new SqlParameter("dtTo", denNgay),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            dt = helper.ExecuteDataSet("sp_web_BaoCaoAnhChup", pars).Tables[0];
        }
        catch (Exception)
        {
            dt = null;
        }
        
        return dt;
    }

    public DataTable BaoCaoQuangDuongDiChuyen(int ID_QLLH, int ID_NhanVien, DateTime tungay, DateTime denNgay, int ID_QuanLy)
    {
        DataTable dt = null;
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("tungay", tungay),
            new SqlParameter("denNgay", denNgay),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            dt = helper.ExecuteDataSet("sp_QL_BaoCaoKmGPSDiChuyenMoi_v2", pars).Tables[0];
        }
        catch (Exception)
        {
            dt = null;
        }

        return dt;
    }

    public DataTable BaoCaoDonHangTheoDiem(int ID_QLLH, int ID_NhanVien, DateTime TuNgay, DateTime DenNgay, int ID_QuanLy)
    {
        DataTable dt = null;
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("TuNgay", TuNgay),
            new SqlParameter("DenNgay", DenNgay),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            dt = helper.ExecuteDataSet("sp_QL_BaoCaoDonHangTheoDiem", pars).Tables[0];
        }
        catch (Exception)
        {
            dt = null;
        }

        return dt;
    }

    public DataTable BaoCaoDonHangTheoDiem_ChiTiet(int ID_NhanVien, DateTime TuNgay, DateTime DenNgay)
    {
        DataTable dt = null;
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("TuNgay", TuNgay),
            new SqlParameter("DenNgay", DenNgay),
        };
        try
        {
            dt = helper.ExecuteDataSet("sp_QL_BaoCaoDonHangTheoDiem_ChiTiet", pars).Tables[0];
        }
        catch (Exception)
        {
            dt = null;
        }

        return dt;
    }

    public int DeleteAnhChup(int ImageID)
    {
        try
        {
            return helper.ExecuteNonQuery("sp_QL_DeleteImage", new SqlParameter("ImageID", ImageID));
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public static ThongKeTrucTuyenOBJ ThongKeTrucTuyenIDCT(int idct,int ID_QuanLy,int ID_Nhom)
    {
        ThongKeTrucTuyenOBJ rs = new ThongKeTrucTuyenOBJ();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@idct", idct),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
            new SqlParameter("@ID_Nhom", ID_Nhom)
        };
        try
        {
            DataRow dr = db.ExecuteDataSet("sp_QL_ThongKeNhanVienTrucTuyen", pars).Tables[0].Rows[0];
            rs.tructuyen = int.Parse(dr["ol"].ToString());
            rs.chuadangnhapduoi60 = int.Parse(dr["60off"].ToString());
            rs.chuadangnhaptren60 = int.Parse(dr["off60"].ToString());
            rs.mattinhieuduoi60 = int.Parse(dr["60dc"].ToString());
            rs.mattinhieutren60 = int.Parse(dr["dc60"].ToString());
            rs.tong = int.Parse(dr["total"].ToString());

        }
        catch
        {
            rs.tructuyen = 0;
            rs.chuadangnhapduoi60 = 0;
            rs.chuadangnhaptren60 = 0;
            rs.mattinhieutren60 = 0;
            rs.mattinhieuduoi60 = 0;
            rs.tong = 0;
        }
        return rs;
    }
    /// <summary>
    /// v1.9.9.3 : Lay trang thai truc tuyen theo trang thai ket noi - TRUONGNM
    /// </summary>
    /// <param name="idct"></param>
    /// <param name="ID_QuanLy"></param>
    /// <param name="ID_Nhom"></param>
    /// <returns></returns>
    public static ThongKeTrucTuyenOBJ ThongKeTrucTuyenIDCT_v2(int idct, int ID_QuanLy, int ID_Nhom)
    {
        ThongKeTrucTuyenOBJ rs = new ThongKeTrucTuyenOBJ();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@idct", idct),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
            new SqlParameter("@ID_Nhom", ID_Nhom)
        };
        try
        {
            DataRow dr = db.ExecuteDataSet("sp_QL_ThongKeNhanVienTrucTuyen_v2", pars).Tables[0].Rows[0];
            rs.tructuyen = int.Parse(dr["ol"].ToString());
            rs.chuadangnhapduoi60 = int.Parse(dr["60off"].ToString());
            rs.chuadangnhaptren60 = int.Parse(dr["off60"].ToString());
            rs.mattinhieuduoi60 = int.Parse(dr["60dc"].ToString());
            rs.mattinhieutren60 = int.Parse(dr["dc60"].ToString());
            rs.tong = int.Parse(dr["total"].ToString());

        }
        catch
        {
            rs.tructuyen = 0;
            rs.chuadangnhapduoi60 = 0;
            rs.chuadangnhaptren60 = 0;
            rs.mattinhieutren60 = 0;
            rs.mattinhieuduoi60 = 0;
            rs.tong = 0;
        }
        return rs;
    }
    public DataSet BaoCaoTongHopTheoKhachHang(int IDQLLH,  int ID_KhachHang, DateTime dtFrom, DateTime dtTo, int ID_QuanLy)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", IDQLLH),
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("from", dtFrom),
            new SqlParameter("to", dtTo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_QL_BaoCaoTongHopTheoKhachHang", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }

    public DataSet BaoCaoThuHoiCongNo(int ID_QLLH, int ID_QuanLy, int ID_KhachHang, int ID_NhanVien, DateTime dtFrom, DateTime dtTo)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", ID_QLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
            new SqlParameter("@ID_KhachHang", ID_KhachHang),
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom.ToString("yyyy-MM-dd")),
            new SqlParameter("dtTo", dtTo.ToString("yyyy-MM-dd 23:59:59"))
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_QL_BaoCaoThuHoiCongNo", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }
    public DataSet BaoCaoThuHoiCongNo_ChiTiet(int ID_QLLH, int ID_QuanLy, int ID_KhachHang, int ID_NhanVien, DateTime dtFrom, DateTime dtTo)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
             new SqlParameter("@ID_QLLH", ID_QLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
            new SqlParameter("@ID_KhachHang", ID_KhachHang),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom.ToString("yyyy-MM-dd")),
            new SqlParameter("dtTo", dtTo.ToString("yyyy-MM-dd 23:59:59"))
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_QL_BaoCaoThuHoiCongNo_ChiTiet", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }

    public DataTable BaoCaoTatBatGPS(int ID_NhanVien, int ID_QuanLy, int ID_QLLH, DateTime TuNgay, DateTime DenNgay, int Loai)
    {
        DataTable dt = null;
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("IDNV", ID_NhanVien),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
               new SqlParameter("dtFrom", TuNgay.ToString("yyyy-MM-dd")),
            new SqlParameter("dtTo", DenNgay.ToString("yyyy-MM-dd")),
             new SqlParameter("Loai", Loai),
        };
        try
        {
            dt = helper.ExecuteDataSet("sp_BaoCaoMatTinHieuGPS", pars).Tables[0];
        }
        catch (Exception ex)
        {
            dt = null;
        }

        return dt;
    }
    public DataTable BaoCaoTatBatFakeGPS(int ID_NhanVien, int ID_QuanLy, int ID_QLLH, DateTime TuNgay, DateTime DenNgay, int Loai)
    {
        DataTable dt = null;
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("IDNV", ID_NhanVien),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("dtFrom", TuNgay.ToString("yyyy-MM-dd")),
            new SqlParameter("dtTo", DenNgay.ToString("yyyy-MM-dd")),
             new SqlParameter("Loai", Loai),
        };
        try
        {
            dt = helper.ExecuteDataSet("sp_BaoCaoBatTatFake", pars).Tables[0];
        }
        catch (Exception ex)
        {
            dt = null;
        }

        return dt;
    }

    public DataTable BaoCaoDungDo(int ID_NhanVien, int ID_QuanLy, int ID_QLLH, DateTime TuNgay, DateTime DenNgay)
    {
        DataTable dt = null;
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("dtFrom", TuNgay.ToString("yyyy-MM-dd")),
            new SqlParameter("dtTo", DenNgay.ToString("yyyy-MM-dd")),
             
        };
        try
        {
            dt = helper.ExecuteDataSet("sp_BaoCaoDungDo", pars).Tables[0];
        }
        catch (Exception ex)
        {
            dt = null;
        }

        return dt;
    }

    public DataTable BaoCaoKmDiChuyen(int ID_NhanVien, int ID_QuanLy, int ID_QLLH, DateTime TuNgay, DateTime DenNgay)
    {
        DataTable dt = null;
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("dtFrom", TuNgay.ToString("yyyy-MM-dd")),
            new SqlParameter("dtTo", DenNgay.ToString("yyyy-MM-dd 23:59:59")),

        };
        try
        {
            dt = helper.ExecuteDataSet("sp_NhanVien_BaoCaoKMDiChuyen", pars).Tables[0];
        }
        catch (Exception ex)
        {
            dt = null;
        }

        return dt;
    }
    public DataTable BaoCaoKmDiChuyen_TrongNgay(int ID_NhanVien, int ID_QuanLy, int ID_QLLH, DateTime TuNgay, DateTime DenNgay)
    {
        DataTable dt = null;
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("dtFrom", TuNgay.ToString("yyyy-MM-dd")),
           new SqlParameter("dtTo", DenNgay.ToString("yyyy-MM-dd 23:59:59")),

        };
        try
        {
            dt = helper.ExecuteDataSet("sp_NhanVien_BaoCaoKMDiChuyen_TrongNgay", pars).Tables[0];
        }
        catch (Exception ex)
        {
            dt = null;
        }

        return dt;
    }

    public DataSet BaoCaoLichSuThaoTac(int ID_QLLH, int ID_QuanLy, int ID_KhachHang, int ID_NhanVien, DateTime dtFrom, DateTime dtTo,int Loai)
    {
        log4net.ILog log = log4net.LogManager.GetLogger(typeof(BaoCao_dl));
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", ID_QLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
            new SqlParameter("@ID_KhachHang", ID_KhachHang),
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
            new SqlParameter("dtFrom", dtFrom.ToString("yyyy-MM-dd")),
            new SqlParameter("dtTo", dtTo.ToString("yyyy-MM-dd 23:59:59")),
             new SqlParameter("@Loai", Loai),
        };
        try
        {
           
            ds = helper.ExecuteDataSet("sp_BaoCao_LichSuThaoTac", pars);
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
        return ds;
    }


    public DataSet BaoCaoViengThamKhachHang(int ID_QLLH, int ID_QuanLy,int ID_Nhom, int ID_KhachHang, int ID_NhanVien, DateTime dtFrom, DateTime dtTo)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", ID_QLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
            new SqlParameter("@ID_KhachHang", ID_KhachHang),
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
             new SqlParameter("@ID_Nhom", ID_Nhom),
            new SqlParameter("@TuNgay", dtFrom.ToString("yyyy-MM-dd")),
            new SqlParameter("@DenNgay", dtTo.ToString("yyyy-MM-dd 23:59:59"))
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_BaoCao_ViengThamTheoKhachHang", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }

    public DataSet BaoCaoDoanhThuTongHopNhanVien(int ID_QLLH, int ID_QuanLy, int ID_Nhom, int ID_NhanVien, DateTime dtFrom, DateTime dtTo)
    {
        DataSet ds = new DataSet();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", ID_QLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
            new SqlParameter("@ID_NhanVien", ID_NhanVien),
             new SqlParameter("@ID_Nhom", ID_Nhom),
            new SqlParameter("@TuNgay", dtFrom.ToString("yyyy-MM-dd")),
            new SqlParameter("@DenNgay", dtTo.ToString("yyyy-MM-dd 23:59:59"))
        };
        try
        {
            ds = helper.ExecuteDataSet("sp_web_BaoCaoDoanhThu_TongHop", pars);
        }
        catch (Exception)
        {
            return null;
        }
        return ds;
    }


    public DataTable BaoCaoKhachHangMoMoi(int ID_NhanVien, int ID_KhachHang,  int ID_QuanLy, int ID_QLLH, DateTime TuNgay, DateTime DenNgay,int SoDonHang, double GiaTriDonHang)
    {
        DataTable dt = null;
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
                new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("TuNgay", TuNgay.ToString("yyyy-MM-dd")),
           new SqlParameter("DenNgay", DenNgay.ToString("yyyy-MM-dd 23:59:59")),
              new SqlParameter("SoDonHang", SoDonHang),
                 new SqlParameter("GiaTriDonHang", GiaTriDonHang),

        };
        try
        {
            dt = helper.ExecuteDataSet("sp_BaoCaoKhachHangMoMoi", pars).Tables[0];
        }
        catch (Exception ex)
        {
            dt = null;
        }

        return dt;
    }

    public DataTable BaoCaoKhachHangTheoGiaoDich(int ID_NhanVien, int ID_KhachHang, int ID_QuanLy, int ID_QLLH,   int SoNgay, int Loai)
    {
        DataTable dt = null;
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
                new SqlParameter("ID_KhachHang", ID_KhachHang),
                new SqlParameter("Loai", Loai),
                new SqlParameter("SoNgay", SoNgay),
        };
        try
        {
            dt = helper.ExecuteDataSet("sp_BaoCaoKhachHangTheoGiaoDich", pars).Tables[0];
        }
        catch (Exception ex)
        {
            dt = null;
        }

        return dt;
    }


    public DataTable BaoCaoTongHopPhanHoi(int ID_NhanVien, int ID_KhachHang, int ID_QuanLy, int ID_QLLH, DateTime TuNgay, DateTime DenNgay)
    {
        DataTable dt = null;
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
                new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("TuNgay", TuNgay.ToString("yyyy-MM-dd")),
           new SqlParameter("DenNgay", DenNgay.ToString("yyyy-MM-dd 23:59:59")), 

        };
        try
        {
            dt = helper.ExecuteDataSet("sp_PhanHoi_BaoCaoTongHop", pars).Tables[0];
        }
        catch (Exception ex)
        {
            dt = null;
        }

        return dt;
    }
    public DataTable BaoCaoChiTietPhanHoi(int ID_NhanVien, int ID_KhachHang, int ID_QuanLy, int ID_QLLH, DateTime TuNgay, DateTime DenNgay,int ID_PhanHoi)
    {
        DataTable dt = null;
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_PhanHoi", ID_PhanHoi),
              new SqlParameter("ID_QuanLy", ID_QuanLy),
                new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("TuNgay", TuNgay.ToString("yyyy-MM-dd")),
           new SqlParameter("DenNgay", DenNgay.ToString("yyyy-MM-dd 23:59:59")),

        };
        try
        {
            dt = helper.ExecuteDataSet("sp_PhanHoi_BaoCaoChiTiet", pars).Tables[0];
        }
        catch (Exception ex)
        {
            dt = null;
        }

        return dt;
    }

}