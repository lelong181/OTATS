
using LSPosMVC.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using LSPos_Data.Models;
using LSPosMVC.Models.Models_Filter;

using static LSPos_Data.Data.KhachHangData;
using LSPosMVC.Models;
using RazorEngine;

namespace LSPos_Data.Data
{
    [Authorize]
    [RoutePrefix("api/khachhang")]
    public class KhachHangController : ApiController
    {
        [HttpGet]
        [Route("getallKendo")]
        public DataSourceResult GetOrders(HttpRequestMessage requestMessage)
        {
            RequestGridParam param = JsonConvert.DeserializeObject<RequestGridParam>(
            // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
            requestMessage.RequestUri.ParseQueryString().GetKey(0)
        );
            UserInfo userinfo = utilsCommon.checkAuthorization();

            KhachHang_dl kh_dl = new KhachHang_dl();
            KhachHangData khd = new KhachHangData();

            List<KhachHangDTO> lstKhachHang = new List<KhachHangDTO>();
            KhachHangData.FilterGrid filter = new KhachHangData.FilterGrid();
            int tongso = 0;
            if (param.request.Filter != null)
            {
                foreach (Filter f in param.request.Filter.Filters)
                {
                    switch (f.Field)
                    {
                        case "tenKhachHang":
                            filter.TenKhachHang = f.Value.ToString(); ;
                            break;
                        case "maKH":
                            filter.MaKH = f.Value.ToString();
                            break;
                        case "diaChi":
                            filter.DiaChi = f.Value.ToString(); ;
                            break;
                        case "tenTinh":
                            filter.TenTinh = f.Value.ToString();
                            break;
                        case "tenQuan":
                            filter.TenQuan = f.Value.ToString();
                            break;
                        case "tenPhuong":
                            filter.TenPhuong = f.Value.ToString();
                            break;
                        case "dienThoai":
                            filter.DienThoai = f.Value.ToString();
                            break;
                        case "email":
                            filter.Email = f.Value.ToString();
                            break;
                        case "tenNhanVien":
                            filter.TenNhanVien = f.Value.ToString();
                            break;
                        case "tenLoaiKhachHang":
                            filter.TenLoaiKhachHang = f.Value.ToString();
                            break;
                        case "tenNhomKH":
                            filter.TenNhomKH = f.Value.ToString(); ;
                            break;
                        case "tenKenhBanHang":
                            filter.TenKhachHang = f.Value.ToString();
                            break;
                        case "maSoThue":
                            filter.MaSoThue = f.Value.ToString(); ;
                            break;
                        case "nguoiLienHe":
                            filter.NguoiLienHe = f.Value.ToString();
                            break;
                        case "ghiChu":
                            filter.GhiChu = f.Value.ToString();
                            break;
                            //case "ngayTao":
                            //    filter.NgayTao = f.Value.ToString();
                            //    break;
                    }
                }
            }
            lstKhachHang = khd.GetDataKhachHang_Kendo(userinfo.ID_QLLH, userinfo.ID_QuanLy, param.tieuchiloc.IdTinh, param.tieuchiloc.IdQuan, param.tieuchiloc.IdLoaiKhachHang, 0, param.request.Skip, param.request.Take, filter, ref tongso);
            //foreach (DataRow dr in dskh.Rows)
            //{
            //    KhachHang kh = khd.GetKhachHangFromDataRow(dr);
            //    if (kh != null)
            //    {
            //        if (kh.danhsachanh.Count > 0)
            //        {
            //            kh.Imgurl = kh.danhsachanh[0].path_thumbnail_medium;
            //        }
            //        lstKhachHang.Add(kh);
            //    }
            //}
            if (param.request.Sort != null && param.request.Sort.Count() > 0)
            {
                string sortField = param.request.Sort.First().Field;
                switch (sortField)
                {
                    case "tenKhachHang":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstKhachHang = lstKhachHang.OrderBy(x => x.TenKhachHang).ToList();
                        }
                        else
                        {
                            lstKhachHang = lstKhachHang.OrderByDescending(x => x.TenKhachHang).ToList();
                        }
                        break;
                    case "maKH":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstKhachHang = lstKhachHang.OrderBy(x => x.MaKH).ToList();
                        }
                        else
                        {
                            lstKhachHang = lstKhachHang.OrderByDescending(x => x.MaKH).ToList();
                        }
                        break;
                    case "diaChi":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstKhachHang = lstKhachHang.OrderBy(x => x.DiaChi).ToList();
                        }
                        else
                        {
                            lstKhachHang = lstKhachHang.OrderByDescending(x => x.DiaChi).ToList();
                        }
                        break;
                    case "tenTinh":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstKhachHang = lstKhachHang.OrderBy(x => x.TenTinh).ToList();
                        }
                        else
                        {
                            lstKhachHang = lstKhachHang.OrderByDescending(x => x.TenTinh).ToList();
                        }
                        break;
                    case "tenQuan":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstKhachHang = lstKhachHang.OrderBy(x => x.TenQuan).ToList();
                        }
                        else
                        {
                            lstKhachHang = lstKhachHang.OrderByDescending(x => x.TenQuan).ToList();
                        }
                        break;
                    case "tenPhuong":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstKhachHang = lstKhachHang.OrderBy(x => x.TenPhuong).ToList();
                        }
                        else
                        {
                            lstKhachHang = lstKhachHang.OrderByDescending(x => x.TenPhuong).ToList();
                        }
                        break;
                    case "dienThoai":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstKhachHang = lstKhachHang.OrderBy(x => x.DienThoai).ToList();
                        }
                        else
                        {
                            lstKhachHang = lstKhachHang.OrderByDescending(x => x.DienThoai).ToList();
                        }
                        break;
                    case "email":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstKhachHang = lstKhachHang.OrderBy(x => x.Email).ToList();
                        }
                        else
                        {
                            lstKhachHang = lstKhachHang.OrderByDescending(x => x.Email).ToList();
                        }
                        break;
                    case "tenNhanVien":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstKhachHang = lstKhachHang.OrderBy(x => x.TenNhanVien).ToList();
                        }
                        else
                        {
                            lstKhachHang = lstKhachHang.OrderByDescending(x => x.TenNhanVien).ToList();
                        }
                        break;
                    case "tenLoaiKhachHang":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstKhachHang = lstKhachHang.OrderBy(x => x.TenLoaiKhachHang).ToList();
                        }
                        else
                        {
                            lstKhachHang = lstKhachHang.OrderByDescending(x => x.TenLoaiKhachHang).ToList();
                        }
                        break;
                    case "tenNhomKH":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstKhachHang = lstKhachHang.OrderBy(x => x.TenNhomKH).ToList();
                        }
                        else
                        {
                            lstKhachHang = lstKhachHang.OrderByDescending(x => x.TenNhomKH).ToList();
                        }
                        break;
                    case "tenKenhBanHang":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstKhachHang = lstKhachHang.OrderBy(x => x.TenKenhCapTren).ToList();
                        }
                        else
                        {
                            lstKhachHang = lstKhachHang.OrderByDescending(x => x.TenKenhCapTren).ToList();
                        }
                        break;
                    case "maSoThue":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstKhachHang = lstKhachHang.OrderBy(x => x.MaSoThue).ToList();
                        }
                        else
                        {
                            lstKhachHang = lstKhachHang.OrderByDescending(x => x.MaSoThue).ToList();
                        }
                        break;
                    case "nguoiLienHe":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstKhachHang = lstKhachHang.OrderBy(x => x.NguoiLienHe).ToList();
                        }
                        else
                        {
                            lstKhachHang = lstKhachHang.OrderByDescending(x => x.NguoiLienHe).ToList();
                        }
                        break;
                    case "ghiChu":
                        if (param.request.Sort.First().Dir == "asc")
                        {
                            lstKhachHang = lstKhachHang.OrderBy(x => x.GhiChu).ToList();
                        }
                        else
                        {
                            lstKhachHang = lstKhachHang.OrderByDescending(x => x.GhiChu).ToList();
                        }
                        break;
                }
            }
            else
            {
                lstKhachHang = lstKhachHang.OrderByDescending(x => x.ID_KhachHang).ToList();
            }
            DataSourceResult s = new DataSourceResult();
            s.Data = lstKhachHang;
            s.Total = tongso;
            s.Aggregates = null;
            //return lstKhachHang.AsQueryable().ToDataSourceResult(param.request.Take, param.request.Skip, param.request.Sort, param.request.Filter);
            return s;


        }

        [HttpGet]
        [Route("getallKendobyNhanVien")]
        public DataSourceResult GetByNhanVienQL(HttpRequestMessage requestMessage)
        {
            RequestGridParam_ByNhanVien param = JsonConvert.DeserializeObject<RequestGridParam_ByNhanVien>(
            // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
            requestMessage.RequestUri.ParseQueryString().GetKey(0)
        );
            UserInfo userinfo = utilsCommon.checkAuthorization();

            KhachHang_dl kh_dl = new KhachHang_dl();
            KhachHangData khd = new KhachHangData();

            //DataTable dskh = kh_dl.GetDataKhachHangAll(userinfo.ID_QLLH, userinfo.ID_QuanLy, param.tieuchiloc.IdTinh, param.tieuchiloc.IdQuan, param.tieuchiloc.IdLoaiKhachHang, 0);
            //dskh.Rows.RemoveAt(0);
            List<KhachHangDTO> lstKhachHang = new List<KhachHangDTO>();
            KhachHangData.FilterGrid filter = new KhachHangData.FilterGrid();
            int tongso = 0;
            if (param.request.Filter != null)
            {
                foreach (Filter f in param.request.Filter.Filters)
                {
                    switch (f.Field)
                    {
                        case "tenKhachHang":
                            filter.TenKhachHang = f.Value.ToString(); ;
                            break;
                        case "maKH":
                            filter.MaKH = f.Value.ToString();
                            break;
                        case "diaChi":
                            filter.DiaChi = f.Value.ToString(); ;
                            break;
                        case "tenTinh":
                            filter.TenTinh = f.Value.ToString();
                            break;
                        case "tenQuan":
                            filter.TenQuan = f.Value.ToString();
                            break;
                        case "tenPhuong":
                            filter.TenPhuong = f.Value.ToString();
                            break;
                        case "dienThoai":
                            filter.DienThoai = f.Value.ToString();
                            break;
                        case "email":
                            filter.Email = f.Value.ToString();
                            break;
                        case "tenNhanVien":
                            filter.TenNhanVien = f.Value.ToString();
                            break;
                        case "tenLoaiKhachHang":
                            filter.TenLoaiKhachHang = f.Value.ToString();
                            break;
                        case "tenNhomKH":
                            filter.TenNhomKH = f.Value.ToString(); ;
                            break;
                        case "tenKenhBanHang":
                            filter.TenKhachHang = f.Value.ToString();
                            break;
                        case "maSoThue":
                            filter.MaSoThue = f.Value.ToString(); ;
                            break;
                        case "nguoiLienHe":
                            filter.NguoiLienHe = f.Value.ToString();
                            break;
                        case "ghiChu":
                            filter.GhiChu = f.Value.ToString();
                            break;
                    }
                }
            }

            lstKhachHang = khd.GetDataKhachHangByNhanVien_Kendo(userinfo.ID_QLLH, userinfo.ID_QuanLy, param.ID_NhanViens, param.request.Skip, param.request.Take, filter, ref tongso);

            lstKhachHang = lstKhachHang.OrderByDescending(x => x.ID_KhachHang).ToList();
            DataSourceResult s = new DataSourceResult();
            s.Data = lstKhachHang;
            s.Total = tongso;
            s.Aggregates = null;
            return s;


        }

        [HttpGet]
        [Route("getallKendobyNhanVienDT")]
        public object getallKendobyNhanVienDT(HttpRequestMessage requestMessage)
        {
            RequestGridParam_ByNhanVien param = JsonConvert.DeserializeObject<RequestGridParam_ByNhanVien>(
            // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
            requestMessage.RequestUri.ParseQueryString().GetKey(0)
        );
            UserInfo userinfo = utilsCommon.checkAuthorization();

            KhachHang_dl kh_dl = new KhachHang_dl();
            KhachHangData khd = new KhachHangData();

            //DataTable dskh = kh_dl.GetDataKhachHangAll(userinfo.ID_QLLH, userinfo.ID_QuanLy, param.tieuchiloc.IdTinh, param.tieuchiloc.IdQuan, param.tieuchiloc.IdLoaiKhachHang, 0);
            //dskh.Rows.RemoveAt(0);
            List<KhachHangDTO> lstKhachHang = new List<KhachHangDTO>();
            KhachHangData.FilterGrid filter = new KhachHangData.FilterGrid();
            int tongso = 0;
            if (param.request.Filter != null)
            {
                foreach (Filter f in param.request.Filter.Filters)
                {
                    switch (f.Field)
                    {
                        case "tenKhachHang":
                            filter.TenKhachHang = f.Value.ToString(); ;
                            break;
                        case "maKH":
                            filter.MaKH = f.Value.ToString();
                            break;
                        case "diaChi":
                            filter.DiaChi = f.Value.ToString(); ;
                            break;
                        case "tenTinh":
                            filter.TenTinh = f.Value.ToString();
                            break;
                        case "tenQuan":
                            filter.TenQuan = f.Value.ToString();
                            break;
                        case "tenPhuong":
                            filter.TenPhuong = f.Value.ToString();
                            break;
                        case "dienThoai":
                            filter.DienThoai = f.Value.ToString();
                            break;
                        case "email":
                            filter.Email = f.Value.ToString();
                            break;
                        case "tenNhanVien":
                            filter.TenNhanVien = f.Value.ToString();
                            break;
                        case "tenLoaiKhachHang":
                            filter.TenLoaiKhachHang = f.Value.ToString();
                            break;
                        case "tenNhomKH":
                            filter.TenNhomKH = f.Value.ToString(); ;
                            break;
                        case "tenKenhBanHang":
                            filter.TenKhachHang = f.Value.ToString();
                            break;
                        case "maSoThue":
                            filter.MaSoThue = f.Value.ToString(); ;
                            break;
                        case "nguoiLienHe":
                            filter.NguoiLienHe = f.Value.ToString();
                            break;
                        case "ghiChu":
                            filter.GhiChu = f.Value.ToString();
                            break;
                    }
                }
            }

            DataTable dt = khd.GetDataKhachHangByNhanVien_KendoDT(userinfo.ID_QLLH, userinfo.ID_QuanLy, param.ID_NhanViens, param.request.Skip, param.request.Take, filter, ref tongso);
            DataSourceResult s = new DataSourceResult();
            //s.Data = dt;
            s.Total = tongso;
            s.Aggregates = null;
            return new { Data = dt, Total = tongso };


        }

        [HttpGet]
        [AllowAnonymous]
        [Route("exportdskhachhang")]
        public HttpResponseMessage ExportDSKhachHang([FromUri] string username, string maCongTy)
        {
            User_dl userDL = new User_dl();

            UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);
            string filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("//FileExport/Excel//DSKhachHang_Template.xls"));
            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            HSSFWorkbook inputWorkbook = new HSSFWorkbook(stream, true);
            stream.Close();
            HSSFSheet sheet = (HSSFSheet)inputWorkbook.GetSheetAt(0);
            ICellStyle datastyle = (HSSFCellStyle)inputWorkbook.CreateCellStyle();
            datastyle.BorderBottom = datastyle.BorderRight = datastyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            KhachHang_dl kh_dl = new KhachHang_dl();
            DataTable dskh = kh_dl.GetDataKhachHangAll(userinfo.ID_QLLH, userinfo.ID_QuanLy, 0, 0, -1, 0);
            if (dskh.Rows.Count > 0)
            {
                for (int i = 1; i < dskh.Rows.Count; i++)
                {
                    sheet.CreateRow(sheet.LastRowNum + 1);
                    HSSFRow row = (HSSFRow)sheet.GetRow(sheet.LastRowNum);
                    row.CreateCell(0).CellStyle = datastyle;
                    row.CreateCell(1).CellStyle = datastyle;
                    row.CreateCell(2).CellStyle = datastyle;
                    row.CreateCell(3).CellStyle = datastyle;
                    row.CreateCell(4).CellStyle = datastyle;
                    row.CreateCell(5).CellStyle = datastyle;
                    row.CreateCell(6).CellStyle = datastyle;
                    row.CreateCell(7).CellStyle = datastyle;
                    row.CreateCell(8).CellStyle = datastyle;
                    row.CreateCell(9).CellStyle = datastyle;
                    row.CreateCell(10).CellStyle = datastyle;
                    row.CreateCell(11).CellStyle = datastyle;
                    row.CreateCell(12).CellStyle = datastyle;
                    row.CreateCell(13).CellStyle = datastyle;
                    row.CreateCell(14).CellStyle = datastyle;
                    row.CreateCell(15).CellStyle = datastyle;
                    row.Cells[0].SetCellValue(dskh.Rows[i]["MaKH"].ToString());
                    row.Cells[1].SetCellValue(dskh.Rows[i]["TenKhachHang"].ToString());
                    row.Cells[2].SetCellValue(dskh.Rows[i]["DiaChi"].ToString());
                    row.Cells[3].SetCellValue(dskh.Rows[i]["ToaDoKhachHang"].ToString());
                    row.Cells[4].SetCellValue(dskh.Rows[i]["TenTinh"].ToString());
                    row.Cells[5].SetCellValue(dskh.Rows[i]["TenQuan"].ToString());
                    row.Cells[6].SetCellValue(dskh.Rows[i]["TenPhuong"].ToString());
                    row.Cells[7].SetCellValue(dskh.Rows[i]["DienThoai"].ToString());
                    row.Cells[8].SetCellValue(dskh.Rows[i]["Email"].ToString());
                    row.Cells[9].SetCellValue(dskh.Rows[i]["TenNhanVien"].ToString());
                    row.Cells[10].SetCellValue(dskh.Rows[i]["TenLoaiKhachHang"].ToString());
                    row.Cells[11].SetCellValue(dskh.Rows[i]["TenNhomKH"].ToString());
                    row.Cells[12].SetCellValue(dskh.Rows[i]["MaSoThue"].ToString());
                    row.Cells[13].SetCellValue(dskh.Rows[i]["NguoiLienHe"].ToString());
                    row.Cells[14].SetCellValue(dskh.Rows[i]["GhiChu"].ToString());
                    if (dskh.Rows[i]["insertedtime"].ToString() != "")
                    {
                        DateTime ngaytao = Convert.ToDateTime(dskh.Rows[i]["insertedtime"]);
                        row.Cells[15].SetCellValue(ngaytao.ToString("dd/MM/yyyy"));
                    }
                    else
                    {
                        row.Cells[14].SetCellValue("");
                    }

                    //row.Cells[15].SetCellValue(dskh.Rows[i]["MaKH"].ToString());
                }
            }

            //string filename = "DSKhachHang_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls";
            string filename = "DSKH" + ".xls";
            string path = System.Web.HttpContext.Current.Server.MapPath("//FileExport/Excel//" + filename);
            FileStream fsw = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
            inputWorkbook.Write(fsw);
            fsw.Close();
            //byte[] bytesInStream = inputWorkbook.GetBytes();
            //Stream str = new MemoryStream(bytesInStream);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            result.Content = new StreamContent(fs);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = filename
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            UploadingUtils.RemoveFileWithDelay(filename, path, 10);
            return result;
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

                    List<KhachHangDTO> lstKhachHang = new List<KhachHangDTO>();

                    foreach (DataRow dr in dskhDataSrc.Rows)
                    {
                        KhachHangDTO kh = new KhachHangDTO();
                        kh.ID_KhachHang = (dr["ID_KhachHang"] != null) ? int.Parse(dr["ID_KhachHang"].ToString()) : 0;
                        kh.TenKhachHang = (dr["TenKhachHang"] != null) ? dr["TenKhachHang"].ToString() : "";
                        kh.MaKH = (dr["MaKH"] != null) ? dr["MaKH"].ToString() : "";
                        kh.DiaChi = (dr["DiaChi"] != null) ? dr["DiaChi"].ToString() : "";
                        kh.ID_Tinh = (dr["ID_Tinh"].ToString() != "") ? int.Parse(dr["ID_Tinh"].ToString()) : 0;
                        kh.ID_Quan = (dr["ID_Quan"].ToString() != "") ? int.Parse(dr["ID_Quan"].ToString()) : 0;
                        kh.ID_Phuong = (dr["ID_Phuong"].ToString() != "") ? int.Parse(dr["ID_Phuong"].ToString()) : 0;
                        kh.ID_Quyen = (dr["ID_Quyen"] != null) ? dr["ID_Quyen"].ToString() : "";
                        try
                        {
                            kh.TenTinh = (dr["TenTinh"].ToString() != "") ? dr["TenTinh"].ToString() : "";
                            kh.TenQuan = (dr["TenQuan"].ToString() != "") ? dr["TenQuan"].ToString() : "";
                            kh.TenPhuong = (dr["TenPhuong"].ToString() != "") ? dr["TenPhuong"].ToString() : "";
                        }
                        catch { }

                        lstKhachHang.Add(kh);
                    }


                    response = Request.CreateResponse(HttpStatusCode.OK, lstKhachHang);
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
        [Route("GetKhachHangChuaCapQuyen")]
        public HttpResponseMessage GetKhachHangChuaCapQuyen([FromUri] int idNhanvien)
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
                    DataTable dskhDataSrc = kh_dl.GetKhachHangChuaCapQuyen(idNhanvien, userinfo.ID_QLLH, userinfo.ID_QuanLy, -1, 0, 0, 0, 0);

                    List<KhachHangDTO> lstKhachHang = new List<KhachHangDTO>();

                    foreach (DataRow dr in dskhDataSrc.Rows)
                    {
                        KhachHangDTO kh = new KhachHangDTO();
                        kh.ID_KhachHang = (dr["ID_KhachHang"] != null) ? int.Parse(dr["ID_KhachHang"].ToString()) : 0;
                        kh.TenKhachHang = (dr["TenKhachHang"] != null) ? dr["TenKhachHang"].ToString() : "";
                        kh.MaKH = (dr["MaKH"] != null) ? dr["MaKH"].ToString() : "";
                        kh.DiaChi = (dr["DiaChi"] != null) ? dr["DiaChi"].ToString() : "";
                        kh.DienThoai = (dr["DienThoai"] != null) ? dr["DienThoai"].ToString() : "";
                        kh.ID_Tinh = (dr["ID_Tinh"].ToString() != "") ? int.Parse(dr["ID_Tinh"].ToString()) : 0;
                        kh.ID_Quan = (dr["ID_Quan"].ToString() != "") ? int.Parse(dr["ID_Quan"].ToString()) : 0;
                        kh.ID_Phuong = (dr["ID_Phuong"].ToString() != "") ? int.Parse(dr["ID_Phuong"].ToString()) : 0;

                        lstKhachHang.Add(kh);
                    }


                    response = Request.CreateResponse(HttpStatusCode.OK, lstKhachHang);
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
        [Route("getall")]
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
                    KhachHangData khd = new KhachHangData();
                    DataTable dt = new DataTable();
                    dt = khd.GetDataKhachHangByNhanVien_IDQL(userinfo.ID_QLLH, userinfo.ID_QuanLy);
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
        [Route("getDanhSachKhachHangTheoIdNhanVien")]
        public HttpResponseMessage GetDanhSachKhachHangTheoIdNhanVien([FromUri] int idNhanvien)
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
                    KhachHangData kh_dl = new KhachHangData();
                    DataTable dskh = kh_dl.GetKhachHangQuanLy(idNhanvien);

                    //List<KhachHang> lstKhachHang = new List<KhachHang>();
                    //KhachHangData khachHangData = new KhachHangData();

                    //foreach (DataRow dr in dskh.Rows)
                    //{
                    //    KhachHang kh = khachHangData.GetKhachHangFromDataRow(dr);
                    //    if (kh != null)
                    //        lstKhachHang.Add(kh);
                    //}

                    response = Request.CreateResponse(HttpStatusCode.OK, dskh);
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
        public HttpResponseMessage GetByID([FromUri] int ID)
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
                    KhachHang item = kh_dl.GetKhachHangID(ID);
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
        [Route("CheckTrungDienThoai")]
        public HttpResponseMessage CheckPhone([FromBody] ParamKhach param)
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
                    Boolean rs = new Boolean();
                    if (param.type.Equals("CheckTrungDienThoai"))
                    {
                        KhachHang_dl kh_dl = new KhachHang_dl();
                        rs = kh_dl.CheckTrungKhachHangTheoSDT(userinfo.ID_QLLH, param.DienThoai, param.ID_KhachHang);
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, rs);
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
        [Route("LoadExcelFile")]
        public HttpResponseMessage LoadExcelFile([FromBody] FileExcelUpload file)
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
                    try
                    {
                        List<KhachHang_ExcelModel> result = new List<KhachHang_ExcelModel>();
                        string Extension = file.filename.Split('.')[1];
                        switch (Extension)
                        {
                            case "xls": //Excel 97-03
                                result = GetListFromFile_Xls(file.filename);
                                break;
                            case "xlsx": //Excel 07
                                result = GetListFromFile_Xlsx(file.filename);
                                break;
                        }
                        response = Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                    catch (Exception ex)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                        LSPos_Data.Utilities.Log.Error(ex);
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

        public List<KhachHang_ExcelModel> GetListFromFile_Xlsx(string filename)
        {
            List<KhachHang_ExcelModel> result = new List<KhachHang_ExcelModel>();
            FileStream fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath(filename), FileMode.Open, FileAccess.Read);
            XSSFWorkbook inputWorkbook = new XSSFWorkbook(fs);
            XSSFSheet sheet = (XSSFSheet)inputWorkbook.GetSheetAt(0);
            int rowStart = 3;
            for (int i = rowStart; i <= sheet.LastRowNum; i++)
            {
                try
                {
                    XSSFRow dataRow = (XSSFRow)sheet.GetRow(i);
                    KhachHang_ExcelModel item = new KhachHang_ExcelModel();
                    string err = "";

                    // Đọc mã khách hàng
                    string makh = "";
                    if (dataRow.GetCell(0, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        err += " Mã khách hàng trống ";
                    }
                    else
                    {
                        makh = dataRow.GetCell(0).StringCellValue;
                    }
                    item.MaKH = makh;

                    // Đọc tên khách hàng
                    string tenkh = "";
                    if (dataRow.GetCell(1, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        err += " Tên khách hàng trống ";
                    }
                    else
                    {
                        tenkh = dataRow.GetCell(1).StringCellValue;
                    }
                    item.Ten = tenkh;

                    // Đọc địa chỉ
                    string diachi = "";
                    if (dataRow.GetCell(2, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        err += " Địa chỉ khách hàng trống ";
                    }
                    else
                    {
                        diachi = dataRow.GetCell(2).StringCellValue;
                    }
                    item.DiaChi = diachi;

                    // Đọc kinh độ
                    string kinhdotext = "";
                    if (dataRow.GetCell(3, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        //err += " Địa chỉ khách hàng trống ";
                    }
                    else
                    {
                        kinhdotext = dataRow.GetCell(3).StringCellValue;
                    }
                    double kinhdo = 0;
                    item.KinhDo = double.TryParse(kinhdotext, out kinhdo) ? 0 : kinhdo;

                    // Đọc vĩ độ
                    string vidotext = "";
                    if (dataRow.GetCell(4, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        //err += " Địa chỉ khách hàng trống ";
                    }
                    else
                    {
                        vidotext = dataRow.GetCell(4).StringCellValue;
                    }
                    double vido = 0;
                    item.KinhDo = double.TryParse(vidotext, out vido) ? 0 : vido;

                    // Đọc đường phố
                    string duongpho = "";
                    if (dataRow.GetCell(5, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        //err += " Đường phố trống ";
                    }
                    else
                    {
                        duongpho = dataRow.GetCell(5).StringCellValue;
                    }
                    item.DuongPho = duongpho;

                    // Đọc khu vực
                    int khuvuc = 0;
                    if (dataRow.GetCell(24, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        //err += " Địa chỉ khách hàng trống ";
                    }
                    else
                    {
                        khuvuc = (int)dataRow.GetCell(24).NumericCellValue;
                    }
                    item.ID_KhuVuc = khuvuc;

                    // Đọc tỉnh
                    int tinh = 0;
                    if (dataRow.GetCell(25, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        //err += " Địa chỉ khách hàng trống ";
                    }
                    else
                    {
                        tinh = (int)dataRow.GetCell(25).NumericCellValue;
                    }
                    item.ID_Tinh = tinh;

                    // Đọc quận huyện
                    int quan = 0;
                    if (dataRow.GetCell(26, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        //err += " Địa chỉ khách hàng trống ";
                    }
                    else
                    {
                        quan = (int)dataRow.GetCell(26).NumericCellValue;
                    }
                    item.ID_Quan = quan;

                    // Đọc xã , phường
                    int phuong = 0;
                    if (dataRow.GetCell(27, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        //err += " Địa chỉ khách hàng trống ";
                    }
                    else
                    {
                        phuong = (int)dataRow.GetCell(27).NumericCellValue;
                    }
                    item.ID_Phuong = phuong;

                    // Đọc điện thoại
                    string dienthoai = "";
                    if (dataRow.GetCell(10, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            dienthoai = dataRow.GetCell(10).StringCellValue;
                        }
                        catch
                        {
                            dienthoai = dataRow.GetCell(10).NumericCellValue.ToString();
                        }
                    }
                    item.SoDienThoai = dienthoai;

                    // Đọc fax
                    string fax = "";
                    if (dataRow.GetCell(11, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            fax = dataRow.GetCell(11).StringCellValue;
                        }
                        catch
                        {
                            fax = dataRow.GetCell(11).NumericCellValue.ToString();
                        }
                    }
                    item.Fax = fax;

                    // Đọc điện thoại 2                    
                    string dienthoai2 = "";
                    if (dataRow.GetCell(12, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            dienthoai2 = dataRow.GetCell(12).StringCellValue;
                        }
                        catch
                        {
                            dienthoai2 = dataRow.GetCell(12).NumericCellValue.ToString();
                        }
                    }
                    item.SoDienThoai2 = dienthoai2;

                    // Đọc điện thoại 3
                    string dienthoai3 = "";
                    if (dataRow.GetCell(13, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            dienthoai3 = dataRow.GetCell(13).StringCellValue;
                        }
                        catch
                        {
                            dienthoai3 = dataRow.GetCell(13).NumericCellValue.ToString();
                        }
                    }
                    item.SoDienThoai3 = dienthoai3;

                    // Đọc loại khách hàng
                    int loaikhachhang = 0;
                    if (dataRow.GetCell(28, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                    }
                    else
                    {
                        loaikhachhang = (int)dataRow.GetCell(28).NumericCellValue;
                    }
                    item.ID_LoaiKhachHang = loaikhachhang;

                    // Đọc kênh bán hàng
                    int kenh = 0;
                    if (dataRow.GetCell(29, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                    }
                    else
                    {
                        kenh = (int)dataRow.GetCell(29).NumericCellValue;
                    }
                    item.ID_NhomKH = kenh;

                    // Đọc kênh bán hàng cấp trên
                    int kenhcaptren = 0;
                    if (dataRow.GetCell(30, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                    }
                    else
                    {
                        kenhcaptren = (int)dataRow.GetCell(30).NumericCellValue;
                    }
                    item.ID_Cha = kenhcaptren;

                    // Đọc người liên hệ
                    string nguoilienhe = "";
                    if (dataRow.GetCell(17, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            nguoilienhe = dataRow.GetCell(17).StringCellValue;
                        }
                        catch
                        {
                            nguoilienhe = dataRow.GetCell(17).NumericCellValue.ToString();
                        }
                    }
                    item.NguoiLienHe = nguoilienhe;

                    // Đọc hòm thư
                    string homthu = "";
                    if (dataRow.GetCell(18, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            homthu = dataRow.GetCell(18).StringCellValue;
                        }
                        catch
                        {
                            homthu = dataRow.GetCell(18).NumericCellValue.ToString();
                        }
                    }
                    item.Email = homthu;

                    // Đọc website
                    string website = "";
                    if (dataRow.GetCell(19, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            website = dataRow.GetCell(19).StringCellValue;
                        }
                        catch
                        {
                            website = dataRow.GetCell(19).NumericCellValue.ToString();
                        }
                    }
                    item.Website = website;

                    // Đọc địa chỉ xuất hóa đơn
                    string diachihoadon = "";
                    if (dataRow.GetCell(20, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            diachihoadon = dataRow.GetCell(20).StringCellValue;
                        }
                        catch
                        {
                            diachihoadon = dataRow.GetCell(20).NumericCellValue.ToString();
                        }
                    }
                    item.DiaChiXuatHoaDon = diachihoadon;

                    // Đọc số tài khoản
                    string stk = "";
                    if (dataRow.GetCell(21, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            stk = dataRow.GetCell(21).StringCellValue;
                        }
                        catch
                        {
                            stk = dataRow.GetCell(21).NumericCellValue.ToString();
                        }
                    }
                    item.SoTKNganHang = stk;

                    // Đọc mã số thuế
                    string mst = "";
                    if (dataRow.GetCell(22, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            mst = dataRow.GetCell(22).StringCellValue;
                        }
                        catch
                        {
                            mst = dataRow.GetCell(22).NumericCellValue.ToString();
                        }
                    }
                    item.MaSoThue = mst;

                    // Đọc ghi chú
                    string ghichu = "";
                    if (dataRow.GetCell(23, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            ghichu = dataRow.GetCell(23).StringCellValue;
                        }
                        catch
                        {
                            ghichu = dataRow.GetCell(23).NumericCellValue.ToString();
                        }
                    }
                    item.GhiChu = ghichu;

                    result.Add(item);

                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                    LSPos_Data.Utilities.Log.Info("Error in data line " + i);
                }

            }
            return result;
        }
        public List<KhachHang_ExcelModel> GetListFromFile_Xls(string filename)
        {
            List<KhachHang_ExcelModel> result = new List<KhachHang_ExcelModel>();
            FileStream fs = new FileStream(System.Web.HttpContext.Current.Server.MapPath(filename), FileMode.Open, FileAccess.Read);
            HSSFWorkbook inputWorkbook = new HSSFWorkbook(fs, true);

            HSSFSheet sheet = (HSSFSheet)inputWorkbook.GetSheetAt(0);
            int rowStart = 3;
            for (int i = rowStart; i <= sheet.LastRowNum; i++)
            {
                try
                {
                    HSSFRow dataRow = (HSSFRow)sheet.GetRow(i);
                    KhachHang_ExcelModel item = new KhachHang_ExcelModel();
                    string err = "";

                    // Đọc mã khách hàng
                    string makh = "";
                    if (dataRow.GetCell(0).StringCellValue == "" || dataRow.GetCell(0, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        err += " Mã khách hàng trống ";
                        break;
                    }
                    else
                    {
                        try
                        {
                            makh = dataRow.GetCell(0).StringCellValue;
                        }
                        catch
                        {
                            makh = dataRow.GetCell(0).NumericCellValue.ToString();
                        }
                    }
                    item.MaKH = makh;

                    // Đọc tên khách hàng
                    string tenkh = "";
                    if (dataRow.GetCell(1, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        err += " Tên khách hàng trống ";
                    }
                    else
                    {
                        try
                        {
                            tenkh = dataRow.GetCell(1).StringCellValue;
                        }
                        catch
                        {
                            tenkh = dataRow.GetCell(1).NumericCellValue.ToString();
                        }
                    }
                    item.Ten = tenkh;

                    // Đọc địa chỉ
                    string diachi = "";
                    if (dataRow.GetCell(2, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        err += " Địa chỉ khách hàng trống ";
                    }
                    else
                    {
                        try
                        {
                            diachi = dataRow.GetCell(2).StringCellValue;
                        }
                        catch
                        {
                            diachi = dataRow.GetCell(2).NumericCellValue.ToString();
                        }
                    }
                    item.DiaChi = diachi;

                    // Đọc kinh độ
                    string kinhdotext = "";
                    if (dataRow.GetCell(3, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        //err += " Địa chỉ khách hàng trống ";
                    }
                    else
                    {
                        try
                        {
                            kinhdotext = dataRow.GetCell(3).StringCellValue;
                        }
                        catch
                        {
                            kinhdotext = dataRow.GetCell(3).NumericCellValue.ToString();
                        }
                    }
                    double kinhdo = 0;
                    item.KinhDo = double.TryParse(kinhdotext, out kinhdo) ? kinhdo : 0;

                    // Đọc vĩ độ
                    string vidotext = "";
                    if (dataRow.GetCell(4, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        //err += " Địa chỉ khách hàng trống ";
                    }
                    else
                    {
                        try
                        {
                            vidotext = dataRow.GetCell(4).StringCellValue;
                        }
                        catch
                        {
                            vidotext = dataRow.GetCell(4).NumericCellValue.ToString();
                        }
                    }
                    double vido = 0;
                    item.KinhDo = double.TryParse(vidotext, out vido) ? vido : 0;

                    // Đọc đường phố
                    string duongpho = "";
                    if (dataRow.GetCell(5, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        //err += " Đường phố trống ";
                    }
                    else
                    {
                        try
                        {
                            duongpho = dataRow.GetCell(5).StringCellValue;
                        }
                        catch
                        {
                            duongpho = dataRow.GetCell(5).NumericCellValue.ToString();
                        }
                    }
                    item.DuongPho = duongpho;

                    // Đọc khu vực
                    int khuvuc = 0;
                    if (dataRow.GetCell(24, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        //err += " Địa chỉ khách hàng trống ";
                    }
                    else
                    {
                        khuvuc = (int)dataRow.GetCell(24).NumericCellValue;
                    }
                    item.ID_KhuVuc = khuvuc;

                    // Đọc tỉnh
                    int tinh = 0;
                    if (dataRow.GetCell(25, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        //err += " Địa chỉ khách hàng trống ";
                    }
                    else
                    {
                        tinh = (int)dataRow.GetCell(25).NumericCellValue;
                    }
                    item.ID_Tinh = tinh;
                    if (tinh > 0)
                    {
                        Tinh_dl tdl = new Tinh_dl();
                        Tinh t = tdl.GetTinhByID(tinh);
                        item.TenTinh = t != null ? t.TenTinh : "";
                    }

                    // Đọc quận huyện
                    int quan = 0;
                    if (dataRow.GetCell(26, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        //err += " Địa chỉ khách hàng trống ";
                    }
                    else
                    {
                        quan = (int)dataRow.GetCell(26).NumericCellValue;
                    }
                    item.ID_Quan = quan;
                    if (quan > 0)
                    {
                        Quan_dl qdl = new Quan_dl();
                        Quan q = qdl.GetQuanByID(quan);
                        item.TenQuan = q != null ? q.TenQuan : "";
                    }

                    // Đọc xã , phường
                    int phuong = 0;
                    if (dataRow.GetCell(27, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                        //err += " Địa chỉ khách hàng trống ";
                    }
                    else
                    {
                        phuong = (int)dataRow.GetCell(27).NumericCellValue;
                    }
                    item.ID_Phuong = phuong;
                    if (phuong > 0)
                    {
                        Phuong_dl pdl = new Phuong_dl();
                        Phuong p = pdl.GetPhuongByID(phuong);
                        item.TenPhuong = p != null ? p.TenPhuong : "";
                    }

                    // Đọc điện thoại
                    string dienthoai = "";
                    if (dataRow.GetCell(10, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            dienthoai = dataRow.GetCell(10).StringCellValue;
                        }
                        catch
                        {
                            dienthoai = dataRow.GetCell(10).NumericCellValue.ToString();
                        }
                    }
                    item.SoDienThoai = dienthoai;

                    // Đọc fax
                    string fax = "";
                    if (dataRow.GetCell(11, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            fax = dataRow.GetCell(11).StringCellValue;
                        }
                        catch
                        {
                            fax = dataRow.GetCell(11).NumericCellValue.ToString();
                        }
                    }
                    item.Fax = fax;

                    // Đọc điện thoại 2                    
                    string dienthoai2 = "";
                    if (dataRow.GetCell(12, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            dienthoai2 = dataRow.GetCell(12).StringCellValue;
                        }
                        catch
                        {
                            dienthoai2 = dataRow.GetCell(12).NumericCellValue.ToString();
                        }
                    }
                    item.SoDienThoai2 = dienthoai2;

                    // Đọc điện thoại 3
                    string dienthoai3 = "";
                    if (dataRow.GetCell(13, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            dienthoai3 = dataRow.GetCell(13).StringCellValue;
                        }
                        catch
                        {
                            dienthoai3 = dataRow.GetCell(13).NumericCellValue.ToString();
                        }
                    }
                    item.SoDienThoai3 = dienthoai3;

                    // Đọc loại khách hàng
                    int loaikhachhang = 0;
                    if (dataRow.GetCell(28, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                    }
                    else
                    {
                        loaikhachhang = (int)dataRow.GetCell(28).NumericCellValue;
                    }
                    item.ID_LoaiKhachHang = loaikhachhang;
                    if (loaikhachhang > 0)
                    {
                        LoaiKhachHangDB lkhdb = new LoaiKhachHangDB();
                        LoaiKhachHangOBJ lkh = lkhdb.GetLoaiKhachHangById(loaikhachhang);
                        item.TenLoaiKhachHang = lkh != null ? lkh.TenLoaiKhachHang : "";
                    }

                    // Đọc kênh bán hàng
                    int kenh = 0;
                    if (dataRow.GetCell(29, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                    }
                    else
                    {
                        kenh = (int)dataRow.GetCell(29).NumericCellValue;
                    }
                    item.ID_NhomKH = kenh;
                    if (kenh > 0)
                    {
                        KenhBanHangOBJ nhom = KenhBanHangDB.GetKenhBanHangById(kenh);
                        item.TenKenhBanHang = nhom != null ? nhom.TenKenhBanHang : "";
                    }

                    // Đọc kênh bán hàng cấp trên
                    int kenhcaptren = 0;
                    if (dataRow.GetCell(30, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) == null)
                    {
                    }
                    else
                    {
                        kenhcaptren = (int)dataRow.GetCell(30).NumericCellValue;
                    }
                    item.ID_Cha = kenhcaptren;
                    if (kenhcaptren > 0)
                    {
                        NhomOBJ nhom = NhomDB.get_NhomById(kenh);
                        item.TenNhom = nhom != null ? nhom.TenNhom : "";
                    }

                    // Đọc người liên hệ
                    string nguoilienhe = "";
                    if (dataRow.GetCell(17, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            nguoilienhe = dataRow.GetCell(17).StringCellValue;
                        }
                        catch
                        {
                            nguoilienhe = dataRow.GetCell(17).NumericCellValue.ToString();
                        }
                    }
                    item.NguoiLienHe = nguoilienhe;

                    // Đọc hòm thư
                    string homthu = "";
                    if (dataRow.GetCell(18, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            homthu = dataRow.GetCell(18).StringCellValue;
                        }
                        catch
                        {
                            homthu = dataRow.GetCell(18).NumericCellValue.ToString();
                        }
                    }
                    item.Email = homthu;

                    // Đọc website
                    string website = "";
                    if (dataRow.GetCell(19, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            website = dataRow.GetCell(19).StringCellValue;
                        }
                        catch
                        {
                            website = dataRow.GetCell(19).NumericCellValue.ToString();
                        }
                    }
                    item.Website = website;

                    // Đọc địa chỉ xuất hóa đơn
                    string diachihoadon = "";
                    if (dataRow.GetCell(20, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            diachihoadon = dataRow.GetCell(20).StringCellValue;
                        }
                        catch
                        {
                            diachihoadon = dataRow.GetCell(20).NumericCellValue.ToString();
                        }
                    }
                    item.DiaChiXuatHoaDon = diachihoadon;

                    // Đọc số tài khoản
                    string stk = "";
                    if (dataRow.GetCell(21, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            stk = dataRow.GetCell(21).StringCellValue;
                        }
                        catch
                        {
                            stk = dataRow.GetCell(21).NumericCellValue.ToString();
                        }
                    }
                    item.SoTKNganHang = stk;

                    // Đọc mã số thuế
                    string mst = "";
                    if (dataRow.GetCell(22, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            mst = dataRow.GetCell(22).StringCellValue;
                        }
                        catch
                        {
                            mst = dataRow.GetCell(22).NumericCellValue.ToString();
                        }
                    }
                    item.MaSoThue = mst;

                    // Đọc ghi chú
                    string ghichu = "";
                    if (dataRow.GetCell(23, NPOI.SS.UserModel.MissingCellPolicy.RETURN_NULL_AND_BLANK) != null)
                    {
                        try
                        {
                            ghichu = dataRow.GetCell(23).StringCellValue;
                        }
                        catch
                        {
                            ghichu = dataRow.GetCell(23).NumericCellValue.ToString();
                        }
                    }
                    item.GhiChu = ghichu;

                    result.Add(item);

                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                    LSPos_Data.Utilities.Log.Info("Error in data line " + i);
                }

            }
            return result;
        }

        #region import excel khách hàng
        [HttpGet]
        [Route("ExportTeamplateKH")]
        public HttpResponseMessage ExportTeamplateKH()
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
                    DataSet ds = new DataSet();
                    DataTable dtKhuvuc = new DataTable();
                    DataTable dtTinh = new DataTable();
                    DataTable dtQuan = new DataTable();
                    DataTable dtXaPhuong = new DataTable();
                    DataTable dtLoaiKH = new DataTable();
                    DataTable dtKenhBH = new DataTable();
                    // Khu vuc
                    KhuVuc_dl kv = new KhuVuc_dl();
                    List<KhuVuc> lstKhuVuc = kv.GetAll();
                    ExportExcel exp = new ExportExcel();
                    dtKhuvuc = exp.ConvertToDataTable(lstKhuVuc);
                    // Tinh
                    DiaChiData diachi = new DiaChiData();
                    dtTinh = diachi.usp_vuongtm_getdanhsachtinh(userinfo.ID_QLLH);
                    // Quận
                    dtQuan = diachi.usp_vuongtm_getdanhsachquanhuyen();
                    // Xã phường
                    dtXaPhuong = diachi.usp_vuongtm_getdanhsachxaphuong();
                    // loại KH
                    LoaiKhachHangDB lkh = new LoaiKhachHangDB();
                    List<LoaiKhachHangOBJ> lstLoaiKH = lkh.GetListDanhSachLoaiKhachHang(userinfo.ID_QLLH);
                    if (lstLoaiKH != null)
                    {
                        dtLoaiKH = exp.ConvertToDataTable(lstLoaiKH);
                    }
                    // Kênh bán hàng
                    KenhBanHangDB kenhBanHangDB = new KenhBanHangDB();
                    List<KenhBanHangOBJ> lstKBH = KenhBanHangDB.GetListKenhBanHang(userinfo.ID_QLLH);
                    if (lstKBH != null)
                    {
                        dtKenhBH = exp.ConvertToDataTable(lstKBH);
                    }
                    // Kênh bán hàng cấp trên
                    KhachHangData kh_daata = new KhachHangData();
                    DataTable KenhBHCT = kh_daata.GetDataKhachHangBy_IDQLLH(userinfo.ID_QLLH);
                    // string templatefolder = System.Web.Configuration.WebConfigurationManager.AppSettings["ReportTemplatesFolder"];3
                    dt.TableName = "DATA";
                    dtKhuvuc.TableName = "DATA1";
                    dtTinh.TableName = "DATA2";
                    dtQuan.TableName = "DATA3";
                    dtXaPhuong.TableName = "DATA4";
                    dtLoaiKH.TableName = "DATA5";
                    dtKenhBH.TableName = "DATA6";
                    KenhBHCT.TableName = "DATA7";
                    ds.Tables.Add(dt.Copy());
                    ds.Tables.Add(dtKhuvuc.Copy());
                    ds.Tables.Add(dtTinh.Copy());
                    ds.Tables.Add(dtQuan.Copy());
                    ds.Tables.Add(dtXaPhuong.Copy());
                    ds.Tables.Add(dtLoaiKH.Copy());
                    ds.Tables.Add(dtKenhBH.Copy());
                    ds.Tables.Add(KenhBHCT.Copy());
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
                        filename = "BM002_FileMauKhachHang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("TeamplateImport_KhachHang.xls", ds, null, ref stream);
                    }
                    else
                    {
                        filename = "BM002_CustomerTemplate_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToTeamplateImportStreamGird("TeamplateImport_KhachHang_en.xls", ds, null, ref stream);
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
        [Route("importkhachhang")]
        public HttpResponseMessage importkhachhang([FromBody] FileUploadModelFilter file)
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

                    if (validatedatakhachhang(dtDataHeard, userinfo, ref tempPath))
                    {
                        try
                        {
                            KhachHang_dl kh_dl = new KhachHang_dl();
                            foreach (DataRow row in dtDataHeard.Rows)
                            {
                                double kinhdo = 0;
                                double vido = 0;
                                int idTinh = -1;
                                int idKhuVuc = -1;
                                int idQuan = -1;
                                int idPhuong = -1;
                                int idLoaiKhachHang = 0;
                                int idNhomKhachHang = 0;
                                int idCha = 0;

                                KhachHang kh_new = new KhachHang();
                                kh_new.IDKhachHang = 0;
                                kh_new.IDQLLH = userinfo.ID_QLLH;
                                kh_new.ID_QuanLy = userinfo.ID_QuanLy;

                                try
                                {
                                    if ((row["ID_KhuVuc"].ToString() != "") && (row["ID_KhuVuc"].ToString() != "#N/A"))
                                        idKhuVuc = int.Parse(row["ID_KhuVuc"].ToString().Trim());
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }

                                try
                                {
                                    if ((row["ID_Tinh"].ToString() != "") && (row["ID_Tinh"].ToString() != "#N/A"))
                                        idTinh = int.Parse(row["ID_Tinh"].ToString().Trim());
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }

                                try
                                {
                                    if ((row["ID_Quan"].ToString() != "") && (row["ID_Quan"].ToString() != "#N/A"))
                                        idQuan = int.Parse(row["ID_Quan"].ToString().Trim());
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }

                                try
                                {
                                    if ((row["ID_Phuong"].ToString() != "") && (row["ID_Phuong"].ToString() != "#N/A"))
                                        idPhuong = int.Parse(row["ID_Phuong"].ToString().Trim());
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }

                                try
                                {
                                    if ((row["ID_LoaiKhachHang"].ToString() != "") && (row["ID_LoaiKhachHang"].ToString() != "#N/A"))
                                        idLoaiKhachHang = int.Parse(row["ID_LoaiKhachHang"].ToString().Trim());
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }

                                try
                                {
                                    if ((row["ID_KenhBanHang"].ToString() != "") && (row["ID_KenhBanHang"].ToString() != "#N/A"))
                                        idNhomKhachHang = int.Parse(row["ID_KenhBanHang"].ToString().Trim());
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }

                                try
                                {
                                    if ((row["ID_KhachHang"].ToString() != "") && (row["ID_KhachHang"].ToString() != "#N/A"))
                                        idCha = int.Parse(row["ID_KhachHang"].ToString().Trim());
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }

                                try
                                {
                                    if ((row["KinhDo"].ToString() != "") && (row["KinhDo"].ToString() != "#N/A"))
                                        kinhdo = double.TryParse(row["KinhDo"].ToString(), out kinhdo) ? kinhdo : 0;
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }

                                try
                                {
                                    if ((row["ViDo"].ToString() != "") && (row["ViDo"].ToString() != "#N/A"))
                                        vido = double.TryParse(row["ViDo"].ToString(), out vido) ? vido : 0;
                                }
                                catch (Exception ex)
                                {
                                    LSPos_Data.Utilities.Log.Error(ex);
                                }

                                kh_new.KinhDo = kinhdo;
                                kh_new.ViDo = vido;

                                kh_new.ID_LoaiKhachHang = idLoaiKhachHang;
                                kh_new.ID_NhomKH = idNhomKhachHang;
                                kh_new.ID_Cha = idCha;
                                kh_new.ID_Tinh = idTinh;
                                kh_new.ID_KhuVuc = idKhuVuc;
                                kh_new.ID_Quan = idQuan;
                                kh_new.ID_Phuong = idPhuong;

                                kh_new.Ten = row["TenKH"].ToString();
                                kh_new.MaKH = row["MaKH"].ToString();
                                kh_new.DiaChi = row["DiaChi"].ToString();
                                kh_new.Imgurl = "";
                                kh_new.SoDienThoai = row["DienThoai"].ToString();
                                kh_new.SoDienThoai2 = row["DienThoai1"].ToString();
                                kh_new.SoDienThoai3 = row["DienThoai2"].ToString();
                                kh_new.SoDienThoaiMacDinh = row["DienThoai"].ToString();
                                kh_new.Email = row["HomThu"].ToString();
                                kh_new.NguoiLienHe = row["NguoiLienHe"].ToString();
                                kh_new.Fax = row["Fax"].ToString();
                                kh_new.Website = row["Website"].ToString();
                                kh_new.SoTKNganHang = row["SoTaiKhoan"].ToString();
                                kh_new.DuongPho = row["DuongPho"].ToString();
                                kh_new.MaSoThue = row["MaSoThue"].ToString();
                                kh_new.GhiChu = row["GhiChu"].ToString();
                                kh_new.DiaChiXuatHoaDon = row["DiaChiXuatHoaDon"].ToString();

                                if (kh_new.IDKhachHang > 0)
                                    kh_dl.UpdateKhachHang(kh_new, userinfo.ID_QuanLy);
                                else
                                    kh_dl.ThemKhachHangv2(kh_new, userinfo.ID_QuanLy);
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
                            filename = "BM003_FileMauKhachHangError_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        }
                        else
                        {
                            filename = "BM003_CustomerTemplateError_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
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

        private bool validatedatakhachhang(DataTable dtDataHeard, UserInfo userinfo, ref MemoryStream fileStream)
        {
            try
            {
                BaoCaoCommon baocao = new BaoCaoCommon();
                string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                KhachHang_dl kh_dl = new KhachHang_dl();
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
                    if ((row["MaKH"].ToString() != ""))
                    {
                        KhachHang khachhangobj = kh_dl.GetKhachHangTheoMa(userinfo.ID_QLLH, row["MaKH"].ToString(), 0);
                        if (khachhangobj != null)
                        {
                            rowError["MaKH"] = (lang == "vi") ? "Đã tồn tại mã khách hàng trên hệ thống" : "Customer code already exists";
                            IsError = true;
                        }
                    }
                    sError = (lang == "vi") ? "Địa chỉ khách hàng chưa nhập" : "Missing customer's address";
                    ImportValidate.EmptyValue("DiaChi", ref _datarow, ref rowError, ref IsError, sError);
                    sError = (lang == "vi") ? "Tên khách hàng chưa nhập" : "Missing customer's name";
                    ImportValidate.EmptyValue("TenKH", ref _datarow, ref rowError, ref IsError, sError);
                    sError = (lang == "vi") ? "Số điện thoại hàng chưa nhập" : "Missing customer's phone number";
                    ImportValidate.EmptyValue("DienThoai", ref _datarow, ref rowError, ref IsError, sError);
                    if ((row["DienThoai"].ToString() != ""))
                    {
                        Boolean rs = new Boolean();

                        rs = kh_dl.CheckTrungKhachHangTheoSDT_V1(userinfo.ID_QLLH, row["DienThoai"].ToString(), 0);
                        if (!rs)
                        {
                            rowError["DienThoai"] = (lang == "vi") ? "Đã tồn tại số điện thoại khách hàng trên hệ thống" : "Phone number is already exists";
                            IsError = true;
                        }
                    }
                    if (row["KhuVuc"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Khu vực" : "Area";
                        ImportValidate.IsValidList("KhuVuc", "ID_KhuVuc", ref _datarow, ref rowError, ref IsError, sError, lang);
                    }
                    if (row["KhuVuc"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Khu vực" : "Area";
                        ImportValidate.IsValidList("KhuVuc", "ID_KhuVuc", ref _datarow, ref rowError, ref IsError, sError, lang);
                    }
                    if (row["HomThu"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Hòm thư không đúng định dạng" : "Invalid mailbox format";
                        ImportValidate.IsValidEmail("HomThu", ref _datarow, ref rowError, ref IsError, sError);
                    }
                    if (row["KhuVuc"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Khu vực" : "Area";
                        ImportValidate.IsValidList("KhuVuc", "ID_KhuVuc", ref _datarow, ref rowError, ref IsError, sError, lang);
                    }
                    if (row["Tinh"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Tỉnh" : "Province";
                        ImportValidate.IsValidList("Tinh", "ID_Tinh", ref _datarow, ref rowError, ref IsError, sError, lang);
                    }
                    if (row["Quan"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Quận" : "District";
                        ImportValidate.IsValidList("Quan", "ID_Quan", ref _datarow, ref rowError, ref IsError, sError, lang);
                    }
                    if (row["Phuong"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Xã phường" : "Ward";
                        ImportValidate.IsValidList("Phuong", "ID_Phuong", ref _datarow, ref rowError, ref IsError, sError, lang);
                    }
                    if (row["LoaiKhachHang"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Loại khách hàng" : "Customer type";
                        ImportValidate.IsValidList("LoaiKhachHang", "ID_LoaiKhachHang", ref _datarow, ref rowError, ref IsError, sError, lang);
                    }
                    if (row["KenhBanHang"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Kênh bán hàng" : "Sale channel";
                        ImportValidate.IsValidList("KenhBanHang", "ID_KenhBanHang", ref _datarow, ref rowError, ref IsError, sError, lang);
                    }

                    if (row["KenhBanHangCapTren"].ToString() != "")
                    {
                        sError = (lang == "vi") ? "Kênh bán hàng" : "Sale channel";
                        ImportValidate.IsValidList("KenhBanHangCapTren", "ID_KhachHang", ref _datarow, ref rowError, ref IsError, sError, lang);
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
                        ExportExcel.ExportTemplateToStream("TeamplateImport_KhachHang_error.xls", dtError, null, ref fileStream);
                    }
                    else
                    {
                        ExportExcel.ExportTemplateToStream("TeamplateImport_KhachHang_error_en.xls", dtError, null, ref fileStream);
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
        [Route("loadexceldata")]
        public HttpResponseMessage LoadExcelData([FromBody] FileExcelUpload file)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            List<KhachHangParam> lstkhachhang = new List<KhachHangParam>();
            MemoryStream tempPath = new MemoryStream();
            try
            {
                //DataTable dtDataHeard = new DataTable();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
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
                    dt.Columns.Add("DienThoai3", typeof(String));
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
                    DataTable dtDataHeard = new DataTable();
                    string fileName = System.Web.HttpContext.Current.Server.MapPath(file.filename);
                    Aspose.Cells.Workbook workbook;
                    Aspose.Cells.Worksheet worksheet;
                    workbook = new Aspose.Cells.Workbook(fileName);
                    if (workbook.Worksheets.GetSheetByCodeName("DATA") == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "File mẫu không đúng định dạng." });
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
                    if (loadToGrid(dtDataHeard, userinfo.ID_QLLH, ref tempPath))
                    {
                        foreach (DataRow row in dtDataHeard.Rows)
                        {
                            KhachHangParam khachhang = new KhachHangParam();
                            khachhang.MaKH = row["MaKH"].ToString();
                            khachhang.Ten = row["TenKH"].ToString();
                            khachhang.DiaChi = row["DiaChi"].ToString();
                            khachhang.KinhDo = row["KinhDo"].ToString();
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
                            if ((row["ID_KhachHang"].ToString() != "") && (row["ID_KhachHang"].ToString() != "#N/A"))
                            {
                                khachhang.ID_KenhBanHangCapTren = int.Parse(row["ID_KhachHang"].ToString());
                            }
                            khachhang.NguoiLienHe = row["NguoiLienHe"].ToString();

                            khachhang.HomThu = row["HomThu"].ToString();
                            khachhang.Website = row["Website"].ToString();
                            khachhang.DiaChiXuatHoaDon = row["DiaChiXuatHoaDon"].ToString();
                            khachhang.SoTaiKhoan = row["SoTaiKhoan"].ToString();
                            khachhang.MaSoThue = row["MaSoThue"].ToString();
                            khachhang.GhiChu = row["GhiChu"].ToString();
                            lstkhachhang.Add(khachhang);
                        }
                        response = Request.CreateResponse(HttpStatusCode.Created, lstkhachhang);
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
        [Route("checkfile")]
        public HttpResponseMessage CheckFile([FromBody] FileExcelUpload file)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            List<KhachHangParam> lstkhachhang = new List<KhachHangParam>();
            MemoryStream tempPath = new MemoryStream();
            try
            {
                //DataTable dtDataHeard = new DataTable();
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
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
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "File mẫu không đúng định dạng." });
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
                        response = Request.CreateResponse(HttpStatusCode.NoContent, new { success = false, msg = "Không tồn tại bản ghi." });
                        //Trả về lỗi ko có dữ liệu import
                        //response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                    }
                    if (loadToGrid(dtDataHeard, userinfo.ID_QLLH, ref tempPath))
                    {
                        response = Request.CreateResponse(HttpStatusCode.Created, lstkhachhang);
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

        private bool loadToGrid(DataTable dtDataHeard, int ID_QLLH, ref MemoryStream fileStream)
        {
            //try
            //{
            //    KhachHang_dl kh_dl = new KhachHang_dl();
            //    DataTable dtError = dtDataHeard.Clone();
            //    dtError.TableName = "DATA";
            //    int iRow = 4;
            //    bool IsError = false;
            //    foreach (DataRow row in dtDataHeard.Rows)
            //    {
            //        string sError = "";
            //        DataRow _datarow = row as DataRow;
            //        var rowError = dtError.NewRow();
            //        if ((row["MaKH"].ToString() != ""))
            //        {
            //            KhachHang khachhangobj = kh_dl.GetKhachHangTheoMa(ID_QLLH, row["MaKH"].ToString(), 0);
            //            if (khachhangobj != null)
            //            {
            //                rowError["MaKH"] = "Đã tồn tại mã khách hàng trên hệ thống";
            //                IsError = true;
            //            }
            //        }
            //        sError = "Địa chỉ khách hàng chưa nhập";
            //        ImportValidate.EmptyValue("DiaChi", ref _datarow, ref rowError, ref IsError, sError);
            //        sError = "Tên khách hàng chưa nhập";
            //        ImportValidate.EmptyValue("TenKH", ref _datarow, ref rowError, ref IsError, sError);
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
            //        if (row["KhuVuc"].ToString() != "")
            //        {
            //            sError = "Khu vực";
            //            ImportValidate.IsValidList("KhuVuc", "ID_KhuVuc", ref _datarow, ref rowError, ref IsError, sError);
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
            //            ImportValidate.IsValidList("Tinh", "ID_Tinh", ref _datarow, ref rowError, ref IsError, sError, lang);
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
            //        bool success = ExportExcel.ExportTemplateToStream(@"ReportTemplates\TeamplateImport_KhachHang_error.xls", dtError, null, ref fileStream);
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

        #region ThemMoiKhachHang

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

        [HttpPost]
        [Route("savemultidata")]
        public HttpResponseMessage SaveMultiData([FromBody] List<KhachHangParam> lparamkh)
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
                    int count_success = 0;
                    int count_fail = 0;
                    KhachHang_dl kh_dl = new KhachHang_dl();
                    foreach (KhachHangParam paramkh in lparamkh)
                    {
                        count++;
                        bool flag = true;
                        try
                        {
                            KhachHang kh_new = kh_dl.GetKhachHangID(paramkh.IDKhachHang);
                            kh_new.Ten = paramkh.Ten;
                            if (string.IsNullOrWhiteSpace(paramkh.Ten))
                            {
                                flag = false;
                            }
                            if (paramkh.MaKH != "" && kh_new.MaKH != paramkh.MaKH)
                            {
                                KhachHang khachhangobj = kh_dl.GetKhachHangTheoMa(userinfo.ID_QLLH, paramkh.MaKH, 0);
                                if (khachhangobj != null)
                                {
                                    flag = false;
                                }
                            }
                            kh_new.MaKH = paramkh.MaKH;
                            kh_new.DiaChi = paramkh.DiaChi;
                            if (string.IsNullOrWhiteSpace(paramkh.DiaChi))
                            {
                                flag = false;
                            }
                            kh_new.ID_QuanLy = userinfo.ID_QuanLy;
                            kh_new.Imgurl = paramkh.ImgUrl;
                            int idTinh = -1;
                            int idKhuVuc = -1;
                            int idQuan = -1;
                            int idPhuong = -1;
                            try
                            {
                                idTinh = paramkh.ID_Tinh;
                            }
                            catch { }

                            try
                            {
                                idKhuVuc = paramkh.ID_KhuVuc;
                            }
                            catch { }

                            try
                            {
                                idQuan = paramkh.ID_Quan;
                            }
                            catch { }

                            try
                            {
                                idPhuong = paramkh.ID_Phuong;
                            }
                            catch { }

                            kh_new.ID_Tinh = idTinh;
                            kh_new.ID_KhuVuc = idKhuVuc;
                            kh_new.ID_Quan = idQuan;
                            kh_new.ID_Phuong = idPhuong;
                            if (string.IsNullOrWhiteSpace(paramkh.SoDienThoai1))
                            {
                                flag = false;
                            }
                            if (!string.IsNullOrWhiteSpace(paramkh.SoDienThoai1) && !string.IsNullOrWhiteSpace(paramkh.SoDienThoai2))
                            {
                                if (paramkh.SoDienThoai1 == paramkh.SoDienThoai2)
                                {
                                    flag = false;
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(paramkh.SoDienThoai1) && !string.IsNullOrWhiteSpace(paramkh.SoDienThoai3))
                            {
                                if (paramkh.SoDienThoai1 == paramkh.SoDienThoai3)
                                {
                                    flag = false;
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(paramkh.SoDienThoai3) && !string.IsNullOrWhiteSpace(paramkh.SoDienThoai2))
                            {
                                if (paramkh.SoDienThoai3 == paramkh.SoDienThoai2)
                                {
                                    flag = false;
                                }
                            }

                            if (!kh_dl.CheckTrungKhachHangTheoSDT(userinfo.ID_QLLH, paramkh.SoDienThoai1, 0) && kh_new.SoDienThoai != paramkh.SoDienThoai1)
                            {
                                flag = false;
                            }
                            if (!kh_dl.CheckTrungKhachHangTheoSDT(userinfo.ID_QLLH, paramkh.SoDienThoai2, 0) && kh_new.SoDienThoai2 != paramkh.SoDienThoai2)
                            {
                                flag = false;
                            }
                            if (!kh_dl.CheckTrungKhachHangTheoSDT(userinfo.ID_QLLH, paramkh.SoDienThoai3, 0) && kh_new.SoDienThoai3 != paramkh.SoDienThoai3)
                            {
                                flag = false;
                            }
                            kh_new.SoDienThoai = paramkh.SoDienThoai1;
                            kh_new.SoDienThoai2 = paramkh.SoDienThoai2;
                            kh_new.SoDienThoai3 = paramkh.SoDienThoai3;
                            kh_new.SoDienThoaiMacDinh = paramkh.SoDienThoaiMacDinh;
                            if (!string.IsNullOrWhiteSpace(paramkh.Email))
                            {
                                flag = flag && IsValidEmail(paramkh.Email.Trim());
                            }
                            kh_new.Email = paramkh.Email;
                            kh_new.NguoiLienHe = paramkh.NguoiLienHe;
                            if (paramkh.MaSoThue != "" && kh_dl.CheckTrungMST(paramkh.MaSoThue, userinfo.ID_QLLH, 0) && kh_new.MaSoThue != paramkh.MaSoThue)
                            {
                                flag = false;
                            }
                            kh_new.MaSoThue = paramkh.MaSoThue;
                            kh_new.GhiChu = paramkh.GhiChu;
                            kh_new.ID_LoaiKhachHang = paramkh.ID_LoaiKhachHang;
                            kh_new.ID_NhomKH = paramkh.ID_NhomKH;
                            kh_new.ID_Cha = paramkh.ID_Cha;

                            //int idNhanVien = -1;
                            //try
                            //{
                            //    idNhanVien = paramkh.ID_NhanVien;
                            //}
                            //catch { }
                            //kh_new.ID_NhanVien = idNhanVien;

                            kh_new.IDQLLH = userinfo.ID_QLLH;
                            if (flag)
                            {
                                if (kh_dl.UpdateKhachHang(kh_new, userinfo.ID_QuanLy))
                                {
                                    count_success++;
                                }
                                else
                                {
                                    count_fail++;
                                }
                            }
                            else
                            {
                                count_fail++;
                            }
                        }
                        catch (Exception ex)
                        {
                            count_fail++;
                            LSPos_Data.Utilities.Log.Info("Lỗi update thông tin dòng dữ liệu số " + count);
                            LSPos_Data.Utilities.Log.Error(ex);
                        }
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        success = true,
                        msg = count_success > 0 ? "label_luukhachhangthanhcong" : "label_luukhachhangkhongthanhcongvuilonglienhequantri"
                    }); ;
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luukhachhangkhongthanhcongvuilonglienhequantri" });
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }


        [HttpPost]
        [Route("saveexceldata")]
        public HttpResponseMessage SaveExcelData([FromBody] List<KhachHangParam> lparamkh)
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
                    KhachHang_dl kh_dl = new KhachHang_dl();
                    foreach (KhachHangParam paramkh in lparamkh)
                    {
                        count++;
                        try
                        {
                            KhachHang kh_new = new KhachHang();
                            kh_new.Ten = paramkh.Ten;
                            kh_new.MaKH = paramkh.MaKH;
                            kh_new.DiaChi = paramkh.DiaChi;
                            kh_new.ID_QuanLy = userinfo.ID_QuanLy;
                            kh_new.Imgurl = paramkh.ImgUrl;
                            int idTinh = -1;
                            int idKhuVuc = -1;
                            int idQuan = -1;
                            int idPhuong = -1;
                            try
                            {
                                idTinh = paramkh.ID_Tinh;
                            }
                            catch { }

                            try
                            {
                                idKhuVuc = paramkh.ID_KhuVuc;
                            }
                            catch { }

                            try
                            {
                                idQuan = paramkh.ID_Quan;
                            }
                            catch { }

                            try
                            {
                                idPhuong = paramkh.ID_Phuong;
                            }
                            catch { }

                            kh_new.ID_Tinh = idTinh;
                            kh_new.ID_KhuVuc = idKhuVuc;
                            kh_new.ID_Quan = idQuan;
                            kh_new.ID_Phuong = idPhuong;

                            double kinhdo = 0;
                            double vido = 0;
                            kh_new.KinhDo = double.TryParse(paramkh.KinhDo, out kinhdo) ? kinhdo : 0;
                            kh_new.ViDo = double.TryParse(paramkh.ViDo, out vido) ? vido : 0;

                            kh_new.SoDienThoai = paramkh.SoDienThoai1;
                            kh_new.SoDienThoai2 = paramkh.SoDienThoai2;
                            kh_new.SoDienThoai3 = paramkh.SoDienThoai3;
                            kh_new.SoDienThoaiMacDinh = paramkh.SoDienThoaiMacDinh;

                            kh_new.Email = paramkh.Email;
                            kh_new.NguoiLienHe = paramkh.NguoiLienHe;
                            kh_new.Fax = paramkh.Fax;
                            kh_new.Website = paramkh.Website;
                            kh_new.SoTKNganHang = paramkh.SoTKNganHang;
                            kh_new.DuongPho = paramkh.DuongPho;
                            kh_new.MaSoThue = paramkh.MaSoThue;
                            kh_new.GhiChu = paramkh.GhiChu;

                            kh_new.ID_LoaiKhachHang = paramkh.ID_LoaiKhachHang;
                            kh_new.ID_NhomKH = paramkh.ID_NhomKH;
                            kh_new.ID_Cha = paramkh.ID_Cha;

                            //int idNhanVien = -1;
                            //try
                            //{
                            //    idNhanVien = paramkh.ID_NhanVien;
                            //}
                            //catch { }
                            //kh_new.ID_NhanVien = idNhanVien;

                            kh_new.IDQLLH = userinfo.ID_QLLH;
                            kh_new.DiaChiXuatHoaDon = paramkh.DiaChiXuatHoaDon;
                            kh_new.IDKhachHang = paramkh.IDKhachHang;
                            int Id = kh_new.IDKhachHang;
                            if (kh_new.IDKhachHang > 0)
                            {
                                kh_dl.UpdateKhachHang(kh_new, userinfo.ID_QuanLy);
                            }
                            else
                            {
                                Id = kh_dl.ThemKhachHangv2(kh_new, userinfo.ID_QuanLy);
                            }
                        }
                        catch (Exception ex)
                        {
                            LSPos_Data.Utilities.Log.Info("Lỗi insert thông tin dòng dữ liệu số " + count);
                            LSPos_Data.Utilities.Log.Error(ex);
                        }
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_luuthanhcong" });
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luukhachhangkhongthanhcongvuilonglienhequantri" });
                LSPos_Data.Utilities.Log.Error(ex);
            }
            return response;
        }


        [HttpPost]
        [Route("themmoi")]
        public HttpResponseMessage Add([FromBody] KhachHangParam paramkh)
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
                    KhachHang kh_new = new KhachHang();
                    try
                    {
                        #region validation
                        bool flag = true;
                        bool update = false;
                        if (paramkh.IDKhachHang > 0)
                        {
                            kh_new = kh_dl.GetKhachHangID(paramkh.IDKhachHang);
                            update = true;
                            if (paramkh.MaSoThue != "" && kh_dl.CheckTrungMST(paramkh.MaSoThue, userinfo.ID_QLLH, 0) && flag && !update)
                            {
                                flag = false;
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_masothuedatontaivuilongkiemtralai" });
                            }
                            if (paramkh.Email.Trim().Length > 0 && flag)
                            {
                                if (!IsValidEmail(paramkh.Email.Trim()))
                                {
                                    flag = false;
                                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luuthatbaiemailkhongdungdinhdang" });
                                }
                            }
                        }
                        else
                        {
                            if (paramkh.MaKH != "" && flag)
                            {
                                KhachHang khachhangobj = kh_dl.GetKhachHangTheoMa(userinfo.ID_QLLH, paramkh.MaKH, 0);
                                if (khachhangobj != null && !update)
                                {
                                    flag = false;
                                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luuthatbaitrungmakhachhang" });
                                }
                            }
                            if (paramkh.Ten == "" && flag)
                            {
                                flag = false;
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luuthatbaitenkhongduocdetrong" });
                            }

                            if (paramkh.DiaChi == "" && flag)
                            {
                                flag = false;
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luuthatbaidiachikhongduocdetrong" });
                            }
                            if (paramkh.MaSoThue != "" && kh_dl.CheckTrungMST(paramkh.MaSoThue, userinfo.ID_QLLH, 0) && flag && !update)
                            {
                                flag = false;
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_masothuedatontaivuilongkiemtralai" });
                            }
                            if (paramkh.Email.Trim().Length > 0 && flag)
                            {
                                if (!IsValidEmail(paramkh.Email.Trim()))
                                {
                                    flag = false;
                                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luuthatbaiemailkhongdungdinhdang" });
                                }
                            }

                            if (flag)
                            {
                                bool rs = kh_dl.CheckTrungKhachHangTheoSDT_V1(userinfo.ID_QLLH, paramkh.SoDienThoai1, 0);
                                if (rs)
                                {
                                    if (paramkh.SoDienThoaiMacDinh.Trim().Length > 0)
                                    {
                                        if (paramkh.SoDienThoaiMacDinh.Trim().Length > 15)
                                        {
                                            flag = false;
                                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luuthatbaisodienthoaiquadai" });
                                        }
                                        else
                                        {
                                            foreach (char c in paramkh.SoDienThoaiMacDinh.Trim())
                                            {
                                                if (c < '0' || c > '9')
                                                {
                                                    flag = false;
                                                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luuthatbaisodienthoai1chiduocnhapso" });
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        flag = false;
                                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luuthatbaisodienthoaimacdinhkhongduoctrong" });
                                    }
                                }
                                else
                                {
                                    string ThongBao = "label_luuthatbai";
                                    if (rs == false)
                                    {
                                        if (string.IsNullOrWhiteSpace(paramkh.SoDienThoai1))
                                            ThongBao = "label_sodienthoaikhongduocbotrong";
                                        else
                                            ThongBao = "label_sodienthoaidabitrung";
                                    }
                                    flag = false;
                                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = ThongBao });
                                }
                            }
                        }
                        #endregion

                        if (flag)
                        {
                            double kinhdo = 0;
                            double vido = 0;

                            try
                            {
                                kinhdo = double.Parse(paramkh.KinhDo);
                            }
                            catch (Exception ex)
                            {
                                LSPos_Data.Utilities.Log.Error(ex);
                            }

                            try
                            {
                                vido = double.Parse(paramkh.ViDo);
                            }
                            catch (Exception ex)
                            {
                                LSPos_Data.Utilities.Log.Error(ex);
                            }

                            kh_new.IDQLLH = userinfo.ID_QLLH;
                            kh_new.ID_QuanLy = userinfo.ID_QuanLy;
                            kh_new.IDKhachHang = paramkh.IDKhachHang;

                            kh_new.ID_KhuVuc = paramkh.ID_KhuVuc;
                            kh_new.ID_Tinh = paramkh.ID_Tinh;
                            kh_new.ID_Quan = paramkh.ID_Quan;
                            kh_new.ID_Phuong = paramkh.ID_Phuong;
                            kh_new.ID_LoaiKhachHang = paramkh.ID_LoaiKhachHang;
                            kh_new.ID_NhomKH = paramkh.ID_NhomKH;
                            kh_new.ID_Cha = paramkh.ID_Cha;

                            kh_new.KinhDo = kinhdo;
                            kh_new.ViDo = vido;

                            kh_new.Ten = paramkh.Ten;
                            kh_new.MaKH = paramkh.MaKH;
                            kh_new.DiaChi = paramkh.DiaChi;
                            kh_new.SoDienThoai = paramkh.SoDienThoai1;
                            kh_new.SoDienThoai2 = paramkh.SoDienThoai2;
                            kh_new.SoDienThoai3 = paramkh.SoDienThoai3;
                            kh_new.SoDienThoaiMacDinh = paramkh.SoDienThoaiMacDinh;
                            kh_new.Email = paramkh.Email;
                            kh_new.NguoiLienHe = paramkh.NguoiLienHe;
                            kh_new.Fax = paramkh.Fax;
                            kh_new.Website = paramkh.Website;
                            kh_new.SoTKNganHang = paramkh.SoTKNganHang;
                            kh_new.DuongPho = paramkh.DuongPho;
                            kh_new.MaSoThue = paramkh.MaSoThue;
                            kh_new.GhiChu = paramkh.GhiChu;
                            kh_new.DiaChiXuatHoaDon = paramkh.DiaChiXuatHoaDon;

                            kh_new.Imgurl = paramkh.ImgUrl;

                            int Id = kh_new.IDKhachHang;

                            if (userinfo.IsAdmin)
                            {
                                kh_new.ID_NhanVien = 0;
                            }
                            else
                            {
                                kh_new.ID_NhanVien = paramkh.ID_NhanVien;
                            }

                            if (update)
                                kh_dl.UpdateKhachHang(kh_new, userinfo.ID_QuanLy);
                            else
                                Id = kh_dl.ThemKhachHangv2(kh_new, userinfo.ID_QuanLy);

                            if (Id > 0)
                            {
                                //try
                                //{
                                //    if (!string.IsNullOrWhiteSpace(kh_new.Imgurl))
                                //    {
                                //        string teampatch = AppDomain.CurrentDomain.BaseDirectory + kh_new.Imgurl;
                                //        if (File.Exists(teampatch))
                                //        {
                                //            byte[] binData = GetBytesFromFile(teampatch);

                                //            if (binData != null && binData.Length > 0)
                                //            {
                                //                //string svURL = Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]);
                                //                string strLinkServer = svURL + "/AppUpload_AnhKhachHang.aspx?token=6e22b116f5111220741848ccd290e9e9062522d88a1fb00ba9b168db7a480271&idnhanvien=0&kinhdo=0&vido=0&ghichu=Upload ảnh từ web&thoigianchup="
                                //                    + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "&idkhachhang=" + Id + "&idqllh=" + userinfo.ID_QLLH + "&imagename=" + "test.jpg";
                                //                PostMultipleFiles_Stream(strLinkServer, binData, "test.jpg");
                                //            }
                                //        }
                                //    }
                                //}
                                //catch (Exception ex)
                                //{
                                //    LSPos_Data.Utilities.Log.Error(ex);
                                //}

                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, data = Id, msg = "label_luukhachhangthanhcong" });
                            }
                            else
                            {
                                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luukhachhangkhongthanhcongvuilonglienhequantri" });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LSPos_Data.Utilities.Log.Error(ex);
                        response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luukhachhangkhongthanhcongvuilonglienhequantri" });
                    }
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luukhachhangkhongthanhcongvuilonglienhequantri" });
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dangkythongtin")]
        public HttpResponseMessage AddContact([FromBody] KhachHangParam paramkh)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                KhachHang_dl kh_dl = new KhachHang_dl();
                KhachHang kh_new = new KhachHang();
                try
                {
                    #region validation
                    bool flag = true;
                    bool update = false;


                    if (paramkh.Ten == "" && flag)
                    {
                        flag = false;
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luuthatbaitenkhongduocdetrong" });
                    }

                    if (paramkh.Email.Trim().Length > 0 && flag)
                    {
                        if (!IsValidEmail(paramkh.Email.Trim()))
                        {
                            flag = false;
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luuthatbaiemailkhongdungdinhdang" });
                        }
                    }

                    if (flag)
                    {
                        bool rs = kh_dl.CheckTrungKhachHangTheoSDT_V1(1, paramkh.SoDienThoai1, 0);
                        if (rs)
                        {
                            if (paramkh.SoDienThoaiMacDinh.Trim().Length > 0)
                            {
                                if (paramkh.SoDienThoaiMacDinh.Trim().Length > 15)
                                {
                                    flag = false;
                                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luuthatbaisodienthoaiquadai" });
                                }
                                else
                                {
                                    foreach (char c in paramkh.SoDienThoaiMacDinh.Trim())
                                    {
                                        if (c < '0' || c > '9')
                                        {
                                            flag = false;
                                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luuthatbaisodienthoai1chiduocnhapso" });
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                flag = false;
                                response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_luuthatbaisodienthoaimacdinhkhongduoctrong" });
                            }
                        }
                        else
                        {
                            string ThongBao = "label_luuthatbai";
                            if (rs == false)
                            {
                                if (string.IsNullOrWhiteSpace(paramkh.SoDienThoai1))
                                    ThongBao = "label_sodienthoaikhongduocbotrong";
                                else
                                    ThongBao = "label_sodienthoaidabitrung";
                            }
                            flag = false;
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = ThongBao });
                        }
                    }

                    #endregion

                    if (flag)
                    {
                        double kinhdo = 0;
                        double vido = 0;

                        try
                        {
                            kinhdo = double.Parse(paramkh.KinhDo);
                        }
                        catch (Exception ex)
                        {
                            LSPos_Data.Utilities.Log.Error(ex);
                        }

                        try
                        {
                            vido = double.Parse(paramkh.ViDo);
                        }
                        catch (Exception ex)
                        {
                            LSPos_Data.Utilities.Log.Error(ex);
                        }

                        kh_new.IDQLLH = 1;
                        kh_new.ID_QuanLy = 1;
                        kh_new.IDKhachHang = paramkh.IDKhachHang;

                        kh_new.ID_KhuVuc = paramkh.ID_KhuVuc;
                        kh_new.ID_Tinh = paramkh.ID_Tinh;
                        kh_new.ID_Quan = paramkh.ID_Quan;
                        kh_new.ID_Phuong = paramkh.ID_Phuong;
                        kh_new.ID_LoaiKhachHang = paramkh.ID_LoaiKhachHang;
                        kh_new.ID_NhomKH = paramkh.ID_NhomKH;
                        kh_new.ID_Cha = paramkh.ID_Cha;

                        kh_new.KinhDo = kinhdo;
                        kh_new.ViDo = vido;

                        kh_new.Ten = paramkh.Ten;
                        kh_new.MaKH = paramkh.MaKH;
                        kh_new.DiaChi = paramkh.DiaChi;
                        kh_new.SoDienThoai = paramkh.SoDienThoai1;
                        kh_new.SoDienThoai2 = paramkh.SoDienThoai2;
                        kh_new.SoDienThoai3 = paramkh.SoDienThoai3;
                        kh_new.SoDienThoaiMacDinh = paramkh.SoDienThoaiMacDinh;
                        kh_new.Email = paramkh.Email;
                        kh_new.NguoiLienHe = paramkh.NguoiLienHe;
                        kh_new.Fax = paramkh.Fax;
                        kh_new.Website = paramkh.Website;
                        kh_new.SoTKNganHang = paramkh.SoTKNganHang;
                        kh_new.DuongPho = paramkh.DuongPho;
                        kh_new.MaSoThue = paramkh.MaSoThue;
                        kh_new.GhiChu = paramkh.GhiChu;
                        kh_new.DiaChiXuatHoaDon = paramkh.DiaChiXuatHoaDon;

                        kh_new.Imgurl = paramkh.ImgUrl;

                        int Id = kh_new.IDKhachHang;

                        kh_new.ID_NhanVien = paramkh.ID_NhanVien;

                        Id = kh_dl.ThemKhachHangv2(kh_new, 1);

                        if (Id > 0)
                        {
                            EmailHelper helper = new EmailHelper();
                            helper.SendEmail("Tên khách hàng: " + kh_new.Ten + ", điện thoại: " + kh_new.SoDienThoaiMacDinh + ", email: " + kh_new.Email, "mkt@mspace.com.vn", null, "[Thông báo] Khách hàng để lại thông tin nhận tư vấn!");
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, data = Id, msg = "label_luukhachhangthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luukhachhangkhongthanhcongvuilonglienhequantri" });
                        }
                    }
                }
                catch (Exception ex)
                {
                    LSPos_Data.Utilities.Log.Error(ex);
                    response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luukhachhangkhongthanhcongvuilonglienhequantri" });
                }
            }

            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, new { success = false, msg = "label_luukhachhangkhongthanhcongvuilonglienhequantri" });
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
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
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        [HttpPost]
        [Route("deleteKhachHang")]
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
                    KhachHang_dl kh_dl = new KhachHang_dl();
                    foreach (int id in Ids)
                    {
                        kh_dl.DeleteKhachHang(userinfo.ID_QLLH, id);
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
        [Route("deleteall")]
        public HttpResponseMessage deleteall()
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
                    KhachHangData khachHangData = new KhachHangData();
                    int re = khachHangData.DeleteAllKhachHang(userinfo.ID_QLLH, userinfo.ID_QuanLy);
                    if (re > 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoakhachhangthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoathatbaixinvuilongthulai" });
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
        [Route("getallKH")]

        [AllowAnonymous]
        [HttpPost]
        [Route("ExportExcelKhachHang")]
        public HttpResponseMessage ExportExcelKhachHang([FromBody] ExportKhachHangDTO obj)
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
                    KhachHangData khd = new KhachHangData();
                    DataSet ds = khd.ExportDataKhachHang_Kendo(userinfo.ID_QLLH, userinfo.ID_QuanLy, obj);
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
                        filename = "BM001_DanhSachKhachHang_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("ExcelKhachHang.xls", dataSet, null, ref stream);
                    }
                    else
                    {
                        filename = "BM001_CustomerList_" + DateTime.Now.ToString("ddMMMyyyy") + ".xls";
                        excel.ExportTemplateToStreamGird("ExcelKhachHang_en.xls", dataSet, null, ref stream);
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

        public HttpResponseMessage getKHall()
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
                    DataTable dskh = kh_dl.GetDataKhachHangAll(userinfo.ID_QLLH, userinfo.ID_QuanLy, 0, 0, -1, 0);
                    dskh = dskh.Select("TenKhachHang not like 'sontest%'").CopyToDataTable();
                    List<KhachHang> lstKhachHang = new List<KhachHang>();
                    KhachHang tatca = new KhachHang();
                    tatca.IDKhachHang = 0;
                    tatca.Ten = "Tất cả";
                    lstKhachHang.Add(tatca);
                    foreach (DataRow dr in dskh.Rows)
                    {
                        KhachHang kh = kh_dl.GetKhachHangFromDataRow(dr);

                        if (kh != null)
                            lstKhachHang.Add(kh);
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, lstKhachHang);
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
        private class RequestGridParam
        {
            public DataSourceRequest request { get; set; }
            public TieuChiLoc tieuchiloc { get; set; }
        }
        private class RequestGridParam_ByNhanVien
        {
            public DataSourceRequest request { get; set; }
            public string ID_NhanViens { get; set; }
        }
        public class ParamKhach
        {
            public string type { set; get; }
            public int ID_KhachHang { set; get; }
            public string DienThoai { set; get; }
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
