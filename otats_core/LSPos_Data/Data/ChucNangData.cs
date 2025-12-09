using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LSPos_Data.Data
{
    public class ChucNangData
    {
        private SqlDataHelper helper;
        public ChucNangData()
        {
            helper = new SqlDataHelper();
        }

        public List<MenuModels> getDanhSachQuyen_WEB(int IDQLLH)
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", IDQLLH)
            };

            DataSet ds = helper.ExecuteDataSet("sp_ChucNang_GetDanhSachChucNang_Web_TheoCongTy", pars);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return null;

            try
            {
                List<MenuModels> dsdv = new List<MenuModels>();
                foreach (DataRow dr in dt.Rows)
                {
                    MenuModels dv = new MenuModels();
                    dv = DataFromDataRow(dr);
                    dsdv.Add(dv);
                }

                return dsdv;
            }
            catch
            {
                return null;
            }
        }

        public List<ChucNangModels> getallchucnangbycongty(int IDQLLH)
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", IDQLLH)
            };

            DataSet ds = helper.ExecuteDataSet("usp_getallchucnangbycongty", pars);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return null;

            try
            {
                List<ChucNangModels> dsdv = new List<ChucNangModels>();
                foreach (DataRow dr in dt.Rows)
                {
                    ChucNangModels dv = new ChucNangModels();
                    dv = GetDataFromDataRow(dr);
                    dsdv.Add(dv);
                }

                return dsdv;
            }
            catch
            {
                return null;
            }
        }
        public List<MenuModels> getmenu(int IDQLLH)
        {
            SqlParameter[] pars = new SqlParameter[] {
                new SqlParameter("ID_QLLH", IDQLLH)
            };

            DataSet ds = helper.ExecuteDataSet("usp_getallchucnangbycongty_V2", pars);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return null;

            try
            {
                List<MenuModels> dsdv = new List<MenuModels>();
                foreach (DataRow dr in dt.Rows)
                {
                    MenuModels dv = new MenuModels();
                    dv = DataFromDataRow(dr);
                    dsdv.Add(dv);
                }

                return dsdv;
            }
            catch
            {
                return null;
            }
        }
        public MenuModels DataFromDataRow(DataRow dr)
        {
            try
            {
                MenuModels dv = new MenuModels();
                dv.Id = int.Parse(dr["ID_ChucNang"].ToString());
                dv.ParentId = int.Parse(dr["ID_NhomChucNang"].ToString());
                dv.Name = dr["TenChucNang"].ToString();
                dv.Url = dr["URL"].ToString();
                dv.URL_NEW = dr["URL_NEW"].ToString();
                dv.icon = dr["icon"].ToString().Trim();

                return dv;
            }
            catch
            {
                return null;
            }
        }
        public ChucNangModels GetDataFromDataRow(DataRow dr)
        {
            try
            {
                ChucNangModels dv = new ChucNangModels();
                dv.ID_ChucNang = int.Parse(dr["ID_ChucNang"].ToString());
                dv.ID_NhomChucNang = int.Parse(dr["ID_NhomChucNang"].ToString());
                dv.TenChucNang = dr["TenChucNang"].ToString();

                //try
                //{
                //    dv.TenNhomChucNang = dr["TenNhomChucNang"].ToString();
                //}
                //catch (Exception)
                //{
                //    dv.TenNhomChucNang = "";
                //}

                //dv.MaChucNang = dr["MaChucNang"].ToString();
                dv.URL = dr["URL"].ToString();
                //dv.InsertedTime = DateTime.Parse(dr["InsertedTime"].ToString());
                dv.icon = dr["icon"].ToString().Trim();
                dv.iconparent = dr["iconparent"].ToString().Trim();

                return dv;
            }
            catch
            {
                return null;
            }
        }

        public List<MenuModels> GetChucNangID_QuanLy(int ID_Quanly)
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_Quanly", ID_Quanly)
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_GetChucNangTheoIDQuanLy_v3", pars);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return null;
            try
            {
                List<MenuModels> dsChucNang = new List<MenuModels>();
                foreach (DataRow dr in dt.Rows)
                {
                    MenuModels dv = new MenuModels();
                    dv = DataFromDataRow(dr);
                    dsChucNang.Add(dv);
                }

                return dsChucNang;
            }
            catch
            {
                return null;
            }
        }

        public List<MenuModels> GetChucNangID_QuanLy_v1(int ID_Quanly)
        {
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@ID_Quanly", ID_Quanly)
        };

            DataSet ds = helper.ExecuteDataSet("sp_QL_GetChucNangTheoIDQuanLy_v4", pars);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count == 0)
                return null;
            try
            {
                List<MenuModels> dsChucNang = new List<MenuModels>();
                foreach (DataRow dr in dt.Rows)
                {
                    MenuModels dv = new MenuModels();
                    dv = DataFromDataRow(dr);
                    dsChucNang.Add(dv);
                }

                return dsChucNang;
            }
            catch
            {
                return null;
            }
        }
    }
}