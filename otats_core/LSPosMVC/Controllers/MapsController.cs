using LSPosMVC.Common;
using LSPos_Data.Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/map")]
    public class MapsController : ApiController
    {
        #region DetectTinhQuanHuyen
        public class Param
        {
            public string type { set; get; }
            public string diachi { set; get; }
        }
        public class ParamGetKhachHang
        {
            public string kinhdo { set; get; }
            public string vido { set; get; }
            public string ID_Tinh { set; get; }
            public string ID_Quan { set; get; }
            public string ID_LoaiKhachHang { set; get; }
            
        }

        public class ParamGetLoTrinh
        {
            public int minkhoangcach { set; get; }
            public int thamsohienthi { set; get; }
            public int khongnoidiem { set; get; }
            public int idnhanvien { set; get; }
            public int thoigianlaybantin { set; get; }
            public int loaiLoTrinh { set; get; }

            public string tungay { set; get; }
            public string denngay { set; get; }
            public double tocDoHopLeToiDa { set; get; }

        }
        [HttpPost]
        [Route("DetectTinhQuanHuyen")]
        public HttpResponseMessage Detect([FromBody] Param parameters)
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
                    string type = (parameters.type != null) ? parameters.type : "";
                    objID a = new objID() { ID_Tinh = 0, ID_Quan = 0, ID_Phuong = 0 };

                    if (type.Equals("LDC"))
                    {
                        string diachi = (parameters.diachi != null) ? parameters.diachi : "";
                        a = DetectDiaChi(diachi);
                    }
                    if (type.Equals("Tinh"))
                    {
                        Quan_dl quan_dl = new Quan_dl();
                        int ID_Tinh = 0;
                        try
                        {
                            ID_Tinh = (parameters.diachi != null) ? Convert.ToInt32(parameters.diachi) : 0;
                        }
                        catch { }

                        a.dsquan = quan_dl.GetQuanTheoTinh(ID_Tinh);
                    }
                    if (type.Equals("Quan"))
                    {
                        Phuong_dl phuong_dl = new Phuong_dl();
                        int ID_Quan = 0;
                        try
                        {
                            ID_Quan = (parameters.diachi != null) ? Convert.ToInt32(parameters.diachi) : 0;
                        }
                        catch { }

                        a.dsphuong = phuong_dl.GetPhuongTheoQuan(ID_Quan);
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, a);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
        public int detectquan(string[] input, List<Quan> a)
        {
            int rs = 0;
            foreach (Quan q in a)
            {
                if (q.TenQuan.ToUpper().Contains(input[input.Length - 3].ToString().Trim().ToUpper()))
                {
                    rs = q.ID_Quan;
                    break;
                }
            }
            return rs;
        }
        public int detectphuong(string[] input, List<Phuong> a)
        {
            int rs = 0;
            foreach (Phuong p in a)
            {
                if (p.TenPhuong.ToUpper().Contains(input[input.Length - 4].ToString().Trim().ToUpper()))
                {
                    rs = p.ID_Phuong;
                    break;
                }
            }
            return rs;
        }
        public objID DetectDiaChi(string diachi)
        {
            objID obj = new objID();
            try
            {
                Tinh_dl tinhdl = new Tinh_dl();
                List<Tinh> listTinh = tinhdl.GetTinhAll();
                string[] dcArr = diachi.Split(',');

                if (dcArr[dcArr.Length - 1].Trim().ToUpper() == "VIETNAM" || dcArr[dcArr.Length - 1].Trim().ToUpper() == "VIỆT NAM")
                {
                    Quan_dl quandl = new Quan_dl();
                    Phuong_dl phuongdl = new Phuong_dl();

                    for (int i = 0; i < listTinh.Count; i++)
                    {
                        if (dcArr[dcArr.Length - 2].Trim().ToUpper() == listTinh[i].TenTinh.ToUpper())
                        {
                            obj.ID_Tinh = listTinh[i].ID_Tinh;
                            List<Quan> listQuan = quandl.GetQuanTheoTinh(listTinh[i].ID_Tinh);
                            obj.ID_Quan = detectquan(dcArr, listQuan);
                            List<Phuong> listPhuong = phuongdl.GetPhuongTheoQuan(obj.ID_Quan);
                            obj.ID_Phuong = detectphuong(dcArr, listPhuong);
                            obj.dsquan = listQuan;
                            obj.dsphuong = listPhuong;
                            break;

                        }
                        else if ("TP " + dcArr[dcArr.Length - 2].Trim().ToUpper() == listTinh[i].TenTinh.ToUpper())
                        {
                            obj.ID_Tinh = listTinh[i].ID_Tinh;
                            List<Quan> listQuan = quandl.GetQuanTheoTinh(listTinh[i].ID_Tinh);
                            obj.ID_Quan = detectquan(dcArr, listQuan);
                            List<Phuong> listPhuong = phuongdl.GetPhuongTheoQuan(obj.ID_Quan);
                            obj.ID_Phuong = detectphuong(dcArr, listPhuong);
                            obj.dsquan = listQuan;
                            obj.dsphuong = listPhuong;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return obj;
        }
        public class objID
        {
            public int ID_Tinh { get; set; }
            public int ID_Quan { get; set; }
            public List<Quan> dsquan { get; set; }
            public int ID_Phuong { get; set; }
            public List<Phuong> dsphuong { get; set; }
        }

        #endregion

        #region GetToaDoDiaChi
        [HttpPost]
        [Route("GetToaDoDiaChi")]
        public HttpResponseMessage get([FromBody] Param parameters)
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
                    Location toado = new Location();

                    string type = (parameters.type != null) ? parameters.type : "";
                    if (type.Equals("getGeo"))
                    {
                        string diachi = (parameters.diachi != null) ? parameters.diachi.ToString() : "";

                        toado = GetToaDoByDiaDiem(diachi);

                        if (toado != null && toado.Longitude > 0)
                        {
                            toado.ID_TinhThanh = Tinh_dl.GetTinhByTen(toado.Tinh).ID_Tinh;
                            if (toado.QuanHuyen != null && toado.QuanHuyen != "")
                                toado.ID_QuanHuyen = Quan_dl.GetQuanByTen(toado.QuanHuyen, toado.ID_TinhThanh).ID_Quan;
                            if (toado.PhuongXa != null && toado.PhuongXa != "")
                                toado.ID_PhuongXa = Phuong_dl.GetPhuongByTen(toado.PhuongXa, toado.ID_QuanHuyen).ID_Phuong;
                        }
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, toado);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, new Geo());
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
        public static Location GetToaDoByDiaDiem(string DiaDiem)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(typeof(LayViTriTheoToaDo));
            Location obj = new Location();

            // string apiKey = "AIzaSyBnwO1ETMtZC7AonESIQbpnwNaPvBhqVnI";
            string apiKey = "";
            if (ConfigurationManager.AppSettings["GOOGLEAPIKEY"] != null)
            {
                apiKey = ConfigurationManager.AppSettings["GOOGLEAPIKEY"];

            }
            string url = "https://maps.google.com/maps/api/geocode/xml?address={0}&sensor=false&key=" + apiKey;
            url = string.Format(url, DiaDiem);
            WebRequest request = WebRequest.Create(url);
            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    try
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(reader.ReadToEnd());
                        //  obj.status = doc.DocumentElement.ChildNodes[0].ChildNodes[0].InnerText;
                        XmlNode result = doc.DocumentElement.ChildNodes[1];
                        if (result != null)
                        {
                            foreach (XmlNode address_component in result.ChildNodes)
                            {
                                if (address_component.Name == "geometry")
                                {
                                    //check type

                                    XmlNodeList type = address_component.SelectNodes("location");

                                    foreach (XmlNode typenode in type)
                                    {
                                        foreach (XmlNode loca in typenode.ChildNodes)
                                        {
                                            if (loca.Name == "lat")
                                                obj.Latitude = double.Parse(loca.InnerText);
                                            else if (loca.Name == "lng")
                                                obj.Longitude = double.Parse(loca.InnerText);
                                        }
                                    }
                                }
                                else if (address_component.Name == "address_component")
                                {
                                    //check type

                                    XmlNodeList type = address_component.SelectNodes("type");
                                    bool isKhac = true;
                                    foreach (XmlNode typenode in type)
                                    {
                                        if (typenode.InnerText == "country")
                                        {
                                            isKhac = false;
                                            break;
                                        }
                                        else if (typenode.InnerText == "locality")
                                        {
                                            isKhac = false;
                                            break;
                                        }
                                        if (typenode.InnerText == "administrative_area_level_1")
                                        {
                                            isKhac = false;
                                            obj.Tinh = address_component.ChildNodes[0].InnerText;
                                            break;
                                        }
                                        else if (typenode.InnerText == "administrative_area_level_2")
                                        {
                                            isKhac = false;
                                            obj.QuanHuyen = address_component.ChildNodes[0].InnerText;
                                            break;
                                        }
                                        else if (typenode.InnerText == "sublocality_level_1")
                                        {
                                            isKhac = false;
                                            obj.PhuongXa = address_component.ChildNodes[0].InnerText;
                                            break;
                                        }
                                        else if (typenode.InnerText == "route")
                                        {
                                            isKhac = false;
                                            obj.Duong = address_component.ChildNodes[0].InnerText;
                                            break;
                                        }
                                        else if (typenode.InnerText == "street_number")
                                        {
                                            isKhac = false;
                                            obj.SoNha = address_component.ChildNodes[0].InnerText;
                                            break;
                                        }
                                        else if (typenode.InnerText == "neighborhood")
                                        {
                                            isKhac = false;
                                            obj.LangBan = address_component.ChildNodes[0].InnerText;
                                            break;
                                        }
                                    }
                                    if (isKhac)
                                    {
                                        if (obj.Khac != null && obj.Khac.Length > 0)
                                        {
                                            obj.Khac += ", ";
                                        }
                                        obj.Khac += address_component.ChildNodes[0].InnerText;
                                    }

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }

                    return obj;
                    //return dsResult.Tables["result"].Rows[0]["formatted_address"].ToString();
                }
            }
        }
        public class Geo
        {
            public string Geolat { get; set; }
            public string Geolong { get; set; }
        }
        #endregion


        [HttpPost]
        [Route("GetVitri_KhachHang")]
        public HttpResponseMessage GetVitri_KhachHang([FromBody] ParamGetKhachHang parameters)
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
                    DanhSachTatCaDaiLyOBJ OBJ = new DanhSachTatCaDaiLyOBJ();
                    double kinhdo = (parameters.kinhdo != null && parameters.kinhdo != "") ? double.Parse(parameters.kinhdo) : 0;
                    double vido = (parameters.vido != null && parameters.vido != "") ? double.Parse(parameters.vido) : 0;
                    int ID_Tinh = (parameters.ID_Tinh != null && parameters.ID_Tinh != "") ? int.Parse(parameters.ID_Tinh) : 0;
                    int ID_Quan = (parameters.ID_Quan != null && parameters.ID_Quan != "") ? int.Parse(parameters.ID_Quan) : 0;
                    int ID_LoaiKhachHang = (parameters.ID_LoaiKhachHang  != null && parameters.ID_LoaiKhachHang != "") ? int.Parse(parameters.ID_LoaiKhachHang) : -1;
                    OBJ.status = true;
                    OBJ.msg = "Thành công";
                    if (kinhdo > 0)
                    {
                        OBJ.data = KhachHang_dl.TatCaKhachHangTheoIDCT_ToaDo(userinfo.ID_QLLH, userinfo.ID_QuanLy, kinhdo, vido, ID_Tinh, ID_Quan, ID_LoaiKhachHang);
                    }
                    else
                    {
                        OBJ.data = KhachHang_dl.TatCaKhachHangTheoIDCT(userinfo.ID_QLLH, userinfo.ID_QuanLy, ID_Tinh, ID_Quan, ID_LoaiKhachHang);
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, OBJ);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, new Geo());
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpPost]
        [Route("GetVitri_NhanVienOnline")]
        public HttpResponseMessage GetVitri_NhanVienOnline([FromBody]  int trangthai)
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
                    ViTriTatCaNhanVienOBJ OBJ = new ViTriTatCaNhanVienOBJ();
                    
                    OBJ.status = true;
                    OBJ.msg = "Thành công";
                    OBJ.data =  LoTrinhDB.GetViTriTatCaNVOnline(userinfo.ID_QLLH, userinfo.ID_QuanLy,  trangthai );
                  

                    response = Request.CreateResponse(HttpStatusCode.OK, OBJ);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, new Geo());
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        [HttpPost]
        [Route("GetVitri_NhanVien")]
        public HttpResponseMessage GetVitri_NhanVien([FromBody]   int idnhom, int loctrangthai)
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
                    ViTriTatCaNhanVienOBJ OBJ = new ViTriTatCaNhanVienOBJ();

                    OBJ.status = true;
                    OBJ.msg = "Thành công";
                    OBJ.data = LoTrinhDB.GetViTriTatCaNV(userinfo.ID_QLLH, userinfo.ID_QuanLy, idnhom, loctrangthai);

                    response = Request.CreateResponse(HttpStatusCode.OK, OBJ);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, new Geo());
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }
        log4net.ILog log = log4net.LogManager.GetLogger(typeof(LayViTriTheoToaDo));
        [HttpPost]
        [Route("GetLoTrinh_NhanVien")]
        public HttpResponseMessage GetLoTrinh_NhanVien([FromBody] ParamGetLoTrinh parameters)
        {
           
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            if (parameters.minkhoangcach == 0)
                parameters.minkhoangcach = 100;

            if (parameters.thamsohienthi == 0)
                parameters.thamsohienthi = 1;

           
            if (parameters.thoigianlaybantin == 0)
                parameters.thoigianlaybantin = 20000;

            if (parameters.loaiLoTrinh == 0)
                parameters.loaiLoTrinh = 1;//1: suy dien, 0: theo thiet bi gps
            if (parameters.tocDoHopLeToiDa == 0)
                parameters.tocDoHopLeToiDa = 300;
       
            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    OBJdataLotrinh OBJ = new OBJdataLotrinh();
                   
                    OBJ.status = true;
                    OBJ.msg = "Thành công";
                    if (parameters.tungay != "")
                    {
                        DateTime tungayF = new DateTime(1900, 01, 01);
                        DateTime denngayF = new DateTime(1900, 01, 01);
                        try
                        {
                            tungayF = DateTime.ParseExact(parameters.tungay, new string[] { "dd-MM-yyyy H:m", "dd/MM/yyyy H:m" }, null, DateTimeStyles.None);
                            denngayF = DateTime.ParseExact(parameters.denngay, new string[] { "dd-MM-yyyy H:m", "dd/MM/yyyy H:m" }, null, DateTimeStyles.None);

                        }
                        catch (Exception)
                        {


                        }
                        if (tungayF.Year > 1900)
                        {
                            if ((denngayF - tungayF).TotalDays > 5)
                            {
                                OBJ.msg = "Khoảng cách giữa 2 ngày không được vượt quá 5 ngày";

                            }
                            else if (tungayF > denngayF)
                            {
                                OBJ.msg = "Khoảng thời gian từ ngày không được phép lớn hơn đến ngày, vui lòng thử lại";

                            }
                            else
                            {
                                KeHoachDiChuyen_dl khdc = new KeHoachDiChuyen_dl();
                                List<KeHoachDiChuyenObj> lstKeHoach = khdc.GetKeHoachTheoNhanVien_Moi(parameters.idnhanvien, tungayF, denngayF, userinfo.ID_QLLH, userinfo.ID_QuanLy);


                                DataTable dt = LoTrinhDB.LichSuDiChuyenTheoNhanVien_Online_Offline_v2(parameters.idnhanvien, tungayF, denngayF, userinfo.ID_QLLH, userinfo.ID_QuanLy);
                                List<lotrinhmattinhieuOBJ> lotrinhmattinhieu_new = new List<lotrinhmattinhieuOBJ>();
                                List<Point> lpoint = new List<Point>();
                                List<Point> lpoint_Offline = new List<Point>();
                                int x = 0;
                                int j = 0;
                                int dem = 0;
                              
                                DataTable dtLoTrinh = new DataTable();
                                dtLoTrinh.Columns.Add("STT");
                                dtLoTrinh.Columns.Add("Nhan vien");
                                dtLoTrinh.Columns.Add("Thoi gian");
                                dtLoTrinh.Columns.Add("Kinh do");
                                dtLoTrinh.Columns.Add("Vi do");
                               
                                string strDataPoint = "";
                       
                                List<lotrinhOBJ> lotrinh_napvaoduong = new List<lotrinhOBJ>();
                                List<string> lstDataPoint = new List<string>();
                                List<string> lstDataPoint_Offline = new List<string>();
                                List<Point> lpointDiChuyen = new List<Point>();
                                List<Point> lpointDiChuyen_Offline = new List<Point>();
                                List<Point> lpointVaoRaDiem = new List<Point>();
                                List<lotrinhOBJ> lotrinhdt = new List<lotrinhOBJ>();
                                List<lotrinhmattinhieuOBJ> lotrinhmattinhieu = new List<lotrinhmattinhieuOBJ>();
                                List<banglotrinhOBJ> banglotrinhdt = new List<banglotrinhOBJ>();
                                List<lotrinhtrinhvaodiemOBJ> lotrinhtrinhvaodiem = new List<lotrinhtrinhvaodiemOBJ>();
                                List<lotrinhtrinhvaodiemOBJ> lotrinhtrinhradiem = new List<lotrinhtrinhvaodiemOBJ>();
                                foreach (DataRow i in dt.Rows)
                                {

                                    if (x % parameters.thamsohienthi == 1 || parameters.thamsohienthi == 1 || parameters.khongnoidiem == 0)
                                    {
                                        Point.PointType t = Point.PointType.Move;

                                        if (i["ghichu"].ToString() == "Vào điểm")
                                        {
                                            t = Point.PointType.CheckIn;
                                        }
                                        else if (i["ghichu"].ToString() == "Ra điểm")
                                        {
                                            t = Point.PointType.CheckOut;
                                        }
                                        else if (i["ghichu"].ToString() == "Ngoại tuyến")
                                        {
                                            t = Point.PointType.Offline;
                                        }
                                        strDataPoint += i["vido"].ToString() + "," + i["kinhdo"].ToString();
                                        Point p = null;
                                        try
                                        {
                                            p = new Point { speed = i["speed"].ToString() != "" ? int.Parse(i["speed"].ToString()) : 0 ,  tinhtrangpin = i["tinhtrangpin"].ToString(), tennhanvien = i["nhanvien"].ToString(), idnhanvien = int.Parse(i["idnhanvien"].ToString()), Lat = double.Parse(i["vido"].ToString()), Lng = double.Parse(i["kinhdo"].ToString()), thoigianbantin = DateTime.Parse(i["thoigian"].ToString()), Time = DateTime.Parse(i["thoigian"].ToString()), Type = t, OrigIndex = j, accuracy = i["accuracy"].ToString() != "" ? double.Parse(i["accuracy"].ToString()) : 0, idkhachhang = int.Parse(i["idkhachhang"].ToString()), tenkhachhang = i["tenkhachhang"].ToString(), diachikhachhang = i["diachikhachhang"].ToString(), thoigiantaidiem = i["thoigiantaidiem"].ToString() != "" ? DateTime.Parse(i["thoigiantaidiem"].ToString()) : new DateTime(1900, 01, 01) };

                                        }
                                        catch (Exception ex)
                                        {

                                            log.Error(ex);
                                        }
                                        lpoint.Add(p);
                                        j++;
                                        dem++;
                                        if (p.Type == Point.PointType.Move || p.Type == Point.PointType.Offline)
                                        {
                                            lpointDiChuyen.Add(p);
                                        }
                                        else if (p.Type == Point.PointType.CheckIn || p.Type == Point.PointType.CheckOut)
                                        {
                                            string gc = "Trực tuyến";

                                            if (p.Type == Point.PointType.CheckIn)
                                            {
                                                gc = "Vào điểm";
                                                lotrinhtrinhvaodiem.Add(new lotrinhtrinhvaodiemOBJ { idnhanvien = int.Parse(i["idnhanvien"].ToString()), vido = double.Parse(i["vido"].ToString()), kinhdo = double.Parse(i["kinhdo"].ToString()), ghichu = gc, thoigian = DateTime.Parse(i["thoigian"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"), accuracy = i["accuracy"].ToString() != "" ? double.Parse(i["accuracy"].ToString()) : 0, idkhachhang = int.Parse(i["idkhachhang"].ToString()), tenkhachhang = (i["idkhachhang"].ToString() == "" || int.Parse(i["idkhachhang"].ToString()) == 0 ? "Vào điểm tự do" : i["tenkhachhang"].ToString()), diachikhachhang = ((i["idkhachhang"].ToString() == "" || int.Parse(i["idkhachhang"].ToString()) == 0) ? "Vào điểm tự do" : i["diachikhachhang"].ToString()), thoigiantaidiem = i["thoigiantaidiem"].ToString() != "" ? DateTime.Parse(i["thoigiantaidiem"].ToString()).ToString("HH:mm:ss") : new DateTime(1900, 01, 01).ToString("HH:mm:ss"), thoigianvaodiem = DateTime.Parse(i["thoigianvaodiem"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"), thoigianradiem = (i["thoigianradiem"].ToString() == "" || DateTime.Parse(i["thoigianradiem"].ToString()).Year == 1900) ? "chưa ra điểm" : DateTime.Parse(i["thoigianradiem"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") });

                                            }
                                            else if (p.Type == Point.PointType.CheckOut)
                                            {
                                                gc = "Ra điểm";
                                                lotrinhtrinhradiem.Add(new lotrinhtrinhvaodiemOBJ { idnhanvien = int.Parse(i["idnhanvien"].ToString()), vido = double.Parse(i["vido"].ToString()), kinhdo = double.Parse(i["kinhdo"].ToString()), ghichu = gc, thoigian = DateTime.Parse(i["thoigian"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"), accuracy = i["accuracy"].ToString() != "" ? double.Parse(i["accuracy"].ToString()) : 0, idkhachhang = int.Parse(i["idkhachhang"].ToString()), tenkhachhang = (i["idkhachhang"].ToString() == "" || int.Parse(i["idkhachhang"].ToString()) == 0 ? "Vào điểm tự do" : i["tenkhachhang"].ToString()), diachikhachhang = ((i["idkhachhang"].ToString() == "" || int.Parse(i["idkhachhang"].ToString()) == 0) ? "Vào điểm tự do" : i["diachikhachhang"].ToString()), thoigiantaidiem = i["thoigiantaidiem"].ToString() != "" ? DateTime.Parse(i["thoigiantaidiem"].ToString()).ToString("HH:mm:ss") : new DateTime(1900, 01, 01).ToString("HH:mm:ss"), thoigianvaodiem = DateTime.Parse(i["thoigianvaodiem"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"), thoigianradiem = (i["thoigianradiem"].ToString() == "" || DateTime.Parse(i["thoigianradiem"].ToString()).Year == 1900) ? "chưa ra điểm" : DateTime.Parse(i["thoigianradiem"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") });

                                            }

                                            lpointVaoRaDiem.Add(p);
                                        }

                                        try
                                        {
                                            if (dem == 100)
                                            {
                                                lstDataPoint.Add(strDataPoint);
                                                strDataPoint = "";
                                                dem = 0;
                                            }
                                            
                                        }
                                        catch (Exception ex)
                                        {
                                            LSPos_Data.Utilities.Log.Error(ex);
                                        }

                                        if (j != dt.Rows.Count && dem != 0)
                                        {
                                            strDataPoint += "|";
                                        }
                                    }
                                    x++;
                                }
                                if (strDataPoint != "")
                                    lstDataPoint.Add(strDataPoint);
                                
                                if (parameters.loaiLoTrinh > 0)
                                {
                                    lotrinh_napvaoduong = NapVaoDuong(lstDataPoint);
                                }

                              
                                List<Point> filteredPoints = null;
                                List<Point> filteredPoints_Offline = null;
                                if (parameters.loaiLoTrinh == -1)
                                {

                                    //khong loai diem trung
                                    filteredPoints = lpointDiChuyen;
                                    filteredPoints_Offline = lpointDiChuyen_Offline;

                                }
                                else
                                {
                                    // filteredPoints = filter.FilterKhoangCachTheoThoiGian(filteredPoints);
                                    //loc lo trinh
                                    LoTrinhGPSFilter filter = new LoTrinhGPSFilter(lpointDiChuyen);
                                    LoTrinhGPSFilter filter_Offline = new LoTrinhGPSFilter(lpointDiChuyen_Offline);
                                    filteredPoints = filter.FilterAnhTrungNangCap_v2(lpointDiChuyen, parameters.minkhoangcach, parameters.thoigianlaybantin, parameters.tocDoHopLeToiDa);
                                    filteredPoints_Offline = filter_Offline.FilterAnhTrungNangCap_v2(lpointDiChuyen_Offline, parameters.minkhoangcach , parameters.thoigianlaybantin, parameters.tocDoHopLeToiDa);

                                    
                                }

                                List<LichSuDiChuyenOBJ> ldc = new List<LichSuDiChuyenOBJ>();
                                foreach (Point p in filteredPoints)
                                {
                                    string gc = "Trực tuyến";

                                    if (p.Type == Point.PointType.CheckIn)
                                    {
                                        gc = "Vào điểm";
                                    }
                                    else if (p.Type == Point.PointType.CheckOut)
                                    {
                                        gc = "Ra điểm";
                                    }
                                    else if (p.Type == Point.PointType.Offline)
                                    {
                                        gc = "Ngoại tuyến";
                                    }
                                    else if (p.Type == Point.PointType.Stop)
                                    {
                                        gc = "Dừng đỗ";
                                    }
                                    ldc.Add(new LichSuDiChuyenOBJ { nhanvien = p.tennhanvien, tinhtrangpin = p.tinhtrangpin, idnhanvien = p.idnhanvien, kinhdo = p.Lng, vido = p.Lat, thoigian = p.thoigianbantin, ghichu = gc, accuracy = p.accuracy, speed = p.speed, thoigianketthuc = p.thoigianketthuc });
                                    
                                }
                                 
                                foreach (Point p in lpointVaoRaDiem)
                                {
                                    string gc = "Trực tuyến";

                                    if (p.Type == Point.PointType.CheckIn)
                                    {
                                        gc = "Vào điểm";
                                    }
                                    else if (p.Type == Point.PointType.CheckOut)
                                    {
                                        gc = "Ra điểm";
                                    }
                                    else if (p.Type == Point.PointType.Offline)
                                    {
                                        gc = "Ngoại tuyến";
                                    }
                                    ldc.Add(new LichSuDiChuyenOBJ { nhanvien = p.tennhanvien, tinhtrangpin = p.tinhtrangpin, idnhanvien = p.idnhanvien, kinhdo = p.Lng, vido = p.Lat, thoigian = p.thoigianbantin, ghichu = gc, accuracy = p.accuracy, speed = p.speed, thoigianketthuc = p.thoigianketthuc });


                                }


                                List<LichSuDiChuyenOBJ> newList = ldc.OrderBy(o => o.thoigian).ToList();
                                int k = 0;
                                foreach (LichSuDiChuyenOBJ item in newList)
                                {
                                    k++;
                                    if (!item.kinhdo.ToString().Trim().Equals("nan") && !item.vido.ToString().ToLower().Equals("nan"))
                                    {
                                        lotrinhOBJ lt = new lotrinhOBJ { tennhanvien = item.nhanvien, tinhtrangpin = item.tinhtrangpin, idnhanvien = item.idnhanvien, thoigian = item.thoigian.ToString("dd/MM/yyyy HH:mm:ss"), ghichu = item.ghichu, kinhdo = item.kinhdo, vido = item.vido, accuracy = item.accuracy, speed = item.speed, thoigianketthuc = item.thoigianketthuc.ToString("dd/MM/yyyy HH:mm:ss") };

                                        if (k == 1)
                                        {
                                            OBJ.LoTrinhDauTien = lt;
                                        }
                                        if (k == newList.Count)
                                        {
                                            OBJ.LoTrinhCuoiCung = lt;
                                        }
                                        lotrinhdt.Add(lt);
                                        DataRow dr = dtLoTrinh.NewRow();
                                        dr["STT"] = k;
                                        dr["Nhan vien"] = item.nhanvien;
                                        dr["Thoi gian"] = item.thoigian.ToString("dd/MM/yyyy HH:mm:ss");
                                        dr["Kinh do"] = item.kinhdo;
                                        dr["Vi do"] = item.vido;
                                        dtLoTrinh.Rows.Add(dr);


                                        banglotrinhdt.Add(new banglotrinhOBJ { tenhanvien = item.nhanvien, tinhtrangpin = item.tinhtrangpin, idnhanvien = item.idnhanvien, thoigian = item.thoigian.ToString("dd/MM/yyyy HH:mm:ss"), ghichu = item.ghichu, accuracy = item.accuracy, kinhdo = item.kinhdo, vido = item.vido, speed = item.speed, thoigianketthuc = item.thoigianketthuc.ToString("dd/MM/yyyy HH:mm:ss") });
                                    }
                                }
                                for (int i = 1; i < ldc.Count; i++)
                                {
                                    if (!ldc[i - 1].kinhdo.ToString().Trim().Equals("nan")
                                        && !ldc[i - 1].vido.ToString().ToLower().Equals("nan")
                                        && !ldc[i].kinhdo.ToString().ToLower().Equals("nan")
                                        && !ldc[i].vido.ToString().ToLower().Equals("nan")
                                        )
                                    {
                                        if (ldc[i].ghichu == "Ngoại tuyến")
                                        {
                                            try
                                            {
                                                lotrinhmattinhieu.Add(new lotrinhmattinhieuOBJ
                                                {
                                                    diemdau = new lotrinhOBJ { kinhdo = ldc[i - 1].kinhdo, vido = ldc[i - 1].vido },
                                                    diemcuoi = new lotrinhOBJ { kinhdo = ldc[i].kinhdo, vido = ldc[i].vido }
                                                });
                                            }
                                            catch (Exception)
                                            {


                                            }
                                        }
                                      
                                    }
                                }


                                OBJ.datalotrinh = lotrinhdt;
                                OBJ.datavaodiem = lotrinhtrinhvaodiem;
                                OBJ.dataradiem = lotrinhtrinhradiem;
                                OBJ.datalotrinh_suydien = lotrinh_napvaoduong;
                                OBJ.databanglotrinh = banglotrinhdt;
                                OBJ.datamattinhieu = lotrinhmattinhieu;
                                OBJ.dataKeHoachDiChuyen = lstKeHoach;
                                OBJ.status = true;
                                OBJ.msg = "thành công";
                            }
                        }
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, OBJ);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, new Geo());
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        List<lotrinhOBJ> NapVaoDuong(List<string> data)
        {
            List<lotrinhOBJ> lotrinhdt = new List<lotrinhOBJ>();
            string apiKey = "AIzaSyCtSL8GLck0Y_GyuvvjlFn1-tYyKQ_A6WY";
            if (ConfigurationManager.AppSettings["GOOGLEAPIKEY"] != null)
            {
                apiKey = ConfigurationManager.AppSettings["GOOGLEAPIKEY"];

            }
            try
            {
                foreach (string s in data)
                {
                    string strLinkServer = "https://roads.googleapis.com/v1/snapToRoads";
                    Hashtable htParam = new Hashtable();
                    htParam.Add("interpolate", "true");
                    htParam.Add("path", s);
                    //htParam.Add("path", "-35.27801,149.12958|-35.28032,149.12907|-35.28099,149.12929|-35.28144,149.12984|-35.28194,149.13003|-35.28282,149.12956|-35.28302,149.12881|-35.28473,149.12836");
                    htParam.Add("key", apiKey);
                    Utils ut = new Utils();
                    string sJsonKetQua = ut.CallHTTP(strLinkServer, htParam);
                    var results = JsonConvert.DeserializeObject<dynamic>(sJsonKetQua);
                    if (results.snappedPoints != null)
                    {
                        for (int i = 0; i < results.snappedPoints.Count; i++)
                        {
                            lotrinhdt.Add(new lotrinhOBJ { kinhdo = results.snappedPoints[i].location.longitude, vido = results.snappedPoints[i].location.latitude });
                        }
                    }
                    else
                    {
                        string[] lsttoado = s.Split(new string[] { "|" }, StringSplitOptions.None);
                        foreach (string str in lsttoado)
                        {
                            string[] sToaDO = str.Split(',');
                            lotrinhdt.Add(new lotrinhOBJ { kinhdo = double.Parse(sToaDO[1]), vido = double.Parse(sToaDO[0]) });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                
                //throw new Exception("Đã xảy ra lỗi trong việc lấy lộ trình.");
            }
            return lotrinhdt;

        }
        [HttpPost]
        [Route("GetChiTiet_NhanVien")]
        public HttpResponseMessage GetChiTiet_NhanVien([FromBody]   int idnhanvien)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();
                NhanVien nv = null;

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    if ((nv = NhanVien_dl.ChiTietNhanVienTheoIDNV(idnhanvien)) == null)
                    {
                        nv = new NhanVien();
                        nv.TenDayDu = "Không xác định";
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, nv);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, new Geo());
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        //[HttpPost]
        //[Route("GetChiTiet_DungDo")]
        //public HttpResponseMessage GetChiTiet_DungDo([FromBody]   int idnhanvien, int kinhdo, int vido, string thoigianbatdau, string thoigianketthuc)
        //{
        //    HttpResponseMessage response = new HttpResponseMessage();
        //    response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
         
        //    DateTime dtthoigianbatdau = DateTime.ParseExact(thoigianbatdau, new string[] { "dd/MM/yyyy HH:mm:ss" }, null, System.Globalization.DateTimeStyles.None);
        //    DateTime dtthoigianketthuc = DateTime.ParseExact(thoigianketthuc, new string[] { "dd/MM/yyyy HH:mm:ss" }, null, System.Globalization.DateTimeStyles.None);
        //    string KHGanNhat = "Không có";


        //    try
        //    {
        //        UserInfo userinfo = utilsCommon.checkAuthorization();
        //        NhanVien nv = null;

        //        if (userinfo == null)
        //        {
        //            response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
        //        }
        //        else
        //        {
        //            if (kinhdo > 0)
        //            {
        //                SqlParameter[] par = new SqlParameter[]{
        //                    new SqlParameter("@kinhdo", kinhdo),
        //                    new SqlParameter("@vido", vido),
        //                    new SqlParameter("@idnhanvien",  idnhanvien)
        //                };
        //                SqlDataHelper helper = new SqlDataHelper();
        //                DataSet dsKH = helper.ExecuteDataSet("sp_App_VaoDiemCuaHangGanNhat_KhongTheoKeHoach_GomKhachHangCuaNhanVienTrongNhom", par);
        //                if (dsKH.Tables.Count > 0)
        //                {
        //                    DataTable dtKH = dsKH.Tables[0];
        //                    if (dtKH.Rows.Count > 0)
        //                    {
        //                        KHGanNhat = dtKH.Rows[0]["TenKhachHang"].ToString() + "(" + dtKH.Rows[0]["DiaChi"].ToString() + ")";
        //                    }
        //                }
        //            }

        //            diachi.InnerText = LayViTriTheoToaDo.GetDiaDiemTheoToaDo(vido, kinhdo);

        //            TimeSpan ts = thoigianketthuc - thoigianbatdau;


        //            kh.InnerHtml = "<li>Thời gian bắt đầu dừng: " + Request.Params["thoigianbatdau"] + "</li>"
        //                + "<li>Thời gian kết thúc dừng: " + thoigianketthuc.ToString("dd/MM/yyyy HH:mm:ss") + " </li>"
        //                + "<li>Thời gian dừng: " + ts.ToString(@"hh\:mm\:ss") + "</li> "
        //                + "<li>Khách hàng gần nhất: " + KHGanNhat + "</li> ";
                     

        //            response = Request.CreateResponse(HttpStatusCode.OK, nv);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response = Request.CreateResponse(HttpStatusCode.NotFound, new Geo());
        //        LSPos_Data.Utilities.Log.Error(ex);
        //    }

        //    return response;
        //}
    }

    public class DanhSachTatCaDaiLyOBJ
    {
        public DanhSachTatCaDaiLyOBJ() { }

        public bool status { get; set; }

        public string msg { get; set; }

        public List<ToaDoOBJ> data { get; set; }
    }

    public class ViTriTatCaNhanVienOBJ
    {
        public ViTriTatCaNhanVienOBJ() { }

        public bool status { get; set; }

        public string msg { get; set; }

        public List<gpsnhanvien> data { get; set; }
    }
    public class OBJdataLotrinh
    {
        public OBJdataLotrinh() { }

        public List<banglotrinhOBJ> databanglotrinh { get; set; }
        public List<KeHoachDiChuyenObj> dataKeHoachDiChuyen { get; set; }
        public List<lotrinhOBJ> datalotrinh { get; set; }
        public List<lotrinhOBJ> datalotrinh_suydien { get; set; }
        public List<lotrinhOBJ> datalotrinh_suydien_offline { get; set; }
        public List<lotrinhmattinhieuOBJ> datamattinhieu { get; set; }
        public List<lotrinhtrinhvaodiemOBJ> dataradiem { get; set; }
        public List<lotrinhtrinhvaodiemOBJ> datavaodiem { get; set; }
        public lotrinhOBJ LoTrinhCuoiCung { get; set; }
        public lotrinhOBJ LoTrinhDauTien { get; set; }
        public string msg { get; set; }
        public bool status { get; set; }
    }
}
