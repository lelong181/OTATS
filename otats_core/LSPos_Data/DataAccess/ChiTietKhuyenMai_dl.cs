using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


/// <summary>
/// Summary description for ChiTietKhuyenMai_dl
/// </summary>
public class ChiTietKhuyenMai_dl
{

   static log4net.ILog log = log4net.LogManager.GetLogger(typeof( ChiTietKhuyenMai_dl));
    public ChiTietKhuyenMai_dl()
    {
        //helper = new SqlDataHelper();
    }

    public static ChiTietKhuyenMai GetChiTietKhuyenMaiFromDataRow(DataRow drChiTiet)
    {
        try
        {
            ChiTietKhuyenMai km = new ChiTietKhuyenMai();
            try
            {
               
                km.ID_ChiTietCTKM = (drChiTiet["ID"].ToString() != "") ? int.Parse(drChiTiet["ID"].ToString()) : 0;
                km.ID_CTKM = (drChiTiet["ID_CTKM"].ToString() != "") ? int.Parse(drChiTiet["ID_CTKM"].ToString()) : 0;
                km.ID_Hang = (drChiTiet["ID_Hang"].ToString() != "") ? int.Parse(drChiTiet["ID_Hang"].ToString()) : 0;

                km.ChietKhauPhanTram_BanBuon = (drChiTiet["ChietKhauPhanTram_BanBuon"].ToString() != "") ? float.Parse(drChiTiet["ChietKhauPhanTram_BanBuon"].ToString()) : 0;
                km.ChietKhauTien_BanBuon = (drChiTiet["ChietKhauTien_BanBuon"].ToString() != "") ? double.Parse(drChiTiet["ChietKhauTien_BanBuon"].ToString()) : 0;
                km.ChietKhauPhanTram_BanLe = (drChiTiet["ChietKhauPhanTram_BanLe"].ToString() != "") ? float.Parse(drChiTiet["ChietKhauPhanTram_BanLe"].ToString()) : 0;
                km.ChietKhauTien_BanLe = (drChiTiet["ChietKhauTien_BanLe"].ToString() != "") ? double.Parse(drChiTiet["ChietKhauTien_BanLe"].ToString()) : 0;
                km.NgayTao = (drChiTiet["Ngay"].ToString() != "") ? Convert.ToDateTime(drChiTiet["Ngay"].ToString()) : km.NgayTao;
                km.GhiChu = drChiTiet["GhiChu"].ToString();
                km.TenMatHang = drChiTiet["TenHang"].ToString();
                km.GiaBanBuon = (drChiTiet["GiaBanBuon"].ToString() != "") ? double.Parse(drChiTiet["GiaBanBuon"].ToString()) : 0;
                km.GiaBanLe = (drChiTiet["GiaBanLe"].ToString() != "") ? double.Parse(drChiTiet["GiaBanLe"].ToString()) : 0;
                km.MaHang = drChiTiet["MaHang"].ToString();
                km.DonViTinh = drChiTiet["TenDonVi"].ToString();

               

                km.SoLuongDatKM_Den = (drChiTiet["SoLuongDatKM_Den"].ToString() != "") ? double.Parse(drChiTiet["SoLuongDatKM_Den"].ToString()) : 0;
                km.SoLuongDatKM_Tu = (drChiTiet["SoLuongDatKM_Tu"].ToString() != "") ? double.Parse(drChiTiet["SoLuongDatKM_Tu"].ToString()) : 0;
                km.TongTienDatKM_Den = (drChiTiet["TongTienDatKM_Den"].ToString() != "") ? double.Parse(drChiTiet["TongTienDatKM_Den"].ToString()) : 0;
                km.TongTienDatKM_Tu = (drChiTiet["TongTienDatKM_Tu"].ToString() != "") ? double.Parse(drChiTiet["TongTienDatKM_Tu"].ToString()) : 0;
                km.ApDungBoiSo = (drChiTiet["ApDungBoiSo"].ToString() != "") ? int.Parse(drChiTiet["ApDungBoiSo"].ToString()) : 0;
                km.TenDanhMuc = drChiTiet["TenDanhMuc"].ToString();
                km.ID_DANHMUC = (drChiTiet["ID_DANHMUC"].ToString() != "") ? int.Parse(drChiTiet["ID_DANHMUC"].ToString()) : 0;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                
            }

            return km;
        }
        catch
        {
            return null;
        }
    }

    public static List<ChiTietKhuyenMai> GetChiTietKhuyenMai(int ID_QLLH, int ID_CTKM)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_CTKM", ID_CTKM)
        };
        SqlDataHelper helper = new SqlDataHelper();
        DataSet ds = helper.ExecuteDataSet("sp_QL_GetChiTietKhuyenMai_ById", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {
            List<ChiTietKhuyenMai> dsdh = new List<ChiTietKhuyenMai>();
            foreach (DataRow dr in dt.Rows)
            {
                ChiTietKhuyenMai ctdh = ChiTietKhuyenMai_dl.GetChiTietKhuyenMaiFromDataRow(dr);
                dsdh.Add(ctdh);
            }

            return dsdh;
        }
        catch
        {
            return null;
        }
    }

    public static List<ChiTietKhuyenMai> GetChiTietKhuyenMai_DanhMuc(int ID_DANHMUC,  int ID_QLLH, int ID_CTKM)
    {
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("ID_CTKM", ID_CTKM),
             new SqlParameter("@ID_DANHMUC", ID_DANHMUC),
        };
        SqlDataHelper helper = new SqlDataHelper();
        DataSet ds = helper.ExecuteDataSet("sp_QL_GetChiTietKhuyenMai_ById_AND_ID_DANHMUC", pars);
        DataTable dt = ds.Tables[0];
         
        try
        {
            List<ChiTietKhuyenMai> dsdh = new List<ChiTietKhuyenMai>();
            foreach (DataRow dr in dt.Rows)
            {
                ChiTietKhuyenMai ctdh = ChiTietKhuyenMai_dl.GetChiTietKhuyenMaiFromDataRow(dr);
                dsdh.Add(ctdh);
            }
            //lay cac mat hang cua danh muc con
            List<DanhMucOBJ> lstDMCon = DanhMucDB.getDS_DanhMucCon_ByIdDanhMuc(ID_DANHMUC);
            foreach (DanhMucOBJ dm in lstDMCon)
            {
                List<ChiTietKhuyenMai> lstHangHoaCon = GetChiTietKhuyenMai_DanhMuc(dm.ID_DANHMUC, ID_QLLH, ID_CTKM);
                foreach (ChiTietKhuyenMai h in lstHangHoaCon)
                {
                    dsdh.Add(h);
                }
            }
            return dsdh;
        }
        catch
        {
            return null;
        }
    }

    public static DataTable  GetDonHangByCTKM(int ID_CTKM)
    {
        SqlParameter[] pars = new SqlParameter[] {
             
            new SqlParameter("ID_CTKM", ID_CTKM)
        };
        SqlDataHelper helper = new SqlDataHelper();
        DataSet ds = helper.ExecuteDataSet("sp_QL_GetDonHang_ByID_CTKM", pars);
        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count == 0)
            return null;

        try
        {


            return dt;
        }
        catch
        {
            return null;
        }
    }
     
}