using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for HangHoa
/// </summary>
public class HangHoaDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(HangHoaDB));
   public  int ID_QLLH { set; get; }
    public int ID_Hang { set; get; }
   public  string MaHang { set; get; }
   public  string TenHang { set; get; }
   public double  GiaBanBuon { set; get; }
   public double  GiaBanLe { set; get; }
   public string TenDonVi { set; get; }
   public double SoLuong { set; get; }
   public string KhuyenMai { get; set; }
   public string GhiChuGia { get; set; }
   public static SqlDataHelper db = new SqlDataHelper();

	public HangHoaDB()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    //public static  List<HangHoaDB> getDS_HangHoa(int  Id_QLLH)
    //{
    //    try
    //    {
    //        List<HangHoaDB> lstHangHoa = new List<HangHoaDB>();
    //        SqlDataHelper sql = new SqlDataHelper();
    //        DataSet ds = sql.ExecuteDataSet("getDSHangHoaTheoID_QLLH",
    //            new SqlParameter("@id", Id_QLLH)
    //          );

    //        if (ds == null)
    //        {
    //            return null;
    //        }

    //        DataTable dtbl = null;
    //        dtbl = ds.Tables[0];
    //        for (int i = 0; i < dtbl.Rows.Count; i++)
    //        {
    //            DataRow dr = dtbl.Rows[i];
    //            int idHang = int.Parse(dr[1].ToString());
    //            string maHang = dr[2].ToString();
    //            string tenHang = dr[3].ToString();
    //            double giaBanBuon = double.Parse(dr[4].ToString());
    //            double giaBanLe = double.Parse(dr[5].ToString());
    //            string tenDonVi = dr[6].ToString();
    //            double soLuong = double.Parse(dr[7].ToString());
    //            string khuyenmai = dr[8].ToString();
    //            string ghichugia = dr[9].ToString();

    //            HangHoaDB hanghoa = new HangHoaDB();
    //            hanghoa.ID_Hang = idHang;
    //            hanghoa.MaHang = maHang;
    //            hanghoa.TenHang = tenHang;
    //            hanghoa.GiaBanBuon = giaBanBuon;
    //            hanghoa.GiaBanLe = giaBanLe;
    //            hanghoa.TenDonVi = tenDonVi;
    //            hanghoa.SoLuong = soLuong;
    //            hanghoa.KhuyenMai = khuyenmai;
    //            hanghoa.GhiChuGia = ghichugia;
    //            lstHangHoa.Add(hanghoa);
    //        }

    //        return lstHangHoa;
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
    //        return null;
    //    }
    //}

    //public static List<HangHoaOBJ> DanhSachHangHoaTheoIDCT(int idct)
    //{
    //    List<HangHoaOBJ> rs = new List<HangHoaOBJ>();

    //    try
    //    {

    //        SqlParameter[] pars = new SqlParameter[] {
    //            new SqlParameter("@idct", idct)
    //        };

    //        DataTable dt = db.ExecuteDataSet("sp_App_DanhSachMatHangTheoIDCT", pars).Tables[0];
    //        int a = dt.Rows.Count;
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            try
    //            {
    //                rs.Add(new HangHoaOBJ
    //                {
    //                    idhang = int.Parse(dr["ID_Hang"].ToString()),
    //                    mahang = dr["MaHang"].ToString(),
    //                    tenhang = dr["TenHang"].ToString(),
    //                    tenhienthi = dr["TenHang"].ToString(),

    //                    giabuon = dr["GiaBanBuon"].ToString() != "" ? double.Parse(dr["GiaBanBuon"].ToString()).ToString("N2") : (-1).ToString("N2"),
    //                    giale = dr["GiaBanLe"].ToString() != "" ? double.Parse(dr["GiaBanLe"].ToString()).ToString("N2") : (-1).ToString("N2"),
    //                    tonkho = dr["SoLuong"].ToString() != "" ? double.Parse(dr["SoLuong"].ToString()) : 0,
    //                    donvi = dr["TenDonVi"].ToString(),
    //                    ctkm =  CTKMDB.LayCTKM_TheoMatHang(idct,int.Parse(dr["ID_Hang"].ToString()),DateTime.Now),
    //                    soluong = dr["TonKhoOrder"].ToString() != "" ? double.Parse(dr["TonKhoOrder"].ToString()) : 0,
    //                    khuyenmai = dr["KhuyenMai"].ToString(),
    //                    iddanhmuc = (dr["ID_DANHMUC"].ToString() != "") ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0,
    //                    tendanhmuc = dr["TenDanhMuc"].ToString(),
    //                    ghichugia = dr["GhiChuGia"].ToString(),
    //                    tennhacungcap = dr["TenNhaCungCap"].ToString(),
    //                    tennhanhieu = dr["TenNhanHieu"].ToString(),
    //                    mota = dr["MoTa"].ToString(),
    //                    linkgioithieu = dr["LinkGioiThieu"].ToString(),
    //                });
    //            }
    //            catch { }
    //        }

    //        return rs;
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
    //        return rs;
    //    }
    //}
    //public static string LayTruongThongTinHienThiChiTietHangHoa(int idct)
    //{
    //    string str = "";

    //    try
    //    {

    //        SqlParameter[] pars = new SqlParameter[] {
    //            new SqlParameter("@idct", idct), 
    //        };

    //        DataTable dt = db.ExecuteDataSet("sp_PhanQuyen_HienThiChiTietSanPham_GetByID_QLLH", pars).Tables[0];
             
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            str = dr["HienThi"].ToString();
    //        }

          
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
           
    //    }
    //    return str;
    //}
    //public static  HangHoaOBJ  GetByID(int ID_HangHoa)
    //{
    //    HangHoaOBJ  rs = new HangHoaOBJ ();

    //    try
    //    {

    //        SqlParameter[] pars = new SqlParameter[] {
    //            new SqlParameter("@ID_HangHoa", ID_HangHoa) 
    //        };

    //        DataTable dt = db.ExecuteDataSet("sp_HangHoa_GetByID", pars).Tables[0];
    //        int a = dt.Rows.Count;
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            try
    //            {
    //                rs  = new HangHoaOBJ
    //                {
    //                    idhang = int.Parse(dr["ID_Hang"].ToString()),
    //                    mahang = dr["MaHang"].ToString(),
    //                    tenhang = dr["TenHang"].ToString(),
    //                    tenhienthi = dr["TenHang"].ToString(),
    //                    giabuon = dr["GiaBanBuon"].ToString() != "" ? double.Parse(dr["GiaBanBuon"].ToString()).ToString("N2") : (-1).ToString("N2"),
    //                    giale = dr["GiaBanLe"].ToString() != "" ? double.Parse(dr["GiaBanLe"].ToString()).ToString("N2") : (-1).ToString("N2"),
    //                    tonkho = dr["SoLuong"].ToString() != "" ? double.Parse(dr["SoLuong"].ToString()) : 0,
    //                    donvi = dr["TenDonVi"].ToString(),
    //                    ctkm = CTKMDB.LayCTKM_TheoMatHang(int.Parse(dr["ID_QLLH"].ToString()), int.Parse(dr["ID_Hang"].ToString()), DateTime.Now),
    //                    soluong = dr["TonKhoOrder"].ToString() != "" ? double.Parse(dr["TonKhoOrder"].ToString()) : 0,
    //                    khuyenmai = dr["KhuyenMai"].ToString(),
    //                    iddanhmuc = (dr["ID_DANHMUC"].ToString() != "") ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0,
    //                    tendanhmuc = dr["TenDanhMuc"].ToString(),
    //                    ghichugia = dr["GhiChuGia"].ToString(),
    //                    tennhacungcap = dr["TenNhaCungCap"].ToString(),
    //                    tennhanhieu = dr["TenNhanHieu"].ToString(),
    //                    mota = dr["MoTa"].ToString(),
    //                    linkgioithieu = dr["LinkGioiThieu"].ToString(),
    //                } ;
    //            }
    //            catch { }
    //        }

    //        return rs;
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
    //        return rs;
    //    }
    //}

    public static HangHoaDB Get(int ID_HangHoa)
    {
        HangHoaDB rs = new HangHoaDB();

        try
        {

            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@ID_HangHoa", ID_HangHoa)
            };

            DataTable dt = db.ExecuteDataSet("sp_HangHoa_GetByID", pars).Tables[0];
            int a = dt.Rows.Count;
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    rs = new HangHoaDB
                    {
                        ID_Hang = int.Parse(dr["ID_Hang"].ToString()),
                        MaHang = dr["MaHang"].ToString(),
                        TenHang = dr["TenHang"].ToString(),
                        
                        GiaBanBuon = (dr["GiaBanBuon"].ToString() != "" ? double.Parse(dr["GiaBanBuon"].ToString()) : 0 ),
                        GiaBanLe = (dr["GiaBanLe"].ToString() != "" ? double.Parse(dr["GiaBanLe"].ToString()) : 0 ) 
    
                      
                    };
                }
                catch { }
            }

            return rs;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return rs;
        }
    }
    //theo phan quyen
    //public static List<HangHoaOBJ> DanhSachHangHoaTheoIDCT_TheoPhanQuyen(int idct,int ID_NhanVien)
    //{
    //    List<HangHoaOBJ> rs = new List<HangHoaOBJ>();

    //    try
    //    {

    //        SqlParameter[] pars = new SqlParameter[] {
    //            new SqlParameter("@idct", idct),
    //              new SqlParameter("@ID_NhanVien", ID_NhanVien)
    //        };

    //        DataTable dt = db.ExecuteDataSet("sp_HangHoa_DanhSachMatHangTheoIDCT_TheoPhanQuyen", pars).Tables[0];
    //        int a = dt.Rows.Count;
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            try
    //            {
    //                rs.Add(new HangHoaOBJ
    //                {
    //                    idhang = int.Parse(dr["ID_Hang"].ToString()),
    //                    mahang = dr["MaHang"].ToString(),
    //                    tenhang = dr["TenHang"].ToString(),
    //                    tenhienthi = dr["TenHang"].ToString(),
    //                    giabuon = dr["GiaBanBuon"].ToString() != "" ? double.Parse(dr["GiaBanBuon"].ToString()).ToString("N2") : (-1).ToString("N2"),
    //                    giale = dr["GiaBanLe"].ToString() != "" ? double.Parse(dr["GiaBanLe"].ToString()).ToString("N2") : (-1).ToString("N2"),
    //                    tonkho = dr["SoLuong"].ToString() != "" ? double.Parse(dr["SoLuong"].ToString()) : 0,
    //                    donvi = dr["TenDonVi"].ToString(),
    //                    ctkm = CTKMDB.LayCTKM_TheoMatHang(idct, int.Parse(dr["ID_Hang"].ToString()), DateTime.Now),
    //                    soluong = dr["TonKhoOrder"].ToString() != "" ? double.Parse(dr["TonKhoOrder"].ToString()) : 0,
    //                    khuyenmai = dr["KhuyenMai"].ToString(),
    //                    iddanhmuc = (dr["ID_DANHMUC"].ToString() != "") ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0,
    //                    tendanhmuc = dr["TenDanhMuc"].ToString(),
    //                    ghichugia = dr["GhiChuGia"].ToString(),
    //                    tennhacungcap = dr["TenNhaCungCap"].ToString(),
    //                    tennhanhieu = dr["TenNhanHieu"].ToString(),
    //                        mota = dr["MoTa"].ToString(),
    //                    linkgioithieu = dr["LinkGioiThieu"].ToString(),
    //                });
    //            }
    //            catch { }
    //        }

    //        return rs;
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
    //        return rs;
    //    }
    //}
    //public static DataTable DataHangHoaTheoIDCT_TheoPhanQuyen(int idct, int ID_NhanVien,string loctatca, int loaigia, double giatu, double giaden)
    //{
    //    DataTable dt = new DataTable();

    //    try
    //    {

    //        SqlParameter[] pars = new SqlParameter[] {
    //            new SqlParameter("@idct", idct),
    //              new SqlParameter("@ID_NhanVien", ID_NhanVien),
    //               new SqlParameter("@loctatca", loctatca),
    //                 new SqlParameter("@loaigia", loaigia),
    //               new SqlParameter("@giatu", giatu),
    //                new SqlParameter("@giaden", giaden),
    //        };

    //          dt = db.ExecuteDataSet("sp_HangHoa_DanhSachMatHangTheoIDCT_TheoPhanQuyen", pars).Tables[0];
             
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
           
    //    }
    //    return dt;
    //}
    //public static List<HangHoaOBJ> DanhSachHangHoaTheoIDCT_TheoPhanQuyen_Paging(int idct, int ID_NhanVien, int lastID, int SoLuongItemCanLay, string loctatca, int loaigia, double giatu, double giaden)
    //{
    //    List<HangHoaOBJ> rs = new List<HangHoaOBJ>();

    //    try
    //    {

    //        SqlParameter[] pars = new SqlParameter[] {
    //            new SqlParameter("@idct", idct),
    //              new SqlParameter("@ID_NhanVien", ID_NhanVien) ,
    //            new SqlParameter("@LastId", lastID),
    //            new SqlParameter("@SoLuongItemCanLay", SoLuongItemCanLay),
    //             new SqlParameter("@loctatca", loctatca),
    //              new SqlParameter("@loaigia", loaigia),
    //               new SqlParameter("@giatu", giatu),
    //                new SqlParameter("@giaden", giaden),

    //        };

    //        DataTable dt = db.ExecuteDataSet("sp_HangHoa_DanhSachMatHangTheoIDCT_TheoPhanQuyen_Paging", pars).Tables[0];
    //        int a = dt.Rows.Count;
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            try
    //            {
    //                rs.Add(new HangHoaOBJ
    //                {
    //                    idhang = int.Parse(dr["ID_Hang"].ToString()),
    //                    mahang = dr["MaHang"].ToString(),
    //                    tenhang = dr["TenHang"].ToString(),
    //                    tenhienthi = dr["TenHang"].ToString(),
    //                    giabuon = dr["GiaBanBuon"].ToString() != "" ? double.Parse(dr["GiaBanBuon"].ToString()).ToString("N2") : (-1).ToString("N2"),
    //                    giale = dr["GiaBanLe"].ToString() != "" ? double.Parse(dr["GiaBanLe"].ToString()).ToString("N2") : (-1).ToString("N2"),
    //                    tonkho = dr["SoLuong"].ToString() != "" ? double.Parse(dr["SoLuong"].ToString()) : 0,
    //                    donvi = dr["TenDonVi"].ToString(),
    //                    ctkm = CTKMDB.LayCTKM_TheoMatHang(idct, int.Parse(dr["ID_Hang"].ToString()), DateTime.Now),
    //                    soluong = dr["TonKhoOrder"].ToString() != "" ? double.Parse(dr["TonKhoOrder"].ToString()) : 0,
    //                    khuyenmai = dr["KhuyenMai"].ToString(),
    //                    iddanhmuc = (dr["ID_DANHMUC"].ToString() != "") ? int.Parse(dr["ID_DANHMUC"].ToString()) : 0,
    //                    tendanhmuc = dr["TenDanhMuc"].ToString(),
    //                    ghichugia = dr["GhiChuGia"].ToString(),
    //                    tennhacungcap = dr["TenNhaCungCap"].ToString(),
    //                    tennhanhieu = dr["TenNhanHieu"].ToString(),
    //                    mota = dr["MoTa"].ToString(),
    //                    linkgioithieu = dr["LinkGioiThieu"].ToString(),
    //                    RowNum = dr["RowNum"].ToString() != "" ? int.Parse(dr["RowNum"].ToString()) : 0,
    //                });
    //            }
    //            catch { }
    //        }

    //        return rs;
    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
    //        return rs;
    //    }
    //}
}