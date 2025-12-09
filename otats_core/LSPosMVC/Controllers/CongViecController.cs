using LSPosMVC.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LSPos_Data.Models;
using LSPos_Data.Data;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/congviec")]
    public class CongViecController : ApiController
    {

        [HttpGet]
        [Route("getallKendo")]
        public DataSourceResult GetOrders(HttpRequestMessage requestMessage)
        {
            RequestGridParam param = JsonConvert.DeserializeObject<RequestGridParam>(
            // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
            requestMessage.RequestUri.ParseQueryString().GetKey(0)
        );
            UserInfo userinfo = utilsCommon.checkAuthorization();

            CongViecData congViecData = new CongViecData();

            //DataTable dskh = kh_dl.GetDataKhachHangAll(userinfo.ID_QLLH, userinfo.ID_QuanLy, param.tieuchiloc.IdTinh, param.tieuchiloc.IdQuan, param.tieuchiloc.IdLoaiKhachHang, 0);
            //dskh.Rows.RemoveAt(0);
            List<CongViecModel> result = new List<CongViecModel>();
            FilterGrid filter = new FilterGrid();
            int tongso = 0;
            filter.Ngay = new DateTime(1900, 1, 1);
            filter.NgayHetHan = new DateTime(1900, 1, 1);
            filter.NgayHoanThanh = new DateTime(1900, 1, 1);
            filter.NgayNhan = new DateTime(1900, 1, 1);
            filter.DiaDiemDen = "";
            filter.DiaDiemDi = "";
            filter.NguoiDuocGiao = "";
            filter.NhomNhanVien = "";
            filter.NoiDung = "";
            filter.TenCongViec = "";
            filter.TenNguoiGui = "";
            filter.TrangThai = "";
            filter.DiaDiemDen = "";
            if (param.request.Filter != null)
            {
                foreach (Filter f in param.request.Filter.Filters)
                {
                    switch (f.Field)
                    {
                        case "tenKhachHang":
                            filter.Ngay = Convert.ToDateTime(f.Value.ToString());
                            break;
                        case "ngayHetHan":
                            filter.NgayHetHan = Convert.ToDateTime(f.Value.ToString());
                            break;
                        case "ngayNhan":
                            filter.NgayNhan = Convert.ToDateTime(f.Value.ToString());
                            break;
                        case "ngayHoanThanh":
                            filter.NgayHoanThanh = Convert.ToDateTime(f.Value.ToString());
                            break;
                        case "diaDiemDen":
                            filter.DiaDiemDen = f.Value.ToString();
                            break;
                        case "diaDiemDi":
                            filter.DiaDiemDi = f.Value.ToString();
                            break;
                        case "nguoiDuocGiao":
                            filter.NguoiDuocGiao = f.Value.ToString();
                            break;
                        case "tenNhom":
                            filter.NhomNhanVien = f.Value.ToString();
                            break;
                        case "noiDung":
                            filter.NoiDung = f.Value.ToString();
                            break;
                        case "tenCongViec":
                            filter.TenCongViec = f.Value.ToString();
                            break;
                        case "tenNguoiGui":
                            filter.TenNguoiGui = f.Value.ToString();
                            break;
                        case "tenTrangThai":
                            filter.TrangThai = f.Value.ToString(); ;
                            break;
                    }
                }
            }

            result = congViecData.DanhSachCongViec_Kendo(userinfo.ID_QLLH, param.ID_Loai, param.request.Skip, param.request.Take, filter, ref tongso);
            DataSourceResult s = new DataSourceResult();
            s.Data = result;
            s.Total = tongso;
            s.Aggregates = null;
            return s;
        }

        [HttpGet]
        [Route("getlist_grid")]
        public HttpResponseMessage getlist_grid()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                CongViecData congViecData = new CongViecData();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = congViecData.DanhSachCongViec_gridtab(userinfo.ID_QLLH);
                    if (ds.Tables.Count > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, ds);
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, Config.NODATANOTFOUND);
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }
        [HttpGet]
        [Route("getchitietcongviec")]
        public HttpResponseMessage getchitietcongviec([FromUri] int idcongviec)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                CongViecData congViecData = new CongViecData();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataTable ds = congViecData.ChiTietCongViecTheoId_v2(idcongviec, userinfo.ID_QuanLy);
                    response = Request.CreateResponse(HttpStatusCode.OK, ds);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }
        [HttpGet]
        [Route("getchitiettrangthaicongviec")]
        public HttpResponseMessage getchitiettrangthaicongviec([FromUri] int idcongviec)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                CongViecData congViecData = new CongViecData();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataTable dt = congViecData.ChiTietTrangThaiCongViec(idcongviec);
                    response = Request.CreateResponse(HttpStatusCode.OK, dt);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        [HttpPost]
        [Route("create")]
        public HttpResponseMessage Create([FromBody] CongViecModel item)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    CongViecData cvdb = new CongViecData();
                    item.NgayTao = new DateTime();
                    item.ID_QLLH = userinfo.ID_QLLH;
                    item.ID_NguoiGui = userinfo.ID_QuanLy;
                    if (item.NgayHetHan < DateTime.Now)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_khongthetaocongviectrongquakhu" });
                    }
                    else
                    {

                        int idCongViec = cvdb.Add(item);
                        if (idCongViec > 0)
                        {
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(item.FileGiaoViec))
                                {
                                    List<string> listfile = item.FileGiaoViec.Split(';').ToList();
                                    foreach (string pathfile in listfile)
                                    {
                                        if (!string.IsNullOrWhiteSpace(pathfile))
                                        {
                                            string teampatch = AppDomain.CurrentDomain.BaseDirectory + pathfile;
                                            //byte[] array = File.ReadAllBytes(teampatch);
                                            byte[] binData = GetBytesFromFile(teampatch);

                                            if (binData != null && binData.Length > 0)
                                            {
                                                string fileName = pathfile.Substring(19);
                                                string url2 = Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + "/AppUpload.aspx?idqllh=" + userinfo.ID_QLLH + "&idquanly=" + userinfo.ID_QuanLy + "&idcongviec=" + idCongViec.ToString() + "&filename=" + fileName + "&typeupload=file";
                                                PostMultipleFiles_Stream(url2, binData, fileName);
                                            }
                                        }
                                    }

                                }
                                if (item.ID_NhanVienDuocGiao.Count == 0 && item.ID_NhomNhan > 0)
                                {
                                    NhanVien_dl nv = new NhanVien_dl();
                                    DataTable dtNhanVien = nv.GetDataNhanVien_TheoNhomQuanLy(userinfo.ID_QLLH, item.ID_NhomNhan);
                                    foreach (DataRow dr in dtNhanVien.Rows)
                                    {
                                        int x = cvdb.ThemGiaoViec(idCongViec, int.Parse(dr["ID_NhanVien"].ToString()), userinfo.ID_QuanLy);
                                        if (x > 0)
                                        {
                                            string url1 = Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + "/AppPush.aspx?idnhanvien=" + int.Parse(dr["ID_NhanVien"].ToString()) + "&type=congviecmoi&idcongviec=" + idCongViec.ToString() + "&message=Ban có một công việc mới: " + item.TenCongViec;
                                            String response1 = new System.Net.WebClient().DownloadString(url1);
                                            LSPos_Data.Utilities.Log.Info("Da push giao viec cho id :  " + int.Parse(dr["ID_NhanVien"].ToString()));

                                        }
                                    }
                                }
                                else
                                {
                                    foreach (object s in item.ID_NhanVienDuocGiao)
                                    {
                                        //them vao bang nhan vien
                                        if (s != null)
                                        {
                                            int x = cvdb.ThemGiaoViec(idCongViec, int.Parse(s.ToString()), userinfo.ID_QuanLy);
                                            if (x > 0)
                                            {
                                                string url1 = Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + "/AppPush.aspx?idnhanvien=" + s + "&type=congviecmoi&idcongviec=" + idCongViec.ToString() + "&message=Ban có một công việc mới: " + item.TenCongViec;
                                                String response1 = new System.Net.WebClient().DownloadString(url1);
                                                LSPos_Data.Utilities.Log.Info("Da push giao viec cho id :  " + s);
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LSPos_Data.Utilities.Log.Error(ex);
                            }
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_taocongviecthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_taocongviecthatbaivuilongthulai" });

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        public void PostMultipleFiles_Stream(string url, byte[] file, string filename)
        {
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            httpWebRequest.Method = "POST";
            httpWebRequest.KeepAlive = true;
            httpWebRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
            Stream memStream = new System.IO.MemoryStream();
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition:  form-data; name=\"{0}\";\r\n\r\n{1}";
            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n Content-Type: application/octet-stream\r\n\r\n";
            memStream.Write(boundarybytes, 0, boundarybytes.Length);

            string header = string.Format(headerTemplate, "LHIMAGE", filename);
            //string header = string.Format(headerTemplate, "uplTheFile", files[i]);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            memStream.Write(headerbytes, 0, headerbytes.Length);

            memStream.Write(file, 0, file.Length);

            memStream.Write(boundarybytes, 0, boundarybytes.Length);


            httpWebRequest.ContentLength = memStream.Length;
            Stream requestStream = httpWebRequest.GetRequestStream();
            memStream.Position = 0;
            byte[] tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();
            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();
            try
            {
                WebResponse webResponse = httpWebRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                string var = reader.ReadToEnd();

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            httpWebRequest = null;
        }

        public static byte[] GetBytesFromFile(string fullFilePath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)

            FileStream fs = File.OpenRead(fullFilePath);
            try
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                return bytes;
            }
            finally
            {
                fs.Close();
            }

        }
    }
}
