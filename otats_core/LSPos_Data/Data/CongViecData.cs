using Kendo.DynamicLinq;
using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using static LSPos_Data.Data.KhachHangData;

namespace LSPos_Data.Data
{
    public class RequestGridParam
    {
        public DataSourceRequest request { get; set; }
        public int ID_Loai { get; set; }
    }

    public class FilterGrid
    {
        public DateTime Ngay { get; set; }
        public string TenNguoiGui { get; set; }
        public string NhomNhanVien { get; set; }
        public string NguoiDuocGiao { get; set; }
        public string TenCongViec { get; set; }
        public string NoiDung { get; set; }
        public string DiaDiemDi { get; set; }
        public string DiaDiemDen { get; set; }
        public DateTime NgayHetHan { get; set; }
        public DateTime NgayNhan { get; set; }
        public DateTime NgayHoanThanh { get; set; }
        public string TrangThai { get; set; }

    }
    public class CongViecData
    {
        private SqlDataHelper helper;
        private static SqlDataHelper db = new SqlDataHelper();

        public CongViecData()
        {
            helper = new SqlDataHelper();
        }

        private CongViecModel GetObjDataRow(DataRow dr)
        {
            CongViecModel obj = new CongViecModel();
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

        public int Add(CongViecModel congViec)
        {

            DateTime ngayHetHan = congViec.NgayHetHan;

            if (ngayHetHan.Year < 1900)
                ngayHetHan = new DateTime(1900, 1, 1);

            SqlParameter[] Parammeter = new SqlParameter[] {
                new SqlParameter("@ID_QLLH", congViec.ID_QLLH),
                new SqlParameter("@ID_NguoiGui", congViec.ID_NguoiGui),
                new SqlParameter("@ID_NhomNhan", congViec.ID_NhomNhan),
                new SqlParameter("@TenCongViec", congViec.TenCongViec),
                new SqlParameter("@NoiDung", congViec.NoiDung),
                new SqlParameter("@DiaDiemDi", congViec.DiaDiemDi),
                new SqlParameter("@DiaDiemDen", congViec.DiaDiemDen),
                new SqlParameter("@BatBuocCheckin", congViec.BatBuocCheckin),
                new SqlParameter("@BatBuocChupAnh", congViec.BatBuocChupAnh),
                new SqlParameter("@BatBuocDeadline", congViec.BatBuocDeadline),
                new SqlParameter("@SoTien", congViec.SoTien),
                new SqlParameter("@SoTienHang", congViec.SoTienHang),
                new SqlParameter("@SoNguoiNhan", congViec.SoNguoiNhan),
                new SqlParameter("@NgayHetHan", ngayHetHan),
                new SqlParameter("@KinhDoDiemDi", congViec.KinhDoDiemDi),
                new SqlParameter("@KinhDoDiemDen", congViec.KinhDoDiemDen),
                new SqlParameter("@ViDoDiemDi", congViec.ViDoDiemDi),
                new SqlParameter("@ViDoDiemDen", congViec.ViDoDiemDen),
                };

            int result = 0;

            try
            {
                object id = helper.ExecuteScalar("usp_vuongtm_CongViec_insert_v2", Parammeter);
                result = int.Parse(id.ToString());
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return result > 0 ? result : 0;
        }
        public int ThemGiaoViec(int ID_CongViec, int ID_NhanVien, int ID_QuanLy)
        {


            SqlParameter[] Parammeter = new SqlParameter[] {
                new SqlParameter("@idcongviec", ID_CongViec),
                new SqlParameter("@idnguoinhan", ID_NhanVien),
                new SqlParameter("@ID_QuanLy", ID_QuanLy),

            };

            int result = 0;

            try
            {
                result = helper.ExecuteNonQuery("sp_AppKsmartS_ThemGiaoViec", Parammeter);

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return result;
        }
        public bool Update(CongViecModel congViec)
        {
            DateTime ngayHetHan = congViec.NgayHetHan;

            if (ngayHetHan.Year < 1900)
                ngayHetHan = new DateTime(1900, 1, 1);

            SqlParameter[] Parammeter = new SqlParameter[]
            {
                new SqlParameter("@ID_CongViec", congViec.ID_CongViec),
                new SqlParameter("@ID_QLLH", congViec.ID_QLLH),
                new SqlParameter("@ID_NguoiGui", congViec.ID_NguoiGui),
                new SqlParameter("@ID_NhomNhan", congViec.ID_NhomNhan),
                 new SqlParameter("@TenCongViec", congViec.TenCongViec),
                new SqlParameter("@NoiDung", congViec.NoiDung),
                new SqlParameter("@DiaDiemDi", congViec.DiaDiemDi),
                new SqlParameter("@DiaDiemDen", congViec.DiaDiemDen),
                new SqlParameter("@BatBuocCheckin", congViec.BatBuocCheckin),
                new SqlParameter("@BatBuocChupAnh", congViec.BatBuocChupAnh),
                new SqlParameter("@BatBuocDeadline", congViec.BatBuocDeadline),
                new SqlParameter("@SoTien", congViec.SoTien),
                new SqlParameter("@SoNguoiNhan", congViec.SoNguoiNhan),
                new SqlParameter("@NgayHetHan", ngayHetHan),
                  new SqlParameter("@KinhDoDiemDi", congViec.KinhDoDiemDi),
                new SqlParameter("@KinhDoDiemDen", congViec.KinhDoDiemDen),
                new SqlParameter("@ViDoDiemDi", congViec.ViDoDiemDi),
                new SqlParameter("@ViDoDiemDen", congViec.ViDoDiemDen),
            };

            int result = 0;

            try
            {
                result = helper.ExecuteNonQuery("usp_vuongtm_CongViec_update_v2", Parammeter);
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return result > 0 ? true : false;
        }

        public DataSet GetMulti(int id_QLLH, int loai = 0)
        {
            SqlParameter[] Param = new SqlParameter[]
            {
                new SqlParameter("@id_qllh", id_QLLH),
                new SqlParameter("@loai", loai)
            };

            DataSet ds = new DataSet();
            try
            {
                ds = helper.ExecuteDataSet("usp_vuongtm_CongViec_GetList", Param);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataSet DanhSachCongViec(int id_QLLH, int loai)
        {
            SqlParameter[] Param = new SqlParameter[]
            {
                new SqlParameter("@id_qllh", id_QLLH),
                new SqlParameter("@loai", loai)
            };

            DataSet ds = new DataSet();
            try
            {
                ds = helper.ExecuteDataSet("usp_vuongtm_CongViec_GetDanhSach", Param);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }
        public DataSet DanhSachCongViec_gridtab(int id_QLLH)
        {
            SqlParameter[] Param = new SqlParameter[]
            {
                new SqlParameter("@id_qllh", id_QLLH)
            };

            DataSet ds = new DataSet();
            try
            {
                ds = helper.ExecuteDataSet("usp_vuongtm_CongViec_GetDanhSach_Grid", Param);
            }
            catch (Exception)
            {
                return null;
            }
            return ds;
        }

        public DataTable ChiTietCongViecTheoId(int id_CongViec)
        {
            DataTable dt = new DataTable();

            SqlParameter[] Parammeter = new SqlParameter[] {
                new SqlParameter("id_CongViec", id_CongViec)
                };

            try
            {
                DataSet ds = helper.ExecuteDataSet("usp_vuongtm_ChiTietCongViecTheoId", Parammeter);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return dt;
        }
        public DataTable ChiTietCongViecTheoId_v2(int id_CongViec, int ID_QuanLy)
        {
            DataTable dt = new DataTable();

            SqlParameter[] Parammeter = new SqlParameter[] {
                   new SqlParameter("ID_QuanLy", ID_QuanLy),
                new SqlParameter("id_CongViec", id_CongViec)
                };

            try
            {
                DataSet ds = helper.ExecuteDataSet("usp_vuongtm_ChiTietCongViecTheoId_v2", Parammeter);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return dt;
        }
        public List<CongViecModel> DanhSachCongViec_Kendo(int id_QLLH, int loai, int skip, int take, FilterGrid filter, ref int TongSo)
        {
            SqlParameter[] Param = new SqlParameter[]
            {
                new SqlParameter("@id_qllh", id_QLLH),
                new SqlParameter("@loai", loai),
                new SqlParameter("@skip", skip),
                new SqlParameter("@take", take),
                new SqlParameter("@ngay", filter.Ngay),
                new SqlParameter("@nguoiguicongviec", filter.TenNguoiGui),
                new SqlParameter("@nhomnhancongviec", filter.NhomNhanVien),
                new SqlParameter("@nguoiduocgiao", filter.NguoiDuocGiao),
                new SqlParameter("@tencongviec", filter.TenCongViec),
                new SqlParameter("@noidungcongviec", filter.NoiDung),
                new SqlParameter("@diadiemdi", filter.DiaDiemDi),
                new SqlParameter("@diadiemden", filter.DiaDiemDen),
                new SqlParameter("@ngayhethan", filter.NgayHetHan),
                new SqlParameter("@ngaynhan", filter.NgayNhan),
                new SqlParameter("@ngayhoanthanh", filter.NgayHoanThanh),
                new SqlParameter("@trangthai", filter.TrangThai)

            };

            DataSet ds = new DataSet();
            List<CongViecModel> listcongviec = new List<CongViecModel>();
            try
            {
                ds = helper.ExecuteDataSet("CongViec_GetDanhSach_KenDo", Param);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        listcongviec.Add(GetObjDataRow(dr));
                    }
                    TongSo = int.Parse(ds.Tables[1].Rows[0]["SoLuong"].ToString());
                }
            }
            catch (Exception)
            {
                return listcongviec;
            }
            return listcongviec;
        }

        public CongViecModel GetById(int id)
        {
            SqlParameter[] Param = new SqlParameter[]
            {
                new SqlParameter("id", id)
            };

            DataSet ds = helper.ExecuteDataSet("usp_vuongtm_CongViec_GetById", Param);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
                return GetObjDataRow(dt.Rows[0]);
            else return null;
        }

        public List<NhanVien> ListNguoiNhanCongViec(int id)
        {
            SqlParameter[] Param = new SqlParameter[]
            {
                new SqlParameter("id", id)
            };
            List<NhanVien> lnv = new List<NhanVien>();
            NhanVien_dl nvdl = new NhanVien_dl();
            DataSet ds = helper.ExecuteDataSet("usp_sontq_CongViec_GetIdNguoiNhanById", Param);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int idnv = int.Parse(dr[0].ToString());
                    lnv.Add(nvdl.GetNVTheoID(idnv));
                }
            }
            return lnv;
        }

        public DataTable ChiTietTrangThaiCongViec(int ID)
        {
            SqlParameter[] Param = new SqlParameter[]
            {
                new SqlParameter("ID", ID)
            };
            DataSet ds = helper.ExecuteDataSet("sp_sontq_CongViec_ChiTietTrangThaiCongViec", Param);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
                return dt;
            else return null;
        }
    }
}