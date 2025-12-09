using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ImageDB
/// </summary>
public class ImageDB
{

    public static SqlDataHelper db = new SqlDataHelper();
    public ImageDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static List<ImageOBJ> DanhSachAnh(int idnhanvien, int idkhachhang, DateTime thoigian)
    {
        List<ImageOBJ> rs = new List<ImageOBJ>();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@idnhanvien", idnhanvien),
            new SqlParameter("@idkhachhang", idkhachhang),
            new SqlParameter("@thoigian", thoigian)
        };

            DataTable dt = db.ExecuteDataSet("sp_QL_AnhChup", pars).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                rs.Add(new ImageOBJ
                {
                    imageid = int.Parse(dr["imageid"].ToString()),
                    path = dr["path"].ToString(),
                    thoigian = DateTime.Parse(dr["insertedtime"].ToString())
                });
            }
            return rs;
        }
        catch (Exception ex)
        {            
            return rs;
        }
    }

    public static List<ImageOBJ> bonAnhDaiLy(int idkhachahng)
    {
        List<ImageOBJ> rs = new List<ImageOBJ>();
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("@idkhachhang", idkhachahng)
            };

            DataTable dt = db.ExecuteDataSet("Get4ImageDaiLy", pars).Tables[0];

            foreach (DataRow item in dt.Rows)
            {
                rs.Add(new ImageOBJ { 
                    imageid = int.Parse(item["imageid"].ToString()),
                    path = item["path"].ToString(),
                    thoigian = DateTime.Parse(item["insertedtime"].ToString())
                });
            }

            return rs;

        }
        catch
        {
            return null;
        }

    }

    public static int ThemAlbum(AlbumOBJ obj)
    {
        int ID = 0;
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idnhanvien", obj.idnhanvien),
                new SqlParameter("@idkhachang", obj.idkhachhang),
                new SqlParameter("@idcongty", obj.idcongty),
                new SqlParameter("@hinhdaidien", obj.hinhdaidien),
                new SqlParameter("@kinhdo", obj.kinhdo),
                new SqlParameter("@vido", obj.vido),
                new SqlParameter("@ghichu", obj.ghichu),
                 new SqlParameter("@diachi", obj.diachi),
                 new SqlParameter("@idalbum", obj.idalbum),
            };
            object objID = db.ExecuteScalar("sp_Album_ThemMoi", par);
            if (objID != null)
            {
                ID = int.Parse(objID.ToString());
            }
        }
        catch (Exception ex)
        {
           
        }

        return ID;
    }

    public static List<AlbumOBJ> DanhSachAlbum(int idnhanvien, DateTime from, DateTime to)
    {
        List<AlbumOBJ> rs = new List<AlbumOBJ>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idnhanvien", idnhanvien),
             new SqlParameter("@from", from.ToString("yyyy-MM-dd")),
              new SqlParameter("@to", to.ToString("yyyy-MM-dd 23:59:59")),
            };
            DataTable dt = db.ExecuteDataSet("sp_Album_GetDanhSach", par).Tables[0];
            DateTime d;
            foreach (DataRow dr in dt.Rows)
            {
                rs.Add(GetDuLieuFromDataRow(dr));
            }
            return rs;
        }
        catch (Exception ex)
        {
            
            return rs;
        }

    }
    public static List<AlbumOBJ> DanhSachAlbum_TheoDonHang(int ID_DonHang)
    {
        List<AlbumOBJ> rs = new List<AlbumOBJ>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@iddonhang", ID_DonHang) 
            };
            DataTable dt = db.ExecuteDataSet("sp_Album_GetDanhSach_ByIDDonHang", par).Tables[0];
            DateTime d;
            foreach (DataRow dr in dt.Rows)
            {
                rs.Add(GetDuLieuFromDataRow(dr));
            }
            return rs;
        }
        catch (Exception ex)
        {

            return rs;
        }

    }
    public static List<ImageOBJ> DanhSachAnh_ByIDAlbum(int ID_Album)
    {
        List<ImageOBJ> rs = new List<ImageOBJ>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_Album", ID_Album)
            };
            DataTable dt = db.ExecuteDataSet("sp_Album_GetImage_ByIDAlbum", par).Tables[0];
            DateTime d;
            int i =0;
            foreach (DataRow dr in dt.Rows)
            {
                rs.Add(new ImageOBJ
                {
                    imageid = int.Parse(dr["imageid"].ToString()),
                    path = dr["path"].ToString(),
                    path_thumbnail_medium = dr["path_thumbnail_medium"].ToString(),
                    path_thumbnail_small = dr["path_thumbnail_small"].ToString(),
                    thoigian =   DateTime.Parse(dr["insertedtime"].ToString()),
                    kinhdo = double.Parse(dr["kinhdo"].ToString()),
                    vido = double.Parse(dr["vido"].ToString()),
                    idkhachhang = int.Parse(dr["idkhachhang"].ToString()),
                    idnhanvien = int.Parse(dr["idnhanvien"].ToString()),
                    idcongty = int.Parse(dr["idcongty"].ToString()),
                    ghichu = dr["ghichu"].ToString(),
                    tendaily = dr["TenKhachHang"].ToString(),
                    diachi = dr["diachi"].ToString(),
                    idalbum = dr["ID_Album"].ToString() != "" ? int.Parse(dr["ID_Album"].ToString()) : 0,
                    stt = dr["STT"].ToString() != "" ? int.Parse(dr["STT"].ToString()) : 0,
                    stt_identity =i
                });

                i++;
            }
            return rs;
        }
        catch (Exception ex)
        {
           
            return rs;
        }

    }
    public static AlbumOBJ GetAlbumById(int ID_Album)
    {
        AlbumOBJ rs = new AlbumOBJ();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_Album", ID_Album)
            };
            DataTable dt = db.ExecuteDataSet("sp_Album_GetById", par).Tables[0];
            DateTime d;
            foreach (DataRow dr in dt.Rows)
            {
                rs = (GetDuLieuFromDataRow(dr));
            }
            return rs;
        }
        catch (Exception ex)
        {
            
            return rs;
        }

    }
    public static AlbumOBJ GetDuLieuFromDataRow(DataRow dr)
    {
        AlbumOBJ rs = new AlbumOBJ();
        try
        {

            rs = new AlbumOBJ();
            rs.danhsachanh = DanhSachAnh_ByIDAlbum(int.Parse(dr["ID_Album"].ToString()));
            rs.idalbum = int.Parse(dr["ID_Album"].ToString());
            rs.hinhdaidien = dr["HinhDaiDien"].ToString() != "" ? dr["HinhDaiDien"].ToString() : ((rs.danhsachanh != null && rs.danhsachanh.Count > 0) ? rs.danhsachanh[0].path : "");
            rs.thoigiantao = dr["NgayTao"].ToString() != "" ? DateTime.Parse(dr["NgayTao"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") : "";
            rs.kinhdo = dr["KinhDo"].ToString() != "" ? double.Parse(dr["KinhDo"].ToString()) : 0;
            rs.vido = dr["ViDo"].ToString() != "" ? double.Parse(dr["ViDo"].ToString()) : 0;
            rs.idkhachhang = dr["ID_KhachHang"].ToString() != "" ? int.Parse(dr["ID_KhachHang"].ToString()) : 0;
            rs.idnhanvien = dr["ID_NhanVien"].ToString() != "" ? int.Parse(dr["ID_NhanVien"].ToString()) : 0;
            rs.idcongty = dr["ID_QLLH"].ToString() != "" ? int.Parse(dr["ID_QLLH"].ToString()) : 0;
            rs.ghichu = dr["GhiChu"].ToString();
            rs.diachi = dr["DiaChi"].ToString();
            rs.tennhanvien = dr["TenNhanVien"].ToString();
            try
            {
                rs.tenkhachhang = dr["TenKhachHang"].ToString();
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

    public static DataTable BaoCaoAnhChupTheoAlbum(int idcty, int idnhanvien, int idkhachhang, DateTime tungay, DateTime denNgay, int ID_QuanLy)
    {
        DataTable dt = null;
        SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", idcty),
            new SqlParameter("ID_NhanVien", idnhanvien),
            new SqlParameter("ID_KhachHang", idkhachhang),
            new SqlParameter("dtFrom", tungay.ToString("yyyy-MM-dd")),
            new SqlParameter("dtTo", denNgay.ToString("yyyy-MM-dd 23:59:59")),
            new SqlParameter("ID_QuanLy", ID_QuanLy)
        };
        try
        {
            dt = db.ExecuteDataSet("sp_web_BaoCaoAnhChup_album", pars).Tables[0];
        }
        catch (Exception)
        {
            dt = null;
        }

        return dt;
    }

    public static  ImageOBJ  GetImageById(int IDImage)
    {
         ImageOBJ  rs = new ImageOBJ();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@IDImage", IDImage)
            };
            DataTable dt = db.ExecuteDataSet("sp_Images_GetImageById", par).Tables[0];
            DateTime d;
            foreach (DataRow dr in dt.Rows)
            {
                rs = new ImageOBJ
                {
                    imageid = int.Parse(dr["imageid"].ToString()),
                    path = dr["path"].ToString(),
                    thoigian = DateTime.Parse(dr["insertedtime"].ToString()),
                    kinhdo = double.Parse(dr["kinhdo"].ToString()),
                    vido = double.Parse(dr["vido"].ToString()),
                    idkhachhang = int.Parse(dr["idkhachhang"].ToString()),
                    idnhanvien = int.Parse(dr["idnhanvien"].ToString()),
                    idcongty = int.Parse(dr["idcongty"].ToString()),
                    ghichu = dr["ghichu"].ToString(),
                    tendaily = dr["TenKhachHang"].ToString(),
                    tennhanvien = dr["TenNhanVien"].ToString(),
                    diachi = dr["diachi"].ToString(),
                    idalbum = dr["ID_Album"].ToString() != "" ? int.Parse(dr["ID_Album"].ToString()) : 0,
                    stt = dr["STT"].ToString() != "" ? int.Parse(dr["STT"].ToString()) : 0,
                };
            }
            return rs;
        }
        catch (Exception ex)
        {

            return rs;
        }

    }
    public static List<ImageOBJ> DanhSachAnh_ByIDCheckIn(int ID_CheckIn)
    {
        List<ImageOBJ> rs = new List<ImageOBJ>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_CheckIn", ID_CheckIn)
            };
            DataTable dt = db.ExecuteDataSet("sp_CheckIn_GetImage_ByIDCheckIn", par).Tables[0];
            DateTime d;
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                rs.Add(new ImageOBJ
                {
                    imageid = int.Parse(dr["imageid"].ToString()),
                    path = dr["path"].ToString(),
                    path_thumbnail_medium = dr["path_thumbnail_medium"].ToString(),
                    path_thumbnail_small = dr["path_thumbnail_small"].ToString(),
                    thoigian = DateTime.Parse(dr["insertedtime"].ToString()),
                    kinhdo = double.Parse(dr["kinhdo"].ToString()),
                    vido = double.Parse(dr["vido"].ToString()),
                    idkhachhang = int.Parse(dr["idkhachhang"].ToString()),
                    idnhanvien = int.Parse(dr["idnhanvien"].ToString()),
                    idcongty = int.Parse(dr["idcongty"].ToString()),
                    ghichu = dr["ghichu"].ToString(),
                    tendaily = dr["TenKhachHang"].ToString(),
                    diachi = dr["diachi"].ToString(),
                    idalbum = dr["ID_Album"].ToString() != "" ? int.Parse(dr["ID_Album"].ToString()) : 0,
                    stt = dr["STT"].ToString() != "" ? int.Parse(dr["STT"].ToString()) : 0,
                    stt_identity = i
                });

                i++;
            }
            return rs;
        }
        catch (Exception ex)
        {

            return rs;
        }

    }

    public static List<ImageOBJ> DanhSachAnh_ByIDLichSu(int IDLichSu)
    {
        List<ImageOBJ> rs = new List<ImageOBJ>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_Xe_LichSuBaoDuong", IDLichSu)
            };
            DataTable dt = db.ExecuteDataSet("sp_Images_GetByIDBaoDuong", par).Tables[0];
            DateTime d;
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                rs.Add(new ImageOBJ
                {
                    imageid = int.Parse(dr["imageid"].ToString()),
                    path = dr["path"].ToString(),
                    path_thumbnail_medium = dr["path_thumbnail_medium"].ToString(),
                    path_thumbnail_small = dr["path_thumbnail_small"].ToString(),
                    thoigian = DateTime.Parse(dr["insertedtime"].ToString()),
                    kinhdo = double.Parse(dr["kinhdo"].ToString()),
                    vido = double.Parse(dr["vido"].ToString()),
                    idkhachhang = int.Parse(dr["idkhachhang"].ToString()),
                    idnhanvien = int.Parse(dr["idnhanvien"].ToString()),
                    idcongty = int.Parse(dr["idcongty"].ToString()),
                    ghichu = dr["ghichu"].ToString(),
                    
                    diachi = dr["diachi"].ToString(),
                    idalbum = dr["ID_Album"].ToString() != "" ? int.Parse(dr["ID_Album"].ToString()) : 0,
                    stt = dr["STT"].ToString() != "" ? int.Parse(dr["STT"].ToString()) : 0,
                    stt_identity = i
                });

                i++;
            }
            return rs;
        }
        catch (Exception ex)
        {

            return rs;
        }

    }
}