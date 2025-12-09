using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSPos_Data.DataAccess
{
    public class ChiTietMatHang_DichVuDAO
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(SiteDAO));
        public static SqlDataHelper db = new SqlDataHelper();
        public List<ChiTietMatHang_DichVuModel> GetAllByChiTietDonHang(int ID_ChiTietDonHang)
        {
            List<ChiTietMatHang_DichVuModel> rs = new List<ChiTietMatHang_DichVuModel>();
            SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@ID_ChiTietDonHang", ID_ChiTietDonHang),

            };
            try
            {
                DataTable dt = db.ExecuteDataSet("sp_ChiTietMatHang_DichVu_GetByChiTietDonHang", param).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    ChiTietMatHang_DichVuModel item = GetObjectFromDataRowUtil<ChiTietMatHang_DichVuModel>.ToOject(dr);
                    rs.Add(item);
                }
                return rs;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return rs;
            }
        }

        public bool InsertOrUpdate(ChiTietMatHang_DichVuModel item)
        {
            bool ret = false;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {  

                    new SqlParameter("ID", item.ID),
                    new SqlParameter("ID_ChiTietDonHang", item.ID_ChiTietDonHang),
                    new SqlParameter("ID_DonHang", item.ID_DonHang),
                    new SqlParameter("ID_MatHang", item.ID_MatHang),
                    new SqlParameter("ID_DichVu", item.ID_DichVu),
                    new SqlParameter("Loai", item.Loai),
                    new SqlParameter("SoLuong", item.SoLuong),
                    new SqlParameter("GiaBan", item.GiaBan),
                    new SqlParameter("BookingCode", item.BookingCode),
                    new SqlParameter("InvoiceCode", item.InvoiceCode)
                };
                int i = db.ExecuteNonQuery("sp_ChiTietMatHang_DichVu_InsertOrUpdate", pars);
                if (i > 0)
                {
                    ret = true;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return ret;

        }

        public bool InsertLS(int ID_ChiTietDonHang)
        {
            bool ret = false;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {

                    new SqlParameter("ID_ChiTietDonHang", ID_ChiTietDonHang),
                };
                int i = db.ExecuteNonQuery("sp_ChiTietMatHang_DichVu_LS_Insert", pars);
                if (i > 0)
                {
                    ret = true;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return ret;

        }

        public bool Delete(int ID)
        {
            bool ret = false;
            try
            {
                SqlParameter[] pars = new SqlParameter[] {

                    new SqlParameter("@ID", ID)
                };
                int i = db.ExecuteNonQuery("sp_ChiTietMatHang_DichVu_Delete", pars);
                if (i > 0)
                {
                    ret = true;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return ret;

        }
    }
}
