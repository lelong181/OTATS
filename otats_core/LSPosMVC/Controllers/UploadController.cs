using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace LSPosMVC.Controllers
{
    [RoutePrefix("api/uploadfile")]
    [EnableCors(origins: "*", "*", "*")]
    public class UploadController : ApiController
    {
        public class HttpPostedFile
        {
            public HttpPostedFile(string name, string filename, byte[] file)
            {
                //Property Assignment
            }
            public string Name { get; private set; }
            public string Filename { get; private set; }
            public byte[] File { private set; get; }
        }

        [HttpPost]
        [Route("savefile")]
        public async Task<string> Upload()
        {
            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                LSPos_Data.Utilities.Log.Info("Đoc file từ request");
                var files = new Dictionary<string, HttpPostedFile>(StringComparer.InvariantCultureIgnoreCase);
                LSPos_Data.Utilities.Log.Info("Lấy file");
                var fileNameParam = provider.Contents[0].Headers.ContentDisposition.Parameters
                    .FirstOrDefault(p => p.Name.ToLower() == "filename");
                LSPos_Data.Utilities.Log.Info("Lấy tên file : " + fileNameParam);
                string fileName = (fileNameParam == null) ? "" : fileNameParam.Value.Trim('"');
                fileName = DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + fileName.Split('.')[1];
                LSPos_Data.Utilities.Log.Info("Tạo filename : " + fileName);
                byte[] file = await provider.Contents[0].ReadAsByteArrayAsync();
                LSPos_Data.Utilities.Log.Info("Đọc byte : " + file.Length);
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"FileUpload\Images" + @"\" + fileName;
                LSPos_Data.Utilities.Log.Info("Filepath: " + filePath);
                File.WriteAllBytes(filePath, file);
                LSPos_Data.Utilities.Log.Info("Ghi file");
                string path_thumbnail_medium = fileName.Split('.')[0] + "_medium." + fileName.Split('.')[1];
                string path_thumbnail_small = fileName.Split('.')[0] + "_small." + fileName.Split('.')[1];
                ThumbnailImage(filePath, AppDomain.CurrentDomain.BaseDirectory + @"FileUpload\Images\" + path_thumbnail_medium, 0, 200);
                ThumbnailImage(filePath, AppDomain.CurrentDomain.BaseDirectory + @"FileUpload\Images\" + path_thumbnail_small, 0, 64);
                string result = "/FileUpload/Images/" + fileName;
                return result;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public class Mainrequests
        {
            public List<requests> requests { get; set; }
        }

        public class requests
        {
            public image image { get; set; }
            public List<features> features { get; set; }
        }

        public class image
        {
            public string content { get; set; }
        }
        public class features
        {
            public string type { get; set; }
        }

        public class AnhThanhToan
        {
            public string base64String { get; set; }
        }

        [HttpPost]
        [Route("checkAnhThanhToan")]
        public HttpResponseMessage Capture(AnhThanhToan model)
        {
            var imageParts = model.base64String.Split(',').ToList<string>();
            byte[] imageBytes = Convert.FromBase64String(imageParts[1]);

            using (var client = new WebClient())
            {
                Mainrequests Mainrequests = new Mainrequests()
                {
                    requests = new List<requests>()
                {
                     new requests()
                {
                     image = new image()
                     {
                     content = imageParts[1]
                 },

                 features = new List<features>()
                 {
                     new features()
                     {
                         type = "DOCUMENT_TEXT_DETECTION",
                         //type = "FACE_DETECTION",
                     }

                 }

             }

             }

                };

                string GG_APIKey = System.Configuration.ConfigurationManager.AppSettings["GOOGLEAPIKEY"];
                var uri = "https://vision.googleapis.com/v1/images:annotate?key=" + GG_APIKey;
                client.Headers.Add("Content-Type:application/json");
                client.Headers.Add("Accept:application/json");
                client.Encoding = Encoding.UTF8;
                var response = client.UploadString(uri, JsonConvert.SerializeObject(Mainrequests));
                return Request.CreateResponse(HttpStatusCode.OK, JObject.Parse(response));
            }
        }

        [HttpPost]
        [Route("checkAnhNapVi")]
        public HttpResponseMessage Capture2(AnhThanhToan model)
        {
            var imageParts = model.base64String.Split(',').ToList<string>();
            byte[] imageBytes = Convert.FromBase64String(imageParts[1]);

            using (var client = new WebClient())
            {
                Mainrequests Mainrequests = new Mainrequests()
                {
                    requests = new List<requests>()
                {
                     new requests()
                {
                     image = new image()
                     {
                     content = imageParts[1]
                 },

                 features = new List<features>()
                 {
                     new features()
                     {
                         type = "DOCUMENT_TEXT_DETECTION",
                     }

                 }

             }

             }

                };

                string GG_APIKey = System.Configuration.ConfigurationManager.AppSettings["GOOGLEAPIKEY"];
                var uri = "https://vision.googleapis.com/v1/images:annotate?key=" + GG_APIKey;
                client.Headers.Add("Content-Type:application/json");
                client.Headers.Add("Accept:application/json");
                client.Encoding = Encoding.UTF8;
                var response = client.UploadString(uri, JsonConvert.SerializeObject(Mainrequests));
                return Request.CreateResponse(HttpStatusCode.OK, JObject.Parse(response));
            }
        }

        //public IReadOnlyList<EntityAnnotation> DetectText(AnhThanhToan model)
        //{
        //    //var credential = GoogleCredential.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"\Assets\tourshopping-6bb85abcf6fd.json").CreateScoped(ImageAnnotatorClient.DefaultScopes);
        //    //var channel = new Grpc.Core.Channel(ImageAnnotatorClient.DefaultEndpoint.ToString(), credential.ToChannelCredentials());
        //    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", AppDomain.CurrentDomain.BaseDirectory + @"\Assets\tourshopping-6bb85abcf6fd.json");
        //    var imageParts = model.base64String.Split(',').ToList<string>();
        //    byte[] imageBytes = Convert.FromBase64String(imageParts[1]);
        //    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
        //    // Convert byte[] to Image
        //    ms.Write(imageBytes, 0, imageBytes.Length);
        //    Image image = Image.FromStream(ms);
        //    ImageAnnotatorClient client = ImageAnnotatorClient.Create();
        //    IReadOnlyList<EntityAnnotation> response = client.DetectText(image);
        //    return response;
        //}

        [HttpPost]
        [Route("checkfilechuyenkhoan")]
        public async Task<string> CheckUpload_AnhChuyenKhoan()
        {
            try
            {
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                LSPos_Data.Utilities.Log.Info("Đoc file từ request");
                var files = new Dictionary<string, HttpPostedFile>(StringComparer.InvariantCultureIgnoreCase);
                LSPos_Data.Utilities.Log.Info("Lấy file");
                var fileNameParam = provider.Contents[0].Headers.ContentDisposition.Parameters
                    .FirstOrDefault(p => p.Name.ToLower() == "filename");
                LSPos_Data.Utilities.Log.Info("Lấy tên file : " + fileNameParam);
                string fileName = (fileNameParam == null) ? "" : fileNameParam.Value.Trim('"');
                fileName = DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + fileName.Split('.')[1];
                LSPos_Data.Utilities.Log.Info("Tạo filename : " + fileName);
                byte[] file = await provider.Contents[0].ReadAsByteArrayAsync();
                LSPos_Data.Utilities.Log.Info("Đọc byte : " + file.Length);
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"FileUpload\Images" + @"\" + fileName;
                LSPos_Data.Utilities.Log.Info("Filepath: " + filePath);
                File.WriteAllBytes(filePath, file);
                LSPos_Data.Utilities.Log.Info("Ghi file");
                string path_thumbnail_medium = fileName.Split('.')[0] + "_medium." + fileName.Split('.')[1];
                string path_thumbnail_small = fileName.Split('.')[0] + "_small." + fileName.Split('.')[1];
                ThumbnailImage(filePath, AppDomain.CurrentDomain.BaseDirectory + @"FileUpload\Images\" + path_thumbnail_medium, 0, 200);
                ThumbnailImage(filePath, AppDomain.CurrentDomain.BaseDirectory + @"FileUpload\Images\" + path_thumbnail_small, 0, 64);
                string result = "/FileUpload/Images/" + fileName;
                return result;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        [HttpPost]
        [Route("savemultifile")]
        public async Task<string> UploadMulti()
        {
            try
            {
                string result = "";
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                var files = new Dictionary<string, HttpPostedFile>(StringComparer.InvariantCultureIgnoreCase);
                for (int i = 0; i < provider.Contents.Count; i++)
                {

                    var fileNameParam = provider.Contents[i].Headers.ContentDisposition.Parameters
                        .FirstOrDefault(p => p.Name.ToLower() == "filename");
                    string fileName = (fileNameParam == null) ? "" : fileNameParam.Value.Trim('"');
                    fileName = fileName.Split('.')[0] + DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + fileName.Split('.')[1];
                    byte[] file = await provider.Contents[i].ReadAsByteArrayAsync();
                    string filePath = AppDomain.CurrentDomain.BaseDirectory + @"FileUpload\Images" + @"\" + fileName;
                    File.WriteAllBytes(filePath, file);
                    string path_thumbnail_medium = fileName.Split('.')[0] + "_medium." + fileName.Split('.')[1];
                    string path_thumbnail_small = fileName.Split('.')[0] + "_small." + fileName.Split('.')[1];
                    ThumbnailImage(filePath, AppDomain.CurrentDomain.BaseDirectory + @"FileUpload\Images\" + path_thumbnail_medium, 0, 200);
                    ThumbnailImage(filePath, AppDomain.CurrentDomain.BaseDirectory + @"FileUpload\Images\" + path_thumbnail_small, 0, 64);
                    result += "/FileUpload/Images/" + fileName + ";";
                }

                return result;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
        [HttpPost]
        [Route("savefilenv")]
        public async Task<string> Uploadnv()
        {
            try
            {
                var provider = new MultipartMemoryStreamProvider();

                provider = await Request.Content.ReadAsMultipartAsync(provider);
                LSPos_Data.Utilities.Log.Info("Đoc file từ request");
                var files = new Dictionary<string, HttpPostedFile>(StringComparer.InvariantCultureIgnoreCase);
                LSPos_Data.Utilities.Log.Info("Lấy file");

                var fileNameParam = provider.Contents[0].Headers.ContentDisposition.Parameters
                    .FirstOrDefault(p => p.Name.ToLower() == "filename");
                LSPos_Data.Utilities.Log.Info("Lấy tên file");
                string fileName = (fileNameParam == null) ? "" : fileNameParam.Value.Trim('"');
                fileName = fileName.Trim();
                string extention = fileName.Substring(fileName.LastIndexOf('.') + 1);
                fileName = fileName.Substring(0, fileName.LastIndexOf('.')).Replace('.', '_').ToLower();
                fileName = fileName + DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + extention;
                LSPos_Data.Utilities.Log.Info("Tạo filename");

                string templatefolder = System.Web.Configuration.WebConfigurationManager.AppSettings["EmpImage"];
                string filePath = AppDomain.CurrentDomain.BaseDirectory + templatefolder + @"\" + fileName;
                byte[] file = await provider.Contents[0].ReadAsByteArrayAsync();
                LSPos_Data.Utilities.Log.Info("Đọc byte : " + file.Length);
                File.WriteAllBytes(filePath, file);
                LSPos_Data.Utilities.Log.Info("Ghi file");
                string result = templatefolder + @"\" + fileName;
                return result;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);

            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (System.Drawing.Image)b;
        }
        private void ThumbnailImage(string originalImagePath, string thumbnailImagePath, int height, int width)
        {

            try
            {
                if (!System.IO.File.Exists(thumbnailImagePath))
                {
                    System.Drawing.Image imThumbnailImage;
                    System.Drawing.Image OriginalImage = System.Drawing.Image.FromFile(originalImagePath);
                    if (height == 0)
                    {
                        height = Convert.ToInt32((OriginalImage.Height / (OriginalImage.Width / width)));
                    }
                    imThumbnailImage = resizeImage(OriginalImage, new Size(width, height));
                    //imThumbnailImage = OriginalImage.GetThumbnailImage(width, height,
                    //             new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);

                    imThumbnailImage.Save(thumbnailImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                    imThumbnailImage.Dispose();
                    OriginalImage.Dispose();
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

        }

        [HttpPost]
        [Route("savefileExcel")]

        public async Task<string> UploadExcel()
        {
            try
            {
                var provider = new MultipartMemoryStreamProvider();

                await Request.Content.ReadAsMultipartAsync(provider);
                var files = new Dictionary<string, HttpPostedFile>(StringComparer.InvariantCultureIgnoreCase);
                var fileNameParam = provider.Contents[0].Headers.ContentDisposition.Parameters
                    .FirstOrDefault(p => p.Name.ToLower() == "filename");
                string fileName = (fileNameParam == null) ? "" : fileNameParam.Value.Trim('"');
                fileName = fileName.Split('.')[0] + DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + fileName.Split('.')[1];
                byte[] file = await provider.Contents[0].ReadAsByteArrayAsync();
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"FileUpload\ExcelFiles" + @"\" + fileName;
                File.WriteAllBytes(filePath, file);
                // Here you can use EF with an entity with a byte[] property, or
                // an stored procedure with a varbinary parameter to insert the
                // data into the DB

                //var result
                //    = string.Format("Received '{0}' with length: {1}", fileName, file.Length);
                string result = "/FileUpload/ExcelFiles/" + fileName;
                return result;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return ex.Message;
            }
        }
    }
}
