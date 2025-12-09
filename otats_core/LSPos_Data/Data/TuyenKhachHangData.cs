using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.Data
{
    public class TuyenKhachHangData
    {
        private SqlDataHelper helper;
        public TuyenKhachHangData()
        {
            helper = new SqlDataHelper();
        }

        public int XoaKeHoachDiChuyen(int ID_QuanLy, int ID_KeHoach)
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_KeHoach", ID_KeHoach),
            new SqlParameter("@ID_QuanLy", ID_QuanLy)
        };
            try
            {
                return helper.ExecuteNonQuery("sp_QL_XoaKeHoach_vuongtm_v1", pars);
            }
            catch
            {
                return 0;
            }
        }
        public DataTable GetDsKhachHangTheoTuyen(int ID_QLLH, int ID_QuanLy, int ID_Tuyen)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@id_qllh", ID_QLLH),
            new SqlParameter("@ID_QuanLy", ID_QuanLy),
            new SqlParameter("@ID_Tuyen", ID_Tuyen)
        };
            try
            {
                ds = helper.ExecuteDataSet("sp_QL_getDSKhachHangTheoTuyen", pars);
            }
            catch (Exception)
            {
                return null;
            }
            return ds.Tables[0];
        }

        private TuyenDTO GetDeObjDataRow(DataRow dr)
        {
            TuyenDTO obj = new TuyenDTO();
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

        public DataSet GetdsTuyenKhachHang(int ID_QLLH)
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", ID_QLLH)
            };
            try
            {
                DataSet ds = helper.ExecuteDataSet("sp_TuyenKhachHang_GetAll", pars);
                return ds;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<TuyenDTO> GetAll(int ID_QLLH)
        {
            List<TuyenDTO> lt = new List<TuyenDTO>();

            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", ID_QLLH)
            };
            try
            {
                DataSet ds = helper.ExecuteDataSet("sp_TuyenKhachHang_GetAll", pars);
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    lt.Add(GetDeObjDataRow(dr));
                }
            }
            catch (Exception)
            {
                return null;
            }

            return lt;
        }

        public List<TuyenDTO> GetAllSoLuongKhachHang(int ID_QLLH)
        {
            DataSet ds = helper.ExecuteDataSet(string.Format(@"Select Tuyen.*,count(KhachHang_Tuyen.ID_Tuyen) as SoLuongKhachHang from Tuyen 
                                                               left join KhachHang_Tuyen on KhachHang_Tuyen.ID_Tuyen = Tuyen.ID
                                                                 where ID_QLLH = {0}
                                                                group by Tuyen.ID, Tuyen.ID_NhanVien, TUyen.ID_QLLH, Tuyen.TenTuyen, Tuyen.MoTa
                                                                ", ID_QLLH));
            DataTable dt = ds.Tables[0];
            List<TuyenDTO> lt = new List<TuyenDTO>();
            foreach (DataRow dr in dt.Rows)
            {
                lt.Add(GetDeObjDataRow(dr));
            }
            foreach (TuyenDTO t in lt)
            {
                t.TenTuyen += " (" + t.SoLuongKhachHang + ") ";
            }
            return lt;
        }



        public TuyenDTO GetByID(int id)
        {
            DataSet ds = helper.ExecuteDataSet("Select t.*,ts.ID_LoaiTanSuat,ts.CacNgayThucHien,ts.NgayKetThuc from Tuyen t left join TanSuatViengThamTuyen ts on t.ID = ts.Id_Tuyen where t.ID = " + id);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
                return GetDeObjDataRow(dt.Rows[0]);
            else return null;
        }

        public int CreateTuyen(TuyenDTO t)
        {
            object rowaff = helper.ExecuteScalar("Insert into Tuyen(TenTuyen,MoTa,ID_NhanVien,ID_QLLH) values(N'" + t.TenTuyen + "',N'" + t.MoTa + "'," + t.ID_NhanVien + "," + t.ID_QLLH + ") Select SCOPE_IDENTITY()");
            return int.Parse(rowaff.ToString());
        }
        public bool UpdateTuyen(TuyenDTO t)
        {
            int rowaff = helper.ExecuteNonQuery("Update Tuyen set TenTuyen = N'" + t.TenTuyen + "', MoTa = N'" + t.MoTa + "', ID_NhanVien = " + t.ID_NhanVien + " where ID = " + t.ID);
            if (rowaff > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteTuyen(int id)
        {
            int rowaff = helper.ExecuteNonQuery("Delete Tuyen  where ID = " + id);
            if (rowaff > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int Insert_TuyenNhanVien(int idqllh, int idtuyen, int idnhanvien)
        {
            int rowaff = helper.ExecuteNonQuery("Insert into Tuyen_NhanVien(ID_QLLH,ID_Tuyen,ID_NhanVien) values(" + idqllh + "," + idtuyen + "," + idnhanvien + ") Select SCOPE_IDENTITY()");
            return rowaff;
        }

        public int Insert_TuyenNhomNhanVien(int idqllh, int idtuyen, int idnhomnhanvien)
        {
            int rowaff = helper.ExecuteNonQuery("Insert into Tuyen_NhomNhanVien(ID_QLLH,ID_Tuyen,ID_NhomNhanVien) values(" + idqllh + "," + idtuyen + "," + idnhomnhanvien + ") Select SCOPE_IDENTITY()");
            return rowaff;
        }

        public int ClearNhanVienTuyen(int idqllh, int idtuyen)
        {
            int rowaff = helper.ExecuteNonQuery("Delete Tuyen_NhanVien where ID_QLLH = " + idqllh + " and ID_Tuyen = " + idtuyen);
            return rowaff;
        }

        public int ClearNhomNhanVienTuyen(int idqllh, int idtuyen)
        {
            int rowaff = helper.ExecuteNonQuery("Delete Tuyen_NhomNhanVien where ID_QLLH = " + idqllh + " and ID_Tuyen = " + idtuyen);
            return rowaff;
        }

        public int ClearKhachHangTuyen(int idtuyen)
        {
            int rowaff = helper.ExecuteNonQuery("Delete KhachHang_Tuyen where ID_Tuyen = " + idtuyen);
            return rowaff;
        }
        public DataTable GetNhanVienTuyen(int idtuyen)
        {
            DataSet ds = helper.ExecuteDataSet("Select t.ID_NhanVien, nv.TenNhanVien from Tuyen_NhanVien t inner join NhanVien nv on t.ID_NhanVien = nv.ID_NhanVien  where t.ID_Tuyen = " + idtuyen);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
                return dt;
            else return null;
        }

        public DataTable GetNhomNhanVienTuyen(int idtuyen)
        {
            DataSet ds = helper.ExecuteDataSet("Select t.ID_NhomNhanVien, n.TenNhom from Tuyen_NhomNhanVien t join Nhom n on t.ID_NhomNhanVien = n.ID_Nhom  where t.ID_Tuyen = " + idtuyen);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
                return dt;
            else return null;
        }


        public List<ComboboxDTO> ComboboxTuyen(int ID_QLLH)
        {
            List<ComboboxDTO> combo = new List<ComboboxDTO>();

            try
            {
                SqlParameter[] Parammeter = new SqlParameter[] {
                new SqlParameter("@ID_QLLH", ID_QLLH)};
                DataSet ds = helper.ExecuteDataSet("sp_getComboboxTuyen", Parammeter);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ComboboxDTO cb = new ComboboxDTO();
                        cb.id = int.Parse(dr["ID"].ToString());
                        cb.Name = dr["TenTuyen"].ToString();
                        combo.Add(cb);
                    }
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
            return combo;
        }
    }
}