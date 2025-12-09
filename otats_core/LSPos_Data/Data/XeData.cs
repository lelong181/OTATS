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
    public class XeData
    {
        private SqlDataHelper helper;
        public XeData()
        {
            helper = new SqlDataHelper();
        }

        public List<XeDTO> GetDataFromRow(DataTable dt)
        {
            List<XeDTO> result = new List<XeDTO>();
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    XeDTO x = new XeDTO();
                    x.ID = (dr["ID_Xe"] != null) ? int.Parse(dr["ID_Xe"].ToString()) : 0;
                    x.BienKiemSoat = (dr["BienKiemSoat"] != null) ? dr["BienKiemSoat"].ToString() : "";
                    x.TenNhanVien = (dr["TenNhanVien"] != null) ? dr["TenNhanVien"].ToString() : "";
                    x.NamSX = (dr["NamSanXuat"] != null) ? int.Parse(dr["NamSanXuat"].ToString()) : 0;
                    x.NgayBDGanNhat = dr["NgayBDGanNhat"].ToString() == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["NgayBDGanNhat"]);
                    x.NgayBDTiepTheo = dr["NgayBDTiepTheo"].ToString() == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["NgayBDTiepTheo"]);
                    x.ChuKy = (dr["ChuKyBaoDuong"] != null) ? int.Parse(dr["ChuKyBaoDuong"].ToString()) : 0;
                    x.SoCho = (dr["SoCho"] != null) ? int.Parse(dr["SoCho"].ToString()) : 0;
                    x.LoaiXe = (dr["LoaiXe"] != null) ? dr["LoaiXe"].ToString() : "";
                    x.MoTa = (dr["MoTa"] != null) ? dr["MoTa"].ToString() : "";
                    result.Add(x);
                }
                catch
                {

                }
            }
            return result;
        }

        public List<XeDTO> GetDSXe(int ID_QLLH, string BienKiemSoat, int NamSX,string TenNhanVien,int SoCho, DateTime NgayBDGanNhat, DateTime NgayDBTiepTheo, ref int TongSo)
        {
            DataSet ds = new DataSet();
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_QLLH", ID_QLLH),
            new SqlParameter("@BienKiemSoat", BienKiemSoat),
            new SqlParameter("@NamSX", NamSX),
            new SqlParameter("@TenNhanVien", TenNhanVien),
            new SqlParameter("@SoCho", SoCho),
            new SqlParameter("@NgayBDGanNhat", NgayBDGanNhat),
            new SqlParameter("@NgayDBTiepTheo", NgayDBTiepTheo)
        };
            List<XeDTO> result = new List<XeDTO>();

            try
            {
                ds = helper.ExecuteDataSet("sp_Xe_GetAllKenDo", pars);
                if (ds.Tables[1].Rows.Count > 0)
                {
                    TongSo = int.Parse(ds.Tables[1].Rows[0]["TongSo"].ToString());
                }
                else
                {
                    TongSo = 0;
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        XeDTO x = new XeDTO();
                        x.ID = (dr["ID_Xe"] != null) ? int.Parse(dr["ID_Xe"].ToString()) : 0;
                        x.BienKiemSoat = (dr["BienKiemSoat"] != null) ? dr["BienKiemSoat"].ToString() : "";
                        x.TenNhanVien = (dr["TenNhanVien"] != null) ? dr["TenNhanVien"].ToString() : "";
                        x.NamSX = (dr["NamSanXuat"] != null) ? int.Parse(dr["NamSanXuat"].ToString()) : 0;
                        x.NgayBDGanNhat = dr["NgayBDGanNhat"].ToString() == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["NgayBDGanNhat"]);
                        x.NgayBDTiepTheo = dr["NgayBDTiepTheo"].ToString() == "" ? new DateTime(1900, 1, 1) : Convert.ToDateTime(dr["NgayBDTiepTheo"]);
                        x.ChuKy = (dr["ChuKyBaoDuong"] != null) ? int.Parse(dr["ChuKyBaoDuong"].ToString()) : 0;
                        x.SoCho = (dr["SoCho"] != null) ? int.Parse(dr["SoCho"].ToString()) : 0;
                        x.LoaiXe = (dr["LoaiXe"] != null) ? dr["LoaiXe"].ToString() : "";
                        x.MoTa = (dr["MoTa"] != null) ? dr["MoTa"].ToString() : "";
                        result.Add(x);
                    }
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return result;
            }
            return result;
        }
    }
}