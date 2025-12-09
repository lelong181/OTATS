using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using log4net;
using System.Data;

/// <summary>
/// Summary description for DonHangDB
/// </summary>
public class CTKMDB
{
    private static ILog log = LogManager.GetLogger(typeof(CTKMDB));
    public static SqlDataHelper db = new SqlDataHelper();

    public CTKMDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static List<CTKM_OBJ> LayTatCaCTKM(int idconty, DateTime ApDungTuNgay, DateTime ApDungDenNgay)
    {
        List<CTKM_OBJ> lst = new List<CTKM_OBJ>();
        try
        {
          
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idcongty", idconty),
                new SqlParameter("@TuNgay", ApDungTuNgay),
                new SqlParameter("@DenNgay", ApDungDenNgay),
                new SqlParameter("@trangthai", -1)
            };

            DataTable dt = db.ExecuteDataSet("getDSCTKMTheoIDCongTy", par).Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    CTKM_OBJ rs = new CTKM_OBJ();
                    rs.idctkm = int.Parse(dr["ID_CTKM"].ToString());
                    rs.idct = int.Parse(dr["ID_QLLH"].ToString());
                    rs.idnhanvien = (dr["ID_NhanVien"].ToString() != "") ? int.Parse(dr["ID_NhanVien"].ToString()) : 0;
                    rs.idquanly = (dr["ID_QuanLy"].ToString() != "") ? int.Parse(dr["ID_QuanLy"].ToString()) : 0;
                    rs.loai = (dr["Loai"].ToString() != "") ? int.Parse(dr["Loai"].ToString()) : 0;
                    rs.chietkhauphantram = (dr["ChietKhauPhanTram"].ToString() != "") ? double.Parse(dr["ChietKhauPhanTram"].ToString()) : 0;
                    rs.chietkhautien = (dr["ChietKhauTien"].ToString() != "") ? double.Parse(dr["ChietKhauTien"].ToString()) : 0;

                    rs.ngayapdung = (dr["NgayApDung"].ToString() != "") ? Convert.ToDateTime(dr["NgayApDung"].ToString()) : rs.ngayapdung;


                    rs.ngayketthuc = (dr["NgayKetThuc"].ToString() != "") ? Convert.ToDateTime(dr["NgayKetThuc"].ToString()) : rs.ngayketthuc;
                    rs.ngaytao = (dr["NgayTao"].ToString() != "") ? Convert.ToDateTime(dr["NgayTao"].ToString()) : rs.ngaytao; Convert.ToDateTime(dr["NgayTao"].ToString());
                    rs.ghichu = dr["GhiChu"].ToString();
                    rs.tenctkm = dr["TenCTKM"].ToString();
                    rs.trangthai = (dr["trangthai"].ToString() != "") ? int.Parse(dr["trangthai"].ToString()) : 0;
                    try
                    {
                        if (rs.ngayketthuc.Year != 1900 && (new DateTime(rs.ngayketthuc.Year, rs.ngayketthuc.Month, rs.ngayketthuc.Day) < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)))
                        {
                            rs.hethan = 1;
                            rs.trangthai = 0;
                        }
                        else
                        {
                            rs.hethan = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                        rs.hethan = 0;
                    }

                    rs.chitietkhuyenmai = new List<ChiTietCTKMOBJ>();
                    SqlParameter[] parChitiet = new SqlParameter[]{
                        new SqlParameter("@idctkm", rs.idctkm)

                    };
                    DataTable dtChitiet = db.ExecuteDataSet("getDS_ChitietCTKM_TheoIDCTKM", parChitiet).Tables[0];
                    foreach (DataRow drChiTiet in dtChitiet.Rows)
                    {
                        ChiTietCTKMOBJ ct = new ChiTietCTKMOBJ();
                        ct.id = int.Parse(drChiTiet["ID"].ToString());
                        ct.idctkm = int.Parse(drChiTiet["ID_CTKM"].ToString());
                        ct.idhang = int.Parse(drChiTiet["ID_Hang"].ToString());
                        ct.chietkhauphantram_banbuon = (drChiTiet["ChietKhauPhanTram_BanBuon"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauPhanTram_BanBuon"].ToString())) : 0;
                        ct.chietkhautien_banbuon = (drChiTiet["ChietKhauTien_BanBuon"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauTien_BanBuon"].ToString())) : 0;
                        ct.chietkhauphantram_banle = (drChiTiet["ChietKhauPhanTram_BanLe"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauPhanTram_BanLe"].ToString())) : 0;
                        ct.chietkhautien_banle = (drChiTiet["ChietKhauTien_BanLe"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauTien_BanLe"].ToString())) : 0;
                        ct.ghichu = drChiTiet["GhiChu"].ToString();

                        ct.donvi = drChiTiet["TenDonVi"].ToString();
                        ct.tenhang = drChiTiet["TenHang"].ToString();
                        ct.mahang = drChiTiet["MaHang"].ToString();
                        ct.giabuon = drChiTiet["GiaBanBuon"].ToString() != "" ? double.Parse(drChiTiet["GiaBanBuon"].ToString()).ToString("N2") : (-1).ToString("N2");
                        ct.giale = drChiTiet["GiaBanLe"].ToString() != "" ? double.Parse(drChiTiet["GiaBanLe"].ToString()).ToString("N2") : (-1).ToString("N2");

                        rs.chitietkhuyenmai.Add(ct);
                    }
                    lst.Add(rs);
                    //cac chuong trinh het han thi ko show ra
                    //if (rs.ngayketthuc.Year == 1900 || (new DateTime(rs.ngayketthuc.Year, rs.ngayketthuc.Month, rs.ngayketthuc.Day) >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-7)))
                    //{
                    //    
                    //}

                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }


            return lst;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return lst;
        }


    }

    public static List<CTKM_OBJ> LayCTKMDangHoatDong(int idconty, DateTime ApDungTuNgay, DateTime ApDungDenNgay)
    {
        List<CTKM_OBJ> lst = new List<CTKM_OBJ>();
        try
        {
            
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idcongty", idconty),
                new SqlParameter("@TuNgay", ApDungTuNgay),
                new SqlParameter("@DenNgay", ApDungDenNgay),
                new SqlParameter("@trangthai", 1)
            };

            DataTable dt = db.ExecuteDataSet("getDSCTKMTheoIDCongTy", par).Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    CTKM_OBJ rs = new CTKM_OBJ();
                    rs.idctkm = int.Parse(dr["ID_CTKM"].ToString());
                    rs.idct = int.Parse(dr["ID_QLLH"].ToString());
                    rs.idnhanvien = (dr["ID_NhanVien"].ToString() != "") ? int.Parse(dr["ID_NhanVien"].ToString()) : 0;
                    rs.idquanly = (dr["ID_QuanLy"].ToString() != "") ? int.Parse(dr["ID_QuanLy"].ToString()) : 0;
                    rs.loai =   (dr["Loai"].ToString() != "") ? int.Parse(dr["Loai"].ToString()) : 0;
                    rs.chietkhauphantram  = (dr["ChietKhauPhanTram"].ToString() != "") ? double.Parse(dr["ChietKhauPhanTram"].ToString()) : 0;
                    rs.chietkhautien  = (dr["ChietKhauTien"].ToString() != "") ? double.Parse(dr["ChietKhauTien"].ToString()) : 0;
                   
                    rs.ngayapdung = (dr["NgayApDung"].ToString() != "") ? Convert.ToDateTime(dr["NgayApDung"].ToString()) : rs.ngayapdung;

                  
                    rs.ngayketthuc = (dr["NgayKetThuc"].ToString() != "") ? Convert.ToDateTime(dr["NgayKetThuc"].ToString()) : rs.ngayketthuc;
                    rs.ngaytao = (dr["NgayTao"].ToString() != "") ? Convert.ToDateTime(dr["NgayTao"].ToString()) : rs.ngaytao; Convert.ToDateTime(dr["NgayTao"].ToString());
                    rs.ghichu = dr["GhiChu"].ToString();
                    rs.tenctkm = dr["TenCTKM"].ToString();
                    rs.trangthai = (dr["trangthai"].ToString() != "") ? int.Parse(dr["trangthai"].ToString()) : 0;
                    try
                    {
                        if (rs.ngayketthuc.Year != 1900  && (new DateTime(rs.ngayketthuc.Year, rs.ngayketthuc.Month, rs.ngayketthuc.Day) < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)))
                        {
                            rs.hethan = 1;
                            rs.trangthai = 0;
                        }
                        else
                        {
                            rs.hethan = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                        rs.hethan = 0;
                    }

                    rs.chitietkhuyenmai = new List<ChiTietCTKMOBJ>();
                    SqlParameter[] parChitiet = new SqlParameter[]{
                        new SqlParameter("@idctkm", rs.idctkm)
              
                    };
                    DataTable dtChitiet = db.ExecuteDataSet("getDS_ChitietCTKM_TheoIDCTKM", parChitiet).Tables[0];
                    foreach (DataRow drChiTiet in dtChitiet.Rows)
                    {
                        ChiTietCTKMOBJ ct = new ChiTietCTKMOBJ();
                        ct.id = int.Parse(drChiTiet["ID"].ToString());
                        ct.idctkm = int.Parse(drChiTiet["ID_CTKM"].ToString());
                        ct.idhang = int.Parse(drChiTiet["ID_Hang"].ToString());
                        ct.chietkhauphantram_banbuon = (drChiTiet["ChietKhauPhanTram_BanBuon"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauPhanTram_BanBuon"].ToString())) : 0;
                        ct.chietkhautien_banbuon = (drChiTiet["ChietKhauTien_BanBuon"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauTien_BanBuon"].ToString())) : 0;
                        ct.chietkhauphantram_banle = (drChiTiet["ChietKhauPhanTram_BanLe"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauPhanTram_BanLe"].ToString())) : 0;
                        ct.chietkhautien_banle = (drChiTiet["ChietKhauTien_BanLe"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauTien_BanLe"].ToString())) : 0;
                        ct.ghichu = drChiTiet["GhiChu"].ToString();

                        ct.donvi = drChiTiet["TenDonVi"].ToString();
                        ct.tenhang = drChiTiet["TenHang"].ToString();
                        ct.mahang = drChiTiet["MaHang"].ToString();
                        ct.giabuon = drChiTiet["GiaBanBuon"].ToString() != "" ? double.Parse(drChiTiet["GiaBanBuon"].ToString()).ToString("N2") : (-1).ToString("N2");
                        ct.giale = drChiTiet["GiaBanLe"].ToString() != "" ? double.Parse(drChiTiet["GiaBanLe"].ToString()).ToString("N2") : (-1).ToString("N2");

                        rs.chitietkhuyenmai.Add(ct);
                    }
                    //cac chuong trinh het han thi ko show ra
                    if (rs.ngayketthuc.Year == 1900 ||  (new DateTime(rs.ngayketthuc.Year, rs.ngayketthuc.Month, rs.ngayketthuc.Day) >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-7)))
                    {
                        lst.Add(rs);
                    }

                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }


            return lst;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return lst;
        }


    }
    public static CTKM_OBJ LayCTKM_ByIDCTKM(int idctkm)
    {
        CTKM_OBJ rs = new CTKM_OBJ();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idctkm", idctkm) 
              
            };


            DataTable dt = db.ExecuteDataSet("getDSCTKMTheoIDCTKM", par).Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                   
                    rs.idctkm = int.Parse(dr["ID_CTKM"].ToString());
                    rs.idct = int.Parse(dr["ID_QLLH"].ToString());
                    rs.idnhanvien = (dr["ID_NhanVien"].ToString() != "") ? int.Parse(dr["ID_NhanVien"].ToString()) : 0;
                    rs.idquanly = (dr["ID_QuanLy"].ToString() != "") ? int.Parse(dr["ID_QuanLy"].ToString()) : 0;
                    rs.loai = (dr["Loai"].ToString() != "") ? int.Parse(dr["Loai"].ToString()) : 0;
                    rs.chietkhauphantram = (dr["ChietKhauPhanTram"].ToString() != "") ? double.Parse(dr["ChietKhauPhanTram"].ToString()) : 0;
                    rs.chietkhautien = (dr["ChietKhauTien"].ToString() != "") ? double.Parse(dr["ChietKhauTien"].ToString()) : 0;
                    try
                    {
                        if (rs.ngayketthuc.Year != 1900 && ( new DateTime(rs.ngayketthuc.Year, rs.ngayketthuc.Month, rs.ngayketthuc.Day) < new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) ))
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
                        LSPos_Data.Utilities.Log.Error(ex);
                        rs.hethan = 0;
                    }
                    rs.ngayapdung = (dr["NgayApDung"].ToString() != "") ? Convert.ToDateTime(dr["NgayApDung"].ToString()) : rs.ngayapdung;
                    rs.ngayketthuc = (dr["NgayKetThuc"].ToString() != "") ? Convert.ToDateTime(dr["NgayKetThuc"].ToString()) : rs.ngayketthuc;
                    rs.ngaytao = (dr["NgayTao"].ToString() != "") ? Convert.ToDateTime(dr["NgayTao"].ToString()) : rs.ngaytao; Convert.ToDateTime(dr["NgayTao"].ToString());
                    rs.ghichu = dr["GhiChu"].ToString();
                    rs.tenctkm = dr["TenCTKM"].ToString();
                    rs.trangthai = (dr["trangthai"].ToString() != "") ? int.Parse(dr["trangthai"].ToString()) : 0;
                    rs.chitietkhuyenmai = new List<ChiTietCTKMOBJ>();
                    SqlParameter[] parChitiet = new SqlParameter[]{
                        new SqlParameter("@idctkm", rs.idctkm)
              
                    };
                    DataTable dtChitiet = db.ExecuteDataSet("getDS_ChitietCTKM_TheoIDCTKM", parChitiet).Tables[0];
                    foreach (DataRow drChiTiet in dtChitiet.Rows)
                    {
                        ChiTietCTKMOBJ ct = new ChiTietCTKMOBJ();
                        ct.id = int.Parse(drChiTiet["ID"].ToString());
                        ct.idctkm = int.Parse(drChiTiet["ID_CTKM"].ToString());
                        ct.idhang = int.Parse(drChiTiet["ID_Hang"].ToString());
                        ct.chietkhauphantram_banbuon = (drChiTiet["ChietKhauPhanTram_BanBuon"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauPhanTram_BanBuon"].ToString())) : 0;
                        ct.chietkhautien_banbuon = (drChiTiet["ChietKhauTien_BanBuon"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauTien_BanBuon"].ToString())) : 0;
                        ct.chietkhauphantram_banle = (drChiTiet["ChietKhauPhanTram_BanLe"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauPhanTram_BanLe"].ToString())) : 0;
                        ct.chietkhautien_banle = (drChiTiet["ChietKhauTien_BanLe"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauTien_BanLe"].ToString())) : 0;

                        ct.ghichu = drChiTiet["GhiChu"].ToString();

                        ct.donvi = drChiTiet["TenDonVi"].ToString();
                        ct.tenhang = drChiTiet["TenHang"].ToString();
                        ct.mahang = drChiTiet["MaHang"].ToString();
                        ct.giabuon = drChiTiet["GiaBanBuon"].ToString() != "" ? double.Parse(drChiTiet["GiaBanBuon"].ToString()).ToString("N2") : (-1).ToString("N2");
                        ct.giale = drChiTiet["GiaBanLe"].ToString() != "" ? double.Parse(drChiTiet["GiaBanLe"].ToString()).ToString("N2") : (-1).ToString("N2");


                        rs.chitietkhuyenmai.Add(ct);
                    }
                    

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

    public static List<ChiTietCTKMMatHang_OBJ> LayCTKM_TheoMatHang(int idconty,int idmathang, DateTime ApDungTuNgay)
    {
        List<ChiTietCTKMMatHang_OBJ> rs = new List<ChiTietCTKMMatHang_OBJ>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idcongty", idconty),
                new SqlParameter("@idmathang", idmathang),
            };

            try
            {
                     
                DataTable dtChitiet = db.ExecuteDataSet("getDS_ChitietCTKM_TheoIDCTKM_IDMatHang", par).Tables[0];
                foreach (DataRow drChiTiet in dtChitiet.Rows)
                {
                    ChiTietCTKMMatHang_OBJ ct = new ChiTietCTKMMatHang_OBJ();
                    ct.id = int.Parse(drChiTiet["ID"].ToString());
                    ct.idctkm = int.Parse(drChiTiet["ID_CTKM"].ToString());
                    ct.idhang = int.Parse(drChiTiet["ID_Hang"].ToString());
                    ct.chietkhauphantram_banbuon = (drChiTiet["ChietKhauPhanTram_BanBuon"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauPhanTram_BanBuon"].ToString())) : 0;
                    ct.chietkhautien_banbuon = (drChiTiet["ChietKhauTien_BanBuon"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauTien_BanBuon"].ToString())) : 0;
                    ct.chietkhauphantram_banle = (drChiTiet["ChietKhauPhanTram_BanLe"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauPhanTram_BanLe"].ToString())) : 0;
                    ct.chietkhautien_banle = (drChiTiet["ChietKhauTien_BanLe"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauTien_BanLe"].ToString())) : 0; 
                     
                    ct.ghichu = drChiTiet["GhiChu"].ToString();

                    ct.ngayapdung = (drChiTiet["NgayApDung"].ToString() != "") ? Convert.ToDateTime(drChiTiet["NgayApDung"].ToString()) : ct.ngayapdung;
                    ct.ngayketthuc = (drChiTiet["NgayKetThuc"].ToString() != "") ? Convert.ToDateTime(drChiTiet["NgayKetThuc"].ToString()) : ct.ngayketthuc;
                    ct.ngaytao = (drChiTiet["NgayTao"].ToString() != "") ? Convert.ToDateTime(drChiTiet["NgayTao"].ToString()) : ct.ngaytao; Convert.ToDateTime(drChiTiet["NgayTao"].ToString());
                    ct.loai = (drChiTiet["Loai"].ToString() != "") ? int.Parse(drChiTiet["Loai"].ToString()) : 0;
                    ct.tenctkm = drChiTiet["TenCTKM"].ToString();
                    ct.trangthai = (drChiTiet["trangthai"].ToString() != "") ? int.Parse(drChiTiet["trangthai"].ToString()) : 0;

                    ct.donvi = drChiTiet["TenDonVi"].ToString();
                    ct.tenhang = drChiTiet["TenHang"].ToString();
                    ct.mahang = drChiTiet["MaHang"].ToString();
                    ct.giabuon = drChiTiet["GiaBanBuon"].ToString() != "" ? double.Parse(drChiTiet["GiaBanBuon"].ToString()).ToString("N2") : (-1).ToString("N2");
                    ct.giale = drChiTiet["GiaBanLe"].ToString() != "" ? double.Parse(drChiTiet["GiaBanLe"].ToString()).ToString("N2") : (-1).ToString("N2");
                    if (ct.ngayketthuc.Year == 1900  ||  (new DateTime(ct.ngayketthuc.Year, ct.ngayketthuc.Month, ct.ngayketthuc.Day) > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)))
                    {
                        rs.Add(ct);
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
             
            return rs;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return rs;
        }
    }
    public static List<ChiTietCTKMMatHang_OBJ> LayCTKM_TheoMatHang(int idconty, int idmathang,int ID_CTKM)
    {
        List<ChiTietCTKMMatHang_OBJ> rs = new List<ChiTietCTKMMatHang_OBJ>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idcongty", idconty),
                new SqlParameter("@idmathang", idmathang),
                new SqlParameter("@idCTKM", ID_CTKM),
            };

            try
            {
                DataTable dtChitiet = db.ExecuteDataSet("getDS_ChitietCTKM_TheoIDCTKM_IDMatHang_v2", par).Tables[0];
                foreach (DataRow drChiTiet in dtChitiet.Rows)
                {
                    ChiTietCTKMMatHang_OBJ ct = new ChiTietCTKMMatHang_OBJ();
                    ct.id = int.Parse(drChiTiet["ID"].ToString());
                    ct.idctkm = int.Parse(drChiTiet["ID_CTKM"].ToString());
                    ct.idhang = int.Parse(drChiTiet["ID_Hang"].ToString());
                    ct.chietkhauphantram_banbuon = (drChiTiet["ChietKhauPhanTram_BanBuon"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauPhanTram_BanBuon"].ToString())) : 0;
                    ct.chietkhautien_banbuon = (drChiTiet["ChietKhauTien_BanBuon"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauTien_BanBuon"].ToString())) : 0;
                    ct.chietkhauphantram_banle = (drChiTiet["ChietKhauPhanTram_BanLe"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauPhanTram_BanLe"].ToString())) : 0;
                    ct.chietkhautien_banle = (drChiTiet["ChietKhauTien_BanLe"].ToString() != "") ? (double.Parse(drChiTiet["ChietKhauTien_BanLe"].ToString())) : 0;

                    ct.ghichu = drChiTiet["GhiChu"].ToString();

                    ct.ngayapdung = (drChiTiet["NgayApDung"].ToString() != "") ? Convert.ToDateTime(drChiTiet["NgayApDung"].ToString()) : ct.ngayapdung;
                    ct.ngayketthuc = (drChiTiet["NgayKetThuc"].ToString() != "") ? Convert.ToDateTime(drChiTiet["NgayKetThuc"].ToString()) : ct.ngayketthuc;
                    ct.ngaytao = (drChiTiet["NgayTao"].ToString() != "") ? Convert.ToDateTime(drChiTiet["NgayTao"].ToString()) : ct.ngaytao; Convert.ToDateTime(drChiTiet["NgayTao"].ToString());
                    ct.loai = (drChiTiet["Loai"].ToString() != "") ? int.Parse(drChiTiet["Loai"].ToString()) : 0;
                    ct.tenctkm = drChiTiet["TenCTKM"].ToString();
                    ct.trangthai = (drChiTiet["trangthai"].ToString() != "") ? int.Parse(drChiTiet["trangthai"].ToString()) : 0;
                     
                    ct.donvi = drChiTiet["TenDonVi"].ToString();
                    ct.tenhang = drChiTiet["TenHang"].ToString();
                    ct.mahang = drChiTiet["MaHang"].ToString();
                    ct.giabuon = drChiTiet["GiaBanBuon"].ToString() != "" ? double.Parse(drChiTiet["GiaBanBuon"].ToString()).ToString("N2") : (-1).ToString("N2");
                    ct.giale = drChiTiet["GiaBanLe"].ToString() != "" ? double.Parse(drChiTiet["GiaBanLe"].ToString()).ToString("N2") : (-1).ToString("N2");

                    ct.SoLuongDatKM_Tu = (!string.IsNullOrEmpty(drChiTiet["SoLuongDatKM_Tu"].ToString())) ? double.Parse(drChiTiet["SoLuongDatKM_Tu"].ToString()) : 0;
                    ct.SoLuongDatKM_Den = (!string.IsNullOrEmpty(drChiTiet["SoLuongDatKM_Den"].ToString())) ? double.Parse(drChiTiet["SoLuongDatKM_Den"].ToString()) : 0;
                    ct.TongTienDatKM_Den = (!string.IsNullOrEmpty(drChiTiet["TongTienDatKM_Den"].ToString())) ? double.Parse(drChiTiet["TongTienDatKM_Den"].ToString()) : 0;
                    ct.TongTienDatKM_Tu = (!string.IsNullOrEmpty(drChiTiet["TongTienDatKM_Tu"].ToString())) ? double.Parse(drChiTiet["TongTienDatKM_Tu"].ToString()) : 0;
                    ct.ApDungBoiSo = (!string.IsNullOrEmpty(drChiTiet["ApDungBoiSo"].ToString())) ? int.Parse(drChiTiet["ApDungBoiSo"].ToString()) : 0;

                    if (ct.ngayketthuc.Year == 1900 || (new DateTime(ct.ngayketthuc.Year, ct.ngayketthuc.Month, ct.ngayketthuc.Day) > new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)))
                    {
                        rs.Add(ct);
                    }

                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return rs;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return rs;
        }
    }
    public static DataTable GetChiTietHangTang(int ID_CTKM)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_CTKM", ID_CTKM)
        };

            DataSet ds = db.ExecuteDataSet("sp_CTKM_GetChiTietHangTang", pars);
            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {

            log.Error(ex);
        }
        return dt;
    }


}
 