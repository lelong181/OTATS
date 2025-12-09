
using LSPosMVC.Common;
using System;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LSPos_Data.Data;
using System.IO;
using System.Net.Http.Headers;
using LSPos_Data.Models;
using LSPosMVC.Models;
using System.Web.Http.Cors;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/phieunhap")]
    [EnableCors(origins: "*", "*", "*")]
    public class PhieuNhapKhoController : ApiController
    {
        [HttpGet]
        [Route("getlist")]
        public HttpResponseMessage getlist([FromUri] DateTime from, DateTime to)
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
                    PhieuNhap_dl pn = new PhieuNhap_dl();
                    DataTable dt = pn.GetDanhSachPhieuNhap(userinfo.ID_QLLH, from, to);
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

        [HttpGet]
        [Route("getlistdetail")]
        public HttpResponseMessage getlistdetail([FromUri] int idphieunhap)
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
                    PhieuNhap_dl pn = new PhieuNhap_dl();
                    DataTable dt = pn.GetChiTietPhieuNhapById(idphieunhap);
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
        [Route("themphieunhap")]
        public HttpResponseMessage themphieunhap([FromBody] PhieuNhapOBJ phieunhap)
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
                    if (phieunhap.ID_Kho <= 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "Kho hàng không được để trống" });
                    }
                    else
                    {
                        PhieuNhap_dl pn = new PhieuNhap_dl();
                        PhieuNhapOBJ pnNew = new PhieuNhapOBJ();
                        pnNew.ID_QLLH = userinfo.ID_QLLH;
                        pnNew.ID_QuanLy = userinfo.ID_QuanLy;
                        pnNew.NoiDung = phieunhap.NoiDung;
                        pnNew.ID_Kho = phieunhap.ID_Kho;
                        pnNew.NgayNhap = DateTime.Now;
                        pnNew = PhieuNhap_dl.TaoPhieuNhap(pnNew);
                        if (pnNew.ID_PhieuNhap > 0)
                        {
                            foreach (PhieuNhapChiTietOBJ obj in phieunhap.ChiTiet)
                            {
                                obj.ID_PhieuNhap = pnNew.ID_PhieuNhap;
                                obj.ID_Kho = phieunhap.ID_Kho;
                                PhieuNhap_dl.ThemChiTietPhieuNhap(obj);
                                LSPos_Data.Utilities.Log.Info("Chi tiết phiếu nhập: ID_PhieuNhap:" + obj.ID_PhieuNhap + "|SoLuong:" + obj.SoLuong + "|ID_HangHoa:" + obj.ID_HangHoa);
                            }

                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "Thêm mới thành công" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "Thêm mới thất bại vui lòng kiểm tra lại trường dữ liệu" });
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
        [Route("gettemplatephieunhap")]
        public HttpResponseMessage gettemplatephieunhap()
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
                    dt.TableName = "DATA";

                    DataSet dataSet = new DataSet();
                    dataSet.Tables.Add(dt.Copy());

                    BaoCaoCommon baocao = new BaoCaoCommon();
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    string filename = "";
                    var stream = new MemoryStream();
                    ExportExcel excel = new ExportExcel();

                    if (lang == "vi")
                    {
                        filename = "BM030_FileMauPhieuNhapKho_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("TeamplateImport_PhieuNhap.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM030_GoodsReceiptTemplate_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("TeamplateImport_PhieuNhap_en.xls", dataSet, null, ref stream);
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
        [Route("importphieunhap")]
        public HttpResponseMessage importphieunhap([FromBody] FileUploadModelFilter file)
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
                    BaoCaoCommon baocao = new BaoCaoCommon();
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);


                    DataTable dt = new DataTable();
                    dt.Columns.Add("STT", typeof(String));
                    dt.Columns.Add("TenHang", typeof(String));
                    dt.Columns.Add("MaHang", typeof(String));
                    dt.Columns.Add("SoLuong", typeof(String));
                    dt.Columns.Add("TenKho", typeof(String));

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

                    if (validatedataphieunhap(dtDataHeard, userinfo, ref tempPath))
                    {
                        try
                        {
                            var dataView = new DataView(dtDataHeard);
                            dataView.Sort = "TenKho ASC, MaHang ASC, TenHang ASC";
                            DataTable dataTable = dataView.ToTable();

                            int idkhohang_new = -1;
                            int idkhohang_old = -1;
                            int idphieunhap = -1;

                            foreach (DataRow row in dataTable.Rows)
                            {
                                int idmathang = 0;
                                float soluong = 0;
                                if ((row["TenHang"].ToString() != ""))
                                {
                                    MatHang_dl mhdl = new MatHang_dl();
                                    MatHang mh = mhdl.GetMatHangTheoTenHang(row["TenHang"].ToString().Trim(), row["MaHang"].ToString().Trim(), userinfo.ID_QLLH);
                                    idmathang = mh.IDMatHang;
                                }

                                if ((row["TenKho"].ToString() != ""))
                                {
                                    KhoDB khoDB = new KhoDB();
                                    KhoOBJ kho = khoDB.GetKhoByName(row["TenKho"].ToString(), userinfo.ID_QLLH);
                                    idkhohang_new = kho.ID_Kho;
                                }

                                if ((row["SoLuong"].ToString() != "") && (row["SoLuong"].ToString() != "#N/A"))
                                    soluong = float.Parse(row["SoLuong"].ToString().Trim());

                                if (idkhohang_new != idkhohang_old)
                                {
                                    PhieuNhapOBJ pnNew = new PhieuNhapOBJ();
                                    pnNew.ID_QLLH = userinfo.ID_QLLH;
                                    pnNew.ID_QuanLy = userinfo.ID_QuanLy;
                                    if (lang == "en")
                                    {
                                        pnNew.NoiDung = "Import from excel";
                                    }
                                    else  
                                    {
                                        pnNew.NoiDung = "Phiếu nhập từ excel";
                                    }
                                    pnNew.ID_Kho = idkhohang_new;
                                    pnNew.NgayNhap = DateTime.Now;
                                    pnNew = PhieuNhap_dl.TaoPhieuNhap(pnNew);
                                    idphieunhap = pnNew.ID_PhieuNhap;
                                    if (idphieunhap > 0)
                                    {
                                        PhieuNhapChiTietOBJ obj = new PhieuNhapChiTietOBJ();
                                        obj.ID_PhieuNhap = idphieunhap;
                                        obj.ID_Kho = idkhohang_new;
                                        obj.ID_HangHoa = idmathang;
                                        obj.SoLuong = soluong;
                                        PhieuNhap_dl.ThemChiTietPhieuNhap(obj);
                                    }
                                }
                                else
                                {
                                    if (idphieunhap > 0)
                                    {
                                        PhieuNhapChiTietOBJ obj = new PhieuNhapChiTietOBJ();
                                        obj.ID_PhieuNhap = idphieunhap;
                                        obj.ID_Kho = idkhohang_new;
                                        obj.ID_HangHoa = idmathang;
                                        obj.SoLuong = soluong;
                                        PhieuNhap_dl.ThemChiTietPhieuNhap(obj);
                                    }
                                }

                                idkhohang_old = idkhohang_new;
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
                       
                        string filename = "";
                        if (lang == "vi")
                        {
                            filename = "BM031_FileMauPhieuNhapKhoError_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        }
                        else
                        {
                            filename = "BM031_GoodsReceiptTemplateError_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
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

        private bool validatedataphieunhap(DataTable dtDataHeard, UserInfo userinfo, ref MemoryStream fileStream)
        {
            try
            {
                BaoCaoCommon baocao = new BaoCaoCommon();
                string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                DataTable dtError = dtDataHeard.Clone();
                dtError.TableName = "DATA";

                int iRow = 4;
                bool IsError = false;
                foreach (DataRow row in dtDataHeard.Rows)
                {
                    string sError = "";
                    DataRow _datarow = row as DataRow;
                    var rowError = dtError.NewRow();

                    sError = (lang == "vi") ? "Tên mặt hàng chưa nhập" : "Product name is missing";
                    ImportValidate.EmptyValue("TenHang", ref _datarow, ref rowError, ref IsError, sError);

                    sError = (lang == "vi") ? "Mã mặt hàng chưa nhập" : "Product ID is missing";
                    ImportValidate.EmptyValue("MaHang", ref _datarow, ref rowError, ref IsError, sError);

                    sError = (lang == "vi") ? "Số lượng chưa nhập" : "Quantity is missing";
                    ImportValidate.EmptyValue("SoLuong", ref _datarow, ref rowError, ref IsError, sError);

                    sError = (lang == "vi") ? "Tên kho hàng chưa nhập" : "Warehouse is missing";
                    ImportValidate.EmptyValue("TenKho", ref _datarow, ref rowError, ref IsError, sError);

                    if (row["SoLuong"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Số lượng không phải kiểu số" : "Quantity is currently not in number format";
                        ImportValidate.IsValidNumber("SoLuong", ref _datarow, ref rowError, ref IsError, sError, false, false, lang);
                    }

                    if ((row["TenHang"].ToString() != ""))
                    {
                        MatHang_dl mhdl = new MatHang_dl();
                        MatHang mh = mhdl.GetMatHangTheoTenHang(row["TenHang"].ToString().Trim(), row["MaHang"].ToString().Trim(), userinfo.ID_QLLH);
                        if (mh == null || mh.IDMatHang <= 0)
                        {
                            rowError["TenHang"] = (lang == "vi") ? "Mặt hàng không tồn tại" : "Product does not exist";
                            IsError = true;
                        }
                    }

                    if ((row["TenKho"].ToString() != ""))
                    {
                        KhoDB khoDB = new KhoDB();
                        KhoOBJ kho = khoDB.GetKhoByName(row["TenKho"].ToString(), userinfo.ID_QLLH);
                        if (kho == null || kho.ID_Kho < 1)
                        {
                            rowError["TenKho"] = (lang == "vi") ? "Kho không tồn tại" : "Warehouse does not exist";
                            IsError = true;
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
                        ExportExcel.ExportTemplateToStream("TeamplateImport_PhieuNhap_error.xls", dtError, null, ref fileStream);
                    }
                    else
                    {
                        ExportExcel.ExportTemplateToStream("TeamplateImport_PhieuNhap_error_en.xls", dtError, null, ref fileStream);
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

        [HttpGet]
        [Route("excellistphieunhap")]
        public HttpResponseMessage excellistphieunhap([FromUri] DateTime from, DateTime to)
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
                    PhieuNhapKhoData pn = new PhieuNhapKhoData();
                    DataSet ds = pn.getexcelphieunhap(userinfo.ID_QLLH, from, to);
                    
                    DataTable dt = new DataTable();
                    dt.TableName = "DATA";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dt = GroupPhieuNhap(ds);
                        dt.TableName = "DATA";
                    }

                    BaoCaoCommon baocao = new BaoCaoCommon();
                    DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    dt2.TableName = "DATA2";
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    string title = "";
                    if (lang == "vi")
                    {
                        title = "Từ " + from.ToString("dd/MM/yyyy") + " đến " + to.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        title = "From " + from.ToString("dd/MM/yyyy") + " to " + to.ToString("dd/MM/yyyy");
                    }

                    DataTable dt1 = new DataTable();
                    dt1.TableName = "DATA1";
                    dt1.Columns.Add("TITLE", typeof(String));
                    DataRow row = dt1.NewRow();
                    row["TITLE"] = title;
                    dt1.Rows.Add(row);

                    DataSet dataSet = new DataSet();
                    dataSet.Tables.Add(dt.Copy());
                    dataSet.Tables.Add(dt1.Copy());
                    dataSet.Tables.Add(dt2.Copy());

                    string filename = "";
                    var stream = new MemoryStream();
                    ExportExcel excel = new ExportExcel();
                    if (lang == "vi")
                    {
                        filename = "BM029_PhieuNhapKho_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("BaoCao_DanhSachPhieuNhap.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM029_GoodsReceiptNote_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("BaoCao_DanhSachPhieuNhap_en.xls", dataSet, null, ref stream);
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
        private DataTable GroupPhieuNhap(DataSet _dataset)
        {
            DataTable result = _dataset.Tables[0].Copy();
            result.Clear();
            try
            {
                for (int i = 0; i < _dataset.Tables[1].Rows.Count; i++)
                {
                    DataRow _row_master = result.NewRow();
                    _row_master["STT"] = _dataset.Tables[1].Rows[i]["TenPhieuNhap"].ToString();
                    _row_master["IsGroup"] = "1";

                    result.Rows.Add(_row_master);

                    for (int j = 0; j < _dataset.Tables[0].Rows.Count; j++)
                    {
                        if (_dataset.Tables[1].Rows[i]["ID_PhieuNhap"].ToString() == _dataset.Tables[0].Rows[j]["ID_PhieuNhap"].ToString())
                        {
                            DataRow _row_detail = result.NewRow();
                            _row_detail = _dataset.Tables[0].Rows[j];
                            result.ImportRow(_row_detail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return result;
        }
    }
}
