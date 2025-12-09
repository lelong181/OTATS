using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.DataAccess
{
    public class DonHang_DichVuRequestAPIDAO
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(DichVuDAO));
        public static SqlDataHelper db = new SqlDataHelper();
        public DonHang_DichVuRequestAPIDAO()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public List<DonHang_DichVuRequestAPIModel> GetAllByDonHang(long ID_DonHang)
        {
            List<DonHang_DichVuRequestAPIModel> rs = new List<DonHang_DichVuRequestAPIModel>();

            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                new SqlParameter("@ID_DonHang", ID_DonHang),

                };
                DataTable dt = db.ExecuteDataSet("sp_DichVuDonHang_GetByDonHang", param).Tables[0];
                SiteDAO sdao = new SiteDAO();
                DichVuDAO dvdao = new DichVuDAO();
                foreach (DataRow dr in dt.Rows)
                {
                    DonHang_DichVuRequestAPIModel item = GetObjectFromDataRowUtil<DonHang_DichVuRequestAPIModel>.ToOject(dr);
                    item.DichVu = dvdao.GetByID(item.ID_DichVu);
                    item.Site = sdao.GetSite(item.DichVu.SiteCode);
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
        
        
        public List<DonHang_DichVuRequestAPIModel> GetCauHinhFOC()
        {
            List<DonHang_DichVuRequestAPIModel> rs = new List<DonHang_DichVuRequestAPIModel>();

            try
            {
                DataTable dt = db.ExecuteDataSet("sp_GetCauHinhFOC").Tables[0];
                SiteDAO sdao = new SiteDAO();
                DichVuDAO dvdao = new DichVuDAO();
                foreach (DataRow dr in dt.Rows)
                {
                    DonHang_DichVuRequestAPIModel item = GetObjectFromDataRowUtil<DonHang_DichVuRequestAPIModel>.ToOject(dr);
                    item.DichVu = dvdao.GetByID(item.ID_DichVu);
                    item.Site = sdao.GetSite(item.DichVu.SiteCode);
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
    }
}
