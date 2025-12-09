using LSPosMVC.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Aspose.Cells;
using System.Reflection;
using System.Configuration;
using System.Web.UI;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using LSPos_Data.Models;
using static LSPos_Data.Data.NhanVienApp;
using LSPosMVC.Models;
using LSPosMVC.Models.Models_Filter;
using Aspose.Words.Lists;

namespace LSPos_Data.Data
{
    [Authorize]
    [RoutePrefix("api/nhanvienapp")]
    public class NhanVienAppController : ApiController
    {
        [HttpGet]
        [Route("getall")]
        public HttpResponseMessage get([FromUri] int IdNhom)
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
                    NhanVienApp nvapp = new NhanVienApp();
                    List<NhanVienModels> dsnv = nvapp.GetDSNhanVien_TheoNhomQuanLy(userinfo.ID_QLLH, userinfo.ID_QuanLy, IdNhom);

                    response = Request.CreateResponse(HttpStatusCode.OK, dsnv);
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
        [Route("getnhanviendangnhapphanmem")]
        public HttpResponseMessage getnhanviendangnhapphanmem([FromUri] int idnhom)
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
                    NhanVienApp nvapp = new NhanVienApp();
                    DataTable dsnv = nvapp.GetDSNhanVien_DaDangNhapApp_TheoNhom(userinfo.ID_QLLH, idnhom);

                    response = Request.CreateResponse(HttpStatusCode.OK, dsnv);
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
        [Route("getallbynhom")]
        public HttpResponseMessage getallbynhom([FromBody] ParamLocTheoNhom param)
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

                    NhanVien_dl nv_dl = new NhanVien_dl();
                    List<NhanVien> dsnv = new List<NhanVien>();

                    if (param.IDNhom == "-1")
                    {
                        dsnv = nv_dl.GetDSNhanVien(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    }
                    else
                    {

                        List<string> ids = param.IDNhom.Split(',').ToList<string>();
                        dsnv = new List<NhanVien>();
                        foreach (string id in ids)
                        {
                            try
                            {
                                dsnv.AddRange(nv_dl.GetDSNhanVien_TheoNhomQuanLy(userinfo.ID_QLLH, int.Parse(id)));
                            }
                            catch (Exception ex)
                            {
                                LSPos_Data.Utilities.Log.Error(ex);
                            }

                        }
                    }



                    response = Request.CreateResponse(HttpStatusCode.OK, dsnv);
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
        [Route("getallNhanvien")]
        public DataSourceResult GetOrders(HttpRequestMessage requestMessage)
        {
            RequestGridParam param = JsonConvert.DeserializeObject<RequestGridParam>(
            // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
            requestMessage.RequestUri.ParseQueryString().GetKey(0)
        );
            UserInfo userinfo = utilsCommon.checkAuthorization();

            NhanVienApp nvapp = new NhanVienApp();

            //DataTable dskh = kh_dl.GetDataKhachHangAll(userinfo.ID_QLLH, userinfo.ID_QuanLy, param.tieuchiloc.IdTinh, param.tieuchiloc.IdQuan, param.tieuchiloc.IdLoaiKhachHang, 0);
            //dskh.Rows.RemoveAt(0);
            List<NhanVienDTO> lstNhanvien = new List<NhanVienDTO>();
            FilterGridNV filter = new FilterGridNV();
            int tongso = 0;
            if (param.request.Filter != null)
            {
                foreach (Filter f in param.request.Filter.Filters)
                {
                    switch (f.Field)
                    {
                        case "tenDangNhap":
                            filter.TenDangNhap = f.Value.ToString(); ;
                            break;
                        case "tenDayDu":
                            filter.TenDayDu = f.Value.ToString(); ;
                            break;
                        case "dienThoai":
                            filter.DienThoai = f.Value.ToString(); ;
                            break;
                        case "trangThaiTrucTuyen":
                            filter.TrangThaiTrucTuyen = f.Value.ToString(); ;
                            break;
                        case "thoiGianGuiBanTinCuoiCung":
                            filter.ThoiGianGuiBanTinCuoiCung = f.Value.ToString(); ;
                            break;
                        case "phienBan":
                            filter.PhienBan = f.Value.ToString(); ;
                            break;
                        case "tenNhom":
                            filter.TenNhom = f.Value.ToString(); ;
                            break;

                            //case "ngayTao":
                            //    filter.NgayTao = f.Value.ToString(); ;
                            //    break;
                    }
                }
            }

            lstNhanvien = nvapp.GetDataNhanVien_Kendo(userinfo.ID_QLLH, param.tieuchiloc.IdNhom, param.request.Skip, param.request.Take, filter, ref tongso);
            if (param.request.Sort != null && param.request.Sort.Count() > 0)
            {
                string sortField = param.request.Sort.First().Field;
                switch (sortField)
                {
                    case "tenDangNhap":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstNhanvien = lstNhanvien.OrderBy(x => x.TenDangNhap).ToList();
                        }
                        else
                        {
                            lstNhanvien = lstNhanvien.OrderByDescending(x => x.TenDangNhap).ToList();
                        }
                        break;
                    case "tenDayDu":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstNhanvien = lstNhanvien.OrderBy(x => x.TenDayDu).ToList();
                        }
                        else
                        {
                            lstNhanvien = lstNhanvien.OrderByDescending(x => x.TenDayDu).ToList();
                        }
                        break;
                    case "dienThoai":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstNhanvien = lstNhanvien.OrderBy(x => x.DienThoai).ToList();
                        }
                        else
                        {
                            lstNhanvien = lstNhanvien.OrderByDescending(x => x.DienThoai).ToList();
                        }
                        break;
                    case "thoiGianGuiBanTinCuoiCung":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstNhanvien = lstNhanvien.OrderBy(x => x.ThoiGianGuiBanTinCuoiCung).ToList();
                        }
                        else
                        {
                            lstNhanvien = lstNhanvien.OrderByDescending(x => x.ThoiGianGuiBanTinCuoiCung).ToList();
                        }
                        break;
                    case "phienBan":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstNhanvien = lstNhanvien.OrderBy(x => x.PhienBan).ToList();
                        }
                        else
                        {
                            lstNhanvien = lstNhanvien.OrderByDescending(x => x.PhienBan).ToList();
                        }
                        break;
                    case "tenNhom":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstNhanvien = lstNhanvien.OrderBy(x => x.TenNhom).ToList();
                        }
                        else
                        {
                            lstNhanvien = lstNhanvien.OrderByDescending(x => x.TenNhom).ToList();
                        }
                        break;
                    case "trangThaiTrucTuyen":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstNhanvien = lstNhanvien.OrderBy(x => x.TrangThaiTrucTuyen).ToList();
                        }
                        else
                        {
                            lstNhanvien = lstNhanvien.OrderByDescending(x => x.TrangThaiTrucTuyen).ToList();
                        }
                        break;
                    case "tinhTrangPin":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstNhanvien = lstNhanvien.OrderBy(x => x.TinhTrangPin).ToList();
                        }
                        else
                        {
                            lstNhanvien = lstNhanvien.OrderByDescending(x => x.TinhTrangPin).ToList();
                        }
                        break;
                    case "anhDaiDien":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstNhanvien = lstNhanvien.OrderBy(x => x.AnhDaiDien).ToList();
                        }
                        else
                        {
                            lstNhanvien = lstNhanvien.OrderByDescending(x => x.AnhDaiDien).ToList();
                        }
                        break;

                }
            }
            else
            {
                lstNhanvien = lstNhanvien.OrderByDescending(x => x.IDNV).ToList();
            }

            DataSourceResult s = new DataSourceResult();
            s.Data = lstNhanvien;

            DataTable tong = nvapp.CountSoLuong(userinfo.ID_QLLH, param.tieuchiloc.IdNhom, param.request.Skip, param.request.Take, filter);
            //if (tong.Rows.Count > 0)
            //{
            //    tongso = int.Parse(tong.Rows[0]["soluong"].ToString());
            //}
            //else
            //{
            //    tongso = 0;
            //}
            s.Total = tongso;
            s.Aggregates = null;
            //return lstNhanvien.AsQueryable().ToDataSourceResult(param.request.Take, param.request.Skip, param.request.Sort, param.request.Filter);
            return s;
            //return dsnv.AsQueryable().ToDataSourceResult(param.request.Take, param.request.Skip, param.request.Sort, param.request.Filter);

        }

        [HttpGet]
        [Route("getnvbyId")]
        public HttpResponseMessage getnvID([FromUri] TieuChiLoc tieuchi)
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
                    NhanVien_dl nv_dl = new NhanVien_dl();
                    NhanVien nv = nv_dl.GetNVTheoID(tieuchi.ID_NV);
                    response = Request.CreateResponse(HttpStatusCode.OK, nv);
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
        [Route("GetKhachHangDaCapQuyen")]
        public HttpResponseMessage GetKhachHangDaCapQuyen([FromUri] int idNhanvien)
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
                    KhachHang_dl kh_dl = new KhachHang_dl();
                    DataTable dskhDataSrc = kh_dl.GetKhachHangDaCapQuyen(idNhanvien, -1, 0, 0, 0, 0);

                    response = Request.CreateResponse(HttpStatusCode.OK, dskhDataSrc);
                }

            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("exportexcelnv")]
        public HttpResponseMessage exportexcelnv([FromUri] string username, string maCongTy)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                //string authHeader = HttpContext.Current.Request.Headers["Authorization"];

                ////decode token string
                //var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                //string username = token.Claims.First(c => c.Type == "Username").Value;
                //string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;

                User_dl userDL = new User_dl();

                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("tenDayDu", typeof(String));
                    dt.Columns.Add("tenDangNhap", typeof(String));
                    dt.Columns.Add("matKhau", typeof(String));
                    dt.Columns.Add("queQuan", typeof(String));
                    dt.Columns.Add("diaChi", typeof(String));
                    dt.Columns.Add("ngaySinh", typeof(String));
                    dt.Columns.Add("dienThoai", typeof(String));
                    dt.Columns.Add("email", typeof(String));
                    dt.Columns.Add("tenNhom", typeof(String));
                    dt.Columns.Add("iD_Nhom", typeof(String));
                    dt.Columns.Add("truongNhom", typeof(String));
                    dt.Columns.Add("gioiTinhName", typeof(String));
                    dt.Columns.Add("chucVu", typeof(String));
                    DataSet ds = new DataSet();
                    KhachHang_dl kh_dl = new KhachHang_dl();
                    NhomData nhom = new NhomData();
                    //DataTable dtkh = kh_dl.GetDataKhachHangAll(userinfo.ID_QLLH, userinfo.ID_QuanLy, 0, 0, -1, 0);
                    DataTable dtNhom = nhom.GetDsNhomByIdCongTy(userinfo.ID_QLLH);
                    // string templatefolder = System.Web.Configuration.WebConfigurationManager.AppSettings["ReportTemplatesFolder"];
                    ds.Tables.Add(dt.Copy());
                    ds.Tables.Add(dtNhom.Copy());
                    ds.Tables[0].TableName = "DATA";
                    ds.Tables[1].TableName = "DATA1";
                    for (int i1 = 1; i1 <= 2000; i1++)
                    {
                        var row = ds.Tables[0].NewRow();
                        ds.Tables[0].Rows.Add(row);
                    }
                    string tempPath = "";
                    ExportTemplate(@"ReportTemplates\Danh_sach_nhan_vien.xlsx", ds, null, ref tempPath);

                    var stream = new MemoryStream();
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(stream.ToArray())
                    };
                    response.Content.Headers.ContentDisposition =
                        new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                        {
                            FileName = tempPath
                        };
                    response.Content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);

            }
            return response;

        }

        #region import excel nhân viên
        [HttpPost]
        [Route("ExportExcelNhanVien")]
        public HttpResponseMessage ExportExcelNhanVien([FromBody] ExportNhanVienDTO obj)
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
                    NhanVienApp nvapp = new NhanVienApp();
                    DataSet ds = nvapp.ExportExcelNhanVien(userinfo.ID_QLLH, userinfo.ID_QuanLy, obj);
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
                        filename = "BM004_DanhSachNhanVien_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("ExcelNhanVien.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM004_EmployeeList_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("ExcelNhanVien_en.xls", dataSet, null, ref stream);
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
        [Route("ExportTeamplateNhanVien")]
        public HttpResponseMessage ExportTeamplateNhanVien()
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
                    DataTable dt = new DataTable();
                    dt.Columns.Add("tenDayDu", typeof(String));
                    dt.Columns.Add("tenDangNhap", typeof(String));
                    dt.Columns.Add("matKhau", typeof(String));
                    dt.Columns.Add("queQuan", typeof(String));
                    dt.Columns.Add("diaChi", typeof(String));
                    dt.Columns.Add("ngaySinh", typeof(String));
                    dt.Columns.Add("dienThoai", typeof(String));
                    dt.Columns.Add("email", typeof(String));
                    dt.Columns.Add("tenNhom", typeof(String));
                    dt.Columns.Add("iD_Nhom", typeof(String));
                    dt.Columns.Add("truongNhom", typeof(String));
                    dt.Columns.Add("gioiTinhName", typeof(String));
                    dt.Columns.Add("chucVu", typeof(String));
                    DataSet ds = new DataSet();
                    KhachHang_dl kh_dl = new KhachHang_dl();
                    NhomData nhom = new NhomData();
                    //DataTable dtkh = kh_dl.GetDataKhachHangAll(userinfo.ID_QLLH, userinfo.ID_QuanLy, 0, 0, -1, 0);
                    DataTable dtNhom = nhom.GetDsNhomByIdCongTy(userinfo.ID_QLLH);
                    // string templatefolder = System.Web.Configuration.WebConfigurationManager.AppSettings["ReportTemplatesFolder"];
                    ds.Tables.Add(dt.Copy());
                    ds.Tables.Add(dtNhom.Copy());
                    ds.Tables[0].TableName = "DATA";
                    ds.Tables[1].TableName = "DATA1";
                    for (int i1 = 1; i1 <= 2000; i1++)
                    {
                        var row = ds.Tables[0].NewRow();
                        ds.Tables[0].Rows.Add(row);
                    }

                    BaoCaoCommon baocao = new BaoCaoCommon();
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    string filename = "";
                    var stream = new MemoryStream();
                    ExportExcel excel = new ExportExcel();

                    if (lang == "vi")
                    {
                        filename = "BM005_FileMauNhanVien_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("TeamplateImport_NhanVien.xls", ds, null, ref stream);
                    }
                    else
                    {
                        filename = "BM005_EmployeeTemplate_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("TeamplateImport_NhanVien_en.xls", ds, null, ref stream);
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
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);

            }
            return response;
        }

        [HttpPost]
        [Route("importnhanvien")]
        public HttpResponseMessage importnhanvien([FromBody] FileUploadModelFilter file)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            MemoryStream tempPath = new MemoryStream();

            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("STT", typeof(String));
                    dt.Columns.Add("tenDayDu", typeof(String));
                    dt.Columns.Add("tenDangNhap", typeof(String));
                    dt.Columns.Add("matKhau", typeof(String));
                    dt.Columns.Add("queQuan", typeof(String));
                    dt.Columns.Add("diaChi", typeof(String));
                    dt.Columns.Add("ngaySinh", typeof(String));
                    dt.Columns.Add("gioiTinhName", typeof(String));
                    dt.Columns.Add("chucVu", typeof(String));
                    dt.Columns.Add("dienThoai", typeof(String));
                    dt.Columns.Add("email", typeof(String));
                    dt.Columns.Add("truongNhom", typeof(String));
                    dt.Columns.Add("tenNhom", typeof(String));
                    dt.Columns.Add("iD_Nhom", typeof(String));
                    dt.Columns.Add("gioiTinh", typeof(String));

                    Aspose.Cells.Workbook workbook;
                    Aspose.Cells.Worksheet worksheet;
                    DataTable dtDataHeard = new DataTable();
                    string templatefolder = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUpload"];
                    string fileName = AppDomain.CurrentDomain.BaseDirectory + templatefolder + @"\" + file.filename;
                    workbook = new Aspose.Cells.Workbook(fileName);
                    if (workbook.Worksheets.GetSheetByCodeName("DATA") == null)
                    {
                        return response = Request.CreateResponse(HttpStatusCode.NoContent, new { success = false, msg = "File mẫu không đúng định dạng." });
                    }

                    worksheet = workbook.Worksheets[0];
                    dt = worksheet.Cells.ExportDataTableAsString(2, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, true);
                    if (System.IO.File.Exists(fileName))
                        System.IO.File.Delete(fileName);
                    dtDataHeard = dt.Clone();
                    foreach (var row in dt.Rows)
                    {
                        DataRow _datarow = row as DataRow;
                        var isRow = ImportValidate.TrimRow(ref _datarow, false);
                        if (!isRow)
                            continue;
                        dtDataHeard.ImportRow(_datarow);
                    }
                    if (dtDataHeard.Rows.Count == 0)
                    {
                        return response = Request.CreateResponse(HttpStatusCode.NoContent, new { success = false, msg = "Không tồn tại bản ghi." });
                    }
                    if (dtDataHeard.Rows.Count > 2000)
                    {
                        return response = Request.CreateResponse(HttpStatusCode.LengthRequired, new { success = false, msg = "Dữ liệu trong file import không được quá 2000 dòng, vui lòng kiểm tra lại." });
                    }

                    if (validatedatanhanvien(dtDataHeard, userinfo, ref tempPath))
                    {
                        try
                        {
                            NhanVien_dl nv_dl = new NhanVien_dl();
                            foreach (DataRow row in dtDataHeard.Rows)
                            {
                                NhanVien obj = new NhanVien();
                                obj.IDNV = 0;
                                obj.IDQLLH = userinfo.ID_QLLH;
                                obj.ID_QuanLy = userinfo.ID_QuanLy;
                                obj.ID_ChucVu = 0;
                                obj.TenDangNhap = row["tenDangNhap"].ToString();
                                obj.TenDayDu = row["tenDayDu"].ToString();
                                obj.MatKhau = row["matKhau"].ToString();
                                obj.DiaChi = row["diaChi"].ToString();
                                obj.QueQuan = row["queQuan"].ToString();
                                obj.DienThoai = row["dienThoai"].ToString();
                                obj.Email = row["email"].ToString();
                                obj.ChucVu = row["chucVu"].ToString();

                                if ((row["iD_Nhom"].ToString() != "") && (row["iD_Nhom"].ToString() != "#N/A"))
                                    obj.ID_Nhom = int.Parse(row["iD_Nhom"].ToString().Trim());
                                if ((row["gioiTinh"].ToString() != "") && (row["gioiTinh"].ToString() != "#N/A"))
                                    obj.GioiTinh = int.Parse(row["gioiTinh"].ToString().Trim());
                                if ((row["truongNhom"].ToString() != "") && (row["truongNhom"].ToString() != "#N/A"))
                                    obj.TruongNhom = 1;

                                if (row["ngaySinh"].ToString() != "")
                                    obj.NgaySinh = DateTime.ParseExact(row["ngaySinh"].ToString(), "dd/MM/yyyy", null);
                                else
                                    obj.NgaySinh = new DateTime(1900, 1, 1);

                                if (!nv_dl.ThemNhanVien(obj))
                                    return response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                            }

                            return response = Request.CreateResponse(HttpStatusCode.OK, true);
                        }
                        catch (Exception ex)
                        {
                            LSPos_Data.Utilities.Log.Error(ex);
                            return response = Request.CreateResponse(HttpStatusCode.NotModified);
                        }
                    }
                    else
                    {
                        BaoCaoCommon baocao = new BaoCaoCommon();
                        string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                        string filename = "";
                        if (lang == "vi")
                        {
                            filename = "BM006_FileMauNhanVienError_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        }
                        else
                        {
                            filename = "BM006_EmployeeTemplateError_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        }

                        response.Content = new ByteArrayContent(tempPath.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.Created;
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        private bool validatedatanhanvien(DataTable dtDataHeard, UserInfo userinfo, ref MemoryStream fileStream)
        {
            try
            {
                BaoCaoCommon baocao = new BaoCaoCommon();
                string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                DataTable dtError = dtDataHeard.Clone();
                dtError.TableName = "DATA";

                List<string> lstEmp = new List<string>();

                int iRow = 4;
                bool IsError = false;
                foreach (DataRow row in dtDataHeard.Rows)
                {
                    string sError = "";
                    DataRow _datarow = row as DataRow;
                    var rowError = dtError.NewRow();
                    sError = (lang == "vi") ? "Tên đầy đủ chưa nhập" : "Full name is missing";
                    ImportValidate.EmptyValue("tenDayDu", ref _datarow, ref rowError, ref IsError, sError);
                    sError = (lang == "vi") ? "Tên đăng nhập chưa nhập" : "Username is missing";
                    ImportValidate.EmptyValue("tenDangNhap", ref _datarow, ref rowError, ref IsError, sError);
                    sError = (lang == "vi") ? "Mật khẩu chưa nhập" : "Password is missing";
                    ImportValidate.EmptyValue("matKhau", ref _datarow, ref rowError, ref IsError, sError);
                    sError = (lang == "vi") ? "Tên nhóm chưa nhập" : "Group name is missing";
                    ImportValidate.EmptyValue("tenNhom", ref _datarow, ref rowError, ref IsError, sError);
                    if ((row["tenDangNhap"].ToString() != ""))
                    {
                        NhanVien nhanV = NhanVien_dl.ChiTietNhanVienTheoTenDangNhap(row["tenDangNhap"].ToString(), userinfo.ID_QLLH);
                        if (nhanV != null && nhanV.IDNV > 0)
                        {
                            rowError["tenDangNhap"] = (lang == "vi") ? "Đã tồn tại tên đăng nhập trên hệ thống" : "Username is already exists";
                            IsError = true;
                        }
                    }
                    if (row["ngaySinh"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Ngày sinh không đúng định dạng" : "Invalid birth date format";
                        ImportValidate.IsValidDate("ngaySinh", ref _datarow, ref rowError, ref IsError, sError, lang);
                    }
                    if (row["tenNhom"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Tên nhóm" : "Group name";
                        ImportValidate.IsValidList("tenNhom", "iD_Nhom", ref _datarow, ref rowError, ref IsError, sError, lang);
                    }
                    if (row["email"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Email không đúng định dạng" : "Invalid email format";
                        ImportValidate.IsValidEmail("email", ref _datarow, ref rowError, ref IsError, sError);
                    }

                    if (row["gioiTinhName"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Giới tính" : "Gender";
                        ImportValidate.IsValidList("gioiTinhName", "gioiTinh", ref _datarow, ref rowError, ref IsError, sError, lang);
                    }
                    if (IsError)
                    {
                        rowError["STT"] = iRow;
                        dtError.Rows.Add(rowError);
                    }

                    iRow = (iRow + 1);
                    IsError = false;
                }
                if ((dtError.Rows.Count > 0))
                {
                    dtError.TableName = "DATA";

                    ExportExcel ExportExcel = new ExportExcel();
                    if (lang == "vi")
                    {
                        ExportExcel.ExportTemplateToStream("Danh_sach_nhan_vien_error.xls", dtError, null, ref fileStream);
                    }
                    else
                    {
                        ExportExcel.ExportTemplateToStream("Danh_sach_nhan_vien_error_en.xls", dtError, null, ref fileStream);
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return true;
        }
        #endregion

        [HttpPost]
        [Route("LoadExcelnhanvien")]
        public HttpResponseMessage LoadExcelnhanvien([FromBody] FileExcelUpload file)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            List<nhanvienParam> lstNhanvien = new List<nhanvienParam>();

            try
            {
                //DataTable dtDataHeard = new DataTable();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    return response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("STT", typeof(String));
                    dt.Columns.Add("tenDayDu", typeof(String));
                    dt.Columns.Add("tenDangNhap", typeof(String));
                    dt.Columns.Add("matKhau", typeof(String));
                    dt.Columns.Add("queQuan", typeof(String));
                    dt.Columns.Add("diaChi", typeof(String));
                    dt.Columns.Add("ngaySinh", typeof(String));
                    dt.Columns.Add("gioiTinhName", typeof(String));
                    dt.Columns.Add("chucVu", typeof(String));
                    dt.Columns.Add("dienThoai", typeof(String));
                    dt.Columns.Add("email", typeof(String));
                    dt.Columns.Add("truongNhom", typeof(String));
                    dt.Columns.Add("tenNhom", typeof(String));
                    dt.Columns.Add("iD_Nhom", typeof(String));
                    dt.Columns.Add("gioiTinh", typeof(String));
                    Aspose.Cells.Workbook workbook;
                    Aspose.Cells.Worksheet worksheet;
                    DataTable dtDataHeard = new DataTable();
                    string fileName = System.Web.HttpContext.Current.Server.MapPath(file.filename);
                    //fileName = System.IO.Path.Combine(savepath, Guid.NewGuid().ToString() + ".xls");
                    //file.SaveAs(fileName, true);
                    workbook = new Aspose.Cells.Workbook(fileName);
                    if (workbook.Worksheets.GetSheetByCodeName("DATA") == null)
                    {
                        return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "File mẫu không đúng định dạng." });
                        // Trả về file mẫu không đúng định dạng (file mẫu chiết xuất từ hệ thống)
                    }
                    worksheet = workbook.Worksheets[0];
                    dt = worksheet.Cells.ExportDataTableAsString(2, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, true);
                    if (System.IO.File.Exists(fileName))
                        System.IO.File.Delete(fileName);
                    dtDataHeard = dt.Clone();
                    foreach (var row in dt.Rows)
                    {
                        DataRow _datarow = row as DataRow;
                        var isRow = ImportValidate.TrimRow(ref _datarow, false);
                        if (!isRow)
                            continue;
                        dtDataHeard.ImportRow(_datarow);
                    }
                    if (dtDataHeard.Rows.Count == 0)
                    {
                        //204
                        return response = Request.CreateResponse(HttpStatusCode.NoContent, new { success = false, msg = "Không tồn tại bản ghi." });
                        //Trả về lỗi ko có dữ liệu import
                        //response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                    }

                    //  DateTime.ParseExact(tieuchi.fromdate, formatString, null);
                    //if (loadToGridnv(dtDataHeard, userinfo.ID_QLLH, ref tempPath))
                    //{
                    else
                    {
                        foreach (DataRow row in dtDataHeard.Rows)
                        {
                            nhanvienParam nhanvien = new nhanvienParam();
                            nhanvien.tenDayDu = row["tenDayDu"].ToString();
                            nhanvien.tenDangNhap = row["tenDangNhap"].ToString();
                            nhanvien.matKhau = row["matKhau"].ToString();
                            nhanvien.queQuan = row["queQuan"].ToString();
                            nhanvien.diaChi = row["diaChi"].ToString();
                            if (row["ngaySinh"].ToString() != "")
                            {
                                var abc = row["ngaySinh"].ToString();
                                int day = int.Parse(abc.Split('/')[0].ToString());
                                int month = int.Parse(abc.Split('/')[1].ToString());
                                int year = int.Parse(abc.Split('/')[2].ToString());
                                nhanvien.ngaySinh = new DateTime(year, month, day);
                            }
                            nhanvien.gioiTinhName = row["gioiTinhName"].ToString();
                            if ((row["gioiTinh"].ToString() != "") && (row["gioiTinh"].ToString() != "#N/A"))
                            {
                                nhanvien.gioiTinh = int.Parse(row["gioiTinh"].ToString());
                            }
                            nhanvien.chucVu = row["chucVu"].ToString();

                            nhanvien.dienThoai = row["dienThoai"].ToString();
                            nhanvien.email = row["email"].ToString();
                            if (row["truongNhom"].ToString().ToUpper() == "X")
                            {
                                nhanvien.truongNhom = 1;
                            }
                            else
                            {
                                nhanvien.truongNhom = 0;
                            }
                            nhanvien.tenNhom = row["tenNhom"].ToString();
                            if ((row["iD_Nhom"].ToString() != "") && (row["iD_Nhom"].ToString() != "#N/A"))
                            {
                                nhanvien.iD_Nhom = int.Parse(row["iD_Nhom"].ToString());
                            }
                            lstNhanvien.Add(nhanvien);
                        }
                        return response = Request.CreateResponse(HttpStatusCode.OK, lstNhanvien);
                    }
                    //}

                    //else
                    //{
                    //    var stream = new MemoryStream();
                    //    //205
                    //    response = new HttpResponseMessage(HttpStatusCode.ResetContent)
                    //    {
                    //        Content = new ByteArrayContent(stream.ToArray())
                    //    };
                    //    response.Content.Headers.ContentDisposition =
                    //        new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    //        {
                    //            FileName = tempPath
                    //        };
                    //    response.Content.Headers.ContentType =
                    //        new MediaTypeHeaderValue("application/octet-stream");
                    //}
                }

            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        [HttpPost]
        [Route("checkfile")]
        public HttpResponseMessage checkfile([FromBody] FileExcelUpload file)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            List<nhanvienParam> lstNhanvien = new List<nhanvienParam>();
            MemoryStream tempPath = new MemoryStream();
            try
            {
                //DataTable dtDataHeard = new DataTable();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    return response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("STT", typeof(String));
                    dt.Columns.Add("tenDayDu", typeof(String));
                    dt.Columns.Add("tenDangNhap", typeof(String));
                    dt.Columns.Add("matKhau", typeof(String));
                    dt.Columns.Add("queQuan", typeof(String));
                    dt.Columns.Add("diaChi", typeof(String));
                    dt.Columns.Add("ngaySinh", typeof(String));
                    dt.Columns.Add("gioiTinhName", typeof(String));
                    dt.Columns.Add("chucVu", typeof(String));
                    dt.Columns.Add("dienThoai", typeof(String));
                    dt.Columns.Add("email", typeof(String));
                    dt.Columns.Add("truongNhom", typeof(String));
                    dt.Columns.Add("tenNhom", typeof(String));
                    dt.Columns.Add("iD_Nhom", typeof(String));
                    dt.Columns.Add("gioiTinh", typeof(String));

                    Aspose.Cells.Workbook workbook;
                    Aspose.Cells.Worksheet worksheet;
                    DataTable dtDataHeard = new DataTable();
                    string fileName = System.Web.HttpContext.Current.Server.MapPath(file.filename);
                    //fileName = System.IO.Path.Combine(savepath, Guid.NewGuid().ToString() + ".xls");
                    //file.SaveAs(fileName, true);
                    workbook = new Aspose.Cells.Workbook(fileName);
                    if (workbook.Worksheets.GetSheetByCodeName("DATA") == null)
                    {
                        return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "File mẫu không đúng định dạng." });
                        // Trả về file mẫu không đúng định dạng (file mẫu chiết xuất từ hệ thống)
                    }
                    worksheet = workbook.Worksheets[0];
                    dt = worksheet.Cells.ExportDataTableAsString(2, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, true);
                    dtDataHeard = dt.Clone();
                    foreach (var row in dt.Rows)
                    {
                        DataRow _datarow = row as DataRow;
                        var isRow = ImportValidate.TrimRow(ref _datarow, false);
                        if (!isRow)
                            continue;
                        dtDataHeard.ImportRow(_datarow);
                    }
                    if (dtDataHeard.Rows.Count == 0)
                    {
                        //204
                        return response = Request.CreateResponse(HttpStatusCode.NoContent, new { success = false, msg = "Không tồn tại bản ghi." });
                        //Trả về lỗi ko có dữ liệu import
                        //response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                    }
                    if (loadToGridnv(dtDataHeard, userinfo.ID_QLLH, ref tempPath))
                    {
                        //foreach (DataRow row in dtDataHeard.Rows)
                        //{
                        //    nhanvienParam nhanvien = new nhanvienParam();
                        //    nhanvien.tenDayDu = row["tenDayDu"].ToString();
                        //    nhanvien.tenDangNhap = row["tenDangNhap"].ToString();
                        //    nhanvien.matKhau = row["matKhau"].ToString();
                        //    nhanvien.queQuan = row["queQuan"].ToString();
                        //    nhanvien.diaChi = row["diaChi"].ToString();
                        //    if (row["ngaySinh"].ToString() != "")
                        //    {
                        //        nhanvien.ngaySinh = DateTime.Parse(row["ngaySinh"].ToString());
                        //    }

                        //    nhanvien.gioiTinhName = row["gioiTinhName"].ToString();
                        //    if ((row["gioiTinh"].ToString() != "") && (row["gioiTinh"].ToString() != "#N/A"))
                        //    {
                        //        nhanvien.gioiTinh = int.Parse(row["gioiTinh"].ToString());
                        //    }
                        //    nhanvien.chucVu = row["chucVu"].ToString();

                        //    nhanvien.dienThoai = row["dienThoai"].ToString();
                        //    nhanvien.email = row["email"].ToString();
                        //    if (row["truongNhom"].ToString().ToUpper() != "X")
                        //    {
                        //        nhanvien.truongNhom = 1;
                        //    }
                        //    else
                        //    {
                        //        nhanvien.truongNhom = 0;
                        //    }
                        //    nhanvien.tenNhom = row["tenNhom"].ToString();
                        //    if ((row["iD_Nhom"].ToString() != "") && (row["iD_Nhom"].ToString() != "#N/A"))
                        //    {
                        //        nhanvien.iD_Nhom = int.Parse(row["iD_Nhom"].ToString());
                        //    }
                        //    lstNhanvien.Add(nhanvien);
                        //}
                        return response = Request.CreateResponse(HttpStatusCode.Created, lstNhanvien);
                    }

                    else
                    {
                        response = new HttpResponseMessage(HttpStatusCode.OK)
                        {
                            Content = new ByteArrayContent(tempPath.ToArray())
                        };
                        response.Content.Headers.ContentDisposition =
                            new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                            {
                                FileName = "DataError" + DateTime.Now.ToString("yyyyMMdd-HHMMss") + ".xls"
                            };
                        response.Content.Headers.ContentType =
                            new MediaTypeHeaderValue("application/octet-stream");
                    }
                }

            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        private bool loadToGridnv(DataTable dtDataHeard, int ID_QLLH, ref MemoryStream fileStream)
        {
            //try
            //{
            //    DataTable dtError = dtDataHeard.Clone();
            //    dtError.TableName = "DATA";
            //    int iRow = 4;
            //    bool IsError = false;
            //    foreach (DataRow row in dtDataHeard.Rows)
            //    {
            //        string sError = "";
            //        DataRow _datarow = row as DataRow;
            //        var rowError = dtError.NewRow();
            //        sError = "Tên đầy đủ chưa nhập";
            //        ImportValidate.EmptyValue("tenDayDu", ref _datarow, ref rowError, ref IsError, sError);
            //        sError = "Tên đăng nhập chưa nhập";
            //        ImportValidate.EmptyValue("tenDangNhap", ref _datarow, ref rowError, ref IsError, sError);
            //        sError = "Mật khẩu chưa nhập";
            //        ImportValidate.EmptyValue("matKhau", ref _datarow, ref rowError, ref IsError, sError);
            //        sError = "Tên nhóm chưa nhập";
            //        ImportValidate.EmptyValue("tenNhom", ref _datarow, ref rowError, ref IsError, sError);
            //        if ((row["tenDangNhap"].ToString() != ""))
            //        {
            //            NhanVien nhanV = NhanVien_dl.ChiTietNhanVienTheoTenDangNhap(row["tenDangNhap"].ToString(), ID_QLLH);
            //            if (nhanV != null && nhanV.IDNV > 0)
            //            {
            //                rowError["tenDangNhap"] = "Đã tồn tại tên đăng nhập trên hệ thống";
            //                IsError = true;
            //            }
            //        }
            //        if (row["ngaySinh"].ToString() != "")
            //        {
            //            sError = "Ngày sinh không đúng định dạng";
            //            ImportValidate.IsValidDate("ngaySinh", ref _datarow, ref rowError, ref IsError, sError);
            //        }
            //        if (row["tenNhom"].ToString() != "")
            //        {
            //            sError = "Tên nhóm";
            //            ImportValidate.IsValidList("tenNhom", "iD_Nhom", ref _datarow, ref rowError, ref IsError, sError, lang);
            //        }
            //        if (row["email"].ToString() != "")
            //        {
            //            sError = "Email không đúng định dạng";
            //            ImportValidate.IsValidEmail("email", ref _datarow, ref rowError, ref IsError, sError);
            //        }

            //        if (row["gioiTinhName"].ToString() != "")
            //        {
            //            sError = "Giới tính";
            //            ImportValidate.IsValidList("gioiTinhName", "gioiTinh", ref _datarow, ref rowError, ref IsError, sError, lang);
            //        }
            //        if (IsError)
            //        {
            //            rowError["STT"] = iRow;
            //            dtError.Rows.Add(rowError);
            //        }

            //        iRow = (iRow + 1);
            //        IsError = false;
            //    }
            //    if ((dtError.Rows.Count > 0))
            //    {
            //        dtError.TableName = "DATA";
            //        ExportExcel ExportExcel = new ExportExcel();
            //        bool success = ExportExcel.ExportTemplateToStream(@"ReportTemplates\Danh_sach_nhan_vien_error.xls", dtError, null, ref fileStream);
            //        return false;
            //        //return tempPath = "";
            //    }
            //    else
            //    {
            //        return true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LSPos_Data.Utilities.Log.Error(ex);
            //}
            return true;
        }


        [HttpGet]
        [Route("exportexcel")]
        public HttpResponseMessage exportexcel()
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
                    DataTable dt = new DataTable();
                    dt.Columns.Add("tenDayDu", typeof(String));
                    dt.Columns.Add("tenDangNhap", typeof(String));
                    dt.Columns.Add("matKhau", typeof(String));
                    dt.Columns.Add("queQuan", typeof(String));
                    dt.Columns.Add("diaChi", typeof(String));
                    dt.Columns.Add("ngaySinh", typeof(String));
                    dt.Columns.Add("dienThoai", typeof(String));
                    dt.Columns.Add("email", typeof(String));
                    dt.Columns.Add("tenNhom", typeof(String));
                    dt.Columns.Add("iD_Nhom", typeof(String));

                    DataSet ds = new DataSet();
                    KhachHang_dl kh_dl = new KhachHang_dl();
                    NhomData nhom = new NhomData();
                    //DataTable dtkh = kh_dl.GetDataKhachHangAll(userinfo.ID_QLLH, userinfo.ID_QuanLy, 0, 0, -1, 0);
                    DataTable dtNhom = nhom.GetDsNhomByIdCongTy(userinfo.ID_QLLH);
                    // string templatefolder = System.Web.Configuration.WebConfigurationManager.AppSettings["ReportTemplatesFolder"];
                    ds.Tables.Add(dt.Copy());
                    ds.Tables.Add(dtNhom.Copy());
                    ds.Tables[0].TableName = "DATA";
                    ds.Tables[1].TableName = "DATA1";
                    for (int i1 = 1; i1 <= 100; i1++)
                    {
                        var row = ds.Tables[0].NewRow();
                        ds.Tables[0].Rows.Add(row);
                    }
                    string tempPath = "";
                    ExportTemplate(@"ReportTemplates\Danh_sach_nhan_vien.xlsx", ds, null, ref tempPath);



                    //var stream = new MemoryStream();
                    // var result = new HttpResponseMessage(HttpStatusCode.OK)
                    //{
                    //    Content = new ByteArrayContent(stream.ToArray())
                    //};
                    //result.Content.Headers.ContentDisposition =
                    //    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    //    {
                    //        FileName = tempPath;
                    //    }
                    //result.Content.Headers.ContentType =
                    //    new MediaTypeHeaderValue("application/octet-stream");

                    //return result;

                    // response = Request.CreateResponse(HttpStatusCode.OK);
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
        [Route("getnhanvienbodauxoa")]
        public HttpResponseMessage getnhanvienbodauxoa()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();
                //get token string from Headers Request
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];
                //decode token string
                var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                string isNhanVien = token.Claims.First(c => c.Type == "IsNhanVien").Value;
                int idQuanLy = userinfo.ID_QuanLy;
                if (isNhanVien == "1")
                    idQuanLy = 0;

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    NhanVien_dl nv_dl = new NhanVien_dl();
                    List<NhanVien> dsnv = nv_dl.GetDSNhanVien_LoaiBoDanhDauXoa(userinfo.ID_QLLH, idQuanLy);


                    response = Request.CreateResponse(HttpStatusCode.OK, dsnv);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        #region thêm sửa xóa
        [HttpPost]
        [Route("themmoinhanvien")]
        public HttpResponseMessage add([FromBody] NhanVienNew nhanVienNew)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                string formatString = "dd/MM/yyyy";
                if (nhanVienNew.NgaySinh != "")
                {
                    try
                    {
                        nhanVienNew.datenow = DateTime.ParseExact(nhanVienNew.NgaySinh, formatString, null);
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                        nhanVienNew.datenow = new DateTime(1900, 1, 1);
                    }
                }
                else
                {
                    nhanVienNew.datenow = new DateTime(1900, 1, 1);
                }

                if (userinfo == null)
                {
                    return response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    NhanVien_dl nv_dl = new NhanVien_dl();
                    NhanVien nhanV = NhanVien_dl.ChiTietNhanVienTheoTenDangNhap(nhanVienNew.TenDangNhap, userinfo.ID_QLLH);
                    if (nhanV != null && nhanV.IDNV > 0)
                    {
                        if (nhanV.TrangThaiXoa > 0)
                        {
                            return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "Tài khoản đã từng tồn tại nhấn vào <a href='DSNhanVien.aspx?khoiphucnhanvien=" + nhanV.IDNV + "'> đây </a> để khôi phục." });
                        }
                        else
                        {
                            return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_tacvuthuchienkhongthanhcongdatontaitaikhoantronghethong" });
                        }
                    }
                    NhanVien nhanvien = new NhanVien();
                    nhanvien.IDNV = nhanVienNew.IDNV;
                    nhanvien.TenDangNhap = nhanVienNew.TenDangNhap;
                    nhanvien.TenDayDu = nhanVienNew.TenDayDu;
                    nhanvien.MatKhau = nhanVienNew.MatKhau;
                    nhanvien.ID_Nhom = nhanVienNew.ID_Nhom;
                    nhanvien.DiaChi = nhanVienNew.DiaChi;
                    nhanvien.QueQuan = nhanVienNew.QueQuan;
                    nhanvien.NgaySinh = nhanVienNew.datenow;
                    nhanvien.DienThoai = nhanVienNew.DienThoai;
                    nhanvien.Email = nhanVienNew.Email == null ? "" : nhanVienNew.Email;
                    nhanvien.TruongNhom = nhanVienNew.TruongNhom;
                    nhanvien.ChucVu = nhanVienNew.chucVu;
                    nhanvien.GioiTinh = nhanVienNew.GioiTinh;
                    nhanvien.IDQLLH = userinfo.ID_QLLH;
                    nhanvien.ID_QuanLy = userinfo.ID_QuanLy;
                    nhanvien.ID_ChucVu = 0;

                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(nhanvien);
                    LSPos_Data.Utilities.Log.Info("Data Add NhanVien:" + output);

                    if (nv_dl.ThemNhanVien(nhanvien))
                    {
                        int ID_NhanVien_New = nv_dl.GetLastNhanVienID(userinfo.ID_QLLH);
                        LSPos_Data.Utilities.Log.Info("Data Add NhanVien Thành công:" + ID_NhanVien_New.ToString());
                        try
                        {
                            if (nhanVienNew.image_url != "")
                            {
                                string teampatch = AppDomain.CurrentDomain.BaseDirectory + nhanVienNew.image_url;
                                if (File.Exists(teampatch))
                                {
                                    byte[] binData = GetBytesFromFile(teampatch);

                                    if (binData != null && binData.Length > 0)
                                    {
                                        string svURL = Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]);
                                        string strLinkServer = svURL + "/AppUpload_AnhNhanVien.aspx?token=6e22b116f5111220741848ccd290e9e9062522d88a1fb00ba9b168db7a480271&idnhanvien=" + ID_NhanVien_New + "&kinhdo=0&vido=0&ghichu=Upload ảnh từ web&thoigianchup=" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "&idqllh=" + userinfo.ID_QLLH + "&imagename=" + "test.jpg";
                                        PostMultipleFiles_Stream(strLinkServer, binData, "test.jpg");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LSPos_Data.Utilities.Log.Error(ex);
                        }

                        foreach (object objKHID in nhanVienNew.listIdKH)
                        {
                            try
                            {
                                int IDKH = Convert.ToInt32(objKHID);
                                if (nv_dl.CapNhatKhachHangChoNhanVien(ID_NhanVien_New, IDKH))
                                {
                                    nv_dl.LuuLichSuChuyenGiaoKH(userinfo.ID_QLLH, ID_NhanVien_New, IDKH);
                                }
                            }
                            catch (Exception ex)
                            {
                                response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                                LSPos_Data.Utilities.Log.Error(ex);
                            }
                        }
                        string output1 = Newtonsoft.Json.JsonConvert.SerializeObject(nhanvien);
                        //Utils.GhiLog(userinfo.ID_QLLH, userinfo.ID_QuanLy, "Quản lý nhân viên", "Thêm mới nhân viên", "Data:" + output1, "");

                        return response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_tacvuthuchienthanhcong" });
                    }
                    else
                    {
                        return response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        [HttpPost]
        [Route("themmoinhanvien_v1")]
        public HttpResponseMessage add_v1([FromBody] NhanVienNew nhanVienNew)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                string formatString = "dd/MM/yyyy";
                if (nhanVienNew.NgaySinh != "")
                {
                    try
                    {
                        nhanVienNew.datenow = DateTime.ParseExact(nhanVienNew.NgaySinh, formatString, null);
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                        nhanVienNew.datenow = new DateTime(1900, 1, 1);
                    }
                }
                else
                {
                    nhanVienNew.datenow = new DateTime(1900, 1, 1);
                }

                if (userinfo == null)
                {
                    return response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    NhanVien_dl nv_dl = new NhanVien_dl();
                    NhanVien nhanV = NhanVien_dl.ChiTietNhanVienTheoTenDangNhap(nhanVienNew.TenDangNhap, userinfo.ID_QLLH);
                    if (nhanV != null && nhanV.IDNV > 0)
                    {
                        if (nhanV.TrangThaiXoa > 0)
                        {
                            return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "Tài khoản đã từng tồn tại nhấn vào <a href='DSNhanVien.aspx?khoiphucnhanvien=" + nhanV.IDNV + "'> đây </a> để khôi phục." });
                        }
                        else
                        {
                            return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_tacvuthuchienkhongthanhcongdatontaitaikhoantronghethong" });
                        }
                    }
                    NhanVienModels nhanvien = new NhanVienModels();
                    nhanvien.IDNV = nhanVienNew.IDNV;
                    nhanvien.TenDangNhap = nhanVienNew.TenDangNhap;
                    nhanvien.TenDayDu = nhanVienNew.TenDayDu;
                    nhanvien.MatKhau = nhanVienNew.MatKhau;
                    nhanvien.ID_Nhom = nhanVienNew.ID_Nhom;
                    nhanvien.DiaChi = nhanVienNew.DiaChi;
                    nhanvien.QueQuan = nhanVienNew.QueQuan;
                    nhanvien.NgaySinh = nhanVienNew.datenow;
                    nhanvien.DienThoai = nhanVienNew.DienThoai;
                    nhanvien.Email = nhanVienNew.Email == null ? "" : nhanVienNew.Email;
                    nhanvien.TruongNhom = nhanVienNew.TruongNhom;
                    nhanvien.ChucVu = nhanVienNew.chucVu;
                    nhanvien.GioiTinh = nhanVienNew.GioiTinh;
                    nhanvien.IDQLLH = userinfo.ID_QLLH;
                    nhanvien.ID_QuanLy = userinfo.ID_QuanLy;
                    nhanvien.ID_ChucVu = 0;
                    nhanvien.Loai = 1;
                    nhanvien.ID_NhomKhachHang_MacDinh = nhanVienNew.ID_NhomKhachHang_MacDinh;

                    string output = Newtonsoft.Json.JsonConvert.SerializeObject(nhanvien);
                    LSPos_Data.Utilities.Log.Info("Data Add NhanVien:" + output);

                    NhanVienApp nhanVienApp = new NhanVienApp();
                    int re = nhanVienApp.ThemNhanVien_v1(nhanvien);
                    if (re > 0)
                    {
                        int ID_NhanVien_New = nv_dl.GetLastNhanVienID(userinfo.ID_QLLH);
                        LSPos_Data.Utilities.Log.Info("Data Add NhanVien Thành công:" + ID_NhanVien_New.ToString());
                        try
                        {
                            if (nhanVienNew.image_url != "")
                            {
                                string teampatch = AppDomain.CurrentDomain.BaseDirectory + nhanVienNew.image_url;
                                if (File.Exists(teampatch))
                                {
                                    byte[] binData = GetBytesFromFile(teampatch);

                                    if (binData != null && binData.Length > 0)
                                    {
                                        string svURL = Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]);
                                        string strLinkServer = svURL + "/AppUpload_AnhNhanVien.aspx?token=6e22b116f5111220741848ccd290e9e9062522d88a1fb00ba9b168db7a480271&idnhanvien=" + ID_NhanVien_New + "&kinhdo=0&vido=0&ghichu=Upload ảnh từ web&thoigianchup=" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "&idqllh=" + userinfo.ID_QLLH + "&imagename=" + "test.jpg";
                                        PostMultipleFiles_Stream(strLinkServer, binData, "test.jpg");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LSPos_Data.Utilities.Log.Error(ex);
                        }

                        foreach (object objKHID in nhanVienNew.listIdKH)
                        {
                            try
                            {
                                int IDKH = Convert.ToInt32(objKHID);
                                if (nv_dl.CapNhatKhachHangChoNhanVien(ID_NhanVien_New, IDKH))
                                {
                                    nv_dl.LuuLichSuChuyenGiaoKH(userinfo.ID_QLLH, ID_NhanVien_New, IDKH);
                                }
                            }
                            catch (Exception ex)
                            {
                                response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                                LSPos_Data.Utilities.Log.Error(ex);
                            }
                        }
                        //string output1 = Newtonsoft.Json.JsonConvert.SerializeObject(nhanvien);
                        //Utils.GhiLog(userinfo.ID_QLLH, userinfo.ID_QuanLy, "Quản lý nhân viên", "Thêm mới nhân viên", "Data:" + output1, "");

                        return response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_tacvuthuchienthanhcong" });
                    }
                    else if (re <= -1)
                    {
                        return response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_soluongnhanviendavuotquagioichophep" });
                    }
                    else
                    {
                        return response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        [HttpPost]
        [Route("themnhanvienkhongdangnhap")]
        public HttpResponseMessage addnhanvienkhongdangnhap([FromBody] NhanVienModels nhanVienModels)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();
                if (userinfo == null)
                {
                    return response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    nhanVienModels.IDQLLH = userinfo.ID_QLLH;
                    nhanVienModels.ID_QuanLy = userinfo.ID_QuanLy;
                    NhanVienApp nhanVienApp = new NhanVienApp();
                    int re = nhanVienApp.ThemNhanVienKhongDangNhap(nhanVienModels);

                    if (re <= -1)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_soluongnhanviendavuotquagioichophep" });
                    }
                    else if (re == 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_tacvuthuchienthanhcong" });
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        [HttpPost]
        [Route("suanhanvien")]
        public HttpResponseMessage edit([FromBody] NhanVienNew nhanVienupdate)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                string formatString = "dd/MM/yyyy";
                if (nhanVienupdate.NgaySinh != "")
                {
                    try
                    {
                        nhanVienupdate.datenow = DateTime.ParseExact(nhanVienupdate.NgaySinh, formatString, null);
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                        nhanVienupdate.datenow = new DateTime(1900, 1, 1);
                    }
                }
                else
                {
                    nhanVienupdate.datenow = new DateTime(1900, 1, 1);
                }

                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    return response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    NhanVienApp NhanVienApp = new NhanVienApp();
                    NhanVien_dl nv_dl = new NhanVien_dl();
                    NhanVien nvOBJ = NhanVien_dl.ChiTietNhanVienTheoTenDangNhap(nhanVienupdate.TenDangNhap, userinfo.ID_QLLH);
                    if (nvOBJ != null && nvOBJ.IDNV != nhanVienupdate.IDNV)
                    {
                        return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "Tác vụ thực hiện không thành công. Đã tồn tại tại tài khoản trong hệ thống" });
                    }
                    NhanVien nhanvien = new NhanVien();
                    nhanvien.IDNV = nhanVienupdate.IDNV;
                    nhanvien.TenDangNhap = nhanVienupdate.TenDangNhap;
                    nhanvien.TenDayDu = nhanVienupdate.TenDayDu;
                    nhanvien.MatKhau = nhanVienupdate.MatKhau;
                    nhanvien.ID_Nhom = nhanVienupdate.ID_Nhom;
                    nhanvien.DiaChi = nhanVienupdate.DiaChi;
                    nhanvien.QueQuan = nhanVienupdate.QueQuan;
                    nhanvien.NgaySinh = nhanVienupdate.datenow;
                    nhanvien.DienThoai = nhanVienupdate.DienThoai == null ? "" : nhanVienupdate.DienThoai;
                    //nhanvien.DienThoai = nhanVienupdate.DienThoai;
                    nhanvien.Email = nhanVienupdate.Email == null ? "" : nhanVienupdate.Email;
                    nhanvien.TruongNhom = nhanVienupdate.TruongNhom;
                    nhanvien.ChucVu = nhanVienupdate.chucVu;
                    nhanvien.GioiTinh = nhanVienupdate.GioiTinh;
                    nhanvien.IDQLLH = userinfo.ID_QLLH;
                    nhanvien.ID_QuanLy = userinfo.ID_QuanLy;
                    nhanvien.ID_ChucVu = 0;
                    nhanvien.ID_NhomKhachHang_MacDinh = nhanVienupdate.ID_NhomKhachHang_MacDinh;
                    nhanvien.TrucTuyen = nhanVienupdate.chkDoiMatKhau;
                    if (NhanVienApp.CapNhatNhanVien(nhanvien, userinfo.ID_QuanLy))
                    {
                        try
                        {
                            if (nhanVienupdate.image_url != "")
                            {
                                string teampatch = AppDomain.CurrentDomain.BaseDirectory + nhanVienupdate.image_url;
                                LSPos_Data.Utilities.Log.Info(nhanVienupdate.image_url);
                                LSPos_Data.Utilities.Log.Info(teampatch);

                                if (File.Exists(teampatch))
                                {
                                    byte[] binData = GetBytesFromFile(teampatch);

                                    if (binData != null && binData.Length > 0)
                                    {
                                        string svURL = Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]);
                                        string strLinkServer = svURL + "/AppUpload_AnhNhanVien.aspx?token=6e22b116f5111220741848ccd290e9e9062522d88a1fb00ba9b168db7a480271&idnhanvien=" + nhanVienupdate.IDNV + "&kinhdo=0&vido=0&ghichu=Upload ảnh từ web&thoigianchup=" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "&idqllh=" + userinfo.ID_QLLH + "&imagename=" + "test.jpg";
                                        PostMultipleFiles_Stream(strLinkServer, binData, "test.jpg");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LSPos_Data.Utilities.Log.Error(ex);
                        }

                        return response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_tacvuthuchienthanhcong" });
                    }
                    else
                    {
                        return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_phatsinhloitrongquatrinhthaotactacvuthuchienkhongthanhcong" });
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        [HttpPost]
        [Route("xoanhanvien")]
        public HttpResponseMessage DeleteEmployee([FromUri] string IDNV)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    return response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    NhanVienApp nv_app = new NhanVienApp();
                    DataTable dt = nv_app.GetKhachHangDaCapQuyen(IDNV, -1, 0, 0, 0, 0);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_tontainhanviendacokhachangquanlycanchuyenkhachhangchonhanvienkhactruockhixoa" });
                    }
                    else
                    {
                        if (nv_app.XoaNhanvien(IDNV))
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_tacvuthuchienthanhcong" });
                        else
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_phatsinhloitrongquatrinhthaotactacvuthuchienkhongthanhcong" });
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

        #endregion
        [HttpPost]
        [Route("resetimei")]
        public HttpResponseMessage ResetImei([FromUri] int IDNV)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    return response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    NhanVienApp nv_app = new NhanVienApp();
                    bool result = nv_app.ResetImei(IDNV);
                    if (result)
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "Tác vụ thực hiện thành công" });
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "Phát sinh lỗi trong quá trình thao tác. Tác vụ thực hiện không thành công" });

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
        [Route("updateapppush")]
        public HttpResponseMessage UpdateAppPush([FromUri] string AppPush)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    return response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    NhanVienApp nv_app = new NhanVienApp();
                    bool result = nv_app.UpdateIDPush((userinfo.IsAdmin ? userinfo.ID_QuanLy: 0), 
                        (userinfo.IsAdmin ? 0 : userinfo.ID_QuanLy), 
                        AppPush);
                    if (result)
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "Tác vụ thực hiện thành công" });
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "Phát sinh lỗi trong quá trình thao tác. Tác vụ thực hiện không thành công" });

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
        [Route("removephanquyen")]
        public HttpResponseMessage removephanquyen([FromUri] int idnhanvien, string lstidkh)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    return response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {

                    string[] strIDKH = lstidkh.Split(',');
                    bool result = true;
                    NhanVienApp nv_dl = new NhanVienApp();
                    foreach (object objKHID in strIDKH)
                    {
                        try
                        {
                            int IDKH = Convert.ToInt32(objKHID);

                            if (nv_dl.XoaPhanQuyenKhachHangChoNhanVien(idnhanvien, IDKH))
                            {
                                result = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                            LSPos_Data.Utilities.Log.Error(ex);
                        }
                    }



                    if (result)
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_tacvuthuchienthanhcong" });
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_phatsinhloitrongquatrinhthaotactacvuthuchienkhongthanhcong" });

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
        [Route("addphanquyen")]
        public HttpResponseMessage addphanquyen([FromUri] int idnhanvien, string lstidkh)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    return response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {

                    string[] strIDKH = lstidkh.Split(',');
                    bool result = true;
                    NhanVienApp nv_dl = new NhanVienApp();
                    foreach (object objKHID in strIDKH)
                    {
                        try
                        {
                            int IDKH = Convert.ToInt32(objKHID);

                            if (nv_dl.CapNhatKhachHangChoNhanVien(idnhanvien, IDKH))
                            {
                                result = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                            LSPos_Data.Utilities.Log.Error(ex);
                        }
                    }

                    if (result)
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "Tác vụ thực hiện thành công" });
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "Phát sinh lỗi trong quá trình thao tác. Tác vụ thực hiện không thành công" });
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
            LSPos_Data.Utilities.Log.Info(url);
            LSPos_Data.Utilities.Log.Info(filename);

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
                LSPos_Data.Utilities.Log.Info(var);

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            httpWebRequest = null;
        }


        [HttpPost]
        [Route("saveexceldata")]
        public HttpResponseMessage SaveExcelData([FromBody] List<nhanvienParam> lparamkh)
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
                    int count = 0;
                    NhanVien_dl nv_dl = new NhanVien_dl();
                    foreach (nhanvienParam paramnv in lparamkh)
                    {
                        count++;
                        try
                        {
                            // DateTime _ngay = DateTime.ParseExact(param.tieuchiloc.fromdate, formatString, null);
                            NhanVien nhanvien = new NhanVien();
                            nhanvien.IDNV = paramnv.ID_NV;
                            nhanvien.TenDangNhap = paramnv.tenDangNhap;
                            nhanvien.TenDayDu = paramnv.tenDayDu;
                            nhanvien.MatKhau = paramnv.matKhau;
                            nhanvien.ID_Nhom = paramnv.iD_Nhom;
                            nhanvien.DiaChi = paramnv.diaChi;
                            nhanvien.QueQuan = paramnv.queQuan;
                            nhanvien.NgaySinh = paramnv.ngaySinh.Year <= 1900 ? new DateTime(1900, 1, 1) : paramnv.ngaySinh;
                            nhanvien.DienThoai = paramnv.dienThoai;
                            nhanvien.Email = paramnv.email == null ? "" : paramnv.email;
                            nhanvien.TruongNhom = paramnv.truongNhom;
                            nhanvien.ChucVu = paramnv.chucVu;
                            nhanvien.GioiTinh = paramnv.gioiTinh;
                            nhanvien.IDQLLH = userinfo.ID_QLLH;
                            nhanvien.ID_QuanLy = userinfo.ID_QuanLy;
                            nhanvien.ID_ChucVu = 0;
                            if (nv_dl.ThemNhanVien(nhanvien))
                            {
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "Tác vụ thực hiện thành công" });
                            }
                        }

                        catch (Exception ex)
                        {
                            LSPos_Data.Utilities.Log.Info("Lỗi insert thông tin dòng dữ liệu số " + count);
                            LSPos_Data.Utilities.Log.Error(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }


        public bool ExportTemplate(string sReportFileName, DataSet dsData, DataTable dtVariable, ref string filename)
        {
            string filePath;
            string templatefolder;

            WorkbookDesigner designer;
            try
            {
                templatefolder = System.Web.Configuration.WebConfigurationManager.AppSettings["ReportTemplatesFolder"];
                //templatefolder = WebConfigurationManager.AppSettings("ReportTemplatesFolder");
                filePath = AppDomain.CurrentDomain.BaseDirectory + templatefolder + @"\" + sReportFileName;

                if (!File.Exists(filePath))
                {
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "alert('msg');", true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "javascriptfunction", "goBack()", true);
                    //ScriptManager.RegisterStartupScript(this.Page, GetType(), "alert", "alert('msg');", true);
                    return false;
                }

                Workbook wb = new Workbook(filePath);
                designer = new WorkbookDesigner(wb);
                designer.SetDataSource(dsData);
                if (dtVariable != null)
                {
                    int intCols = dtVariable.Columns.Count;
                    for (int i = 0; i <= intCols - 1; i++)
                        designer.SetDataSource(dtVariable.Columns[i].ColumnName.ToString(), dtVariable.Rows[0].ItemArray[i].ToString());
                }
                string fileNameNV = "TemplateImport_nhanvien";
                string teamplate = fileNameNV + DateTime.Now.ToString("yyyyMMdd-HHMMss") + ".xls";
                designer.Process();
                designer.Workbook.CalculateFormula();
                //designer.Workbook.Save(_fileSave, new XlsSaveOptions(SaveFormat.Xlsx));
                designer.Workbook.Save(HttpContext.Current.Response, teamplate, ContentDisposition.Attachment, new XlsSaveOptions());
                filename = teamplate;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
            return true;
        }


        [HttpPost]
        [Route("LoadExcelKhachHang")]
        public HttpResponseMessage LoadExcelKhachHang([FromBody] FileExcelUpload file)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            List<KhachHangParam> lstNhanvien = new List<KhachHangParam>();
            string tempPath = "";
            try
            {
                //DataTable dtDataHeard = new DataTable();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    return response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("STT", typeof(String));
                    dt.Columns.Add("MaKH", typeof(String));
                    dt.Columns.Add("TenKH", typeof(String));
                    dt.Columns.Add("DiaChi", typeof(String));
                    dt.Columns.Add("KinhDo", typeof(String));
                    dt.Columns.Add("ViDo", typeof(String));
                    dt.Columns.Add("DuongPho", typeof(String));
                    dt.Columns.Add("KhuVuc", typeof(String));
                    dt.Columns.Add("Tinh", typeof(String));
                    dt.Columns.Add("Quan", typeof(String));
                    dt.Columns.Add("Phuong", typeof(String));
                    dt.Columns.Add("DienThoai", typeof(String));
                    dt.Columns.Add("Fax", typeof(String));
                    dt.Columns.Add("DienThoai1", typeof(String));
                    dt.Columns.Add("DienThoai2", typeof(String));
                    dt.Columns.Add("LoaiKhachHang", typeof(String));
                    dt.Columns.Add("KenhBanHang", typeof(String));
                    dt.Columns.Add("KenhBanHangCapTren", typeof(String));
                    dt.Columns.Add("NguoiLienHe", typeof(String));
                    dt.Columns.Add("HomThu", typeof(String));
                    dt.Columns.Add("Website", typeof(String));
                    dt.Columns.Add("DiaChiXuatHoaDon", typeof(String));
                    dt.Columns.Add("SoTaiKhoan", typeof(String));
                    dt.Columns.Add("MaSoThue", typeof(String));
                    dt.Columns.Add("GhiChu", typeof(String));
                    dt.Columns.Add("ID_KhuVuc", typeof(String));
                    dt.Columns.Add("ID_Tinh", typeof(String));
                    dt.Columns.Add("ID_Quan", typeof(String));
                    dt.Columns.Add("ID_Phuong", typeof(String));
                    dt.Columns.Add("ID_LoaiKhachHang", typeof(String));
                    dt.Columns.Add("ID_KenhBanHang", typeof(String));
                    dt.Columns.Add("ID_KhachHang", typeof(String));

                    Aspose.Cells.Workbook workbook;
                    Aspose.Cells.Worksheet worksheet;
                    DataTable dtDataHeard = new DataTable();
                    string fileName = System.Web.HttpContext.Current.Server.MapPath(file.filename);
                    //fileName = System.IO.Path.Combine(savepath, Guid.NewGuid().ToString() + ".xls");
                    //file.SaveAs(fileName, true);
                    workbook = new Aspose.Cells.Workbook(fileName);
                    if (workbook.Worksheets.GetSheetByCodeName("DATA") == null)
                    {
                        // 302
                        return response = Request.CreateResponse(HttpStatusCode.Found, new { success = false, msg = "File mẫu không đúng định dạng." });
                        // Trả về file mẫu không đúng định dạng (file mẫu chiết xuất từ hệ thống)
                    }
                    worksheet = workbook.Worksheets[0];
                    dt = worksheet.Cells.ExportDataTableAsString(2, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, true);
                    if (System.IO.File.Exists(fileName))
                        System.IO.File.Delete(fileName);
                    dtDataHeard = dt.Clone();
                    foreach (var row in dt.Rows)
                    {
                        DataRow _datarow = row as DataRow;
                        var isRow = ImportValidate.TrimRow(ref _datarow, false);
                        if (!isRow)
                            continue;
                        dtDataHeard.ImportRow(_datarow);
                    }
                    if (dtDataHeard.Rows.Count == 0)
                    {
                        //204
                        return response = Request.CreateResponse(HttpStatusCode.NoContent, new { success = false, msg = "Không tồn tại bản ghi." });
                        //Trả về lỗi ko có dữ liệu import
                        //response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                    }
                    if (loadToGrid(dtDataHeard, userinfo.ID_QLLH, ref tempPath))
                    {
                        foreach (DataRow row in dtDataHeard.Rows)
                        {
                            KhachHangParam khachhang = new KhachHangParam();
                            khachhang.MaKH = row["MaKH"].ToString();
                            khachhang.Ten = row["TenKH"].ToString();
                            khachhang.DiaChi = row["DiaChi"].ToString();
                            khachhang.KinhDo = row["DiaChi"].ToString();
                            khachhang.ViDo = row["ViDo"].ToString();
                            khachhang.DuongPho = row["DuongPho"].ToString();
                            khachhang.KhuVuc = row["KhuVuc"].ToString();
                            if ((row["ID_KhuVuc"].ToString() != "") && (row["ID_KhuVuc"].ToString() != "#N/A"))
                            {
                                khachhang.ID_KhuVuc = int.Parse(row["ID_KhuVuc"].ToString());
                            }
                            khachhang.Tinh = row["Tinh"].ToString();
                            if ((row["ID_Tinh"].ToString() != "") && (row["ID_Tinh"].ToString() != "#N/A"))
                            {
                                khachhang.ID_Tinh = int.Parse(row["ID_Tinh"].ToString());
                            }
                            khachhang.Quan = row["Quan"].ToString();
                            if ((row["ID_Quan"].ToString() != "") && (row["ID_Quan"].ToString() != "#N/A"))
                            {
                                khachhang.ID_Quan = int.Parse(row["ID_Quan"].ToString());
                            }
                            khachhang.Phuong = row["Phuong"].ToString();
                            if ((row["ID_Phuong"].ToString() != "") && (row["ID_Phuong"].ToString() != "#N/A"))
                            {
                                khachhang.ID_Phuong = int.Parse(row["ID_Phuong"].ToString());
                            }
                            khachhang.SoDienThoai1 = row["DienThoai"].ToString();
                            khachhang.Fax = row["Fax"].ToString();
                            khachhang.SoDienThoai2 = row["DienThoai1"].ToString();
                            khachhang.SoDienThoai3 = row["DienThoai2"].ToString();
                            khachhang.LoaiKhachHang = row["LoaiKhachHang"].ToString();
                            if ((row["ID_LoaiKhachHang"].ToString() != "") && (row["ID_LoaiKhachHang"].ToString() != "#N/A"))
                            {
                                khachhang.ID_LoaiKhachHang = int.Parse(row["ID_LoaiKhachHang"].ToString());
                            }
                            khachhang.KenhBanHang = row["KenhBanHang"].ToString();
                            if ((row["ID_KenhBanHang"].ToString() != "") && (row["ID_KenhBanHang"].ToString() != "#N/A"))
                            {
                                khachhang.ID_KenhBanHang = int.Parse(row["ID_KenhBanHang"].ToString());
                            }
                            khachhang.KenhBanHangCapTren = row["KenhBanHangCapTren"].ToString();
                            if ((row["ID_KenhBanHangCapTren"].ToString() != "") && (row["ID_KenhBanHangCapTren"].ToString() != "#N/A"))
                            {
                                khachhang.ID_KenhBanHangCapTren = int.Parse(row["ID_KenhBanHangCapTren"].ToString());
                            }
                            khachhang.NguoiLienHe = row["NguoiLienHe"].ToString();

                            khachhang.HomThu = row["HomThu"].ToString();
                            khachhang.Website = row["Website"].ToString();
                            khachhang.DiaChiXuatHoaDon = row["DiaChiXuatHoaDon"].ToString();
                            khachhang.SoTaiKhoan = row["SoTaiKhoan"].ToString();
                            khachhang.MaSoThue = row["MaSoThue"].ToString();
                            khachhang.GhiChu = row["GhiChu"].ToString();
                            lstNhanvien.Add(khachhang);
                        }
                        return response = Request.CreateResponse(HttpStatusCode.OK, lstNhanvien);
                    }

                    else
                    {
                        var stream = new MemoryStream();
                        //205
                        response = new HttpResponseMessage(HttpStatusCode.ResetContent)
                        {
                            Content = new ByteArrayContent(stream.ToArray())
                        };
                        response.Content.Headers.ContentDisposition =
                            new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                            {
                                FileName = tempPath
                            };
                        response.Content.Headers.ContentType =
                            new MediaTypeHeaderValue("application/octet-stream");
                    }
                }

            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        private bool loadToGrid(DataTable dtDataHeard, int ID_QLLH, ref string tempPath)
        {
            //KhachHang_dl kh_dl = new KhachHang_dl();
            //try
            //{
            //    DataTable dtError = dtDataHeard.Clone();
            //    dtError.TableName = "DATA";
            //    int iRow = 4;
            //    bool IsError = false;
            //    foreach (DataRow row in dtDataHeard.Rows)
            //    {
            //        string sError = "";
            //        DataRow _datarow = row as DataRow;
            //        var rowError = dtError.NewRow();
            //        sError = "Mã khách hàng chưa nhập";
            //        ImportValidate.EmptyValue("MaKH", ref _datarow, ref rowError, ref IsError, sError);
            //        if ((row["MaKH"].ToString() != ""))
            //        {
            //            KhachHang khachhangobj = kh_dl.GetKhachHangTheoMa(ID_QLLH, row["MaKH"].ToString(), 0);
            //            if (khachhangobj != null)
            //            {
            //                rowError["MaKH"] = "Mã khách hàng đã tồn tại";
            //                IsError = true;
            //            }
            //        }
            //        sError = "Tên khách hàng chưa nhập";
            //        ImportValidate.EmptyValue("TenKH", ref _datarow, ref rowError, ref IsError, sError);
            //        sError = "Địa chỉ khách hàng chưa nhập";
            //        ImportValidate.EmptyValue("DiaChi", ref _datarow, ref rowError, ref IsError, sError);
            //        sError = "Số điện thoại hàng chưa nhập";
            //        ImportValidate.EmptyValue("DienThoai", ref _datarow, ref rowError, ref IsError, sError);
            //        if ((row["DienThoai"].ToString() != ""))
            //        {
            //            Boolean rs = new Boolean();
            //            rs = kh_dl.CheckTrungKhachHangTheoSDT(ID_QLLH, row["DienThoai"].ToString(), 0);
            //            if (!rs)
            //            {
            //                rowError["DienThoai"] = "Đã tồn tại số điện thoại khách hàng trên hệ thống";
            //                IsError = true;
            //            }
            //        }
            //        if ((row["MaSoThue"].ToString() != ""))
            //        {
            //            Boolean rs = new Boolean();
            //            rs = kh_dl.CheckTrungKhachHangTheoMST(ID_QLLH, row["MaSoThue"].ToString(), 0);
            //            if (!rs)
            //            {
            //                rowError["MaSoThue"] = "Đã tồn tại mã số thuế khách hàng trên hệ thống";
            //                IsError = true;
            //            }
            //        }


            //        if (row["KhuVuc"].ToString() != "")
            //        {
            //            sError = "Khu vực";
            //            ImportValidate.IsValidList("KhuVuc", "ID_KhuVuc", ref _datarow, ref rowError, ref IsError, sError, lang);
            //        }
            //        if (row["KhuVuc"].ToString() != "")
            //        {
            //            sError = "Khu vực";
            //            ImportValidate.IsValidList("KhuVuc", "ID_KhuVuc", ref _datarow, ref rowError, ref IsError, sError, lang);
            //        }
            //        if (row["HomThu"].ToString() != "")
            //        {
            //            sError = "Hòm thư không đúng định dạng";
            //            ImportValidate.IsValidEmail("HomThu", ref _datarow, ref rowError, ref IsError, sError);
            //        }
            //        if (row["KhuVuc"].ToString() != "")
            //        {
            //            sError = "Khu vực";
            //            ImportValidate.IsValidList("KhuVuc", "ID_KhuVuc", ref _datarow, ref rowError, ref IsError, sError);
            //        }
            //        if (row["Tinh"].ToString() != "")
            //        {
            //            sError = "Tỉnh";
            //            ImportValidate.IsValidList("Tinh", "ID_Tinh", ref _datarow, ref rowError, ref IsError, sError);
            //        }
            //        if (row["Quan"].ToString() != "")
            //        {
            //            sError = "Quận";
            //            ImportValidate.IsValidList("Quan", "ID_Quan", ref _datarow, ref rowError, ref IsError, sError);
            //        }
            //        if (row["Phuong"].ToString() != "")
            //        {
            //            sError = "Xã phường";
            //            ImportValidate.IsValidList("Phuong", "ID_Phuong", ref _datarow, ref rowError, ref IsError, sError);
            //        }
            //        if (row["LoaiKhachHang"].ToString() != "")
            //        {
            //            sError = "Loại khách hàng";
            //            ImportValidate.IsValidList("LoaiKhachHang", "ID_LoaiKhachHang", ref _datarow, ref rowError, ref IsError, sError);
            //        }
            //        if (row["KenhBanHang"].ToString() != "")
            //        {
            //            sError = "Kênh bán hàng";
            //            ImportValidate.IsValidList("KenhBanHang", "ID_KenhBanHang", ref _datarow, ref rowError, ref IsError, sError);
            //        }
            //        if (row["KenhBanHangCapTren"].ToString() != "")
            //        {
            //            sError = "Kênh bán hàng";
            //            ImportValidate.IsValidList("KenhBanHangCapTren", "ID_KhachHang", ref _datarow, ref rowError, ref IsError, sError);
            //        }
            //        if (IsError)
            //        {
            //            rowError["STT"] = iRow;
            //            dtError.Rows.Add(rowError);
            //        }

            //        iRow = (iRow + 1);
            //        IsError = false;

            //    }
            //    if ((dtError.Rows.Count > 0))
            //    {
            //        dtError.TableName = "DATA";
            //        ExportExcel ExportExcel = new ExportExcel();
            //        string filename = "";
            //        ExportExcel.ExportTemplateTable(@"ReportTemplates\Teamplatekhachang_error.xls", dtError, null, ref filename);
            //        tempPath = filename;
            //        return false;
            //        //return tempPath = "";
            //    }
            //    else
            //    {
            //        return true;
            //    }



            //}
            //catch (Exception ex)
            //{
            //    LSPos_Data.Utilities.Log.Error(ex);
            //}
            return true;
        }

        [HttpPost]
        [Route("SaveHinhThucThanhToan")]
        public HttpResponseMessage SaveHinhThucThanhToan([FromUri] int idNV, string HTTTs)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            

            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    return response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else 
                {                    
                    NhanVienApp nv_dl = new NhanVienApp();
                    NhanVienHinhThucTTModels obj = new NhanVienHinhThucTTModels();
                    nv_dl.XoaHinhThucTT_NhanVien(idNV);

                    if (HTTTs != null)
                    {
                        HTTTs.Split(',').ToList();
                        List<string> list = HTTTs.Split(',').ToList();
                        for (int i = 0; i < list.Count; i++)
                        {
                            
                            obj.ID_NhanVien = idNV;
                            obj.ID_HTTT = Int32.Parse(list[i]);
                            obj.NgayTao = DateTime.Now;
                            nv_dl.LuuHinhThucTT_NhanVien(obj);
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

        [HttpGet]
        [Route("GetNV_ListHTTT")]
        public HttpResponseMessage GetNvHTTT([FromUri] int IDNV)
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
                    NhanVienApp nv = new NhanVienApp();

                    List<HinhThucThanhToanModel> list = new List<HinhThucThanhToanModel>();
                    list = nv.getListHinhThucThanhToan(IDNV);
                    response = Request.CreateResponse(HttpStatusCode.OK, list);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }


        public class ParamLocTheoNhom
        {
            public string IDNhom { get; set; }
        }
        
        public Page Page { get; private set; }
        private class RequestGridParam
        {
            public DataSourceRequest request { get; set; }
            public TieuChiLoc tieuchiloc { get; set; }
        }
        public class KhachHangParam
        {
            public string DiaChi { get; set; }
            public string DiaChiXuatHoaDon { get; set; }
            public string DuongPho { get; set; }
            public string Email { get; set; }
            public string Fax { get; set; }
            public string GhiChu { get; set; }
            public int IDKhachHang { get; set; }
            public int IDQLLH { get; set; }
            public int ID_Cha { get; set; }
            public int ID_KhuVuc { get; set; }
            public string KhuVuc { get; set; }
            public int ID_LoaiKhachHang { get; set; }
            public string LoaiKhachHang { get; set; }
            public int ID_NhanVien { get; set; }
            public string NhanVien { get; set; }
            public int ID_NhomKH { get; set; }
            public int ID_Phuong { get; set; }
            public string Phuong { get; set; }
            public int ID_Quan { get; set; }
            public string Quan { get; set; }
            public int ID_QuanLy { get; set; }
            public int ID_Tinh { get; set; }
            public string Tinh { get; set; }
            public string KinhDo { get; set; }
            public string ViDo { get; set; }
            public string MaKH { get; set; }
            public string MaSoThue { get; set; }
            public int ID_KenhBanHang { get; set; }
            public string KenhBanHang { get; set; }
            public int ID_KenhBanHangCapTren { get; set; }
            public string KenhBanHangCapTren { get; set; }
            public string HomThu { get; set; }
            public string SoTaiKhoan { get; set; }
            public string NguoiLienHe { get; set; }
            public string SoDienThoai1 { get; set; }
            public string SoDienThoai2 { get; set; }
            public string SoDienThoai3 { get; set; }
            public string SoDienThoaiMacDinh { get; set; }
            public string SoTKNganHang { get; set; }
            public string Ten { get; set; }
            public string Website { get; set; }
            public string ImgUrl { get; set; }
        }
        public class nhanvienParam
        {
            public int ID_NV { get; set; }
            public string tenDayDu { get; set; }
            public string tenDangNhap { get; set; }
            public string matKhau { get; set; }
            public string queQuan { get; set; }
            public string diaChi { get; set; }
            public DateTime ngaySinh { get; set; }
            public string gioiTinhName { get; set; }
            public string chucVu { get; set; }
            public string dienThoai { get; set; }
            public string email { get; set; }
            public int truongNhom { get; set; }
            public string tenNhom { get; set; }
            public int iD_Nhom { get; set; }
            public int gioiTinh { get; set; }

        }

        
        public class NhanVienNew
        {
            public int IDNV { get; set; }
            public int IDQLLH { get; set; }
            public int ID_ChucVu { get; set; }
            public string chucVu { get; set; }
            public int ID_Nhom { get; set; }
            public int ID_NhomKhachHang_MacDinh { get; set; }
            public int Loai { get; set; }
            public int ID_QuanLy { get; set; }
            public string Imei { get; set; }
            public bool isCheDoTietKiemPin { get; set; }
            public bool IsFakeGPS { get; set; }
            public double KinhDo { get; set; }
            public int LastUpdate_ID_NhanVien { get; set; }
            public int LastUpdate_ID_QuanLy { get; set; }
            public string LastUpdate_Ten_NhanVien { get; set; }
            public string LastUpdate_Ten_QuanLy { get; set; }
            public DateTime LastUpdate_ThoiGian_NhanVien { get; set; }
            public DateTime LastUpdate_ThoiGian_QuanLy { get; set; }
            public string MatKhau { get; set; }
            public string NgaySinh { get; set; }
            public DateTime datenow { get; set; }
            public DateTime NgayXoa { get; set; }
            public string OS { get; set; }
            public string OSVersion { get; set; }
            public string PhienBan { get; set; }
            public string QueQuan { get; set; }
            public string TenDangNhap { get; set; }
            public string TenDayDu { get; set; }
            public string TenLoaiKetNoi { get; set; }
            public string TenMay { get; set; }
            public string TenNhom { get; set; }
            public DateTime ThoiGianCapNhat { get; set; }
            public DateTime ThoiGianDangXuat { get; set; }
            public DateTime ThoiGianGuiBanTinCuoiCung { get; set; }
            public DateTime ThoiGianHoatDong { get; set; }
            public string TinhTrangPin { get; set; }
            public int TrangThai { get; set; }
            public string TrangThaiTrucTuyen { get; set; }
            public int TrangThaiXoa { get; set; }
            public int TrucTuyen { get; set; }
            public int TruongNhom { get; set; }
            public int chkDoiMatKhau { get; set; }
            public double ViDo { get; set; }
            public string AnhDaiDien { get; set; }
            public string AnhDaiDien_thumbnail_medium { get; set; }
            public string AnhDaiDien_thumbnail_small { get; set; }
            public string AppFakeGPS { get; set; }
            public string DiaChi { get; set; }
            public string DienThoai { get; set; }
            public string DoiMay { get; set; }
            public string DongMay { get; set; }
            public string Email { get; set; }
            public int GioiTinh { get; set; }
            public string HinhThucDangXuat { get; set; }
            public string image_url { get; set; }
            public int IsHDV { get; set; }
            public string MaTheHDV { get; set; }
            public string CCCD { get; set; }

            public List<Int32> listIdKH = new List<Int32>();
        }

        public byte[] FileToByteArray(string fileName)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(fileName,
                                           FileMode.Open,
                                           FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
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
