using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using log4net;

/// <summary>
/// Summary description for KhuyenMai_dl
/// </summary>
public class KhuyenMai_dl
{
    private SqlDataHelper helper;

    public KhuyenMai_dl()
    {
        helper = new SqlDataHelper();
    }
    ILog log = LogManager.GetLogger(typeof(KhuyenMai_dl));
    public KhuyenMai GetKhuyenMaiFromDataRow(DataRow dr)
    {
        try
        {
            KhuyenMai km = new KhuyenMai();
            km.ID_CTKM = int.Parse(dr["ID_CTKM"].ToString());
            km.ID_QLLH = int.Parse(dr["ID_QLLH"].ToString());
            km.ID_NhanVien = (dr["ID_NhanVien"].ToString() != "") ? int.Parse(dr["ID_NhanVien"].ToString()) : 0;
            km.ID_QuanLy = (dr["ID_QuanLy"].ToString() != "") ? int.Parse(dr["ID_QuanLy"].ToString()) : 0;
            km.TenCTKM = dr["TenCTKM"].ToString();

            km.Loai = (dr["Loai"].ToString() != "") ? int.Parse(dr["Loai"].ToString()) : 0;
            km.ChietKhauPhanTram = (dr["ChietKhauPhanTram"].ToString() != "") ? float.Parse(dr["ChietKhauPhanTram"].ToString()) : 0;
            km.ChietKhauTien = (dr["ChietKhauTien"].ToString() != "") ? double.Parse(dr["ChietKhauTien"].ToString()) : 0;

            km.NgayApDung = (dr["NgayApDung"].ToString() != "") ? Convert.ToDateTime(dr["NgayApDung"].ToString()) : km.NgayApDung;
            km.NgayKetThuc = (dr["NgayKetThuc"].ToString() != "") ? Convert.ToDateTime(dr["NgayKetThuc"].ToString()) : km.NgayKetThuc;
            km.NgayTao = (dr["NgayTao"].ToString() != "") ? Convert.ToDateTime(dr["NgayTao"].ToString()) : km.NgayTao;
            km.TrangThai = int.Parse(dr["TrangThai"].ToString());
            km.GhiChu = dr["GhiChu"].ToString();
            km.AnhDaiDien = dr["AnhDaiDien"].ToString();
            km.ChiTietCTKM = ChiTietKhuyenMai_dl.GetChiTietKhuyenMai(km.ID_QLLH, km.ID_CTKM);
            //km.DanhSachAnh = ImageDB.GetAlbumById(dr["ID_Album"].ToString() != "" ? int.Parse(dr["ID_Album"].ToString()) : 0);
            km.TongTienDatKM_Tu = (dr["TongTienDatKM_Tu"].ToString() != "") ? double.Parse(dr["TongTienDatKM_Tu"].ToString()) : 0;
            km.TongTienDatKM_Den = (dr["TongTienDatKM_Den"].ToString() != "") ? double.Parse(dr["TongTienDatKM_Den"].ToString()) : 0;

            try
            {
                km.TenHinhThucKM = dr["TenHinhThucKM"].ToString();

            }
            catch (Exception)
            {


            }
            return km;
        }
        catch
        {
            return null;
        }
    }



    public List<KhuyenMai> GetDSKhuyenmaiAll(int ID_QLLH)
    {
        //sửa parameter
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH)

        };

        //sửa tên stored
        DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllKhuyenMai", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<KhuyenMai> dskm = new List<KhuyenMai>();
            foreach (DataRow dr in dt.Rows)
            {
                KhuyenMai dh = GetKhuyenMaiFromDataRow(dr);
                dskm.Add(dh);
            }
            return dskm;
        }
        catch
        {
            return null;
        }
    }


    public List<KhuyenMai> GetDSKhuyenmaiConHieuLuc(int ID_QLLH)
    {
        //sửa parameter
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH)

        };

        //sửa tên stored
        DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllKhuyenMaiConHieuLuc", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<KhuyenMai> dskm = new List<KhuyenMai>();
            foreach (DataRow dr in dt.Rows)
            {
                KhuyenMai dh = GetKhuyenMaiFromDataRow(dr);
                dskm.Add(dh);
            }
            return dskm;
        }
        catch
        {
            return null;
        }
    }

    public List<KhuyenMai> GetDSKhuyenmaiConHieuLucTheoLoai(int ID_QLLH, int Loai)
    {
        List<KhuyenMai> dskm = new List<KhuyenMai>();
        //sửa parameter
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
             new SqlParameter("Loai", Loai)
        };

        //sửa tên stored
        DataSet ds = helper.ExecuteDataSet("sp_QL_GetAllKhuyenMaiConHieuLuc_v2", pars);
        DataTable dt = ds.Tables[0];




        try
        {

            foreach (DataRow dr in dt.Rows)
            {
                KhuyenMai dh = GetKhuyenMaiFromDataRow(dr);
                dskm.Add(dh);
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return dskm;
    }
    public List<ChiTietHangTangOBJ_v2> DanhSachChiTietHangTang_v2(int idchitietctkm)
    {
        List<ChiTietHangTangOBJ_v2> ds = new List<ChiTietHangTangOBJ_v2>();
        try
        {
            DataTable dt = helper.ExecuteDataSet("getDS_ChiTietHangTang", new SqlParameter("@idchitietctkm", idchitietctkm)).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                ChiTietHangTangOBJ_v2 obj = new ChiTietHangTangOBJ_v2();
                obj.idchitiethangtang = int.Parse(dr["ID_ChiTietKM_HangTang"].ToString());
                obj.idchitietctkm = int.Parse(dr["ID_CTKM_ChiTiet"].ToString());
                obj.idhanghoa = int.Parse(dr["ID_HangHoa"].ToString());
                obj.tenhang = dr["TenHang"].ToString();
                obj.mahang = dr["MaHang"].ToString();
                obj.soluong = int.Parse(dr["SoLuong"].ToString());
                obj.tongtien = double.Parse(dr["TongTien"].ToString());
                obj.giabanbuon = double.Parse(dr["GiaBanBuon"].ToString());
                obj.giabanle = double.Parse(dr["GiaBanLe"].ToString());

                ds.Add(obj);
            }

            return ds;
        }
        catch (Exception ex)
        {

            return ds;
        }
    }

    public List<ChiTietHangTangOBJ_v2> DanhSachChiTietHangTang_TheoCTKM(int ID_CTKM)
    {
        List<ChiTietHangTangOBJ_v2> ds = new List<ChiTietHangTangOBJ_v2>();
        try
        {
            DataTable dt = helper.ExecuteDataSet("getDS_ChiTietHangTangTheoCTKM", new SqlParameter("@ID_CTKM", ID_CTKM)).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                ChiTietHangTangOBJ_v2 obj = new ChiTietHangTangOBJ_v2();
                obj.idchitiethangtang = int.Parse(dr["ID_ChiTietKM_HangTang"].ToString());
                obj.idchitietctkm = int.Parse(dr["ID_CTKM_ChiTiet"].ToString());
                obj.idhanghoa = int.Parse(dr["ID_HangHoa"].ToString());
                obj.tenhang = dr["TenHang"].ToString();
                obj.mahang = dr["MaHang"].ToString();
                obj.soluong = int.Parse(dr["SoLuong"].ToString());
                obj.tongtien = double.Parse(dr["TongTien"].ToString());
                obj.giabanbuon = double.Parse(dr["GiaBanBuon"].ToString());
                obj.giabanle = double.Parse(dr["GiaBanLe"].ToString());

                ds.Add(obj);
            }

            return ds;
        }
        catch (Exception ex)
        {

            return ds;
        }
    }
    public List<ChiTietCTKMMatHangOBJ> LayCTKM_TheoMatHang(int idconty, int idmathang, DateTime ApDungTuNgay)
    {
        List<ChiTietCTKMMatHangOBJ> rs = new List<ChiTietCTKMMatHangOBJ>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idcongty", idconty),
                new SqlParameter("@idmathang", idmathang),

            };



            try
            {

                DataTable dtChitiet = helper.ExecuteDataSet("getDS_ChitietCTKM_TheoIDCTKM_IDMatHang_v2", par).Tables[0];
                foreach (DataRow drChiTiet in dtChitiet.Rows)
                {

                    ChiTietCTKMMatHangOBJ ct = new ChiTietCTKMMatHangOBJ();
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
                    //ct.anhdaidienctkm = (!string.IsNullOrEmpty(drChiTiet["AnhDaiDien"].ToString())) ? Configs.BASEURL + drChiTiet["AnhDaiDien"].ToString() : "";
                    ct.chitiethangtang = DanhSachChiTietHangTang_v2(ct.id);

                    ct.donvi = drChiTiet["TenDonVi"].ToString();
                    ct.tenhang = drChiTiet["TenHang"].ToString();
                    ct.mahang = drChiTiet["MaHang"].ToString();
                    ct.giabuon = drChiTiet["GiaBanBuon"].ToString() != "" ? double.Parse(drChiTiet["GiaBanBuon"].ToString()) : 0;
                    ct.giale = drChiTiet["GiaBanLe"].ToString() != "" ? double.Parse(drChiTiet["GiaBanLe"].ToString()) : 0;

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

            }



            return rs;
        }
        catch (Exception ex)
        {

            return rs;
        }


    }
    public KhuyenMai GetKhuyenMaiByID(int ID_CTKM)
    {
        KhuyenMai km = new KhuyenMai();
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_CTKM", ID_CTKM)
        };

        DataSet ds = helper.ExecuteDataSet("sp_QL_GetKhuyenMai_ById", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<ChiTietKhuyenMai> dsdh = new List<ChiTietKhuyenMai>();
            foreach (DataRow dr in dt.Rows)
            {
                km = GetKhuyenMaiFromDataRow(dr);

            }

            return km;
        }
        catch (Exception ex)
        {
            log.Error(ex);
            return null;
        }
    }

    public bool ThemKhuyenMai(KhuyenMai km)
    {
        bool ret = false;
        try
        {
            if (km.Loai == 1 && km.ChietKhauPhanTram < 0 || km.ChietKhauPhanTram > 100 || km.ChietKhauTien < 0)
            {
                return false;
            }

            SqlParameter output = new SqlParameter("@ID_CTKM", DbType.Int64);
            SqlParameter[] pars = new SqlParameter[] {

                    new SqlParameter("ID_QLLH", km.ID_QLLH),
                    new SqlParameter("TenCTKM", km.TenCTKM),
                    new SqlParameter("NgayApDung", km.NgayApDung),
                    new SqlParameter("NgayKetThuc", km.NgayKetThuc),
                    new SqlParameter("ID_NhanVien", km.ID_NhanVien),
                    new SqlParameter("ID_QuanLy", km.ID_QuanLy),
                    new SqlParameter("Loai", km.Loai),
                    new SqlParameter("ChietKhauPhanTram", km.ChietKhauPhanTram),
                    new SqlParameter("ChietKhauTien", km.ChietKhauTien),
                    new SqlParameter("GhiChu", km.GhiChu),
                     new SqlParameter("ChietKhauTien", km.ChietKhauTien),

                    new SqlParameter("TrangThai", 1),

                        new SqlParameter("TongTienDatKM_Den", km.TongTienDatKM_Den),
                          new SqlParameter("TongTienDatKM_Tu", km.TongTienDatKM_Tu),

                   output,
                };
            object ID = helper.ExecuteScalar("sp_CTKM_ThemMoi", pars);
            if (ID != null && int.Parse(ID.ToString()) > 0)
            {
                try
                {
                    //int ID_CTKM = int.Parse(output.SqlValue.ToString());

                    foreach (ChiTietKhuyenMai ct in km.ChiTietCTKM)
                    {
                        if (ct.ChietKhauPhanTram_BanBuon < 0 || ct.ChietKhauPhanTram_BanBuon > 100 || ct.ChietKhauPhanTram_BanLe < 0 || ct.ChietKhauPhanTram_BanLe > 100 || ct.ChietKhauTien_BanLe < 0 || ct.ChietKhauTien_BanBuon < 0)
                            break;
                        SqlParameter[] parsChiTiet = new SqlParameter[] {

                        new SqlParameter("ChietKhauPhanTram_BanBuon", ct.ChietKhauPhanTram_BanBuon),
                        new SqlParameter("ChietKhauPhanTram_BanLe", ct.ChietKhauPhanTram_BanLe),
                        new SqlParameter("ChietKhauTien_BanBuon", ct.ChietKhauTien_BanBuon),
                        new SqlParameter("ChietKhauTien_BanLe", ct.ChietKhauTien_BanLe),
                        new SqlParameter("GhiChu", ct.GhiChu != null ? ct.GhiChu : ""),
                        new SqlParameter("ID_CTKM", ID),
                        new SqlParameter("ID_Hang", ct.ID_Hang),
                         new SqlParameter("TongTienDatKM_Den", ct.TongTienDatKM_Den),
                          new SqlParameter("TongTienDatKM_Tu", ct.TongTienDatKM_Tu),
                           new SqlParameter("SoLuongDatKM_Den", ct.SoLuongDatKM_Den),
                            new SqlParameter("SoLuongDatKM_Tu", ct.SoLuongDatKM_Tu),
                             new SqlParameter("ApDungBoiSo", ct.ApDungBoiSo),

                    };

                        if (helper.ExecuteNonQuery("sp_CTKM_ChiTiet_ThemMoi", parsChiTiet) != 0)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);

                }
                // them thanh cong
                ret = true;
            }


        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return ret;
    }

    public int ThemKhuyenMai(KhuyenMai km, DataTable dtHangTang)
    {
        int ret = 0;
        try
        {
            if (km.Loai == 1 && km.ChietKhauPhanTram < 0 || km.ChietKhauPhanTram > 100 || km.ChietKhauTien < 0)
            {
                return ret;
            }

            SqlParameter output = new SqlParameter("@ID_CTKM", DbType.Int64);
            SqlParameter[] pars = new SqlParameter[] {

                    new SqlParameter("ID_QLLH", km.ID_QLLH),
                    new SqlParameter("TenCTKM", km.TenCTKM),
                    new SqlParameter("NgayApDung", km.NgayApDung),
                    new SqlParameter("NgayKetThuc", km.NgayKetThuc),
                    new SqlParameter("ID_NhanVien", km.ID_NhanVien),
                    new SqlParameter("ID_QuanLy", km.ID_QuanLy),
                    new SqlParameter("Loai", km.Loai),
                    new SqlParameter("ChietKhauPhanTram", km.ChietKhauPhanTram),
                    new SqlParameter("ChietKhauTien", km.ChietKhauTien),
                    new SqlParameter("GhiChu", km.GhiChu),
                    new SqlParameter("TrangThai", 1),
                        new SqlParameter("TongTienDatKM_Den", km.TongTienDatKM_Den),
                          new SqlParameter("TongTienDatKM_Tu", km.TongTienDatKM_Tu),
                   output,
                };
            object ID = helper.ExecuteScalar("sp_CTKM_ThemMoi", pars);
            if (ID != null && int.Parse(ID.ToString()) > 0)
            {
                ret = int.Parse(ID.ToString());
                try
                {
                    //int ID_CTKM = int.Parse(output.SqlValue.ToString());

                    foreach (ChiTietKhuyenMai ct in km.ChiTietCTKM)
                    {
                        if (ct.ChietKhauPhanTram_BanBuon < 0 || ct.ChietKhauPhanTram_BanBuon > 100 || ct.ChietKhauPhanTram_BanLe < 0 || ct.ChietKhauPhanTram_BanLe > 100 || ct.ChietKhauTien_BanLe < 0 || ct.ChietKhauTien_BanBuon < 0)
                            break;
                        SqlParameter[] parsChiTiet = new SqlParameter[] {

                        new SqlParameter("ChietKhauPhanTram_BanBuon", ct.ChietKhauPhanTram_BanBuon),
                        new SqlParameter("ChietKhauPhanTram_BanLe", ct.ChietKhauPhanTram_BanLe),
                        new SqlParameter("ChietKhauTien_BanBuon", ct.ChietKhauTien_BanBuon),
                        new SqlParameter("ChietKhauTien_BanLe", ct.ChietKhauTien_BanLe),
                        new SqlParameter("GhiChu", ct.GhiChu != null ? ct.GhiChu : ""),
                        new SqlParameter("ID_CTKM", ID),
                        new SqlParameter("ID_Hang", ct.ID_Hang),
                         new SqlParameter("TongTienDatKM_Den", ct.TongTienDatKM_Den),
                          new SqlParameter("TongTienDatKM_Tu", ct.TongTienDatKM_Tu),
                           new SqlParameter("SoLuongDatKM_Den", ct.SoLuongDatKM_Den),
                            new SqlParameter("SoLuongDatKM_Tu", ct.SoLuongDatKM_Tu),
                            new SqlParameter("ApDungBoiSo", ct.ApDungBoiSo),
                    };

                        object ID_ChiTiet = helper.ExecuteScalar("sp_CTKM_ChiTiet_ThemMoi_v2", parsChiTiet);


                        if (ID_ChiTiet != null && int.Parse(ID_ChiTiet.ToString()) > 0)
                        {
                            foreach (DataRow dr in dtHangTang.Rows)
                            {
                                if (dr["ID_Hang"].ToString() == ct.ID_Hang.ToString())
                                {
                                    SqlParameter[] parsHangTang = new SqlParameter[] {
                        new SqlParameter("@ID_CTKM_ChiTiet", ID_ChiTiet),
                        new SqlParameter("@ID_HangHoa", dr["ID_HangHoa"]),
                        new SqlParameter("@SoLuong",dr["SoLuong"]),
                          new SqlParameter("@ID_CTKM",ct.ID_CTKM)

                    };
                                    helper.ExecuteScalar("sp_CTKM_ChiTiet_HangTang_ThemMoi", parsHangTang);
                                }
                            }


                            //the moi hang tang

                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);

                }
                // them thanh cong

            }


        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return ret;
    }

    //ham son sua bị loi
    //public int ThemKhuyenMai(KhuyenMai km, DataTable dtHangTang)
    //{
    //    int ret = 0;
    //    try
    //    {
    //        if (km.Loai == 1 && km.ChietKhauPhanTram < 0 || km.ChietKhauPhanTram > 100 || km.ChietKhauTien < 0)
    //        {
    //            return ret;
    //        }

    //        SqlParameter output = new SqlParameter("@ID_CTKM", DbType.Int64);
    //        SqlParameter[] pars = new SqlParameter[] {

    //                new SqlParameter("ID_QLLH", km.ID_QLLH),
    //                new SqlParameter("TenCTKM", km.TenCTKM),
    //                new SqlParameter("NgayApDung", km.NgayApDung),
    //                new SqlParameter("NgayKetThuc", km.NgayKetThuc),
    //                new SqlParameter("ID_NhanVien", km.ID_NhanVien),
    //                new SqlParameter("ID_QuanLy", km.ID_QuanLy),
    //                new SqlParameter("Loai", km.Loai),
    //                new SqlParameter("ChietKhauPhanTram", km.ChietKhauPhanTram),
    //                new SqlParameter("ChietKhauTien", km.ChietKhauTien),
    //                new SqlParameter("GhiChu", km.GhiChu),
    //                new SqlParameter("TrangThai", 1),
    //                    new SqlParameter("TongTienDatKM_Den", km.TongTienDatKM_Den),
    //                      new SqlParameter("TongTienDatKM_Tu", km.TongTienDatKM_Tu),
    //               output,
    //            };
    //        object ID = helper.ExecuteScalar("sp_CTKM_ThemMoi", pars);
    //        if (ID != null && int.Parse(ID.ToString()) > 0)
    //        {
    //            ret = int.Parse(ID.ToString());
    //            try
    //            {
    //                //int ID_CTKM = int.Parse(output.SqlValue.ToString());

    //                foreach (ChiTietKhuyenMai ct in km.ChiTietCTKM)
    //                {
    //                    if (ct.ChietKhauPhanTram_BanBuon < 0 || ct.ChietKhauPhanTram_BanBuon > 100 || ct.ChietKhauPhanTram_BanLe < 0 || ct.ChietKhauPhanTram_BanLe > 100 || ct.ChietKhauTien_BanLe < 0 || ct.ChietKhauTien_BanBuon < 0)
    //                        break;
    //                    SqlParameter[] parsChiTiet = new SqlParameter[] {

    //                    new SqlParameter("ChietKhauPhanTram_BanBuon", ct.ChietKhauPhanTram_BanBuon),
    //                    new SqlParameter("ChietKhauPhanTram_BanLe", ct.ChietKhauPhanTram_BanLe),
    //                    new SqlParameter("ChietKhauTien_BanBuon", ct.ChietKhauTien_BanBuon),
    //                    new SqlParameter("ChietKhauTien_BanLe", ct.ChietKhauTien_BanLe),
    //                    new SqlParameter("GhiChu", ct.GhiChu != null ? ct.GhiChu : ""),
    //                    new SqlParameter("ID_CTKM", ID),
    //                    new SqlParameter("ID_Hang", ct.ID_Hang),
    //                     new SqlParameter("TongTienDatKM_Den", ct.TongTienDatKM_Den),
    //                      new SqlParameter("TongTienDatKM_Tu", ct.TongTienDatKM_Tu),
    //                       new SqlParameter("SoLuongDatKM_Den", ct.SoLuongDatKM_Den),
    //                        new SqlParameter("SoLuongDatKM_Tu", ct.SoLuongDatKM_Tu),
    //                        new SqlParameter("ApDungBoiSo", ct.ApDungBoiSo),
    //                };

    //                    object ID_ChiTiet = helper.ExecuteScalar("sp_CTKM_ChiTiet_ThemMoi_v2", parsChiTiet);


    //                    if (ID_ChiTiet != null && int.Parse(ID_ChiTiet.ToString()) > 0)
    //                    {
    //                        foreach (DataRow dr in dtHangTang.Rows)
    //                        {
    //                            if (dr["ID_Hang"].ToString() == ct.ID_Hang.ToString())
    //                            {
    //                                SqlParameter[] parsHangTang = new SqlParameter[] {
    //                    new SqlParameter("@ID_CTKM_ChiTiet", ID_ChiTiet),
    //                    new SqlParameter("@ID_HangHoa", dr["ID_HangHoa"]),
    //                    new SqlParameter("@SoLuong",dr["SoLuong"]),
    //                      new SqlParameter("@ID_CTKM",ct.ID_CTKM)

    //                };
    //                                helper.ExecuteScalar("sp_CTKM_ChiTiet_HangTang_ThemMoi", parsHangTang);
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                log.Error(ex);

    //            }
    //            // them thanh cong
    //            foreach (DataRow dr in dtHangTang.Rows)
    //            {
    //                if (int.Parse(dr["ID_Hang"].ToString()) == 0)
    //                {
    //                    SqlParameter[] parsHangTang = new SqlParameter[] {
    //                    new SqlParameter("@ID_CTKM_ChiTiet", 0),
    //                    new SqlParameter("@ID_HangHoa", dr["ID_HangHoa"]),
    //                    new SqlParameter("@SoLuong",dr["SoLuong"]),
    //                      new SqlParameter("@ID_CTKM",ret)

    //                };
    //                    helper.ExecuteScalar("sp_CTKM_ChiTiet_HangTang_ThemMoi", parsHangTang);
    //                }
    //            }

    //        }


    //    }
    //    catch (Exception ex)
    //    {
    //        log.Error(ex);
    //    }
    //    return ret;
    //}

    public bool SuaKhuyenMai(KhuyenMai km)
    {
        try
        {
            if (km.ChietKhauPhanTram < 0 || km.ChietKhauPhanTram > 100 || km.ChietKhauTien < 0)
            {
                return false;
            }
            SqlParameter[] pars = new SqlParameter[] {
                 new SqlParameter("ID_CTKM", km.ID_CTKM),
                new SqlParameter("ID_QLLH", km.ID_QLLH),
                new SqlParameter("TenCTKM", km.TenCTKM),
                new SqlParameter("NgayApDung", km.NgayApDung),
                new SqlParameter("NgayKetThuc", km.NgayKetThuc),
                new SqlParameter("ID_NhanVien", km.ID_NhanVien),
                new SqlParameter("ID_QuanLy", km.ID_QuanLy),
                new SqlParameter("Loai", km.Loai),
                new SqlParameter("ChietKhauPhanTram", km.ChietKhauPhanTram),
                new SqlParameter("ChietKhauTien", km.ChietKhauTien),
                 new SqlParameter("GhiChu", km.GhiChu != null ? km.GhiChu : ""),
                new SqlParameter("TrangThai", 1),
                    new SqlParameter("TongTienDatKM_Den", km.TongTienDatKM_Den),
                          new SqlParameter("TongTienDatKM_Tu", km.TongTienDatKM_Tu),

            };

            if (helper.ExecuteNonQuery("sp_CTKM_Sua", pars) != 0)
            {
                try
                {
                    //xoa chi tiet
                    SqlParameter[] parXoa = new SqlParameter[]{
                     new SqlParameter("ID_CTKM", km.ID_CTKM)
                    };

                    helper.ExecuteNonQuery("sp_CTKM_ChiTiet_Xoa", parXoa);
                    foreach (ChiTietKhuyenMai ct in km.ChiTietCTKM)
                    {
                        if (ct.ChietKhauPhanTram_BanBuon < 0 || ct.ChietKhauPhanTram_BanBuon > 100 || ct.ChietKhauPhanTram_BanLe < 0 || ct.ChietKhauPhanTram_BanLe > 100 || ct.ChietKhauTien_BanLe < 0 || ct.ChietKhauTien_BanBuon < 0)
                            break;
                        SqlParameter[] parsChiTiet = new SqlParameter[] {
                        new SqlParameter("ChietKhauPhanTram_BanBuon", ct.ChietKhauPhanTram_BanBuon),
                        new SqlParameter("ChietKhauPhanTram_BanLe", ct.ChietKhauPhanTram_BanLe),
                        new SqlParameter("ChietKhauTien_BanBuon", ct.ChietKhauTien_BanBuon),
                        new SqlParameter("ChietKhauTien_BanLe", ct.ChietKhauTien_BanLe),
                        new SqlParameter("GhiChu", ct.GhiChu != null ? ct.GhiChu : ""),
                        new SqlParameter("ID_CTKM", ct.ID_CTKM),
                        new SqlParameter("ID_Hang", ct.ID_Hang),
                         new SqlParameter("TongTienDatKM_Den", ct.TongTienDatKM_Den),
                          new SqlParameter("TongTienDatKM_Tu", ct.TongTienDatKM_Tu),
                           new SqlParameter("SoLuongDatKM_Den", ct.SoLuongDatKM_Den),
                            new SqlParameter("SoLuongDatKM_Tu", ct.SoLuongDatKM_Tu),
                            new SqlParameter("ApDungBoiSo", ct.ApDungBoiSo),
                    };
                        if (helper.ExecuteNonQuery("sp_CTKM_ChiTiet_ThemMoi", parsChiTiet) != 0)
                        {

                        }
                    }

                }
                catch (Exception ex)
                {


                }
                // them thanh cong
                return true;
            }
            else
            {
                // them that bai
                return false;
            }
        }
        catch
        {
            return false;
        }
    }


    //public bool SuaKhuyenMai_v2(KhuyenMai km, DataTable dtHangTang)
    //{
    //    try
    //    {
    //        if (km.ChietKhauPhanTram < 0 || km.ChietKhauPhanTram > 100 || km.ChietKhauTien < 0)
    //        {
    //            return false;
    //        }
    //        SqlParameter[] pars = new SqlParameter[] {
    //             new SqlParameter("ID_CTKM", km.ID_CTKM),
    //            new SqlParameter("ID_QLLH", km.ID_QLLH),
    //            new SqlParameter("TenCTKM", km.TenCTKM),
    //            new SqlParameter("NgayApDung", km.NgayApDung),
    //            new SqlParameter("NgayKetThuc", km.NgayKetThuc),
    //            new SqlParameter("ID_NhanVien", km.ID_NhanVien),
    //            new SqlParameter("ID_QuanLy", km.ID_QuanLy),
    //            new SqlParameter("Loai", km.Loai),
    //            new SqlParameter("ChietKhauPhanTram", km.ChietKhauPhanTram),
    //            new SqlParameter("ChietKhauTien", km.ChietKhauTien),
    //             new SqlParameter("GhiChu", km.GhiChu != null ? km.GhiChu : ""),
    //            new SqlParameter("TrangThai", 1),
    //                new SqlParameter("TongTienDatKM_Den", km.TongTienDatKM_Den),
    //                      new SqlParameter("TongTienDatKM_Tu", km.TongTienDatKM_Tu),

    //        };

    //        if (helper.ExecuteNonQuery("sp_CTKM_Sua", pars) != 0)
    //        {
    //            try
    //            {
    //                //xoa chi tiet
    //                SqlParameter[] parXoa = new SqlParameter[]{
    //                 new SqlParameter("ID_CTKM", km.ID_CTKM)
    //                };

    //                SqlParameter[] parXoa2 = new SqlParameter[]{
    //                 new SqlParameter("ID_CTKM", km.ID_CTKM)
    //                };

    //                helper.ExecuteNonQuery("sp_CTKM_ChiTiet_Xoa", parXoa);

    //                if (km.Loai == 10)
    //                {
    //                    helper.ExecuteNonQuery("sp_CTKM_ChiTiet_HangTang_Xoa", parXoa2);
    //                    foreach (DataRow dr in dtHangTang.Rows)
    //                    {
    //                        if (int.Parse(dr["ID_Hang"].ToString()) == 0)
    //                        {
    //                            SqlParameter[] parsHangTang = new SqlParameter[] {
    //                    new SqlParameter("@ID_CTKM_ChiTiet", 0),
    //                    new SqlParameter("@ID_HangHoa", dr["ID_HangHoa"]),
    //                    new SqlParameter("@SoLuong",dr["SoLuong"]),
    //                      new SqlParameter("@ID_CTKM",km.ID_CTKM)

    //                };
    //                            helper.ExecuteScalar("sp_CTKM_ChiTiet_HangTang_ThemMoi", parsHangTang);
    //                        }
    //                    }
    //                }
    //                foreach (ChiTietKhuyenMai ct in km.ChiTietCTKM)
    //                {
    //                    if (ct.ChietKhauPhanTram_BanBuon < 0 || ct.ChietKhauPhanTram_BanBuon > 100 || ct.ChietKhauPhanTram_BanLe < 0 || ct.ChietKhauPhanTram_BanLe > 100 || ct.ChietKhauTien_BanLe < 0 || ct.ChietKhauTien_BanBuon < 0)
    //                        break;
    //                    SqlParameter[] parsChiTiet = new SqlParameter[] {
    //                    new SqlParameter("ChietKhauPhanTram_BanBuon", ct.ChietKhauPhanTram_BanBuon),
    //                    new SqlParameter("ChietKhauPhanTram_BanLe", ct.ChietKhauPhanTram_BanLe),
    //                    new SqlParameter("ChietKhauTien_BanBuon", ct.ChietKhauTien_BanBuon),
    //                    new SqlParameter("ChietKhauTien_BanLe", ct.ChietKhauTien_BanLe),
    //                    new SqlParameter("GhiChu", ct.GhiChu != null ? ct.GhiChu : ""),
    //                    new SqlParameter("ID_CTKM", ct.ID_CTKM),
    //                    new SqlParameter("ID_Hang", ct.ID_Hang),
    //                     new SqlParameter("TongTienDatKM_Den", ct.TongTienDatKM_Den),
    //                      new SqlParameter("TongTienDatKM_Tu", ct.TongTienDatKM_Tu),
    //                       new SqlParameter("SoLuongDatKM_Den", ct.SoLuongDatKM_Den),
    //                        new SqlParameter("SoLuongDatKM_Tu", ct.SoLuongDatKM_Tu),
    //                        new SqlParameter("ApDungBoiSo", ct.ApDungBoiSo),
    //                };
    //                    //if (helper.ExecuteNonQuery("sp_CTKM_ChiTiet_ThemMoi", parsChiTiet) != 0)
    //                    //{

    //                    //}


    //                    object ID_ChiTiet = helper.ExecuteScalar("sp_CTKM_ChiTiet_ThemMoi_v2", parsChiTiet);


    //                    if (ID_ChiTiet != null && int.Parse(ID_ChiTiet.ToString()) > 0)
    //                    {

    //                        foreach (DataRow dr in dtHangTang.Rows)
    //                        {
    //                            if (dr["ID_Hang"].ToString() == ct.ID_Hang.ToString())
    //                            {
    //                                SqlParameter[] parsHangTang = new SqlParameter[] {
    //                    new SqlParameter("@ID_CTKM_ChiTiet", ID_ChiTiet),
    //                    new SqlParameter("@ID_HangHoa", dr["ID_HangHoa"]),
    //                    new SqlParameter("@SoLuong",dr["SoLuong"]),
    //                      new SqlParameter("@ID_CTKM",ct.ID_CTKM)

    //                };
    //                                helper.ExecuteScalar("sp_CTKM_ChiTiet_HangTang_ThemMoi", parsHangTang);
    //                            }
    //                        }

    //                        //the moi hang tang

    //                    }
    //                }
    //            }
    //            catch (Exception ex)
    //            {


    //            }
    //            // them thanh cong
    //            return true;
    //        }
    //        else
    //        {
    //            // them that bai
    //            return false;
    //        }
    //    }
    //    catch
    //    {
    //        return false;
    //    }
    //}

    public bool SuaKhuyenMai_v2(KhuyenMai km, DataTable dtHangTang)
    {
        try
        {
            if (km.ChietKhauPhanTram < 0 || km.ChietKhauPhanTram > 100 || km.ChietKhauTien < 0)
            {
                return false;
            }
            SqlParameter[] pars = new SqlParameter[] {
                 new SqlParameter("ID_CTKM", km.ID_CTKM),
                new SqlParameter("ID_QLLH", km.ID_QLLH),
                new SqlParameter("TenCTKM", km.TenCTKM),
                new SqlParameter("NgayApDung", km.NgayApDung),
                new SqlParameter("NgayKetThuc", km.NgayKetThuc),
                new SqlParameter("ID_NhanVien", km.ID_NhanVien),
                new SqlParameter("ID_QuanLy", km.ID_QuanLy),
                new SqlParameter("Loai", km.Loai),
                new SqlParameter("ChietKhauPhanTram", km.ChietKhauPhanTram),
                new SqlParameter("ChietKhauTien", km.ChietKhauTien),
                 new SqlParameter("GhiChu", km.GhiChu != null ? km.GhiChu : ""),
                new SqlParameter("TrangThai", 1),
                    new SqlParameter("TongTienDatKM_Den", km.TongTienDatKM_Den),
                          new SqlParameter("TongTienDatKM_Tu", km.TongTienDatKM_Tu),

            };

            if (helper.ExecuteNonQuery("sp_CTKM_Sua", pars) != 0)
            {
                try
                {
                    //xoa chi tiet
                    SqlParameter[] parXoa = new SqlParameter[]{
                     new SqlParameter("ID_CTKM", km.ID_CTKM)
                    };

                    helper.ExecuteNonQuery("sp_CTKM_ChiTiet_Xoa", parXoa);
                    foreach (ChiTietKhuyenMai ct in km.ChiTietCTKM)
                    {
                        if (ct.ChietKhauPhanTram_BanBuon < 0 || ct.ChietKhauPhanTram_BanBuon > 100 || ct.ChietKhauPhanTram_BanLe < 0 || ct.ChietKhauPhanTram_BanLe > 100 || ct.ChietKhauTien_BanLe < 0 || ct.ChietKhauTien_BanBuon < 0)
                            break;
                        SqlParameter[] parsChiTiet = new SqlParameter[] {
                        new SqlParameter("ChietKhauPhanTram_BanBuon", ct.ChietKhauPhanTram_BanBuon),
                        new SqlParameter("ChietKhauPhanTram_BanLe", ct.ChietKhauPhanTram_BanLe),
                        new SqlParameter("ChietKhauTien_BanBuon", ct.ChietKhauTien_BanBuon),
                        new SqlParameter("ChietKhauTien_BanLe", ct.ChietKhauTien_BanLe),
                        new SqlParameter("GhiChu", ct.GhiChu != null ? ct.GhiChu : ""),
                        new SqlParameter("ID_CTKM", ct.ID_CTKM),
                        new SqlParameter("ID_Hang", ct.ID_Hang),
                         new SqlParameter("TongTienDatKM_Den", ct.TongTienDatKM_Den),
                          new SqlParameter("TongTienDatKM_Tu", ct.TongTienDatKM_Tu),
                           new SqlParameter("SoLuongDatKM_Den", ct.SoLuongDatKM_Den),
                            new SqlParameter("SoLuongDatKM_Tu", ct.SoLuongDatKM_Tu),
                            new SqlParameter("ApDungBoiSo", ct.ApDungBoiSo),
                    };
                        //if (helper.ExecuteNonQuery("sp_CTKM_ChiTiet_ThemMoi", parsChiTiet) != 0)
                        //{

                        //}


                        object ID_ChiTiet = helper.ExecuteScalar("sp_CTKM_ChiTiet_ThemMoi_v2", parsChiTiet);


                        if (ID_ChiTiet != null && int.Parse(ID_ChiTiet.ToString()) > 0)
                        {

                            foreach (DataRow dr in dtHangTang.Rows)
                            {
                                if (dr["ID_Hang"].ToString() == ct.ID_Hang.ToString())
                                {
                                    SqlParameter[] parsHangTang = new SqlParameter[] {
                        new SqlParameter("@ID_CTKM_ChiTiet", ID_ChiTiet),
                        new SqlParameter("@ID_HangHoa", dr["ID_HangHoa"]),
                        new SqlParameter("@SoLuong",dr["SoLuong"]),
                          new SqlParameter("@ID_CTKM",ct.ID_CTKM)

                    };
                                    helper.ExecuteScalar("sp_CTKM_ChiTiet_HangTang_ThemMoi", parsHangTang);
                                }
                            }

                            //the moi hang tang

                        }
                    }

                }
                catch (Exception ex)
                {


                }
                // them thanh cong
                return true;
            }
            else
            {
                // them that bai
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    public DataTable GetChiTietHangTang(int ID_CTKM)
    {
        DataTable dt = new DataTable();
        try
        {
            KhuyenMai km = new KhuyenMai();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_CTKM", ID_CTKM)
        };

            DataSet ds = helper.ExecuteDataSet("sp_CTKM_GetChiTietHangTang", pars);
            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {

            log.Error(ex);
        }
        return dt;



    }
    public bool ThemKhuyenMai_ChiTietHangTang(int ID_CTKM, int ID_CTKM_ChiTiet, int ID_HangHoa, double SoLuong)
    {
        bool ret = false;
        try
        {

            SqlParameter[] parsHangTang = new SqlParameter[] {
                    new SqlParameter("@ID_CTKM_ChiTiet", ID_CTKM_ChiTiet),
                    new SqlParameter("@ID_HangHoa",ID_HangHoa),
                    new SqlParameter("@SoLuong",SoLuong),
                        new SqlParameter("@ID_CTKM", ID_CTKM)

                };
            int x = helper.ExecuteNonQuery("sp_CTKM_ChiTiet_HangTang_ThemMoi", parsHangTang);
            if (x > 0)
                ret = true;



        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return ret;
    }
    public DataTable GetChiTietHangTang_TheoMatHang(int ID_CTKM, int ID_Hang)
    {
        DataTable dt = new DataTable();
        try
        {
            KhuyenMai km = new KhuyenMai();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_CTKM", ID_CTKM),
             new SqlParameter("ID_Hang", ID_Hang)
        };

            DataSet ds = helper.ExecuteDataSet("sp_CTKM_GetChiTietHangTang", pars);
            dt = ds.Tables[0];
        }
        catch (Exception ex)
        {

            log.Error(ex);
        }
        return dt;



    }

    public bool DeleteKhuyenMai(KhuyenMai km)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_CTKM", km.ID_CTKM)

            };

            if (helper.ExecuteNonQuery("sp_CTKM_Delete_By_ID_CTKM", pars) != 0)
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
        catch
        {
            return false;
        }
    }
    public bool NgungSuDungKhuyenMai(KhuyenMai km)
    {
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_CTKM", km.ID_CTKM)

            };

            if (helper.ExecuteNonQuery("sp_CTKM_NgungSuDung_By_ID_CTKM", pars) != 0)
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
        catch
        {
            return false;
        }
    }
}