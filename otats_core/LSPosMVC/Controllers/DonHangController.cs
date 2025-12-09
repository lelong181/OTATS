using LSPosMVC.Common;
using LSPos_Data.Models;
using LSPos_Data.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Text;
using Ksmart_DataSon.DataAccess;
using Ksmart_DataSon.Models;
using System.Collections;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System.Globalization;
using static LSPos_Data.Data.DonHangData;
using System.Net.Http.Headers;
using System.IO;
using BusinessLayer.Repository;
using BusinessLayer.Model.API;
using BusinessLayer.Model;
using Kendo.Mvc.Extensions;
using BusinessLayer.Model.Sell;
using Ticket;
using LSPosMVC.App_Start;
using RazorEngine;
using RazorEngine.Templating;
using LSPos_Data.DataAccess;
using LSPosMVC.Models.Models_Filter;
using System.Threading.Tasks;
using Model.ResponseModel;
using NPOI.SS.Formula.Functions;
using System.Configuration;
using System.Net.Sockets;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static LSPosMVC.Controllers.MapsController;
using Swashbuckle.Swagger;
using Business.Model;
using System.Security.Cryptography.Xml;
using Ticket.Utils;
using System.Web.Script.Serialization;
using NPOI.OpenXmlFormats.Vml;
using static LSPosMVC.Common.VnPayUtils;
using System.Security.Policy;
using LSPosMVC.Common.Paycollect;
using Newtonsoft.Json.Linq;
using LSPosMVC.Models;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/donhang")]
    public class DonHangController : ApiController
    {

        private SellRepository _sellRepository;
        private ManagerProfileRepository _managerProfileRepository;
        private CommonRepository _commonRepository;
        private BookingRepository _bookingRepository;
        private InvoiceRepository _invoiceRepository;
        public DonHangController()
        {
            _sellRepository = new SellRepository();
            _managerProfileRepository = new ManagerProfileRepository();
            _commonRepository = new CommonRepository();
            _bookingRepository = new BookingRepository();
            _invoiceRepository = new InvoiceRepository();
        }

        [HttpPost]
        [Route("getlistdonghang")]
        public HttpResponseMessage getlistdonghang([FromBody] TieuChiLoc tieuchi)
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
                    if (tieuchi.ListIDNhom == null)
                        tieuchi.ListIDNhom = "";
                    if (tieuchi.from.Year <= 1900)
                        tieuchi.from = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
                    if (tieuchi.to.Year <= 1900)
                        tieuchi.to = tieuchi.from.AddDays(1).AddSeconds(-1);

                    DonHangData baoCao_DB = new DonHangData();
                    DataTable list = baoCao_DB.GetListDonHang(userinfo.ID_QLLH, userinfo.IsAdmin ? userinfo.ID_QuanLy : 0, tieuchi.from, tieuchi.to, tieuchi.ttgh, tieuchi.tttt
                        , tieuchi.ttht, tieuchi.idKhachHang, tieuchi.IdNhanVien, tieuchi.IdMatHang, tieuchi.ListIDNhom, tieuchi.donhangtaidiem, tieuchi.trangthaixem);

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

        [HttpPost]
        [Route("getlistdonhangbyhdv")]
        public HttpResponseMessage getlistdonhangbyhdv([FromBody] TieuChiLoc tieuchi)
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
                    if (tieuchi.from.Year <= 1900)
                        tieuchi.from = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
                    if (tieuchi.to.Year <= 1900)
                        tieuchi.to = tieuchi.from.AddDays(1).AddSeconds(-1);
                    DonHangData baoCao_DB = new DonHangData();
                    DataTable list = baoCao_DB.GetListDonHangByHDV(userinfo.ID_QuanLy, tieuchi.from, tieuchi.to);

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

        [HttpGet]
        [Route("getdsdonhang")]
        public DataSourceResult getdsDonHangPagingServer(HttpRequestMessage requestMessage)
        {
            DonHangData donHangData = new DonHangData();
            DataSourceResult result = new DataSourceResult();
            try
            {
                RequestGridParam param = JsonConvert.DeserializeObject<RequestGridParam>(
                    requestMessage.RequestUri.ParseQueryString().GetKey(0)
                );
                TieuChiLoc tieuchi = param.tieuchiloc;

                FilterGridDonHang filter = new FilterGridDonHang();
                if (param.request.Filter != null)
                {
                    foreach (Filter f in param.request.Filter.Filters)
                    {
                        switch (f.Field)
                        {
                            case "maDonHang":
                                filter.MaDonHang = f.Value.ToString();
                                break;
                            case "tenKhachHang":
                                filter.TenKhachHang = f.Value.ToString();
                                break;
                            case "ngayLap":
                                try
                                {
                                    filter.NgayLap = Convert.ToDateTime(DateTime.ParseExact(f.Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture));
                                }
                                catch { }
                                break;
                            case "tenNhanVien":
                                filter.TenNhanVien = f.Value.ToString();
                                break;
                            case "maKH":
                                filter.MaKhachHang = f.Value.ToString();
                                break;
                            case "dienThoai":
                                filter.DienThoai = f.Value.ToString();
                                break;
                            case "diaChi":
                                filter.DiaChi = f.Value.ToString();
                                break;
                            case "diaChiTao":
                                filter.ViTriTao = f.Value.ToString();
                                break;
                            case "ghiChu":
                                filter.GhiChu = f.Value.ToString();
                                break;
                            case "isProcess_Name":
                                filter.TrangThaiDonHang = -1;
                                if (f.Value.ToString() == "Chưa hoàn tất")
                                    filter.TrangThaiDonHang = 0;
                                if (f.Value.ToString() == "Đã hoàn tất")
                                    filter.TrangThaiDonHang = 1;
                                if (f.Value.ToString() == "Đã hủy")
                                    filter.TrangThaiDonHang = 2;
                                break;
                            case "tenTrangThaiGiaoHang":
                                filter.TrangThaiGiaoHang = donHangData.GetTrangThaiGiaoHang_ByName(f.Value.ToString());
                                break;
                            case "tongTien":
                                filter.TongTien = Convert.ToSingle(f.Value.ToString());
                                break;
                            case "tienDaThanhToan":
                                filter.DaThanhToan = Convert.ToSingle(f.Value.ToString());
                                break;
                            case "conLai":
                                filter.ConLai = Convert.ToSingle(f.Value.ToString());
                                break;
                        }
                    }
                }

                try
                {
                    if (filter.NgayLap.Year < 1900)
                    {
                        filter.NgayLap = new DateTime(1900, 1, 1);
                    }
                }
                catch
                {
                    filter.NgayLap = new DateTime(1900, 1, 1);
                }
                if (filter.MaDonHang == null)
                    filter.MaDonHang = "";
                if (filter.TenKhachHang == null)
                    filter.TenKhachHang = "";
                if (filter.TenNhanVien == null)
                    filter.TenNhanVien = "";
                if (filter.MaKhachHang == null)
                    filter.MaKhachHang = "";
                if (filter.DienThoai == null)
                    filter.DienThoai = "";
                if (filter.DiaChi == null)
                    filter.DiaChi = "";
                if (filter.ViTriTao == null)
                    filter.ViTriTao = "";
                if (filter.GhiChu == null)
                    filter.GhiChu = "";

                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo != null)
                {
                    if (tieuchi.ListIDNhom == null)
                        tieuchi.ListIDNhom = "";
                    if (tieuchi.from.Year <= 1900)
                        tieuchi.from = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
                    if (tieuchi.to.Year <= 1900)
                        tieuchi.to = tieuchi.from.AddDays(1).AddSeconds(-1);

                    DonHang_dl dh_dl = new DonHang_dl();


                    int tongso = 0;

                    double TongTien = 0;
                    double TienDaThanhToan = 0;
                    double ConLai = 0;

                    List<DonHang> dsdh = null;
                    if (tieuchi.trangthaixem == 1)
                    {

                        dsdh = dh_dl.GetDSDonHangAll_ChuaXem(userinfo.ID_QLLH, userinfo.ID_QuanLy, tieuchi.from, tieuchi.to, tieuchi.ttgh, tieuchi.tttt, tieuchi.ttht);
                    }
                    else if (tieuchi.donhangtaidiem != 0)
                    {
                        if (tieuchi.donhangtaidiem == 1)
                        {
                            dsdh = dh_dl.GetDSDonHangAll_TaiDiem(tieuchi.IdNhanVien, tieuchi.from, tieuchi.to);
                        }
                        else
                            dsdh = dh_dl.GetDSDonHangAll_KhongTaiDiem(tieuchi.IdNhanVien, tieuchi.from, tieuchi.to);
                    }
                    else
                    {
                        ResultPaging Result = null;

                        if (userinfo.CapDo == -1)//Nhan vien
                            Result = donHangData.GetDSDonHangAll(userinfo.ID_QLLH, 0, tieuchi.from, tieuchi.to, tieuchi.ttgh, tieuchi.tttt, tieuchi.ttht, tieuchi.idKhachHang, userinfo.ID_QuanLy, tieuchi.IdMatHang, tieuchi.ListIDNhom,
                         param.request.Skip, param.request.Take, filter, ref tongso);
                        else
                            Result = donHangData.GetDSDonHangAll(userinfo.ID_QLLH, userinfo.ID_QuanLy, tieuchi.from, tieuchi.to, tieuchi.ttgh, tieuchi.tttt, tieuchi.ttht, tieuchi.idKhachHang, tieuchi.IdNhanVien, tieuchi.IdMatHang, tieuchi.ListIDNhom,
                          param.request.Skip, param.request.Take, filter, ref tongso);
                        dsdh = Result.li;
                        TongTien = Result.TongTien;
                        TienDaThanhToan = Result.TienDaThanhToan;
                        ConLai = Result.ConLai;
                    }

                    List<DonHangObj> list = new List<DonHangObj>();

                    list = RenderGridViewFromDSDH(dsdh, tieuchi);

                    if (param.request.Sort != null && param.request.Sort.Count() > 0)
                    {
                        string sortField = param.request.Sort.First().Field;
                        switch (sortField)
                        {
                            case "maDonHang":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    list = list.OrderBy(x => x.MaThamChieu).ToList();
                                }
                                else
                                {
                                    list = list.OrderByDescending(x => x.MaThamChieu).ToList();
                                }
                                break;
                            case "tenKhachHang":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    list = list.OrderBy(x => x.TenKhachHang).ToList();
                                }
                                else
                                {
                                    list = list.OrderByDescending(x => x.TenKhachHang).ToList();
                                }
                                break;
                            case "ngayLap":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    list = list.OrderBy(x => x.NgayLap).ToList();
                                }
                                else
                                {
                                    list = list.OrderByDescending(x => x.NgayLap).ToList();
                                }
                                break;
                            case "tenNhanVien":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    list = list.OrderBy(x => x.TenNhanVien).ToList();
                                }
                                else
                                {
                                    list = list.OrderByDescending(x => x.TenNhanVien).ToList();
                                }
                                break;
                            case "maKH":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    list = list.OrderBy(x => x.MaKH).ToList();
                                }
                                else
                                {
                                    list = list.OrderByDescending(x => x.MaKH).ToList();
                                }
                                break;
                            case "dienThoai":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    list = list.OrderBy(x => x.DienThoai).ToList();
                                }
                                else
                                {
                                    list = list.OrderByDescending(x => x.DienThoai).ToList();
                                }
                                break;
                            case "diaChi":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    list = list.OrderBy(x => x.DiaChi).ToList();
                                }
                                else
                                {
                                    list = list.OrderByDescending(x => x.DiaChi).ToList();
                                }
                                break;
                            case "diaChiTao":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    list = list.OrderBy(x => x.DiaChiTao).ToList();
                                }
                                else
                                {
                                    list = list.OrderByDescending(x => x.DiaChiTao).ToList();
                                }
                                break;
                            case "ghiChu":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    list = list.OrderBy(x => x.GhiChu).ToList();
                                }
                                else
                                {
                                    list = list.OrderByDescending(x => x.GhiChu).ToList();
                                }
                                break;
                            case "isProcess_Name":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    list = list.OrderBy(x => x.isProcess_Name).ToList();
                                }
                                else
                                {
                                    list = list.OrderByDescending(x => x.isProcess_Name).ToList();
                                }
                                break;
                            case "tenTrangThaiGiaoHang":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    list = list.OrderBy(x => x.TenTrangThaiGiaoHang).ToList();
                                }
                                else
                                {
                                    list = list.OrderByDescending(x => x.TenTrangThaiGiaoHang).ToList();
                                }
                                break;
                            case "tongTien":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    list = list.OrderBy(x => x.TongTien).ToList();
                                }
                                else
                                {
                                    list = list.OrderByDescending(x => x.TongTien).ToList();
                                }
                                break;
                            case "tienDaThanhToan":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    list = list.OrderBy(x => x.TienDaThanhToan).ToList();
                                }
                                else
                                {
                                    list = list.OrderByDescending(x => x.TienDaThanhToan).ToList();
                                }
                                break;
                            case "conLai":
                                if (param.request.Sort.First().Dir == "asc")
                                {
                                    list = list.OrderBy(x => x.ConLai).ToList();
                                }
                                else
                                {
                                    list = list.OrderByDescending(x => x.ConLai).ToList();
                                }
                                break;
                        }
                    }
                    else
                    {
                        list = list.OrderByDescending(x => x.ID_DonHang).ToList();
                    }


                    Aggregates aggregates = new Aggregates();
                    aggregates.tienDaThanhToan = new AggregatesValue(TienDaThanhToan);
                    aggregates.tongTien = new AggregatesValue(TongTien);
                    aggregates.conLai = new AggregatesValue(ConLai);

                    result.Data = list;
                    result.Total = tongso;
                    result.Aggregates = aggregates;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return result;
        }

        [HttpGet]
        [Route("dsdonhang")]
        public HttpResponseMessage get([FromUri] TieuChiLoc tieuchi)
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
                    if (tieuchi.ListIDNhom == null)
                        tieuchi.ListIDNhom = "";
                    if (tieuchi.from.Year <= 1900)
                        tieuchi.from = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
                    if (tieuchi.to.Year <= 1900)
                        tieuchi.to = tieuchi.from.AddDays(1).AddSeconds(-1);

                    DonHang_dl dh_dl = new DonHang_dl();

                    List<DonHang> dsdh = null;
                    if (tieuchi.trangthaixem == 1)
                    {
                        dsdh = dh_dl.GetDSDonHangAll_ChuaXem(userinfo.ID_QLLH, userinfo.ID_QuanLy, tieuchi.from, tieuchi.to, tieuchi.ttgh, tieuchi.tttt, tieuchi.ttht);
                    }
                    else if (tieuchi.donhangtaidiem != 0)
                    {
                        if (tieuchi.donhangtaidiem == 1)
                        {
                            dsdh = dh_dl.GetDSDonHangAll_TaiDiem(tieuchi.IdNhanVien, tieuchi.from, tieuchi.to);
                        }
                        else
                            dsdh = dh_dl.GetDSDonHangAll_KhongTaiDiem(tieuchi.IdNhanVien, tieuchi.from, tieuchi.to);
                    }
                    else
                        dsdh = dh_dl.GetDSDonHangAll(userinfo.ID_QLLH, userinfo.ID_QuanLy, tieuchi.from, tieuchi.to, tieuchi.ttgh, tieuchi.tttt, tieuchi.ttht, tieuchi.idKhachHang, tieuchi.IdNhanVien, tieuchi.IdMatHang, tieuchi.ListIDNhom);

                    List<DonHangObj> list = new List<DonHangObj>();

                    list = RenderGridViewFromDSDH(dsdh, tieuchi);

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

        [HttpPost]
        [Route("getall")]
        public HttpResponseMessage getall([FromBody] TieuChiLoc tieuchi)
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
                    if (tieuchi.ListIDNhom == null)
                        tieuchi.ListIDNhom = "";
                    if (tieuchi.from.Year <= 1900)
                        tieuchi.from = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
                    if (tieuchi.to.Year <= 1900)
                        tieuchi.to = tieuchi.from.AddDays(1).AddSeconds(-1);

                    DonHang_dl dh_dl = new DonHang_dl();

                    List<DonHang> dsdh = null;
                    if (tieuchi.trangthaixem == 1)
                    {
                        dsdh = dh_dl.GetDSDonHangAll_ChuaXem(userinfo.ID_QLLH, userinfo.ID_QuanLy, tieuchi.from, tieuchi.to, tieuchi.ttgh, tieuchi.tttt, tieuchi.ttht);
                    }
                    else if (tieuchi.donhangtaidiem != 0)
                    {
                        if (tieuchi.donhangtaidiem == 1)
                        {
                            dsdh = dh_dl.GetDSDonHangAll_TaiDiem(tieuchi.IdNhanVien, tieuchi.from, tieuchi.to);
                        }
                        else
                            dsdh = dh_dl.GetDSDonHangAll_KhongTaiDiem(tieuchi.IdNhanVien, tieuchi.from, tieuchi.to);
                    }
                    else
                        dsdh = dh_dl.GetDSDonHangAll(userinfo.ID_QLLH, userinfo.ID_QuanLy, tieuchi.from, tieuchi.to, tieuchi.ttgh, tieuchi.tttt, tieuchi.ttht, tieuchi.idKhachHang, tieuchi.IdNhanVien, tieuchi.IdMatHang, tieuchi.ListIDNhom);

                    List<DonHangObj> list = new List<DonHangObj>();

                    list = RenderGridViewFromDSDH(dsdh, tieuchi);

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

        private List<DonHangObj> RenderGridViewFromDSDH(List<DonHang> dsdh, TieuChiLoc tieuchi)
        {
            List<DonHangObj> list = new List<DonHangObj>();


            try
            {
                DonHang_dl dh_dl = new DonHang_dl();
                List<TrangThaiGiaoHang> dsttgh = dh_dl.GetDSTrangThaiGiaoHang();

                int i = 0;
                int trangthaixem = tieuchi.trangthaixem;
                foreach (DonHang dh in dsdh)
                {
                    try
                    {
                        if (tieuchi.IdNhanVien == 0 || tieuchi.IdNhanVien == dh.ID_NhanVien)
                        {
                            i++;
                            int index = 0;
                            for (int y = 0; y < dsttgh.Count; y++)
                            {
                                if (dh.ID_TrangThaiGiaoHang == dsttgh[y].ID_TrangThaiGiaoHang)
                                {
                                    index = y;
                                    break;
                                }
                            }
                            string processDate = "";
                            if (dh.ProcessDate > new DateTime(2000, 1, 1))
                            {
                                processDate = dh.ProcessDate.ToString("dd/MM/yyyy");
                            }
                            string anhdaidien = "";
                            if (dh.danhsachanh != null)
                            {
                                foreach (AlbumOBJ album in dh.danhsachanh)
                                {
                                    if (!string.IsNullOrWhiteSpace(album.hinhdaidien))
                                    {
                                        anhdaidien = Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]) + "/" + album.hinhdaidien;
                                    }
                                }
                            }


                            if (trangthaixem == -1)
                            {
                                DonHangObj obj = new DonHangObj();
                                obj.ID_DonHang = dh.ID_DonHang;
                                obj.Stt = i.ToString();
                                obj.NgayLap = dh.NgayTao.ToString("dd/MM/yyyy");
                                obj.TenNhanVien = dh.TenNhanVien;
                                obj.TenKhachHang = dh.TenKhachHang;
                                obj.DienThoai = dh.DienThoai;
                                obj.DiaChi = dh.DiaChi;
                                obj.TongTien = dh.TongTien;
                                obj.GhiChu = dh.GhiChu;
                                obj.TrangThaiGiaoHang = index;
                                obj.MaThamChieu = dh.MaThamChieu;
                                obj.TienDaThanhToan = dh.TienDaThanhToan;
                                obj.ConLai = ((float)dh.TongTien - dh.TienDaThanhToan <= 0) ? 0 : (dh.TongTien - dh.TienDaThanhToan);
                                obj.IsProcess = dh.isProcess;
                                obj.ID_TTTT = dh.ID_TrangThaiThanhToan;
                                obj.ID_TTGH = dh.ID_TrangThaiGiaoHang;
                                obj.ProcessDate = processDate;
                                obj.TenTrangThaiGiaoHang = dh.TenTrangThaiGiaoHang;
                                obj.ID_NhanVien = dh.ID_NhanVien;
                                obj.isProcess_Name = dh.isProcess_Name;
                                obj.MaKH = dh.MaKH;
                                obj.ToaDoKhachHang = dh.ToaDoKhachHang;
                                obj.KinhDo = dh.KinhDo.ToString();
                                obj.ViDo = dh.ViDo.ToString();
                                obj.DiaChiTao = dh.DiaChiTao;
                                obj.DiaChiXuatHoaDon = dh.DiaChiXuatHoaDon;
                                obj.AnhDaiDien = anhdaidien;
                                obj.ID_TTDH = dh.ID_TrangThaiDongHang;
                                obj.TenTrangThaiDonHang = dh.TenTrangThaiDongHang;
                                list.Add(obj);
                            }
                            else
                            {
                                if (dh.DaXem == trangthaixem)
                                {
                                    DonHangObj obj = new DonHangObj();
                                    obj.ID_DonHang = dh.ID_DonHang;
                                    obj.Stt = i.ToString();
                                    obj.NgayLap = dh.NgayTao.ToString("dd/MM/yyyy");
                                    obj.TenNhanVien = dh.TenNhanVien;
                                    obj.TenKhachHang = dh.TenKhachHang;
                                    obj.DienThoai = dh.DienThoai;
                                    obj.DiaChi = dh.DiaChi;
                                    obj.TongTien = dh.TongTien;
                                    obj.GhiChu = dh.GhiChu;
                                    obj.TrangThaiGiaoHang = index;
                                    obj.MaThamChieu = dh.MaThamChieu;
                                    obj.TienDaThanhToan = dh.TienDaThanhToan;
                                    obj.ConLai = ((float)dh.TongTien - dh.TienDaThanhToan <= 0) ? 0 : (dh.TongTien - dh.TienDaThanhToan);
                                    obj.IsProcess = dh.isProcess;
                                    obj.ID_TTTT = dh.ID_TrangThaiThanhToan;
                                    obj.ID_TTGH = dh.ID_TrangThaiGiaoHang;
                                    obj.ProcessDate = processDate;
                                    obj.TenTrangThaiGiaoHang = dh.TenTrangThaiGiaoHang;
                                    obj.ID_NhanVien = dh.ID_NhanVien;
                                    obj.isProcess_Name = dh.isProcess_Name;
                                    obj.MaKH = dh.MaKH;
                                    obj.ToaDoKhachHang = dh.ToaDoKhachHang;
                                    obj.KinhDo = dh.KinhDo.ToString();
                                    obj.ViDo = dh.ViDo.ToString();
                                    obj.DiaChiTao = dh.DiaChiTao;
                                    obj.DiaChiXuatHoaDon = dh.DiaChiXuatHoaDon;
                                    obj.AnhDaiDien = anhdaidien;
                                    obj.ID_TTDH = dh.ID_TrangThaiDongHang;
                                    obj.TenTrangThaiDonHang = dh.TenTrangThaiDongHang;
                                    list.Add(obj);
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }
                }

            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return list;

        }

        public class DonHangObj
        {
            public int ID_DonHang { get; set; }
            public string Stt { get; set; }
            public string NgayLap { get; set; }
            public string TenNhanVien { get; set; }
            public string TenKhachHang { get; set; }
            public string DienThoai { get; set; }
            public string DiaChi { get; set; }
            public double TongTien { get; set; }
            public string GhiChu { get; set; }
            public int TrangThaiGiaoHang { get; set; }
            public string MaThamChieu { get; set; }
            public double TienDaThanhToan { get; set; }
            public double ConLai { get; set; }
            public int IsProcess { get; set; }
            public int ID_TTTT { get; set; }
            public int ID_TTGH { get; set; }
            public string ProcessDate { get; set; }
            public string TenTrangThaiGiaoHang { get; set; }
            public int ID_NhanVien { get; set; }
            public string TenTrangThaiDonHang { get; set; }
            public int ID_TTDH { get; set; }

            public string isProcess_Name { get; set; }
            public string MaKH { get; set; }
            public string ToaDoKhachHang { get; set; }
            public string KinhDo { get; set; }
            public string ViDo { get; set; }
            public string DiaChiTao { get; set; }
            public string DiaChiXuatHoaDon { get; set; }
            public string AnhDaiDien { get; set; }
        }

        #region TrangThai
        public class TrangThai
        {
            public TrangThai(int id, string value, string color)
            {
                this.ID = id;
                this.Value = value;
                this.Color = color;
            }
            public int ID { get; set; }
            public string Value { get; set; }
            public string Color { get; set; }
        }

        [HttpGet]
        [Route("getalltttt")]
        public HttpResponseMessage gettttt([FromUri] string lang)
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
                    DonHangData donHangData = new DonHangData();

                    DataTable dt = donHangData.GetDSTrangThaiThanhToan(userinfo.ID_QuanLy, lang);
                    List<TrangThai> list = new List<TrangThai>();
                    //list.Add(new TrangThai(0, "Tất cả", "#"));

                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(
                            new TrangThai(int.Parse(dr["ID_TrangThaiThanhToan"].ToString()),
                                dr["TenTrangThaiThanhToan"].ToString().Trim(),
                                dr["Color"].ToString().Trim())
                            );
                    }

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

        [HttpGet]
        [Route("getallttgh")]
        public HttpResponseMessage getttgh([FromUri] string lang)
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
                    DonHangData donHangData = new DonHangData();

                    DataTable dt = donHangData.GetDSTrangThaiGiaoHang(userinfo.ID_QuanLy, lang);
                    List<TrangThai> list = new List<TrangThai>();
                    //list.Add(new TrangThai(0, "Tất cả", "#"));

                    foreach (DataRow dr in dt.Rows)
                    {
                        list.Add(
                            new TrangThai(int.Parse(dr["ID_TrangThaiGiaoHang"].ToString()),
                                dr["TenTrangThaiGiaoHang"].ToString().Trim(),
                                dr["Color"].ToString().Trim())
                            );
                    }

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

        [HttpGet]
        [Route("getallttht")]
        public HttpResponseMessage getttht([FromUri] string lang)
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
                    List<TrangThai> list = new List<TrangThai>();

                    if (lang == "vi")
                    {
                        //list.Add(new TrangThai(-1, "Tất cả", "#"));
                        list.Add(new TrangThai(0, "Chưa hoàn tất", "#fdcb6e"));
                        list.Add(new TrangThai(1, "Đã hoàn tất", "#0984e3"));
                        list.Add(new TrangThai(2, "Đã hủy", "#d63031"));
                    }
                    else
                    {
                        list.Add(new TrangThai(0, "Unfinished", "#fdcb6e"));
                        list.Add(new TrangThai(1, "Finished", "#0984e3"));
                        list.Add(new TrangThai(2, "Canceled", "#d63031"));
                    }


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
        [HttpGet]
        [Route("getallttx")]
        public HttpResponseMessage getttx([FromUri] string lang)
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
                    List<TrangThai> list = new List<TrangThai>();

                    if (lang == "vi")
                    {
                        //list.Add(new TrangThai(-1, "Tất cả", "#"));
                        list.Add(new TrangThai(1, "Đã xem", "#0984e3"));
                        list.Add(new TrangThai(0, "Chưa xem", "#ff7675"));
                    }
                    else
                    {
                        //list.Add(new TrangThai(-1, "Tất cả", "#"));
                        list.Add(new TrangThai(1, "Seen", "#0984e3"));
                        list.Add(new TrangThai(0, "Un seen", "#ff7675"));
                    }


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
        #endregion

        [HttpGet]
        [Route("getnvdacapquyen")]
        public HttpResponseMessage getnvdacapquyen([FromUri] int id_donhang)
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

                    List<DonHangPhanQuyen> list = new List<DonHangPhanQuyen>();

                    DonHang_dl kh_dl = new DonHang_dl();
                    DataTable dskhDataSrc = kh_dl.GetNhanVienDaCapQuyen(id_donhang);

                    foreach (DataRow dr in dskhDataSrc.Rows)
                    {
                        DonHangPhanQuyen donHangPhanQuyen = new DonHangPhanQuyen();
                        donHangPhanQuyen.ID = int.Parse(dr["ID_ChucNang"].ToString());
                        donHangPhanQuyen.ID_NhanVien = int.Parse(dr["ID_NhanVien"].ToString());
                        donHangPhanQuyen.ID_DonHang = int.Parse(dr["ID_DonHang"].ToString());
                        donHangPhanQuyen.ID_Quyen = int.Parse(dr["ID_Quyen"].ToString());
                        donHangPhanQuyen.ID_QuanLy = int.Parse(dr["ID_QuanLy"].ToString());
                        try
                        {
                            donHangPhanQuyen.NgayGanQuyen = DateTime.Parse(dr["NgayGanQuyen"].ToString());
                        }
                        catch { }

                        donHangPhanQuyen.TenNhanVien = dr["TenNhanVien"].ToString();

                        list.Add(donHangPhanQuyen);
                    }

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

        public class DonHangPhanQuyen
        {
            public int ID { get; set; }
            public int ID_NhanVien { get; set; }
            public int ID_DonHang { get; set; }
            public int ID_Quyen { get; set; }
            public int ID_QuanLy { get; set; }
            public DateTime NgayGanQuyen { get; set; }
            public string TenNhanVien { get; set; }
        }

        [HttpGet]
        [Route("huydonhang")]
        public HttpResponseMessage HuyDonHang([FromUri] string id_donhang, string lydo)
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
                    DonHangData donHangData = new DonHangData();

                    bool resultHoanTien = donHangData.BienDongSoDuNhom_HuyDonHang(userinfo.ID_QuanLy, int.Parse(id_donhang));
                    bool result = donHangData.HuyDonHang(lydo, userinfo.ID_QuanLy, id_donhang);
                    if (result)
                        response = Request.CreateResponse(HttpStatusCode.OK, "Hủy đơn hàng thành công");
                    else
                        response = Request.CreateResponse(HttpStatusCode.NotAcceptable, "Hủy đơn hàng không thành công");
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
        [Route("giaHanVe")]
        public HttpResponseMessage GiaHanVe([FromUri] long id_donhang, DateTime ngay)
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
                    DonHangData donHangData = new DonHangData();

                    bool result = donHangData.GiaHanVe(id_donhang, ngay);
                    if (result)
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "Gia hạn vé thành công" });
                    else
                        response = Request.CreateResponse(HttpStatusCode.NotAcceptable, "Gia hạn vé không thành công");
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        #region ThemMoiDonHangTrenWeb

        /**
         * danh sách trạng thái dơn hàng
         * @param: 
         * @result: response satus 201: tạo mới thành công, 401 Unauthorized, 500 Internal Server Error
         * @author: VTLan
         * **/
        [HttpGet]
        [Route("danhSachTrangThaiDonHang")]
        public HttpResponseMessage getListStatusInvoices()
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
                    TrangThaiDonHangDB ttdh_dl = new TrangThaiDonHangDB();
                    List<TrangThaiDonHangOBJ> dstt = ttdh_dl.LayDanhSachTrangThaiDonHang(userinfo.ID_QLLH);
                    foreach (TrangThaiDonHangOBJ t in dstt)
                    {
                        t.MauTrangThai = "#" + t.MauTrangThai;
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, dstt);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
        /**
         * danh sách danh mục khuyến mại theo loại
         * @param: 
         * @result: response satus 201: tạo mới thành công, 401 Unauthorized, 500 Internal Server Error
         * @author: VTLan
         * **/
        [HttpGet]
        [Route("getListKhuyenMai")]
        public HttpResponseMessage getListDiscount([FromUri] int loai)
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
                    KhuyenMai_dl kmdl = new KhuyenMai_dl();
                    List<KhuyenMai> dskm = kmdl.GetDSKhuyenmaiConHieuLucTheoLoai(userinfo.ID_QLLH, loai);

                    response = Request.CreateResponse(HttpStatusCode.OK, dskm);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
        /**
         * danh sách danh mục theo user
         * @param: 
         * @result: response satus 201: tạo mới thành công, 401 Unauthorized, 500 Internal Server Error
         * @author: VTLan
         * **/
        [HttpGet]
        [Route("getSanPham")]
        public HttpResponseMessage getCatalogs()
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
                    List<MatHang> dsmh = new List<MatHang>();
                    dsmh = mh_dl.getDS_HangHoa_ByIdDanhMuc(-1, userinfo.ID_QLLH);
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
        [Route("getsite")]
        public HttpResponseMessage getsite([FromUri] string sitecode)
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
                    SiteDAO sdao = new SiteDAO();
                    response = Request.CreateResponse(HttpStatusCode.OK, sdao.GetSite(sitecode));
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
        [Route("getallsite")]
        public HttpResponseMessage getallsite()
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
                    SiteDAO sdao = new SiteDAO();
                    response = Request.CreateResponse(HttpStatusCode.OK, sdao.GetAllSite());
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        /**
         * getDanhSachThanhToan
         * @param: 
         * @result: response satus 201: tạo mới thành công, 401 Unauthorized, 500 Internal Server Error
         * @author: VTLan
         * **/
        [HttpGet]
        [Route("getDanhSachThanhToan")]
        public HttpResponseMessage getDanhSachThanhToan([FromUri] int idDonHang)
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
                    DonHang_dl dh_dl = new DonHang_dl();
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    DataTable dtLichSu = dh_dl.LichSuThanhToan(idDonHang);
                    response = Request.CreateResponse(HttpStatusCode.OK, dtLichSu);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
        /**
         * Thanh toan don hang
         * @param: 
         * @result: response satus 201: tạo mới thành công, 401 Unauthorized, 500 Internal Server Error
         * @author: VTLan
         * **/
        public class AppThanhToanOBJ
        {
            public AppThanhToanOBJ() { }

            public bool status { get; set; }

            public string msg { get; set; }

        }

        [HttpPost]
        [Route("thanhToanDonHang")]
        public async Task<HttpResponseMessage> thanhToanDonHang([FromBody] paramTToan param)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            AppThanhToanOBJ OBJ = new AppThanhToanOBJ();
            OBJ.status = false;
            OBJ.msg = Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG;
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
                    string type = param.type;
                    int iddonhang = param.iddonhang;
                    int idnhanvien = param.idnhanvien;
                    long idquanly = param.idquanly;
                    string ghichu = param.ghichu;
                    double tien = param.tien;
                    string url = param.url;
                    DonHangData dhData = new DonHangData();
                    DonHangModels donhang = dhData.LayDonHang(param.iddonhang);
                    KhachHang kh = new KhachHang_dl().GetKhachHangID(donhang.idcuahang);
                    if (donhang.trangthaithanhtoan == 4 && donhang.tongtien > 0)
                    {
                        //don hang da xu ly xong
                        OBJ.status = false;
                        OBJ.msg = Config.DON_HANG_DA_XU_LY;
                    }
                    if (type.Equals("save"))
                    {

                        if (dhData.ThanhToanDonHang1phan(iddonhang, tien, idnhanvien, ghichu, idquanly, url, 2))
                        {
                            OBJ.status = true;
                            OBJ.msg = Config.THANH_CONG;
                        }
                    }
                    else if (type.Equals("thanhtoan"))
                    {
                        if (dhData.ThanhToanDonHang(iddonhang, tien, idnhanvien, idquanly, ghichu, url, 2))
                        {
                            OBJ.status = true;
                            OBJ.msg = Config.THANH_CONG;
                        }
                    }
                    donhang = dhData.LayDonHang(param.iddonhang);
                    if (donhang.trangthaithanhtoan == 4)
                    {
                        try
                        {
                            LSPos_Data.Utilities.Log.Info("Start");
                            DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                            ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();

                            List<DonHang_DichVuRequestAPIModel> lstdv = dvdao.GetAllByDonHang(donhang.iddonhang);
                            if (lstdv.Count > 0)
                            {
                                List<SiteModel> lstSite = lstdv.Select(x => x.Site).ToList();
                                List<SiteModel> lstSitedistinct = new List<SiteModel>();
                                foreach (SiteModel s in lstSite)
                                {
                                    if (lstSitedistinct.Where(x => x.SiteCode == s.SiteCode).FirstOrDefault() == null)
                                    {
                                        lstSitedistinct.Add(s);
                                    }
                                }
                                SaveBookingModel saveBooking = null;
                                foreach (SiteModel site in lstSitedistinct)
                                {
                                    List<DonHang_DichVuRequestAPIModel> models = lstdv.Where(x => x.Site.SiteCode == site.SiteCode).ToList();

                                    var data = await BookingUtilities.XyLuTaoBooking(models, site, _sellRepository, kh);

                                }
                                //if (saveBooking != null)
                                //{
                                DonHangv2 dh = dhData.GetDonHangTheoID_v2(param.iddonhang, userinfo.ID_QuanLy);
                                dh.chitietdonhang = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(param.iddonhang);
                                dh.lichsuthanhtoan = new DonHang_dl().GetLichSuThanhToan(param.iddonhang);

                                EmailHelper helper = new EmailHelper();
                                string path = HttpContext.Current.Server.MapPath("~/EmailTemplate/InvoicePaid.html");
                                string bodyTemplate = System.IO.File.ReadAllText(path);
                                var html = Engine.Razor.RunCompile(bodyTemplate, "MailInvoicePaid", dh.GetType(), dh);
                                //helper.SendEmail(html.ToString(), dh.Email, null, "[THÔNG BÁO] XÁC NHẬN ĐƠN " + dh.MaThamChieu + " THANH TOÁN!");
                                //}
                            }
                            else
                            {
                                dhData.UpdateTrangThaiDonHang(donhang.iddonhang, 3);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    string sJsonKetQua = JsonConvert.SerializeObject(OBJ);
                    response = Request.CreateResponse(HttpStatusCode.OK, sJsonKetQua);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(OBJ));
            });
        }

        [HttpPost]
        [Route("thanhToanDonHangLspay")]
        public async Task<HttpResponseMessage> thanhToanDonHangLspay([FromBody] paramTToan param)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            AppThanhToanOBJ OBJ = new AppThanhToanOBJ();
            OBJ.status = false;
            OBJ.msg = Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG;
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
                    string type = param.type;
                    int iddonhang = param.iddonhang;
                    int idnhanvien = param.idnhanvien;
                    long idquanly = param.idquanly;
                    string ghichu = param.ghichu;
                    double tien = param.tien;
                    string url = param.url;
                    DonHangData dhData = new DonHangData();
                    DonHangModels donhang = dhData.LayDonHang(param.iddonhang);


                    NhanVienApp nvApp = new NhanVienApp();
                    NhanVienAppModels nhanVienInfor = nvApp.ThongTinNhanVienTheoID(donhang.idnhanvien);
                    if (dhData.BienDongSoDuNhomTaiKhoan(nhanVienInfor.idnhom, DateTime.Now, -1, iddonhang, (float)donhang.tongtien, 0, ghichu))
                    {
                        float sodu = dhData.BienDongSoDuNhom_TongTienHienTai(nhanVienInfor.idnhom);
                        float gioihangcn = NhomData.getNhomByID(nhanVienInfor.idnhom).CongNoGioiHan;
                        if ((sodu - gioihangcn) >= donhang.tongtien)
                        {
                            if (dhData.BienDongSoDuNhom_UpdateTrangThaiThanhToan(userinfo.ID_QuanLy, param.iddonhang) && dhData.ThanhToanDonHang(iddonhang, tien, idnhanvien, idquanly, ghichu, url, 4))
                            {
                                OBJ.status = true;
                                OBJ.msg = Config.THANH_CONG;
                            }
                        }
                        else
                        {
                            OBJ.status = false;
                            OBJ.msg = "Số dư trong ví không đủ để thực hiện thanh toán! Vui lòng nạp thêm!";
                        }
                    }
                    else
                    {
                        OBJ.status = false;
                        OBJ.msg = "Không thể sử dụng ví";
                    }

                    donhang = dhData.LayDonHang(param.iddonhang);
                    KhachHang kh = new KhachHang_dl().GetKhachHangID(donhang.idcuahang);
                    if (donhang.trangthaithanhtoan == 4)
                    {
                        try
                        {
                            LSPos_Data.Utilities.Log.Info("Start");
                            DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                            ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();

                            List<DonHang_DichVuRequestAPIModel> lstdv = dvdao.GetAllByDonHang(donhang.iddonhang);
                            if (lstdv.Count > 0)
                            {
                                List<SiteModel> lstSite = lstdv.Select(x => x.Site).ToList();
                                List<SiteModel> lstSitedistinct = new List<SiteModel>();
                                foreach (SiteModel s in lstSite)
                                {
                                    if (lstSitedistinct.Where(x => x.SiteCode == s.SiteCode).FirstOrDefault() == null)
                                    {
                                        lstSitedistinct.Add(s);
                                    }
                                }
                                SaveBookingModel saveBooking = null;
                                foreach (SiteModel site in lstSitedistinct)
                                {
                                    List<DonHang_DichVuRequestAPIModel> models = lstdv.Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                                    var data = await BookingUtilities.XyLuTaoBooking(models, site, _sellRepository, kh);

                                }
                                //if (saveBooking != null)
                                //{
                                DonHangv2 dh = dhData.GetDonHangTheoID_v2(param.iddonhang, userinfo.ID_QuanLy);
                                dh.chitietdonhang = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(param.iddonhang);
                                dh.lichsuthanhtoan = new DonHang_dl().GetLichSuThanhToan(param.iddonhang);

                                EmailHelper helper = new EmailHelper();
                                string path = HttpContext.Current.Server.MapPath("~/EmailTemplate/InvoicePaid.html");
                                string bodyTemplate = System.IO.File.ReadAllText(path);
                                var html = Engine.Razor.RunCompile(bodyTemplate, "MailInvoicePaid", dh.GetType(), dh);
                                //helper.SendEmail(html.ToString(), dh.Email, null, "[THÔNG BÁO] XÁC NHẬN ĐƠN " + dh.MaThamChieu + " THANH TOÁN!");
                                //}
                            }
                            else
                            {
                                dhData.UpdateTrangThaiDonHang(donhang.iddonhang, 3);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    //}
                    //else
                    //{
                    //    OBJ.msg = Config.KHONG_CO_QUYEN_THAO_TAC;
                    //}
                    string sJsonKetQua = JsonConvert.SerializeObject(OBJ);
                    response = Request.CreateResponse(HttpStatusCode.OK, sJsonKetQua);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(OBJ));
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ipnVnpay")]
        public HttpResponseMessage ipnVnpay(VnPayIPNRequest data)
        {
            LSPos_Data.Utilities.Log.Info(JsonConvert.SerializeObject(data));
            HttpResponseMessage response = new HttpResponseMessage();
            VnPayIPNResponse res = new VnPayIPNResponse();
            res.code = data.code;
            res.message = "Success";
            DonHangModels donhang = new DonHangModels();
            DonHangData dhData = new DonHangData();

            if (!string.IsNullOrWhiteSpace(data.txnId))
            {
                try
                {
                    LSPos_Data.Utilities.Log.Info("Process Invoice: " + data.txnId);
                    int iddonhang = int.Parse(data.txnId);
                    donhang = dhData.LayDonHang(iddonhang);
                }
                catch (Exception ex)
                {
                    donhang = dhData.LayDonHang(data.txnId);
                }
                string secretKey = "lscloud@vnpay2023";
                string checksum = new VnPayUtils().Md5(data.CreateChecksum(secretKey));


                if (checksum.ToUpper() == data.checksum.ToUpper())
                {
                    int idnhanvien = 0;
                    long idquanly = 0;
                    string ghichu = "VNPAY";
                    if (donhang.trangthaithanhtoan == 4 && donhang.tongtien > 0)
                    {
                        //don hang da xu ly xong
                        res.code = "03";
                        res.message = Config.DON_HANG_DA_XU_LY;
                    }
                    else if (donhang.iddonhang == 0)
                    {
                        res.code = "04";
                        res.message = "Order Not found";
                    }
                    else if (double.Parse(data.amount) != donhang.tongtien)
                    {
                        res.code = "07";
                        res.message = "Invalid Amount";
                    }
                    else
                    {
                        if (dhData.ThanhToanDonHang(donhang.iddonhang, double.Parse(data.amount), idnhanvien, idquanly, ghichu, JsonConvert.SerializeObject(data), 5))
                        {
                            donhang = dhData.LayDonHang(donhang.iddonhang);
                            if (donhang.trangthaithanhtoan == 4)
                            {
                                try
                                {
                                    DonHangv2 dh = dhData.GetDonHangTheoID_v2(donhang.iddonhang, 0);
                                    dh.chitietdonhang = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(donhang.iddonhang);
                                    dh.lichsuthanhtoan = new DonHang_dl().GetLichSuThanhToan(donhang.iddonhang);
                                    EmailHelper helper = new EmailHelper();
                                    string path = HttpContext.Current.Server.MapPath("~/EmailTemplate/InvoicePaid.html");
                                    string bodyTemplate = System.IO.File.ReadAllText(path);
                                    var html = Engine.Razor.RunCompile(bodyTemplate, "MailInvoicePaid", dh.GetType(), dh);
                                    //helper.SendEmail(html.ToString(), dh.Email, null, "[THÔNG BÁO] XÁC NHẬN ĐƠN " + dh.MaThamChieu + " THANH TOÁN!");
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }
                                try
                                {
                                    LSPos_Data.Utilities.Log.Info("Start");
                                    DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                                    ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();

                                    List<DonHang_DichVuRequestAPIModel> lstdv = dvdao.GetAllByDonHang(donhang.iddonhang);
                                    if (lstdv.Count > 0)
                                    {
                                        List<SiteModel> lstSite = lstdv.Select(x => x.Site).ToList();
                                        List<SiteModel> lstSitedistinct = new List<SiteModel>();
                                        foreach (SiteModel s in lstSite)
                                        {
                                            if (lstSitedistinct.Where(x => x.SiteCode == s.SiteCode).FirstOrDefault() == null)
                                            {
                                                lstSitedistinct.Add(s);
                                            }
                                        }
                                        SaveBookingModel saveBooking = null;
                                        foreach (SiteModel site in lstSitedistinct)
                                        {
                                            List<DonHang_DichVuRequestAPIModel> models = lstdv.Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                                            var dd = BookingUtilities.XuLyXuatVe(models, site, donhang.iddonhang, 0, _bookingRepository, _sellRepository);
                                            //var data = await BookingUtilities.XyLuTaoBooking(models, site, _sellRepository);

                                        }

                                    }
                                    else
                                    {
                                        dhData.UpdateTrangThaiDonHang(donhang.iddonhang, 3);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }
                            }
                        }
                    }
                }
                else
                {
                    res.code = "06";
                    res.message = "Infomation not valid";
                }
            }

            response = Request.CreateResponse(HttpStatusCode.OK, res);
            return response;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ipnVnpay_KC")]
        public HttpResponseMessage ipnVnpayKC(VnPayIPNRequest data)
        {
            LSPos_Data.Utilities.Log.Info(JsonConvert.SerializeObject(data));
            HttpResponseMessage response = new HttpResponseMessage();
            VnPayIPNResponse res = new VnPayIPNResponse();
            res.code = data.code;
            res.message = "Success";
            DonHangModels donhang = new DonHangModels();
            DonHangData dhData = new DonHangData();

            NhanVienApp nvApp = new NhanVienApp();
            NhanVienAppModels nhanVienInfor = new NhanVienAppModels();

            if (!string.IsNullOrWhiteSpace(data.txnId))
            {
                try
                {
                    LSPos_Data.Utilities.Log.Info("Process Invoice KC: " + data.txnId);
                    int iddonhang = int.Parse(data.txnId);
                    donhang = dhData.LayDonHang(iddonhang);
                    nhanVienInfor = nvApp.ThongTinNhanVienTheoID(donhang.idnhanvien);
                }
                catch (Exception ex)
                {
                    donhang = dhData.LayDonHang(data.txnId);
                    nhanVienInfor = nvApp.ThongTinNhanVienTheoID(donhang.idnhanvien);
                }
                string secretKey = "lscloud@vnpay2023";
                string checksum = new VnPayUtils().Md5(data.CreateChecksum(secretKey));


                if (checksum.ToUpper() == data.checksum.ToUpper())
                {
                    int idnhanvien = 0;
                    long idquanly = 0;
                    string ghichu = "VNPAY";
                    if (donhang.trangthaithanhtoan == 4 && donhang.tongtien > 0)
                    {
                        //don hang da xu ly xong
                        res.code = "03";
                        res.message = Config.DON_HANG_DA_XU_LY;
                    }
                    else if (donhang.iddonhang == 0)
                    {
                        res.code = "04";
                        res.message = "Order Not found";
                    }
                    else if (double.Parse(data.amount) != donhang.tongtien)
                    {
                        res.code = "07";
                        res.message = "Invalid Amount";
                    }
                    else
                    {
                        if (dhData.ThanhToanDonHang(donhang.iddonhang, double.Parse(data.amount), idnhanvien, idquanly, ghichu, JsonConvert.SerializeObject(data), 7))
                        {
                            dhData.BienDongSoDuNhomTaiKhoan(nhanVienInfor.idnhom, DateTime.Now, -1, donhang.iddonhang, (float)donhang.tongtien, 1, "VNPAY");
                            donhang = dhData.LayDonHang(donhang.iddonhang);
                            if (donhang.trangthaithanhtoan == 4)
                            {
                                try
                                {
                                    DonHangv2 dh = dhData.GetDonHangTheoID_v2(donhang.iddonhang, 0);
                                    dh.chitietdonhang = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(donhang.iddonhang);
                                    dh.lichsuthanhtoan = new DonHang_dl().GetLichSuThanhToan(donhang.iddonhang);
                                    EmailHelper helper = new EmailHelper();
                                    string path = HttpContext.Current.Server.MapPath("~/EmailTemplate/InvoicePaid.html");
                                    string bodyTemplate = System.IO.File.ReadAllText(path);
                                    var html = Engine.Razor.RunCompile(bodyTemplate, "MailInvoicePaid", dh.GetType(), dh);
                                    //helper.SendEmail(html.ToString(), dh.Email, null, "[THÔNG BÁO] XÁC NHẬN ĐƠN " + dh.MaThamChieu + " THANH TOÁN!");
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }
                                try
                                {
                                    LSPos_Data.Utilities.Log.Info("Start");
                                    DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                                    ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();

                                    List<DonHang_DichVuRequestAPIModel> lstdv = dvdao.GetAllByDonHang(donhang.iddonhang);
                                    if (lstdv.Count > 0)
                                    {
                                        List<SiteModel> lstSite = lstdv.Select(x => x.Site).ToList();
                                        List<SiteModel> lstSitedistinct = new List<SiteModel>();
                                        foreach (SiteModel s in lstSite)
                                        {
                                            if (lstSitedistinct.Where(x => x.SiteCode == s.SiteCode).FirstOrDefault() == null)
                                            {
                                                lstSitedistinct.Add(s);
                                            }
                                        }
                                        SaveBookingModel saveBooking = null;
                                        foreach (SiteModel site in lstSitedistinct)
                                        {
                                            List<DonHang_DichVuRequestAPIModel> models = lstdv.Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                                            var dd = BookingUtilities.XuLyXuatVe(models, site, donhang.iddonhang, 0, _bookingRepository, _sellRepository);
                                            //var data = await BookingUtilities.XyLuTaoBooking(models, site, _sellRepository);

                                        }

                                    }
                                    else
                                    {
                                        dhData.UpdateTrangThaiDonHang(donhang.iddonhang, 3);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }
                            }
                        }
                    }
                }
                else
                {
                    res.code = "06";
                    res.message = "Infomation not valid";
                }
            }

            response = Request.CreateResponse(HttpStatusCode.OK, res);
            return response;
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("thanhToanDonHangOnepay")]
        public async Task<HttpResponseMessage> thanhToanDonHangOnepay([FromUri] string vpc_Amount, string vpc_CardNum, string vpc_Command, string vpc_MerchTxnRef, string vpc_Merchant, string vpc_Message, string vpc_OrderInfo, string vpc_PayChannel, string vpc_TransactionNo, string vpc_TxnResponseCode, string vpc_Version, string vpc_SecureHash)
        {
            string dataipn = vpc_Amount + "/" + vpc_CardNum + "/" + vpc_Command + "/" + vpc_MerchTxnRef + "/" + vpc_Merchant + "/" + vpc_Message + "/" + vpc_OrderInfo + "/" + vpc_PayChannel + "/" + vpc_TransactionNo + "/" + vpc_TxnResponseCode + "/" + vpc_Version + "/" + vpc_SecureHash;
            LSPos_Data.Utilities.Log.Info(dataipn);
            HttpResponseMessage response = new HttpResponseMessage();
            AppThanhToanOBJ OBJ = new AppThanhToanOBJ();
            OBJ.status = false;
            OBJ.msg = Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG;
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                //UserInfo userinfo = utilsCommon.checkAuthorization();

                //if (userinfo == null)
                //{
                //    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                //}
                //else
                //{

                if (vpc_TxnResponseCode == "0")
                {
                    if (vpc_MerchTxnRef.IndexOf("lspay") < 0)
                    {
                        int iddonhang = int.Parse(vpc_MerchTxnRef.Split('_')[0].ToString());
                        LSPos_Data.Utilities.Log.Info("Process Invoice: " + iddonhang);

                        string tick = vpc_MerchTxnRef.Split('_')[1].ToString();

                        int idnhanvien = 0;
                        long idquanly = 0;
                        string ghichu = "ONEPAY";
                        DonHangData dhData = new DonHangData();
                        DonHangModels donhang = dhData.LayDonHang(iddonhang);
                        //if (userinfo.IsAdmin)
                        //{
                        if (tick != donhang.thoigiantao.Ticks.ToString())
                        {
                            OBJ.status = false;
                            OBJ.msg = Config.THONG_TIN_KHONG_DUNG;
                        }
                        else
                        {
                            if (donhang.trangthaithanhtoan == 4 && donhang.tongtien > 0)
                            {
                                //don hang da xu ly xong
                                OBJ.status = false;
                                OBJ.msg = Config.DON_HANG_DA_XU_LY;
                            }
                            else
                            {

                                if (dhData.ThanhToanDonHang(iddonhang, double.Parse(vpc_Amount) / 100, idnhanvien, idquanly, ghichu, dataipn, 3))
                                {
                                    OBJ.status = true;
                                    OBJ.msg = Config.THANH_CONG;


                                    donhang = dhData.LayDonHang(iddonhang);
                                    if (donhang.trangthaithanhtoan == 4)
                                    {
                                        try
                                        {
                                            DonHangv2 dh = dhData.GetDonHangTheoID_v2(iddonhang, 0);
                                            dh.chitietdonhang = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(iddonhang);
                                            dh.lichsuthanhtoan = new DonHang_dl().GetLichSuThanhToan(iddonhang);
                                            EmailHelper helper = new EmailHelper();
                                            string path = HttpContext.Current.Server.MapPath("~/EmailTemplate/InvoicePaid.html");
                                            string bodyTemplate = System.IO.File.ReadAllText(path);
                                            var html = Engine.Razor.RunCompile(bodyTemplate, "MailInvoicePaid", dh.GetType(), dh);
                                            //helper.SendEmail(html.ToString(), dh.Email, null, "[THÔNG BÁO] XÁC NHẬN ĐƠN " + dh.MaThamChieu + " THANH TOÁN!");
                                        }
                                        catch (Exception ex)
                                        {
                                            LSPos_Data.Utilities.Log.Error(ex);
                                        }
                                        try
                                        {
                                            LSPos_Data.Utilities.Log.Info("Start");
                                            DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                                            ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();

                                            List<DonHang_DichVuRequestAPIModel> lstdv = dvdao.GetAllByDonHang(donhang.iddonhang);
                                            if (lstdv.Count > 0)
                                            {
                                                List<SiteModel> lstSite = lstdv.Select(x => x.Site).ToList();
                                                List<SiteModel> lstSitedistinct = new List<SiteModel>();
                                                foreach (SiteModel s in lstSite)
                                                {
                                                    if (lstSitedistinct.Where(x => x.SiteCode == s.SiteCode).FirstOrDefault() == null)
                                                    {
                                                        lstSitedistinct.Add(s);
                                                    }
                                                }
                                                SaveBookingModel saveBooking = null;
                                                foreach (SiteModel site in lstSitedistinct)
                                                {
                                                    List<DonHang_DichVuRequestAPIModel> models = lstdv.Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                                                    var data = await BookingUtilities.XuLyXuatVe(models, site, iddonhang, 0, _bookingRepository, _sellRepository);
                                                    //var data = await BookingUtilities.XyLuTaoBooking(models, site, _sellRepository);

                                                }

                                            }
                                            else
                                            {
                                                dhData.UpdateTrangThaiDonHang(donhang.iddonhang, 3);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            LSPos_Data.Utilities.Log.Error(ex);
                                        }
                                    }
                                }
                            }
                        }
                        string sJsonKetQua = JsonConvert.SerializeObject(OBJ);
                        response = Request.CreateResponse(HttpStatusCode.OK, sJsonKetQua);
                    }
                    else
                    {
                        LichSuNapVi_NhomTaiKhoanModel item = new LichSuNapVi_NhomTaiKhoanModel();
                        int ID_NhomTaiKhoan = int.Parse(vpc_MerchTxnRef.Split('_')[1].ToString());
                        LSPos_Data.Utilities.Log.Info("Process Lspay: " + ID_NhomTaiKhoan);
                        LichSuNapVi_NhomTaiKhoanDB db = new LichSuNapVi_NhomTaiKhoanDB();
                        item = db.GetLichSuNapViByID(int.Parse(vpc_MerchTxnRef.Split('_')[2].ToString()));
                        item.TrangThai = 1;
                        item.ImgUrl = "";
                        item.SoTien = decimal.Parse(vpc_Amount) / 100m;
                        item.DuLieuThanhToan = vpc_Amount + "/" + vpc_CardNum + "/" + vpc_Command + "/" + vpc_MerchTxnRef + "/" + vpc_Merchant + "/" + vpc_Message + "/" + vpc_OrderInfo + "/" + vpc_PayChannel + "/" + vpc_TransactionNo + "/" + vpc_TxnResponseCode + "/" + vpc_Version + "/" + vpc_SecureHash;
                        if (db.UpdateThanhCong_LichSuNapVi(item))
                        {
                            OBJ.status = true;
                            OBJ.msg = Config.THANH_CONG;
                        }
                    }
                }
                else
                {
                    string sJsonKetQua = JsonConvert.SerializeObject(OBJ);
                    response = Request.CreateResponse(HttpStatusCode.BadRequest, sJsonKetQua);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(OBJ));
            });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("thanhToanDonHangOnepayB2C")]
        public async Task<HttpResponseMessage> thanhToanDonHangOnepayB2C([FromUri] string vpc_Amount, string vpc_CardNum, string vpc_Command, string vpc_MerchTxnRef, string vpc_Merchant, string vpc_Message, string vpc_OrderInfo, string vpc_PayChannel, string vpc_TransactionNo, string vpc_TxnResponseCode, string vpc_Version, string vpc_SecureHash)
        {
            string dataipn = vpc_Amount + "/" + vpc_CardNum + "/" + vpc_Command + "/" + vpc_MerchTxnRef + "/" + vpc_Merchant + "/" + vpc_Message + "/" + vpc_OrderInfo + "/" + vpc_PayChannel + "/" + vpc_TransactionNo + "/" + vpc_TxnResponseCode + "/" + vpc_Version + "/" + vpc_SecureHash;
            LSPos_Data.Utilities.Log.Info(dataipn);
            HttpResponseMessage response = new HttpResponseMessage();
            AppThanhToanOBJ OBJ = new AppThanhToanOBJ();
            OBJ.status = false;
            OBJ.msg = Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG;
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                //UserInfo userinfo = utilsCommon.checkAuthorization();

                //if (userinfo == null)
                //{
                //    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                //}
                //else
                //{

                if (vpc_TxnResponseCode == "0")
                {

                    int iddonhang = int.Parse(vpc_MerchTxnRef.Split('_')[0].ToString());
                    LSPos_Data.Utilities.Log.Info("Process Invoice: " + iddonhang);

                    string tick = vpc_MerchTxnRef.Split('_')[1].ToString();

                    int idnhanvien = 0;
                    long idquanly = 0;
                    string ghichu = "ONEPAY-B2C";
                    DonHangData dhData = new DonHangData();
                    DonHangModels donhang = dhData.LayDonHang(iddonhang);
                    //if (userinfo.IsAdmin)
                    //{
                    if (tick != donhang.thoigiantao.Ticks.ToString())
                    {
                        OBJ.status = false;
                        OBJ.msg = Config.THONG_TIN_KHONG_DUNG;
                    }
                    else
                    {
                        if (donhang.trangthaithanhtoan == 4 && donhang.tongtien > 0)
                        {
                            //don hang da xu ly xong
                            OBJ.status = false;
                            OBJ.msg = Config.DON_HANG_DA_XU_LY;
                        }
                        else
                        {

                            if (dhData.ThanhToanDonHang(iddonhang, double.Parse(vpc_Amount) / 100, idnhanvien, idquanly, ghichu, dataipn, 3))
                            {
                                OBJ.status = true;
                                OBJ.msg = Config.THANH_CONG;


                                donhang = dhData.LayDonHang(iddonhang);
                                if (donhang.trangthaithanhtoan == 4)
                                {
                                    try
                                    {
                                        DonHangv2 dh = dhData.GetDonHangTheoID_v2(iddonhang, 0);
                                        dh.chitietdonhang = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(iddonhang);
                                        dh.lichsuthanhtoan = new DonHang_dl().GetLichSuThanhToan(iddonhang);
                                        EmailHelper helper = new EmailHelper();
                                        string path = HttpContext.Current.Server.MapPath("~/EmailTemplate/InvoicePaid.html");
                                        string bodyTemplate = System.IO.File.ReadAllText(path);
                                        var html = Engine.Razor.RunCompile(bodyTemplate, "MailInvoicePaid", dh.GetType(), dh);
                                        //helper.SendEmail(html.ToString(), dh.Email, null, "[THÔNG BÁO] XÁC NHẬN ĐƠN " + dh.MaThamChieu + " THANH TOÁN!");
                                    }
                                    catch (Exception ex)
                                    {
                                        LSPos_Data.Utilities.Log.Error(ex);
                                    }
                                    try
                                    {
                                        LSPos_Data.Utilities.Log.Info("Start");
                                        DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                                        ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();

                                        List<DonHang_DichVuRequestAPIModel> lstdv = dvdao.GetAllByDonHang(donhang.iddonhang);
                                        if (lstdv.Count > 0)
                                        {
                                            List<SiteModel> lstSite = lstdv.Select(x => x.Site).ToList();
                                            List<SiteModel> lstSitedistinct = new List<SiteModel>();
                                            foreach (SiteModel s in lstSite)
                                            {
                                                if (lstSitedistinct.Where(x => x.SiteCode == s.SiteCode).FirstOrDefault() == null)
                                                {
                                                    lstSitedistinct.Add(s);
                                                }
                                            }
                                            SaveBookingModel saveBooking = null;
                                            foreach (SiteModel site in lstSitedistinct)
                                            {
                                                List<DonHang_DichVuRequestAPIModel> models = lstdv.Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                                                var data = await BookingUtilities.XuLyXuatVe(models, site, iddonhang, 0, _bookingRepository, _sellRepository);
                                                //var data = await BookingUtilities.XyLuTaoBooking(models, site, _sellRepository);

                                            }

                                        }
                                        else
                                        {
                                            dhData.UpdateTrangThaiDonHang(donhang.iddonhang, 3);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        LSPos_Data.Utilities.Log.Error(ex);
                                    }
                                }
                            }
                        }
                    }
                    string sJsonKetQua = JsonConvert.SerializeObject(OBJ);
                    response = Request.CreateResponse(HttpStatusCode.OK, sJsonKetQua);
                }
                else
                {
                    string sJsonKetQua = JsonConvert.SerializeObject(OBJ);
                    response = Request.CreateResponse(HttpStatusCode.BadRequest, sJsonKetQua);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return await Task<HttpResponseMessage>.Factory.StartNew(() =>
            {
                return Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(OBJ));
            });
        }

        [HttpPost]
        [Route("smsBanking")]
        public HttpResponseMessage smsBanking([FromBody] SmsBankingModel param)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            AppThanhToanOBJ OBJ = new AppThanhToanOBJ();
            OBJ.status = false;
            OBJ.msg = Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG;
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
                    DonHangData dhData = new DonHangData();
                    DonHangModels donhang = dhData.LayDonHang((int)param.ID_DonHang);
                    SmsBankingDAO smsdao = new SmsBankingDAO();
                    if (smsdao.Insert(param))
                    {
                        //if (userinfo.IsAdmin)
                        //{
                        if (donhang.trangthaithanhtoan == 4)
                        {
                            //don hang da xu ly xong
                            OBJ.status = false;
                            OBJ.msg = Config.DON_HANG_DA_XU_LY;
                        }
                        else
                        {
                            if (param.Amount < donhang.tongtien)
                            {

                                if (dhData.ThanhToanDonHang1phan((int)param.ID_DonHang, param.Amount, 0, "SMS Banking", 1, "", 2))
                                {
                                    OBJ.status = true;
                                    OBJ.msg = Config.THANH_CONG;
                                }
                            }
                            else if (param.Amount >= donhang.tongtien)
                            {
                                if (dhData.ThanhToanDonHang((int)param.ID_DonHang, param.Amount, 0, 1, "SMS Banking", "", 2))
                                {
                                    OBJ.status = true;
                                    OBJ.msg = Config.THANH_CONG;
                                }
                            }
                            donhang = dhData.LayDonHang((int)param.ID_DonHang);
                        }
                    }
                    if (donhang.trangthaithanhtoan == 4)
                    {
                        DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                        ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();
                        List<DonHang_DichVuRequestAPIModel> lstdv = dvdao.GetAllByDonHang(donhang.iddonhang);
                        if (lstdv.Count > 0)
                        {
                            List<SiteModel> lstSite = lstdv.Select(x => x.Site).ToList();
                            List<SiteModel> lstSitedistinct = new List<SiteModel>();
                            foreach (SiteModel s in lstSite)
                            {
                                if (lstSitedistinct.Where(x => x.SiteCode == s.SiteCode).FirstOrDefault() == null)
                                {
                                    lstSitedistinct.Add(s);
                                }
                            }
                            foreach (SiteModel site in lstSitedistinct)
                            {
                                List<DonHang_DichVuRequestAPIModel> models = lstdv.Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                                DonHang_DichVuRequestAPIModel dichvudacbiet = models.Where(x => !string.IsNullOrEmpty(x.ProfileCode)).FirstOrDefault();
                                if (dichvudacbiet != null)
                                {
                                    new GlobalUtilities().SetGlobal(site.ApiUrl, site.UserName, site.Password, site.SiteCode, dichvudacbiet.ProfileID, dichvudacbiet.ProfileCode, dichvudacbiet.MemberID);
                                }
                                SaveBookingModel saveBooking = CreateBookingToLocalAPI(models);
                                if (saveBooking != null)
                                {
                                    foreach (DonHang_DichVuRequestAPIModel model in models)
                                    {
                                        ChiTietMatHang_DichVuModel ct = new ChiTietMatHang_DichVuModel();
                                        ct.ID = model.ID;
                                        ct.BookingCode = saveBooking.BookingCode;
                                        ct.InvoiceCode = saveBooking.InvoiceCode;
                                        ct.SoLuong = model.SoLuong;
                                        ctmhdao.InsertOrUpdate(ct);
                                    }
                                }
                            }


                            DonHangv2 dh = dhData.GetDonHangTheoID_v2((int)param.ID_DonHang, userinfo.ID_QuanLy);
                            dh.chitietdonhang = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang((int)param.ID_DonHang);
                            dh.lichsuthanhtoan = new DonHang_dl().GetLichSuThanhToan((int)param.ID_DonHang);

                            EmailHelper helper = new EmailHelper();
                            string path = HttpContext.Current.Server.MapPath("~/EmailTemplate/InvoicePaid.html");
                            string bodyTemplate = System.IO.File.ReadAllText(path);
                            var html = Engine.Razor.RunCompile(bodyTemplate, "MailInvoicePaid", dh.GetType(), dh);
                            //helper.SendEmail(html.ToString(), dh.Email, null, "[THÔNG BÁO] XÁC NHẬN ĐƠN " + dh.MaThamChieu + " THANH TOÁN!");
                        }
                        else
                        {
                            dhData.UpdateTrangThaiDonHang(donhang.iddonhang, 3);
                        }
                    }
                    //}
                    //else
                    //{
                    //    OBJ.msg = Config.KHONG_CO_QUYEN_THAO_TAC;
                    //}
                    //string sJsonKetQua = JsonConvert.SerializeObject(OBJ);
                    response = Request.CreateResponse(HttpStatusCode.OK, OBJ);
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
        [Route("xuLyDonHang")]
        public HttpResponseMessage xuLyDonHang([FromUri] int ID_DonHang)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            AppThanhToanOBJ OBJ = new AppThanhToanOBJ();
            OBJ.status = false;
            OBJ.msg = Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG;
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
                    DonHangData dhData = new DonHangData();
                    DonHangModels donhang = dhData.LayDonHang(ID_DonHang);
                    //if (userinfo.IsAdmin || userinfo.Username.Contains("pchl"))
                    //{
                    if (donhang.trangthaithanhtoan == 4 && donhang.trangthaidonhang == 2)
                    {
                        DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                        ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();
                        List<DonHang_DichVuRequestAPIModel> lstdv = dvdao.GetAllByDonHang(donhang.iddonhang);
                        List<SiteModel> lstSite = lstdv.Select(x => x.Site).ToList();
                        List<SiteModel> lstSitedistinct = new List<SiteModel>();
                        foreach (SiteModel s in lstSite)
                        {
                            if (lstSitedistinct.Where(x => x.SiteCode == s.SiteCode).FirstOrDefault() == null)
                            {
                                lstSitedistinct.Add(s);
                            }
                        }
                        foreach (SiteModel site in lstSitedistinct)
                        {
                            List<DonHang_DichVuRequestAPIModel> models = lstdv.Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                            BookingUtilities.XuLyXuatVe(models, site, ID_DonHang, userinfo.ID_QuanLy, _bookingRepository, _sellRepository);

                            OBJ.status = true;
                            OBJ.msg = Config.THANH_CONG;
                        }

                    }
                    //}
                    //else
                    //{
                    //    OBJ.msg = Config.KHONG_CO_QUYEN_THAO_TAC;
                    //}
                    string sJsonKetQua = JsonConvert.SerializeObject(OBJ);
                    response = Request.CreateResponse(HttpStatusCode.OK, sJsonKetQua);
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
        [Route("guiLaiEmail")]
        public HttpResponseMessage guiLaiEmail([FromUri] int ID_DonHang)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            AppThanhToanOBJ OBJ = new AppThanhToanOBJ();
            OBJ.status = false;
            OBJ.msg = Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG;
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
                    DonHangData dhData = new DonHangData();
                    DonHangModels donhang = dhData.LayDonHang(ID_DonHang);

                    DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                    ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();
                    List<DonHang_DichVuRequestAPIModel> lstdv = dvdao.GetAllByDonHang(donhang.iddonhang);
                    List<SiteModel> lstSite = lstdv.Select(x => x.Site).ToList();
                    List<SiteModel> lstSitedistinct = new List<SiteModel>();
                    foreach (SiteModel s in lstSite)
                    {
                        if (lstSitedistinct.Where(x => x.SiteCode == s.SiteCode).FirstOrDefault() == null)
                        {
                            lstSitedistinct.Add(s);
                        }
                    }
                    foreach (SiteModel site in lstSitedistinct)
                    {
                        List<DonHang_DichVuRequestAPIModel> models = lstdv.Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                        BookingUtilities.ResendEmail(site, ID_DonHang, userinfo.ID_QuanLy);

                        OBJ.status = true;
                        OBJ.msg = Config.THANH_CONG;
                    }

                    string sJsonKetQua = JsonConvert.SerializeObject(OBJ);
                    response = Request.CreateResponse(HttpStatusCode.OK, sJsonKetQua);
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
        [Route("capNhatDsVe")]
        public HttpResponseMessage capNhatDsVe([FromUri] int ID_DonHang)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            AppThanhToanOBJ OBJ = new AppThanhToanOBJ();
            OBJ.status = false;
            OBJ.msg = Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG;
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
                    DonHangData dhData = new DonHangData();
                    DonHangModels donhang = dhData.LayDonHang(ID_DonHang);
                    if (userinfo.IsAdmin || userinfo.Username.Contains("pchl"))
                    {
                        //if (donhang.trangthaithanhtoan == 4 && donhang.trangthaidonhang == 2)
                        //{
                        DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                        ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();
                        List<DonHang_DichVuRequestAPIModel> lstdv = dvdao.GetAllByDonHang(donhang.iddonhang);
                        List<SiteModel> lstSite = lstdv.Select(x => x.Site).ToList();
                        List<SiteModel> lstSitedistinct = new List<SiteModel>();
                        foreach (SiteModel s in lstSite)
                        {
                            if (lstSitedistinct.Where(x => x.SiteCode == s.SiteCode).FirstOrDefault() == null)
                            {
                                lstSitedistinct.Add(s);
                            }
                        }
                        foreach (SiteModel site in lstSitedistinct)
                        {
                            List<DonHang_DichVuRequestAPIModel> models = lstdv.Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                            DonHang_DichVuRequestAPIModel dichvudacbiet = models.Where(x => !string.IsNullOrEmpty(x.ProfileCode) && !string.IsNullOrEmpty(x.BookingCode)).FirstOrDefault();
                            if (dichvudacbiet != null)
                            {
                                new GlobalUtilities().SetGlobal(site.ApiUrl, site.UserName, site.Password, site.SiteCode, dichvudacbiet.ProfileID, dichvudacbiet.ProfileCode, dichvudacbiet.MemberID);
                            }
                            BookingListB2bDetail bookingListPosResult = _bookingRepository.GetBookingDetail(Global.ProfileID, dichvudacbiet.BookingCode, dichvudacbiet.Ngay, dichvudacbiet.Ngay.AddDays(dichvudacbiet.HanSuDung), "B2B");
                            BookingB2bDetail blp = bookingListPosResult.Bookings.FirstOrDefault();
                            BookingRes bk = _bookingRepository.Detail(blp.BookingCode);



                            if (bk != null)
                            {
                                foreach (DonHang_DichVuRequestAPIModel dhdv in models)
                                {
                                    ChiTietMatHang_DichVuModel ct = new ChiTietMatHang_DichVuModel();
                                    ct.ID = dhdv.ID;
                                    ct.BookingCode = blp.BookingCode;
                                    ct.InvoiceCode = blp.InvoiceCode;
                                    ctmhdao.InsertOrUpdate(ct);
                                    List<ChiTietMatHangDonHangModels> lstct = new List<ChiTietMatHangDonHangModels>();
                                    foreach (BookingAccountResponse account in bk.Account.Where(x => x.ServiceRateID.ToUpper() == dhdv.DichVu.MaDichVu.ToUpper()).ToList())
                                    {
                                        try
                                        {
                                            ChiTietMatHangDonHangModels m = new ChiTietMatHangDonHangModels();
                                            m.ID_MatHang = dhdv.ID_MatHang;
                                            m.ID_DonHang = ID_DonHang;
                                            m.MaVeDichVu = account.AccountCode;
                                            m.MaBookingDichVu = blp.BookingCode;
                                            m.MaDonHangDichVu = blp.InvoiceCode;
                                            m.HanSuDung = account.ExpirationDate;
                                            m.ID_DichVu = dhdv.ID_DichVu;
                                            lstct.Add(m);

                                        }
                                        catch (Exception ex)
                                        {

                                        }

                                    }
                                    dhData.TaoDSChiTietMatHangDonHang(dhdv.ID_ChiTietDonHang, ID_DonHang, lstct.Where(x => x.ID_MatHang == dhdv.ID_MatHang).ToList());
                                }
                                OBJ.status = true;
                                OBJ.msg = Config.THANH_CONG;
                            }
                            else
                            {

                            }
                        }

                        //}
                    }
                    else
                    {
                        OBJ.msg = Config.KHONG_CO_QUYEN_THAO_TAC;
                    }
                    string sJsonKetQua = JsonConvert.SerializeObject(OBJ);
                    response = Request.CreateResponse(HttpStatusCode.OK, sJsonKetQua);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }


        /**
         * Thanh toan don hang
         * @param: 
         * @result: response satus 201: tạo mới thành công, 401 Unauthorized, 500 Internal Server Error
         * @author: VTLan
         * **/

        public class AppGiaoHangOBJ
        {
            public AppGiaoHangOBJ() { }

            public bool status { get; set; }

            public string msg { get; set; }

            public List<GiaoHangOBJ> dataLichSuGiaoHang { get; set; }

        }
        [HttpPost]
        [Route("giaoHang")]
        public HttpResponseMessage giaoHang([FromBody] paramGiaoHang param)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            AppGiaoHangOBJ OBJ = new AppGiaoHangOBJ();
            OBJ.status = false;
            OBJ.msg = Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG;
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
                    GiaoHangOBJ GHOBJ = JsonConvert.DeserializeObject<GiaoHangOBJ>(param.dulieugiaohang);

                    string type = param.type;
                    int iddonhang = GHOBJ.iddonhang;
                    int idnhanvien = GHOBJ.idnhanvien;
                    GiaoHangOBJ giaohang = new GiaoHangOBJ();
                    giaohang.iddonhang = GHOBJ.iddonhang;
                    giaohang.idnhanvien = GHOBJ.idnhanvien;
                    giaohang.idquanly = GHOBJ.idquanly;
                    giaohang.ghichu = GHOBJ.ghichu;
                    DonHangData dhData = new DonHangData();
                    DonHangModels donhang = dhData.LayDonHang(GHOBJ.iddonhang);
                    if (GHOBJ.chitietgiaohang.Count > 0)
                    {
                        int ID_GiaoHang = dhData.ThemMoiGiaoHang(giaohang);
                        double slgiao = 0;
                        foreach (ChiTietGiaoHangOBJ ctgh in GHOBJ.chitietgiaohang)
                        {
                            try
                            {
                                int loai = 1;
                                if (type.Equals("save")) { loai = 2; }
                                if (type.Equals("giaohang")) { loai = 1; }
                                //kiem tra quyen vao giao hang
                                if (userinfo.IsAdmin)
                                {
                                    if (ctgh.soluonggiao > 0)
                                    {
                                        slgiao += ctgh.soluonggiao;
                                        if (donhang.trangthaigiaohang == 4)
                                        {
                                            //don hang da xu ly xong
                                            OBJ.status = false;
                                            OBJ.msg = Config.DON_HANG_DA_XU_LY;
                                        }
                                        else
                                        {

                                            if (dhData.GiaoHangDonHang(loai, GHOBJ.iddonhang, GHOBJ.idnhanvien, ctgh.idhang, ctgh.ghichu, ctgh.soluonggiao, ctgh.hinhthucban, GHOBJ.idquanly, ID_GiaoHang))
                                            {
                                                OBJ.status = true;
                                                OBJ.msg = Config.THANH_CONG;
                                            }
                                            else
                                            {
                                                OBJ.msg = Config.THONG_TIN_KHONG_DUNG;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    OBJ.msg = Config.KHONG_CO_QUYEN_THAO_TAC;
                                }
                            }
                            catch
                            {
                                OBJ.msg = Config.THONG_TIN_KHONG_DUNG;
                                break;
                            }
                        }
                        if (slgiao <= 0 && !OBJ.status)
                        {
                            OBJ.msg = Config.GIAO_HANG_KHONG_NHAP_SO_LUONG;
                        }
                    }
                    string sJsonKetQua = JsonConvert.SerializeObject(OBJ);
                    response = Request.CreateResponse(HttpStatusCode.OK, sJsonKetQua);

                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
        /**
         * tạo mới đơn hàng
         * @param: DonHangParam
         * @result: respose satus 201: tạo mới thành công, 401 Unauthorized, 500 Internal Server Error
         * @author: VTLan
         * **/
        [HttpPost]
        [Route("taoMoiDonHang")]
        public HttpResponseMessage addNewInvoice([FromBody] DonHangModels paramDH)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    return response;
                }
                else
                {
                    paramDH.idnhanvientao = userinfo.ID_QuanLy;
                    paramDH.idct = userinfo.ID_QLLH; // id công ty
                    paramDH.thoigiantao = DateTime.Now; // thời gian tao
                    NhanVienApp nvApp = new NhanVienApp();
                    DonHangData dhData = new DonHangData();
                    ChuongTrinhKhuyenMaiData ctkmData = new ChuongTrinhKhuyenMaiData();

                    NhanVienAppModels nhanVienInfor = nvApp.ThongTinNhanVienTheoID(paramDH.idnhanvien);
                    int sttDonHang = 0;
                    string macuoicung = dhData.GetMaThamChieu(paramDH.idct, "");
                    paramDH.mathamchieu = utilsCommon.genMaDonHang(nhanVienInfor, macuoicung, nhanVienInfor.idnhom.ToString(), nhanVienInfor.MaNhom, nhanVienInfor.STT_DonHang);


                    DonHangModels DHThemOBJ = dhData.TaoDonHang(paramDH);
                    if (DHThemOBJ.iddonhang > 0 && DHThemOBJ.tongtien >= 0)
                    {
                        DataTable dtCTKM = ctkmData.GetChiTietHangTang(paramDH.idctkm);
                        //KhuyenMai_dl kmdl = new KhuyenMai_dl();
                        //KhuyenMai km = kmdl.GetKhuyenMaiByID(paramDH.idctkm);
                        //if (km != null)
                        //{
                        //    if (km.Loai == 10 && km.TongTienDatKM_Tu <= paramDH.tongtien)
                        //    {
                        //        foreach (DataRow dr in dtCTKM.Rows)
                        //        {
                        //            try
                        //            {
                        //                ChiTietDonHangModels newOBJ = new ChiTietDonHangModels();
                        //                newOBJ.iddonhang = DHThemOBJ.iddonhang;
                        //                newOBJ.idhanghoa = int.Parse(dr["ID_HangHoa"].ToString());
                        //                newOBJ.soluong = float.Parse(dr["SoLuong"].ToString());
                        //                newOBJ.hinhthucban = 2;
                        //                newOBJ.HangKhuyenMai = 1;
                        //                newOBJ.giaban = 0;
                        //                int ApDungBoiSo = dr["ApDungBoiSo"].ToString() != "" ? int.Parse(dr["ApDungBoiSo"].ToString()) : 0;

                        //                float TongTienDatKM_Tu = dr["TongTienDatKM_Tu"].ToString() != "" ? float.Parse(dr["TongTienDatKM_Tu"].ToString()) : 0;

                        //                float TongTienDatKM_Den = dr["TongTienDatKM_Den"].ToString() != "" ? float.Parse(dr["TongTienDatKM_Den"].ToString()) : 0;
                        //                int bs = 1;


                        //                if (TongTienDatKM_Den > 0 && paramDH.tongtien >= TongTienDatKM_Tu && paramDH.tongtien <= TongTienDatKM_Den && TongTienDatKM_Tu > 0)
                        //                {

                        //                    if (ApDungBoiSo > 0)
                        //                    {
                        //                        bs = (int)(paramDH.tongtien / (double)TongTienDatKM_Tu);

                        //                    }
                        //                }
                        //                else if (TongTienDatKM_Den == 0 && paramDH.tongtien >= TongTienDatKM_Tu && TongTienDatKM_Tu > 0)
                        //                {
                        //                    if (ApDungBoiSo > 0)
                        //                    {
                        //                        bs = (int)(paramDH.tongtien / (double)TongTienDatKM_Tu);

                        //                    }
                        //                }
                        //                if (bs == 0)
                        //                    bs = 1;


                        //                newOBJ.soluong = float.Parse(dr["SoLuong"].ToString()) * (int)bs;

                        //                int idChiTietDH = dhData.TaoChiTietDonHang(newOBJ);
                        //            }
                        //            catch (Exception ex)
                        //            {
                        //                LSPos_Data.Utilities.Log.Error(ex);
                        //            }
                        //        }
                        //    }
                        //}
                        dhData.CapNhatSTT_DonHang(nhanVienInfor.idnhom.ToString(), sttDonHang + 1);
                        foreach (ChiTietDonHangModels ctdhobj in paramDH.chitietdonhang)
                        {
                            try
                            {
                                ctdhobj.iddonhang = DHThemOBJ.iddonhang;
                                int ID_ChiTietDonHang = dhData.TaoChiTietDonHang(ctdhobj);
                                if (ID_ChiTietDonHang > 0)
                                {
                                    thongTinChiTietDonHang(DHThemOBJ, ctdhobj, dtCTKM, ctkmData, dhData, ID_ChiTietDonHang);
                                    if (ctdhobj.dschitietmathang != null)
                                        dhData.TaoDSChiTietMatHangDonHang(ID_ChiTietDonHang, DHThemOBJ.iddonhang, ctdhobj.dschitietmathang);
                                }
                            }
                            catch (Exception ex)
                            {
                                LSPos_Data.Utilities.Log.Error(ex);
                            }
                        }
                        response = Request.CreateResponse(HttpStatusCode.Created, DHThemOBJ);
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.InternalServerError, Config.KHONG_CO_DU_LIEU);
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }


        [HttpPost]
        [Route("taoMoiVaThanhToanDonHang")]
        public HttpResponseMessage addAndPaymentNewInvoice([FromBody] DonHangModels paramDH)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    return response;
                }
                else
                {
                    // bắt đầu tạo đơn                    
                    paramDH.idnhanvientao = userinfo.ID_QuanLy;
                    paramDH.idct = userinfo.ID_QLLH; // id công ty
                    paramDH.thoigiantao = DateTime.Now; // thời gian tao
                    NhanVienApp nvApp = new NhanVienApp();
                    //KhachHang kh = new KhachHang_dl().GetKhachHangTheoMa(userinfo.ID_QLLH, paramDH.macuahang, 0);
                    DonHangData dhData = new DonHangData();
                    ChuongTrinhKhuyenMaiData ctkmData = new ChuongTrinhKhuyenMaiData();
                    ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();
                    NhanVienAppModels nhanVienInfor = nvApp.ThongTinNhanVienTheoID(paramDH.idnhanvien);
                    int sttDonHang = 0;
                    string macuoicung = dhData.GetMaThamChieu(paramDH.idct, "");
                    paramDH.mathamchieu = utilsCommon.genMaDonHang(nhanVienInfor, macuoicung, nhanVienInfor.idnhom.ToString(), nhanVienInfor.MaNhom, nhanVienInfor.STT_DonHang);

                    //if (string.IsNullOrWhiteSpace(paramDH.macuahang))
                    //{
                    //    CommonRepository commonRe = new CommonRepository();
                    //    Account a = commonRe.GetAccount(paramDH.macuahang);
                    //    paramDH.LS_AccountCode = a.AccountCode;
                    //}

                    DonHangModels DHThemOBJ = dhData.TaoDonHang(paramDH);
                    if (DHThemOBJ.iddonhang > 0 && DHThemOBJ.tongtien >= 0)
                    {
                        dhData.CapNhatSTT_DonHang(nhanVienInfor.idnhom.ToString(), sttDonHang + 1);
                        List<string> lstSiteCode = new List<string>();
                        foreach (ChiTietDonHangModels ctdhobj in paramDH.chitietdonhang)
                        {
                            try
                            {
                                ctdhobj.iddonhang = DHThemOBJ.iddonhang;
                                int ID_ChiTietDonHang = dhData.TaoChiTietDonHang(ctdhobj);
                                if (ID_ChiTietDonHang > 0)
                                {
                                    ctdhobj.idchitietdonhang = ID_ChiTietDonHang;
                                    if (ctdhobj.dschitietmathang != null)
                                        dhData.TaoDSChiTietMatHangDonHang(ID_ChiTietDonHang, DHThemOBJ.iddonhang, ctdhobj.dschitietmathang);
                                    if (ctdhobj.lstDichVu != null)
                                    {
                                        foreach (HangHoa_DichVuModel hhdv in ctdhobj.lstDichVu)
                                        {
                                            ChiTietMatHang_DichVuModel it = new ChiTietMatHang_DichVuModel();
                                            it.ID_DonHang = DHThemOBJ.iddonhang;
                                            it.ID_ChiTietDonHang = ctdhobj.idchitietdonhang;
                                            it.ID_MatHang = hhdv.ID_HangHoa;
                                            it.ID_DichVu = hhdv.ID_DichVu;
                                            it.SoLuong = hhdv.SoLuong * (int)ctdhobj.soluong;
                                            it.GiaBan = hhdv.GiaBan;
                                            it.Loai = hhdv.Loai;
                                            ctmhdao.InsertOrUpdate(it);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LSPos_Data.Utilities.Log.Error(ex);
                            }
                        }
                        DonHangModels donHangCu = dhData.LayDonHang(DHThemOBJ.iddonhang);
                        //kết thúc tạo đơn

                        //bắt đầu thanh toán                        
                        dhData.ThanhToanDonHang(DHThemOBJ.iddonhang, paramDH.tongtien, userinfo.IsAdmin ? 0 : userinfo.ID_QuanLy, userinfo.IsAdmin ? userinfo.ID_QuanLy : 0, "", "", paramDH.hinhthucthanhtoan);
                        donHangCu = dhData.LayDonHang(DHThemOBJ.iddonhang);
                        bool paymentSuccess = false;
                        if (paramDH.hinhthucthanhtoan != 4 && paramDH.hinhthucthanhtoan != 7 && paramDH.hinhthucthanhtoan != 8 && paramDH.hinhthucthanhtoan != 9)
                        {
                            if (dhData.ThanhToanDonHang(DHThemOBJ.iddonhang, paramDH.tongtien, userinfo.IsAdmin ? 0 : userinfo.ID_QuanLy, userinfo.IsAdmin ? userinfo.ID_QuanLy : 0, "", "", paramDH.hinhthucthanhtoan))
                            {
                                paymentSuccess = true;
                            }
                        }
                        else
                        {
                            if (dhData.BienDongSoDuNhomTaiKhoan(nhanVienInfor.idnhom, DateTime.Now, -1, DHThemOBJ.iddonhang, (float)paramDH.tongtien, 0, ""))
                            {
                                //float sodu = dhData.BienDongSoDuNhom_TongTienHienTai(nhanVienInfor.idnhom);
                                //if (sodu >= paramDH.tongtien)
                                //{
                                if (dhData.BienDongSoDuNhom_UpdateTrangThaiThanhToan(userinfo.ID_QuanLy, DHThemOBJ.iddonhang)
                                    && dhData.ThanhToanDonHang(DHThemOBJ.iddonhang, paramDH.tongtien, userinfo.IsAdmin ? 0 : userinfo.ID_QuanLy, userinfo.IsAdmin ? userinfo.ID_QuanLy : 0, "", "", paramDH.hinhthucthanhtoan))
                                {
                                    paymentSuccess = true;
                                }
                                //}
                                //else
                                //{
                                //    response = Request.CreateResponse(HttpStatusCode.Created, new { status = false, msg = "Số dư trong ví không đủ để thực hiện thanh toán! Vui lòng nạp thêm!" });
                                //}
                            }
                            else
                            {
                                response = Request.CreateResponse(HttpStatusCode.Created, new { status = false, msg = "Không thể sử dụng ví" });
                            }
                        }
                        //kết thúc thanh toán

                        //bắt đầu xuất vé
                        string bookingcode = "";
                        if (donHangCu.trangthaithanhtoan == 4 && donHangCu.trangthaidonhang == 2)
                        {
                            DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                            List<DonHang_DichVuRequestAPIModel> lstdv = dvdao.GetAllByDonHang(donHangCu.iddonhang);
                            List<SiteModel> lstSite = lstdv.Select(x => x.Site).ToList();
                            List<SiteModel> lstSitedistinct = new List<SiteModel>();
                            foreach (SiteModel s in lstSite)
                            {
                                if (lstSitedistinct.Where(x => x.SiteCode == s.SiteCode).FirstOrDefault() == null)
                                {
                                    lstSitedistinct.Add(s);
                                }
                            }
                            foreach (SiteModel site in lstSitedistinct)
                            {
                                List<DonHang_DichVuRequestAPIModel> models = lstdv.Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                                BookingUtilities.XuLyXuatVePOS(models, site, donHangCu.iddonhang, 0, _bookingRepository, _sellRepository);
                                //BookingUtilities.XuLyXuatVe(models, site, donHangCu.iddonhang, 0, _bookingRepository, _sellRepository);
                                //BookingUtilities.XuLyXuatVe(models, site, donHangCu.iddonhang, userinfo.ID_QuanLy, _bookingRepository, _sellRepository);
                                //DonHang_DichVuRequestAPIModel dichvudacbiet = models.Where(x => !string.IsNullOrEmpty(x.ProfileCode)).FirstOrDefault();
                                //if (dichvudacbiet != null)
                                //{
                                //    new GlobalUtilities().SetGlobal(site.ApiUrl, site.UserName, site.Password, site.SiteCode, dichvudacbiet.ProfileID, dichvudacbiet.ProfileCode, dichvudacbiet.MemberID);
                                //}
                                //SaveBookingModel saveBooking = ConfirmBookingToLocalAPI(models);
                                //if (saveBooking != null)
                                //{
                                //    foreach (DonHang_DichVuRequestAPIModel model in models)
                                //    {
                                //        ChiTietMatHang_DichVuModel ct = new ChiTietMatHang_DichVuModel();
                                //        ct.ID = model.ID;
                                //        ct.BookingCode = saveBooking.BookingCode;
                                //        ct.InvoiceCode = saveBooking.InvoiceCode;
                                //        ct.SoLuong = model.SoLuong;
                                //        ctmhdao.InsertOrUpdate(ct);

                                //        ChiTietDonHangModels chitiet = paramDH.chitietdonhang.Where(x => x.idhanghoa == model.ID_MatHang).FirstOrDefault();
                                //        List<ChiTietMatHangDonHangModels> lstct = new List<ChiTietMatHangDonHangModels>();
                                //        BookingRes bk = _bookingRepository.Detail(saveBooking.BookingCode);
                                //        saveBooking.Accounts = bk.Account;
                                //        foreach (BookingAccountResponse account in saveBooking.Accounts.Where(x => x.ServiceRateID.ToUpper() == model.DichVu.MaDichVu.ToUpper()).ToList())
                                //        {
                                //            try
                                //            {
                                //                ChiTietMatHangDonHangModels m = new ChiTietMatHangDonHangModels();
                                //                m.ID_MatHang = model.ID_MatHang;
                                //                m.ID_DonHang = donHangCu.iddonhang;
                                //                m.MaVeDichVu = account.AccountCode;
                                //                m.MaBookingDichVu = saveBooking.BookingCode;
                                //                m.MaDonHangDichVu = saveBooking.InvoiceCode;
                                //                m.HanSuDung = account.ExpirationDate;
                                //                m.ID_DichVu = model.ID_DichVu;
                                //                lstct.Add(m);
                                //                bookingcode = saveBooking.BookingCode;
                                //            }
                                //            catch (Exception ex)
                                //            {

                                //            }

                                //        }
                                //        dhData.TaoDSChiTietMatHangDonHang(model.ID_ChiTietDonHang, donHangCu.iddonhang, lstct.Where(x => x.ID_MatHang == model.ID_MatHang).ToList());
                                //    }
                                //}
                                //else
                                //{

                                //}
                            }

                        }
                        //kết thúc xuất vé
                        //List<ChiTietDonHangModels> data = dhData.LayChiTietDonHang(DHThemOBJ.iddonhang, "vi");
                        LSPos_Data.Utilities.Log.Info(nhanVienInfor.tendangnhap + " - " + donHangCu.iddonhang + " - " + donHangCu.mathamchieu);
                        response = Request.CreateResponse(HttpStatusCode.Created, new { mabooking = bookingcode, iddonhang = donHangCu.iddonhang });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.InternalServerError, Config.KHONG_CO_DU_LIEU);
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpPost]
        [Route("xuatveota")]
        public HttpResponseMessage addAndPaymentNewway([FromBody] DonHangModels paramDH)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    return response;
                }
                else
                {
                    // bắt đầu tạo đơn                    
                    paramDH.idnhanvientao = userinfo.ID_QuanLy;
                    paramDH.idct = userinfo.ID_QLLH; // id công ty
                    paramDH.thoigiantao = DateTime.Now; // thời gian tao
                    NhanVienApp nvApp = new NhanVienApp();
                    DonHangData dhData = new DonHangData();
                    ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();
                    //NhanVienAppModels nhanVienInfor = nvApp.ThongTinNhanVienTheoID(paramDH.idnhanvien);
                    NhanVienAppModels nhanVienInfor = nvApp.ThongTinNhanVienTheoTenDangNhap(userinfo.Username, userinfo.ID_QLLH);
                    int sttDonHang = 0;
                    string macuoicung = dhData.GetMaThamChieu(paramDH.idct, "");
                    paramDH.mathamchieu = utilsCommon.genMaDonHang(nhanVienInfor, macuoicung, nhanVienInfor.idnhom.ToString(), nhanVienInfor.MaNhom, nhanVienInfor.STT_DonHang);

                    DonHangModels DHThemOBJ = dhData.TaoDonHang(paramDH);
                    if (DHThemOBJ.iddonhang > 0 && DHThemOBJ.tongtien >= 0)
                    {
                        dhData.CapNhatSTT_DonHang(nhanVienInfor.idnhom.ToString(), sttDonHang + 1);
                        List<string> lstSiteCode = new List<string>();
                        foreach (ChiTietDonHangModels ctdhobj in paramDH.chitietdonhang)
                        {
                            try
                            {
                                ctdhobj.iddonhang = DHThemOBJ.iddonhang;
                                int ID_ChiTietDonHang = dhData.TaoChiTietDonHang(ctdhobj);
                                if (ID_ChiTietDonHang > 0)
                                {
                                    ctdhobj.idchitietdonhang = ID_ChiTietDonHang;
                                    if (ctdhobj.dschitietmathang != null)
                                        dhData.TaoDSChiTietMatHangDonHang(ID_ChiTietDonHang, DHThemOBJ.iddonhang, ctdhobj.dschitietmathang);
                                    if (ctdhobj.lstDichVu != null)
                                    {
                                        foreach (HangHoa_DichVuModel hhdv in ctdhobj.lstDichVu)
                                        {
                                            ChiTietMatHang_DichVuModel it = new ChiTietMatHang_DichVuModel();
                                            it.ID_DonHang = DHThemOBJ.iddonhang;
                                            it.ID_ChiTietDonHang = ctdhobj.idchitietdonhang;
                                            it.ID_MatHang = hhdv.ID_HangHoa;
                                            it.ID_DichVu = hhdv.ID_DichVu;
                                            it.SoLuong = hhdv.SoLuong * (int)ctdhobj.soluong;
                                            it.GiaBan = hhdv.GiaBan;
                                            it.Loai = hhdv.Loai;
                                            ctmhdao.InsertOrUpdate(it);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LSPos_Data.Utilities.Log.Error(ex);
                            }
                        }
                        DonHangModels donHangCu = dhData.LayDonHang(DHThemOBJ.iddonhang);
                        //kết thúc tạo đơn

                        //bắt đầu thanh toán
                        bool paymentSuccess = false;
                        if (paramDH.hinhthucthanhtoan != 4)
                        {
                            if (dhData.ThanhToanDonHang(DHThemOBJ.iddonhang, paramDH.tongtien, userinfo.IsAdmin ? 0 : userinfo.ID_QuanLy, userinfo.IsAdmin ? userinfo.ID_QuanLy : 0, "", "", paramDH.hinhthucthanhtoan))
                            {
                                paymentSuccess = true;
                            }
                        }
                        else
                        {
                            if (dhData.BienDongSoDuNhomTaiKhoan(nhanVienInfor.idnhom, DateTime.Now, -1, DHThemOBJ.iddonhang, (float)paramDH.tongtien, 0, ""))
                            {
                                float sodu = dhData.BienDongSoDuNhom_TongTienHienTai(nhanVienInfor.idnhom);
                                if (sodu >= paramDH.tongtien)
                                {
                                    if (dhData.BienDongSoDuNhom_UpdateTrangThaiThanhToan(userinfo.ID_QuanLy, DHThemOBJ.iddonhang)
                                        && dhData.ThanhToanDonHang(DHThemOBJ.iddonhang, paramDH.tongtien, userinfo.IsAdmin ? 0 : userinfo.ID_QuanLy, userinfo.IsAdmin ? userinfo.ID_QuanLy : 0, "", "", paramDH.hinhthucthanhtoan))
                                    {
                                        paymentSuccess = true;
                                    }
                                }
                                else
                                {
                                    response = Request.CreateResponse(HttpStatusCode.Created, new { status = false, msg = "Số dư trong ví không đủ để thực hiện thanh toán! Vui lòng nạp thêm!" });
                                }
                            }
                            else
                            {
                                response = Request.CreateResponse(HttpStatusCode.Created, new { status = false, msg = "Không thể sử dụng ví" });
                            }
                        }
                        donHangCu = dhData.LayDonHang(DHThemOBJ.iddonhang);
                        //kết thúc thanh toán

                        if (paymentSuccess)
                        {
                            //bắt đầu xuất vé
                            string bookingcode = "";
                            if (donHangCu.trangthaithanhtoan == 4 && donHangCu.trangthaidonhang == 2)
                            {
                                DonHang_DichVuRequestAPIDAO dvdao = new DonHang_DichVuRequestAPIDAO();
                                List<DonHang_DichVuRequestAPIModel> lstdv = dvdao.GetAllByDonHang(donHangCu.iddonhang);
                                List<SiteModel> lstSite = lstdv.Select(x => x.Site).ToList();
                                List<SiteModel> lstSitedistinct = new List<SiteModel>();
                                foreach (SiteModel s in lstSite)
                                {
                                    if (lstSitedistinct.Where(x => x.SiteCode == s.SiteCode).FirstOrDefault() == null)
                                    {
                                        lstSitedistinct.Add(s);
                                    }
                                }
                                foreach (SiteModel site in lstSitedistinct)
                                {
                                    List<DonHang_DichVuRequestAPIModel> models = lstdv.Where(x => x.Site.SiteCode == site.SiteCode).ToList();
                                    BookingUtilities.XuLyXuatVePOS(models, site, donHangCu.iddonhang, 0, _bookingRepository, _sellRepository);
                                }

                            }
                            //kết thúc xuất vé
                            //List<ChiTietDonHangModels> data = dhData.LayChiTietDonHang(DHThemOBJ.iddonhang, "vi");
                            LSPos_Data.Utilities.Log.Info(nhanVienInfor.tendangnhap + " - " + donHangCu.iddonhang + " - " + donHangCu.mathamchieu);
                            response = Request.CreateResponse(HttpStatusCode.Created, new { mabooking = bookingcode, iddonhang = donHangCu.iddonhang });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.Created, new { status = false, msg = "Thanh toán đơn hàng không thành công" });
                        }
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.InternalServerError, Config.KHONG_CO_DU_LIEU);
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpPost]
        [Route("taoMoiDonHangDichVu")]
        public HttpResponseMessage addNewServiceInvoice([FromBody] DonHangModels paramDH)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    return response;
                }
                else
                {
                    paramDH.idnhanvientao = userinfo.ID_QuanLy;
                    paramDH.idct = userinfo.ID_QLLH; // id công ty
                    paramDH.thoigiantao = DateTime.Now; // thời gian tao
                    NhanVienApp nvApp = new NhanVienApp();
                    KhachHang kh = new KhachHang_dl().GetKhachHangID(paramDH.idcuahang);
                    DonHangData dhData = new DonHangData();
                    ChuongTrinhKhuyenMaiData ctkmData = new ChuongTrinhKhuyenMaiData();

                    NhanVienAppModels nhanVienInfor = nvApp.ThongTinNhanVienTheoID(paramDH.idnhanvien);
                    int sttDonHang = 0;
                    string macuoicung = dhData.GetMaThamChieu(paramDH.idct, "");
                    paramDH.mathamchieu = utilsCommon.genMaDonHang(nhanVienInfor, macuoicung, nhanVienInfor.idnhom.ToString(), nhanVienInfor.MaNhom, nhanVienInfor.STT_DonHang);

                    //if (string.IsNullOrWhiteSpace(paramDH.macuahang))
                    //{
                    //    CommonRepository commonRe = new CommonRepository();
                    //    Account a = commonRe.GetAccount(paramDH.macuahang);
                    //    paramDH.LS_AccountCode = a.AccountCode;
                    //}

                    //if (paramDH.tongtien <= dhData.BienDongSoDuNhom_TongTienHienTai(nhanVienInfor.idnhom))
                    //{
                    DonHangModels DHThemOBJ = dhData.TaoDonHang(paramDH);
                    //dhData.BienDongSoDuNhomTaiKhoan(nhanVienInfor.idnhom, paramDH.thoigiantao, -1, DHThemOBJ.iddonhang, (float)paramDH.tongtien, paramDH.trangthaithanhtoan, paramDH.ghichu);
                    if (DHThemOBJ.iddonhang > 0 && DHThemOBJ.tongtien >= 0)
                    {
                        DataTable dtCTKM = ctkmData.GetChiTietHangTang(paramDH.idctkm);
                        //KhuyenMai_dl kmdl = new KhuyenMai_dl();
                        //KhuyenMai km = kmdl.GetKhuyenMaiByID(paramDH.idctkm);
                        //if (km != null)
                        //{
                        //    if (km.Loai == 10 && km.TongTienDatKM_Tu <= paramDH.tongtien)
                        //    {
                        //    {
                        //        foreach (DataRow dr in dtCTKM.Rows)
                        //        {
                        //            try
                        //            {
                        //                ChiTietDonHangModels newOBJ = new ChiTietDonHangModels();
                        //                newOBJ.iddonhang = DHThemOBJ.iddonhang;
                        //                newOBJ.idhanghoa = int.Parse(dr["ID_HangHoa"].ToString());
                        //                newOBJ.soluong = float.Parse(dr["SoLuong"].ToString());
                        //                newOBJ.hinhthucban = 2;
                        //                newOBJ.HangKhuyenMai = 1;
                        //                newOBJ.giaban = 0;
                        //                int ApDungBoiSo = dr["ApDungBoiSo"].ToString() != "" ? int.Parse(dr["ApDungBoiSo"].ToString()) : 0;

                        //                float TongTienDatKM_Tu = dr["TongTienDatKM_Tu"].ToString() != "" ? float.Parse(dr["TongTienDatKM_Tu"].ToString()) : 0;

                        //                float TongTienDatKM_Den = dr["TongTienDatKM_Den"].ToString() != "" ? float.Parse(dr["TongTienDatKM_Den"].ToString()) : 0;
                        //                int bs = 1;


                        //                if (TongTienDatKM_Den > 0 && paramDH.tongtien >= TongTienDatKM_Tu && paramDH.tongtien <= TongTienDatKM_Den && TongTienDatKM_Tu > 0)
                        //                {

                        //                    if (ApDungBoiSo > 0)
                        //                    {
                        //                        bs = (int)(paramDH.tongtien / (double)TongTienDatKM_Tu);

                        //                    }
                        //                }
                        //                else if (TongTienDatKM_Den == 0 && paramDH.tongtien >= TongTienDatKM_Tu && TongTienDatKM_Tu > 0)
                        //                {
                        //                    if (ApDungBoiSo > 0)
                        //                    {
                        //                        bs = (int)(paramDH.tongtien / (double)TongTienDatKM_Tu);

                        //                    }
                        //                }
                        //                if (bs == 0)
                        //                    bs = 1;


                        //                newOBJ.soluong = float.Parse(dr["SoLuong"].ToString()) * (int)bs;

                        //                int idChiTietDH = dhData.TaoChiTietDonHang(newOBJ);
                        //            }
                        //            catch (Exception ex)
                        //            {
                        //                LSPos_Data.Utilities.Log.Error(ex);
                        //            }
                        //        }
                        //    }
                        //}
                        dhData.CapNhatSTT_DonHang(nhanVienInfor.idnhom.ToString(), sttDonHang + 1);
                        ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();
                        foreach (ChiTietDonHangModels ctdhobj in paramDH.chitietdonhang)
                        {
                            try
                            {
                                ctdhobj.iddonhang = DHThemOBJ.iddonhang;
                                int ID_ChiTietDonHang = dhData.TaoChiTietDonHang(ctdhobj);
                                if (ID_ChiTietDonHang > 0)
                                {
                                    ctdhobj.idchitietdonhang = ID_ChiTietDonHang;
                                    thongTinChiTietDonHang(DHThemOBJ, ctdhobj, dtCTKM, ctkmData, dhData, ID_ChiTietDonHang);
                                    if (ctdhobj.dschitietmathang != null)
                                        dhData.TaoDSChiTietMatHangDonHang(ID_ChiTietDonHang, DHThemOBJ.iddonhang, ctdhobj.dschitietmathang);
                                    if (ctdhobj.lstDichVu != null)
                                    {
                                        foreach (HangHoa_DichVuModel hhdv in ctdhobj.lstDichVu)
                                        {
                                            ChiTietMatHang_DichVuModel it = new ChiTietMatHang_DichVuModel();
                                            it.ID_DonHang = DHThemOBJ.iddonhang;
                                            it.ID_ChiTietDonHang = ctdhobj.idchitietdonhang;
                                            it.ID_MatHang = hhdv.ID_HangHoa;
                                            it.ID_DichVu = hhdv.ID_DichVu;
                                            if (hhdv.Loai == 1) //Optional - tự nhập số lượng bằng tay
                                            {
                                                it.SoLuong = hhdv.SoLuong;
                                            }
                                            else if (hhdv.Loai == 2) // Fixed - số lượng cố định
                                            {
                                                it.SoLuong = hhdv.SoLuong * (int)ctdhobj.soluong;
                                            }
                                            it.GiaBan = hhdv.GiaBan;
                                            it.Loai = hhdv.Loai;
                                            ctmhdao.InsertOrUpdate(it);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LSPos_Data.Utilities.Log.Error(ex);
                            }
                        }
                        //dhData.ThanhToanDonHang(DHThemOBJ.iddonhang, paramDH.tongtien, userinfo.IsAdmin ? 0 : userinfo.ID_QuanLy, userinfo.IsAdmin ? userinfo.ID_QuanLy : 0, "");

                        //foreach (string siteCode in lstSiteCode)
                        //{
                        //    List<ChiTietDonHangModels> lstchitiet = paramDH.chitietdonhang.Where(x => x.isdichvu > 0 && x.sitecode == siteCode).ToList();
                        //    List<ServiceRateModel> lstServiceRateModel = (from i in lstchitiet select new ServiceRateModel((int)i.soluong, i.tenhang, i.servicerateid, (decimal)i.giaban)).ToList();
                        //    SiteDAO sdao = new SiteDAO();
                        //    SiteModel site = sdao.GetSite(siteCode);
                        //    new GlobalUtilities().SetGlobal(site.ApiUrl, site.UserName, site.Password, site.SiteCode, site.ProfileID, site.ProfileCode);

                        //    SaveBookingModel booking = BookingToLocalAPI(lstServiceRateModel, kh);
                        //    if (booking != null)
                        //    {
                        //        paramDH.LS_BookingCode = booking.BookingCode;
                        //        List<ChiTietMatHangDonHangModels> lst = new List<ChiTietMatHangDonHangModels>();
                        //        //Update chitietdonhang
                        //        foreach (BookingAccountResponse account in booking.Accounts)
                        //        {
                        //            try
                        //            {
                        //                List<ChiTietMatHangDonHangModels> lstct = new List<ChiTietMatHangDonHangModels>();

                        //                ChiTietMatHangDonHangModels m = new ChiTietMatHangDonHangModels();
                        //                ChiTietDonHangModels ct = lstchitiet.Find(x => x.servicerateid == account.ServiceRateID);
                        //                m.ID_MatHang = ct.idhanghoa;
                        //                m.ID_DonHang = DHThemOBJ.iddonhang;
                        //                m.MaVeDichVu = account.AccountCode;
                        //                m.MaBookingDichVu = booking.BookingCode;
                        //                lstct.Add(m);
                        //                lst.Add(m);
                        //                ct.dschitietmathang = lstct;
                        //                dhData.TaoDSChiTietMatHangDonHang(ct.idchitietdonhang, DHThemOBJ.iddonhang, lstct);

                        //            }
                        //            catch (Exception ex)
                        //            {

                        //            }
                        //        }
                        //        // Send email
                        //    }
                        //}
                        DonHangv2 dh = dhData.GetDonHangTheoID_v2(DHThemOBJ.iddonhang, userinfo.ID_QuanLy);
                        dh.chitietdonhang = new DonHang_DichVuRequestAPIDAO().GetAllByDonHang(DHThemOBJ.iddonhang);
                        dh.NhanVien = new NhanVien_dl().GetNVTheoID(dh.ID_NhanVien);
                        //dh.lichsuthanhtoan = new DonHang_dl().GetLichSuThanhToan(DHThemOBJ.iddonhang);
                        EmailHelper helper = new EmailHelper();
                        string path = "";
                        string bodyTemplate = "";
                        var html = "";
                        if (userinfo.Username.Contains("pchl"))
                        {
                            path = HttpContext.Current.Server.MapPath("~/EmailTemplate/InvoiceCreatedPCHL.html");
                            bodyTemplate = System.IO.File.ReadAllText(path);
                            html = Engine.Razor.RunCompile(bodyTemplate, "MailIInvoiceCreatedPCHL", dh.GetType(), dh);
                        }
                        else
                        {
                            path = HttpContext.Current.Server.MapPath("~/EmailTemplate/InvoiceCreated.html");
                            bodyTemplate = System.IO.File.ReadAllText(path);
                            html = Engine.Razor.RunCompile(bodyTemplate, "MailIInvoiceCreated", dh.GetType(), dh);
                        }
                        //helper.SendEmail(html.ToString(), kh.Email, null, "[THÔNG BÁO] XÁC NHẬN ĐƠN " + dh.MaThamChieu + " KHỞI TẠO!");
                        response = Request.CreateResponse(HttpStatusCode.Created, DHThemOBJ);
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.InternalServerError, Config.KHONG_CO_DU_LIEU);
                    }
                    //}
                    //else
                    //{
                    //    response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Số dư không đủ!!! Vui lòng nạp thêm.");
                    //}

                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("taoMoiDonHangDichVuLanding")]
        public HttpResponseMessage addNewServiceInvoiceFromLanding([FromBody] TaoDonHangLandingModel model)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = new User_dl().GetUserInfo(model.username, model.company);
                NhanVienApp nvApp = new NhanVienApp();
                NhanVienAppModels nhanVienInfor = nvApp.ThongTinNhanVienTheoTenDangNhap(model.username, userinfo.ID_QLLH);
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    return response;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(model.khachhang.MaKH) || string.IsNullOrWhiteSpace(model.khachhang.SoDienThoaiMacDinh))
                    {
                        response = Request.CreateResponse(HttpStatusCode.InternalServerError, Config.KHONG_CO_DU_LIEU);
                        return response;
                    }
                    DonHangModels paramDH = model.donhang;
                    paramDH.idnhanvien = nhanVienInfor.idnhanvien;
                    paramDH.idnhanvientao = nhanVienInfor.idnhanvien;
                    paramDH.thoigiantao = DateTime.Now;
                    KhachHang_dl kh_dl = new KhachHang_dl();
                    KhachHang khachhangobj = kh_dl.GetKhachHangTheoTenSDT(userinfo.ID_QLLH, model.khachhang.MaKH, "", model.khachhang.SoDienThoaiMacDinh);
                    int Id = 0;
                    if (khachhangobj != null)
                    {
                        Id = khachhangobj.IDKhachHang;
                        model.khachhang.IDKhachHang = Id;
                        model.khachhang.IDQLLH = khachhangobj.IDQLLH;
                        model.khachhang.Ten = khachhangobj.Ten;
                        model.khachhang.MaKH = khachhangobj.MaKH;
                        model.khachhang.KinhDo = khachhangobj.KinhDo;
                        model.khachhang.ViDo = khachhangobj.ViDo;
                        model.khachhang.ID_NhanVien = khachhangobj.ID_NhanVien;
                        model.khachhang.MaSoThue = khachhangobj.MaSoThue;
                        model.khachhang.DiaChi = khachhangobj.DiaChi;
                        model.khachhang.ID_Tinh = khachhangobj.ID_Tinh;
                        model.khachhang.ID_Quan = khachhangobj.ID_Quan;
                        model.khachhang.ID_Phuong = khachhangobj.ID_Phuong;
                        model.khachhang.SoDienThoai = khachhangobj.SoDienThoai;
                        model.khachhang.SoDienThoai2 = khachhangobj.SoDienThoai2;
                        model.khachhang.SoDienThoai3 = khachhangobj.SoDienThoai3;
                        model.khachhang.SoDienThoaiMacDinh = khachhangobj.SoDienThoaiMacDinh;
                        model.khachhang.NguoiLienHe = khachhangobj.NguoiLienHe;
                        model.khachhang.SoTKNganHang = khachhangobj.SoTKNganHang;
                        model.khachhang.Email = khachhangobj.Email;
                        model.khachhang.Fax = khachhangobj.Fax;
                        model.khachhang.Website = khachhangobj.Website;
                        model.khachhang.GhiChu = khachhangobj.GhiChu;
                        model.khachhang.DuongPho = khachhangobj.DuongPho;
                        model.khachhang.ID_QuanLy = khachhangobj.ID_QuanLy;
                        model.khachhang.ID_LoaiKhachHang = khachhangobj.ID_LoaiKhachHang;
                        model.khachhang.ID_NhomKH = khachhangobj.ID_NhomKH;
                        model.khachhang.ID_KhuVuc = khachhangobj.ID_KhuVuc;
                        model.khachhang.ID_Cha = khachhangobj.ID_Cha;
                        model.khachhang.DiaChiXuatHoaDon = khachhangobj.DiaChiXuatHoaDon;
                        model.khachhang.ID_LoaiKhachHang = nhanVienInfor.ID_NhomKhachHang_MacDinh;
                        kh_dl.UpdateKhachHang(model.khachhang, userinfo.ID_QuanLy);
                    }
                    else
                    {
                        model.khachhang.ID_LoaiKhachHang = nhanVienInfor.ID_NhomKhachHang_MacDinh;
                        Id = kh_dl.ThemKhachHangv2(model.khachhang, userinfo.ID_QuanLy);
                    }
                    nvApp.CapNhatKhachHangChoNhanVien(nhanVienInfor.idnhanvien, Id);
                    KhachHang kh = new KhachHang_dl().GetKhachHangID(Id);
                    paramDH.idcuahang = Id;
                    paramDH.idct = kh.IDQLLH;
                    DonHangData dhData = new DonHangData();
                    ChuongTrinhKhuyenMaiData ctkmData = new ChuongTrinhKhuyenMaiData();


                    int sttDonHang = 0;
                    string macuoicung = dhData.GetMaThamChieu(paramDH.idct, "");
                    paramDH.mathamchieu = utilsCommon.genMaDonHang(nhanVienInfor, macuoicung, nhanVienInfor.idnhom.ToString(), nhanVienInfor.MaNhom, nhanVienInfor.STT_DonHang);
                    double TongTien = 0;
                    foreach (ChiTietDonHangModels ctdhobj in paramDH.chitietdonhang)
                    {
                        HangHoaDB hh = HangHoaDB.Get(ctdhobj.idhanghoa);
                        TongTien += ctdhobj.soluong * hh.GiaBanLe;
                    }
                    if(paramDH.tongtien < TongTien)
                    {
                        //response = Request.CreateResponse(HttpStatusCode.InternalServerError, Config.KHONG_CO_DU_LIEU);
                        //return response;
                        paramDH.tongtien = TongTien;
                    }
                    DonHangModels DHThemOBJ = dhData.TaoDonHang(paramDH);
                    if (DHThemOBJ.iddonhang > 0 && DHThemOBJ.tongtien >= 0)
                    {
                        DataTable dtCTKM = ctkmData.GetChiTietHangTang(paramDH.idctkm);
                        dhData.CapNhatSTT_DonHang(nhanVienInfor.idnhom.ToString(), sttDonHang + 1);
                        ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();
                        foreach (ChiTietDonHangModels ctdhobj in paramDH.chitietdonhang)
                        {
                            try
                            {
                                ctdhobj.iddonhang = DHThemOBJ.iddonhang;
                                int ID_ChiTietDonHang = ctdhobj.soluong > 0 ? dhData.TaoChiTietDonHang(ctdhobj) : 0;
                                if (ID_ChiTietDonHang > 0)
                                {
                                    ctdhobj.idchitietdonhang = ID_ChiTietDonHang;
                                    thongTinChiTietDonHang(DHThemOBJ, ctdhobj, dtCTKM, ctkmData, dhData, ID_ChiTietDonHang);
                                    if (ctdhobj.dschitietmathang != null)
                                        dhData.TaoDSChiTietMatHangDonHang(ID_ChiTietDonHang, DHThemOBJ.iddonhang, ctdhobj.dschitietmathang);
                                    ctdhobj.lstDichVu = new HangHoa_DichVuDAO().GetAllByHangHoa(ctdhobj.idhanghoa);
                                    if (ctdhobj.lstDichVu != null)
                                    {
                                        foreach (HangHoa_DichVuModel hhdv in ctdhobj.lstDichVu)
                                        {
                                            ChiTietMatHang_DichVuModel it = new ChiTietMatHang_DichVuModel();
                                            it.ID_DonHang = DHThemOBJ.iddonhang;
                                            it.ID_ChiTietDonHang = ctdhobj.idchitietdonhang;
                                            it.ID_MatHang = hhdv.ID_HangHoa;
                                            it.ID_DichVu = hhdv.ID_DichVu;
                                            if (hhdv.Loai == 1) //Optional - tự nhập số lượng bằng tay
                                            {
                                                it.SoLuong = hhdv.SoLuong;
                                            }
                                            else if (hhdv.Loai == 2) // Fixed - số lượng cố định
                                            {
                                                it.SoLuong = hhdv.SoLuong * (int)ctdhobj.soluong;
                                            }
                                            it.GiaBan = hhdv.GiaBan;
                                            it.Loai = hhdv.Loai;
                                            ctmhdao.InsertOrUpdate(it);
                                        }
                                    }
                                    else
                                    {
                                        LSPos_Data.Utilities.Log.Info(JsonConvert.SerializeObject(model));
                                        return Request.CreateResponse(HttpStatusCode.InternalServerError, Config.KHONG_CO_DU_LIEU);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LSPos_Data.Utilities.Log.Error(ex);
                            }
                        }
                        DonHangv2 dh = dhData.GetDonHangTheoID_v2(DHThemOBJ.iddonhang, userinfo.ID_QuanLy);
                        dh.chitietdonhang = new List<DonHang_DichVuRequestAPIModel>();
                        foreach (ChiTietDonHangModels ct in model.donhang.chitietdonhang)
                        {
                            DonHang_DichVuRequestAPIModel item = new DonHang_DichVuRequestAPIModel();
                            item.DichVu = new DichVuModel();
                            item.DichVu.TenDichVu = ct.tenhang;
                            item.SoLuong = (int)ct.soluong;
                            item.Ngay = ct.Ngay;
                            item.GiaBan = (decimal)ct.giaban;
                            dh.chitietdonhang.Add(item);
                        }
                        dh.NhanVien = new NhanVien_dl().GetNVTheoID(dh.ID_NhanVien);
                        try
                        {
                            EmailHelper helper = new EmailHelper();
                            string path = "";
                            string bodyTemplate = "";
                            var html = "";
                            //var htmlforOTA = "";

                            path = HttpContext.Current.Server.MapPath("~/EmailTemplate/InvoiceCreated.html");
                            bodyTemplate = System.IO.File.ReadAllText(path);
                            html = Engine.Razor.RunCompile(bodyTemplate, "MailIInvoiceCreated", dh.GetType(), dh);

                            //path = HttpContext.Current.Server.MapPath("~/EmailTemplate/InvoiceCreatedForOTA.html");
                            //bodyTemplate = System.IO.File.ReadAllText(path);
                            //htmlforOTA = Engine.Razor.RunCompile(bodyTemplate, "MailIInvoiceCreatedForOTA", dh.GetType(), dh);

                            //helper.SendEmail(html.ToString(), kh.Email, null, "[THÔNG BÁO] XÁC NHẬN ĐƠN " + dh.MaThamChieu + " KHỞI TẠO!");
                            //helper.SendEmail(htmlforOTA.ToString(), nhanVienInfor.Email, null, "[THÔNG BÁO] ĐƠN HÀNG " + dh.MaThamChieu + " ĐƯỢC TẠO TRÊN MÃ GIỚI THIỆU CỦA BẠN!");
                        }
                        catch (Exception ex)
                        {

                        }
                        response = Request.CreateResponse(HttpStatusCode.Created, dh);
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.InternalServerError, Config.KHONG_CO_DU_LIEU);
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        /**
         * thuc hien tra hang
         * @param: DonHangParam
         * @result: respose satus 201: tạo mới thành công, 401 Unauthorized, 500 Internal Server Error
         * @author: VTLan
         * **/
        [HttpPost]
        [Route("thucHienTraHang")]
        public HttpResponseMessage thucHienTraHang([FromBody] phieuTraHang paramDH)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    return response;
                }
                else
                {
                    NhanVienApp nvApp = new NhanVienApp();
                    DonHang_dl dhdl = new DonHang_dl();
                    int idPhieuTra = dhdl.ThemMoi_PhieuTraHang(paramDH.idDonHang, userinfo.ID_QuanLy, DateTime.Now, "");
                    if (idPhieuTra > 0)
                    {
                        List<int> result = new List<int>();
                        foreach (ChiTietHangTra ct in paramDH.chiTietHangTra)
                        {
                            double thanhtien = ct.giaban * ct.soluong;
                            int tmp = 0;
                            tmp = dhdl.ThemMoi_PhieuTraHang_ChiTiet(ct.idhanghoa, idPhieuTra, ct.soluong, ct.giaban, thanhtien, paramDH.idDonHang, ct.idLichSuGiaoHang, ct.idKho, ct.hinhthucban);
                            result.Add(tmp);
                        }
                        response = Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.InternalServerError, Config.KHONG_CO_DU_LIEU);
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        /**
         * sửa đơn hàng
         * @param: DonHangParam
         * @result: respose satus 201: tạo mới thành công, 401 Unauthorized, 500 Internal Server Error
         * @author: VTLan
         * **/
        [HttpPost]
        [Route("suaDonHang")]
        public HttpResponseMessage editInvoice([FromBody] DonHangParam paramDH)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotModified, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DonHangv2 dh = paramDH.donHang;

                    ////Chưa có CTKM models
                    //bool kmconhieuluc = false;

                    //if (dh.ID_CTKM > 0)
                    //{
                    //    //co ap dung CTKM
                    //    //check thoi gian va hieu luc CTKM
                    //     km = CTKMDB.LayCTKM_ByIDCTKM(dh.ID_CTKM);
                    //    if (km.trangthai != 0 && new DateTime(km.ngayapdung.Year, km.ngayapdung.Month, km.ngayapdung.Day) <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) && new DateTime(km.ngayketthuc.Year, km.ngayketthuc.Month, km.ngayketthuc.Day) >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                    //    {
                    //        //con hieu luc
                    //        kmconhieuluc = true;


                    //    }
                    //}

                    //bool KM_MatHang = false;
                    //foreach (ChiTietDonHangModels ctdhobj in paramDH.chiTietDonHang)
                    //{
                    //    try
                    //    {
                    //        if (ctdhobj.idctkm > 0)
                    //        {
                    //            CTKMOBJ kmmathang = CTKMDB.LayCTKM_ByIDCTKM(ctdhobj.idctkm);
                    //            if (kmmathang.trangthai != 0 && new DateTime(kmmathang.ngayapdung.Year, kmmathang.ngayapdung.Month, kmmathang.ngayapdung.Day) <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) && new DateTime(kmmathang.ngayketthuc.Year, kmmathang.ngayketthuc.Month, kmmathang.ngayketthuc.Day) >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                    //            {
                    //                //con hieu luc
                    //                kmconhieuluc = true;
                    //                KM_MatHang = true;
                    //            }
                    //            else
                    //            {
                    //                KM_MatHang = true;
                    //                kmconhieuluc = false;
                    //                break;
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        LSPos_Data.Utilities.Log.Error(ex);
                    //    }
                    //}

                    dh.ID_QLLH = userinfo.ID_QLLH;
                    dh.ProcessDate = DateTime.Today;
                    NhanVienApp nvApp = new NhanVienApp();
                    DonHangData dhData = new DonHangData();
                    DonHang_dl dhdl = new DonHang_dl();
                    ChuongTrinhKhuyenMaiData ctkmData = new ChuongTrinhKhuyenMaiData();

                    DonHangModels donHangCu = dhData.LayDonHang(dh.ID_DonHang);
                    //Utils.GhiLog(donHangCu.idct, donHangCu.idnhanvien, 0, "Cập nhật đơn hàng", "Cập nhật đơn hàng", "|Mã đơn hàng:" + donHangCu.mathamchieu + "|Dữ liệu:" + paramDH, "");

                    if (donHangCu != null && donHangCu.isProcess > 0)
                    {
                        LSPos_Data.Utilities.Log.Info("Loi don hang dang xu ly khong duoc sua : DHOBJ_CU.isProcess:" + donHangCu.isProcess);
                        response = Request.CreateResponse(HttpStatusCode.NotModified, "Loi don hang dang xu ly khong duoc sua : DHOBJ_CU.isProcess:" + donHangCu.isProcess);
                    }
                    else
                    {
                        if (dh.ID_DonHang > 0)
                        {
                            if (donHangCu.tiendathanhtoan > dh.TongTien && donHangCu.isProcess == 1)
                            {

                                response = Request.CreateResponse(HttpStatusCode.NotModified, Config.DON_HANG_KHONG_THE_XU_LY_VUOT_QUA_TIEN_THANH_TOAN);
                            }
                            else
                            {
                                int loi = 0;
                                int tontai = 0;

                                foreach (ChiTietDonHangModels ctdhobj in donHangCu.chitietdonhang)
                                {
                                    tontai = 0;
                                    foreach (ChiTietDonHangModels ctdhobj_New in paramDH.chiTietDonHang)
                                    {
                                        if (ctdhobj_New.idchitietdonhang == ctdhobj.idchitietdonhang)
                                        {
                                            tontai = 1;
                                            if (ctdhobj_New.soluong < ctdhobj.dagiao)
                                            {
                                                //log.Info("So luong sua nho hon so luong da giao ctdhobj_New.soluong: " + ctdhobj_New.soluong + " > ctdhobj.dagiao : " + ctdhobj.dagiao + " : DHOBJ_CU.isProcess:" + DHOBJ_CU.isProcess
                                                response = Request.CreateResponse(HttpStatusCode.NotModified, Config.DON_HANG_KHONG_THE_XU_LY_VUOT_QUA_SO_LUONG_GIAO);
                                                loi = 1;
                                                break;
                                            }
                                        }
                                    }

                                    if (tontai == 0)
                                    {
                                        if (ctdhobj.dagiao > 0)
                                        {
                                            //da giao khong duoc xoa
                                            response = Request.CreateResponse(HttpStatusCode.NotModified, Config.DON_HANG_KHONG_THE_XU_LY_XOA_MAT_HANG_DA_GIAO);
                                            loi = 1;
                                            break;
                                        }
                                    }

                                    if (loi == 1)
                                        break;
                                }

                                if (loi == 0)
                                {
                                    //dhData.XoaChiTietDonHangTang(dh.ID_DonHang);
                                    foreach (ChiTietDonHangModels ct in donHangCu.chitietdonhang)
                                    {
                                        try
                                        {

                                            dhData.XoaChiTietDonHang(ct);
                                        }
                                        catch (Exception ex)
                                        {
                                            LSPos_Data.Utilities.Log.Error(ex);
                                            //OBJ.msg = Config.THONG_TIN_KHONG_DUNG;
                                        }
                                    }

                                    dhData.SuaDonHang(dh, dh.ID_NhanVien, userinfo.ID_QuanLy);//sua don hang
                                    double tongtien = 0;
                                    tongtien = paramDH.donHang.TongTien;
                                    double tongtienchenhlech = donHangCu.tiendathanhtoan - dh.TongTien;
                                    ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();
                                    foreach (ChiTietDonHangModels ctdhobj in paramDH.chiTietDonHang)
                                    {
                                        if (ctdhobj.giaban > 0 && ctdhobj.tongTien > 0)
                                        {
                                            try
                                            {
                                                int ID_ChiTietDonHang = dhData.TaoChiTietDonHang(ctdhobj);
                                                if (ID_ChiTietDonHang > 0)
                                                {
                                                    thongTinChiTietDonHang(donHangCu, ctdhobj, null, ctkmData, dhData, ID_ChiTietDonHang);
                                                    if (ctdhobj.dschitietmathang != null)
                                                        dhData.TaoDSChiTietMatHangDonHang(ID_ChiTietDonHang, donHangCu.iddonhang, ctdhobj.dschitietmathang);
                                                    if (ctdhobj.lstDichVu != null)
                                                    {
                                                        foreach (HangHoa_DichVuModel hhdv in ctdhobj.lstDichVu)
                                                        {
                                                            ChiTietMatHang_DichVuModel it = new ChiTietMatHang_DichVuModel();
                                                            it.ID_DonHang = donHangCu.iddonhang;
                                                            it.ID_ChiTietDonHang = ctdhobj.idchitietdonhang;
                                                            it.ID_MatHang = hhdv.ID_HangHoa;
                                                            it.ID_DichVu = hhdv.ID_DichVu;
                                                            it.SoLuong = hhdv.SoLuong;
                                                            it.GiaBan = hhdv.GiaBan;
                                                            it.Loai = hhdv.Loai;
                                                            ctmhdao.InsertOrUpdate(it);
                                                        }
                                                    }


                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                LSPos_Data.Utilities.Log.Error(ex);
                                            }
                                        }
                                    }
                                    if (tongtienchenhlech > 0)
                                    {
                                        dhData.ThanhToanDonHang1phan(dh.ID_DonHang, tongtienchenhlech * (-1), userinfo.IsAdmin ? 0 : userinfo.ID_QuanLy, "Trả hàng", userinfo.ID_QuanLy, "", 2);
                                    }
                                    //foreach (ChiTietDonHangModels ct in paramDH.chiTietDonHang)
                                    //{
                                    //    try
                                    //    {

                                    //        if (ct.tongTien > 0)
                                    //        {
                                    //            dhData.SuaChiTietDonHang(ct);
                                    //            tongtien += ct.tongTien;
                                    //        }
                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //        LSPos_Data.Utilities.Log.Error(ex);
                                    //        //OBJ.msg = Config.THONG_TIN_KHONG_DUNG;
                                    //    }
                                    //}

                                    //DataTable dtCTKM = ctkmData.GetChiTietHangTang(dh.ID_CTKM);
                                    //KhuyenMai_dl kmdl = new KhuyenMai_dl();
                                    //KhuyenMai km = kmdl.GetKhuyenMaiByID(dh.ID_CTKM);
                                    //if (km != null)
                                    //{
                                    //    if (km.Loai == 10 && km.TongTienDatKM_Tu <= tongtien && (km.TongTienDatKM_Den == 0 || km.TongTienDatKM_Den >= tongtien))
                                    //    {
                                    //        foreach (DataRow dr in dtCTKM.Rows)
                                    //        {
                                    //            try
                                    //            {
                                    //                ChiTietDonHangModels newOBJ = new ChiTietDonHangModels();
                                    //                newOBJ.iddonhang = dh.ID_DonHang;
                                    //                newOBJ.idhanghoa = int.Parse(dr["ID_HangHoa"].ToString());

                                    //                newOBJ.hinhthucban = 2;
                                    //                newOBJ.HangKhuyenMai = 1;
                                    //                newOBJ.giaban = 0;


                                    //                int ApDungBoiSo = dr["ApDungBoiSo"].ToString() != "" ? int.Parse(dr["ApDungBoiSo"].ToString()) : 0;

                                    //                float TongTienDatKM_Tu = dr["TongTienDatKM_Tu"].ToString() != "" ? float.Parse(dr["TongTienDatKM_Tu"].ToString()) : 0;

                                    //                float TongTienDatKM_Den = dr["TongTienDatKM_Den"].ToString() != "" ? float.Parse(dr["TongTienDatKM_Den"].ToString()) : 0;
                                    //                int bs = 1;


                                    //                if (TongTienDatKM_Den > 0 && tongtien >= TongTienDatKM_Tu && tongtien <= TongTienDatKM_Den && TongTienDatKM_Tu > 0)
                                    //                {

                                    //                    if (ApDungBoiSo > 0)
                                    //                    {
                                    //                        bs = (int)(tongtien / (double)TongTienDatKM_Tu);

                                    //                    }
                                    //                }
                                    //                else if (TongTienDatKM_Den == 0 && tongtien >= TongTienDatKM_Tu && TongTienDatKM_Tu > 0)
                                    //                {
                                    //                    if (ApDungBoiSo > 0)
                                    //                    {
                                    //                        bs = (int)(tongtien / (double)TongTienDatKM_Tu);

                                    //                    }
                                    //                }
                                    //                if (bs == 0)
                                    //                    bs = 1;



                                    //                newOBJ.soluong = float.Parse(dr["SoLuong"].ToString()) * (int)bs;


                                    //                int idChiTietDH = dhData.TaoChiTietDonHang(newOBJ);
                                    //            }
                                    //            catch (Exception ex)
                                    //            {
                                    //                LSPos_Data.Utilities.Log.Error(ex);
                                    //            }
                                    //        }
                                    //    }
                                    //}

                                    response = Request.CreateResponse(HttpStatusCode.OK, dh.ID_DonHang);
                                }
                                else
                                {
                                    // OBJ.msg = Config.DON_HANG_DA_XU_LY_KHONG_SUA;
                                    response = Request.CreateResponse(HttpStatusCode.NotModified, Config.DON_HANG_KHONG_THE_XU_LY_XOA_MAT_HANG_DA_GIAO);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpPost]
        [Route("suaDonHangDV")]
        public HttpResponseMessage editServiceInvoice([FromBody] DonHangParam paramDH)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotModified, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    DonHangv2 dh = paramDH.donHang;

                    dh.ID_QLLH = userinfo.ID_QLLH;
                    dh.ProcessDate = DateTime.Today;
                    NhanVienApp nvApp = new NhanVienApp();
                    DonHangData dhData = new DonHangData();
                    DonHang_dl dhdl = new DonHang_dl();
                    ChuongTrinhKhuyenMaiData ctkmData = new ChuongTrinhKhuyenMaiData();

                    DonHangModels donHangCu = dhData.LayDonHang(dh.ID_DonHang);

                    if (donHangCu != null && donHangCu.isProcess > 0)
                    {
                        LSPos_Data.Utilities.Log.Info("Loi don hang dang xu ly khong duoc sua : DHOBJ_CU.isProcess:" + donHangCu.isProcess);
                        response = Request.CreateResponse(HttpStatusCode.NotModified, "Loi don hang dang xu ly khong duoc sua : DHOBJ_CU.isProcess:" + donHangCu.isProcess);
                    }
                    else
                    {
                        if (dh.ID_DonHang > 0)
                        {
                            //if (donHangCu.tiendathanhtoan > dh.TongTien && donHangCu.isProcess == 1)
                            if (donHangCu.tiendathanhtoan > 0)
                            {

                                response = Request.CreateResponse(HttpStatusCode.NotModified, Config.DON_HANG_KHONG_THE_XU_LY_DA_THANH_TOAN);
                            }
                            else
                            {
                                int loi = 0;
                                int tontai = 0;
                                if (loi == 0)
                                {
                                    //dhData.XoaChiTietDonHangTang(dh.ID_DonHang);
                                    foreach (ChiTietDonHangModels ct in donHangCu.chitietdonhang)
                                    {
                                        try
                                        {
                                            dhData.XoaChiTietDonHang(ct);
                                        }
                                        catch (Exception ex)
                                        {
                                            LSPos_Data.Utilities.Log.Error(ex);
                                            //OBJ.msg = Config.THONG_TIN_KHONG_DUNG;
                                        }
                                    }

                                    dhData.SuaDonHang(dh, dh.ID_NhanVien, userinfo.ID_QuanLy);//sua don hang
                                    double tongtien = 0;
                                    tongtien = paramDH.donHang.TongTien;
                                    double tongtienchenhlech = donHangCu.tiendathanhtoan - dh.TongTien;
                                    ChiTietMatHang_DichVuDAO ctmhdao = new ChiTietMatHang_DichVuDAO();
                                    foreach (ChiTietDonHangModels ctdhobj in paramDH.chiTietDonHang)
                                    {
                                        try
                                        {
                                            int ID_ChiTietDonHang = dhData.TaoChiTietDonHang(ctdhobj);
                                            if (ID_ChiTietDonHang > 0)
                                            {
                                                ctdhobj.idchitietdonhang = ID_ChiTietDonHang;
                                                thongTinChiTietDonHang(donHangCu, ctdhobj, null, ctkmData, dhData, ID_ChiTietDonHang);
                                                if (ctdhobj.dschitietmathang != null)
                                                    dhData.TaoDSChiTietMatHangDonHang(ID_ChiTietDonHang, donHangCu.iddonhang, ctdhobj.dschitietmathang);
                                                if (ctdhobj.lstDichVu != null)
                                                {
                                                    foreach (HangHoa_DichVuModel hhdv in ctdhobj.lstDichVu)
                                                    {
                                                        ChiTietMatHang_DichVuModel it = new ChiTietMatHang_DichVuModel();
                                                        it.ID_DonHang = donHangCu.iddonhang;
                                                        it.ID_ChiTietDonHang = ctdhobj.idchitietdonhang;
                                                        it.ID_MatHang = ctdhobj.idhanghoa;
                                                        it.ID_DichVu = hhdv.ID_DichVu;
                                                        it.SoLuong = hhdv.SoLuong * (int)ctdhobj.soluong;
                                                        it.GiaBan = hhdv.GiaBan;
                                                        it.Loai = hhdv.Loai;
                                                        ctmhdao.InsertOrUpdate(it);
                                                    }
                                                }


                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            LSPos_Data.Utilities.Log.Error(ex);
                                        }
                                    }
                                    if (tongtienchenhlech > 0)
                                    {
                                        dhData.ThanhToanDonHang1phan(dh.ID_DonHang, tongtienchenhlech * (-1), userinfo.IsAdmin ? 0 : userinfo.ID_QuanLy, "Trả hàng", userinfo.ID_QuanLy, "", 2);
                                    }
                                    //foreach (ChiTietDonHangModels ct in paramDH.chiTietDonHang)
                                    //{
                                    //    try
                                    //    {

                                    //        if (ct.tongTien > 0)
                                    //        {
                                    //            dhData.SuaChiTietDonHang(ct);
                                    //            tongtien += ct.tongTien;
                                    //        }
                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //        LSPos_Data.Utilities.Log.Error(ex);
                                    //        //OBJ.msg = Config.THONG_TIN_KHONG_DUNG;
                                    //    }
                                    //}

                                    //DataTable dtCTKM = ctkmData.GetChiTietHangTang(dh.ID_CTKM);
                                    //KhuyenMai_dl kmdl = new KhuyenMai_dl();
                                    //KhuyenMai km = kmdl.GetKhuyenMaiByID(dh.ID_CTKM);
                                    //if (km != null)
                                    //{
                                    //    if (km.Loai == 10 && km.TongTienDatKM_Tu <= tongtien && (km.TongTienDatKM_Den == 0 || km.TongTienDatKM_Den >= tongtien))
                                    //    {
                                    //        foreach (DataRow dr in dtCTKM.Rows)
                                    //        {
                                    //            try
                                    //            {
                                    //                ChiTietDonHangModels newOBJ = new ChiTietDonHangModels();
                                    //                newOBJ.iddonhang = dh.ID_DonHang;
                                    //                newOBJ.idhanghoa = int.Parse(dr["ID_HangHoa"].ToString());

                                    //                newOBJ.hinhthucban = 2;
                                    //                newOBJ.HangKhuyenMai = 1;
                                    //                newOBJ.giaban = 0;


                                    //                int ApDungBoiSo = dr["ApDungBoiSo"].ToString() != "" ? int.Parse(dr["ApDungBoiSo"].ToString()) : 0;

                                    //                float TongTienDatKM_Tu = dr["TongTienDatKM_Tu"].ToString() != "" ? float.Parse(dr["TongTienDatKM_Tu"].ToString()) : 0;

                                    //                float TongTienDatKM_Den = dr["TongTienDatKM_Den"].ToString() != "" ? float.Parse(dr["TongTienDatKM_Den"].ToString()) : 0;
                                    //                int bs = 1;


                                    //                if (TongTienDatKM_Den > 0 && tongtien >= TongTienDatKM_Tu && tongtien <= TongTienDatKM_Den && TongTienDatKM_Tu > 0)
                                    //                {

                                    //                    if (ApDungBoiSo > 0)
                                    //                    {
                                    //                        bs = (int)(tongtien / (double)TongTienDatKM_Tu);

                                    //                    }
                                    //                }
                                    //                else if (TongTienDatKM_Den == 0 && tongtien >= TongTienDatKM_Tu && TongTienDatKM_Tu > 0)
                                    //                {
                                    //                    if (ApDungBoiSo > 0)
                                    //                    {
                                    //                        bs = (int)(tongtien / (double)TongTienDatKM_Tu);

                                    //                    }
                                    //                }
                                    //                if (bs == 0)
                                    //                    bs = 1;



                                    //                newOBJ.soluong = float.Parse(dr["SoLuong"].ToString()) * (int)bs;


                                    //                int idChiTietDH = dhData.TaoChiTietDonHang(newOBJ);
                                    //            }
                                    //            catch (Exception ex)
                                    //            {
                                    //                LSPos_Data.Utilities.Log.Error(ex);
                                    //            }
                                    //        }
                                    //    }
                                    //}

                                    response = Request.CreateResponse(HttpStatusCode.OK, dh.ID_DonHang);
                                }
                                else
                                {
                                    // OBJ.msg = Config.DON_HANG_DA_XU_LY_KHONG_SUA;
                                    response = Request.CreateResponse(HttpStatusCode.NotModified, Config.DON_HANG_KHONG_THE_XU_LY_XOA_MAT_HANG_DA_GIAO);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        public static void thongTinChiTietDonHang(DonHangModels paramDH, ChiTietDonHangModels ctdhobj, DataTable dtCTKM, ChuongTrinhKhuyenMaiData ctkmData, DonHangData dhData, int ID_ChiTietDonHang_Moi)
        {
            //them chi tiet hang tang
            if (ctdhobj.idctkm > 0)
            {
                dtCTKM = ctkmData.GetChiTietHangTang(ctdhobj.idctkm);

                foreach (DataRow dr in dtCTKM.Rows)
                {
                    try
                    {
                        if (dr["ID_Hang"].ToString() == ctdhobj.idhanghoa.ToString())
                        {

                            ChiTietDonHangModels newOBJ = new ChiTietDonHangModels();
                            newOBJ.iddonhang = paramDH.iddonhang;
                            newOBJ.idhanghoa = int.Parse(dr["ID_HangHoa"].ToString());
                            newOBJ.soluong = float.Parse(dr["SoLuong"].ToString());
                            newOBJ.hinhthucban = 2;
                            newOBJ.HangKhuyenMai = 1;
                            newOBJ.giaban = 0;
                            newOBJ.ID_ChiTietDonHang_DatKhuyenMai = 0;
                            ctdhobj.ID_ChiTietDonHang_DatKhuyenMai = ID_ChiTietDonHang_Moi;
                            //if(int.Parse(dr["ApDungBoiSo"].ToString()) == 1)
                            //{
                            //    int soluongdatden = int.Parse(dr["SoLuongDatKM_Den"].ToString());
                            //    int soluongdattu = int.Parse(dr["SoLuongDatKM_Tu"].ToString());
                            //    newOBJ.soluong = float.Parse(dr["SoLuong"].ToString()) * (int)(ctdhobj.soluong / soluongdatden);
                            //    if(ctdhobj.soluong >= soluongdattu && ctdhobj.soluong <= soluongdatden && newOBJ.soluong == 0)
                            //    {
                            //        newOBJ.soluong = float.Parse(dr["SoLuong"].ToString());
                            //    }
                            //}

                            int ApDungBoiSo = dr["ApDungBoiSo"].ToString() != "" ? int.Parse(dr["ApDungBoiSo"].ToString()) : 0;
                            float SoLuongDatKM_Tu = dr["SoLuongDatKM_Tu"].ToString() != "" ? float.Parse(dr["SoLuongDatKM_Tu"].ToString()) : 0;
                            float SoLuongDatKM_Den = dr["SoLuongDatKM_Den"].ToString() != "" ? float.Parse(dr["SoLuongDatKM_Den"].ToString()) : 0;
                            float TongTienDatKM_Tu = dr["TongTienDatKM_Tu"].ToString() != "" ? float.Parse(dr["TongTienDatKM_Tu"].ToString()) : 0;
                            float TongTienDatKM_Den = dr["TongTienDatKM_Den"].ToString() != "" ? float.Parse(dr["TongTienDatKM_Den"].ToString()) : 0;

                            int bs = 1;



                            //con hieu luc
                            bool km_khonghople = true;



                            if (SoLuongDatKM_Tu > 0 && SoLuongDatKM_Den > 0 && ctdhobj.soluong < SoLuongDatKM_Tu && ctdhobj.soluong > SoLuongDatKM_Den)
                            {
                                //khong dat KM
                                km_khonghople = false;
                            }
                            else if (SoLuongDatKM_Tu > 0 && ctdhobj.soluong < SoLuongDatKM_Tu)
                            {
                                //khong dat KM
                                km_khonghople = false;

                            }


                            if (TongTienDatKM_Tu > 0 && TongTienDatKM_Den > 0 && (ctdhobj.soluong * ctdhobj.giaban) < TongTienDatKM_Tu && (ctdhobj.soluong * ctdhobj.giaban) > TongTienDatKM_Den)
                            {
                                //khong dat KM
                                km_khonghople = false;

                            }
                            else if (TongTienDatKM_Tu > 0 && TongTienDatKM_Den == 0 && (ctdhobj.soluong * ctdhobj.giaban) < TongTienDatKM_Tu)
                            {
                                //khong dat KM
                                km_khonghople = false;

                            }
                            else if (TongTienDatKM_Den > 0 && (ctdhobj.soluong * ctdhobj.giaban) > TongTienDatKM_Den)
                            {
                                km_khonghople = false;

                            }


                            if (km_khonghople)
                            {

                                if (SoLuongDatKM_Den > 0 && ctdhobj.soluong >= SoLuongDatKM_Tu && ctdhobj.soluong <= SoLuongDatKM_Den)
                                {
                                    if (ApDungBoiSo > 0)
                                    {
                                        bs = (int)(ctdhobj.soluong / (double)SoLuongDatKM_Tu);

                                    }
                                }
                                else if (SoLuongDatKM_Den == 0 && ctdhobj.soluong >= SoLuongDatKM_Tu)
                                {
                                    if (ApDungBoiSo > 0)
                                    {
                                        bs = (int)(ctdhobj.soluong / (double)SoLuongDatKM_Tu);

                                    }
                                }

                                HangHoaDB hh = HangHoaDB.Get(ctdhobj.idhanghoa);
                                if (TongTienDatKM_Den > 0 && (ctdhobj.soluong * (ctdhobj.hinhthucban == 1 ? hh.GiaBanBuon : hh.GiaBanLe)) >= TongTienDatKM_Tu && (ctdhobj.soluong * ctdhobj.giaban) <= TongTienDatKM_Den && TongTienDatKM_Tu > 0)
                                {

                                    if (ApDungBoiSo > 0)
                                    {
                                        bs = (int)((ctdhobj.soluong * (ctdhobj.hinhthucban == 1 ? hh.GiaBanBuon : hh.GiaBanLe)) / (double)TongTienDatKM_Den);

                                    }
                                }
                                else if (TongTienDatKM_Den == 0 && (ctdhobj.soluong * (ctdhobj.hinhthucban == 1 ? hh.GiaBanBuon : hh.GiaBanLe)) >= TongTienDatKM_Tu && TongTienDatKM_Tu > 0)
                                {
                                    if (ApDungBoiSo > 0)
                                    {
                                        bs = (int)((ctdhobj.soluong * (ctdhobj.hinhthucban == 1 ? hh.GiaBanBuon : hh.GiaBanLe)) / (double)TongTienDatKM_Tu);

                                    }
                                }

                                if (bs == 0)
                                    bs = 1;
                                newOBJ.soluong = float.Parse(dr["SoLuong"].ToString()) * (int)bs;

                                if (newOBJ.soluong > 0)
                                {
                                    int idChiTietDH = dhData.TaoChiTietDonHang(newOBJ);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                    }
                }
            }
        }
        /**
        * get thông tin chi tiết đơn hàng
        * @param: idDonHang
        * @result: respose satus 200: ok, 401 Unauthorized, 500 Internal Server Error , thông tin đơn hàng
        * @author: VTLan
        * **/
        [HttpGet]
        [Route("chiTietDonHang")]
        public HttpResponseMessage inforInvoice([FromUri] int idDonHang)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    return response;
                }
                else
                {
                    DonHangData donHangData = new DonHangData();
                    DonHangv2 dh = donHangData.GetDonHangTheoID_v2(idDonHang, userinfo.ID_QuanLy);
                    NhanVien_dl nv_dl = new NhanVien_dl();
                    NhanVien nv = nv_dl.GetNVTheoID(dh.ID_NhanVien);
                    DonHang_dl dh_dl = new DonHang_dl();
                    dh_dl.UpdateDonHangDaXem(idDonHang);

                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("donHang", dh);
                    dic.Add("nhanVien", nv);

                    response = Request.CreateResponse(HttpStatusCode.OK, dic);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }


        /**
        * get danh sách hao hụt mặt hàng
        * @param: idDonHang
        * @result: respose satus 200: ok, 401 Unauthorized, 500 Internal Server Error , thông tin đơn hàng
        * @author: VTLan
        * **/
        [HttpGet]
        [Route("danhSachHaoHut")]
        public HttpResponseMessage danhSachHaoHut()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    return response;
                }
                else
                {
                    DonHang_dl dh_dl = new DonHang_dl();
                    HaoHut_Data hhdt = new HaoHut_Data();
                    List<HaoHut> lst = hhdt.GetAll(userinfo.ID_QLLH);

                    response = Request.CreateResponse(HttpStatusCode.OK, lst);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
        /**
       * get danh sách chi tiết đơn hàng
       * @param: idDonHang
       * @result: respose satus 200: ok, 401 Unauthorized, 500 Internal Server Error , thông tin đơn hàng
       * @author: VTLan
       * **/
        [HttpGet]
        [Route("danhSachChiTietDonHang")]
        public HttpResponseMessage danhSachChiTietDonHang([FromUri] int idDonHang)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();


                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    return response;
                }
                else
                {
                    BaoCaoCommon baocao = new BaoCaoCommon();
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    DonHangData dh = new DonHangData();
                    List<ChiTietDonHangModels> dsctdh = dh.LayChiTietDonHang(idDonHang, lang);


                    response = Request.CreateResponse(HttpStatusCode.OK, dsctdh);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("danhSachChiTietMatHangDonHang")]
        public HttpResponseMessage danhSachChiTietMatHangDonHang([FromUri] int iddonhang, int idchitietdonhang, int idhanghoa)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();


                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    return response;
                }
                else
                {
                    DonHangData dh = new DonHangData();
                    List<ChiTietMatHangDonHangModels> dsctdh = dh.GetDSChiTietMatHangDonHang(idchitietdonhang, iddonhang, idhanghoa);


                    response = Request.CreateResponse(HttpStatusCode.OK, dsctdh);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpGet]
        [Route("updateMaVeKhac")]
        public HttpResponseMessage updateMaVeKhac([FromUri] long ID_ChiTiet_MatHang_DonHang, string MaVeKhac)
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
                    DonHangData donHangData = new DonHangData();

                    bool result = donHangData.UpdateMaVeKhac(ID_ChiTiet_MatHang_DonHang, MaVeKhac);
                    if (result)
                        response = Request.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
                    else
                        response = Request.CreateResponse(HttpStatusCode.NotAcceptable, "Cập nhật không thành công");
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
        [Route("updateGroupLink")]
        public HttpResponseMessage updateGroupLink([FromBody] GroupLinkModel model)
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
                    DonHangData donHangData = new DonHangData();

                    bool result = donHangData.UpdateGroupLink(string.Join(",", model.listid), model.grouptext);
                    if (result)
                        response = Request.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
                    else
                        response = Request.CreateResponse(HttpStatusCode.NotAcceptable, "Cập nhật không thành công");
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
        [Route("updateTrangThaiTheDo")]
        public HttpResponseMessage updateTrangThaiTheDo([FromUri] long ID_ChiTiet_MatHang_DonHang, int TrangThai)
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
                    DonHangData donHangData = new DonHangData();

                    bool result = donHangData.UpdateTrangThai(ID_ChiTiet_MatHang_DonHang, TrangThai, userinfo.ID_QuanLy);
                    if (result)
                        response = Request.CreateResponse(HttpStatusCode.OK, "Cập nhật thành công");
                    else
                        response = Request.CreateResponse(HttpStatusCode.NotAcceptable, "Cập nhật không thành công");
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
        [Route("danhSachDichVuDonHang")]
        public HttpResponseMessage danhSachDichVuDonHang([FromUri] int idDonHang)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();


                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    return response;
                }
                else
                {
                    DonHang_DichVuRequestAPIDAO dh = new DonHang_DichVuRequestAPIDAO();
                    List<DonHang_DichVuRequestAPIModel> dsctdh = dh.GetAllByDonHang(idDonHang);


                    response = Request.CreateResponse(HttpStatusCode.OK, dsctdh);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
        /**
   * get danh sách chi tiết đơn hàng
   * @param: idDonHang
   * @result: respose satus 200: ok, 401 Unauthorized, 500 Internal Server Error , thông tin đơn hàng
   * @author: VTLan
   * **/
        [HttpGet]
        [Route("danhSachLichSuTraHang")]
        public HttpResponseMessage danhSachLichSuTraHang([FromUri] int idDonHang)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    return response;
                }
                else
                {
                    DonHang_dl dh_dl = new DonHang_dl();
                    DataTable dtLSTraHang = dh_dl.GetLichSuTraHang(idDonHang);
                    List<LichSuTraHang> list = new List<LichSuTraHang>();
                    foreach (DataRow row in dtLSTraHang.Rows)
                    {
                        LichSuTraHang lichSuTraHang = new LichSuTraHang();
                        lichSuTraHang.idLichSuTraHang = utilsCommon.validateRow(lichSuTraHang.idLichSuTraHang, row, "ID_LichSuTraHang", "System.Double");
                        lichSuTraHang.idDonHang = utilsCommon.validateRow(lichSuTraHang.idDonHang, row, "ID_DonHang", "System.Double");
                        lichSuTraHang.idQuanLy = utilsCommon.validateRow(lichSuTraHang.idQuanLy, row, "ID_QuanLy", "System.Double");
                        lichSuTraHang.idNhanVien = utilsCommon.validateRow(lichSuTraHang.idNhanVien, row, "ID_NhanVien", "System.Double");
                        lichSuTraHang.ghiChu = utilsCommon.validateRow(lichSuTraHang.ghiChu, row, "GhiChu", "System.String");
                        lichSuTraHang.tenQuanLy = utilsCommon.validateRow(lichSuTraHang.tenQuanLy, row, "TenQuanLy", "System.String");
                        lichSuTraHang.insertedTime = utilsCommon.validateRow(lichSuTraHang.insertedTime, row, "InsertedTime", "System.DateTime");
                        lichSuTraHang.ngayTraHang = utilsCommon.validateRow(lichSuTraHang.ngayTraHang, row, "NgayTraHang", "System.DateTime");

                        list.Add(lichSuTraHang);
                    }



                    response = Request.CreateResponse(HttpStatusCode.OK, list);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
        /**
     * get danh sách chi tiết đơn hàng
     * @param: idDonHang
     * @result: respose satus 200: ok, 401 Unauthorized, 500 Internal Server Error , thông tin đơn hàng
     * @author: VTLan
     * **/
        [HttpGet]
        [Route("lichSuGiaoHang")]
        public HttpResponseMessage lichSuGiaoHang([FromUri] int idDonHang)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    return response;
                }
                else
                {
                    DonHang_dl dh_dl = new DonHang_dl();
                    DataTable dtLSGiaoHang = dh_dl.LichSuGiaoHang(idDonHang);
                    List<LichSuGiaoHang> list = new List<LichSuGiaoHang>();
                    foreach (DataRow row in dtLSGiaoHang.Rows)
                    {
                        LichSuGiaoHang lichSuGiaoHang = new LichSuGiaoHang();
                        lichSuGiaoHang.id = utilsCommon.validateRow(lichSuGiaoHang.id, row, "Id", "System.Double");
                        lichSuGiaoHang.idDonHang = utilsCommon.validateRow(lichSuGiaoHang.idDonHang, row, "ID_DonHang", "System.Double");
                        lichSuGiaoHang.idQLLH = utilsCommon.validateRow(lichSuGiaoHang.idQLLH, row, "ID_QLLH", "System.Double");
                        lichSuGiaoHang.idDonVi = utilsCommon.validateRow(lichSuGiaoHang.idDonVi, row, "ID_DonVi", "System.Double");
                        lichSuGiaoHang.idHang = utilsCommon.validateRow(lichSuGiaoHang.idHang, row, "ID_Hang", "System.Double");
                        lichSuGiaoHang.idDANHMUC = utilsCommon.validateRow(lichSuGiaoHang.idDANHMUC, row, "ID_DANHMUC", "System.Double");
                        lichSuGiaoHang.trangThaiXoa = utilsCommon.validateRow(lichSuGiaoHang.trangThaiXoa, row, "TrangThaiXoa", "System.Int32");
                        lichSuGiaoHang.idMatHang = utilsCommon.validateRow(lichSuGiaoHang.idMatHang, row, "ID_MatHang", "System.Double");
                        lichSuGiaoHang.soLuongTraLai = utilsCommon.validateRow(lichSuGiaoHang.soLuongTraLai, row, "SoLuongTraLai", "System.Double");
                        lichSuGiaoHang.soLuongTong = utilsCommon.validateRow(lichSuGiaoHang.soLuongTong, row, "SoLuongTong", "System.Double");
                        lichSuGiaoHang.soLuongGiao = utilsCommon.validateRow(lichSuGiaoHang.soLuongGiao, row, "SoLuongGiao", "System.Double");
                        lichSuGiaoHang.soLuong = utilsCommon.validateRow(lichSuGiaoHang.soLuong, row, "SoLuong", "System.Double");
                        lichSuGiaoHang.giaBanLe = utilsCommon.validateRow(lichSuGiaoHang.giaBanLe, row, "GiaBanLe", "System.Double");
                        lichSuGiaoHang.giaBanBuon = utilsCommon.validateRow(lichSuGiaoHang.giaBanBuon, row, "GiaBanBuon", "System.Double");
                        lichSuGiaoHang.giaBan = utilsCommon.validateRow(lichSuGiaoHang.giaBan, row, "GiaBan", "System.Double");
                        lichSuGiaoHang.lanGiao = utilsCommon.validateRow(lichSuGiaoHang.lanGiao, row, "LanGiao", "System.Int32");
                        lichSuGiaoHang.hinhThucBan = utilsCommon.validateRow(lichSuGiaoHang.hinhThucBan, row, "HinhThucBan", "System.Int32");
                        lichSuGiaoHang.idQuanLy = utilsCommon.validateRow(lichSuGiaoHang.idQuanLy, row, "ID_QuanLy", "System.Int32");
                        lichSuGiaoHang.loaiHangHoa = utilsCommon.validateRow(lichSuGiaoHang.loaiHangHoa, row, "LoaiHangHoa", "System.Int32");
                        lichSuGiaoHang.idQuanLyXoa = utilsCommon.validateRow(lichSuGiaoHang.idQuanLyXoa, row, "ID_QuanLyXoa", "System.Int32");
                        lichSuGiaoHang.idNhanVien = utilsCommon.validateRow(lichSuGiaoHang.idNhanVien, row, "ID_NhanVien", "System.Double");
                        lichSuGiaoHang.idGiaoHang = utilsCommon.validateRow(lichSuGiaoHang.idGiaoHang, row, "ID_GiaoHang", "System.Double");
                        lichSuGiaoHang.idNhaCungCap = utilsCommon.validateRow(lichSuGiaoHang.idNhaCungCap, row, "ID_NhaCungCap", "System.Double");
                        lichSuGiaoHang.idNhanHieu = utilsCommon.validateRow(lichSuGiaoHang.idNhanHieu, row, "ID_NhanHieu", "System.Double");
                        lichSuGiaoHang.tonKhoOrder = utilsCommon.validateRow(lichSuGiaoHang.tonKhoOrder, row, "TonKhoOrder", "System.Double");
                        lichSuGiaoHang.idKho = utilsCommon.validateRow(lichSuGiaoHang.idKho, row, "ID_Kho", "System.Double");
                        lichSuGiaoHang.ghiChu = utilsCommon.validateRow(lichSuGiaoHang.ghiChu, row, "GhiChu", "System.String");
                        lichSuGiaoHang.ngayGiao = utilsCommon.validateRow(lichSuGiaoHang.ngayGiao, row, "NgayGiao", "System.String");
                        lichSuGiaoHang.tenKho = utilsCommon.validateRow(lichSuGiaoHang.tenKho, row, "TenKho", "System.String");
                        lichSuGiaoHang.tenDanhMuc = utilsCommon.validateRow(lichSuGiaoHang.tenDanhMuc, row, "TenDanhMuc", "System.String");
                        lichSuGiaoHang.moTa = utilsCommon.validateRow(lichSuGiaoHang.moTa, row, "MoTa", "System.String");
                        lichSuGiaoHang.linkGioiThieu = utilsCommon.validateRow(lichSuGiaoHang.linkGioiThieu, row, "LinkGioiThieu", "System.String");
                        lichSuGiaoHang.ghiChuGia = utilsCommon.validateRow(lichSuGiaoHang.ghiChuGia, row, "GhiChuGia", "System.String");
                        lichSuGiaoHang.tenDonVi = utilsCommon.validateRow(lichSuGiaoHang.tenDonVi, row, "TenDonVi", "System.String");
                        lichSuGiaoHang.tenNhanVien = utilsCommon.validateRow(lichSuGiaoHang.tenNhanVien, row, "TenNhanVien", "System.String");
                        lichSuGiaoHang.maHang = utilsCommon.validateRow(lichSuGiaoHang.maHang, row, "MaHang", "System.String");
                        lichSuGiaoHang.tenHang = utilsCommon.validateRow(lichSuGiaoHang.maHang, row, "TenHang", "System.String");
                        list.Add(lichSuGiaoHang);
                    }



                    response = Request.CreateResponse(HttpStatusCode.OK, list);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
        /**
        * get thông tin chi tiết đơn hàng
        * @param: idDonHang
        * @result: respose satus 200: ok, 401 Unauthorized, 500 Internal Server Error , thông tin đơn hàng
        * @author: VTLan
        * **/
        [HttpGet]
        [Route("GetChiTietHangTraById")]
        public HttpResponseMessage danhSachChiTietHangTraById([FromUri] int idHangTra)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    return response;
                }
                else
                {
                    //lấy danh sách mặt hàng
                    DonHang_dl dh_dl = new DonHang_dl();
                    DataTable dt = dh_dl.GetChiTietHangTra_ById(idHangTra);
                    response = Request.CreateResponse(HttpStatusCode.OK, dt);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }


        public class paramThanhToan
        {

            public string url { get; set; }
            public string Options { get; set; }


        }
        #endregion

        [HttpGet]
        [Route("getlistnhanvienphanquyen")]
        public HttpResponseMessage getlistnhanvienphanquyen([FromUri] int idDonHang)
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
                    List<NhanVienQuyen> list = new List<NhanVienQuyen>();
                    DonHangData donHangData = new DonHangData();
                    DataTable dt = donHangData.GetNhanVienCapQuyen(idDonHang);
                    foreach (DataRow row in dt.Rows)
                    {
                        NhanVienQuyen nhanVienQuyen = new NhanVienQuyen();

                        nhanVienQuyen.IdNhanVien = (row["ID_NhanVien"] != null) ? int.Parse(row["ID_NhanVien"].ToString()) : 0;
                        nhanVienQuyen.IdDonHang = idDonHang;
                        nhanVienQuyen.TenNhanVien = (row["TenNhanVien"] != null) ? row["TenNhanVien"].ToString() : "";
                        string quyen = (row["ID_Quyen"] != null) ? row["ID_Quyen"].ToString() : "";
                        string quyenkh = (row["QuyenKH"] != null) ? row["QuyenKH"].ToString() : "";

                        if (quyen.Contains("1"))
                            nhanVienQuyen.Xem = 1;
                        else
                        {
                            if (quyenkh.Contains("1"))
                                nhanVienQuyen.Xem = 0;
                            else
                                nhanVienQuyen.Xem = 2;
                        }

                        if (quyen.Contains("2"))
                            nhanVienQuyen.VaoDiem = 1;
                        else
                        {
                            if (quyenkh.Contains("2"))
                                nhanVienQuyen.VaoDiem = 0;
                            else
                                nhanVienQuyen.VaoDiem = 2;
                        }

                        if (quyen.Contains("3"))
                            nhanVienQuyen.GiaoHang = 1;
                        else
                        {
                            if (quyenkh.Contains("3"))
                                nhanVienQuyen.GiaoHang = 0;
                            else
                                nhanVienQuyen.GiaoHang = 2;
                        }

                        if (quyen.Contains("4"))
                            nhanVienQuyen.ThanhToan = 1;
                        else
                        {
                            if (quyenkh.Contains("4"))
                                nhanVienQuyen.ThanhToan = 0;
                            else
                                nhanVienQuyen.ThanhToan = 2;
                        }

                        if (nhanVienQuyen.IdNhanVien > 0)
                            list.Add(nhanVienQuyen);
                    }


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

        [HttpPost]
        [Route("updateQuyenChoNhanVienTheoDonHang")]
        public HttpResponseMessage updateQuyenChoNhanVienTheoDonHang([FromBody] List<NhanVienQuyen> nhanViens)
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
                    DonHang_dl donHangData = new DonHang_dl();
                    foreach (NhanVienQuyen nhanVien in nhanViens)
                    {
                        StringBuilder sb = new StringBuilder();
                        string quyen = "";

                        if (nhanVien.Xem == 1)
                        {
                            sb.Append("1;");
                        }
                        if (nhanVien.VaoDiem == 1)
                        {
                            sb.Append("2;");
                        }
                        if (nhanVien.GiaoHang == 1)
                        {
                            sb.Append("3;");
                        }
                        if (nhanVien.ThanhToan == 1)
                        {
                            sb.Append("4");
                        }
                        quyen = sb.ToString();

                        donHangData.PhanQuyenNhanVien(nhanVien.IdDonHang, nhanVien.IdNhanVien, quyen, userinfo.ID_QuanLy);
                    }


                    response = Request.CreateResponse(HttpStatusCode.OK, "");
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
        [Route("BaoCaoDonHangDanhSach")]
        public HttpResponseMessage BaoCaoDonHangDanhSach([FromBody] TieuChiLoc tieuchi)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            MemoryStream tempPath = new MemoryStream();
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                DonHangData donHangData = new DonHangData();
                FilterGridDonHang filter = new FilterGridDonHang();
                if (tieuchi.Filters != null)
                {
                    foreach (Filter f in tieuchi.Filters.Filters)
                    {
                        switch (f.Field)
                        {
                            case "maDonHang":
                                filter.MaDonHang = f.Value.ToString();
                                break;
                            case "tenKhachHang":
                                filter.TenKhachHang = f.Value.ToString();
                                break;
                            case "ngayLap":
                                try
                                {
                                    filter.NgayLap = Convert.ToDateTime(DateTime.ParseExact(f.Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture));
                                }
                                catch { }
                                break;
                            case "tenNhanVien":
                                filter.TenNhanVien = f.Value.ToString();
                                break;
                            case "maKH":
                                filter.MaKhachHang = f.Value.ToString();
                                break;
                            case "dienThoai":
                                filter.DienThoai = f.Value.ToString();
                                break;
                            case "diaChi":
                                filter.DiaChi = f.Value.ToString();
                                break;
                            case "diaChiTao":
                                filter.ViTriTao = f.Value.ToString();
                                break;
                            case "ghiChu":
                                filter.GhiChu = f.Value.ToString();
                                break;
                            case "isProcess_Name":
                                filter.TrangThaiDonHang = -1;
                                if (f.Value.ToString() == "Chưa hoàn tất")
                                    filter.TrangThaiDonHang = 0;
                                if (f.Value.ToString() == "Đã hoàn tất")
                                    filter.TrangThaiDonHang = 1;
                                if (f.Value.ToString() == "Đã hủy")
                                    filter.TrangThaiDonHang = 2;
                                break;
                            case "tenTrangThaiGiaoHang":
                                filter.TrangThaiGiaoHang = donHangData.GetTrangThaiGiaoHang_ByName(f.Value.ToString());
                                break;
                            case "tongTien":
                                filter.TongTien = Convert.ToSingle(f.Value.ToString());
                                break;
                            case "tienDaThanhToan":
                                filter.DaThanhToan = Convert.ToSingle(f.Value.ToString());
                                break;
                            case "conLai":
                                filter.ConLai = Convert.ToSingle(f.Value.ToString());
                                break;
                        }
                    }
                }

                try
                {
                    if (filter.NgayLap.Year < 1900)
                    {
                        filter.NgayLap = new DateTime(1900, 1, 1);
                    }
                }
                catch
                {
                    filter.NgayLap = new DateTime(1900, 1, 1);
                }
                if (filter.MaDonHang == null)
                    filter.MaDonHang = "";
                if (filter.TenKhachHang == null)
                    filter.TenKhachHang = "";
                if (filter.TenNhanVien == null)
                    filter.TenNhanVien = "";
                if (filter.MaKhachHang == null)
                    filter.MaKhachHang = "";
                if (filter.DienThoai == null)
                    filter.DienThoai = "";
                if (filter.DiaChi == null)
                    filter.DiaChi = "";
                if (filter.ViTriTao == null)
                    filter.ViTriTao = "";
                if (filter.GhiChu == null)
                    filter.GhiChu = "";

                string listid = donHangData.getlistid(userinfo.ID_QLLH, userinfo.ID_QuanLy, tieuchi.from, tieuchi.to, tieuchi.ttgh, tieuchi.tttt, tieuchi.ttht, tieuchi.idKhachHang, tieuchi.IdNhanVien, tieuchi.IdMatHang, tieuchi.ListIDNhom, filter);

                DateTime from_date = tieuchi.from;
                DateTime date_to = tieuchi.to;

                DonHangData baoCao_DB = new DonHangData();

                DataSet ds = baoCao_DB.DanhSachDonHangExport(userinfo.ID_QLLH, userinfo.ID_QuanLy, tieuchi.from, tieuchi.to, tieuchi.ttgh, tieuchi.tttt
                    , tieuchi.ttht, tieuchi.idKhachHang, tieuchi.IdNhanVien, tieuchi.IdMatHang, tieuchi.ListIDNhom, tieuchi.donhangtaidiem, tieuchi.trangthaixem, listid);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "Không tồn tại dữ liệu, tác vụ thực hiện không thành công" });
                }

                ds.Tables[0].TableName = "DATA";

                BaoCaoCommon baocao = new BaoCaoCommon();
                DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                dt2.TableName = "DATA2";
                string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                string title = "";
                if (lang == "vi")
                {
                    title = "Từ " + tieuchi.from.ToString("dd/MM/yyyy") + " đến " + tieuchi.to.ToString("dd/MM/yyyy");
                }
                else
                {
                    title = "From " + tieuchi.from.ToString("dd/MM/yyyy") + " to " + tieuchi.to.ToString("dd/MM/yyyy");
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
                    filename = "BM019_Danhsachdonhang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                    excel.ExportTemplateToStreamGird("ExcelDonHang.xls", dataSet, null, ref stream);
                }
                else
                {
                    filename = "BM019_OrderList_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                    excel.ExportTemplateToStreamGird("ExcelDonHang_en.xls", dataSet, null, ref stream);
                }

                response.Content = new ByteArrayContent(stream.ToArray());
                response.Content.Headers.Add("x-filename", filename);
                response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = filename;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        [HttpPost]
        [Route("BaocaoDonHangChiTiet")]
        public HttpResponseMessage BaocaoDonHangChiTiet([FromBody] TieuChiLoc tieuchi)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            MemoryStream tempPath = new MemoryStream();
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                DonHangData donHangData = new DonHangData();
                FilterGridDonHang filter = new FilterGridDonHang();
                if (tieuchi.Filters != null)
                {
                    foreach (Filter f in tieuchi.Filters.Filters)
                    {
                        switch (f.Field)
                        {
                            case "maDonHang":
                                filter.MaDonHang = f.Value.ToString();
                                break;
                            case "tenKhachHang":
                                filter.TenKhachHang = f.Value.ToString();
                                break;
                            case "ngayLap":
                                try
                                {
                                    filter.NgayLap = Convert.ToDateTime(DateTime.ParseExact(f.Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture));
                                }
                                catch { }
                                break;
                            case "tenNhanVien":
                                filter.TenNhanVien = f.Value.ToString();
                                break;
                            case "maKH":
                                filter.MaKhachHang = f.Value.ToString();
                                break;
                            case "dienThoai":
                                filter.DienThoai = f.Value.ToString();
                                break;
                            case "diaChi":
                                filter.DiaChi = f.Value.ToString();
                                break;
                            case "diaChiTao":
                                filter.ViTriTao = f.Value.ToString();
                                break;
                            case "ghiChu":
                                filter.GhiChu = f.Value.ToString();
                                break;
                            case "isProcess_Name":
                                filter.TrangThaiDonHang = -1;
                                if (f.Value.ToString() == "Chưa hoàn tất")
                                    filter.TrangThaiDonHang = 0;
                                if (f.Value.ToString() == "Đã hoàn tất")
                                    filter.TrangThaiDonHang = 1;
                                if (f.Value.ToString() == "Đã hủy")
                                    filter.TrangThaiDonHang = 2;
                                break;
                            case "tenTrangThaiGiaoHang":
                                filter.TrangThaiGiaoHang = donHangData.GetTrangThaiGiaoHang_ByName(f.Value.ToString());
                                break;
                            case "tongTien":
                                filter.TongTien = Convert.ToSingle(f.Value.ToString());
                                break;
                            case "tienDaThanhToan":
                                filter.DaThanhToan = Convert.ToSingle(f.Value.ToString());
                                break;
                            case "conLai":
                                filter.ConLai = Convert.ToSingle(f.Value.ToString());
                                break;
                        }
                    }
                }

                try
                {
                    if (filter.NgayLap.Year < 1900)
                    {
                        filter.NgayLap = new DateTime(1900, 1, 1);
                    }
                }
                catch
                {
                    filter.NgayLap = new DateTime(1900, 1, 1);
                }
                if (filter.MaDonHang == null)
                    filter.MaDonHang = "";
                if (filter.TenKhachHang == null)
                    filter.TenKhachHang = "";
                if (filter.TenNhanVien == null)
                    filter.TenNhanVien = "";
                if (filter.MaKhachHang == null)
                    filter.MaKhachHang = "";
                if (filter.DienThoai == null)
                    filter.DienThoai = "";
                if (filter.DiaChi == null)
                    filter.DiaChi = "";
                if (filter.ViTriTao == null)
                    filter.ViTriTao = "";
                if (filter.GhiChu == null)
                    filter.GhiChu = "";

                string listid = donHangData.getlistid(userinfo.ID_QLLH, userinfo.ID_QuanLy, tieuchi.from, tieuchi.to, tieuchi.ttgh, tieuchi.tttt, tieuchi.ttht, tieuchi.idKhachHang, tieuchi.IdNhanVien, tieuchi.IdMatHang, tieuchi.ListIDNhom, filter);

                DateTime from_date = tieuchi.from;
                DateTime date_to = tieuchi.to;

                DonHangData baoCao_DB = new DonHangData();

                DataSet ds = baoCao_DB.DanhSachChiTietDonHang(userinfo.ID_QLLH, userinfo.ID_QuanLy, tieuchi.from, tieuchi.to, tieuchi.ttgh, tieuchi.tttt
                    , tieuchi.ttht, tieuchi.idKhachHang, tieuchi.IdNhanVien, tieuchi.IdMatHang, tieuchi.ListIDNhom, tieuchi.donhangtaidiem, tieuchi.trangthaixem, listid);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "Không tồn tại dữ liệu, tác vụ thực hiện không thành công" });
                }
                DataTable dt = GroupBC(ds);
                dt.TableName = "DATA";

                BaoCaoCommon baocao = new BaoCaoCommon();
                DataTable dt2 = baocao.GetInfoCompany(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                dt2.TableName = "DATA2";
                string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                string title = "";
                if (lang == "vi")
                {
                    title = "Từ " + tieuchi.from.ToString("dd/MM/yyyy") + " đến " + tieuchi.to.ToString("dd/MM/yyyy");
                }
                else
                {
                    title = "From " + tieuchi.from.ToString("dd/MM/yyyy") + " to " + tieuchi.to.ToString("dd/MM/yyyy");
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
                    filename = "BM020_Danhsachdonhangchitiet_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                    excel.ExportTemplateToStreamGird("BaoCao_ChiTietDonHang.xls", dataSet, null, ref stream);
                }
                else
                {
                    filename = "BM020_OrderDetailList_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                    excel.ExportTemplateToStreamGird("BaoCao_ChiTietDonHang_en.xls", dataSet, null, ref stream);
                }

                response.Content = new ByteArrayContent(stream.ToArray());
                response.Content.Headers.Add("x-filename", filename);
                response.Content.Headers.Add("Access-Control-Expose-Headers", "x-filename");
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = filename;
                response.StatusCode = HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }

        private DataTable GroupBC(DataSet _dataset)
        {
            DataTable result = _dataset.Tables[0].Copy();
            result.Clear();
            try
            {
                var _fsdff = result.Columns["STT"].DataType;
                // result.Columns["STT"].DataType = typeof(String);

                for (int i = 0; i < _dataset.Tables[1].Rows.Count; i++)
                {
                    //result.Rows.Add(_dataset.Tables[1].Rows[i]);
                    DataRow _row = result.NewRow();
                    _row["STT"] = _dataset.Tables[1].Rows[i]["TenDonHang"].ToString();
                    _row["TongTienChietKhauDonHang"] = Convert.ToDecimal(_dataset.Tables[1].Rows[i]["TongTienChietKhauDonHang"].ToString());
                    _row["IsGroup"] = "1";

                    result.Rows.Add(_row);

                    decimal _tongtien = 0;
                    for (int j = 0; j < _dataset.Tables[0].Rows.Count; j++)
                    {
                        if (_dataset.Tables[1].Rows[i]["ID_DonHang"].ToString() == _dataset.Tables[0].Rows[j]["ID_DonHang"].ToString())
                        {
                            _tongtien += Convert.ToDecimal(_dataset.Tables[0].Rows[j]["TongTien"].ToString());
                            DataRow _row11 = result.NewRow();
                            _row11 = _dataset.Tables[0].Rows[j];
                            result.ImportRow(_row11);
                        }
                        _row["TongTien"] = _tongtien;
                        _row["TongThanhTien"] = _tongtien - Convert.ToDecimal(_dataset.Tables[1].Rows[i]["TongTienChietKhauDonHang"].ToString());


                        //if (_dataset.Tables[1].Rows[i]["ID_DonHang"].ToString() == _dataset.Tables[0].Rows[j]["ID_DonHang"].ToString())
                        //{
                        //    DataRow _row11 = result.NewRow();
                        //    _row11 = _dataset.Tables[0].Rows[j];
                        //    result.ImportRow(_row11);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return result;
        }

        [HttpGet]
        [Route("gethanghoabyidkhachhang")]
        public HttpResponseMessage gethanghoabyidkhachhang([FromUri] int idKhachHang)
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
                    DonHangData dh = new DonHangData();
                    List<MatHang> dsmh = new List<MatHang>();
                    dsmh = dh.getDS_HangHoa_ByIdKhachHang(idKhachHang, userinfo.ID_QLLH);
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
        [Route("gethanghoabyidkhachhangidnhom")]
        public HttpResponseMessage gethanghoabyidkhachhangidnhom([FromUri] int idKhachHang, int ID_DANHMUC)
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
                    DonHangData dh = new DonHangData();
                    List<MatHang> dsmh = new List<MatHang>();
                    if (userinfo.IsAdmin)
                    {
                        dsmh = dh.getDS_HangHoa_ByIdKhachHang_PhanQuyenTheoQuanLy(idKhachHang, userinfo.ID_QLLH, userinfo.ID_QuanLy, ID_DANHMUC);
                    }
                    else
                    {
                        dsmh = dh.getDS_HangHoa_ByIdKhachHang_PhanQuyenTheoNhanVien(idKhachHang, userinfo.ID_QLLH, userinfo.ID_QuanLy, ID_DANHMUC);
                    }
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
        [Route("gethanghoabyidkhachhangmultiidnhom")]
        public HttpResponseMessage gethanghoabyidkhachhangidnhom([FromUri] int idKhachHang, string ID_DANHMUC)
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
                    DonHangData dh = new DonHangData();
                    List<MatHang> dsmh = new List<MatHang>();
                    List<string> ids = ID_DANHMUC.Split(',').ToList<string>();
                    foreach (string id in ids)
                    {
                        if (userinfo.IsAdmin)
                        {
                            dsmh.AddRange(dh.getDS_HangHoa_ByIdKhachHang_PhanQuyenTheoQuanLy(idKhachHang, userinfo.ID_QLLH, userinfo.ID_QuanLy, int.Parse(id)));
                        }
                        else
                        {
                            dsmh.AddRange(dh.getDS_HangHoa_ByIdKhachHang_PhanQuyenTheoNhanVien(idKhachHang, userinfo.ID_QLLH, userinfo.ID_QuanLy, int.Parse(id)));
                        }

                    }
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

        [HttpPost]
        [Route("xemtruocdonhang")]
        public HttpResponseMessage xemtruocdonhang([FromBody] DonHangV2OBJ DHOBJ)
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
                    AppXemTruocDonHangOBJ OBJ = new AppXemTruocDonHangOBJ();
                    OBJ.status = false;
                    OBJ.msg = Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG;
                    try
                    {
                        bool kmconhieuluc = true;
                        double tongtien_donhang_chuatruchietkhau = 0;
                        double tongtien_donhang_thuctra = 0;
                        double tongtien_chietkhau_cacmathangtrongdon = 0;
                        //Kiem tra don hang con hieu luc khong
                        foreach (CTKMOBJ objKM in DHOBJ.dskhuyenmai)
                        {
                            if (DHOBJ.idctkm > 0)
                            {
                                //co ap dung CTKM
                                //check thoi gian va hieu luc CTKM
                                CTKM_OBJ km = CTKMDB.LayCTKM_ByIDCTKM(DHOBJ.idctkm);
                                if (km.trangthai == 0 || new DateTime(km.ngayketthuc.Year, km.ngayketthuc.Month, km.ngayketthuc.Day) <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                                {
                                    //het hieu luc
                                    kmconhieuluc = false;
                                    break;
                                }
                            }
                        }


                        bool KM_MatHang = false;
                        foreach (ChiTietDonHangV2OBJ ctdhobj in DHOBJ.chitietdonhang)
                        {

                            try
                            {
                                foreach (CTKMOBJ objKM in ctdhobj.dskhuyenmai)
                                {
                                    CTKM_OBJ kmmathang = CTKMDB.LayCTKM_ByIDCTKM(objKM.idctkm);
                                    if (kmmathang.trangthai == 0 || new DateTime(kmmathang.ngayketthuc.Year, kmmathang.ngayketthuc.Month, kmmathang.ngayketthuc.Day) <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day))
                                    {

                                        //het hieu luc
                                        kmconhieuluc = false;
                                        KM_MatHang = true;
                                        break;
                                    }
                                    else
                                    {
                                        KM_MatHang = true;
                                        kmconhieuluc = true;

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                LSPos_Data.Utilities.Log.Error(ex);
                            }
                        }


                        if ((DHOBJ.dskhuyenmai.Count > 0 && !kmconhieuluc) || (KM_MatHang && !kmconhieuluc))
                        {
                            OBJ.msg = Config.KHUYEN_MAI_HET_HIEU_LUC;
                        }
                        else if (DHOBJ.idcuahang <= 0)
                        {
                            OBJ.msg = Config.CHUA_CHON_CUA_HANG;
                        }

                        else
                        {
                            DHOBJ.tongtien = Math.Round(DHOBJ.tongtien, 2, MidpointRounding.AwayFromZero);
                            DHOBJ.tongtienchietkhau = Math.Round(DHOBJ.tongtienchietkhau, 2, MidpointRounding.AwayFromZero);
                            KhoDB kho = new KhoDB();


                            DHOBJ.chitiethangtang = new List<ChiTietDonHangV2OBJ>();
                            OBJ.msg = Config.THANH_CONG;
                            OBJ.status = true;
                            foreach (ChiTietDonHangV2OBJ ctdhobj in DHOBJ.chitietdonhang)
                            {
                                try
                                {
                                    double tongtienmathang_chuatru_ck = 0;
                                    double tongtienchietkhaumathang = 0;
                                    double thanhtien = 0;

                                    //phía client bắt buộc truyền số lượng và giá bán lên (nếu chọn giá buôn, hoặc
                                    tongtienmathang_chuatru_ck = ctdhobj.soluong * ctdhobj.giaban;

                                    foreach (CTKMOBJ objCTKM in ctdhobj.dskhuyenmai)
                                    {
                                        #region Get Danh sach hang tang
                                        DataTable dtCTKM = new DataTable();
                                        //them chi tiet hang tang
                                        if (ctdhobj.idctkm > 0)
                                        {
                                            dtCTKM = CTKMDB.GetChiTietHangTang(objCTKM.idctkm);
                                        }
                                        foreach (DataRow dr in dtCTKM.Rows)
                                        {
                                            try
                                            {
                                                if (dr["ID_Hang"].ToString() == ctdhobj.idhanghoa.ToString())
                                                {

                                                    ChiTietDonHangV2OBJ newOBJ = new ChiTietDonHangV2OBJ();
                                                    newOBJ.tenhang = dr["TenHangTang"].ToString();
                                                    newOBJ.mahang = dr["MaHangTang"].ToString();
                                                    newOBJ.tendonvi = dr["TenDonVi"].ToString();

                                                    newOBJ.hinhthucban = 2;//hang khuyen mai
                                                    newOBJ.HangKhuyenMai = 1;
                                                    newOBJ.giaban = 0;



                                                    int ApDungBoiSo = dr["ApDungBoiSo"].ToString() != "" ? int.Parse(dr["ApDungBoiSo"].ToString()) : 0;
                                                    float SoLuongDatKM_Tu = dr["SoLuongDatKM_Tu"].ToString() != "" ? float.Parse(dr["SoLuongDatKM_Tu"].ToString()) : 0;
                                                    float SoLuongDatKM_Den = dr["SoLuongDatKM_Den"].ToString() != "" ? float.Parse(dr["SoLuongDatKM_Den"].ToString()) : 0;

                                                    float TongTienDatKM_Tu = dr["TongTienDatKM_Tu"].ToString() != "" ? float.Parse(dr["TongTienDatKM_Tu"].ToString()) : 0;

                                                    float TongTienDatKM_Den = dr["TongTienDatKM_Den"].ToString() != "" ? float.Parse(dr["TongTienDatKM_Den"].ToString()) : 0;
                                                    int bs = 1;

                                                    if (SoLuongDatKM_Den > 0 && ctdhobj.soluong >= SoLuongDatKM_Tu && ctdhobj.soluong <= SoLuongDatKM_Den && SoLuongDatKM_Tu > 0)
                                                    {
                                                        if (ApDungBoiSo > 0)
                                                        {
                                                            bs = (int)(ctdhobj.soluong / (double)SoLuongDatKM_Den);

                                                        }
                                                    }
                                                    else if (SoLuongDatKM_Den == 0 && ctdhobj.soluong >= SoLuongDatKM_Tu && SoLuongDatKM_Tu > 0)
                                                    {
                                                        if (ApDungBoiSo > 0)
                                                        {
                                                            bs = (int)(ctdhobj.soluong / (double)SoLuongDatKM_Tu);

                                                        }
                                                    }
                                                    HangHoaDB hh = HangHoaDB.Get(ctdhobj.idhanghoa);
                                                    if (TongTienDatKM_Den > 0 && (ctdhobj.soluong * (ctdhobj.hinhthucban == 1 ? hh.GiaBanBuon : hh.GiaBanLe)) >= TongTienDatKM_Tu && (ctdhobj.soluong * ctdhobj.giaban) <= TongTienDatKM_Den && TongTienDatKM_Tu > 0)
                                                    {

                                                        if (ApDungBoiSo > 0)
                                                        {
                                                            bs = (int)((ctdhobj.soluong * (ctdhobj.hinhthucban == 1 ? hh.GiaBanBuon : hh.GiaBanLe)) / (double)TongTienDatKM_Den);

                                                        }
                                                    }
                                                    else if (TongTienDatKM_Den == 0 && (ctdhobj.soluong * (ctdhobj.hinhthucban == 1 ? hh.GiaBanBuon : hh.GiaBanLe)) >= TongTienDatKM_Tu && TongTienDatKM_Tu > 0)
                                                    {
                                                        if (ApDungBoiSo > 0)
                                                        {
                                                            bs = (int)((ctdhobj.soluong * (ctdhobj.hinhthucban == 1 ? hh.GiaBanBuon : hh.GiaBanLe)) / (double)TongTienDatKM_Tu);

                                                        }
                                                    }
                                                    if (bs == 0)
                                                        bs = 1;

                                                    newOBJ.idhanghoa = int.Parse(dr["ID_HangHoa"].ToString());
                                                    newOBJ.soluong = float.Parse(dr["SoLuong"].ToString()) * (int)bs;


                                                    DHOBJ.chitiethangtang.Add(newOBJ);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                LSPos_Data.Utilities.Log.Error(ex);
                                            }
                                        }

                                        #endregion


                                        //tính khuyến mãi đạt số lượng , đạt tiền => chiết khấu hoặc cộng tiền
                                        List<ChiTietCTKMMatHang_OBJ> lstkhuyenmai = CTKMDB.LayCTKM_TheoMatHang(DHOBJ.idct, ctdhobj.idhanghoa, objCTKM.idctkm);
                                        if (lstkhuyenmai.Count > 0)
                                        {
                                            ChiTietCTKMMatHang_OBJ c = lstkhuyenmai[0];

                                            bool datKM = false;
                                            int bs = 1;
                                            if (c.SoLuongDatKM_Den > 0)
                                            {
                                                if (c.SoLuongDatKM_Den > 0 && ctdhobj.soluong >= c.SoLuongDatKM_Tu && ctdhobj.soluong <= c.SoLuongDatKM_Den && c.SoLuongDatKM_Tu > 0)
                                                {
                                                    if (c.ApDungBoiSo > 0)
                                                    {
                                                        bs = (int)(ctdhobj.soluong / (double)c.SoLuongDatKM_Den);

                                                    }

                                                    datKM = true;
                                                }
                                                else if (c.SoLuongDatKM_Den == 0 && ctdhobj.soluong >= c.SoLuongDatKM_Tu && c.SoLuongDatKM_Tu > 0)
                                                {
                                                    if (c.ApDungBoiSo > 0)
                                                    {
                                                        bs = (int)(ctdhobj.soluong / (double)c.SoLuongDatKM_Tu);

                                                    }
                                                    datKM = true;
                                                }
                                                if (c.TongTienDatKM_Den > 0 && (ctdhobj.soluong * ctdhobj.giaban) >= c.TongTienDatKM_Tu && (ctdhobj.soluong * ctdhobj.giaban) <= c.TongTienDatKM_Den && c.TongTienDatKM_Tu > 0)
                                                {

                                                    if (c.ApDungBoiSo > 0)
                                                    {
                                                        bs = (int)((ctdhobj.soluong * ctdhobj.giaban) / (double)c.TongTienDatKM_Den);

                                                    }
                                                    datKM = true;
                                                }
                                                else if (c.TongTienDatKM_Den == 0 && (ctdhobj.soluong * ctdhobj.giaban) >= c.TongTienDatKM_Tu && c.TongTienDatKM_Tu > 0)
                                                {
                                                    if (c.ApDungBoiSo > 0)
                                                    {
                                                        bs = (int)((ctdhobj.soluong * ctdhobj.giaban) / (double)c.TongTienDatKM_Tu);

                                                    }
                                                    datKM = true;
                                                }
                                                if (datKM == true)
                                                {
                                                    if (bs == 0)
                                                        bs = 1;

                                                }
                                                if (datKM)
                                                {
                                                    //lấy chiết khấu theo hình thức bán
                                                    if (ctdhobj.hinhthucban == 1)//1 : ban buon, 0 : le, 2: khac, -1 KM
                                                    {
                                                        tongtienchietkhaumathang = tongtienmathang_chuatru_ck * (c.chietkhauphantram_banbuon / 100);
                                                        if (c.ApDungBoiSo > 0)
                                                        {
                                                            tongtienchietkhaumathang += (c.chietkhautien_banbuon * bs);
                                                        }
                                                        else
                                                        {
                                                            tongtienchietkhaumathang += c.chietkhautien_banbuon;

                                                        }
                                                    }
                                                    else//if (ctdhobj.hinhthucban == 0)
                                                    {
                                                        tongtienchietkhaumathang = tongtienmathang_chuatru_ck * (c.chietkhauphantram_banle / 100);

                                                        if (c.ApDungBoiSo > 0)
                                                        {
                                                            tongtienchietkhaumathang += (c.chietkhautien_banle * bs);
                                                        }
                                                        else
                                                        {
                                                            tongtienchietkhaumathang += c.chietkhautien_banle;

                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                //lấy chiết khấu theo hình thức bán
                                                if (ctdhobj.hinhthucban == 1)//1 : ban buon, 0 : le, 2: khac, -1 KM
                                                {
                                                    tongtienchietkhaumathang = tongtienmathang_chuatru_ck * (c.chietkhauphantram_banbuon / 100);
                                                    tongtienchietkhaumathang += c.chietkhautien_banbuon;
                                                }
                                                else//if (ctdhobj.hinhthucban == 0)
                                                {
                                                    tongtienchietkhaumathang = tongtienmathang_chuatru_ck * (c.chietkhauphantram_banle / 100);
                                                    tongtienchietkhaumathang += c.chietkhautien_banle;
                                                }
                                            }



                                        }

                                    }

                                    ctdhobj.tongtienchietkhau = tongtienchietkhaumathang;
                                    thanhtien = tongtienmathang_chuatru_ck - tongtienchietkhaumathang;

                                    tongtien_donhang_chuatruchietkhau += tongtienmathang_chuatru_ck;
                                    tongtien_chietkhau_cacmathangtrongdon += tongtienchietkhaumathang;

                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }
                            }

                            tongtien_donhang_thuctra = tongtien_donhang_chuatruchietkhau - tongtien_chietkhau_cacmathangtrongdon;
                            double tienkhuyenmaidonhang = 0;
                            //end tinh chiet khau chi tiet don hang

                            //tinh chiet khau phan tram va tien theo don hang
                            foreach (CTKMOBJ objCTKM in DHOBJ.dskhuyenmai)
                            {

                                tienkhuyenmaidonhang = tongtien_donhang_thuctra * (objCTKM.chietkhauphantram / 100);
                                tienkhuyenmaidonhang += objCTKM.chietkhautien;

                            }
                            tongtien_donhang_thuctra = tongtien_donhang_thuctra - tienkhuyenmaidonhang;
                            DHOBJ.tongtien = tongtien_donhang_thuctra;
                            DHOBJ.tongtienchietkhau = tongtien_chietkhau_cacmathangtrongdon;
                        }
                        OBJ.DonHang = DHOBJ;

                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                        OBJ.msg = Config.THONG_TIN_KHONG_DUNG;
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, OBJ);
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
        [Route("xulynapvi")]
        public HttpResponseMessage xulynapvi([FromBody] LichSuNapVi_NhomTaiKhoanModel item)
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

                    LichSuNapVi_NhomTaiKhoanDB db = new LichSuNapVi_NhomTaiKhoanDB();
                    item.TrangThai = 0;
                    bool success = false;
                    if (db.UpdateThanhCong_LichSuNapVi(item))
                    {
                        success = true;
                    }

                    response = Request.CreateResponse(HttpStatusCode.Created, new { success = success });
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
        [Route("xacnhannapvithanhcong")]
        public HttpResponseMessage xacnhannapvithanhcong([FromUri] int ID)
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

                    LichSuNapVi_NhomTaiKhoanDB db = new LichSuNapVi_NhomTaiKhoanDB();
                    LichSuNapVi_NhomTaiKhoanModel item = db.GetLichSuNapViByID(ID);
                    item.TrangThai = 1;
                    bool success = false;
                    if (db.UpdateThanhCong_LichSuNapVi(item))
                    {
                        success = true;
                    }

                    response = Request.CreateResponse(HttpStatusCode.Created, new { success = success });
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
        [Route("suadonhangtuweb")]
        public HttpResponseMessage testapi([FromBody] paramEditDonHang item)
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

                    Hashtable htParam = new Hashtable();
                    htParam.Add("iddonhang", item.iddonhang);
                    htParam.Add("idquanly", item.idquanly);
                    htParam.Add("dulieuchitietdonhang", item.dulieuchitietdonhang);
                    Utils ut = new Utils();

                    //string sJsonKetQua = ut.CallHTTP("http://jav.ksmart.vn/AppSuaDonHangTuWeb.aspx", htParam);
                    string sJsonKetQua = "";

                    response = Request.CreateResponse(HttpStatusCode.Created, sJsonKetQua);
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
        [Route("timkiemkhachhang_accountcode")]
        public HttpResponseMessage timkiemkhachhang_accountcode([FromUri] string accountCode)
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

                    CommonRepository cm = new CommonRepository();
                    BookingRepository br = new BookingRepository();
                    Account a = cm.GetAccount(accountCode);
                    BookingRes b = br.DetailPos(a.BookingCode);
                    BookingCustomerDetailResponse c = br.DetailCustomer(a.BookingCode);
                    response = Request.CreateResponse(HttpStatusCode.Created, new { Account = a, Booking = b, Customer = c });
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
        [Route("GetAllBienDong")]
        public HttpResponseMessage getDanhSachBienDong([FromUri] int ID_Nhom, DateTime from, DateTime to)
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
                    DonHangData dh = new DonHangData();
                    NhanVienApp nvApp = new NhanVienApp();
                    NhanVienAppModels nhanVienInfor = nvApp.ThongTinNhanVienTheoID(userinfo.ID_QuanLy);
                    DataTable list;

                    if (ID_Nhom != 0)
                    {
                        list = dh.BienDongSoDuNhom_GetAll(ID_Nhom, from, to);
                    }
                    else
                    {
                        list = dh.BienDongSoDuNhom_GetAll(nhanVienInfor.idnhom, from, to);
                    }

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

        [HttpPost]
        [Route("GetLichSuNap")]
        public HttpResponseMessage GetLichSuNap([FromUri] int ID_Nhom)
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
                    LichSuNapVi_NhomTaiKhoanDB db = new LichSuNapVi_NhomTaiKhoanDB();
                    DataTable dt = db.GetLichSuNapVi(ID_Nhom);
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
        [Route("GetTongSoDu")]
        public HttpResponseMessage getTongSoDu([FromUri] int ID_Nhom)
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
                    DonHangData dh = new DonHangData();
                    NhanVienApp nvApp = new NhanVienApp();
                    NhanVienAppModels nhanVienInfor = nvApp.ThongTinNhanVienTheoID(userinfo.ID_QuanLy);
                    float total;

                    if (ID_Nhom != 0)
                    {
                        total = dh.BienDongSoDuNhom_TongTienHienTai(ID_Nhom);
                    }
                    else
                    {
                        total = dh.BienDongSoDuNhom_TongTienHienTai(nhanVienInfor.idnhom);
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, total);
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
        [Route("GetSoDuDauKy")]
        public HttpResponseMessage getSoDuDauKy([FromUri] int ID_Nhom, DateTime To)
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
                    DonHangData dh = new DonHangData();
                    NhanVienApp nvApp = new NhanVienApp();
                    NhanVienAppModels nhanVienInfor = nvApp.ThongTinNhanVienTheoID(userinfo.ID_QuanLy);
                    float total;

                    if (ID_Nhom != 0)
                    {
                        total = dh.BienDongSoDuNhom_SoDuDauKy(ID_Nhom, To);
                    }
                    else
                    {
                        total = dh.BienDongSoDuNhom_SoDuDauKy(ID_Nhom, To);
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, total);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        #region ProcessPayment
        [HttpGet]
        [AllowAnonymous]
        [Route("processpayment")]
        public HttpResponseMessage ProcessPayment(int iddh)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                string virtualPaymentClientURL = "";
                string vpc_Version = "";
                string vpc_Command = "";
                string vpc_Merchant = "";
                string vpc_AccessCode = "";
                string vpc_ReturnURL = "";
                string vpc_Currency = "";
                string SECURE_SECRET = "";

                virtualPaymentClientURL = ConfigurationManager.AppSettings["VI_virtualPaymentClientURL"];
                vpc_Version = ConfigurationManager.AppSettings["VI_vpc_Version"];
                vpc_Command = "pay";
                vpc_Merchant = ConfigurationManager.AppSettings["VI_vpc_Merchant"];
                vpc_AccessCode = ConfigurationManager.AppSettings["VI_vpc_AccessCode"];
                vpc_ReturnURL = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ConfigurationManager.AppSettings["VI_vpc_ReturnURL"] + "/" + iddh;
                vpc_Currency = ConfigurationManager.AppSettings["VI_vpc_Currency"];
                SECURE_SECRET = ConfigurationManager.AppSettings["VI_vpc_Hascode"];
                DonHangData dhData = new DonHangData();
                DonHangModels donhang = dhData.LayDonHang(iddh);
                string vpc_MerchTxnRef = iddh + "_" + donhang.thoigiantao.Ticks.ToString();
                string vpc_OrderInfo = donhang.mathamchieu + " " + donhang.tenvilspay;
                string vpc_Amount = (donhang.tongtien * 100).ToString();

                string vpc_TicketNo = "";

                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        vpc_TicketNo = ip.ToString();
                    }
                }
                // Khoi tao lop thu vien va gan gia tri cac tham so gui sang cong thanh toan
                VPCRequest conn = new VPCRequest(virtualPaymentClientURL);
                // Add the Digital Order Fields for the functionality you wish to use
                conn.SetSecureSecret(SECURE_SECRET);

                // Core Transaction Fields
                conn.AddDigitalOrderField("Title", "onepay paygate");
                conn.AddDigitalOrderField("vpc_Locale", "vn");//Chon ngon ngu hien thi tren cong thanh toan (vn/en)
                conn.AddDigitalOrderField("vpc_Version", vpc_Version);
                conn.AddDigitalOrderField("vpc_Command", vpc_Command);
                conn.AddDigitalOrderField("vpc_Merchant", vpc_Merchant);
                conn.AddDigitalOrderField("vpc_AccessCode", vpc_AccessCode);
                conn.AddDigitalOrderField("vpc_MerchTxnRef", vpc_MerchTxnRef);
                conn.AddDigitalOrderField("vpc_OrderInfo", vpc_OrderInfo);
                conn.AddDigitalOrderField("vpc_Amount", vpc_Amount);
                conn.AddDigitalOrderField("vpc_Currency", vpc_Currency);
                conn.AddDigitalOrderField("vpc_ReturnURL", vpc_ReturnURL);
                // Thong tin them ve khach hang. De trong neu khong co thong tin
                conn.AddDigitalOrderField("vpc_SHIP_Street01", "");
                conn.AddDigitalOrderField("vpc_SHIP_Provice", "");
                conn.AddDigitalOrderField("vpc_SHIP_City", "");
                conn.AddDigitalOrderField("vpc_SHIP_Country", "");
                conn.AddDigitalOrderField("vpc_Customer_Phone", "");
                conn.AddDigitalOrderField("vpc_Customer_Email", "");
                conn.AddDigitalOrderField("vpc_Customer_Id", "");
                // Dia chi IP cua khach hang
                conn.AddDigitalOrderField("vpc_TicketNo", vpc_TicketNo);
                // Chuyen huong trinh duyet sang cong thanh toan
                String url = conn.Create3PartyQueryString();
                response = Request.CreateResponse(HttpStatusCode.OK, url);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return response;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("checkquyerydr")]
        public HttpResponseMessage CheckQuyeryDR(int iddh)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                DonHangData dhData = new DonHangData();
                DonHangModels donhang = dhData.LayDonHang(iddh);
                string vpc_MerchTxnRef = iddh + "_" + donhang.thoigiantao.Ticks.ToString();

                string virtualPaymentClientURL = "";
                string vpc_Version = "";
                string vpc_Command = "";
                string vpc_Merchant = "";
                string vpc_User = "";
                string vpc_Password = "";
                string vpc_AccessCode = "";
                string vpc_SecureHash = "";
                string SECURE_SECRET = "";

                virtualPaymentClientURL = ConfigurationManager.AppSettings["VI_virtualPaymentClientURL"];
                vpc_Version = ConfigurationManager.AppSettings["VI_vpc_Version"];
                vpc_Command = "queryDR";
                vpc_Merchant = ConfigurationManager.AppSettings["VI_vpc_Merchant"];
                vpc_User = ConfigurationManager.AppSettings["VI_vpc_User"];
                vpc_Password = ConfigurationManager.AppSettings["VI_vpc_Password"];
                vpc_AccessCode = ConfigurationManager.AppSettings["VI_vpc_AccessCode"];
                SECURE_SECRET = ConfigurationManager.AppSettings["VI_vpc_Hascode"];

                string vpcURL = virtualPaymentClientURL;


                // Khoi tao lop thu vien va gan gia tri cac tham so gui sang cong thanh toan
                VPCRequest conn = new VPCRequest(virtualPaymentClientURL);
                // Add the Digital Order Fields for the functionality you wish to use
                conn.SetSecureSecret(SECURE_SECRET);

                // Core Transaction Fields

                conn.AddDigitalOrderField("vpc_AccessCode", vpc_AccessCode);
                conn.AddDigitalOrderField("vpc_Command", vpc_Command);
                conn.AddDigitalOrderField("vpc_MerchTxnRef", vpc_MerchTxnRef);
                conn.AddDigitalOrderField("vpc_Merchant", vpc_Merchant);
                conn.AddDigitalOrderField("vpc_Password", vpc_Password);
                conn.AddDigitalOrderField("vpc_User", vpc_User);
                conn.AddDigitalOrderField("vpc_Version", vpc_Version);

                vpc_SecureHash = conn.CreateSHA256Signature(true);

                //RestClient client = new RestClient
                //{
                //    BaseUrl = "https://onepay.vn/"
                //};
                //RestRequest request = new RestRequest("msp/api/v1/vpc/invoices/queries", Method.POST)
                //{
                //    RequestFormat = DataFormat.Json
                //};
                //request.AddHeader("Accept", "application/json");
                //client.Timeout = 500000;
                //request.AddBody(new
                //{
                //    vpc_AccessCode = vpc_AccessCode,
                //    vpc_Command = vpc_Command,
                //    vpc_MerchTxnRef = vpc_MerchTxnRef,
                //    vpc_Merchant = vpc_Merchant,
                //    vpc_Password = vpc_Password,
                //    vpc_User = vpc_User,
                //    vpc_Version = vpc_Version,
                //    vpc_SecureHash = vpc_SecureHash
                //});
                //IRestResponse res = client.Execute(request);
                //if (res.StatusCode != HttpStatusCode.OK)
                //{
                //    throw new Exception();
                //}
                string querydata = $"vpc_AccessCode=" + vpc_AccessCode
                    + "&vpc_Command=" + vpc_Command
                    + "&vpc_MerchTxnRef=" + vpc_MerchTxnRef
                    + "&vpc_Merchant=" + vpc_Merchant
                    + "&vpc_Password=" + vpc_Password
                    + "&vpc_User=" + vpc_User
                    + "&vpc_Version=" + vpc_Version
                    + "&vpc_SecureHash=" + vpc_SecureHash;
                string result = ApiUtility.CallOnepayQuery(querydata);
                var dict = HttpUtility.ParseQueryString(result);
                var json = new JavaScriptSerializer().Serialize(
                                    dict.AllKeys.ToDictionary(k => k, k => dict[k])
                           );
                response = Request.CreateResponse(HttpStatusCode.OK, json);
                //response = Request.CreateResponse(HttpStatusCode.OK, vpc_SecureHash);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return response;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("checkquyerydr")]
        public HttpResponseMessage CheckQuyeryDRB2C(int iddh)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                DonHangData dhData = new DonHangData();
                DonHangModels donhang = dhData.LayDonHang(iddh);
                string vpc_MerchTxnRef = iddh + "_" + donhang.thoigiantao.Ticks.ToString();

                string virtualPaymentClientURL = "";
                string vpc_Version = "";
                string vpc_Command = "";
                string vpc_Merchant = "";
                string vpc_User = "";
                string vpc_Password = "";
                string vpc_AccessCode = "";
                string vpc_SecureHash = "";
                string SECURE_SECRET = "";

                virtualPaymentClientURL = ConfigurationManager.AppSettings["B2C_virtualPaymentClientURL"];
                vpc_Version = ConfigurationManager.AppSettings["B2C_vpc_Version"];
                vpc_Command = "queryDR";
                vpc_Merchant = ConfigurationManager.AppSettings["B2C_vpc_Merchant"];
                vpc_User = ConfigurationManager.AppSettings["B2C_vpc_User"];
                vpc_Password = ConfigurationManager.AppSettings["B2C_vpc_Password"];
                vpc_AccessCode = ConfigurationManager.AppSettings["B2C_vpc_AccessCode"];
                SECURE_SECRET = ConfigurationManager.AppSettings["B2C_vpc_Hascode"];

                string vpcURL = virtualPaymentClientURL;


                // Khoi tao lop thu vien va gan gia tri cac tham so gui sang cong thanh toan
                VPCRequest conn = new VPCRequest(virtualPaymentClientURL);
                // Add the Digital Order Fields for the functionality you wish to use
                conn.SetSecureSecret(SECURE_SECRET);

                // Core Transaction Fields

                conn.AddDigitalOrderField("vpc_AccessCode", vpc_AccessCode);
                conn.AddDigitalOrderField("vpc_Command", vpc_Command);
                conn.AddDigitalOrderField("vpc_MerchTxnRef", vpc_MerchTxnRef);
                conn.AddDigitalOrderField("vpc_Merchant", vpc_Merchant);
                conn.AddDigitalOrderField("vpc_Password", vpc_Password);
                conn.AddDigitalOrderField("vpc_User", vpc_User);
                conn.AddDigitalOrderField("vpc_Version", vpc_Version);

                vpc_SecureHash = conn.CreateSHA256Signature(true);

                //RestClient client = new RestClient
                //{
                //    BaseUrl = "https://onepay.vn/"
                //};
                //RestRequest request = new RestRequest("msp/api/v1/vpc/invoices/queries", Method.POST)
                //{
                //    RequestFormat = DataFormat.Json
                //};
                //request.AddHeader("Accept", "application/json");
                //client.Timeout = 500000;
                //request.AddBody(new
                //{
                //    vpc_AccessCode = vpc_AccessCode,
                //    vpc_Command = vpc_Command,
                //    vpc_MerchTxnRef = vpc_MerchTxnRef,
                //    vpc_Merchant = vpc_Merchant,
                //    vpc_Password = vpc_Password,
                //    vpc_User = vpc_User,
                //    vpc_Version = vpc_Version,
                //    vpc_SecureHash = vpc_SecureHash
                //});
                //IRestResponse res = client.Execute(request);
                //if (res.StatusCode != HttpStatusCode.OK)
                //{
                //    throw new Exception();
                //}
                string querydata = $"vpc_AccessCode=" + vpc_AccessCode
                    + "&vpc_Command=" + vpc_Command
                    + "&vpc_MerchTxnRef=" + vpc_MerchTxnRef
                    + "&vpc_Merchant=" + vpc_Merchant
                    + "&vpc_Password=" + vpc_Password
                    + "&vpc_User=" + vpc_User
                    + "&vpc_Version=" + vpc_Version
                    + "&vpc_SecureHash=" + vpc_SecureHash;
                string result = ApiUtility.CallOnepayQuery(querydata);
                var dict = HttpUtility.ParseQueryString(result);
                var json = new JavaScriptSerializer().Serialize(
                                    dict.AllKeys.ToDictionary(k => k, k => dict[k])
                           );
                response = Request.CreateResponse(HttpStatusCode.OK, json);
                //response = Request.CreateResponse(HttpStatusCode.OK, vpc_SecureHash);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return response;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("checkquyerydronepay")]
        public HttpResponseMessage CheckQuyeryDROnepay(string vpc_MerchTxnRef)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                DonHangData dhData = new DonHangData();
                string virtualPaymentClientURL = "";
                string vpc_Version = "";
                string vpc_Command = "";
                string vpc_Merchant = "";
                string vpc_User = "";
                string vpc_Password = "";
                string vpc_AccessCode = "";
                string vpc_SecureHash = "";
                string SECURE_SECRET = "";

                virtualPaymentClientURL = ConfigurationManager.AppSettings["VI_virtualPaymentClientURL"];
                vpc_Version = ConfigurationManager.AppSettings["VI_vpc_Version"];
                vpc_Command = "queryDR";
                vpc_Merchant = ConfigurationManager.AppSettings["VI_vpc_Merchant"];
                vpc_User = ConfigurationManager.AppSettings["VI_vpc_User"];
                vpc_Password = ConfigurationManager.AppSettings["VI_vpc_Password"];
                vpc_AccessCode = ConfigurationManager.AppSettings["VI_vpc_AccessCode"];
                SECURE_SECRET = ConfigurationManager.AppSettings["VI_vpc_Hascode"];

                string vpcURL = virtualPaymentClientURL;


                // Khoi tao lop thu vien va gan gia tri cac tham so gui sang cong thanh toan
                VPCRequest conn = new VPCRequest(virtualPaymentClientURL);
                // Add the Digital Order Fields for the functionality you wish to use
                conn.SetSecureSecret(SECURE_SECRET);

                // Core Transaction Fields

                conn.AddDigitalOrderField("vpc_AccessCode", vpc_AccessCode);
                conn.AddDigitalOrderField("vpc_Command", vpc_Command);
                conn.AddDigitalOrderField("vpc_MerchTxnRef", vpc_MerchTxnRef);
                conn.AddDigitalOrderField("vpc_Merchant", vpc_Merchant);
                conn.AddDigitalOrderField("vpc_Password", vpc_Password);
                conn.AddDigitalOrderField("vpc_User", vpc_User);
                conn.AddDigitalOrderField("vpc_Version", vpc_Version);

                vpc_SecureHash = conn.CreateSHA256Signature(true);

                //RestClient client = new RestClient
                //{
                //    BaseUrl = "https://onepay.vn/"
                //};
                //RestRequest request = new RestRequest("msp/api/v1/vpc/invoices/queries", Method.POST)
                //{
                //    RequestFormat = DataFormat.Json
                //};
                //request.AddHeader("Accept", "application/json");
                //client.Timeout = 500000;
                //request.AddBody(new
                //{
                //    vpc_AccessCode = vpc_AccessCode,
                //    vpc_Command = vpc_Command,
                //    vpc_MerchTxnRef = vpc_MerchTxnRef,
                //    vpc_Merchant = vpc_Merchant,
                //    vpc_Password = vpc_Password,
                //    vpc_User = vpc_User,
                //    vpc_Version = vpc_Version,
                //    vpc_SecureHash = vpc_SecureHash
                //});
                //IRestResponse res = client.Execute(request);
                //if (res.StatusCode != HttpStatusCode.OK)
                //{
                //    throw new Exception();
                //}
                string querydata = $"vpc_AccessCode=" + vpc_AccessCode
                    + "&vpc_Command=" + vpc_Command
                    + "&vpc_MerchTxnRef=" + vpc_MerchTxnRef
                    + "&vpc_Merchant=" + vpc_Merchant
                    + "&vpc_Password=" + vpc_Password
                    + "&vpc_User=" + vpc_User
                    + "&vpc_Version=" + vpc_Version
                    + "&vpc_SecureHash=" + vpc_SecureHash;
                string result = ApiUtility.CallOnepayQuery(querydata);
                var dict = HttpUtility.ParseQueryString(result);
                var json = new JavaScriptSerializer().Serialize(
                                    dict.AllKeys.ToDictionary(k => k, k => dict[k])
                           );
                OnepayDRResponse obj = JsonConvert.DeserializeObject<OnepayDRResponse>(json);
                response = Request.CreateResponse(HttpStatusCode.OK, json);
                //response = Request.CreateResponse(HttpStatusCode.OK, vpc_SecureHash);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return response;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("checkquyerydronepayb2c")]
        public HttpResponseMessage CheckQuyeryDROnepayB2C(string vpc_MerchTxnRef)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                DonHangData dhData = new DonHangData();
                string virtualPaymentClientURL = "";
                string vpc_Version = "";
                string vpc_Command = "";
                string vpc_Merchant = "";
                string vpc_User = "";
                string vpc_Password = "";
                string vpc_AccessCode = "";
                string vpc_SecureHash = "";
                string SECURE_SECRET = "";

                virtualPaymentClientURL = ConfigurationManager.AppSettings["B2C_virtualPaymentClientURL"];
                vpc_Version = ConfigurationManager.AppSettings["B2C_vpc_Version"];
                vpc_Command = "queryDR";
                vpc_Merchant = ConfigurationManager.AppSettings["B2C_vpc_Merchant"];
                vpc_User = ConfigurationManager.AppSettings["B2C_vpc_User"];
                vpc_Password = ConfigurationManager.AppSettings["B2C_vpc_Password"];
                vpc_AccessCode = ConfigurationManager.AppSettings["B2C_vpc_AccessCode"];
                SECURE_SECRET = ConfigurationManager.AppSettings["B2C_vpc_Hascode"];

                string vpcURL = virtualPaymentClientURL;


                // Khoi tao lop thu vien va gan gia tri cac tham so gui sang cong thanh toan
                VPCRequest conn = new VPCRequest(virtualPaymentClientURL);
                // Add the Digital Order Fields for the functionality you wish to use
                conn.SetSecureSecret(SECURE_SECRET);

                // Core Transaction Fields

                conn.AddDigitalOrderField("vpc_AccessCode", vpc_AccessCode);
                conn.AddDigitalOrderField("vpc_Command", vpc_Command);
                conn.AddDigitalOrderField("vpc_MerchTxnRef", vpc_MerchTxnRef);
                conn.AddDigitalOrderField("vpc_Merchant", vpc_Merchant);
                conn.AddDigitalOrderField("vpc_Password", vpc_Password);
                conn.AddDigitalOrderField("vpc_User", vpc_User);
                conn.AddDigitalOrderField("vpc_Version", vpc_Version);

                vpc_SecureHash = conn.CreateSHA256Signature(true);

                //RestClient client = new RestClient
                //{
                //    BaseUrl = "https://onepay.vn/"
                //};
                //RestRequest request = new RestRequest("msp/api/v1/vpc/invoices/queries", Method.POST)
                //{
                //    RequestFormat = DataFormat.Json
                //};
                //request.AddHeader("Accept", "application/json");
                //client.Timeout = 500000;
                //request.AddBody(new
                //{
                //    vpc_AccessCode = vpc_AccessCode,
                //    vpc_Command = vpc_Command,
                //    vpc_MerchTxnRef = vpc_MerchTxnRef,
                //    vpc_Merchant = vpc_Merchant,
                //    vpc_Password = vpc_Password,
                //    vpc_User = vpc_User,
                //    vpc_Version = vpc_Version,
                //    vpc_SecureHash = vpc_SecureHash
                //});
                //IRestResponse res = client.Execute(request);
                //if (res.StatusCode != HttpStatusCode.OK)
                //{
                //    throw new Exception();
                //}
                string querydata = $"vpc_AccessCode=" + vpc_AccessCode
                    + "&vpc_Command=" + vpc_Command
                    + "&vpc_MerchTxnRef=" + vpc_MerchTxnRef
                    + "&vpc_Merchant=" + vpc_Merchant
                    + "&vpc_Password=" + vpc_Password
                    + "&vpc_User=" + vpc_User
                    + "&vpc_Version=" + vpc_Version
                    + "&vpc_SecureHash=" + vpc_SecureHash;
                string result = ApiUtility.CallOnepayQuery(querydata);
                var dict = HttpUtility.ParseQueryString(result);
                var json = new JavaScriptSerializer().Serialize(
                                    dict.AllKeys.ToDictionary(k => k, k => dict[k])
                           );
                response = Request.CreateResponse(HttpStatusCode.OK, json);
                //response = Request.CreateResponse(HttpStatusCode.OK, vpc_SecureHash);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return response;
        }

        public string CreateDataQueryOnepay(string vpc_MerchTxnRef)
        {

            string virtualPaymentClientURL = "";
            string vpc_Version = "";
            string vpc_Command = "";
            string vpc_Merchant = "";
            string vpc_User = "";
            string vpc_Password = "";
            string vpc_AccessCode = "";
            string vpc_SecureHash = "";
            string SECURE_SECRET = "";

            virtualPaymentClientURL = ConfigurationManager.AppSettings["VI_virtualPaymentClientURL"];
            vpc_Version = ConfigurationManager.AppSettings["VI_vpc_Version"];
            vpc_Command = "queryDR";
            vpc_Merchant = ConfigurationManager.AppSettings["VI_vpc_Merchant"];
            vpc_User = ConfigurationManager.AppSettings["VI_vpc_User"];
            vpc_Password = ConfigurationManager.AppSettings["VI_vpc_Password"];
            vpc_AccessCode = ConfigurationManager.AppSettings["VI_vpc_AccessCode"];
            SECURE_SECRET = ConfigurationManager.AppSettings["VI_vpc_Hascode"];

            string vpcURL = virtualPaymentClientURL;


            // Khoi tao lop thu vien va gan gia tri cac tham so gui sang cong thanh toan
            VPCRequest conn = new VPCRequest(virtualPaymentClientURL);
            // Add the Digital Order Fields for the functionality you wish to use
            conn.SetSecureSecret(SECURE_SECRET);

            // Core Transaction Fields

            conn.AddDigitalOrderField("vpc_AccessCode", vpc_AccessCode);
            conn.AddDigitalOrderField("vpc_Command", vpc_Command);
            conn.AddDigitalOrderField("vpc_MerchTxnRef", vpc_MerchTxnRef);
            conn.AddDigitalOrderField("vpc_Merchant", vpc_Merchant);
            conn.AddDigitalOrderField("vpc_Password", vpc_Password);
            conn.AddDigitalOrderField("vpc_User", vpc_User);
            conn.AddDigitalOrderField("vpc_Version", vpc_Version);

            vpc_SecureHash = conn.CreateSHA256Signature(true);

            return vpc_SecureHash;
        }
        #endregion

        public class Aggregates
        {
            public Aggregates()
            {
                this.tongTien = new AggregatesValue(0);
                this.conLai = new AggregatesValue(0);
                this.tienDaThanhToan = new AggregatesValue(0);
            }
            public AggregatesValue tongTien { set; get; }
            public AggregatesValue conLai { set; get; }
            public AggregatesValue tienDaThanhToan { set; get; }
        }
        public class AggregatesValue
        {
            public AggregatesValue(double _sum)
            {
                this.sum = _sum;
            }
            public double sum { set; get; }
        }

        private class RequestGridParam
        {
            public DataSourceRequest request { get; set; }
            public TieuChiLoc tieuchiloc { get; set; }
        }

        public class AppXemTruocDonHangOBJ
        {
            public AppXemTruocDonHangOBJ() { }

            public bool status { get; set; }
            public DonHangV2OBJ DonHang { get; set; }
            public string msg { get; set; }


        }
        public class DonHangParam
        {
            public DonHangv2 donHang { get; set; }
            public List<ChiTietDonHangModels> chiTietDonHang { get; set; }
            public List<ChiTietDonHangModels> chiTietDonHangXoa { get; set; }

        }
        public class NhanVienQuyen
        {
            public int IdNhanVien { get; set; }
            public int IdDonHang { get; set; }
            public string TenNhanVien { get; set; }
            public int Xem { get; set; }
            public int VaoDiem { get; set; }
            public int GiaoHang { get; set; }
            public int ThanhToan { get; set; }
        }
        public class paramTToan
        {
            public string kieuthanhtoan { get; set; }
            public string type { get; set; }
            public int iddonhang { get; set; }
            public int idnhanvien { get; set; }
            public string ghichu { get; set; }
            public int idquanly { get; set; }
            public double tien { get; set; }
            public string token { get; set; }
            public string url { get; set; }
        }
        public class DuLieuChiTietDonHang
        {
            public int idchitietdonhang { get; set; }
            public int idhanghoa { get; set; }
            public int soluong { get; set; }
        }
        public class paramEditDonHang
        {
            public int iddonhang { get; set; }
            public int idquanly { get; set; }
            public string dulieuchitietdonhang { get; set; }
        }
        public class paramGiaoHang
        {
            public string type { get; set; }
            public string dulieugiaohang { get; set; }
            public string token { get; set; }
            public string url { get; set; }
        }
        public class LichSuTraHang
        {
            public double idLichSuTraHang { get; set; }
            public double idDonHang { get; set; }
            public double idQuanLy { get; set; }
            public double idNhanVien { get; set; }
            public string ghiChu { get; set; }
            public DateTime ngayTraHang { get; set; }
            public DateTime insertedTime { get; set; }
            public string tenQuanLy { get; set; }
        }
        public class LichSuGiaoHang
        {
            public double id { get; set; }
            public double idDonHang { get; set; }
            public double idMatHang { get; set; }
            public double soLuongGiao { get; set; }
            public string ghiChu { get; set; }
            public string ngayGiao { get; set; }
            public int lanGiao { get; set; }
            public double idNhanVien { get; set; }
            public int hinhThucBan { get; set; }
            public int idQuanLy { get; set; }
            public double idGiaoHang { get; set; }
            public double idKho { get; set; }
            public double soLuongTraLai { get; set; }
            public double soLuongTong { get; set; }
            public double giaBan { get; set; }
            public double giaBanBuon { get; set; }
            public double giaBanLe { get; set; }
            public string tenDonVi { get; set; }
            public string tenNhanVien { get; set; }
            public double idQLLH { get; set; }
            public double idHang { get; set; }
            public double soLuong { get; set; }
            public double idDANHMUC { get; set; }
            public double idDonVi { get; set; }
            public int trangThaiXoa { get; set; }
            public int idQuanLyXoa { get; set; }
            public double tonKhoOrder { get; set; }
            public double idNhaCungCap { get; set; }
            public double idNhanHieu { get; set; }
            public int loaiHangHoa { get; set; }
            public string maHang { get; set; }
            public string moTa { get; set; }
            public string linkGioiThieu { get; set; }
            public string tenHang { get; set; }
            public string tenDanhMuc { get; set; }
            public string tenKho { get; set; }
            public string ghiChuGia { get; set; }
            public string khuyenMai { get; set; }
            public DateTime ngayXoa { get; set; }
        }
        public class ChiTietHangTra
        {
            public int idLichSuGiaoHang { get; set; }
            public int idhanghoa { get; set; }
            public double soluong { get; set; }
            public double giaban { get; set; }
            public int idKho { get; set; }
            public int hinhthucban { get; set; }
        }
        public class phieuTraHang
        {
            public int idDonHang { get; set; }
            public List<ChiTietHangTra> chiTietHangTra { get; set; }
        }

        public class TaoDonHangLandingModel
        {
            public string username { get; set; }
            public string company { get; set; }
            public KhachHang khachhang { get; set; }
            public DonHangModels donhang { get; set; }
        }


        public SaveBookingModel CreateBookingToLocalAPI(List<DonHang_DichVuRequestAPIModel> serviceRateModel)
        {
            //_sellRepository.GetSellB2BData(Global.SiteID, Global.ProfileID, DateTime.Now);
            ProfileSelectedModel profile = new ProfileSelectedModel
            {
                Name = Global._Profile.Name,
                ProfileId = new Guid(Global._Profile.ProfileId),
                Address = Global._Profile.Address,
                PhoneNumber = Global._Profile.Phone,
                Email = Global._Profile.Email,
                AlternativeEmail = Global._Profile.AlternativeEmail,
                IdentityCard = Global._Profile.IdentityCard,
                MemberId = Global._Profile.MemberId,
                BookingCode = Global._Profile.ProfileCode
            };
            CartB2BModel cart = new CartB2BModel();
            cart.OrderCode = "1";
            cart.CheckInDate = serviceRateModel.First().Ngay;
            cart.CartTime = DateTime.Now.ToShortTimeString();
            cart.MemberId = profile.MemberId;
            cart.profile = profile;
            cart.Note = "B2B_API";
            cart.customer.Add(new CustomerB2BSelectedModel
            {
                CustomerID = Guid.NewGuid(),
                Address = cart.profile.Address,
                CustomerType = "TA_CUSTOMER",
                Email = cart.profile.Email,
                IdOrPPNum = cart.profile.IdentityCard,
                Name = cart.profile.Name,
                PhoneNumber = cart.profile.PhoneNumber
            });
            //cart.customer.Add(new CustomerB2BSelectedModel
            //{
            //    CustomerID = Guid.NewGuid(),
            //    Address = kh.DiaChi,
            //    CustomerType = "GUIDE",
            //    Email = kh.Email,
            //    IdOrPPNum = kh.MaSoThue,
            //    Name = kh.TenDayDu,
            //    PhoneNumber = kh.SoDienThoai
            //});
            cart.EmailTo = cart.profile.Email;

            //Dich vu trong gio hang
            ServiceSelectedModel item = new ServiceSelectedModel();
            //ServiceRateModel servicePackageRate = _sellRepository.GetListServicePackageRate().FirstOrDefault((ServiceRateModel m) => m.ServiceRateID == ServiceRateID);
            foreach (DonHang_DichVuRequestAPIModel serviceRate in serviceRateModel)
            {

                item = new ServiceSelectedModel
                {
                    Amount = Convert.ToInt64(serviceRate.GiaBan),
                    Quantity = (int)serviceRate.SoLuong,
                    Title = serviceRate.DichVu.TenDichVu,
                    ServiceRateID = serviceRate.DichVu.MaDichVu,
                    SellPrice = (decimal)serviceRate.GiaBan
                };
                cart.listServiceSelected.Add(item);

            }

            //Phuong thuc thanh toan
            //cart.listPaymentType.Add(new PaymentTypeModel
            //{
            //    Amount = cart.listServiceSelected.Sum(x => x.Amount),
            //    PaymentTypeID = "b0465aba-7e5b-4726-a434-f2fb4366f9ca", /*_sellRepository.GetListPaymentType().FirstOrDefault().PaymentTypeID,*/
            //    PaymentTypeName = "Chuyển khoản",/*_sellRepository.GetListPaymentType().FirstOrDefault().PaymentTypeID,*/
            //    IsNewPayment = true,
            //    IsPaymentDeposit = false,
            //    BookingPaymentID = Guid.NewGuid()
            //});

            //SaveBookingModel saveBooking = _sellRepository.SaveBookingB2BExportTicket(cart, 0L);
            LSPos_Data.Utilities.Log.Info("SaveBookingB2B - start");
            SaveBookingModel saveBooking = _sellRepository.SaveBookingB2B(cart, 0L);
            LSPos_Data.Utilities.Log.Info("SaveBookingB2B - end");

            return saveBooking;
        }

        public SaveBookingModel ConfirmBookingToLocalAPI(List<DonHang_DichVuRequestAPIModel> serviceRateModel)
        {
            //_sellRepository.GetSellB2BData(Global.SiteID, Global.ProfileID, DateTime.Now);
            ProfileSelectedModel profile = new ProfileSelectedModel
            {
                Name = Global._Profile.Name,
                ProfileId = new Guid(Global._Profile.ProfileId),
                Address = Global._Profile.Address,
                PhoneNumber = Global._Profile.Phone,
                Email = Global._Profile.Email,
                AlternativeEmail = Global._Profile.AlternativeEmail,
                IdentityCard = Global._Profile.IdentityCard,
                MemberId = Global._Profile.MemberId,
                BookingCode = Global._Profile.ProfileCode
            };
            CartB2BModel cart = new CartB2BModel();
            cart.BookingCode = serviceRateModel.FirstOrDefault().BookingCode;
            cart.IsBookingOnline = true;
            cart.OrderCode = "1";
            cart.CheckInDate = serviceRateModel.First().Ngay.AddDays(serviceRateModel.First().HanSuDung);
            cart.CartTime = DateTime.Now.ToShortTimeString();
            cart.MemberId = profile.MemberId;
            cart.profile = profile;
            cart.Note = "B2B_API";
            cart.customer.Add(new CustomerB2BSelectedModel
            {
                CustomerID = Guid.NewGuid(),
                Address = "",//cart.profile.Address,
                CustomerType = "TA_CUSTOMER",
                Email = "",//cart.profile.Email,
                IdOrPPNum = "",//cart.profile.IdentityCard,
                Name = "Khach le",//cart.profile.Name,
                PhoneNumber = ""//cart.profile.PhoneNumber
            });
            //cart.customer.Add(new CustomerB2BSelectedModel
            //{
            //    CustomerID = Guid.Parse("2307AC9F-8634-4AE0-1EC9-08DB16710124"),
            //    Address = "96 Phúc Thành, TP Ninh Bình",
            //    CustomerType = "CUSTOMER",
            //    Email = "",
            //    IdOrPPNum = "",
            //    Name = "B2B_ONLINE_LAMEDIA",
            //    PhoneNumber = "0916863588"
            //});
            cart.EmailTo = cart.profile.Email;

            //Dich vu trong gio hang
            ServiceSelectedModel item = new ServiceSelectedModel();
            //ServiceRateModel servicePackageRate = _sellRepository.GetListServicePackageRate().FirstOrDefault((ServiceRateModel m) => m.ServiceRateID == ServiceRateID);
            foreach (DonHang_DichVuRequestAPIModel serviceRate in serviceRateModel)
            {

                item = new ServiceSelectedModel
                {
                    Amount = Convert.ToInt64(serviceRate.ThanhTien),
                    Quantity = (int)serviceRate.SoLuong,
                    Title = serviceRate.DichVu.TenDichVu,
                    ServiceRateID = serviceRate.DichVu.MaDichVu,
                    SellPrice = (decimal)serviceRate.GiaBan
                };
                cart.listServiceSelected.Add(item);

            }

            //Phuong thuc thanh toan
            cart.listPaymentType.Add(new PaymentTypeModel
            {
                Amount = cart.listServiceSelected.Sum(x => x.Amount),
                PaymentTypeID = serviceRateModel.FirstOrDefault().PaymentTypeID, /*_sellRepository.GetListPaymentType().FirstOrDefault().PaymentTypeID,*/
                PaymentTypeName = serviceRateModel.FirstOrDefault().PaymentTypeName,/*_sellRepository.GetListPaymentType().FirstOrDefault().PaymentTypeID,*/
                IsNewPayment = true,
                IsPaymentDeposit = false,
                BookingPaymentID = Guid.NewGuid()
            });

            SaveBookingModel saveBooking = _sellRepository.SaveBookingB2BExportTicket(cart, 0L);
            return saveBooking;
        }

    }
}
