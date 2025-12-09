using LSPosMVC.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;
using System.Net.Http.Headers;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using LSPos_Data.Data;
using LSPos_Data.Models;
using System.Data.SqlClient;
using LSPosMVC.Models;
using BusinessLayer.Repository;
using Ticket;
using BusinessLayer.Model.Sell;
using LSPosMVC.App_Start;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/mathang")]
    public class MatHangController : ApiController
    {
        private SellRepository _sellRepository;
        private ManagerProfileRepository _managerProfileRepository;
        private CommonRepository _commonRepository;

        public MatHangController()
        {
            _sellRepository = new SellRepository();
            _managerProfileRepository = new ManagerProfileRepository();
            _commonRepository = new CommonRepository();
        }

        [HttpGet]
        [Route("getdichvu")]
        public HttpResponseMessage GetServicesByDate(DateTime date)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            List<ServiceRateModel> listServicePackageRate = new List<ServiceRateModel>();
            //Trang An
            string ApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl_TrangAn"];
            string ProfileID = "03F42E9D-E8F8-484C-8C9C-D9A7A7148767";
            string ProfileCode = "12212";
            new GlobalUtilities().SetGlobal(ApiUrl, "leanhtest", "123", "TRANGAN", ProfileID, ProfileCode);
            _sellRepository.GetSellB2BData(Global.SiteID, Global.ProfileID, date);
            listServicePackageRate.AddRange(_sellRepository.GetListServicePackageRate());
            //Lscloud
            ApiUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["ApiUrl_LSCloud"];
            ProfileID = "03F42E9D-E8F8-484C-8C9C-D9A7A7148767";
            ProfileCode = "12212";
            new GlobalUtilities().SetGlobal(ApiUrl, "pchl", "123", "TRANGAN", ProfileID, ProfileCode);
            _sellRepository.GetSellB2BData(Global.SiteID, Global.ProfileID, date);
            listServicePackageRate.AddRange(_sellRepository.GetListServicePackageRate());
            response = Request.CreateResponse(HttpStatusCode.OK, listServicePackageRate);
            return response;
        }

        [HttpGet]
        [Route("ExportExcelMatHang")]
        public HttpResponseMessage ExportExcelMatHang([FromUri] int ID_DanhMuc)
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
                    MatHangData matHangData = new MatHangData();
                    DataSet ds = matHangData.GetDSMatHang(ID_DanhMuc, userinfo.ID_QLLH);

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
                        filename = "BM014_DanhSachMatHang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("ExcelMatHang.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM014_ProductList_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("ExcelMatHang_en.xls", dataSet, null, ref stream);
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
        [Route("getallmathang")]
        public DataSourceResult GetAllMatHang(HttpRequestMessage requestMessage)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            DataSourceResult s = new DataSourceResult();
            try
            {
                RequestGridParam param = JsonConvert.DeserializeObject<RequestGridParam>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    MatHang_dl tdt = new MatHang_dl();
                    List<MatHang> data = tdt.getDS_HangHoa_ByIdDanhMuc(param.ID_DanhMuc, userinfo.ID_QLLH);
                    if (param.request.Sort != null && param.request.Sort.Count() > 0)
                    {
                        string sortField = param.request.Sort.First().Field;
                        switch (sortField)
                        {
                            case "maHang":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    data = data.OrderBy(x => x.MaHang).ToList();
                                }
                                else
                                {
                                    data = data.OrderByDescending(x => x.MaHang).ToList();
                                }
                                break;
                            case "tenHang":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    data = data.OrderBy(x => x.TenHang).ToList();
                                }
                                else
                                {
                                    data = data.OrderByDescending(x => x.TenHang).ToList();
                                }
                                break;
                            case "tenDonVi":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    data = data.OrderBy(x => x.TenDonVi).ToList();
                                }
                                else
                                {
                                    data = data.OrderByDescending(x => x.TenDonVi).ToList();
                                }
                                break;
                            case "giaBuon":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    data = data.OrderBy(x => x.GiaBuon).ToList();
                                }
                                else
                                {
                                    data = data.OrderByDescending(x => x.GiaBuon).ToList();
                                }
                                break;
                            case "giaLe":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    data = data.OrderBy(x => x.GiaLe).ToList();
                                }
                                else
                                {
                                    data = data.OrderByDescending(x => x.GiaLe).ToList();
                                }
                                break;
                            case "tenDanhMuc":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    data = data.OrderBy(x => x.TenDanhMuc).ToList();
                                }
                                else
                                {
                                    data = data.OrderByDescending(x => x.TenDanhMuc).ToList();
                                }
                                break;
                            case "ghiChuGia":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    data = data.OrderBy(x => x.GhiChuGia).ToList();
                                }
                                else
                                {
                                    data = data.OrderByDescending(x => x.GhiChuGia).ToList();
                                }
                                break;
                        }
                    }
                    int tongso = data.Count;

                    FilterMatHangGrid filter = new FilterMatHangGrid();
                    if (param.request.Filter != null)
                    {
                        foreach (Filter f in param.request.Filter.Filters)
                        {
                            switch (f.Field)
                            {
                                case "maHang":
                                    filter.MaHang = f.Value.ToString(); ;
                                    data = data.Where(x => x.MaHang.ToLower().Contains(filter.MaHang.ToLower())).ToList();
                                    break;
                                case "tenHang":
                                    filter.TenHang = f.Value.ToString(); ;
                                    data = data.Where(x => x.TenHang != null).ToList();
                                    data = data.Where(x => x.TenHang.ToLower().Contains(filter.TenHang.ToLower())).ToList();
                                    break;
                                case "tenDonVi":
                                    filter.DonVi = f.Value.ToString();
                                    data = data.Where(x => x.TenDonVi.ToLower().Contains(filter.DonVi.ToLower())).ToList();
                                    break;
                                case "giaBuon":
                                    filter.GiaBuon = double.Parse(f.Value.ToString());
                                    data = data.Where(x => x.GiaBuon == filter.GiaBuon).ToList();
                                    break;
                                case "giaLe":
                                    filter.GiaLe = double.Parse(f.Value.ToString());
                                    data = data.Where(x => x.GiaLe == filter.GiaLe).ToList();
                                    break;
                                case "tenDanhMuc":
                                    filter.DanhMuc = f.Value.ToString(); ;
                                    data = data.Where(x => x.TenDanhMuc.ToLower().Contains(filter.DanhMuc.ToLower())).ToList();
                                    break;
                                case "ghiChuGia":
                                    filter.GhiChu = f.Value.ToString(); ;
                                    data = data.Where(x => x.GhiChuGia != null).ToList();
                                    data = data.Where(x => x.GhiChuGia.ToLower().Contains(filter.GhiChu.ToLower())).ToList();
                                    break;
                            }
                        }
                        tongso = data.Count;
                    }


                    //s = data.AsQueryable().ToDataSourceResult(param.request.Take, param.request.Skip, param.request.Sort, param.request.Filter);
                    s.Data = data.Skip(param.request.Skip).Take(param.request.Take);
                    foreach (MatHang mh in s.Data)
                    {
                        if (mh.GiaBuon == -1)
                        {
                            mh.GiaBuon = null;
                        }
                        if (mh.GiaLe == -1)
                        {
                            mh.GiaLe = null;
                        }
                    }
                    s.Total = tongso;
                    s.Aggregates = null;
                    //response = Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return s;
        }

        [HttpGet]
        [Route("getalldichvu")]
        public HttpResponseMessage getalldichvu()
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
                    DichVuDAO tdt = new DichVuDAO();
                    List<DichVuModel> data = tdt.GetAllDichVu();
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
        [Route("getallmathangNew")]
        public HttpResponseMessage getallmathangNew([FromUri] int ID_DanhMuc)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            DataSourceResult s = new DataSourceResult();
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    MatHang_dl tdt = new MatHang_dl();
                    List<MatHang> data = tdt.getDS_HangHoa_ByIdDanhMuc(ID_DanhMuc, userinfo.ID_QLLH);
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
        [Route("getbyid")]
        public HttpResponseMessage GetMatHangbyId([FromUri] int ID_MatHang)
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
                    MatHang_dl mhdl = new MatHang_dl();
                    MatHang mh = mhdl.GetMatHangTheoID(ID_MatHang);
                    response = Request.CreateResponse(HttpStatusCode.OK, mh);
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
        [Route("getallcombo")]
        public HttpResponseMessage getallcombo()
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
                    MatHangData mhdl = new MatHangData();
                    List<ComboboxDTO> mh = mhdl.ComboxboxMatHang(userinfo.ID_QLLH);
                    response = Request.CreateResponse(HttpStatusCode.OK, mh);
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
        [Route("getbykhachhang")]
        public HttpResponseMessage getbykhachhang(int idKhachHang)
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
                    GuiBanMatHangData mhdl = new GuiBanMatHangData();
                    List<MatHang> mh = mhdl.getMatHangbykhachhang(userinfo.ID_QLLH, idKhachHang);
                    response = Request.CreateResponse(HttpStatusCode.OK, mh);
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
        [Route("deletemathang")]
        public HttpResponseMessage getKHall([FromBody] List<int> Ids)
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
                    foreach (int id in Ids)
                    {
                        mh_dl.DeleteMatHang(userinfo.ID_QLLH, id);
                    }
                    //DataTable dskh = kh_dl.GetDataKhachHangAll(userinfo.ID_QLLH, userinfo.ID_QuanLy, 0, 0, -1, 0);
                    //List<KhachHang> lstKhachHang = new List<KhachHang>();

                    //foreach (DataRow dr in dskh.Rows)
                    //{
                    //    KhachHang kh = kh_dl.GetKhachHangFromDataRow(dr);

                    //    if(kh != null)
                    //        lstKhachHang.Add(kh);
                    //}

                    response = Request.CreateResponse(HttpStatusCode.OK, true);
                    //response = Request.CreateResponse(HttpStatusCode.OK, dskh);
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
        [Route("themsuamathang")]
        public HttpResponseMessage ThemSuaMatHang([FromBody] MatHang model)
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
                    bool success = false;
                    int idmh = 0;
                    MatHangData mhdt = new MatHangData();
                    HangHoa_DichVuDAO hhdvdao = new HangHoa_DichVuDAO();
                    MatHang_dl mh_dl = new MatHang_dl();
                    if (model.IDMatHang == 0)
                    {
                        model.IDQLLH = userinfo.ID_QLLH;
                        model.SoLuong = 0;
                        if (mhdt.CheckTrungMaMatHang(userinfo.ID_QLLH, model.MaHang))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luuthongtinmathangthatbaimahangdatontai" });
                            return response;
                        }
                        idmh = mh_dl.ThemMatHang(model);
                    }
                    else
                    {
                        MatHang mh = mh_dl.GetMatHangTheoID(model.IDMatHang);
                        if (mhdt.CheckTrungMaMatHang(userinfo.ID_QLLH, model.MaHang) && model.MaHang != mh.MaHang)
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luuthongtinmathangthatbaimahangdatontai" });
                            return response;
                        }
                        mh.MaHang = model.MaHang;
                        mh.TenHang = model.TenHang;
                        mh.IDDonVi = model.IDDonVi;
                        mh.ID_DANHMUC = model.ID_DANHMUC;
                        mh.ID_NhaCungCap = model.ID_NhaCungCap;
                        mh.ID_NhanHieu = model.ID_NhanHieu;
                        mh.LinkGioiThieu = model.LinkGioiThieu;
                        mh.MoTa = model.MoTa;
                        mh.MoTaNgan = model.MoTaNgan;
                        mh.GiaBuon = model.GiaBuon;
                        mh.GiaLe = model.GiaLe;
                        mh.GhiChuGia = model.GhiChuGia;
                        mh.AnhDaiDien = model.AnhDaiDien;
                        mh.IsDichVu = model.IsDichVu;
                        idmh = mh.IDMatHang;

                        success = mh_dl.CapNhatMatHang(mh);

                        List<int> cur_lstID = mh.lstDichVu.Select(c => c.ID).ToList();
                        List<int> update_lstID = model.lstDichVu.Select(c => c.ID).ToList();
                        List<int> delete_lstID = cur_lstID.Except<int>(update_lstID).ToList(); // Danh sách nhân viên bị xóa
                        foreach (int id in delete_lstID)
                        {
                            hhdvdao.Delete(id);
                        }
                    }

                    if (idmh > 0)
                    {
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(model.AnhDaiDien))
                            {
                                //string teampatch = AppDomain.CurrentDomain.BaseDirectory + model.AnhDaiDien;
                                //if (File.Exists(teampatch))
                                //{
                                //    byte[] binData = GetBytesFromFile(teampatch);
                                //    if (binData != null && binData.Length > 0)
                                //    {
                                //        string svURL = Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]);
                                //        string strLinkServer = svURL + "/AppUploadAnh.aspx?token=abc&idqllh="
                                //            + userinfo.ID_QLLH + "&idnhanvien=" + userinfo.ID_QuanLy + "&idmathang=" + idmh
                                //            + "&ghichu=&thoigianchup=" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "&type=anhdaidienmathang";
                                //        PostMultipleFiles_Stream(strLinkServer, binData, "test.jpg");
                                //    }
                                //}
                            }
                            if (model.lstDichVu != null)
                            {

                                foreach (HangHoa_DichVuModel dichvu in model.lstDichVu)
                                {
                                    dichvu.ID_HangHoa = idmh;
                                    hhdvdao.InsertOrUpdate(dichvu);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LSPos_Data.Utilities.Log.Error(ex);
                        }
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, idMatHang = idmh, msg = "label_luuthongtinmathangthanhcong" });
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = true, msg = "label_luuthongtinmathangthatbai" });
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;

        }

        [HttpGet]
        [Route("getnhommathang")]
        public HttpResponseMessage GetNhomMatHang(HttpRequestMessage requestMessage)
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
                    BaoCaoCommon baocao = new BaoCaoCommon();
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    List<DanhMucOBJ> lstDanhMuc = new List<DanhMucOBJ>();
                    lstDanhMuc = MatHangData.getDS_DanhMuc(userinfo.ID_QLLH, userinfo.ID_QuanLy, lang).Where(x => x.ID_PARENT == 0).ToList();
                    List<NhomMatHang> result = new List<NhomMatHang>();
                    foreach (DanhMucOBJ i in lstDanhMuc)
                    {
                        NhomMatHang a = new NhomMatHang();
                        a.ID_Parent = i.ID_PARENT;
                        a.ID = i.ID_DANHMUC;
                        a.Name = i.TenHienThi;
                        a.TenMatHang = i.TenDanhMuc;
                        a.AnhDaiDien = i.AnhDaiDien;
                        a.Childs = GetTree_NhomMatHang(a.ID);
                        result.Add(a);
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, result);
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
        [Route("getdsnhommathang")]
        public HttpResponseMessage GetDsNhomMatHang(HttpRequestMessage requestMessage)
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
                    BaoCaoCommon baocao = new BaoCaoCommon();
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    List<DanhMucOBJ> lstDanhMuc = new List<DanhMucOBJ>();
                    DanhMucOBJ khac = new DanhMucOBJ();
                    khac.TenDanhMuc = (lang == "vi") ? "Khác" : "Other";
                    khac.ID_DANHMUC = -2;
                    DanhMucOBJ tc = new DanhMucOBJ();
                    tc.TenDanhMuc = (lang == "vi") ? "Tất cả" : "All";
                    tc.ID_DANHMUC = -1;
                    lstDanhMuc = new List<DanhMucOBJ>();
                    lstDanhMuc.Add(tc);
                    lstDanhMuc.Add(khac);
                    lstDanhMuc.AddRange(DanhMucDB.getDS_DanhMuc(userinfo.ID_QLLH).Where(x => x.ID_DANHMUC > 0));
                    response = Request.CreateResponse(HttpStatusCode.OK, lstDanhMuc);
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
        [Route("getnhommathangtheophanquyen")]
        public HttpResponseMessage GetNhomMatHangTheoPhanQuyen(HttpRequestMessage requestMessage)
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
                    List<DanhMucOBJ> lstDanhMuc = new List<DanhMucOBJ>();
                    if (userinfo.IsAdmin)
                    {
                        lstDanhMuc = DanhMucDB.getDS_DanhMuc_TheoPhanQuyen_Admin(0, userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    }
                    else
                    {
                        lstDanhMuc = DanhMucDB.getDS_DanhMuc_TheoPhanQuyen(0, userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    }
                    List<NhomMatHang> result = new List<NhomMatHang>();
                    foreach (DanhMucOBJ i in lstDanhMuc.Where(x => x.ID_PARENT == 0).ToList())
                    {
                        NhomMatHang a = new NhomMatHang();
                        a.ID_Parent = i.ID_PARENT;
                        a.ID = i.ID_DANHMUC;
                        a.Name = i.TenHienThi;
                        a.Childs = GetTree_NhomMatHang(a.ID).Where(x => lstDanhMuc.Find(y => y.ID_DANHMUC == x.ID) != null).ToList();
                        result.Add(a);
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, result);
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
        [Route("themsuanhom")]
        public HttpResponseMessage ThemSuaNhomMatHang([FromBody] NhomMatHangCreateModel model)
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
                    bool success = false;
                    if (model.ID_Nhom > 0)
                    {
                        DanhMucOBJ item = DanhMucDB.get_DanhMucById(model.ID_Nhom);
                        item.TenDanhMuc = model.TenNhom;
                        item.AnhDaiDien = model.AnhDaiDien;
                        if (model.ID_Parent == -1)
                        {
                            item.ID_PARENT = 0;
                        }
                        else
                        {
                            item.ID_PARENT = model.ID_Parent;
                        }
                        success = DanhMucDB.SuaDanhMuc(item);
                    }
                    else
                    {
                        DanhMucOBJ item = new DanhMucOBJ();
                        item.TenDanhMuc = model.TenNhom;
                        item.ID_QLLH = userinfo.ID_QLLH;
                        item.AnhDaiDien = model.AnhDaiDien;
                        if (model.ID_Parent == -1)
                        {
                            item.ID_PARENT = 0;

                        }
                        else
                        {
                            item.ID_PARENT = model.ID_Parent;
                        }
                        success = DanhMucDB.ThemDanhMuc(item);
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = success });
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
        [Route("getnhombyid")]
        public HttpResponseMessage GetNhomByID([FromUri] int ID_Nhom)
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
                    if (ID_Nhom > 0)
                    {
                        DanhMucOBJ item = DanhMucDB.getDS_DanhMuc(userinfo.ID_QLLH).Where(x => x.ID_DANHMUC == ID_Nhom).First();
                        response = Request.CreateResponse(HttpStatusCode.OK, item);
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

        [HttpPost]
        [Route("xoanhom")]
        public HttpResponseMessage XoaNhom([FromUri] int ID_Nhom)
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
                    bool success = false;
                    if (ID_Nhom > 0)
                    {
                        DanhMucOBJ item = DanhMucDB.getDS_DanhMuc(userinfo.ID_QLLH).Where(x => x.ID_DANHMUC == ID_Nhom).First();
                        success = DanhMucDB.XoaDanhMuc(item);
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = success });
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
        [Route("getdsdonvi")]
        public HttpResponseMessage GetDSDonVi()
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
                    DonVi_dl dv_dl = new DonVi_dl();
                    List<DonVi> dsdv = dv_dl.GetDonViAll(userinfo.ID_QLLH);
                    response = Request.CreateResponse(HttpStatusCode.OK, dsdv);
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
        [Route("getdsnhacungcap")]
        public HttpResponseMessage GetDSNhaCungCap()
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
                    NhaCungCapDB ncc_dl = new NhaCungCapDB();
                    List<NhaCungCapOBJ> dsNCC = ncc_dl.GetListDanhSach(userinfo.ID_QLLH);
                    response = Request.CreateResponse(HttpStatusCode.OK, dsNCC);
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
        [Route("getdsnhanhieu")]
        public HttpResponseMessage GetDSNhanHieu()
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
                    NhanHieuDB nh_dl = new NhanHieuDB();
                    List<NhanHieuOBJ> dsNH = nh_dl.GetListDanhSach(userinfo.ID_QLLH);
                    response = Request.CreateResponse(HttpStatusCode.OK, dsNH);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;

        }

        public List<NhomMatHang> GetTree_NhomMatHang(int ID_Parent)
        {
            List<NhomMatHang> result = new List<NhomMatHang>();
            List<DanhMucOBJ> childs = DanhMucDB.getDS_DanhMucCon_ByIdDanhMuc(ID_Parent);
            foreach (DanhMucOBJ dm in childs)
            {
                NhomMatHang item = new NhomMatHang();
                List<DanhMucOBJ> child = DanhMucDB.getDS_DanhMucCon_ByIdDanhMuc(dm.ID_DANHMUC);
                item.ID = dm.ID_DANHMUC;
                item.Name = dm.TenHienThi;
                item.TenMatHang = dm.TenDanhMuc;
                item.ID_Parent = dm.ID_PARENT;
                item.AnhDaiDien = dm.AnhDaiDien;
                if (child.Count > 0)
                {
                    item.HasChilds = true;
                    item.Childs = GetTree_NhomMatHang(item.ID);
                }
                else
                {
                    item.HasChilds = false;
                }
                result.Add(item);
            }
            return result;

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

        [AllowAnonymous]
        [HttpGet]
        [Route("Getteamplate_MatHang")]
        public HttpResponseMessage exportexcelMatHang([FromUri] string username, string maCongTy)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                User_dl userDL = new User_dl();

                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DonHangData donhang = new DonHangData();
                    DataSet ds = donhang._GettemplatematHang(userinfo.ID_QLLH);
                    ds.Tables[0].TableName = "DATA";
                    ds.Tables[1].TableName = "DATA1";
                    ds.Tables[2].TableName = "DATA2";
                    ds.Tables[3].TableName = "DATA3";
                    ds.Tables[4].TableName = "DATA4";
                    for (int i1 = 1; i1 <= 2000; i1++)
                    {
                        var row = ds.Tables[0].NewRow();
                        ds.Tables[0].Rows.Add(row);
                    }
                    string tempPath = "";
                    ExportExcel excel = new ExportExcel();
                    excel.ExportTemplate(@"ReportTemplates\Danh_sach_mat_hang.xls", ds, null, ref tempPath);

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

        [AllowAnonymous]
        [HttpGet]
        [Route("Getteamplate_GiaMatHang")]
        public HttpResponseMessage Getteamplate_GiaMatHang([FromUri] string username, string maCongTy, int idNhom)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                User_dl userDL = new User_dl();

                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DonHangData donhang = new DonHangData();
                    MatHang_dl tdt = new MatHang_dl();
                    DataSet ds = donhang._GettemplateGiamatHang(idNhom, userinfo.ID_QLLH);
                    ds.Tables[0].TableName = "DATA";
                    string tempPath = "";
                    ExportExcel excel = new ExportExcel();
                    excel.ExportTemplate(@"ReportTemplates\File_Mau_Cap_Nhat_Gia.xls", ds, null, ref tempPath);

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

        #region import excel mặt hàng
        [HttpGet]
        [Route("GetTamPlateMatHang_New")]
        public HttpResponseMessage GetTamPlateMatHang_New()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                User_dl userDL = new User_dl();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DonHangData donhang = new DonHangData();
                    DataSet ds = donhang._GettemplatematHang(userinfo.ID_QLLH);
                    ds.Tables[0].TableName = "DATA";
                    ds.Tables[1].TableName = "DATA1";
                    ds.Tables[2].TableName = "DATA2";
                    ds.Tables[3].TableName = "DATA3";
                    ds.Tables[4].TableName = "DATA4";
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
                        filename = "BM015_FileMauMatHang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("Danh_sach_mat_hang.xls", ds, null, ref stream);
                    }
                    else
                    {
                        filename = "BM015_ProductTemplate_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("Danh_sach_mat_hang_en.xls", ds, null, ref stream);
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
        [Route("importmathang")]
        public HttpResponseMessage importmathang([FromBody] FileUploadModelFilter file)
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
                    dt.Columns.Add("maHang", typeof(String));
                    dt.Columns.Add("tenHang", typeof(String));
                    dt.Columns.Add("tenDonVi", typeof(String));
                    dt.Columns.Add("tenNhom", typeof(String));
                    dt.Columns.Add("ten_NhaCungCap", typeof(String));
                    dt.Columns.Add("ten_NhanHieu", typeof(String));
                    dt.Columns.Add("giaBuon", typeof(String));
                    dt.Columns.Add("giaLe", typeof(String));
                    dt.Columns.Add("ghiChuGia", typeof(String));
                    dt.Columns.Add("moTa", typeof(String));
                    dt.Columns.Add("linkGioiThieu", typeof(String));
                    dt.Columns.Add("iD_DANHMUC", typeof(String));
                    dt.Columns.Add("idDonVi", typeof(String));
                    dt.Columns.Add("iD_NhaCungCap", typeof(String));
                    dt.Columns.Add("iD_NhanHieu", typeof(String));

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

                    if (validatedatamathang(dtDataHeard, userinfo, ref tempPath))
                    {
                        try
                        {
                            MatHang_dl mh_dl = new MatHang_dl();
                            foreach (DataRow row in dtDataHeard.Rows)
                            {
                                MatHang obj = new MatHang();
                                obj.IDMatHang = 0;
                                obj.IDQLLH = userinfo.ID_QLLH;
                                obj.SoLuong = 0;
                                obj.KhuyenMai = "";

                                obj.MaHang = row["maHang"].ToString();
                                obj.TenHang = row["tenHang"].ToString();
                                obj.GhiChuGia = row["ghiChuGia"].ToString();
                                obj.MoTa = row["moTa"].ToString();
                                obj.LinkGioiThieu = row["linkGioiThieu"].ToString();

                                if ((row["giaBuon"].ToString() != "") && (row["giaBuon"].ToString() != "#N/A"))
                                    obj.GiaBuon = float.Parse(row["giaBuon"].ToString().Trim());
                                if ((row["giaLe"].ToString() != "") && (row["giaLe"].ToString() != "#N/A"))
                                    obj.GiaLe = float.Parse(row["giaLe"].ToString().Trim());

                                if ((row["idDonVi"].ToString() != "") && (row["idDonVi"].ToString() != "#N/A"))
                                    obj.IDDonVi = int.Parse(row["idDonVi"].ToString().Trim());
                                if ((row["iD_DANHMUC"].ToString() != "") && (row["iD_DANHMUC"].ToString() != "#N/A"))
                                    obj.ID_DANHMUC = int.Parse(row["iD_DANHMUC"].ToString().Trim());
                                if ((row["iD_NhaCungCap"].ToString() != "") && (row["iD_NhaCungCap"].ToString() != "#N/A"))
                                    obj.ID_NhaCungCap = int.Parse(row["iD_NhaCungCap"].ToString().Trim());
                                if ((row["iD_NhanHieu"].ToString() != "") && (row["iD_NhanHieu"].ToString() != "#N/A"))
                                    obj.ID_NhanHieu = int.Parse(row["iD_NhanHieu"].ToString().Trim());

                                int id = mh_dl.ThemMatHang(obj);
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
                            filename = "BM016_FileMauMatHangError_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        }
                        else
                        {
                            filename = "BM016_ProductTemplateError_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
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

        private bool validatedatamathang(DataTable dtDataHeard, UserInfo userinfo, ref MemoryStream fileStream)
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
                    sError = (lang == "vi") ? "Mã mặt hàng chưa nhập" : "Missing product code";
                    ImportValidate.EmptyValue("maHang", ref _datarow, ref rowError, ref IsError, sError);
                    sError = (lang == "vi") ? "Tên mặt hàng chưa nhập" : "Missing product name";
                    ImportValidate.EmptyValue("tenHang", ref _datarow, ref rowError, ref IsError, sError);
                    sError = (lang == "vi") ? "Đơn vị chưa nhập" : "Missing product unit";
                    ImportValidate.EmptyValue("tenDonVi", ref _datarow, ref rowError, ref IsError, sError);
                    sError = (lang == "vi") ? "Nhóm mặt hàng chưa nhập" : "Missing product group";
                    ImportValidate.EmptyValue("tenNhom", ref _datarow, ref rowError, ref IsError, sError);
                    if (row["maHang"].ToString() != "")
                    {
                        DataTable dtdata = CheckMaMatHang(row["maHang"].ToString(), userinfo.ID_QLLH);
                        if (int.Parse(dtdata.Rows[0]["SOLUONG"].ToString()) > 0)
                        {
                            rowError["maHang"] = (lang == "vi") ? "Mã mặt hàng đã tồn tại" : "Product code is already exists";
                            IsError = true;
                        }
                    }
                    if (row["maHang"].ToString() != "" && rowError["maHang"].ToString() == "")
                    {
                        string MHCode = row["maHang"].ToString();
                        if (!lstEmp.Contains(MHCode))
                            lstEmp.Add(MHCode);
                        else
                        {
                            IsError = true;
                            rowError["maHang"] = (lang == "vi") ? "Mã mặt hàng đã tồn tại trong file import" : "Product code is already exists in the imported file";
                        }
                    }
                    if (row["tenDonVi"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Đơn vị" : "Unit";
                        ImportValidate.IsValidList("tenDonVi", "idDonVi", ref _datarow, ref rowError, ref IsError, sError, lang);
                    }
                    if (row["tenNhom"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Nhóm mặt hàng" : "Product group";
                        ImportValidate.IsValidList("tenNhom", "iD_DANHMUC", ref _datarow, ref rowError, ref IsError, sError, lang);
                    }
                    if (row["ten_NhaCungCap"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Nhà cung cấp" : "Provider";
                        ImportValidate.IsValidList("ten_NhaCungCap", "iD_NhaCungCap", ref _datarow, ref rowError, ref IsError, sError, lang);
                    }

                    if (row["ten_NhanHieu"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Nhãn hiệu" : "Brand";
                        ImportValidate.IsValidList("ten_NhanHieu", "iD_NhanHieu", ref _datarow, ref rowError, ref IsError, sError, lang);
                    }
                    if (row["giaBuon"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Giá bán buôn không đúng định dạng" : "Invalid wholesale price format";
                        ImportValidate.IsValidNumber("giaBuon", ref _datarow, ref rowError, ref IsError, sError, true, false, lang);
                    }
                    if (row["giaLe"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Giá bán lẻ không đúng định dạng" : "Invalid retail price format";
                        ImportValidate.IsValidNumber("giaLe", ref _datarow, ref rowError, ref IsError, sError, true, true, lang);
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
                        ExportExcel.ExportTemplateToStream("Danh_sach_mat_hang_error.xls", dtError, null, ref fileStream);
                    }
                    else
                    {
                        ExportExcel.ExportTemplateToStream("Danh_sach_mat_hang_error_en.xls", dtError, null, ref fileStream);
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

        #region import excel bảng giá mặt hàng
        [HttpGet]
        [Route("Getteamplate_GiaMatHang_New")]
        public HttpResponseMessage Getteamplate_GiaMatHang_New(int idNhom)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                User_dl userDL = new User_dl();
                UserInfo userinfo = utilsCommon.checkAuthorization();
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DonHangData donhang = new DonHangData();
                    MatHang_dl tdt = new MatHang_dl();
                    DataSet ds = donhang._GettemplateGiamatHang(idNhom, userinfo.ID_QLLH);
                    ds.Tables[0].TableName = "DATA";

                    BaoCaoCommon baocao = new BaoCaoCommon();
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    string filename = "";
                    var stream = new MemoryStream();
                    ExportExcel excel = new ExportExcel();

                    if (lang == "vi")
                    {
                        filename = "BM017_FileMauCapNhatGia_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("File_Mau_Cap_Nhat_Gia.xls", ds, null, ref stream);
                    }
                    else
                    {
                        filename = "BM017_PriceTemplate_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("File_Mau_Cap_Nhat_Gia_en.xls", ds, null, ref stream);
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
        [Route("importbanggiamathang")]
        public HttpResponseMessage importbanggiamathang([FromBody] FileUploadModelFilter file)
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
                    dt.Columns.Add("MaHang", typeof(String));
                    dt.Columns.Add("TenHang", typeof(String));
                    dt.Columns.Add("GiaBuon", typeof(String));
                    dt.Columns.Add("GiaLe", typeof(String));
                    dt.Columns.Add("NgayBatDau", typeof(string));
                    dt.Columns.Add("ID_MatHang", typeof(String));

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

                    if (validatedatabanggiamathang(dtDataHeard, userinfo, ref tempPath))
                    {
                        try
                        {
                            DonHangData donHangData = new DonHangData();
                            foreach (DataRow row in dtDataHeard.Rows)
                            {
                                ApGia obj = new ApGia();
                                obj.ID_QuanLy = userinfo.ID_QuanLy;

                                float giabuon = 0;
                                float giale = 0;
                                int idhang = 0;
                                DateTime ngay = new DateTime(1900, 1, 1);

                                if ((row["ID_MatHang"].ToString() != "") && (row["ID_MatHang"].ToString() != "#N/A"))
                                    idhang = int.Parse(row["ID_MatHang"].ToString().Trim());

                                if ((row["GiaBuon"].ToString() != "") && (row["GiaBuon"].ToString() != "#N/A"))
                                    giabuon = float.Parse(row["GiaBuon"].ToString().Trim());
                                if ((row["GiaLe"].ToString() != "") && (row["GiaLe"].ToString() != "#N/A"))
                                    giale = float.Parse(row["GiaLe"].ToString().Trim());

                                if ((row["NgayBatDau"].ToString() != "") && (row["NgayBatDau"].ToString() != "#N/A"))
                                    ngay = DateTime.ParseExact(row["NgayBatDau"].ToString().Trim(), "dd/MM/yyyy", null);

                                obj.ID_Hang = idhang;
                                obj.GiaBanBuon = giabuon;
                                obj.GiaBanLe = giale;
                                obj.TuNgay = ngay;
                                donHangData.ApGiaMoi(obj);
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
                            filename = "BM018_FileMauCapNhatGiaError_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        }
                        else
                        {
                            filename = "BM018_PriceTemplateError_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
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

        private bool validatedatabanggiamathang(DataTable dtDataHeard, UserInfo userinfo, ref MemoryStream fileStream)
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
                    sError = (lang == "vi") ? "ID mặt hàng không tồn tại trong hệ thống" : "Product ID does not exist";
                    ImportValidate.EmptyValue("ID_MatHang", ref _datarow, ref rowError, ref IsError, sError);

                    if (row["GiaBuon"].ToString() != "")
                    {
                        bool er_banbuon = false;
                        sError = (lang == "vi") ? "Giá bán buôn không đúng định dạng" : "Invalid wholesale price format";
                        ImportValidate.IsValidNumber("GiaBuon", ref _datarow, ref rowError, ref er_banbuon, sError, false, false, lang);

                        if (!er_banbuon)
                        {
                            float giabuon = float.Parse(row["GiaBuon"].ToString().Trim());
                            if (giabuon < 0)
                            {
                                rowError["GiaBuon"] = (lang == "vi") ? "Giá bán buôn không được nhỏ hơn 0" : "Wholesale price cannot be smaller than 0";
                                IsError = true;
                            }

                        }
                        else
                        {
                            IsError = true;
                        }

                    }

                    if (row["GiaLe"].ToString() != "")
                    {
                        bool er_banle = false;
                        sError = (lang == "vi") ? "Giá bán lẻ không đúng định dạng" : "Invalid retail price format";
                        ImportValidate.IsValidNumber("GiaLe", ref _datarow, ref rowError, ref er_banle, sError, true, true, lang);

                        if (!er_banle)
                        {
                            float giale = float.Parse(row["GiaLe"].ToString().Trim());
                            if (giale < 0)
                            {
                                rowError["GiaLe"] = (lang == "vi") ? "Giá bán lẻ không được nhỏ hơn 0" : "Retail price cannot be smaller than 0";
                                IsError = true;
                            }
                        }
                        else
                        {
                            IsError = true;
                        }
                    }

                    if (row["NgayBatDau"].ToString() != "")
                    {
                        bool er_ngay = false;
                        sError = (lang == "vi") ? "Ngày bắt đầu không đúng định dạng" : "Start date is invalid";
                        ImportValidate.IsValidDate("NgayBatDau", ref _datarow, ref rowError, ref er_ngay, sError, lang);

                        if (!er_ngay)
                        {
                            DateTime d = DateTime.Now;
                            DateTime ngay = DateTime.ParseExact(row["NgayBatDau"].ToString().Trim(), "dd/MM/yyyy", null);
                            if (ngay.AddDays(1) < d.AddDays(1))
                            {
                                rowError["NgayBatDau"] = (lang == "vi") ? "Ngày áp dụng không thể nhỏ hơn ngày hiện tại" : "Application date cannot be sooner than current date";
                                IsError = true;
                            }
                        }
                    }
                    else
                    {
                        if ((row["GiaLe"].ToString() != "") || (row["GiaBuon"].ToString() != ""))
                        {
                            sError = (lang == "vi") ? "Ngày áp dụng chưa nhập" : "Missing application date";
                            ImportValidate.EmptyValue("NgayBatDau", ref _datarow, ref rowError, ref IsError, sError);
                        }
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
                        ExportExcel.ExportTemplateToStream("File_Mau_Cap_Nhat_Gia_error.xls", dtError, null, ref fileStream);
                    }
                    else
                    {
                        ExportExcel.ExportTemplateToStream("File_Mau_Cap_Nhat_Gia_error_en.xls", dtError, null, ref fileStream);
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
        [Route("checkfileimport")]
        public HttpResponseMessage checkfile([FromBody] FileExcelUpload file)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            // List<nhanvienParam> lstNhanvien = new List<nhanvienParam>();
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
                    dt.Columns.Add("maHang", typeof(String));
                    dt.Columns.Add("tenHang", typeof(String));
                    dt.Columns.Add("tenDonVi", typeof(String));
                    dt.Columns.Add("tenNhom", typeof(String));
                    dt.Columns.Add("ten_NhaCungCap", typeof(String));
                    dt.Columns.Add("ten_NhanHieu", typeof(String));
                    dt.Columns.Add("giaBuon", typeof(String));
                    dt.Columns.Add("giaLe", typeof(String));
                    dt.Columns.Add("ghiChuGia", typeof(String));
                    dt.Columns.Add("moTa", typeof(String));
                    dt.Columns.Add("linkGioiThieu", typeof(String));
                    dt.Columns.Add("iD_DANHMUC", typeof(String));
                    dt.Columns.Add("idDonVi", typeof(String));
                    dt.Columns.Add("iD_NhaCungCap", typeof(String));
                    dt.Columns.Add("iD_NhanHieu", typeof(String));

                    Aspose.Cells.Workbook workbook;
                    Aspose.Cells.Worksheet worksheet;
                    DataTable dtDataHeard = new DataTable();
                    string fileName = System.Web.HttpContext.Current.Server.MapPath(file.filename);
                    //fileName = System.IO.Path.Combine(savepath, Guid.NewGuid().ToString() + ".xls");
                    //file.SaveAs(fileName, true);
                    workbook = new Aspose.Cells.Workbook(fileName);
                    if (workbook.Worksheets.GetSheetByCodeName("DATA") == null)
                    {
                        return response = Request.CreateResponse(HttpStatusCode.NoContent, new { success = false, msg = "File mẫu không đúng định dạng." });
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
                    if (loadToGridMatHang(dtDataHeard, userinfo.ID_QLLH, ref tempPath))
                    {
                        return response = Request.CreateResponse(HttpStatusCode.Created);
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
                                FileName = "DataMathangError" + DateTime.Now.ToString("yyyyMMdd-HHMMss") + ".xls"
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

        [HttpPost]
        [Route("checkfileimportGia")]
        public HttpResponseMessage checkfileimportGia([FromBody] FileExcelUpload file)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            // List<nhanvienParam> lstNhanvien = new List<nhanvienParam>();
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
                    dt.Columns.Add("MaHang", typeof(String));
                    dt.Columns.Add("TenHang", typeof(String));
                    dt.Columns.Add("GiaBuon", typeof(String));
                    dt.Columns.Add("GiaLe", typeof(String));
                    dt.Columns.Add("NgayBatDau", typeof(string));
                    dt.Columns.Add("ID_MatHang", typeof(String));

                    Aspose.Cells.Workbook workbook;
                    Aspose.Cells.Worksheet worksheet;
                    DataTable dtDataHeard = new DataTable();
                    string fileName = System.Web.HttpContext.Current.Server.MapPath(file.filename);
                    //fileName = System.IO.Path.Combine(savepath, Guid.NewGuid().ToString() + ".xls");
                    //file.SaveAs(fileName, true);
                    workbook = new Aspose.Cells.Workbook(fileName);
                    if (workbook.Worksheets.GetSheetByCodeName("DATA") == null)
                    {
                        return response = Request.CreateResponse(HttpStatusCode.NoContent, new { success = false, msg = "File mẫu không đúng định dạng." });
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
                    if (loadToGridGiaMatHang(dtDataHeard, ref tempPath))
                    {

                        return response = Request.CreateResponse(HttpStatusCode.Created);
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
                                FileName = "DataMathangError" + DateTime.Now.ToString("yyyyMMdd-HHMMss") + ".xls"
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

        private bool loadToGridGiaMatHang(DataTable dtDataHeard, ref MemoryStream fileStream)
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
            //        sError = "ID mặt hằng không tồn tại trong hệ thống";
            //        ImportValidate.EmptyValue("ID_MatHang", ref _datarow, ref rowError, ref IsError, sError);
            //        if (row["GiaBuon"].ToString() != "")
            //        {
            //            sError = "Giá bán buôn không đúng định dạng";
            //            ImportValidate.IsValidNumber("GiaBuon", ref _datarow, ref rowError, ref IsError, sError, false, false, lang);
            //        }
            //        if (row["GiaLe"].ToString() != "")
            //        {
            //            sError = "Giá bán lẻ không đúng định dạng";
            //            ImportValidate.IsValidNumber("GiaLe", ref _datarow, ref rowError, ref IsError, sError, true, true, lang);
            //        }
            //        if (row["NgayBatDau"].ToString() != "")
            //        {
            //            sError = "Ngày bắt đầu không đúng định dạng";
            //            ImportValidate.IsValidDate("NgayBatDau", ref _datarow, ref rowError, ref IsError, sError);
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
            //        bool success = ExportExcel.ExportTemplateToStream(@"ReportTemplates\File_Mau_Cap_Nhat_Gia_error.xls", dtError, null, ref fileStream);
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

        private bool loadToGridMatHang(DataTable dtDataHeard, int ID_QLLH, ref MemoryStream fileStream)
        {
            //try
            //{
            //    DataTable dtError = dtDataHeard.Clone();
            //    dtError.TableName = "DATA";
            //    int iRow = 4;
            //    bool IsError = false;
            //    List<string> lstEmp = new List<string>();
            //    foreach (DataRow row in dtDataHeard.Rows)
            //    {
            //        string sError = "";
            //        DataRow _datarow = row as DataRow;
            //        var rowError = dtError.NewRow();
            //        sError = "Mã mặt hàng chưa nhập";
            //        ImportValidate.EmptyValue("maHang", ref _datarow, ref rowError, ref IsError, sError);
            //        sError = "Tên mặt hàng chưa nhập";
            //        ImportValidate.EmptyValue("tenHang", ref _datarow, ref rowError, ref IsError, sError);
            //        sError = "Đơn vị chưa nhập";
            //        ImportValidate.EmptyValue("tenDonVi", ref _datarow, ref rowError, ref IsError, sError);
            //        sError = "Nhóm mặt hàng chưa nhập";
            //        ImportValidate.EmptyValue("tenNhom", ref _datarow, ref rowError, ref IsError, sError);
            //        if (row["maHang"].ToString() != "")
            //        {
            //            DataTable dtdata = CheckMaMatHang( row["maHang"].ToString() , ID_QLLH);
            //            if (int.Parse(dtdata.Rows[0]["SOLUONG"].ToString()) > 0)
            //            {
            //                rowError["maHang"] = "Mã mặt hàng đã tồn tại";
            //                IsError = true;
            //            }
            //        }
            //        if (row["maHang"].ToString() != "" && rowError["maHang"].ToString() == "")
            //        {
            //            string MHCode = row["maHang"].ToString();
            //            if (!lstEmp.Contains(MHCode))
            //                lstEmp.Add(MHCode);
            //            else
            //            {
            //                IsError = true;
            //                rowError["maHang"] = "Mã mặt hàng đã tồn tại trong file import";
            //            }
            //        }
            //        if (row["tenDonVi"].ToString() != "")
            //        {
            //            sError = "Đơn vị";
            //            ImportValidate.IsValidList("tenDonVi", "idDonVi", ref _datarow, ref rowError, ref IsError, sError, lang);
            //        }
            //        if (row["tenNhom"].ToString() != "")
            //        {
            //            sError = "Nhóm mặt hàng";
            //            ImportValidate.IsValidList("tenNhom", "iD_DANHMUC", ref _datarow, ref rowError, ref IsError, sError, lang);
            //        }
            //        if (row["ten_NhaCungCap"].ToString() != "")
            //        {
            //            sError = "Nhà cung cấp";
            //            ImportValidate.IsValidList("ten_NhaCungCap", "iD_NhaCungCap", ref _datarow, ref rowError, ref IsError, sError);
            //        }

            //        if (row["ten_NhanHieu"].ToString() != "")
            //        {
            //            sError = "Nhãn hiệu";
            //            ImportValidate.IsValidList("ten_NhanHieu", "iD_NhanHieu", ref _datarow, ref rowError, ref IsError, sError);
            //        }
            //        if (row["giaBuon"].ToString() != "")
            //        {
            //            sError = "Giá bán buôn không đúng định dạng";
            //            ImportValidate.IsValidNumber("giaBuon", ref _datarow, ref rowError, ref IsError, sError, true, false);
            //        }
            //        if (row["giaLe"].ToString() != "")
            //        {
            //            sError = "Giá bán lẻ không đúng định dạng";
            //            ImportValidate.IsValidNumber("giaLe", ref _datarow, ref rowError, ref IsError, sError, true, true);
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
            //        bool success = ExportExcel.ExportTemplateToStream(@"ReportTemplates\Danh_sach_mat_hang_error.xls", dtError, null, ref fileStream);
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
        [Route("capnhatgia")]
        public HttpResponseMessage CapNhatGiaHang([FromBody] List<MatHangCapNhatGia> model)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();
                string formatString = "dd/MM/yyyy";
                DateTime Ngayapgia = new DateTime(1900, 01, 01);
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    int count = 0;
                    DonHangData donhang = new DonHangData();
                    foreach (MatHangCapNhatGia paramh in model)
                    {
                        try
                        {

                            ApGia gia = new ApGia();
                            if (paramh.NgayBatDauStr != null)
                            {
                                Ngayapgia = DateTime.ParseExact(paramh.NgayBatDauStr, formatString, null);

                                if (Ngayapgia.Year > 1900)
                                {
                                    gia.TuNgay = Ngayapgia;
                                }
                                else
                                {
                                    gia.TuNgay = new DateTime(1900, 01, 01);
                                }
                            }
                            else
                            {
                                gia.TuNgay = new DateTime(1900, 01, 01);
                            }

                            gia.ID_Hang = paramh.ID_MatHang;
                            gia.GiaBanBuon = paramh.GiaBuon;
                            gia.GiaBanLe = paramh.GiaLe;
                            gia.ID_QuanLy = userinfo.ID_QuanLy;
                            bool isValid = true;
                            if (gia.GiaBanBuon <= 0 || gia.GiaBanLe <= 0)
                                isValid = false;
                            if (isValid)
                            {
                                if (gia.TuNgay.Year > 1900)
                                {
                                    if (donhang.ApGiaMoi(gia) > 0)
                                    {
                                        count += 1;

                                    }
                                }
                            }
                        }

                        catch
                        {

                        }
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "Cập nhật thành công giá cho " + count + " mặt hàng" });
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
        [Route("LoadExcelGiaMatHang")]
        public HttpResponseMessage LoadExcelGiaMatHang([FromBody] FileExcelUpload file)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            List<MatHangCapNhatGia> lstMathang = new List<MatHangCapNhatGia>();

            // string formatString = "dd/MM/yyyy";
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
                    dt.Columns.Add("MaHang", typeof(String));
                    dt.Columns.Add("TenHang", typeof(String));
                    dt.Columns.Add("GiaBuon", typeof(String));
                    dt.Columns.Add("GiaLe", typeof(String));
                    dt.Columns.Add("NgayBatDau", typeof(String));
                    dt.Columns.Add("ID_MatHang", typeof(string));

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

                    else
                    {
                        foreach (DataRow row in dtDataHeard.Rows)
                        {
                            MatHangCapNhatGia mathang = new MatHangCapNhatGia();

                            mathang.MaHang = row["MaHang"].ToString();
                            mathang.TenHang = row["TenHang"].ToString();
                            mathang.ID_MatHang = int.Parse(row["ID_MatHang"].ToString());
                            if (row["GiaBuon"].ToString() != "")
                            {

                                mathang.GiaBuon = double.Parse(row["GiaBuon"].ToString());
                            }
                            else
                            {
                                mathang.GiaBuon = 0;
                            }
                            if (row["GiaLe"].ToString() != "")
                            {

                                mathang.GiaLe = double.Parse(row["GiaLe"].ToString());
                            }
                            else
                            {
                                mathang.GiaBuon = 0;
                            }

                            if (row["NgayBatDau"].ToString() != "")
                            {
                                var abc = row["NgayBatDau"].ToString();
                                int day = int.Parse(abc.Split('/')[0].ToString());
                                int month = int.Parse(abc.Split('/')[1].ToString());
                                int year = int.Parse(abc.Split('/')[2].ToString());
                                mathang.NgayBatDau = new DateTime(year, month, day);
                            }
                            lstMathang.Add(mathang);
                        }
                        return response = Request.CreateResponse(HttpStatusCode.OK, lstMathang);
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
        [Route("LoadExcelMatHang")]
        public HttpResponseMessage LoadExcelnhanvien([FromBody] FileExcelUpload file)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            List<MatHangParam> lstMathang = new List<MatHangParam>();

            // string formatString = "dd/MM/yyyy";
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
                    dt.Columns.Add("maHang", typeof(String));
                    dt.Columns.Add("tenHang", typeof(String));
                    dt.Columns.Add("tenDonVi", typeof(String));
                    dt.Columns.Add("tenNhom", typeof(String));
                    dt.Columns.Add("ten_NhaCungCap", typeof(String));
                    dt.Columns.Add("ten_NhanHieu", typeof(String));
                    dt.Columns.Add("giaBuon", typeof(String));
                    dt.Columns.Add("giaLe", typeof(String));
                    dt.Columns.Add("ghiChuGia", typeof(String));
                    dt.Columns.Add("moTa", typeof(String));
                    dt.Columns.Add("linkGioiThieu", typeof(String));
                    dt.Columns.Add("iD_DANHMUC", typeof(String));
                    dt.Columns.Add("idDonVi", typeof(String));
                    dt.Columns.Add("iD_NhaCungCap", typeof(String));
                    dt.Columns.Add("iD_NhanHieu", typeof(String));
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

                    else
                    {
                        foreach (DataRow row in dtDataHeard.Rows)
                        {
                            MatHangParam mathang = new MatHangParam();
                            mathang.MaHang = row["maHang"].ToString();
                            mathang.TenHang = row["tenHang"].ToString();
                            mathang.TenDonVi = row["tenDonVi"].ToString();
                            mathang.Ten_NhaCungCap = row["ten_NhaCungCap"].ToString();
                            mathang.TenDanhMuc = row["tenNhom"].ToString();
                            mathang.Ten_NhanHieu = row["ten_NhanHieu"].ToString();
                            try
                            {
                                mathang.GiaBuon = double.Parse(row["giaBuon"].ToString());
                            }
                            catch
                            {
                                mathang.GiaBuon = 0;
                            }
                            try
                            {
                                mathang.GiaLe = double.Parse(row["giaLe"].ToString());
                            }
                            catch
                            {
                                mathang.GiaLe = 0;
                            }

                            mathang.GhiChuGia = row["ghiChuGia"].ToString();
                            mathang.MoTa = row["moTa"].ToString();
                            mathang.LinkGioiThieu = row["linkGioiThieu"].ToString();

                            if ((row["iD_DANHMUC"].ToString() != "") && (row["iD_DANHMUC"].ToString() != "#N/A"))
                            {
                                mathang.ID_DANHMUC = int.Parse(row["iD_DANHMUC"].ToString());
                            }

                            if ((row["idDonVi"].ToString() != "") && (row["idDonVi"].ToString() != "#N/A"))
                            {
                                mathang.IDDonVi = int.Parse(row["idDonVi"].ToString());
                            }
                            if ((row["iD_NhaCungCap"].ToString() != "") && (row["iD_NhaCungCap"].ToString() != "#N/A"))
                            {
                                mathang.ID_NhaCungCap = int.Parse(row["iD_NhaCungCap"].ToString());
                            }
                            if ((row["iD_NhanHieu"].ToString() != "") && (row["iD_NhanHieu"].ToString() != "#N/A"))
                            {
                                mathang.ID_NhanHieu = int.Parse(row["iD_NhanHieu"].ToString());
                            }
                            lstMathang.Add(mathang);
                        }
                        return response = Request.CreateResponse(HttpStatusCode.OK, lstMathang);
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
        [Route("saveexcelmathang")]
        public HttpResponseMessage SaveExcelData([FromBody] List<MatHangParam> lparammathang)
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
                    MatHang_dl mh_dl = new MatHang_dl();
                    foreach (MatHangParam paramh in lparammathang)
                    {
                        count++;
                        try
                        {
                            MatHang model = new MatHang();
                            model.IDMatHang = 0;
                            model.IDQLLH = userinfo.ID_QLLH;
                            model.SoLuong = 0;
                            model.MaHang = paramh.MaHang;
                            model.TenHang = paramh.TenHang;
                            model.GiaBuon = paramh.GiaBuon;
                            model.GiaLe = paramh.GiaLe;
                            model.IDDonVi = paramh.IDDonVi;
                            model.KhuyenMai = paramh.KhuyenMai;
                            model.GhiChuGia = paramh.GhiChuGia;
                            model.ID_DANHMUC = paramh.ID_DANHMUC;
                            model.ID_NhaCungCap = paramh.ID_NhaCungCap;
                            model.ID_NhanHieu = paramh.ID_NhanHieu;
                            model.MoTa = paramh.MoTa;
                            model.LinkGioiThieu = paramh.LinkGioiThieu;

                            int ID_HangHoa = mh_dl.ThemMatHang(model);
                            if (ID_HangHoa > 0)
                            {
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "MESSAGE_TRANSACTION_SUCCESS" });
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
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "Lưu mặt hàng không thành công. Vui lòng liên hệ quản trị!" });
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        [HttpGet]
        [Route("getmathangall")]
        public HttpResponseMessage GetMatHangAll()
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
                    List<MatHang> dsmh = mh_dl.GetMatHangAll(userinfo.ID_QLLH);
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
        [Route("getloinhuanbymathang")]
        public HttpResponseMessage getloinhuanbymathang([FromUri] int ID_MatHang)
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
                    HangHoa_LoiNhanData hhlndata = new HangHoa_LoiNhanData();
                    List<HangHoa_LoiNhuanModel> lst = hhlndata.GetAllByHangHoa(ID_MatHang);
                    response = Request.CreateResponse(HttpStatusCode.OK, lst);
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
        [Route("setloinhuanbymathang")]
        public HttpResponseMessage setloinhuanbymathang([FromBody] HangHoa_LoiNhuanModel model)
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
                    HangHoa_LoiNhanData hhln = new HangHoa_LoiNhanData();
                    if (hhln.InsertOrUpdate(model))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "Lưu thông tin thành công!" });

                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "Lưu thông tin không thành công. Vui lòng liên hệ quản trị!" });
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "Lưu thông tin không thành công. Vui lòng liên hệ quản trị!" });
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }


        [HttpPost]
        [Route("updatetrangthailoinhuanbymathang")]
        public HttpResponseMessage disableloinhuanbymathang([FromBody] HangHoa_LoiNhuanModel model)
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
                    HangHoa_LoiNhanData hhln = new HangHoa_LoiNhanData();
                    if (hhln.UpdateTrangThai(model.ID, model.TrangThai))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "Cập nhật thông tin thành công!" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "Cập nhật thông tin không thành công. Vui lòng liên hệ quản trị!" });
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "Cập nhật thông tin không thành công. Vui lòng liên hệ quản trị!" });
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        public DataTable CheckMaMatHang(string MaMatHang, int ID_QLLH)
        {
            try
            {
                SqlDataHelper helper = new SqlDataHelper();

                DataSet ds = new DataSet();
                SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("ID_QLLH", ID_QLLH),
            new SqlParameter("MaHang", MaMatHang),

        };
                ds = helper.ExecuteDataSet("sp_HangHoa_CheckMaHangTonTai", pars);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
        public class NhomMatHangCreateModel
        {
            public int ID_Parent { get; set; }
            public int ID_Nhom { get; set; }
            public string TenNhom { get; set; }
            public string AnhDaiDien { get; set; }
        }
        private class RequestGridParam
        {
            public DataSourceRequest request { get; set; }
            public int ID_DanhMuc { get; set; }
        }

        public class FilterMatHangGrid
        {
            public string MaHang { get; set; }
            public string TenHang { get; set; }
            public string DonVi { get; set; }
            public double GiaBuon { get; set; }
            public double GiaLe { get; set; }
            public string DanhMuc { get; set; }
            public string GhiChu { get; set; }
        }
        public class NhomMatHang
        {
            public int ID { get; set; }
            public int ID_Parent { get; set; }
            public string Name { get; set; }
            public string TenMatHang { get; set; }
            public string AnhDaiDien { get; set; }
            public bool HasChilds { get; set; }
            public List<NhomMatHang> Childs { get; set; }
        }
        public class MatHangCapNhatGia
        {
            public string TenHang { get; set; }
            public string MaHang { get; set; }
            public double GiaBuon { get; set; }
            public double GiaLe { get; set; }
            public DateTime NgayBatDau { get; set; }
            public string NgayBatDauStr { get; set; }
            public int ID_MatHang { get; set; }
        }
        public class MatHangParam
        {
            public string GhiChuGia { get; set; }
            public double? GiaBuon { get; set; }
            public double? GiaLe { get; set; }
            public int IDDonVi { get; set; }
            public string TenDonVi { get; set; }
            public int IDMatHang { get; set; }
            public int IDQLLH { get; set; }
            public int ID_DANHMUC { get; set; }
            //public string TenNhom { get;set }
            public int ID_NhaCungCap { get; set; }
            public string Ten_NhaCungCap { get; set; }
            public int ID_NhanHieu { get; set; }
            public string Ten_NhanHieu { get; set; }
            public string KhuyenMai { get; set; }
            public string LinkGioiThieu { get; set; }
            public string MaHang { get; set; }
            public string MoTa { get; set; }
            public double SoLuong { get; set; }
            public double SoLuongDieuChuyenKho { get; set; }
            public double SoLuongTon { get; set; }
            public string TenDanhMuc { get; set; }
            //   public string TenDonVi { get; set; }
            public string TenHang { get; set; }
        }


    }
}
