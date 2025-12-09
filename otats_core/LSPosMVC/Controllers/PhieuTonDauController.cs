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
using LSPos_Data.Data;
using Ksmart_DataSon.Models;
using LSPos_Data.Models;
using Ksmart_DataSon.DataAccess;
using System.Globalization;
using LSPosMVC.Models;
using System.Web.Http.Cors;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/tonkhodau")]
    [EnableCors(origins: "*", "*", "*")]
    public class PhieuTonDauController : ApiController
    {

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage add([FromBody] ParamPhieu paramPhieu)
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
                    PhieuTonDauData phieuTonDauData = new PhieuTonDauData();

                    PhieuTonDauModel phieu = paramPhieu.PhieuTonDau;
                    phieu.ID_QLLH = userinfo.ID_QLLH;
                    phieu.ID_NhanVien = userinfo.ID_QuanLy;
                    phieu.UpdateBy = userinfo.Username;

                    int idPhieu = phieuTonDauData.addPhieuTonDau(phieu);

                    string listid = "";
                    List<PhieuTonDauChiTietModel> listChiTiet = paramPhieu.ChiTietMatHangTonDau;
                    foreach (PhieuTonDauChiTietModel ct in listChiTiet)
                    {
                        ct.ID_PhieuTonDau = idPhieu;
                        ct.UpdateBy = userinfo.Username;

                        int idChiTietPhieu = phieuTonDauData.addChiTietTonDau(ct);

                        listid = listid + ct.ID_HangHoa.ToString() + ",";
                    }

                    //Đánh dấu xóa các dòng bị xóa (không thuộc listid)
                    bool result = phieuTonDauData.deletemarkmulti(idPhieu, listid, userinfo.Username);
                    if (result)
                        response = Request.CreateResponse(HttpStatusCode.OK, idPhieu);
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
        [Route("deletemark")]
        public HttpResponseMessage deletemark([FromBody] int id, string updatedBy, bool deleteMark)
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
                    BangGiaLoaiKhachHangData bangGiaData = new BangGiaLoaiKhachHangData();

                    if (bangGiaData.deletemark(id, updatedBy, deleteMark))
                        response = Request.CreateResponse(HttpStatusCode.OK, true);
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
        [Route("getphieutondau")]
        public HttpResponseMessage getphieutondau([FromUri] int idKho)
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
                    ResultTonDau resultTonDau = new ResultTonDau();
                    PhieuTonDauData phieuTonDauData = new PhieuTonDauData();
                    DataSet ds = phieuTonDauData.getphieutondau(userinfo.ID_QLLH, idKho);
                    if (ds != null)
                    {
                        DataTable dtTonDau = ds.Tables[0];
                        DataTable dtChiTiet = ds.Tables[1];
                        DataTable dtMatHang = ds.Tables[2];

                        PhieuTonDauModel itemTonDau = new PhieuTonDauModel();
                        foreach (DataRow dr in dtTonDau.Rows)
                        {

                            itemTonDau.ID_PhieuTonDau = Convert.ToInt32(dr["ID_PhieuTonDau"].ToString());
                            itemTonDau.ID_NhanVien = Convert.ToInt32(dr["ID_NhanVien"].ToString());
                            itemTonDau.ID_KhoHang = Convert.ToInt32(dr["ID_KhoHang"].ToString());
                            itemTonDau.DienGiai = dr["DienGiai"].ToString();
                            try
                            {
                                itemTonDau.NgayChotTon = Convert.ToDateTime(dr["NgayChotTon"].ToString());
                            }
                            catch (Exception ex)
                            {
                                LSPos_Data.Utilities.Log.Error(ex);
                            }
                            break;
                        }
                        resultTonDau.PhieuTonDau = itemTonDau;

                        List<PhieuTonDauChiTietModel> list = new List<PhieuTonDauChiTietModel>();
                        foreach (DataRow dr in dtChiTiet.Rows)
                        {
                            PhieuTonDauChiTietModel item = new PhieuTonDauChiTietModel();
                            item.ID_PhieuTonDau = Convert.ToInt32(dr["ID_PhieuTonDau"].ToString());
                            item.ID_ChiTietPhieuTonDau = Convert.ToInt32(dr["ID_ChiTietPhieuTonDau"].ToString());
                            item.ID_HangHoa = Convert.ToInt32(dr["ID_HangHoa"].ToString());

                            item.MaHang = dr["MaHang"].ToString();
                            item.TenHang = dr["TenHang"].ToString();
                            item.TenDonVi = dr["TenDonVi"].ToString();
                            try
                            {
                                item.SoLuong = Convert.ToSingle(dr["SoLuong"].ToString());
                            }
                            catch (Exception ex)
                            {
                                item.SoLuong = 0;
                                LSPos_Data.Utilities.Log.Error(ex);
                            }
                            list.Add(item);
                        }
                        resultTonDau.ChiTietMatHangTonDau = list;

                        List<MatHang> listMatHang = new List<MatHang>();
                        foreach (DataRow dr in dtMatHang.Rows)
                        {
                            MatHang item = new MatHang();
                            item.IDMatHang = Convert.ToInt32(dr["ID_Hang"].ToString());
                            item.MaHang = dr["MaHang"].ToString();
                            item.TenHang = dr["TenHang"].ToString();
                            item.TenDonVi = dr["TenDonVi"].ToString();

                            listMatHang.Add(item);
                        }
                        resultTonDau.DanhSachMatHang = listMatHang;

                        response = Request.CreateResponse(HttpStatusCode.OK, resultTonDau);
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

        #region import excel old
        [AllowAnonymous]
        [HttpGet]
        [Route("Getteamplate_TonDau")]
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
                    PhieuTonDauData donhang = new PhieuTonDauData();
                    DataSet ds = donhang._GettemplateTonDau(userinfo.ID_QLLH);
                    ds.Tables[0].TableName = "DATA";
                    ds.Tables[1].TableName = "DATA1";
                    for (int i1 = 1; i1 <= 2000; i1++)
                    {
                        var row = ds.Tables[0].NewRow();
                        ds.Tables[0].Rows.Add(row);
                    }
                    string tempPath = "";
                    ExportExcel excel = new ExportExcel();
                    excel.ExportTemplate(@"ReportTemplates\File_Mau_Ton_Kho_Ban_Dau.xls", ds, null, ref tempPath);

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
        [HttpPost]
        [Route("checkfileImportTonKho")]
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
                    dt.Columns.Add("MatHang", typeof(String));
                    dt.Columns.Add("TenHang", typeof(String));
                    dt.Columns.Add("SoLuong", typeof(String));
                    dt.Columns.Add("TenKho", typeof(String));
                    dt.Columns.Add("NgayChot", typeof(String));
                    dt.Columns.Add("ID_Kho", typeof(String));
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
                    List<PhieuTonDauChiTietModel> lstPhieuTonDau = new List<PhieuTonDauChiTietModel>();
                    string formatString = "dd/MM/yyyy";
                    if (loadToGridTonDau(dtDataHeard, userinfo.ID_QLLH, ref tempPath))
                    {
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        PhieuTonDauData phieuTonDauData = new PhieuTonDauData();

                        int idPhieuTonDau = 0;
                        foreach (DataRow row in dtDataHeard.Rows)
                        {
                            PhieuTonDauModel phieu = new PhieuTonDauModel();
                            DataTable dtdata = GetMatHangID(row["MaHang"].ToString(), userinfo.ID_QLLH);
                            phieu.ID_KhoHang = int.Parse(row["ID_Kho"].ToString());
                            phieu.ID_QLLH = userinfo.ID_QLLH;
                            phieu.ID_NhanVien = userinfo.ID_QuanLy;
                            phieu.UpdateBy = userinfo.Username;
                            phieu.NgayChotTon = DateTime.ParseExact(row["NgayChot"].ToString(), formatString, provider);

                            idPhieuTonDau = phieuTonDauData.getidphieutondau(userinfo.ID_QLLH, phieu.ID_KhoHang);
                            if (idPhieuTonDau == 0)
                                idPhieuTonDau = phieuTonDauData.addPhieuTonDau(phieu);

                            if (idPhieuTonDau > 0)
                            {
                                PhieuTonDauChiTietModel phieuChiTiet = new PhieuTonDauChiTietModel();


                                phieuChiTiet.ID_PhieuTonDau = idPhieuTonDau;
                                phieuChiTiet.UpdateBy = userinfo.Username;

                                phieuChiTiet.ID_HangHoa = int.Parse(dtdata.Rows[0]["ID_Hang"].ToString());
                                phieuChiTiet.SoLuong = row["SoLuong"].ToString().Trim() != "" ? int.Parse(row["SoLuong"].ToString()) : 0;

                                phieuChiTiet.ID_ChiTietPhieuTonDau = phieuTonDauData.getidphieuChiTiettondau(userinfo.ID_QLLH, phieuChiTiet.ID_HangHoa);

                                int idChiTietPhieu = phieuTonDauData.addChiTietTonDau(phieuChiTiet);
                            }
                        }

                        return response = Request.CreateResponse(HttpStatusCode.OK);
                    }

                    else
                    {
                        response = new HttpResponseMessage(HttpStatusCode.Created)
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
        [Route("LoadExcelTonKho")]
        public HttpResponseMessage LoadExcelnhanvien([FromBody] FileExcelUpload file)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            List<PhieuTonDauChiTietModel> lstPhieuTonDau = new List<PhieuTonDauChiTietModel>();

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
                    dt.Columns.Add("MatHang", typeof(String));
                    dt.Columns.Add("TenHang", typeof(String));
                    dt.Columns.Add("SoLuong", typeof(String));
                    dt.Columns.Add("TenKho", typeof(String));
                    dt.Columns.Add("NgayChot", typeof(String));
                    dt.Columns.Add("ID_Kho", typeof(String));
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
                            PhieuTonDauChiTietModel PhieuTonDau = new PhieuTonDauChiTietModel();
                            PhieuTonDau.ID_HangHoa = 0;
                            PhieuTonDau.MaHang = row["maHang"].ToString();
                            PhieuTonDau.TenHang = row["tenHang"].ToString();
                            PhieuTonDau.TenDonVi = "";
                            if ((row["SoLuong"].ToString().Trim() != ""))
                            {
                                PhieuTonDau.SoLuong = int.Parse(row["SoLuong"].ToString());
                            }
                            DataTable dtdata = GetMatHangID(row["MaHang"].ToString(), userinfo.ID_QLLH);
                            if (row["NgayChot"].ToString() != "")
                            {
                                var abc = row["NgayChot"].ToString();
                                int day = int.Parse(abc.Split('/')[0].ToString());
                                int month = int.Parse(abc.Split('/')[1].ToString());
                                int year = int.Parse(abc.Split('/')[2].ToString());
                                PhieuTonDau.NgayChot = new DateTime(year, month, day);
                            }
                            if (dtdata.Rows.Count > 0)
                            {
                                PhieuTonDau.ID_HangHoa = int.Parse(dtdata.Rows[0]["ID_Hang"].ToString());
                                PhieuTonDau.TenDonVi = dtdata.Rows[0]["TenDonVi"].ToString();
                            }
                            if ((row["ID_Kho"].ToString() != "") && (row["ID_Kho"].ToString() != "#N/A"))
                            {
                                PhieuTonDau.ID_Kho = int.Parse(row["ID_Kho"].ToString());
                            }
                            lstPhieuTonDau.Add(PhieuTonDau);
                        }
                        return response = Request.CreateResponse(HttpStatusCode.OK, lstPhieuTonDau);
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
        private bool loadToGridTonDau(DataTable dtDataHeard, int ID_QLLH, ref MemoryStream fileStream)
        {
            //try
            //{
            //    DataTable dtError = dtDataHeard.Clone();
            //    dtError.TableName = "DATA";
            //    int iRow = 4;
            //    bool IsError = false;
            //    //List<string> lstEmp = new List<string>();
            //    List<CHECKTRUNG> lstEmp = new List<CHECKTRUNG>();
            //    foreach (DataRow row in dtDataHeard.Rows)
            //    {
            //        string sError = "";
            //        DataRow _datarow = row as DataRow;
            //        var rowError = dtError.NewRow();
            //        sError = "Mã mặt hàng chưa nhập";
            //        ImportValidate.EmptyValue("MaHang", ref _datarow, ref rowError, ref IsError, sError);

            //        sError = "Tên mặt hàng chưa nhập";
            //        ImportValidate.EmptyValue("TenHang", ref _datarow, ref rowError, ref IsError, sError);

            //        sError = "Tên kho chưa nhập";
            //        ImportValidate.EmptyValue("TenKho", ref _datarow, ref rowError, ref IsError, sError);

            //        sError = "Ngày chốt chưa nhập";
            //        ImportValidate.EmptyValue("NgayChot", ref _datarow, ref rowError, ref IsError, sError);

            //        if (row["MaHang"].ToString().Trim() != "")
            //        {
            //            DataTable dtdata = CheckMaMatHang(row["MaHang"].ToString(), ID_QLLH);
            //            if (int.Parse(dtdata.Rows[0]["SOLUONG"].ToString()) == 0)
            //            {
            //                rowError["MaHang"] = "Mã mặt hàng không tồn tại trong hệ thống";
            //                IsError = true;
            //            }
            //        }
            //        if (row["MaHang"].ToString().Trim() != "" && rowError["MaHang"].ToString() == "" && rowError["TenKHo"].ToString() == "" && rowError["TenKHo"].ToString() != "")
            //        {
            //            string MHCode = row["MaHang"].ToString();
            //            int ID_KHo = int.Parse(row["ID_Kho"].ToString());
            //            var query = from p in lstEmp
            //                        where p.MaHang.Contains(MHCode) && p.ID_Kho.Equals(ID_KHo)
            //                        select p;
            //            if (query != null)
            //            {
            //                CHECKTRUNG check = new CHECKTRUNG();
            //                check.MaHang = MHCode;
            //                check.ID_Kho = ID_KHo;
            //                lstEmp.Add(check);
            //            }
            //            else
            //            {
            //                IsError = true;
            //                rowError["MaHang"] = "Mã mặt hàng đã tồn tại trong file import";
            //            }
            //        }
            //        //if (row["MaHang"].ToString().Trim() != "" && rowError["MaHang"].ToString() == "")
            //        //{
            //        //    string MHCode = row["MaHang"].ToString();
            //        //    if (!lstEmp.Contains(MHCode))
            //        //        lstEmp.Add(MHCode);
            //        //    else
            //        //    {
            //        //        IsError = true;
            //        //        rowError["MaHang"] = "Mã mặt hàng đã tồn tại trong file import";
            //        //    }
            //        //}
            //        if (row["NgayChot"].ToString() != "")
            //        {
            //            sError = "Ngày chốt không đúng định dạng";
            //            ImportValidate.IsValidDate("NgayChot", ref _datarow, ref rowError, ref IsError, sError);
            //        }
            //        if (row["SoLuong"].ToString() != "")
            //        {
            //            sError = "Số lương không đúng định dạng";
            //            ImportValidate.IsValidNumber("SoLuong", ref _datarow, ref rowError, ref IsError, sError, true, true);
            //        }

            //        if (row["TenKho"].ToString() != "")
            //        {
            //            sError = "Tên Kho";
            //            ImportValidate.IsValidList("TenKho", "ID_Kho", ref _datarow, ref rowError, ref IsError, sError, lang);
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
            //        bool success = ExportExcel.ExportTemplateToStream(@"ReportTemplates\File_Mau_Ton_Kho_Ban_Dau_Error.xls", dtError, null, ref fileStream);
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
        #endregion

        #region import excel new
        [HttpGet]
        [Route("gettemplatephieutondau")]
        public HttpResponseMessage gettemplatephieutondau()
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
                    PhieuTonDauData phieuTonDauData = new PhieuTonDauData();
                    DataSet dataSet = phieuTonDauData._GettemplateTonDau(userinfo.ID_QLLH);
                    dataSet.Tables[0].TableName = "DATA";
                    dataSet.Tables[1].TableName = "DATA1";
                    for (int i1 = 1; i1 <= 2000; i1++)
                    {
                        var row = dataSet.Tables[0].NewRow();
                        dataSet.Tables[0].Rows.Add(row);
                    }

                    BaoCaoCommon baocao = new BaoCaoCommon();
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    string filename = "";
                    var stream = new MemoryStream();
                    ExportExcel excel = new ExportExcel();

                    if (lang == "vi")
                    {
                        filename = "BM026_FileMauTondau_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("File_Mau_Ton_Kho_Ban_Dau.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM026_RemainQuantityTemplate_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("File_Mau_Ton_Kho_Ban_Dau_en.xls", dataSet, null, ref stream);
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
        [Route("importphieutondau")]
        public HttpResponseMessage importphieutondau([FromBody] FileUploadModelFilter file)
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
                    dt.Columns.Add("MatHang", typeof(String));
                    dt.Columns.Add("TenHang", typeof(String));
                    dt.Columns.Add("SoLuong", typeof(String));
                    dt.Columns.Add("TenKho", typeof(String));
                    dt.Columns.Add("NgayChot", typeof(String));
                    dt.Columns.Add("ID_Kho", typeof(String));

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

                    if (validatedataphieutondau(dtDataHeard, userinfo, ref tempPath))
                    {
                        try
                        {
                            CultureInfo provider = CultureInfo.InvariantCulture;
                            PhieuTonDauData phieuTonDauData = new PhieuTonDauData();

                            int idPhieuTonDau = 0;
                            foreach (DataRow row in dtDataHeard.Rows)
                            {
                                PhieuTonDauModel phieu = new PhieuTonDauModel();
                                DataTable dtdata = GetMatHangID(row["MaHang"].ToString(), userinfo.ID_QLLH);
                                phieu.ID_KhoHang = int.Parse(row["ID_Kho"].ToString());
                                phieu.ID_QLLH = userinfo.ID_QLLH;
                                phieu.ID_NhanVien = userinfo.ID_QuanLy;
                                phieu.UpdateBy = userinfo.Username;
                                phieu.NgayChotTon = DateTime.ParseExact(row["NgayChot"].ToString(), "dd/MM/yyyy", provider);

                                idPhieuTonDau = phieuTonDauData.getidphieutondau(userinfo.ID_QLLH, phieu.ID_KhoHang);
                                if (idPhieuTonDau == 0)
                                    idPhieuTonDau = phieuTonDauData.addPhieuTonDau(phieu);

                                if (idPhieuTonDau > 0)
                                {
                                    PhieuTonDauChiTietModel phieuChiTiet = new PhieuTonDauChiTietModel();

                                    phieuChiTiet.ID_PhieuTonDau = idPhieuTonDau;
                                    phieuChiTiet.UpdateBy = userinfo.Username;

                                    phieuChiTiet.ID_HangHoa = int.Parse(dtdata.Rows[0]["ID_Hang"].ToString());
                                    phieuChiTiet.SoLuong = row["SoLuong"].ToString().Trim() != "" ? int.Parse(row["SoLuong"].ToString()) : 0;

                                    phieuChiTiet.ID_ChiTietPhieuTonDau = phieuTonDauData.getidphieuChiTiettondau(userinfo.ID_QLLH, phieuChiTiet.ID_HangHoa);

                                    int idChiTietPhieu = phieuTonDauData.addChiTietTonDau(phieuChiTiet);
                                }
                            }

                            return response = Request.CreateResponse(HttpStatusCode.OK);
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
                            filename = "BM027_FileMauTondauError_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        }
                        else
                        {
                            filename = "BM027_RemainQuantityTemplateError_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
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

        private bool validatedataphieutondau(DataTable dtDataHeard, UserInfo userinfo, ref MemoryStream fileStream)
        {
            try
            {
                BaoCaoCommon baocao = new BaoCaoCommon();
                string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                DataTable dtError = dtDataHeard.Clone();
                dtError.TableName = "DATA";

                int iRow = 4;
                bool IsError = false;
                List<CHECKTRUNG> lstEmp = new List<CHECKTRUNG>();
                foreach (DataRow row in dtDataHeard.Rows)
                {
                    string sError = "";
                    DataRow _datarow = row as DataRow;
                    var rowError = dtError.NewRow();
                    sError = (lang == "vi") ? "Mã mặt hàng chưa nhập" : "Product code is missing";
                    ImportValidate.EmptyValue("MaHang", ref _datarow, ref rowError, ref IsError, sError);

                    sError = (lang == "vi") ? "Tên mặt hàng chưa nhập" : "Product name is missing";
                    ImportValidate.EmptyValue("TenHang", ref _datarow, ref rowError, ref IsError, sError);

                    sError = (lang == "vi") ? "Tên kho chưa nhập" : "Warehouse is missing";
                    ImportValidate.EmptyValue("TenKho", ref _datarow, ref rowError, ref IsError, sError);

                    sError = (lang == "vi") ? "Ngày chốt chưa nhập" : "Closing date is missing";
                    ImportValidate.EmptyValue("NgayChot", ref _datarow, ref rowError, ref IsError, sError);

                    if (row["MaHang"].ToString().Trim() != "")
                    {
                        DataTable dtdata = CheckMaMatHang(row["MaHang"].ToString(), userinfo.ID_QLLH);
                        if (int.Parse(dtdata.Rows[0]["SOLUONG"].ToString()) == 0)
                        {
                            rowError["MaHang"] = (lang == "vi") ? "Mã mặt hàng không tồn tại trong hệ thống" : "Product code does not exist";
                            IsError = true;
                        }
                    }
                    if (row["MaHang"].ToString().Trim() != "" && rowError["MaHang"].ToString() == "" && rowError["TenKHo"].ToString() == "" && rowError["TenKHo"].ToString() != "")
                    {
                        string MHCode = row["MaHang"].ToString();
                        int ID_KHo = int.Parse(row["ID_Kho"].ToString());
                        var query = from p in lstEmp
                                    where p.MaHang.Contains(MHCode) && p.ID_Kho.Equals(ID_KHo)
                                    select p;
                        if (query != null)
                        {
                            CHECKTRUNG check = new CHECKTRUNG();
                            check.MaHang = MHCode;
                            check.ID_Kho = ID_KHo;
                            lstEmp.Add(check);
                        }
                        else
                        {
                            IsError = true;
                            rowError["MaHang"] = (lang == "vi") ? "Mã mặt hàng đã tồn tại trong file import" : "Product code is already exist in the imported file";
                        }
                    }
                    if (row["NgayChot"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Ngày chốt không đúng định dạng" : "Invalid closing date format";
                        ImportValidate.IsValidDate("NgayChot", ref _datarow, ref rowError, ref IsError, sError, lang);
                    }
                    if (row["SoLuong"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Số lương không đúng định dạng" : "Invalid quantity format";
                        ImportValidate.IsValidNumber("SoLuong", ref _datarow, ref rowError, ref IsError, sError, true, true, lang);
                    }

                    if (row["TenKho"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Tên Kho" : "Warehouse";
                        ImportValidate.IsValidList("TenKho", "ID_Kho", ref _datarow, ref rowError, ref IsError, sError, lang);
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
                        ExportExcel.ExportTemplateToStream("File_Mau_Ton_Kho_Ban_Dau_Error.xls", dtError, null, ref fileStream);
                    }
                    else
                    {
                        ExportExcel.ExportTemplateToStream("File_Mau_Ton_Kho_Ban_Dau_Error_en.xls", dtError, null, ref fileStream);
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
        public class CHECKTRUNG
        {
            public string MaHang { get; set; }
            public int ID_Kho { get; set; }

        }
        public DataTable CheckMaMatHang(string MaMatHang, int ID_QLLH)
        {
            try
            {
                SqlDataHelper helper = new SqlDataHelper();
                string sql = string.Format("select count(A.ID_Hang) SOLUONG from HangHoa A WHERE  A.ID_QLLH = '" + ID_QLLH + "' AND (A.TrangThaiXoa != 1 OR A.TrangThaiXoa IS NOT NULL)  AND upper(a.MaHang)  = upper(\'{0}\')", MaMatHang);
                DataSet ds = helper.ExecuteDataSet(sql);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }
        public DataTable GetMatHangID(string MaMatHang, int ID_QLLH)
        {
            try
            {
                SqlDataHelper helper = new SqlDataHelper();
                string sql = string.Format("select h.ID_Hang, d.TenDonVi from HangHoa h left join DonVi d on d.ID_DonVi = h.ID_DonVi WHERE  h.ID_QLLH = '" + ID_QLLH + "' AND (h.TrangThaiXoa != 1 OR h.TrangThaiXoa IS NOT NULL)  AND  upper(h.MaHang)  = upper(\'{0}\')", MaMatHang);
                DataSet ds = helper.ExecuteDataSet(sql);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return null;
            }
        }

        public class ResultTonDau
        {
            public PhieuTonDauModel PhieuTonDau { get; set; }
            public List<PhieuTonDauChiTietModel> ChiTietMatHangTonDau { get; set; }
            public List<MatHang> DanhSachMatHang { get; set; }
        }

        public class ParamPhieu
        {
            public PhieuTonDauModel PhieuTonDau { get; set; }
            public List<PhieuTonDauChiTietModel> ChiTietMatHangTonDau { get; set; }
        }

    }
}
