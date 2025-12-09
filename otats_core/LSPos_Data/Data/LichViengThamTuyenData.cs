using Ksmart_DataSon.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LSPos_Data.Data
{
    public class LichViengThamTuyenData
    {
        private SqlDataHelper helper;
        public LichViengThamTuyenData()
        {
            helper = new SqlDataHelper();
        }

        public DataTable GetAllTuanSuatTuyen(int ID_QLLH)
        {
            DataSet ds = helper.ExecuteDataSet("Select * from Tuyen left join TanSuatViengThamTuyen on Tuyen.ID = TanSuatViengThamTuyen.Id_Tuyen where Tuyen.ID_QLLH = '" + ID_QLLH + "'");
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetTanSuatTuyenByID(int ID_Tuyen)
        {
            DataSet ds = helper.ExecuteDataSet("Select * from TanSuatViengThamTuyen ts where ts.ID_Tuyen = '" + ID_Tuyen + "'");
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }


        public List<DateTime> GetAllLichForTuyen(int IDQLLH, int ID_Tuyen)
        {
            DataSet ds = helper.ExecuteDataSet(string.Format("Select distinct(NgayViengTham_KeHoach_BatDau) from KeHoachViengThamTuyen where ID_Tuyen = {0} and ID_QLLH = {1}", ID_Tuyen, IDQLLH));
            DataTable dt = ds.Tables[0];
            List<DateTime> result = new List<DateTime>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    result.Add(Convert.ToDateTime(dr["NgayViengTham_KeHoach_BatDau"].ToString()));
                }
                return result;
            }
            else
            {
                return result;
            }
        }

        public bool TaoLich(int IDQLLH, int idtuyen, int loai, int tansuat, string ngay, string ghichu, DateTime ngayketthuc)
        {
            object i = helper.ExecuteScalar("Select Id from TanSuatViengThamTuyen where ID_Tuyen = " + idtuyen);
            if (i == null)
            {
                string sql = string.Format(@"Insert into TanSuatViengThamTuyen(ID_Tuyen,ID_QLLH,ID_LoaiTanSuat,TanSuat,CacNgayThucHien,TrangThai,GhiChu,NgayKetThuc) values(
                                    {0},{1},{2},{3},'{4}',1,N'{5}','{6}')", idtuyen, IDQLLH, loai, tansuat, ngay, ghichu, ngayketthuc.ToString("yyyy-MM-dd hh:mm:ss:fff"));
                int rowaff = helper.ExecuteNonQuery(sql);
                if (rowaff > 0)
                {
                    return true;
                }
            }
            else
            {
                string sql = string.Format(@"Update TanSuatViengThamTuyen Set ID_LoaiTanSuat = {1},TanSuat = {2},CacNgayThucHien = '{3}',GhiChu = N'{4}', NgayKetThuc = '{5}' where ID_Tuyen = {0}", idtuyen, loai, tansuat, ngay, ghichu, ngayketthuc.ToString("yyyy-MM-dd hh:mm:ss:fff"));
                int rowaff = helper.ExecuteNonQuery(sql);
                if (rowaff > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool InsertLich(int idqllh, int idtuyen, DateTime ngayd)
        {
            try
            {
                Tuyen_Data tdt = new Tuyen_Data();
                DataTable nhanvien = tdt.GetNhanVienTuyen(idtuyen);
                if (nhanvien.Rows.Count > 0)
                {
                    /**
                     * Code cũ của Sơn bị sai trường hợp khi thêm nhân viên mới vào tuyến, thì các kế hoạch cũ đã có ngày nó sẽ không thêm đc vào nhân viên mới đó
                     * 
                    //object i = helper.ExecuteScalar("Select ID_KeHoachViengThamTuyen from KeHoachViengThamTuyen where ID_Tuyen = " + idtuyen + " and NgayViengTham_KeHoach_BatDau = '" + ngayd.ToString("yyyy-MM-dd") + "'");
                    //if (i == null || idtuyen == 0)
                    //{
                    //    foreach (DataRow dr in nhanvien.Rows)
                    //    {
                    //        int ID_NhanVien = int.Parse(dr["ID_NhanVien"].ToString());

                    //        string sql = string.Format(@"Insert into KeHoachViengThamTuyen(ID_QLLH,ID_Tuyen,NgayViengTham_KeHoach_BatDau,NgayViengTham_KeHoach_KetThuc,TrangThai,ID_NhanVien) values(
                    //                {0},{1},'{2}','{3}',{4},{5})", idqllh, idtuyen, ngayd.ToString("yyyy-MM-dd") + " 00:00:00", ngayd.ToString("yyyy-MM-dd") + " 23:59:00", 0, ID_NhanVien);
                    //        int rowaff = helper.ExecuteNonQuery(sql);

                    //    }
                    //}
                    //else
                    //{

                    //    return false;
                    //}

                     */

                    /**
                     * Trường NM fix ngày 10-10 sửa lại cho bug 1087 của Sơn : 
                     */
                    string strIDNV = "(";
                    int x = 0;
                    foreach (DataRow dr in nhanvien.Rows)
                    {
                        x++;
                        int ID_NhanVien = int.Parse(dr["ID_NhanVien"].ToString());
                        if (x == nhanvien.Rows.Count)
                        {
                            strIDNV += ID_NhanVien;
                        }
                        else
                        {
                            strIDNV += ID_NhanVien + ",";
                        }
                       
                        object i = helper.ExecuteScalar("Select ID_KeHoachViengThamTuyen from KeHoachViengThamTuyen where ID_NhanVien = '" + ID_NhanVien + "' AND ID_Tuyen = " + idtuyen + " and NgayViengTham_KeHoach_BatDau = '" + ngayd.ToString("yyyy-MM-dd") + "'");
                        if (i == null || idtuyen == 0)
                        {

                            string sql = string.Format(@"Insert into KeHoachViengThamTuyen(ID_QLLH,ID_Tuyen,NgayViengTham_KeHoach_BatDau,NgayViengTham_KeHoach_KetThuc,TrangThai,ID_NhanVien) values(
                                    {0},{1},'{2}','{3}',{4},{5})", idqllh, idtuyen, ngayd.ToString("yyyy-MM-dd") + " 00:00:00", ngayd.ToString("yyyy-MM-dd") + " 23:59:00", 0, ID_NhanVien);
                            int rowaff = helper.ExecuteNonQuery(sql);
                        }
                        

                    }

                    strIDNV += ")";
                    if (x > 0)
                    {
                        //TRUONGNM : xóa kế hoạch các nhân viên mà trên quản lý đã xóa, trước không có đoạn xóa :((
                        helper.ExecuteNonQuery("delete from KeHoachViengThamTuyen where ID_NhanVien NOT IN  " + strIDNV + "  AND ID_Tuyen = " + idtuyen + " and NgayViengTham_KeHoach_BatDau = '" + ngayd.ToString("yyyy-MM-dd") + "'");
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

