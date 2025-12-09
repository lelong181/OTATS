using LSPos_Data.Data;
using LSPosMVC.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LSPosMVC.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Authenticate([FromBody] LoginRequest login)
        {
            LoginObj obj = new LoginObj();
            obj.status = false;
            obj.msg = Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG;
            obj.data = null;

            //var loginResponse = new LoginResponse { };
            LoginRequest loginrequest = new LoginRequest { };
            loginrequest.Username = login.Username;
            loginrequest.PassWord = login.PassWord;
            loginrequest.MaCongty = login.MaCongty;

            IHttpActionResult response;
            HttpResponseMessage responseMsg = new HttpResponseMessage();

            bool isUsernamePasswordValid = false;

            if (login != null)
            {
                User_dl userDL = new User_dl();
                isUsernamePasswordValid = userDL.UserLogIn(loginrequest.Username, loginrequest.PassWord, loginrequest.MaCongty);
            }

            // if credentials are valid
            if (isUsernamePasswordValid)
            {
                string token = createToken(loginrequest.Username, loginrequest.MaCongty);

                obj.status = true;
                obj.msg = Config.THANH_CONG;
                obj.data = token;

                HttpResponseMessage responseMessage = Request.CreateResponse(HttpStatusCode.OK, obj);
                response = ResponseMessage(responseMessage);

                //return the token
                //return Ok<string>(token);
                return response;
            }
            else
            {
                obj.msg = "Tài khoản hoặc mật khẩu không đúng hoặc thời hạn hợp đồng đã kết thúc.";

                // if credentials are not valid send unauthorized status code in response
                HttpResponseMessage responseMessage = Request.CreateResponse(HttpStatusCode.Unauthorized, obj);
                response = ResponseMessage(responseMessage);
                return response;
            }
        }

        private string createToken(string userName, string maCongTy)
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
                new Claim("MaCongty", maCongTy)
            });

            const string sec = "!le@son#lsstechvn";
            //var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(
                        issuer: "lstechvn", 
                        audience: "lstechvn",
                        subject: claimsIdentity,
                        notBefore: issuedAt,
                        expires: expires, 
                        signingCredentials: signingCredentials
                        );
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }

    public class LoginObj
    {
        public LoginObj(){
        }
        public bool status { get; set; }
        public string msg { get; set; }
        public string data { get; set; }
    }
}
