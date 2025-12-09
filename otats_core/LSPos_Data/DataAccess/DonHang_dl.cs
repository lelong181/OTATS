using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using log4net;
using LSPos_Data.Models;

/// <summary>
/// Summary description for DonHang_dl
/// </summary>
/// 


public class DonHang
{
   
    public int ID_DonHang { get; set; }
    public int ID_QLLH { get; set; }
    public int ID_NhanVien { get; set; }
    public string TenNhanVien { get; set; }
    public int ID_KhachHang { get; set; }
    public string TenKhachHang { get; set; }
    public string DienThoai { get; set; }
    public string DiaChi { get; set; }
    public double TongTien { get; set; }
    public DateTime NgayTao { get; set; }
    public DateTime ProcessDate { get; set; }
    public int HinhThucBan { get; set; }
    public int isProcess { get; set; }
    public string GhiChu { get; set; }
    public int DaXem { get; set; }
    public int ID_TrangThaiGiaoHang { get; set; }
    public int ID_TrangThaiThanhToan { get; set; }
    public string MaThamChieu { get; set; }
    public double TienDaThanhToan { get; set; }
    //2 cái này để lên pop-up
    public string ThoiGian { get; set; }
    public string SoTien { get; set; }
    private double _ConLai;
    public string LyDo { get; set; }
    public string NguoiThaoTac { get; set; }
    public string ToaDoKhachHang { get; set; }
    public double ConLai
    {
        get { return (TongTien - TienDaThanhToan >= 0 ? TongTien - TienDaThanhToan : 0); }
        set { _ConLai = value; }
    }
    public int ID_CTKM { get; set; }
    public string TenCTKM { get; set; }
    public double ChietKhauPhanTram { get; set; }
    public double ChietKhauTien { get; set; }
    public double TongTienChietKhau { get; set; }
    public string TenTrangThaiGiaoHang { get; set; }
    public int ID_TrangThaiDongHang { get; set; }
    public string TenTrangThaiDongHang { get; set; }
    public string isProcess_Name { get; set; }
    public string MaKH { get; set; }

    public DateTime LastUpdate_ThoiGian_NhanVien { get; set; }
    public DateTime LastUpdate_ThoiGian_QuanLy { get; set; }
    public int LastUpdate_ID_NhanVien { get; set; }
    public int LastUpdate_ID_QuanLy { get; set; }

    public string LastUpdate_Ten_NhanVien { get; set; }
    public string LastUpdate_Ten_QuanLy { get; set; }
    public double KinhDo { get; set; }
    public double ViDo { get; set; }
    public string DiaChiTao { get; set; }
    public string DiaChiXuatHoaDon { get; set; }
    public int SoLanInHoaDon { get; set; }
    public List<AlbumOBJ> danhsachanh { get; set; }

}


public class DonHangv2
{

    public int ID_DonHang { get; set; }
    public int ID_QLLH { get; set; }
    public int ID_NhanVien { get; set; }
    public string TenNhanVien { get; set; }
    public int ID_KhachHang { get; set; }
    public string TenKhachHang { get; set; }
    public string DienThoai { get; set; }
    public string DiaChi { get; set; }
    public string Email { get; set; }
    public string EmailHDV { get; set; }
    public double TongTien { get; set; }
    public DateTime NgayTao { get; set; }
    public DateTime ProcessDate { get; set; }
    public int HinhThucBan { get; set; }
    public int isProcess { get; set; }
    public string GhiChu { get; set; }
    public int DaXem { get; set; }
    public int ID_TrangThaiGiaoHang { get; set; }
    public int ID_TrangThaiThanhToan { get; set; }
    public string MaThamChieu { get; set; }
    public double TienDaThanhToan { get; set; }
    //2 cái này để lên pop-up
    public string ThoiGian { get; set; }
    public string SoTien { get; set; }
    private double _ConLai;
    public string LyDo { get; set; }
    public string NguoiThaoTac { get; set; }
    public string ToaDoKhachHang { get; set; }
    public double ConLai
    {
        get { return (TongTien - TienDaThanhToan >= 0 ? TongTien - TienDaThanhToan : 0); }
        set { _ConLai = value; }
    }
    public int ID_CTKM { get; set; }
    public string TenCTKM { get; set; }
    public double ChietKhauPhanTram { get; set; }
    public double ChietKhauTien { get; set; }
    public double TongTienChietKhau { get; set; }
    public string TenTrangThaiGiaoHang { get; set; }
    public int ID_TrangThaiDongHang { get; set; }
    public string TenTrangThaiDongHang { get; set; }
    public string isProcess_Name { get; set; }
    public string MaKH { get; set; }

    public DateTime LastUpdate_ThoiGian_NhanVien { get; set; }
    public DateTime LastUpdate_ThoiGian_QuanLy { get; set; }
    public int LastUpdate_ID_NhanVien { get; set; }
    public int LastUpdate_ID_QuanLy { get; set; }

    public string LastUpdate_Ten_NhanVien { get; set; }
    public string LastUpdate_Ten_QuanLy { get; set; }
    public double KinhDo { get; set; }
    public double ViDo { get; set; }
    public string DiaChiTao { get; set; }
    public string DiaChiXuatHoaDon { get; set; }
    public int SoLanInHoaDon { get; set; }
    public List<AlbumOBJ> danhsachanh { get; set; }
    public List<CTKMOBJ> dskhuyenmai { get; set; }
    public double ChietKhauPhanTramTheoCTKM { get; set; }
    public double ChietKhauTienTheoCTKM { get; set; }
    public double ChietKhauPhanTramKhac { get; set; }
    public double ChietKhauTienKhac { get; set; }
    public bool XuatHoaDon { get; set; }
    public bool InVeTaiQuay { get; set; }
    public List<DonHang_DichVuRequestAPIModel> chitietdonhang { get; set; }
    public List<LichSuThanhToanModel> lichsuthanhtoan { get; set; }
    public NhanVien NhanVien { get; set; }
    public object MaThanhToan { get; set; }
}
public class ChiTietDonHang
{
    public int STT { get; set; }
    public int STT2 { get; set; }
    public int ID_ChiTietDonHang { get; set; }
    public int ID_DonHang { get; set; }
    public int ID_HangHoa { get; set; }
    public string TenHang { get; set; }
    public string MaHang { get; set; }
    public double SoLuong { get; set; }
    public double DaGiao { get; set; }
    public int HinhThucBan { get; set; }
    public double TongTien { get; set; }
    public double GiaBan { get; set; }
    public double DaThanhToan { get; set; }
    public string GhiChu { get; set; }
    public string TenDonVi { get; set; }
    public double ChietKhauPhanTram { get; set; }
    public double ChietKhauTien { get; set; }
    public double TongTienChietKhau { get; set; }

    public double PhanTramHaoHut { get; set; }
    public double SoLuongHaoHut { get; set; }
    public double SoLuongTraLai { get; set; }
    public int ID_CTKM { get; set; }
    public string TenKho { get; set; }
}
public class TrangThaiGiaoHang
{
    public int ID_TrangThaiGiaoHang { get; set; }
    public string TenTrangThaiGiaoHang { get; set; }
}

public class TrangThaiThanhToan
{
    public int ID_TrangThaiThanhToan { get; set; }
    public string TenTrangThaiThanhToan { get; set; }
}
public class CTKMOBJ
{
    public CTKMOBJ()
    {
    }



    public int idctkm { get; set; }
    public int idct { get; set; }
    public int idnhanvien { get; set; }
    public int idquanly { get; set; }
    public int loai { get; set; }
    public double chietkhauphantram { get; set; }
    public double chietkhautien { get; set; }

    public string tenctkm { get; set; }
    public string ghichu { get; set; }
    public DateTime ngayapdung { get; set; }
    public DateTime ngaytao { get; set; }
    public DateTime ngayketthuc { get; set; }
    public int trangthai { get; set; }
    public int hethan { get; set; }
 
    public int dachon { get; set; }
}

public class DonHang_dl
{
    ILog log = LogManager.GetLogger(typeof(DonHang));
    private SqlDataHelper helper;

    public DonHang_dl()
    {
        //
        // TODO: Add constructor logic here
        //
        helper = new SqlDataHelper();
    }

    public DonHang GetDonHangFromDataRow(DataRow dr)
    {
        try
        {
            DonHang dh = new DonHang();

            dh.ID_DonHang = int.Parse(dr["ID_DonHang"].ToString());
            dh.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            dh.ID_NhanVien = int.Parse(dr["ID_NhanVien"].ToString());
            dh.ID_KhachHang = int.Parse(dr["ID_KhachHang"].ToString());
            dh.TenNhanVien = dr["TenNhanVien"].ToString();
            dh.TenKhachHang = dr["TenKhachHang"].ToString();
            dh.DienThoai = dr["DienThoai"].ToString();
            dh.DiaChi = dr["DiaChi"].ToString();
            dh.TongTien = dr["TongTien"].ToString() != "" ? double.Parse(dr["TongTien"].ToString()) : 0;
            dh.NgayTao = Convert.ToDateTime(dr["CreateDate"].ToString());
            dh.isProcess = int.Parse(dr["IsProcess"].ToString());
            if (dh.isProcess == 0)
            {
                dh.isProcess_Name = "Chưa hoàn tất";
            }
            else if (dh.isProcess == 1)
            {
                dh.isProcess_Name = "Đã hoàn tất";
            }
            else if (dh.isProcess == 2)
            {
                dh.isProcess_Name = "Hủy";
            }
            dh.HinhThucBan = int.Parse(dr["HinhThucBan"].ToString());
            dh.GhiChu = dr["GhiChu"].ToString();
            dh.DaXem = Convert.ToInt32(dr["DaXem"]);
            dh.ID_TrangThaiGiaoHang = dr["ID_TrangThaiGiaoHang"].ToString() != "" ?  Convert.ToInt32(dr["ID_TrangThaiGiaoHang"]) : 0;
           
            dh.ID_TrangThaiThanhToan = dr["ID_TrangThaiThanhToan"].ToString() != "" ? Convert.ToInt32(dr["ID_TrangThaiThanhToan"]) : 0; 
            dh.MaThamChieu = dr["MaThamChieu"].ToString();
            try
            {
                dh.TienDaThanhToan = dr["TienDaThanhToan"].ToString() != "" ? double.Parse(dr["TienDaThanhToan"].ToString()) : 0;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                dh.TienDaThanhToan = 0;
                
            }
            dh.TenTrangThaiGiaoHang = dr["TenTrangThaiGiaoHang"].ToString();
            try
            {
                dh.ID_CTKM = dr["ID_CTKM"].ToString() != "" ? Convert.ToInt32(dr["ID_CTKM"]) : 0;
                dh.TenCTKM = dr["TenCTKM"].ToString();
                dh.ChietKhauPhanTram = dr["ChietKhauPhanTram"].ToString() != "" ? double.Parse(dr["ChietKhauPhanTram"].ToString()) : 0;
                dh.ChietKhauTien = dr["ChietKhauTien"].ToString() != "" ? Convert.ToDouble(dr["ChietKhauTien"]) : 0;
                dh.TongTienChietKhau = dr["TongTienChietKhau"].ToString() != "" ? Convert.ToDouble(dr["TongTienChietKhau"]) : 0;
                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            dh.ProcessDate = dr["ProcessDate"].ToString() != "" ? Convert.ToDateTime(dr["ProcessDate"].ToString()) : new DateTime(1900, 1, 1);
             dh.ID_TrangThaiDongHang = dr["ID_TrangThaiDonHang"].ToString() != "" ? Convert.ToInt32(dr["ID_TrangThaiDonHang"]) : 0;
            dh.TenTrangThaiDongHang = dr["TenTrangThai"].ToString();
            try
            {
                dh.LyDo = dr["LyDo"].ToString();
            }
            catch (Exception)
            {
                dh.LyDo = "";
            }
            try
            {
                dh.MaKH = dr["MaKH"].ToString();
                dh.NguoiThaoTac = dr["NguoiThaoTac"].ToString();
                
            }
            catch (Exception)
            {
                dh.NguoiThaoTac = "";
            }


            try
            {
                dh.ToaDoKhachHang = dr["ToaDoKhachHang"].ToString();
              
            }
            catch (Exception)
            {
                
            }

            try
            {
                dh.DiaChiTao = dr["DiaChiTao"].ToString();
                dh.KinhDo = dr["KinhDo"].ToString() != "" ? double.Parse(dr["KinhDo"].ToString()) : 0;
                dh.ViDo = dr["ViDo"].ToString() != "" ? double.Parse(dr["ViDo"].ToString()) : 0;
                dh.DiaChiXuatHoaDon = dr["DiaChiXuatHoaDon"].ToString();
                dh.SoLanInHoaDon = dr["SoLanInHoaDon"].ToString() != "" ? int.Parse(dr["SoLanInHoaDon"].ToString()) : 0;
            }
            catch (Exception)
            {

            }

            try
            {
                dh.LastUpdate_ID_NhanVien = dr["LastUpdate_ID_NhanVien"].ToString() != "" ? Convert.ToInt32(dr["LastUpdate_ID_NhanVien"]) : 0;
                dh.LastUpdate_ID_QuanLy = dr["LastUpdate_ID_QuanLy"].ToString() != "" ? Convert.ToInt32(dr["LastUpdate_ID_QuanLy"]) : 0;
                dh.LastUpdate_ThoiGian_NhanVien = dr["LastUpdate_ThoiGian_NhanVien"].ToString() != "" ? Convert.ToDateTime(dr["LastUpdate_ThoiGian_NhanVien"]) : dh.LastUpdate_ThoiGian_NhanVien;
                dh.LastUpdate_ThoiGian_QuanLy = dr["LastUpdate_ThoiGian_QuanLy"].ToString() != "" ? Convert.ToDateTime(dr["LastUpdate_ThoiGian_QuanLy"]) : dh.LastUpdate_ThoiGian_QuanLy;
                dh.LastUpdate_Ten_NhanVien = dr["LastUpdate_Ten_NhanVien"].ToString();
                dh.LastUpdate_Ten_QuanLy = dr["LastUpdate_Ten_QuanLy"].ToString();

            }
            catch (Exception)
            {

            }

            //dh.danhsachanh = ImageDB.DanhSachAlbum_TheoDonHang(dh.ID_DonHang);

            //dh.DaXem = Convert.ToInt32(dr["ID_TrangThaiGiaoHang"]);

            //dh.DaXem = Convert.ToInt32(dr["DaXem"]);
            return dh;
        }
        catch(Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public DonHangv2 GetDonHangFromDataRow_v2(DataRow dr)
    {
        try
        {
            DonHangv2 dh = new DonHangv2();

            dh.ID_DonHang = int.Parse(dr["ID_DonHang"].ToString());
            dh.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            dh.ID_NhanVien = int.Parse(dr["ID_NhanVien"].ToString());
            dh.ID_KhachHang = int.Parse(dr["ID_KhachHang"].ToString());
            dh.TenNhanVien = dr["TenNhanVien"].ToString();
            dh.TenKhachHang = dr["TenKhachHang"].ToString();
            dh.DienThoai = dr["DienThoai"].ToString();
            dh.DiaChi = dr["DiaChi"].ToString();
            dh.Email = dr["Email"].ToString();
            dh.TongTien = dr["TongTien"].ToString() != "" ? double.Parse(dr["TongTien"].ToString()) : 0;
            dh.NgayTao = Convert.ToDateTime(dr["CreateDate"].ToString());
            dh.isProcess = int.Parse(dr["IsProcess"].ToString());
            if (dh.isProcess == 0)
            {
                dh.isProcess_Name = "Chưa hoàn tất";
            }
            else if (dh.isProcess == 1)
            {
                dh.isProcess_Name = "Đã hoàn tất";
            }
            else if (dh.isProcess == 2)
            {
                dh.isProcess_Name = "Hủy";
            }
            dh.HinhThucBan = int.Parse(dr["HinhThucBan"].ToString());
            dh.GhiChu = dr["GhiChu"].ToString();
            dh.DaXem = Convert.ToInt32(dr["DaXem"]);
            dh.ID_TrangThaiGiaoHang = dr["ID_TrangThaiGiaoHang"].ToString() != "" ? Convert.ToInt32(dr["ID_TrangThaiGiaoHang"]) : 0;

            dh.ID_TrangThaiThanhToan = dr["ID_TrangThaiThanhToan"].ToString() != "" ? Convert.ToInt32(dr["ID_TrangThaiThanhToan"]) : 0;
            dh.MaThamChieu = dr["MaThamChieu"].ToString();
            try
            {
                dh.TienDaThanhToan = dr["TienDaThanhToan"].ToString() != "" ? double.Parse(dr["TienDaThanhToan"].ToString()) : 0;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                dh.TienDaThanhToan = 0;

            }
            dh.TenTrangThaiGiaoHang = dr["TenTrangThaiGiaoHang"].ToString();
            try
            {
                dh.ID_CTKM = dr["ID_CTKM"].ToString() != "" ? Convert.ToInt32(dr["ID_CTKM"]) : 0;
                dh.TenCTKM = dr["TenCTKM"].ToString();
                dh.ChietKhauPhanTram = dr["ChietKhauPhanTram"].ToString() != "" ? double.Parse(dr["ChietKhauPhanTram"].ToString()) : 0;
                dh.ChietKhauTien = dr["ChietKhauTien"].ToString() != "" ? Convert.ToDouble(dr["ChietKhauTien"]) : 0;
                dh.TongTienChietKhau = dr["TongTienChietKhau"].ToString() != "" ? Convert.ToDouble(dr["TongTienChietKhau"]) : 0;

                dh.ChietKhauPhanTramTheoCTKM = dr["ChietKhauPhanTram_TheoCTKM"].ToString() != "" ? double.Parse(dr["ChietKhauPhanTram_TheoCTKM"].ToString()) : 0;
                dh.ChietKhauTienTheoCTKM = dr["ChietKhauTien_TheoCTKM"].ToString() != "" ? double.Parse(dr["ChietKhauTien_TheoCTKM"].ToString()) : 0;
                dh.ChietKhauPhanTramKhac = dr["ChietKhauPhanTram_Khac"].ToString() != "" ? double.Parse(dr["ChietKhauPhanTram_Khac"].ToString()) : 0;
                dh.ChietKhauTienKhac = dr["ChietKhauTien_Khac"].ToString() != "" ? double.Parse(dr["ChietKhauTien_Khac"].ToString()) : 0;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            dh.ProcessDate = dr["ProcessDate"].ToString() != "" ? Convert.ToDateTime(dr["ProcessDate"].ToString()) : new DateTime(1900, 1, 1);
            dh.ID_TrangThaiDongHang = dr["ID_TrangThaiDonHang"].ToString() != "" ? Convert.ToInt32(dr["ID_TrangThaiDonHang"]) : 0;
            dh.TenTrangThaiDongHang = dr["TenTrangThai"].ToString();
            try
            {
                dh.LyDo = dr["LyDo"].ToString();
            }
            catch (Exception)
            {
                dh.LyDo = "";
            }
            try
            {
                dh.MaKH = dr["MaKH"].ToString();
                dh.NguoiThaoTac = dr["NguoiThaoTac"].ToString();

            }
            catch (Exception)
            {
                dh.NguoiThaoTac = "";
            }


            try
            {
                dh.ToaDoKhachHang = dr["ToaDoKhachHang"].ToString();

            }
            catch (Exception)
            {

            }

            try
            {
                dh.DiaChiTao = dr["DiaChiTao"].ToString();
                dh.KinhDo = dr["KinhDo"].ToString() != "" ? double.Parse(dr["KinhDo"].ToString()) : 0;
                dh.ViDo = dr["ViDo"].ToString() != "" ? double.Parse(dr["ViDo"].ToString()) : 0;
                dh.DiaChiXuatHoaDon = dr["DiaChiXuatHoaDon"].ToString();
                dh.SoLanInHoaDon = dr["SoLanInHoaDon"].ToString() != "" ? int.Parse(dr["SoLanInHoaDon"].ToString()) : 0;
            }
            catch (Exception)
            {

            }

            try
            {
                dh.LastUpdate_ID_NhanVien = dr["LastUpdate_ID_NhanVien"].ToString() != "" ? Convert.ToInt32(dr["LastUpdate_ID_NhanVien"]) : 0;
                dh.LastUpdate_ID_QuanLy = dr["LastUpdate_ID_QuanLy"].ToString() != "" ? Convert.ToInt32(dr["LastUpdate_ID_QuanLy"]) : 0;
                dh.LastUpdate_ThoiGian_NhanVien = dr["LastUpdate_ThoiGian_NhanVien"].ToString() != "" ? Convert.ToDateTime(dr["LastUpdate_ThoiGian_NhanVien"]) : dh.LastUpdate_ThoiGian_NhanVien;
                dh.LastUpdate_ThoiGian_QuanLy = dr["LastUpdate_ThoiGian_QuanLy"].ToString() != "" ? Convert.ToDateTime(dr["LastUpdate_ThoiGian_QuanLy"]) : dh.LastUpdate_ThoiGian_QuanLy;
                dh.LastUpdate_Ten_NhanVien = dr["LastUpdate_Ten_NhanVien"].ToString();
                dh.LastUpdate_Ten_QuanLy = dr["LastUpdate_Ten_QuanLy"].ToString();

            }
            catch (Exception)
            {

            }

            //dh.danhsachanh = ImageDB.DanhSachAlbum_TheoDonHang(dh.ID_DonHang);
            dh.dskhuyenmai = GetDanhSachCTKM_DonHang(dh.ID_DonHang);
            //dh.DaXem = Convert.ToInt32(dr["ID_TrangThaiGiaoHang"]);

            //dh.DaXem = Convert.ToInt32(dr["DaXem"]);
            return dh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public static List<CTKMOBJ> GetDanhSachCTKM_DonHang(int ID_DonHang)
    {
        List<CTKMOBJ> lstrs = new List<CTKMOBJ>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                    new SqlParameter("ID_DonHang", ID_DonHang)
            };
            SqlDataHelper helper = new SqlDataHelper();
            DataTable dt = helper.ExecuteDataSet("sp_CTKM_DonHang_GetByID_DonHang", par).Tables[0];
            DateTime d;
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    CTKMOBJ rs = new CTKMOBJ();
                    rs.idctkm = int.Parse(dr["ID_CTKM"].ToString());
                    rs.idct = int.Parse(dr["ID_QLLH"].ToString());
                    rs.idnhanvien = (dr["ID_NhanVien"].ToString() != "") ? int.Parse(dr["ID_NhanVien"].ToString()) : 0;
                    rs.idquanly = (dr["ID_QuanLy"].ToString() != "") ? int.Parse(dr["ID_QuanLy"].ToString()) : 0;
                    rs.loai = (dr["Loai"].ToString() != "") ? int.Parse(dr["Loai"].ToString()) : 0;
                    rs.chietkhauphantram = (dr["ChietKhauPhanTram"].ToString() != "") ? double.Parse(dr["ChietKhauPhanTram"].ToString()) : 0;
                    rs.chietkhautien = (dr["ChietKhauTien"].ToString() != "") ? double.Parse(dr["ChietKhauTien"].ToString()) : 0;
                    try
                    {
                        if (rs.ngayketthuc.Year != 1900 && (new DateTime(rs.ngayketthuc.Year, rs.ngayketthuc.Month, rs.ngayketthuc.Day) < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)))
                        {
                            rs.hethan = 1;
                        }
                        else
                        {
                            rs.hethan = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        rs.hethan = 0;
                    }
                    rs.ngayapdung = (dr["NgayApDung"].ToString() != "") ? Convert.ToDateTime(dr["NgayApDung"].ToString()) : rs.ngayapdung;
                    rs.ngayketthuc = (dr["NgayKetThuc"].ToString() != "") ? Convert.ToDateTime(dr["NgayKetThuc"].ToString()) : rs.ngayketthuc;
                    rs.ngaytao = (dr["NgayTao"].ToString() != "") ? Convert.ToDateTime(dr["NgayTao"].ToString()) : rs.ngaytao; Convert.ToDateTime(dr["NgayTao"].ToString());
                    rs.ghichu = dr["GhiChu"].ToString();
                    rs.tenctkm = dr["TenCTKM"].ToString();
                    rs.trangthai = (dr["trangthai"].ToString() != "") ? int.Parse(dr["trangthai"].ToString()) : 0;
                    
                    

                    lstrs.Add(rs);


                }
                catch (Exception ex)
                {
                     
                }
            }
        }
        catch (Exception ex)
        {
            
        }
        return lstrs;
    }

    //public List<DonHang> GetDSDonHangAll(int ID_QLLH, int ID_QuanLy, DateTime from, DateTime to, int trangthaigiao, int trangthaithanhtoan, int trangthaihoanthanh,int ID_KhachHang,int ID_NhanVien,int ID_HangHoa)
    //{
    //    List<DonHang> dsdh = new List<DonHang>();
    //    SqlParameter[] pars = new SqlParameter[] {

    //        new SqlParameter("ID_QLLH", ID_QLLH),
    //        new SqlParameter("ID_QuanLy", ID_QuanLy),
    //        new SqlParameter("from", from),
    //        new SqlParameter("to", to),
    //        new SqlParameter("trangthaigiao", trangthaigiao),
    //        new SqlParameter("trangthaithanhtoan", trangthaithanhtoan),
    //        new SqlParameter("trangthaihoanthanh",trangthaihoanthanh)  ,
    //        new SqlParameter("ID_KhachHang", ID_KhachHang),
    //        new SqlParameter("ID_NhanVien", ID_NhanVien),
    //         new SqlParameter("ID_HangHoa", ID_HangHoa)
    //    };

    //    DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllDonHang", pars);
    //    DataTable dt = ds.Tables[0];



    //    try
    //    {

    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            DonHang dh = GetDonHangFromDataRow(dr);
    //            dsdh.Add(dh);
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);

    //    }
    //    return dsdh;
    //}

    public List<DonHang> GetDSDonHangAll(int ID_QLLH, int ID_QuanLy, DateTime from, DateTime to, int trangthaigiao, int trangthaithanhtoan, int trangthaihoanthanh, int ID_KhachHang, int ID_NhanVien, int ID_HangHoa, string ListIDNhom)
    {
        if (ListIDNhom.EndsWith(","))
        {
            ListIDNhom = ListIDNhom.Substring(0, ListIDNhom.Length - 1);
        }

        List<DonHang> dsdh = new List<DonHang>();
        SqlParameter[] pars = new SqlParameter[] {

            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("from", from),
            new SqlParameter("to", to),
            new SqlParameter("trangthaigiao", trangthaigiao),
            new SqlParameter("trangthaithanhtoan", trangthaithanhtoan),
            new SqlParameter("trangthaihoanthanh",trangthaihoanthanh)  ,
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
             new SqlParameter("ID_HangHoa", ID_HangHoa),
              new SqlParameter("ListIDNhom", ListIDNhom)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllDonHang_v2", pars);
        DataTable dt = ds.Tables[0];



        try
        {

            foreach (DataRow dr in dt.Rows)
            {
                DonHang dh = GetDonHangFromDataRow(dr);
                dsdh.Add(dh);
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return dsdh;
    }
    public DataTable GetDataDonHangAll(int ID_QLLH, int ID_QuanLy, DateTime from, DateTime to, int trangthaigiao, int trangthaithanhtoan, int trangthaihoanthanh, int ID_KhachHang, int ID_NhanVien, int ID_HangHoa, string ListIDNhom)
    {
        if (ListIDNhom.EndsWith(","))
        {
            ListIDNhom = ListIDNhom.Substring(0, ListIDNhom.Length - 1);
        }

        DataTable dt = new DataTable();
        SqlParameter[] pars = new SqlParameter[] {

            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("from", from),
            new SqlParameter("to", to),
            new SqlParameter("trangthaigiao", trangthaigiao),
            new SqlParameter("trangthaithanhtoan", trangthaithanhtoan),
            new SqlParameter("trangthaihoanthanh",trangthaihoanthanh)  ,
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
             new SqlParameter("ID_HangHoa", ID_HangHoa),
              new SqlParameter("ListIDNhom", ListIDNhom)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllDonHang_v2", pars);
          dt = ds.Tables[0];



        
        return dt;
    }

    public List<DonHang> GetDSDonHangAll_ChuaXem(int ID_QLLH, int ID_QuanLy, DateTime from, DateTime to, int trangthaigiao, int trangthaithanhtoan, int trangthaihoanthanh)
    {
        SqlParameter[] pars = new SqlParameter[] {
           
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("from", from),
            new SqlParameter("to", to),
            new SqlParameter("trangthaigiao", trangthaigiao),
            new SqlParameter("trangthaithanhtoan", trangthaithanhtoan),
            new SqlParameter("trangthaihoanthanh",trangthaihoanthanh)  
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllDonHang_ChuaXem", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<DonHang> dsdh = new List<DonHang>();
            foreach (DataRow dr in dt.Rows)
            {
                DonHang dh = GetDonHangFromDataRow(dr);
                dsdh.Add(dh);
            }
            return dsdh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public List<DonHang> GetDSDonHangThanhCong(int ID_QLLH, int ID_QuanLy, DateTime from, DateTime to)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("from", from),
            new SqlParameter("to", to)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllDonHangThanhCong", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<DonHang> dsdh = new List<DonHang>();
            foreach (DataRow dr in dt.Rows)
            {
                DonHang dh = GetDonHangFromDataRow(dr);
                dsdh.Add(dh);
            }
            return dsdh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public List<TrangThaiGiaoHang> GetDSTrangThaiGiaoHang()
    {

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllTrangThaiGiaoHang", null);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<TrangThaiGiaoHang> dsttgh = new List<TrangThaiGiaoHang>();
            foreach (DataRow dr in dt.Rows)
            {
                TrangThaiGiaoHang ttgh = new TrangThaiGiaoHang();
                ttgh.ID_TrangThaiGiaoHang = Convert.ToInt32(dr["ID_TrangThaiGiaoHang"]);
                ttgh.TenTrangThaiGiaoHang = dr["TenTrangThaiGiaoHang"].ToString();
                dsttgh.Add(ttgh);
            }
            return dsttgh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }


    public List<TrangThaiThanhToan> GetDSTrangThaiThanhToan()
    {

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllTrangThaiThanhToan", null);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<TrangThaiThanhToan> dstttt = new List<TrangThaiThanhToan>();
            foreach (DataRow dr in dt.Rows)
            {
                TrangThaiThanhToan tttt = new TrangThaiThanhToan();
                tttt.ID_TrangThaiThanhToan = Convert.ToInt32(dr["ID_TrangThaiThanhToan"]);
                tttt.TenTrangThaiThanhToan = dr["TenTrangThaiThanhToan"].ToString();
                dstttt.Add(tttt);
            }
            return dstttt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public List<DonHang> GetDSDonHangChuaDoc(int ID_QLLH, int ID_QuanLy, int SoLuongCanLay)
    {
        List<DonHang> dsdh = new List<DonHang>();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetDSDonHangChuaDoc", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
          
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                DonHang dh = GetDonHangFromDataRow(dr);
                dh.ThoiGian = dh.NgayTao.ToString("dd/MM/yyyy HH:mm:ss");
                dh.SoTien = String.Format("{0:c0}", dh.TongTien).Replace('$', ' ');
                dsdh.Add(dh);
                if(i == SoLuongCanLay)
                {
                    break;
                }
            }

           
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
        return dsdh;
    }

    public int GetSoLuongDonHangChuaXem(int ID_QLLH, int ID_QuanLy)
    {
        int ix = 0;
        try
        { 
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };

        object objds = helper.ExecuteScalar("sp_QL_GetSoLuongDonHangChuaXem", pars);

        if (objds != null)
        {
                ix = int.Parse(objds.ToString());
        }
         
         
        }
        catch (Exception ex)
        {
            log.Error(ex);
             
        }
        return ix;
    }
    public bool UpdateDonHang_DemSoLanXuat(int ID_DonHang)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_DonHang", ID_DonHang)
        };

            if (helper.ExecuteNonQuery("sp_DonHang_UpdateSoLanInHoaDon", pars) != 0)
            {
                // update thanh cong
                return true;
            }
            else
            {
                // update that bai
                return false;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return false;
        }
    }
    public DonHang GetDonHangTheoID(int IDDH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_DonHang", IDDH)
        };

        DataSet ds = helper.ExecuteDataSet("selectDonHangTheoID", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            DataRow dr = dt.Rows[0];
            DonHang dh = GetDonHangFromDataRow(dr);
            return dh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }
    public DonHangv2 GetDonHangTheoID_v2(int IDDH)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_DonHang", IDDH)
        };

        DataSet ds = helper.ExecuteDataSet("selectDonHangTheoID", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            DataRow dr = dt.Rows[0];
            DonHangv2 dh = GetDonHangFromDataRow_v2(dr);
            return dh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }
    public DataTable LichSuGiaoHang(int ID_DonHang)
    {
        DataTable dt = new DataTable();
        try
        {
            dt = helper.ExecuteDataSet("sp_QL_LichSuGiaoHang", new SqlParameter("ID_DonHang", ID_DonHang)).Tables[0];
            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return dt;
        }
    }

    public DataTable LichSuThanhToan(int ID_DonHang)
    {
        DataTable dt = new DataTable();
        try
        {
            dt = helper.ExecuteDataSet("sp_QL_LichSuThanhToan", new SqlParameter("ID_DonHang", ID_DonHang)).Tables[0];
            return dt;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return dt;
        }
    }

    public List<LichSuThanhToanModel> GetLichSuThanhToan(int ID_DonHang)
    {
        DataTable dt = new DataTable();
        List<LichSuThanhToanModel> result = new List<LichSuThanhToanModel>();
        try
        {
            dt = helper.ExecuteDataSet("sp_QL_LichSuThanhToan", new SqlParameter("ID_DonHang", ID_DonHang)).Tables[0];
            if(dt != null)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    LichSuThanhToanModel item = GetObjectFromDataRowUtil<LichSuThanhToanModel>.ToOject(dr);
                    result.Add(item);
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return result;
        }
    }

    public bool UpdateDonHangProcess(int ID_DonHang,string LyDo,int ID_QuanLy)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_DonHang", ID_DonHang),
            new SqlParameter("LyDo", LyDo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };

            if (helper.ExecuteNonQuery("updateDonHangIsProcess", pars) != 0)
            {
                // update thanh cong
                return true;
            }
            else
            {
                // update that bai
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    public bool HuyDonHang(int ID_DonHang, string LyDo, int ID_QuanLy)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_DonHang", ID_DonHang),
            new SqlParameter("LyDo", LyDo),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };

            if (helper.ExecuteNonQuery("sp_QL_HuyDonHang", pars) != 0)
            {
                // update thanh cong
                return true;
            }
            else
            {
                // update that bai
                return false;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return false;
        }
    }

    public bool UpdateDonHangDaXem(int ID_DonHang)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_DonHang", ID_DonHang)
        };

            if (helper.ExecuteNonQuery("sp_QL_DaXemDonHang", pars) != 0)
            {
                // update thanh cong
                return true;
            }
            else
            {
                // update that bai
                return false;
            }
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return false;
        }
    }

    public ChiTietDonHang GetChiTietDonHangFromDataRow(DataRow dr)
    {
        try
        {
            ChiTietDonHang ctdh = new ChiTietDonHang();

            ctdh.ID_ChiTietDonHang = int.Parse(dr["ID_ChiTietDonHang"].ToString());
            ctdh.ID_DonHang = int.Parse(dr["ID_DonHang"].ToString());
            ctdh.ID_HangHoa = int.Parse(dr["ID_HangHoa"].ToString());
            ctdh.TenHang = dr["TenHang"].ToString();
            ctdh.MaHang = dr["MaHang"].ToString();
            ctdh.GhiChu = dr["GhiChu"].ToString();
            ctdh.SoLuong = double.Parse(dr["SoLuong"].ToString());
            ctdh.DaGiao = double.Parse(dr["DaGiao"].ToString());
            ctdh.TenDonVi = dr["TenDonVi"].ToString();
            ctdh.HinhThucBan = int.Parse(dr["HinhThucBan"].ToString());
            ctdh.GiaBan =  double.Parse(dr["GiaBan"].ToString()) ;
            double giaBanBuon = dr["GiaBanBuon"].ToString() != "" ? double.Parse(dr["GiaBanBuon"].ToString()) : 0;
            double giaBanLe = dr["GiaBanLe"].ToString() != "" ?  double.Parse(dr["GiaBanLe"].ToString()) : 0;
            double giaKhac = dr["GiaKhac"].ToString() != "" ? double.Parse(dr["GiaKhac"].ToString()) : 0;

            ctdh.TongTienChietKhau =  double.Parse(dr["TongTienChietKhau"] != DBNull.Value ? dr["TongTienChietKhau"].ToString() : "0");
            ctdh.ChietKhauPhanTram =  double.Parse(dr["ChietKhauPhanTram"] != DBNull.Value ? dr["ChietKhauPhanTram"].ToString() : "0");
            ctdh.ChietKhauTien = double.Parse(dr["ChietKhauTien"] != DBNull.Value ? dr["ChietKhauTien"].ToString() : "0");

            //ctdh.TongTien = (ctdh.HinhThucBan == 1) ? (UInt32)(ctdh.SoLuong * giaBanBuon) : (ctdh.HinhThucBan == 2) ? (UInt32)(ctdh.SoLuong * giaKhac) : (UInt32)(ctdh.SoLuong * giaBanLe);
            ctdh.TongTien = (double)ctdh.SoLuong * ctdh.GiaBan;

            ctdh.DaThanhToan = double.Parse(dr["DaThanhToan"].ToString());
            ctdh.ID_CTKM = dr["ID_CTKM"] != DBNull.Value && dr["ID_CTKM"].ToString() != "" ? int.Parse(dr["ID_CTKM"].ToString()) : 0;
            ctdh.PhanTramHaoHut = double.Parse(dr["PhanTramHaoHut"] != DBNull.Value ? dr["PhanTramHaoHut"].ToString() : "0");
            ctdh.SoLuongHaoHut = double.Parse(dr["SoLuongHaoHut"] != DBNull.Value ? dr["SoLuongHaoHut"].ToString() : "0");

            try
            {
                ctdh.TenKho = dr["TenKho"].ToString();
            }
            catch (Exception)
            {

                 
            }
            return ctdh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public List<ChiTietDonHang> GetChiTietDonHang(int ID_DonHang)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_DonHang", ID_DonHang)
        };

        DataSet ds = helper.ExecuteDataSet("selectChiTietDonHang", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<ChiTietDonHang> dsdh = new List<ChiTietDonHang>();
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                i++;
                ChiTietDonHang ctdh = GetChiTietDonHangFromDataRow(dr);
                ctdh.STT = i;
                ctdh.STT2 = i;
                dsdh.Add(ctdh);
            }

            return dsdh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public DataTable GetNhanVienDaCapQuyen(int ID_DonHang)
    {
        DataTable dt = new DataTable();


        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_DonHang", ID_DonHang)
        };

        DataSet ds = helper.ExecuteDataSet("sp_DongHang_DanhSachNhanVien_DaPhanCong", pars);
        dt = ds.Tables[0];
        return dt;

    }

    public int PhanQuyenNhanVien(int ID_DonHang, int ID_NhanVien, string Quyen, int ID_QuanLy)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("ID_DonHang", ID_DonHang),
            new SqlParameter("Quyen", Quyen),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
        };
        try
        {
            return helper.ExecuteNonQuery("sp_DonHang_PhanQuyenNhanVien", pars);
        }
        catch
        {
            return 0;
        }
    }

    public List<DonHang> GetDSDonHangAll_TaiDiem(int ID_NhanVien, DateTime from, DateTime to)
    {
        SqlParameter[] pars = new SqlParameter[] {
           
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("TuNgay", from),
            new SqlParameter("DenNgay", to),   
        };

        DataSet ds = helper.ExecuteDataSet("sp_DonHang_GetAllDonHang_TaiDiem", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<DonHang> dsdh = new List<DonHang>();
            foreach (DataRow dr in dt.Rows)
            {
                DonHang dh = GetDonHangFromDataRow(dr);
                dsdh.Add(dh);
            }
            return dsdh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }
    public List<DonHang> GetDSDonHangAll_KhongTaiDiem(int ID_NhanVien, DateTime from, DateTime to)
    {
        SqlParameter[] pars = new SqlParameter[] {
           
            new SqlParameter("ID_NhanVien", ID_NhanVien),
            new SqlParameter("TuNgay", from),
            new SqlParameter("DenNgay", to),   
        };

        DataSet ds = helper.ExecuteDataSet("sp_DonHang_GetAllDonHang_KhongTaiDiem", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<DonHang> dsdh = new List<DonHang>();
            foreach (DataRow dr in dt.Rows)
            {
                DonHang dh = GetDonHangFromDataRow(dr);
                dsdh.Add(dh);
            }
            return dsdh;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }


    public int ThemMoi_PhieuTraHang(int ID_DonHang,int ID_QuanLy,DateTime NgayTraHang, string GhiChu)
    {
        int ID = 0;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("ID_DonHang", ID_DonHang),
              new SqlParameter("NgayTraHang", NgayTraHang),
              new SqlParameter("GhiChu", GhiChu)
        };
            object obj = helper.ExecuteScalar("sp_LichSuTraHang_ThemMoi", pars);
            if (obj != null)
            {
                ID = Convert.ToInt32(obj);
                // update thanh cong
                
            }
            
        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return ID;
    }

    public int Xoa_PhieuTraHang(int ID_LichSuTraHang)
    {
        int ID = 0;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_LichSuTraHang", ID_LichSuTraHang),
           
        };
            object obj = helper.ExecuteScalar("sp_LichSuTraHang_Xoa", pars);
            if (obj != null)
            {
                ID = Convert.ToInt32(obj);
                // update thanh cong

            }

        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return ID;
    }

    public int ThemMoi_PhieuTraHang_ChiTiet(int ID_Hang, int ID_LichSuTraHang, double SoLuong, double GiaBan,double TongTien, int ID_DonHang,int ID_LichSuGiaoHang, int ID_Kho, int ID_HinhThucBan)
    {
        int ID = 0;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_LichSuTraHang", ID_LichSuTraHang),
            new SqlParameter("ID_Hang", ID_Hang),
              new SqlParameter("SoLuong", SoLuong),
              new SqlParameter("GiaBan", GiaBan),
               new SqlParameter("TongTien", TongTien),
                   new SqlParameter("ID_LichSuGiaoHang", ID_LichSuGiaoHang),
                   new SqlParameter("ID_Kho", ID_Kho),
                    new SqlParameter("ID_HinhThucBan", ID_HinhThucBan),
        };
            ID = helper.ExecuteNonQuery("sp_ChiTietHangTra_ThemMoi", pars);

        }
        catch (Exception ex)
        {
            log.Error(ex);

        }
        return ID;
    }

    public DataTable GetDonHang_ChiTiet(int ID_QLLH, int ID_QuanLy, DateTime from, DateTime to, int trangthaigiao, int trangthaithanhtoan, int trangthaihoanthanh, int ID_KhachHang, int ID_NhanVien, int ID_HangHoa)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {

            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_QuanLy", ID_QuanLy),
            new SqlParameter("from", from.ToString("yyyy-MM-dd")),
            new SqlParameter("to", to),
            new SqlParameter("trangthaigiao", trangthaigiao),
            new SqlParameter("trangthaithanhtoan", trangthaithanhtoan),
            new SqlParameter("trangthaihoanthanh",trangthaihoanthanh)  ,
            new SqlParameter("ID_KhachHang", ID_KhachHang),
            new SqlParameter("ID_NhanVien", ID_NhanVien),
             new SqlParameter("ID_HangHoa", ID_HangHoa)
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllDonHang_ChiTet", pars);
            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }



        
        return dt;
    }
    public DataTable GetLichSuTraHang(int ID_DonHang)
    {
        DataTable dt = new DataTable();


        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_DonHang", ID_DonHang)
        };

        DataSet ds = helper.ExecuteDataSet("sp_LichSuTraHang_GetBy_ID_DonHang", pars);
        dt = ds.Tables[0];
        return dt;

    }
    public DataTable GetChiTietHangTra_ById(int ID_LichSuTraHang)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {

            new SqlParameter("ID_LichSuTraHang", ID_LichSuTraHang) 
        };

            DataSet ds = helper.ExecuteDataSet("sp_ChiTietHangTra_GetBy_ID_LichSuTraHang", pars);
            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {
            log.Error(ex);
        }




        return dt;
    }
}