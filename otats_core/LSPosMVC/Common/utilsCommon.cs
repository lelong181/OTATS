
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using LSPos_Data.Data;
using LSPos_Data.Models;

namespace LSPosMVC.Common
{
    public class utilsCommon
    {
        public static UserInfo checkAuthorization()
        {
            try
            {
                //get token string from Headers Request
                string authHeader = HttpContext.Current.Request.Headers["Authorization"];
                //decode token string
                var token = new JwtSecurityToken(jwtEncodedString: authHeader);
                string username = token.Claims.First(c => c.Type == "Username").Value;
                string maCongTy = token.Claims.First(c => c.Type == "MaCongty").Value;
                string isNhanVien = token.Claims.First(c => c.Type == "IsNhanVien").Value;

                User_dl userDL = new User_dl();
                UserInfo userinfo = userDL.GetUserInfo(username, maCongTy);

                if (isNhanVien == "1")
                {
                    UserinforData UserinforData = new UserinforData();
                    userinfo = UserinforData.GetUserInfo(username, maCongTy);
                    userinfo.IsAdmin = false;
                }
                else
                {
                    userinfo.IsAdmin = true;
                }

                return userinfo;
            }
            catch (Exception e)
            {
                LSPos_Data.Utilities.Log.Error(e);
                return null;
            }

        }

        public static dynamic validateRow(dynamic parameter, DataRow row, string col, string propType)
        {
            Type type = Type.GetType(propType);


            if (row[col] != null && row[col] != System.DBNull.Value)
            {
                string value = row[col].ToString();
                parameter = Convert.ChangeType(value, type);

            }
            else
            {

            }
            return parameter;
        }

        public static string getPatternByIndexFormat(string format, string indexOfStr)
        {
            string result = "";
            int index = format.IndexOf(indexOfStr);
            int lastIndex = format.LastIndexOf(indexOfStr);
            result = index > -1 ? format.Substring(index, (lastIndex - index + 1)) : "";
            return result;
        }

        public static string genMaDonHang(NhanVienAppModels nhanVienInfor, string macuoicung, string idnhom,
            string MaNhom, int sttDonHang)
        {
            double stt = 0;
            string formatMaDonHang = "yymmddn-00000";


            if (nhanVienInfor.MaDonHang_CauTruc != null)
            {
                formatMaDonHang = nhanVienInfor.MaDonHang_CauTruc;
            }
            string formatGen = "";
            if (nhanVienInfor.MaDonHang_TuSinh == 1)
            {
                if (nhanVienInfor.MaDonHang_TuSinh_TheoKy == 1)
                {
                    string patternSo = getPatternByIndexFormat(formatMaDonHang, "0");
                    string patternNgay = getPatternByIndexFormat(formatMaDonHang, "d");
                    string patternNam = getPatternByIndexFormat(formatMaDonHang, "y");
                    string patternThang = getPatternByIndexFormat(formatMaDonHang, "m");
                    string patternIDNhom = getPatternByIndexFormat(formatMaDonHang, "n");
                    string patternMaNhom = getPatternByIndexFormat(formatMaDonHang, "g");
                    string value = (sttDonHang + 1).ToString("D" + patternSo.Length);

                    //update STT_DonHang cua Nhom

                    formatGen = formatMaDonHang.Replace(patternSo, value);
                    formatGen = patternNam != "" ? formatGen.Replace(patternNam, DateTime.Now.ToString("yy")) : formatGen;
                    formatGen = patternThang != "" ? formatGen.Replace(patternThang, DateTime.Now.ToString("MM")) : formatGen;
                    formatGen = patternIDNhom != "" ? formatGen.Replace(patternIDNhom, idnhom) : formatGen;
                    formatGen = patternMaNhom != "" ? formatGen.Replace(patternMaNhom, MaNhom != "" ? MaNhom : idnhom) : formatGen;
                    formatGen = patternNgay != "" ? formatGen.Replace(patternNgay, DateTime.Now.ToString("dd")) : formatGen;

                }
                else
                {
                    if (macuoicung != "")
                    {
                        string str_stt = Regex.Match(macuoicung.Substring(macuoicung.IndexOf('-') + 1, macuoicung.Length - macuoicung.IndexOf('-') - 1), @"-?\d+").Value;
                        stt = double.Parse(str_stt);
                    }

                    formatMaDonHang = "yy-00000";
                    string patternSo = getPatternByIndexFormat(formatMaDonHang, "0");
                    string patternNam = getPatternByIndexFormat(formatMaDonHang, "y");
                    string value = "";
                    try
                    {
                        value = (stt + 1).ToString("D" + patternSo.Length);
                    }
                    catch (Exception ev)
                    {
                        //LSPos_Data.Utilities.Log.Error(ev);
                        value = (stt + 1).ToString();

                    }

                    formatGen = DateTime.Now.ToString("yyMMdd") + "-" + value;
                }
            }
            return formatGen;
        }
    }

}
