using LSPos_Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;


namespace LSPos_Data.Data
{
    public class LoTrinhData
    {

        private SqlDataHelper helper;
        public LoTrinhData()
        {
            helper = new SqlDataHelper();
        }

        public string GetDiaDiemTheoToaDo(double ViDo, double KinhDo)
        {
            //LSPos_Data.Utilities.Log.Info("Vĩ độ: " + ViDo.ToString() + "| Kinh độ: " + KinhDo.ToString());
            #region lấy điểm theo tọa độ
            string diachi = "";
            try
            {
                //AIzaSyBnwO1ETMtZC7AonESIQbpnwNaPvBhqVnI
                string apiKey = "";
                if (ConfigurationManager.AppSettings["GOOGLEAPIKEY"] != null)
                {
                    apiKey = ConfigurationManager.AppSettings["GOOGLEAPIKEY"];
                    //LSPos_Data.Utilities.Log.Info("apiKey: " + apiKey.ToString());
                }
                string url = "https://maps.google.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false&key=" + apiKey;
                url = string.Format(url, ViDo, KinhDo);
                //LSPos_Data.Utilities.Log.Info("url: " + url.ToString());
                WebRequest request = WebRequest.Create(url);
                using (WebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    //LSPos_Data.Utilities.Log.Info("response : " + JsonConvert.SerializeObject(response));
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        DataSet dsResult = new DataSet();
                        dsResult.ReadXml(reader);

                        //LSPos_Data.Utilities.Log.Info("dsResult : " + JsonConvert.SerializeObject(dsResult));

                        diachi = dsResult.Tables["result"].Rows[0]["formatted_address"].ToString();
                        diachi = diachi.Replace("Unnamed Road,", "").Trim();
                        return diachi;
                    }
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return diachi;
            #endregion
        }
        public List<ViTriNhanVienGPSModel> GetViTriTatCaNV(int idcty, int ID_QuanLy, int ID_Nhom, int loctrangthai)
        {
            List<ViTriNhanVienGPSModel> rs = new List<ViTriNhanVienGPSModel>();
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idcty", idcty),
                new SqlParameter("@ID_QuanLy", ID_QuanLy),
                 new SqlParameter("@ID_Nhom", ID_Nhom),
                 new SqlParameter("@loctrangthai", loctrangthai)
            };
                DataTable dt = helper.ExecuteDataSet("sp_QL_getViTriTatCaNV", par).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {

                        rs.Add(new ViTriNhanVienGPSModel
                        {
                            idnhanvien = int.Parse(dr["idnhanvien"].ToString()),
                            tennhanvien = dr["TenNhanVien"].ToString(),
                            KinhDo = double.Parse(dr["KinhDo"].ToString()),
                            ViDo = double.Parse(dr["ViDo"].ToString()),
                            dangtructuyen = int.Parse(dr["dangtructuyen"].ToString()),
                            thoigiancapnhat = DateTime.Parse(dr["ReceivedTime"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                            anhdaidien = dr["anhdaidien"].ToString(),
                            thoigianguitoado = DateTime.Parse(dr["ThoiGianHoatDong"].ToString()).ToString("dd/MM/yyyy HH:mm:ss")
                        });

                    }
                    catch
                    {
                        rs.Add(new ViTriNhanVienGPSModel
                        {
                            tennhanvien = null,
                            KinhDo = 0,
                            ViDo = 0,
                            dangtructuyen = 0,
                            thoigiancapnhat = null,
                            anhdaidien = null,
                            thoigianguitoado = null
                        });
                    }
                }
                return rs;
            }
            catch
            {
                return rs;
            }
        }
        public List<ViTriNhanVienGPSModel> GetViTriTatCaNVOnline(int idcty, int ID_QuanLy, int trangthai, int idnhom)
        {

            List<ViTriNhanVienGPSModel> rs = new List<ViTriNhanVienGPSModel>();
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idcty", idcty),
                new SqlParameter("@ID_QuanLy", ID_QuanLy),
                new SqlParameter("@ID_Nhom", idnhom),
                new SqlParameter("@loctrangthai", trangthai)
            };
                DataTable dt = helper.ExecuteDataSet("sp_QL_getViTriTatCaNV_v1", par).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        rs.Add(new ViTriNhanVienGPSModel
                        {
                            idnhanvien = int.Parse(dr["idnhanvien"].ToString()),
                            tennhanvien = dr["TenNhanVien"].ToString(),
                            KinhDo = double.Parse(dr["KinhDo"].ToString()),
                            ViDo = double.Parse(dr["ViDo"].ToString()),
                            dangtructuyen = int.Parse(dr["dangtructuyen"].ToString()),
                            thoigiancapnhat = DateTime.Parse(dr["ReceivedTime"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                            ID_Nhom = dr["ID_Nhom"].ToString() != "" ? int.Parse(dr["ID_Nhom"].ToString()) : 0,
                            anhdaidien = dr["anhdaidien"].ToString(),
                            thoigianguitoado = DateTime.Parse(dr["ThoiGianHoatDong"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                            TenDangNhap = dr["TenDangNhap"].ToString(),
                            TinhTrangPin = dr["TinhTrangPin"].ToString()
                        });



                    }
                    catch
                    {
                        rs.Add(new ViTriNhanVienGPSModel
                        {
                            tennhanvien = null,
                            KinhDo = 0,
                            ViDo = 0,
                            dangtructuyen = 0,
                            thoigiancapnhat = null,
                            anhdaidien = null,
                            thoigianguitoado = null
                        });
                    }
                }
                return rs;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
        public List<ViTriNhanVienGPSModel> GetViTriTatCaNVTheoToaDo(int idcty, int ID_QuanLy, float KinhDo, float ViDo, int sombankinh, int trangthai)
        {
            List<ViTriNhanVienGPSModel> rs = new List<ViTriNhanVienGPSModel>();
            try
            {
                SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idcty", idcty),
                new SqlParameter("@ID_QuanLy", ID_QuanLy),
                new SqlParameter("@KinhDo", KinhDo),
                new SqlParameter("@ViDo", ViDo),
                new SqlParameter("@mbankinh", sombankinh),
            };
                DataTable dt = helper.ExecuteDataSet("sp_QL_getViTriTatCaNV_TheoToaDo_v1", par).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    try
                    {
                        if (trangthai == -1 || int.Parse(dr["dangtructuyen"].ToString()) == trangthai)
                        {
                            rs.Add(new ViTriNhanVienGPSModel
                            {
                                idnhanvien = int.Parse(dr["idnhanvien"].ToString()),
                                tennhanvien = dr["TenNhanVien"].ToString(),
                                KinhDo = double.Parse(dr["KinhDo"].ToString()),
                                ViDo = double.Parse(dr["ViDo"].ToString()),
                                dangtructuyen = int.Parse(dr["dangtructuyen"].ToString()),
                                thoigiancapnhat = DateTime.Parse(dr["ReceivedTime"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                                ID_Nhom = dr["ID_Nhom"].ToString() != "" ? int.Parse(dr["ID_Nhom"].ToString()) : 0,
                                anhdaidien = dr["anhdaidien"].ToString(),
                                thoigianguitoado = DateTime.Parse(dr["ThoiGianHoatDong"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                                TenDangNhap = dr["TenDangNhap"].ToString(),
                                TinhTrangPin = dr["TinhTrangPin"].ToString()
                            });
                        }
                    }
                    catch
                    {
                        rs.Add(new ViTriNhanVienGPSModel
                        {
                            tennhanvien = null,
                            KinhDo = 0,
                            ViDo = 0,
                            dangtructuyen = 0,
                            thoigiancapnhat = null,
                            anhdaidien = null,
                            thoigianguitoado = null
                        });
                    }
                }
                return rs;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
    }
}