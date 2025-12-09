using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;


namespace LSPos_Data.Data
{
    public class AlbumData
    {
        private SqlDataHelper helper;
        public AlbumData()
        {
            helper = new SqlDataHelper();
        }

        private AlbumModel GetAlbumModelFromDataRow(DataRow dr)
        {
            AlbumModel obj = new AlbumModel();
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                if (dr.Table.Columns.IndexOf(propertyInfo.Name) >= 0)
                {

                    if (!string.IsNullOrWhiteSpace(dr[propertyInfo.Name].ToString()))
                    {
                        var value = Convert.ChangeType(dr[propertyInfo.Name], propertyInfo.PropertyType);
                        propertyInfo.SetValue(obj, value);
                    }
                    else
                    {
                        propertyInfo.SetValue(obj, null);
                    }
                }
                else
                {
                    propertyInfo.SetValue(obj, null);
                }
            }
            return obj;
        }

        public int Album_ThemMoi(AlbumModel obj)
        {
            int ID = 0;
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idnhanvien", obj.ID_NhanVien),
                new SqlParameter("@idkhachang", obj.ID_KhachHang),
                new SqlParameter("@idcongty", obj.ID_QLLH),
                new SqlParameter("@hinhdaidien", obj.HinhDaiDien),
                new SqlParameter("@kinhdo", obj.KinhDo),
                new SqlParameter("@vido", obj.ViDo),
                new SqlParameter("@ghichu", obj.GhiChu),
                 new SqlParameter("@diachi", obj.DiaChi),
                 new SqlParameter("@loai", obj.Loai),
                 new SqlParameter("@idloaialbum", obj.ID_LoaiAlbum),
                 new SqlParameter("@id_tansuatviengtham", obj.ID_TanSuatViengTham),
                 new SqlParameter("@ngayviengtham", obj.NgayViengTham),
                 new SqlParameter("@id_xe_lichsubd", obj.ID_Xe_LichSuBD),
                 new SqlParameter("@iddonhang", obj.ID_DonHang),
                 new SqlParameter("@idcongviec", obj.ID_CongViec),
                 new SqlParameter("@idcheckin", obj.ID_CheckIn),
            };
                object objID = helper.ExecuteScalar("sp_Album_ThemMoi", par);
                if (objID != null)
                {
                    ID = int.Parse(objID.ToString());
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return ID;
        }

        public AlbumModel GetAlbumById(int ID_Album)
        {
            AlbumModel rs = new AlbumModel();
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@ID_Album", ID_Album)
            };
                DataTable dt = helper.ExecuteDataSet("sp_Album_GetById", par).Tables[0];
                DateTime d;
                foreach (DataRow dr in dt.Rows)
                {
                    rs = GetAlbumModelFromDataRow(dr);
                }
                return rs;
            }
            catch (Exception ex)
            {

                return rs;
            }

        }

    }
}
