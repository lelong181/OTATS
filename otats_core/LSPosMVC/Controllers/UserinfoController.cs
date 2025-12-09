using LSPosMVC.Common;
using LSPos_Data.Data;
using LSPos_Data.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using LSPosMVC.Models;
using System.Web.Http.Cors;
using System.Web;

namespace LSPosMVC.Controllers
{
    [EnableCors(origins: "*", "*", "*")]
    [RoutePrefix("api/userinfo")]
    public class UserinfoController : ApiController
    {
        [HttpPost]
        [Route("login")]
        public HttpResponseMessage Login([FromBody] LoginRequest login)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                LoginObj obj = new LoginObj();

                CauHinhCongTyData cauHinhCongTyData = new CauHinhCongTyData();
                User_dl userDL = new User_dl();
                UserinforData UserinforData = new UserinforData();
                UserInfo userinfo = userDL.GetUserInfo(login.Username, login.MaCongty);

                if (login.IsNhanVien == 1)
                {
                    userinfo = UserinforData.GetUserInfo(login.Username, login.MaCongty);
                }

                obj.userinfo = userinfo;
                obj.congty = CongTyDB.ThongTinCongTyByID(userinfo.ID_QLLH);

                obj.cauhinh = cauHinhCongTyData.get(userinfo.ID_QLLH);

                obj.phanmem = new ThongTinPhanMem();
                obj.phanmem.version = System.Web.Configuration.WebConfigurationManager.AppSettings["Version"];
                obj.phanmem.ngaycapnhat = System.Web.Configuration.WebConfigurationManager.AppSettings["NgayCapNhat"];
                obj.phanmem.phienbannen = System.Web.Configuration.WebConfigurationManager.AppSettings["PhienBanNen"];
                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    string token = createToken(login.Username, login.MaCongty, login.IsNhanVien.ToString());
                    obj.token = token;
                    NhanVienApp nv_app = new NhanVienApp();
                    bool result = nv_app.UpdateIDPush((userinfo.Level == 1 ? userinfo.ID_QuanLy : 0),
                        (userinfo.Level == 1 ? 0 : userinfo.ID_QuanLy),
                        login.IDPUSH);
                    response = Request.CreateResponse(HttpStatusCode.OK, obj);
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
        [Route("authenticate")]
        public HttpResponseMessage DoAuthenticate([FromBody] LoginRequest login)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                LoginObj obj = new LoginObj();

                User_dl userDL = new User_dl();
                UserinforData UserinforData = new UserinforData();

                NhanVienApp nv = new NhanVienApp();
                int ID_QLLH = 0;
                int ID_NHANVIEN = 0;
                int ID_QuanLy = 0;
                if (login.IsNhanVien == 1)
                {

                    int qkq = nv.DangNhap(login.MaCongty, login.Username, login.PassWord, out ID_QLLH, out ID_NHANVIEN);

                    if (qkq == 1)
                    {
                        obj.userinfo = UserinforData.GetUserInfo(login.Username, login.MaCongty);
                        obj.redirect_url = "angular/#!/danhSachDonHang";
                        string token = createToken_v2(login.Username, login.MaCongty, login.IsNhanVien.ToString(), ID_NHANVIEN.ToString(), "0", ID_QLLH.ToString());
                        obj.token = token;
                        response = Request.CreateResponse(HttpStatusCode.OK, obj);
                    }
                    else if (qkq == 2)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { status = true, msg = "label_macongtykhongtontaivuilongkiemtralai" });
                    }
                    else if (qkq == 3)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { status = true, msg = "label_thoihanhopdongdahetvuilongkiemtralai" });
                    }
                    else if (qkq == 4)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { status = true, msg = "label_sonhanvienkhaibaovuotquahanmucvuilongkiemtralai" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                    }
                }
                else
                {
                    int qkq = nv.DangNhap_QuanLy(login.MaCongty, login.Username, login.PassWord, out ID_QLLH, out ID_QuanLy);

                    if (qkq == 1)
                    {

                        obj.redirect_url = "angular/#!/default";
                        obj.userinfo = userDL.GetUserInfo(login.Username, login.MaCongty);



                        string token = createToken_v2(login.Username, login.MaCongty, login.IsNhanVien.ToString(), "0", ID_QuanLy.ToString(), ID_QLLH.ToString());
                        obj.token = token;
                        response = Request.CreateResponse(HttpStatusCode.OK, obj);


                    }
                    else if (qkq == 2)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { status = true, msg = "label_macongtykhongtontaivuilongkiemtralai" });
                    }
                    else if (qkq == 3)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { status = true, msg = "label_thoihanhopdongdahetvuilongkiemtralai" });
                    }
                    else if (qkq == 4)
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { status = true, msg = "label_sonhanvienkhaibaovuotquahanmucvuilongkiemtralai" });
                    }
                    else
                    {

                        //sai mat khau
                        response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
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
        [Route("checklogin")]
        [EnableCors(origins: "*", "*", "*")]
        public HttpResponseMessage checklogin([FromBody] LoginRequest login)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
            try
            {
                User_dl userDL = new User_dl();
                UserinforData user = new UserinforData();
                if (login.IsNhanVien == 1)
                {
                    bool flag = true;
                    //try
                    //{
                    //    string URL_LOGIN = Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]);
                    //    URL_LOGIN += "/AppLogin_v2.aspx";
                    //    URL_LOGIN += "?token=abc";
                    //    URL_LOGIN += "&taikhoan=" + login.Username;
                    //    URL_LOGIN += "&matkhau=" + md5(login.PassWord);
                    //    URL_LOGIN += "&os=3";
                    //    URL_LOGIN += "&kinhdo=0";
                    //    URL_LOGIN += "&vido=0";
                    //    URL_LOGIN += "&idct=" + login.MaCongty;//xu ly encode URL  trên app
                    //    URL_LOGIN += "&ver=" + System.Web.Configuration.WebConfigurationManager.AppSettings["Version"];

                    //    WebClient client = new WebClient();
                    //    client.Encoding = System.Text.Encoding.UTF8;
                    //    string contentLogin = client.DownloadString(URL_LOGIN);

                    //    LoginMdl loginMdl = new LoginMdl();
                    //    loginMdl = JsonConvert.DeserializeObject<LoginMdl>(contentLogin);
                    //    flag = loginMdl.status;

                    //} 
                    //catch (Exception ex)
                    //{
                    //    LSPos_Data.Utilities.Log.Error(ex);
                    //}

                    if (flag && user.UserLogInNhanVien(login.Username, login.PassWord, login.MaCongty))
                        response = Request.CreateResponse(HttpStatusCode.OK, true);
                    else
                        response = Request.CreateResponse(HttpStatusCode.NotModified, false);
                }
                else
                {
                    if (userDL.UserLogIn(login.Username, login.PassWord, login.MaCongty))
                        response = Request.CreateResponse(HttpStatusCode.OK, true);
                    else
                        response = Request.CreateResponse(HttpStatusCode.NotModified, false);
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
        [Route("getroles")]
        public HttpResponseMessage GetRoles([FromUri] string MaChucNang)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                //get token string from Headers Request
                string authHeader = System.Web.HttpContext.Current.Request.Headers["Authorization"];

                //decode token string
                var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                string username = token.Claims.First(c => c.Type == "Username").Value;
                string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;

                User_dl userDL = new User_dl();
                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);

                if (userinfo == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                else
                {
                    ChucNang_dl cndl = new ChucNang_dl();
                    ChucNangOBJ item = cndl.CheckQuyen(MaChucNang, userinfo.ID_QuanLy);
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

        [HttpGet]
        [Route("getquyenbyurl")]
        public HttpResponseMessage getquyenbyurl([FromUri] string url)
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
                    UserinforData user = new UserinforData();
                    ChucNangOBJ item = user.CheckQuyen(url, userinfo.ID_QuanLy);

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
        [Route("changepass")]
        public HttpResponseMessage ChangePass([FromBody] ChangePassRequest login)
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
                    UserinforData user = new UserinforData();
                    string authHeader = HttpContext.Current.Request.Headers["Authorization"];
                    //decode token string
                    var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                    string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;
                    bool flag = false;
                    if (userinfo.Level == 1)
                        flag = user.UserLogIn(userinfo.Username, login.Password, userinfo.ID_QLLH);
                    else if (userinfo.Level == 2)
                        flag = user.UserLogInNhanVien(login.Username, login.Password, maCongTy);

                    if (!flag)
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_matkhauhientaikhongchinhxac" });
                    else
                    {
                        bool r = false;
                        if (userinfo.Level == 1)
                            r = user.ChangeUserPassword(userinfo.Username, login.NewPass, userinfo.ID_QLLH);
                        else if (userinfo.Level == 2)
                            r = user.ResetPassword(userinfo.ID_QLLH, userinfo.ID_QuanLy, login.NewPass);

                        if (r)
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_capnhatmatkhauthanhcong" });
                        else
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_capnhatmatkhaukhongthanhcongvuilongthulai" });
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
        [Route("resetPassword")]
        public HttpResponseMessage resetPassword([FromBody] ResetPassModelFilter obj)
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

                    UserinforData user = new UserinforData();
                    if (user.ResetPassword(userinfo.ID_QLLH, obj.id, obj.newpass))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_capnhatmatkhauthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, new { success = false, msg = "label_capnhatmatkhaukhongthanhcongvuilongthulai" });
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
        [Route("changelang")]
        public HttpResponseMessage changelang([FromUri] string lang)
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

                    UserinforData user = new UserinforData();
                    if (user.ChangeLang(userinfo.ID_QuanLy, lang))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.Accepted, new { success = false });
                    }
                            ;
                }

            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        private string createToken(string userName, string maCongTy, string isNhanVien)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddDays(7);

            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "lstech"),
                new Claim("Username", userName),
                new Claim("MaCongty", maCongTy),
                new Claim("IsNhanVien", isNhanVien)
            });

            const string sec = "!le@son#lsstechvn";
            //var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: "lstechvn", audience: "lstechvn",
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }


        private string createToken_v2(string userName, string maCongTy, string isNhanVien, string ID_NhanVien, string ID_QuanLy, string ID_QLLH)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddYears(1);

            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "lscloud"),
                new Claim("Username", userName),
                new Claim("MaCongty", maCongTy),
                new Claim("IsNhanVien", isNhanVien),
                 new Claim("ID_NhanVien", ID_NhanVien),
                  new Claim("ID_QuanLy", ID_QuanLy),
                   new Claim("ID_QLLH", ID_QLLH)
            });

            const string sec = "!le@son#lsstechvn";
            //var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: "lstechvn", audience: "lstechvn",
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public class ChangePassRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string NewPass { get; set; }
        }
        public class LoginObj
        {
            public string token { get; set; }
            public UserInfo userinfo { get; set; }
            public CongTyOBJ congty { get; set; }
            public ThongTinPhanMem phanmem { get; set; }
            public CauHinhCongTyModel cauhinh { set; get; }
            public string redirect_url { get; set; }

        }
        public class ThongTinPhanMem
        {
            public string version { get; set; }
            public string ngaycapnhat { get; set; }
            public string phienbannen { get; set; }
        }
        public static byte[] encryptData(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }
        public static string md5(string data)
        {
            return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();
        }

        public class LoginMdl
        {
            public bool status { get; set; }
            public string msg { get; set; }
            public string ChiTietHangHoa_HienThi { get; set; }
        }
    }
}
