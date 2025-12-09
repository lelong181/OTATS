using LSPosMVC.Common;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LSPos_Data.Data;
using System.IO;
using System.Net.Http.Headers;
using System.Web.Http.Cors;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/phieudieuchuyen")]
    [EnableCors(origins: "*", "*", "*")]
    public class PhieuDieuChuyenController : ApiController
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
                    PhieuDieuChuyen_dl pdc = new PhieuDieuChuyen_dl();
                    DataTable dt = pdc.GetDanhSachPhieuDieuChuyen(userinfo.ID_QLLH, from, to);
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
        public HttpResponseMessage getlistdetail([FromUri] int idphieudieuchuyen)
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
                    PhieuDieuChuyen_dl pdc = new PhieuDieuChuyen_dl();
                    DataTable dt = pdc.GetChiTietPhieuById(idphieudieuchuyen);
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
        [Route("getlistmathangtheokho")]
        public HttpResponseMessage getlistmathangtheokho([FromUri] int idkho, int idmathang)
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
                    DataTable dt = mhdl.GetMatHangAll_TheoKho(idkho, idmathang);
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
        [Route("themphieudieuchuyen")]
        public HttpResponseMessage themphieudieuchuyen([FromBody] PhieuDieuChuyenOBJ phieudieuchuyen)
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
                    if (phieudieuchuyen.ID_KhoNhap <= 0 || phieudieuchuyen.ID_KhoXuat <= 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_khonhaphoackhoxuatkhongduocdetrong" });
                    }
                    else
                    {
                        PhieuDieuChuyen_dl pdc = new PhieuDieuChuyen_dl();

                        PhieuDieuChuyenOBJ pdcNew = new PhieuDieuChuyenOBJ();
                        pdcNew.ID_QLLH = userinfo.ID_QLLH;
                        pdcNew.ID_QuanLy = userinfo.ID_QuanLy;
                        pdcNew.DienGiai = phieudieuchuyen.DienGiai;
                        pdcNew.ID_KhoNhap = phieudieuchuyen.ID_KhoNhap;
                        pdcNew.ID_KhoXuat = phieudieuchuyen.ID_KhoXuat;
                        pdcNew.NgayDieuChuyen = DateTime.Now;
                        pdcNew = PhieuDieuChuyen_dl.ThemMoi(pdcNew);
                        if (pdcNew.ID_PhieuDieuChuyen > 0)
                        {
                            if (phieudieuchuyen.ChiTiet.Count() > 0)
                            {
                                foreach (PhieuDieuChuyenChiTietOBJ obj in phieudieuchuyen.ChiTiet)
                                {
                                    obj.ID_PhieuDieuChuyen = pdcNew.ID_PhieuDieuChuyen;
                                    obj.ID_KhoNhap = phieudieuchuyen.ID_KhoNhap;
                                    obj.ID_KhoXuat = phieudieuchuyen.ID_KhoXuat;
                                    PhieuDieuChuyen_dl.ThemChiTiet(obj);
                                    LSPos_Data.Utilities.Log.Info("Chi tiết phiếu điều chuyển: ID_PhieuDieuChuyen:" + obj.ID_PhieuDieuChuyen + "|SoLuong:" + obj.SoLuong + "|ID_HangHoa:" + obj.ID_HangHoa);
                                }

                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_themmoithanhcong" });
                            }
                            else
                            {
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_chuachonmathangdieuchuyenvuilongkiemtralai" });
                            }
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_themmoithatbaivuilongkiemtralaitruongdulieu" });
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
        [Route("excellistphieudieuchuyen")]
        public HttpResponseMessage excellistphieudieuchuyen([FromUri] DateTime from, DateTime to)
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
                    PhieuDieuChuyenData pn = new PhieuDieuChuyenData();
                    DataSet ds = pn.getexcelphieudieuchuyen(userinfo.ID_QLLH, from, to);

                    DataTable dt = new DataTable();
                    dt.TableName = "DATA";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dt = GroupPhieuDieuChuyen(ds);
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
                        filename = "BM028_PhieuDieuChuyen_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("BaoCao_DanhSachPhieuDieuChuyen.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM028_TransferReplenishmentNote_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("BaoCao_DanhSachPhieuDieuChuyen_en.xls", dataSet, null, ref stream);
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
        private DataTable GroupPhieuDieuChuyen(DataSet _dataset)
        {
            DataTable result = _dataset.Tables[0].Copy();
            result.Clear();
            try
            {
                for (int i = 0; i < _dataset.Tables[1].Rows.Count; i++)
                {
                    DataRow _row_master = result.NewRow();
                    _row_master["STT"] = _dataset.Tables[1].Rows[i]["TenPhieu"].ToString();
                    _row_master["IsGroup"] = "1";

                    result.Rows.Add(_row_master);

                    for (int j = 0; j < _dataset.Tables[0].Rows.Count; j++)
                    {
                        if (_dataset.Tables[1].Rows[i]["ID_PhieuDieuChuyen"].ToString() == _dataset.Tables[0].Rows[j]["ID_PhieuDieuChuyen"].ToString())
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
