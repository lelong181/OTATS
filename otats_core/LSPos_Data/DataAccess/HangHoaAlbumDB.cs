using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using log4net;
using System.Data;

/// <summary>
/// Summary description for HangHoaAlbumDB
/// </summary>
public class HangHoaAlbumDB
{
    private static ILog log = LogManager.GetLogger(typeof(HangHoaAlbumDB));
    public static SqlDataHelper db = new SqlDataHelper();

    public HangHoaAlbumDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }
 

    public static List<HangHoaAlbumOBJ> LayDanhSachAnh_TheoMatHang(int idconty,int idmathang)
    {
        List<HangHoaAlbumOBJ> rs = new List<HangHoaAlbumOBJ>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idcongty", idconty),
                new SqlParameter("@idmathang", idmathang),
              
            };


            
            try
            {
                     
                DataTable dtChitiet = db.ExecuteDataSet("sp_HangHoa_AlbumImages_GetByIdHang", par).Tables[0];
                foreach (DataRow drChiTiet in dtChitiet.Rows)
                {
                    HangHoaAlbumOBJ alb = new HangHoaAlbumOBJ();
                    alb.ID_AlbumAnh = int.Parse(drChiTiet["ID_AlbumAnh"].ToString());
                    alb.ID_QLLH = (drChiTiet["ID_QLLH"].ToString() != "") ? (int.Parse(drChiTiet["ID_QLLH"].ToString())) : 0;
                    alb.ID_QuanLy = (drChiTiet["ID_QuanLy"].ToString() != "") ? (int.Parse(drChiTiet["ID_QuanLy"].ToString())) : 0;
                    alb.ID_Hang = (drChiTiet["ID_Hang"].ToString() != "") ? (int.Parse(drChiTiet["ID_Hang"].ToString())) : 0;
                    alb.GhiChu = drChiTiet["GhiChu"].ToString();
                    //get chi tiet
                    SqlParameter[] parChiTiet = new SqlParameter[]{
                new SqlParameter("@idalbum", alb.ID_AlbumAnh), 

                    };
                    alb.DanhSachAnh = new List<HangHoaImagesOBJ>();
                    DataTable dtCT = db.ExecuteDataSet("sp_HangHoa_AlbumImages_GetByIdAlbum", parChiTiet).Tables[0];
                    foreach (DataRow drCT in dtCT.Rows)
                    {
                        HangHoaImagesOBJ img = new HangHoaImagesOBJ();
                        img.ID_AlbumAnh = int.Parse(drCT["ID_AlbumAnh"].ToString());
                        img.ID = (drCT["ID"].ToString() != "") ? (int.Parse(drCT["ID"].ToString())) : 0;
                        img.path = drCT["path"].ToString();
                        alb.DanhSachAnh.Add(img);
                    }

                    rs.Add(alb);
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


}
 