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
using BusinessLayer.Repository;
using LSPosMVC.App_Start;
using Ticket;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/danhmuc")]
    public class DanhMucController : ApiController
    {
        #region Danh mục trạng thái đơn hàng

        [HttpGet]
        [Route("getall_trangthaidonhang")]
        public HttpResponseMessage Getall_TrangThaiDonHang()
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
                    DanhMucData danhMucData = new DanhMucData();
                    DataTable list = danhMucData.getlisttrangthaidonhang(userinfo.ID_QLLH);

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
        [Route("them_trangthaidonhang")]
        public HttpResponseMessage Them_TrangThaiDonHang([FromBody] TrangThaiDonHangOBJ model)
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
                    model.ID_QLLH = userinfo.ID_QLLH;
                    if (ttdh_dl.Them(model))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luutrangthaidonhangthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luutrangthaidonhangthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luutrangthaidonhangthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("sua_trangthaidonhang")]
        public HttpResponseMessage Sua_TrangThaiDonHang([FromBody] TrangThaiDonHangOBJ item)
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
                    TrangThaiDonHangOBJ ttdh_Update = ttdh_dl.GetTrangThaiDonHangById(item.ID_TrangThaiDonHang);
                    ttdh_Update.TenTrangThai = item.TenTrangThai;
                    ttdh_Update.MauTrangThai = item.MauTrangThai.Replace("#", "");
                    ttdh_Update.MacDinh = item.MacDinh;
                    ttdh_Update.KetThuc = item.KetThuc;
                    if (ttdh_dl.Sua(ttdh_Update))
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, new { success = true, msg = "label_luutrangthaidonhangthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luutrangthaidonhangthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luutrangthaidonhangthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("themsuatrangthaidonhang")]
        public HttpResponseMessage themsuatrangthaidonhang([FromBody] TrangThaiDonHangOBJ item)
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
                    item.ID_QLLH = userinfo.ID_QLLH;

                    if (item.ID_TrangThaiDonHang > 0)
                    {
                        if (ttdh_dl.Sua(item))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luutrangthaidonhangthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luutrangthaidonhangthatbai" });
                        }
                    }
                    else
                    {
                        if (ttdh_dl.Them(item))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luutrangthaidonhangthanhcong" });
                        }
                        else
                        {
                            string msg = "label_themmoithatbaihoactrangthainaydatontai";
                            if (item.MacDinh == 1)
                                msg = "label_trangthaikhoitaodonhangdatontaikhongthethemmoi";
                            if (item.KetThuc == 1)
                                msg = "label_trangthaihoantatdonhangdatontaikhongthethemmoi";
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = msg });
                        }
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luutrangthaidonhangthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("xoa_trangthaidonhang")]
        public HttpResponseMessage Xoa_TrangThaiDonHang([FromUri] int ID)
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
                    TrangThaiDonHangOBJ ttdh_Update = ttdh_dl.GetTrangThaiDonHangById(ID);
                    ttdh_Update.ID_QuanLyXoa = userinfo.ID_QuanLy;
                    if (ttdh_dl.Xoa(ttdh_Update))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoatrangthaidonhangthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoatrangthaidonhangthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_xoatrangthaidonhangthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("trangthaidonhang_theoid")]
        public HttpResponseMessage TrangThaiDonHang_TheoID([FromUri] int ID)
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
                    TrangThaiDonHangOBJ ttdh_Update = ttdh_dl.GetTrangThaiDonHangById(ID);
                    response = Request.CreateResponse(HttpStatusCode.Created, ttdh_Update);
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.Created, "Không tìm thấy");
            }
            return response;
        }
        #endregion

        #region Danh mục hao hụt
        [HttpGet]
        [Route("getall_haohut")]
        public HttpResponseMessage Getall_HaoHut()
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
                    HaoHut_Data data = new HaoHut_Data();
                    List<HaoHut> list = data.GetAll(userinfo.ID_QLLH);
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
        [Route("haohut_theoid")]
        public HttpResponseMessage HaoHut_TheoID([FromUri] int ID)
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
                    HaoHut_Data data = new HaoHut_Data();
                    HaoHut item = data.GetByID(ID, userinfo.ID_QLLH);
                    response = Request.CreateResponse(HttpStatusCode.OK, item);
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
        [Route("them_haohut")]
        public HttpResponseMessage Them_HaoHut([FromBody] HaoHut model)
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
                    HaoHut_Data data = new HaoHut_Data();
                    model.ID_QLLH = userinfo.ID_QLLH;
                    model.GhiChu = string.IsNullOrWhiteSpace(model.GhiChu) ? "" : model.GhiChu;
                    if (data.GetAll(userinfo.ID_QLLH).Select(x => x.MaLoaiHaoHut == model.MaLoaiHaoHut).Count() > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, new { success = false, msg = "label_luudanhmucthatbai" });
                        return response;
                    }
                    if (data.CreateHaoHut(model))
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, new { success = true, msg = "label_luudanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("sua_haohut")]
        public HttpResponseMessage Sua_HaoHut([FromBody] HaoHut item)
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
                    HaoHut_Data data = new HaoHut_Data();
                    HaoHut hh = data.GetByID(item.ID, userinfo.ID_QLLH);
                    if (data.GetAll(userinfo.ID_QLLH).Select(x => x.MaLoaiHaoHut == item.MaLoaiHaoHut).Count() > 0 && hh.MaLoaiHaoHut != item.MaLoaiHaoHut)
                    {
                        return Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
                    }
                    hh.TenLoaiHaoHut = item.TenLoaiHaoHut;
                    hh.MaLoaiHaoHut = item.MaLoaiHaoHut;
                    hh.TiLe = item.TiLe;
                    hh.GhiChu = string.IsNullOrWhiteSpace(item.GhiChu) ? "" : item.GhiChu;

                    if (data.UpdateHaoHut(hh))
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, new { success = true, msg = "label_luudanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("themsuahaohut")]
        public HttpResponseMessage themsuahaohut([FromBody] HaoHut item)
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
                    DanhMucData danhMucData = new DanhMucData();
                    HaoHut_Data data = new HaoHut_Data();
                    item.ID_QLLH = userinfo.ID_QLLH;

                    if (item.ID > 0)
                    {
                        HaoHut hh = data.GetByID(item.ID, userinfo.ID_QLLH);

                        if (danhMucData.checktrungmahaohut(item.MaLoaiHaoHut, item.ID))
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_mahaohutdatontai" });
                        }

                        if (data.UpdateHaoHut(item))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                        }
                    }
                    else
                    {
                        if (danhMucData.checktrungmahaohut(item.MaLoaiHaoHut, item.ID))
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_mahaohutdatontai" });
                        }
                        if (data.CreateHaoHut(item))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                        }
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }
        [HttpPost]
        [Route("xoa_haohut")]
        public HttpResponseMessage Xoa_HaoHut([FromUri] int ID)
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
                    HaoHut_Data ttdh_dl = new HaoHut_Data();
                    if (ttdh_dl.DeleteHaoHut(ID, userinfo.ID_QLLH))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoadanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoadanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_xoadanhmucthatbai" });
            }
            return response;
        }
        #endregion

        #region Danh mục kho hàng
        [HttpGet]
        [Route("getall_khohang")]
        public HttpResponseMessage Getall_KhoHang()
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
                    KhoDB data = new KhoDB();
                    List<KhoOBJ> list = data.GetListDanhSach(userinfo.ID_QLLH);
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
        [Route("khohang_theoid")]
        public HttpResponseMessage KhoHang_TheoID([FromUri] int ID)
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
                    KhoDB data = new KhoDB();
                    KhoOBJ item = data.GetById(ID);
                    response = Request.CreateResponse(HttpStatusCode.OK, item);
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
        [Route("them_khohang")]
        public HttpResponseMessage Them_KhoHang([FromBody] KhoOBJ model)
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
                    KhoDB data = new KhoDB();
                    model.ID_QLLH = userinfo.ID_QLLH;
                    model.NgayTao = DateTime.Now;
                    if (model.DiaChi == null)
                    {
                        model.DiaChi = "";
                    }
                    if (data.Them(model))
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, new { success = true, msg = "label_luudanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("sua_khohang")]
        public HttpResponseMessage Sua_KhoHang([FromBody] KhoOBJ item)
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
                    KhoDB data = new KhoDB();
                    KhoOBJ hh = data.GetById(item.ID_Kho);
                    hh.TenKho = item.TenKho;
                    hh.MaKho = item.MaKho;
                    hh.TrangThai = item.TrangThai;
                    if (item.DiaChi == null)
                    {
                        hh.DiaChi = "";
                    }
                    else
                    {
                        hh.DiaChi = item.DiaChi;

                    }
                    if (data.Sua(hh, userinfo.ID_QuanLy))
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, new { success = true, msg = "label_luudanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("xoa_khohang")]
        public HttpResponseMessage Xoa_KhoHang([FromUri] int ID)
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
                    KhoDB data = new KhoDB();
                    if (data.Xoa(ID, userinfo.ID_QuanLy))
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, new { success = true, msg = "Xóa danh mục thành công" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "Xóa danh mục thất bại" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "Xóa danh mục thất bại" });
            }
            return response;
        }
        #endregion

        #region Danh mục phản hồi
        [HttpGet]
        [Route("getall_phanhoi")]
        public HttpResponseMessage Getall_PhanHoi()
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
                    PhanHoiDB data = new PhanHoiDB();
                    List<PhanHoiOBJ> list = data.GetList(userinfo.ID_QLLH);
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
        [Route("phanhoi_theoid")]
        public HttpResponseMessage PhanHoi_TheoID([FromUri] int ID)
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
                    PhanHoiDB data = new PhanHoiDB();
                    PhanHoiOBJ item = data.GetPhanHoiById(ID);
                    response = Request.CreateResponse(HttpStatusCode.OK, item);
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
        [Route("them_phanhoi")]
        public HttpResponseMessage Them_PhanHoi([FromBody] PhanHoiOBJ model)
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
                    PhanHoiDB data = new PhanHoiDB();
                    model.ID_QLLH = userinfo.ID_QLLH;
                    if (data.ThemMoi(model))
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, new { success = true, msg = "label_luudanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("sua_phanhoi")]
        public HttpResponseMessage Sua_PhanHoi([FromBody] PhanHoiOBJ item)
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
                    PhanHoiDB data = new PhanHoiDB();
                    PhanHoiOBJ model = data.GetPhanHoiById(item.ID_PhanHoi);
                    model.TenPhanHoi = item.TenPhanHoi;
                    if (data.CapNhat(model))
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, new { success = true, msg = "label_luudanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("themsuaphanhoi")]
        public HttpResponseMessage themsuaphanhoi([FromBody] PhanHoiOBJ item)
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
                    PhanHoiDB data = new PhanHoiDB();

                    item.ID_QLLH = userinfo.ID_QLLH;
                    if (item.ID_PhanHoi > 0)
                    {
                        if (data.CapNhat(item))
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        else
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                    }
                    else
                    {
                        if (data.ThemMoi(item))
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        else
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("xoa_phanhoi")]
        public HttpResponseMessage Xoa_PhanHoi([FromUri] int ID)
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
                    PhanHoiDB data = new PhanHoiDB();
                    if (data.Xoa(ID))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoadanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoadanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_xoadanhmucthatbai" });
            }
            return response;
        }
        #endregion

        #region Danh mục checklist
        [HttpGet]
        [Route("getall_checklist")]
        public HttpResponseMessage Getall_Checklist()
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
                    CheckListDB data = new CheckListDB();
                    DataTable dt = data.GetList(userinfo.ID_QLLH);
                    List<CheckListOBJ> list = new List<CheckListOBJ>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        CheckListOBJ item = data.GetCheckList(dr);
                        list.Add(item);
                    }
                    list = list.OrderByDescending(x => x.ID_CheckList).ToList();
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
        [Route("checklist_theoid")]
        public HttpResponseMessage Checklist_TheoID([FromUri] int ID)
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
                    CheckListDB data = new CheckListDB();
                    CheckListOBJ item = data.GetCheckListById(ID);
                    response = Request.CreateResponse(HttpStatusCode.OK, item);
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
        [Route("them_checklist")]
        public HttpResponseMessage Them_CheckList([FromBody] CheckListOBJ model)
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
                    CheckListDB data = new CheckListDB();
                    model.ID_QLLH = userinfo.ID_QLLH;
                    if (data.ThemMoi(model))
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, new { success = true, msg = "label_luudanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("sua_checklist")]
        public HttpResponseMessage Sua_Checklist([FromBody] CheckListOBJ item)
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
                    CheckListDB data = new CheckListDB();
                    CheckListOBJ model = data.GetCheckListById(item.ID_CheckList);
                    model.TenCheckList = item.TenCheckList;
                    if (data.CapNhat(model))
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, new { success = true, msg = "label_luudanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("themsuachecklist")]
        public HttpResponseMessage themsuachecklist([FromBody] CheckListOBJ item)
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
                    CheckListDB data = new CheckListDB();

                    item.ID_QLLH = userinfo.ID_QLLH;
                    if (item.ID_CheckList > 0)
                    {
                        if (data.CapNhat(item))
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        else
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                    }
                    else
                    {
                        if (data.ThemMoi(item))
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        else
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("xoa_checklist")]
        public HttpResponseMessage Xoa_Checklist([FromUri] int ID)
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
                    CheckListDB data = new CheckListDB();
                    if (data.Xoa(ID))
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoadanhmucthanhcong" });
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoadanhmucthatbai" });
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_xoadanhmucthatbai" });
            }
            return response;
        }
        #endregion

        #region Danh mục đơn vị tính
        [HttpGet]
        [Route("getall_donvi")]
        public HttpResponseMessage Getall_DonVi()
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
                    DonVi_dl data = new DonVi_dl();
                    List<DonVi> list = data.GetDonViByIDQLLH(userinfo.ID_QLLH);
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
        [Route("donvi_theoid")]
        public HttpResponseMessage DonVi_TheoID([FromUri] int ID)
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
                    DonVi_dl data = new DonVi_dl();
                    DonVi item = data.GetDonViByID(ID);
                    response = Request.CreateResponse(HttpStatusCode.OK, item);
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
        [Route("them_donvi")]
        public HttpResponseMessage Them_DonVi([FromBody] DonVi model)
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
                    DonVi_dl data = new DonVi_dl();
                    model.IDQLLH = userinfo.ID_QLLH;
                    if (data.ThemDonVi(model))
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, new { success = true, msg = "label_luudanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("sua_donvi")]
        public HttpResponseMessage Sua_DonVi([FromBody] DonVi item)
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
                    DonVi_dl data = new DonVi_dl();
                    DonVi model = data.GetDonViByID(item.ID_DonVi);
                    model.TenDonVi = item.TenDonVi;
                    if (data.CapNhatDonVi(model))
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, new { success = true, msg = "label_luudanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("themsuadonvi")]
        public HttpResponseMessage themsuadonvi([FromBody] DonVi item)
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
                    item.IDQLLH = userinfo.ID_QLLH;

                    DonVi_dl data = new DonVi_dl();
                    if (item.ID_DonVi > 0)
                    {
                        if (data.CapNhatDonVi(item))
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        else
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                    }
                    else
                    {
                        if (data.ThemDonVi(item))
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        else
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("xoa_donvi")]
        public HttpResponseMessage Xoa_DonVi([FromUri] int ID)
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
                    DonVi_dl data = new DonVi_dl();
                    DonVi item = data.GetDonViByID(ID);
                    if (data.DeleteDonVi(item))
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoadanhmucthanhcong" });
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoadanhmucthatbai" });
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_xoadanhmucthatbai" });
            }
            return response;
        }
        #endregion

        #region Danh mục kênh bán hàng
        public class KenhBanHang
        {
            public int ID { get; set; }
            public int ID_Parent { get; set; }
            public string Name { get; set; }
            public bool HasChilds { get; set; }
            public List<KenhBanHang> Childs { get; set; }
        }

        public List<KenhBanHang> GetTree_KenhBanHang(int ID_Parent, int ID_QLLH)
        {
            List<KenhBanHang> result = new List<KenhBanHang>();
            List<KenhBanHangOBJ> childs = KenhBanHangDB.getKenhBanHang_ByIdCapCha(ID_Parent, ID_QLLH);
            foreach (KenhBanHangOBJ dm in childs)
            {
                KenhBanHang item = new KenhBanHang();
                List<KenhBanHangOBJ> child = KenhBanHangDB.getKenhBanHang_ByIdCapCha(dm.ID_KenhBanHang, ID_QLLH);
                item.ID = dm.ID_KenhBanHang;
                item.Name = dm.TenKenhBanHang;
                item.ID_Parent = dm.ID_KenhCapTren;
                if (child.Count > 0)
                {
                    item.HasChilds = true;
                    item.Childs = GetTree_KenhBanHang(item.ID, ID_QLLH);
                }
                else
                {
                    item.HasChilds = false;
                }
                result.Add(item);
            }
            return result;

        }

        [HttpGet]
        [Route("getall_kenhbanhang")]
        public HttpResponseMessage Getall_KenhBanHang()
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
                    List<KenhBanHang> list = new List<KenhBanHang>();
                    List<KenhBanHangOBJ> l = KenhBanHangDB.GetListKenhBanHang(userinfo.ID_QLLH).Where(x => x.ID_KenhCapTren == 0).ToList();
                    foreach (KenhBanHangOBJ i in l)
                    {
                        KenhBanHang a = new KenhBanHang();
                        a.ID_Parent = i.ID_KenhCapTren;
                        a.ID = i.ID_KenhBanHang;
                        a.Name = i.TenKenhBanHang;
                        a.Childs = GetTree_KenhBanHang(a.ID, userinfo.ID_QLLH);
                        list.Add(a);
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
        [Route("combodata_kenhbanhang")]
        public HttpResponseMessage ComboData_KenhBanHang()
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
                    List<KenhBanHangOBJ> list = new List<KenhBanHangOBJ>();
                    list = KenhBanHangDB.GetListKenhBanHang(userinfo.ID_QLLH);
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
        [Route("getlistkenhbanhang")]
        public HttpResponseMessage getlistkenhbanhang()
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
                    DataTable list = KenhBanHangDB.LayDanhSachKenhBanHang(userinfo.ID_QLLH);
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
        [Route("kenhbanhang_theoid")]
        public HttpResponseMessage KenhBanHang_TheoID([FromUri] int ID)
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
                    KenhBanHangOBJ item = KenhBanHangDB.GetKenhBanHangById(ID);
                    response = Request.CreateResponse(HttpStatusCode.OK, item);
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
        [Route("them_kenhbanhang")]
        public HttpResponseMessage Them_KenhBanHang([FromBody] KenhBanHangOBJ model)
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
                    model.ID_QLLH = userinfo.ID_QLLH;
                    bool result = KenhBanHangDB.Them(model);
                    if (result)
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, new { success = true, msg = "label_luudanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("sua_kenhbanhang")]
        public HttpResponseMessage Sua_KenhBanHang([FromBody] KenhBanHangOBJ item)
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
                    DonVi_dl data = new DonVi_dl();
                    KenhBanHangOBJ model = KenhBanHangDB.GetKenhBanHangById(item.ID_KenhBanHang);
                    model.TenKenhBanHang = item.TenKenhBanHang;
                    model.ID_KenhCapTren = item.ID_KenhCapTren;
                    if (KenhBanHangDB.Sua(model))
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, new { success = true, msg = "label_luudanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("themsuakenhbanhang")]
        public HttpResponseMessage themsuakenhbanhang([FromBody] KenhBanHangOBJ item)
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
                    item.ID_QLLH = userinfo.ID_QLLH;

                    if (item.ID_KenhBanHang > 0)
                    {
                        if (KenhBanHangDB.Sua(item))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                        }
                    }
                    else
                    {
                        if (KenhBanHangDB.Them(item))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                        }
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("xoa_kenhbanhang")]
        public HttpResponseMessage Xoa_KenhBanHang([FromUri] int ID)
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
                    KenhBanHangOBJ item = KenhBanHangDB.GetKenhBanHangById(ID);
                    List<KenhBanHangOBJ> listcon = KenhBanHangDB.getKenhBanHang_ByIdCapCha(ID, userinfo.ID_QLLH);
                    if (KenhBanHangDB.Xoa(ID))
                    {
                        foreach (KenhBanHangOBJ con in listcon)
                        {
                            con.ID_KenhCapTren = 0;
                            KenhBanHangDB.Sua(con);
                        }
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoadanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoadanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_xoadanhmucthatbai" });
            }
            return response;
        }
        #endregion

        #region Danh mục ngành hàng

        [HttpGet]
        [Route("getlistnganhhang")]
        public HttpResponseMessage getlistnganhhang()
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
                    DataTable list = DanhMucDB.getDataDS_DanhMuc(userinfo.ID_QLLH);
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
        [Route("themsuanganhhang")]
        public HttpResponseMessage themsuanganhhang([FromBody] DanhMucOBJ item)
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
                    item.ID_QLLH = userinfo.ID_QLLH;

                    if (item.ID_DANHMUC > 0)
                    {
                        if (DanhMucDB.SuaDanhMuc(item))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthanhcong" });
                        }
                    }
                    else
                    {
                        if (DanhMucDB.ThemDanhMuc(item))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthanhcong" });
                        }
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthanhcong" });
            }
            return response;
        }

        [HttpPost]
        [Route("xoanganhhang")]
        public HttpResponseMessage xoanganhhang([FromUri] int ID)
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
                    DanhMucOBJ item = DanhMucDB.get_DanhMucById(ID);
                    List<DanhMucOBJ> listcon = DanhMucDB.getDS_DanhMucCon_ByIdDanhMuc(ID);
                    if (DanhMucDB.XoaDanhMuc(item))
                    {
                        foreach (DanhMucOBJ con in listcon)
                        {
                            con.ID_PARENT = 0;
                            DanhMucDB.SuaDanhMuc(con);
                        }
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoadanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoadanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_xoadanhmucthatbai" });
            }
            return response;
        }
        #endregion

        #region Danh mục nhãn hiệu

        [HttpGet]
        [Route("getlistnhanhieu")]
        public HttpResponseMessage getlistnhanhieu()
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
                    NhanHieuDB ttdh_dl = new NhanHieuDB();
                    DataTable list = ttdh_dl.GetDanhSach(userinfo.ID_QLLH);
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
        [Route("themsuanhanhieu")]
        public HttpResponseMessage themsuanhanhieu([FromBody] NhanHieuOBJ item)
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
                    NhanHieuDB nhanHieuDB = new NhanHieuDB();
                    item.ID_QLLH = userinfo.ID_QLLH;

                    if (item.ID_NhanHieu > 0)
                    {
                        if (nhanHieuDB.Sua(item))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                        }
                    }
                    else
                    {
                        if (nhanHieuDB.Them(item))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                        }
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("xoanhanhieu")]
        public HttpResponseMessage xoanhanhieu([FromUri] int ID)
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
                    NhanHieuDB nhanHieuDB = new NhanHieuDB();
                    if (nhanHieuDB.Xoa(ID))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoadanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoadanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_xoadanhmucthatbai" });
            }
            return response;
        }
        #endregion

        #region Danh mục nhà cung cấp

        [HttpGet]
        [Route("getlistnhacungcap")]
        public HttpResponseMessage getlistnhacungcap()
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
                    NhaCungCapDB ttdh_dl = new NhaCungCapDB();
                    DataTable list = ttdh_dl.GetDanhSach(userinfo.ID_QLLH);
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
        [Route("themsuanhacungcap")]
        public HttpResponseMessage themsuanhacungcap([FromBody] NhaCungCapOBJ item)
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
                    NhaCungCapDB nhaCungCapDB = new NhaCungCapDB();
                    item.ID_QLLH = userinfo.ID_QLLH;

                    if (item.ID_NhaCungCap > 0)
                    {
                        if (nhaCungCapDB.Sua(item))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                        }
                    }
                    else
                    {
                        if (nhaCungCapDB.Them(item))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                        }
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luudanhmucthatbai" });
            }
            return response;
        }

        [HttpPost]
        [Route("xoanhacungcap")]
        public HttpResponseMessage xoanhacungcap([FromUri] int ID)
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
                    NhaCungCapDB nhaCungCapDB = new NhaCungCapDB();
                    if (nhaCungCapDB.Xoa(ID))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoadanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoadanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_xoadanhmucthatbai" });
            }
            return response;
        }


        //[HttpPost]
        //[Route("xuLyNapVi")]
        //public HttpResponseMessage xuLyNapVi([FromBody] LichSuNapVi_NhomTaiKhoanModel model)
        //{
        //    HttpResponseMessage response = new HttpResponseMessage();
        //    response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
        //    try
        //    {
        //        UserInfo userinfo = utilsCommon.checkAuthorization();

        //        if (userinfo == null)
        //        {
        //            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
        //        }
        //        else
        //        {
        //            NhaCungCapDB nccdb = new NhaCungCapDB();
        //            NhaCungCapOBJ ncc = nccdb.GetById(model.ID_NhaCungCap);
        //            SiteModel site = new SiteDAO().GetSite(ncc.SiteCode);
        //            new GlobalUtilities().SetGlobal(site.ApiUrl, site.UserName, site.Password, site.SiteCode, ncc.ProfileID, ncc.ProfileCode);

        //            List<ARModel> data = new ARRepository().GetDataAr("", ncc.AccountReceivableNo);
        //            ARModel ar = data.FirstOrDefault();

        //            string returnID2 = new ARRepository().InsertTransAR(ar.ID.ToString(), "b0465aba-7e5b-4726-a434-f2fb4366f9ca", (model.SoTien * -1m), 0m, model.ImgUrl);
        //            Guid arPaymentID3 = Guid.Parse(returnID2);
        //            new ARRepository().Allocate("2", string.Empty, ar.ID, arPaymentID3);

        //            model.NgayTao = DateTime.Now;
        //            model.TongSoDu = ar.Balance;
        //            model.PaymentID = arPaymentID3.ToString();
        //            model.ArID = ar.ID.ToString();
        //            if (nccdb.ThemLichSuNapVi(model))
        //            {
        //                response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luutrangthaidonhangthanhcong" });
        //            }
        //            else
        //            {
        //                response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luutrangthaidonhangthatbai" });
        //            }
        //            return response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LSPos_Data.Utilities.Log.Error(ex);
        //        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luutrangthaidonhangthatbai" });
        //    }
        //    return response;
        //}

        [HttpGet]
        [Route("getlsnapvi_nhacungcap")]
        public HttpResponseMessage getlsnapvi_nhacungcap([FromUri] int ID)
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
                    NhaCungCapDB ttdh_dl = new NhaCungCapDB();
                    DataTable list = ttdh_dl.GetLichSuNapVi(ID);
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
        [Route("getdssite")]
        public HttpResponseMessage getdssite()
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
                    SiteDAO sitedao = new SiteDAO();
                    List<SiteModel> list = new List<SiteModel>();
                    List<NhomOBJ> lstnhom = NhomDB.getDS_Nhom_ByIdTaiKhoan(userinfo.ID_QuanLy);
                    List<string> sites = (from x in lstnhom select x.SiteCode).ToList();
                    if (userinfo.Level == 1)
                    {
                        list = sitedao.GetAllSite();
                    }
                    else
                    {
                        list = sitedao.GetAllSite().Where(x => sites.IndexOf(x.SiteCode) >= 0).ToList();

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

        #region Danh mục hình thức thanh toán
        [HttpGet]
        [Route("gethinhthucthanhtoan")]
        public HttpResponseMessage gethinhthucthanhtoan()
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
                    HinhThucThanhToanDB httt = new HinhThucThanhToanDB();
                    List<HinhThucThanhToanModel> list = new List<HinhThucThanhToanModel>();
                    list = httt.getListHinhThucThanhToan();
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
        [Route("luuhinhthucthanhtoan")]
        public HttpResponseMessage luuhinhthucthanhtoan([FromBody] HinhThucThanhToanModel item)
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
                    HinhThucThanhToanDB httt = new HinhThucThanhToanDB();           

                    if (item.Id > 0)
                    {
                        if (httt.EditHinhThucThanhToan(item))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                        }
                    }
                    else
                    {
                        if (httt.ThemHinhThucThanhToan(item))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luudanhmucthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luudanhmucthatbai" });
                        }
                    }

                    return response;
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
        [Route("xoahinhthucthanhtoan")]
        public HttpResponseMessage xoahinhthucthanhtoan([FromUri] int ID)
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
                    HinhThucThanhToanDB httt = new HinhThucThanhToanDB();
                    if (httt.DeleteHinhThucThanhToan(ID))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoadanhmucthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoadanhmucthatbai" });
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_xoadanhmucthatbai" });
            }
            return response;
        }
        #endregion
    }
}
