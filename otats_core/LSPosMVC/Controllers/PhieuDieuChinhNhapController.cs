using LSPosMVC.Common;
using LSPos_Data.Data;
using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;
using System.Web.Http.Cors;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/dieuchinh")]
    [EnableCors(origins: "*", "*", "*")]
    public class PhieuDieuChinhNhapController : ApiController
    {
        [HttpPost]
        [Route("add")]
        public HttpResponseMessage add([FromBody] ParamPhieuDieuChinh paramPhieu)
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
                    PhieuDieuChinhNhapData phieuDieuChinhNhapData = new PhieuDieuChinhNhapData();

                    PhieuDieuChinhNhapKhoModel phieu = paramPhieu.PhieuDieuChinh;
                    phieu.ID_QLLH = userinfo.ID_QLLH;
                    phieu.ID_NhanVien = userinfo.ID_QuanLy;
                    phieu.UpdateBy = userinfo.Username;

                    int idPhieu = phieuDieuChinhNhapData.addPhieuDieuChinh(phieu);

                    string listid = "";
                    List<PhieuDieuChinhNhapKhoChiTietModel> listChiTiet = paramPhieu.ChiTietDieuChinh;
                    foreach (PhieuDieuChinhNhapKhoChiTietModel ct in listChiTiet)
                    {
                        ct.ID_PhieuDieuChinhNhap = idPhieu;
                        ct.UpdateBy = userinfo.Username;

                        int idChiTietPhieu = phieuDieuChinhNhapData.addChiTietDieuChinh(ct);

                        listid = listid + ct.ID_HangHoa.ToString() + ",";
                    }

                    //Đánh dấu xóa các dòng bị xóa (không thuộc listid)
                    bool result = phieuDieuChinhNhapData.deletemarkmulti(idPhieu, listid, userinfo.Username);
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

        [HttpGet]
        [Route("getlistphieunhap")]
        public HttpResponseMessage getlistphieunhap()
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
                    PhieuDieuChinhNhapData data = new PhieuDieuChinhNhapData();
                    List<PhieuNhapModel> li = new List<PhieuNhapModel>();
                    li = data.getlistphieunhap(userinfo.ID_QLLH);
                    response = Request.CreateResponse(HttpStatusCode.OK, li);
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
        [Route("getchitietphieunhap")]
        public HttpResponseMessage getchitietphieunhap([FromUri] int ID_PhieuNhap)
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
                    PhieuDieuChinhNhapData data = new PhieuDieuChinhNhapData();
                    List<PhieuNhapChiTietModel> li = new List<PhieuNhapChiTietModel>();
                    li = data.getchitietphieunhap(ID_PhieuNhap);
                    response = Request.CreateResponse(HttpStatusCode.OK, li);
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
        [Route("getlistphieudieuchinh")]
        public HttpResponseMessage getlistphieudieuchinh()
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
                    PhieuDieuChinhNhapData data = new PhieuDieuChinhNhapData();
                    List<PhieuDieuChinhNhapKhoModel> li = new List<PhieuDieuChinhNhapKhoModel>();
                    li = data.getlistphieudieuchinh(userinfo.ID_QLLH);
                    response = Request.CreateResponse(HttpStatusCode.OK, li);
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
        [Route("getchitietdieuchinh")]
        public HttpResponseMessage getchitietdieuchinh([FromUri] int ID_PhieuDieuChinhNhapKho)
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
                    PhieuDieuChinhNhapData data = new PhieuDieuChinhNhapData();
                    List<PhieuDieuChinhNhapKhoChiTietModel> li = new List<PhieuDieuChinhNhapKhoChiTietModel>();
                    li = data.getchitietdieuchinh(ID_PhieuDieuChinhNhapKho);
                    response = Request.CreateResponse(HttpStatusCode.OK, li);
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
        [Route("baocaodieuchinh")]
        public HttpResponseMessage baocaodieuchinh([FromUri] int ID_Kho, DateTime from, DateTime to)
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
                    PhieuDieuChinhNhapData data = new PhieuDieuChinhNhapData();
                    
                    DataTable li = data.baocaodieuchinh(userinfo.ID_QLLH, userinfo.ID_QuanLy, ID_Kho, from, to);
                    response = Request.CreateResponse(HttpStatusCode.OK, li);
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
        [Route("excelbaocaodieuchinh")]
        public HttpResponseMessage excelbaocaodieuchinh([FromUri] int ID_Kho, DateTime from, DateTime to)
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
                    PhieuDieuChinhNhapData data = new PhieuDieuChinhNhapData();

                    DataSet ds = data.excelbaocaodieuchinh(userinfo.ID_QLLH, userinfo.ID_QuanLy, ID_Kho, from, to);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "DATA";

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
                        dataSet.Tables.Add(ds.Tables[0].Copy());
                        dataSet.Tables.Add(dt1.Copy());
                        dataSet.Tables.Add(dt2.Copy());

                        string filename = "";
                        var stream = new MemoryStream();
                        ExportExcel excel = new ExportExcel();
                        if (lang == "vi")
                        {
                            filename = "BM032_BaoCaoDieuChinhNhapKho_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BaoCaoDieuChinh.xls", dataSet, null, ref stream);
                        }
                        else
                        {
                            filename = "BM032_StockReplenishmentReport_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                            excel.ExportTemplateToStreamGird("Report_BaoCaoDieuChinh_en.xls", dataSet, null, ref stream);
                        }

                        response.Content = new ByteArrayContent(stream.ToArray());
                        response.Content.Headers.Add("x-filename", filename);
                        response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = filename;
                        response.StatusCode = HttpStatusCode.OK;
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

        public class ParamPhieuDieuChinh
        {
            public PhieuDieuChinhNhapKhoModel PhieuDieuChinh { get; set; }
            public List<PhieuDieuChinhNhapKhoChiTietModel> ChiTietDieuChinh { get; set; }
        }
    }
}
