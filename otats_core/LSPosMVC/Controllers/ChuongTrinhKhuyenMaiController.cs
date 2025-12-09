using LSPosMVC.Common;
using LSPos_Data.Data;
using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using LSPosMVC.Models;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/ChuongTrinhKhuyenMai")]
    public class ChuongTrinhKhuyenMaiController : ApiController
    {
        [HttpGet]
        [Route("getallhinhthuc")]
        public HttpResponseMessage getallhinhthuc()
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
                    ChuongTrinhKMDAL ctkm = new ChuongTrinhKMDAL();
                    DataTable data = ctkm.GetAllHinhThucKM(userinfo.ID_QuanLy);
                    response = Request.CreateResponse(HttpStatusCode.OK, data);
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
        [Route("GetAll")]
        public HttpResponseMessage GetAll()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                ChuongTrinhKMDAL bc_dl = new ChuongTrinhKMDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataSet ds = bc_dl.GetDSKhuyenmaiAll(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    if (ds.Tables.Count > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
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
        [Route("GetDonHangKhuyenMai")]
        public HttpResponseMessage GetDonHangKhuyenMai([FromUri] int ID_CTKM)
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
                    DataTable dt = ChiTietKhuyenMai_dl.GetDonHangByCTKM(ID_CTKM);
                    List<ChiTietKhuyenMai> chitiet = ChiTietKhuyenMai_dl.GetChiTietKhuyenMai(0, ID_CTKM);

                    response = Request.CreateResponse(HttpStatusCode.OK, new { donhang = dt, chitiet = chitiet });
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
        [Route("ExportExcelKhuyenMai")]
        public HttpResponseMessage ExportExcelKhuyenMai()
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
                    ChuongTrinhKMDAL bc_dl = new ChuongTrinhKMDAL();
                    DataSet ds = bc_dl.GetDSKhuyenmaiAll(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    ds.Tables[0].TableName = "DATA";

                    BaoCaoCommon baocao = new BaoCaoCommon();
                    DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    dt2.TableName = "DATA2";
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    DataTable dt1 = new DataTable();
                    dt1.TableName = "DATA1";
                    dt1.Columns.Add("TITLE", typeof(String));
                    DataRow row = dt1.NewRow();
                    row["TITLE"] = "";
                    dt1.Rows.Add(row);

                    DataSet dataSet = new DataSet();
                    dataSet.Tables.Add(ds.Tables[0].Copy());
                    dataSet.Tables.Add(dt1.Copy());
                    dataSet.Tables.Add(dt2.Copy());

                    string filename = "";
                    var stream = new MemoryStream();
                    ExportExcel excel = new ExportExcel();

                    if (lang == "vi")
                    {
                        filename = "BM021_DanhSachKhuyenMai_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("ExcelKhuyenMai.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM021_Discount_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("ExcelKhuyenMai_en.xls", dataSet, null, ref stream);
                    }

                    response.Content = new ByteArrayContent(stream.ToArray());
                    response.Content.Headers.Add("x-filename", filename);
                    response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = filename;
                    response.StatusCode = HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;

        }
        [HttpGet]
        [Route("GetDonHangByCTKM")]
        public HttpResponseMessage GetDonHangByCTKM([FromUri] int ID_CTKM)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                ChuongTrinhKMDAL bc_dl = new ChuongTrinhKMDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    List<KhuyenMai> km = new List<KhuyenMai>();
                    DataSet ds = bc_dl.GetDonHangByCTKM(ID_CTKM);
                    if (ds.Tables.Count > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
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
        [Route("GetChiTietKhuyenMai")]
        public HttpResponseMessage GetChiTietKhuyenMai([FromUri] int ID_CTKM)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                ChuongTrinhKMDAL bc_dl = new ChuongTrinhKMDAL();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    List<KhuyenMai> km = new List<KhuyenMai>();
                    DataSet ds = bc_dl.GetChiTietKhuyenMai(userinfo.ID_QLLH, ID_CTKM);
                    if (ds.Tables.Count > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, ds.Tables[0]);
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
        [Route("getmathangctkm")]
        public HttpResponseMessage getmathangctkm([FromUri] int ID_CTKM, int ID_DANHMUC)
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
                    MatHang_dl mh_dl = new MatHang_dl();
                    DataTable dsmh = new DataTable();
                    if (ID_DANHMUC < 1)
                        dsmh = mh_dl.Get_DS_MatHangAll_LoaiBoTheoCTKM(userinfo.ID_QLLH, ID_CTKM);
                    else
                        dsmh = mh_dl.get_DS_HangHoa_ByIdDanhMuc_LoaiBoTheoCTKM(ID_DANHMUC, userinfo.ID_QLLH, ID_CTKM);

                    response = Request.CreateResponse(HttpStatusCode.OK, dsmh);
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
        [Route("GetKhuyenMaiByID")]
        public HttpResponseMessage GetKhuyenMaiByID([FromUri] int ID_CTKM)
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
                    if (ID_CTKM > 0)
                    {
                        KhuyenMai_dl km_dl = new KhuyenMai_dl();
                        KhuyenMai km = km_dl.GetKhuyenMaiByID(ID_CTKM);
                        response = Request.CreateResponse(HttpStatusCode.OK, km);
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified);
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
        [Route("GetChiTietHangTang")]
        public HttpResponseMessage GetChiTietHangTang([FromUri] int ID_CTKM)
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
                    KhuyenMai_dl km_dl = new KhuyenMai_dl();
                    DataTable data = km_dl.GetChiTietHangTang(ID_CTKM);
                    response = Request.CreateResponse(HttpStatusCode.OK, data);
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
        [Route("GetListHangTangByMatHang")]
        public HttpResponseMessage GetListHangTangByMatHang([FromUri] int ID_CTKM, int idmathang)
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
                    KhuyenMai_dl km_dl = new KhuyenMai_dl();
                    DataTable data = km_dl.GetChiTietHangTang_TheoMatHang(ID_CTKM, idmathang);
                    response = Request.CreateResponse(HttpStatusCode.OK, data);
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
        [Route("SuaKhuyenMai_v2")]
        public HttpResponseMessage SuaKhuyenMai_v2([FromBody] KhuyenMaiSaveModelFilter khuyenMai)
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
                    KhuyenMai_dl km_dl = new KhuyenMai_dl();
                    khuyenMai.KhuyenMaiObj.ID_QLLH = userinfo.ID_QLLH;
                    khuyenMai.KhuyenMaiObj.ID_QuanLy = userinfo.ID_QuanLy;
                    bool flag = km_dl.SuaKhuyenMai_v2(khuyenMai.KhuyenMaiObj, khuyenMai.DanhSachHangTang);

                    try
                    {
                        if (!string.IsNullOrWhiteSpace(khuyenMai.url_img))
                        {
                            string teampatch = AppDomain.CurrentDomain.BaseDirectory + khuyenMai.url_img;
                            if (File.Exists(teampatch))
                            {
                                byte[] binData = GetBytesFromFile(teampatch);
                                if (binData != null && binData.Length > 0)
                                {
                                    string svURL = Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]);
                                    string strLinkServer = svURL + "/AppUploadAnh.aspx?token=abc&idqllh=" + userinfo.ID_QLLH + "&idnhanvien=" + userinfo.ID_QuanLy
                                        + "&idctkm=" + khuyenMai.KhuyenMaiObj.ID_CTKM + "&idmathang=0&ghichu=&thoigianchup=" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "&type=ctkm";

                                    PostMultipleFiles_Stream(strLinkServer, binData, "test.jpg");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }

                    if (flag)
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luuchuongtrinhkhuyenmaithanhcong" });
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luuchuongtrinhkhuyenmaikhongthanhcong" });
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
        [Route("ThemKhuyenMai")]
        public HttpResponseMessage ThemKhuyenMai([FromBody] KhuyenMaiSaveModelFilter khuyenMai)
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
                    KhuyenMai_dl km_dl = new KhuyenMai_dl();
                    khuyenMai.KhuyenMaiObj.ID_QLLH = userinfo.ID_QLLH;
                    khuyenMai.KhuyenMaiObj.ID_QuanLy = userinfo.ID_QuanLy;
                    int idctkm = km_dl.ThemKhuyenMai(khuyenMai.KhuyenMaiObj, khuyenMai.DanhSachHangTang);

                    if (idctkm > 0)
                    {
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(khuyenMai.url_img))
                            {
                                string teampatch = AppDomain.CurrentDomain.BaseDirectory + khuyenMai.url_img;
                                if (File.Exists(teampatch))
                                {
                                    byte[] binData = GetBytesFromFile(teampatch);
                                    if (binData != null && binData.Length > 0)
                                    {
                                        string svURL = Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]);
                                        string strLinkServer = svURL + "/AppUploadAnh.aspx?token=abc&idqllh=" + userinfo.ID_QLLH + "&idnhanvien=" + userinfo.ID_QuanLy
                                            + "&idctkm=" + idctkm + "&idmathang=0&ghichu=&thoigianchup=" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "&type=ctkm";

                                        PostMultipleFiles_Stream(strLinkServer, binData, "test.jpg");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LSPos_Data.Utilities.Log.Error(ex);
                        }
                    }

                    if (idctkm > 0)
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, data = idctkm, msg = "label_luuchuongtrinhkhuyenmaithanhcong" });
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, data = 0, msg = "label_luuchuongtrinhkhuyenmaikhongthanhcong" });
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
        [Route("ngungsudung")]
        public HttpResponseMessage ngungsudungctkm([FromUri] int ID_CTKM)
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
                    KhuyenMai_dl km_dl = new KhuyenMai_dl();
                    KhuyenMai km = km_dl.GetKhuyenMaiByID(ID_CTKM);
                    string action = (km.TrangThai == 1) ? "label_ngungchuongtrinhkhuyenmaithanhcong" : "label_mochuongtrinhkhuyenmaithanhcong";

                    if (km_dl.NgungSuDungKhuyenMai(km))
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = action });
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = action });
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
        [Route("xoachuongtrinhkhuyenmai")]
        public HttpResponseMessage xoachuongtrinhkhuyenmai([FromUri] int ID_CTKM)
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
                    KhuyenMai_dl km_dl = new KhuyenMai_dl();
                    KhuyenMai km = km_dl.GetKhuyenMaiByID(ID_CTKM);

                    {
                        if (km_dl.DeleteKhuyenMai(km))
                        {
                            ChuongTrinhKhuyenMaiData chuongTrinhKhuyenMaiData = new ChuongTrinhKhuyenMaiData();
                            if (chuongTrinhKhuyenMaiData.CheckDonHang(km.ID_CTKM))
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_chuongtrinhkhuyenmaidacodonhangkhongthexoasechuyensangtrangthaingunghoatdong" });
                            else
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoachuongtrinhkhuyenmaithanhcong" });
                        }
                        else
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoachuongtrinhkhuyenmaikhongthanhcongvuilonglienhequantri" });
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
