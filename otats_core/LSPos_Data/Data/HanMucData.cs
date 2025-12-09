using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class HanMucData
    {
        private SqlDataHelper helper;
        public HanMucData()
        {
            helper = new SqlDataHelper();
        }

        public DataTable GetDSNhanVien_TheoNhomQuanLy(int ID_QLLH, int ID_Nhom)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] Parammeter = new SqlParameter[] {
                    new SqlParameter("ID_QLLH", ID_QLLH),
                    new SqlParameter("ID_Nhom", ID_Nhom)
                };

                DataSet ds = helper.ExecuteDataSet("sp_vuongtm_QL_GetDSNhanVien_TheoNhom", Parammeter);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }

        public bool SetNguongCongNoNV(double congno, int id)
        {
            bool flag = true;
            try
            {
                int rowaffect = helper.ExecuteNonQuery("Update NhanVien set CongNoChoPhep = " + congno + " where ID_NhanVien = " + id);
                if (rowaffect <= 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return flag;
        }
        public bool SetNguongCongNoNhomKhachHang(double congno, int id)//copy từ Ksmart_DataSon
        {
            bool flag = true;
            try
            {
                int rowaffect = helper.ExecuteNonQuery("Update LoaiKhachHang set CongNoChoPhep = " + congno + " where ID_LoaiKhachHang = " + id);
                if (rowaffect <= 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            
            return flag;
        }

        public bool SetNguongCongNoNhomNV(double congno, int id)//copy từ Ksmart_DataSon
        {
            bool flag = true;
            try
            {
                int rowaffect = helper.ExecuteNonQuery("Update Nhom set CongNoGioiHan = " + congno + " where ID_Nhom = " + id);
                if (rowaffect <= 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return flag;
             
        }

        public DataTable GetListNguongCongNo_NV(int ID_QLLH)
        {
            DataTable dt = new DataTable();
            try
            {
                DataSet ds = helper.ExecuteDataSet("Select * from Nhom where ID_QLLH ='" + ID_QLLH + "' ");
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return dt;
        }
    }
}