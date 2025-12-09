using LSPosMVC.Common;
using LSPos_Data.Data;
using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/cauhinh")]
    public class CauHinhCongTyController : ApiController
    {
        [HttpGet]
        [Route("get")]
        public HttpResponseMessage get()
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
                    CauHinhCongTyModel ch = new CauHinhCongTyModel();
                    CauHinhCongTyData cauHinhCongTyData = new CauHinhCongTyData();
                    ch = cauHinhCongTyData.get(userinfo.ID_QLLH);

                    response = Request.CreateResponse(HttpStatusCode.OK, ch);
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
        [Route("update")]
        public HttpResponseMessage updatecauhinhchung([FromBody] CauHinhCongTyModel cauhinh)
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
                    CauHinhCongTyData cauHinhCongTyData = new CauHinhCongTyData();

                    if (cauhinh.iconPath != "")
                    {
                        try
                        {
                            string teampatch = AppDomain.CurrentDomain.BaseDirectory + cauhinh.iconPath;
                            if (File.Exists(teampatch))
                            {
                                byte[] binData = GetBytesFromFile(teampatch);

                                if (binData != null && binData.Length > 0)
                                {
                                    string svURL = Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]);
                                    string filename = userinfo.ID_QLLH.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".jpg";
                                    string strLinkServer = svURL + "/AppUpload.aspx?token=6e22b116f5111220741848ccd290e9e9062522d88a1fb00ba9b168db7a480271&typeupload=logocongty&idqllh="
                                        + userinfo.ID_QLLH + "&imagename=" + filename;
                                    PostMultipleFiles_Stream(strLinkServer, binData, "test.jpg");
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            LSPos_Data.Utilities.Log.Error(ex);
                        }
                    }

                    if (cauHinhCongTyData.update(userinfo.ID_QLLH, cauhinh))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, "Cập nhật không thành công");
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
    }
}
