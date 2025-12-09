using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for BaoCao_dl
/// </summary>
public class Utils
{
    log4net.ILog log = log4net.LogManager.GetLogger(typeof(Utils));
    public Utils()
	{
		//
		// TODO: Add constructor logic here
        //
        
	}
    public  string CallHTTP(string _Url, Hashtable _Param)
    {

        string result = "";
        try
        {
            string param = "";
            int idx = 0;
            foreach (DictionaryEntry de in _Param)
            {
                if (idx == 0)
                {
                    param += "?" + de.Key + "=" + de.Value;
                }
                else
                {
                    param += "&" + de.Key + "=" + de.Value;
                }
                idx++;
            }

            string strURL = _Url + param;
            

            HttpWebRequest loHttp = (HttpWebRequest)HttpWebRequest.Create(strURL);
            loHttp.Accept = "text/html";
            loHttp.Method = "GET"; //this is the default behavior
            HttpWebResponse loWebResponse = (HttpWebResponse)loHttp.GetResponse();
            //Encoding enc = Encoding.GetEncoding(1252);  // Windows default Code Page
            Encoding enc = Encoding.UTF8;  // Windows default Code Page
            StreamReader loResponseStream =
               new StreamReader(loWebResponse.GetResponseStream(), enc);
            string lcHtml = loResponseStream.ReadToEnd();

            result = lcHtml;
             
        }
        catch (Exception exc)
        {
            log.Error(exc);
        }
        return result;
    }

    public static string GiaiMa(  string stringToDecrypt)
    {
        string keyinput = "lskey888";
        string sEncryptionKey = keyinput;
        byte[] key = { };
        byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };
        byte[] inputByteArray = new byte[stringToDecrypt.Length];
        try
        {
            key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(stringToDecrypt);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            Encoding encoding = Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }
        catch (Exception ex)
        {
            return (string.Empty);
        }
    }
    public static string MaHoa(string stringToEncrypt)
    {
        string keyinput = "lskey888";
        string sEncryptionKey = keyinput;
        byte[] key = { };
        byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };
        byte[] inputByteArray; //Convert.ToByte(stringToEncrypt.Length) 

        try
        {
            key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }
        catch (System.Exception ex)
        {
            return (string.Empty);
        }
    }
    public static string md5(string data)
    {
        return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();
        //return data;
    }
    public static byte[] encryptData(string data)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashedBytes;
        System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
        hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
        return hashedBytes;
    }
    public static IEnumerable<DateTime> GetAllDatesAndInitializeTickets(DateTime startingDate, DateTime endingDate)
    {
        List<DateTime> allDates = new List<DateTime>();


        for (DateTime i = startingDate; i <= endingDate; i = i.AddDays(1))
        {
            allDates.Add(i);
        }
        return allDates.AsReadOnly();
    }
    public static bool GhiLog(int ID_QLLH, int ID_QuanLy, string TenChucNangThayDoi, string TruongThongTinThayDoi, string GiaTriThayDoi, string GiaTriBanDau)
    {
        bool ghiThanhCong = false;

        try
        {
            string svURL = Utils.GiaiMa(System.Web.Configuration.WebConfigurationManager.AppSettings["SERVERIMAGE"]);
            string strLinkServer = svURL + "/AppGhiLichSu.aspx";

            string IP = HttpContext.Current.Request.UserHostAddress;
            int Device = 2;//web
            Hashtable htParam = new Hashtable();
            htParam.Add("IP", IP);
            htParam.Add("Device", Device);
            htParam.Add("ID_QLLH", ID_QLLH);
            htParam.Add("ID_QuanLy", ID_QuanLy);
            htParam.Add("TenChucNangThayDoi", TenChucNangThayDoi);
            htParam.Add("TruongThongTinThayDoi", TruongThongTinThayDoi);
            htParam.Add("GiaTriThayDoi", GiaTriThayDoi);
            htParam.Add("GiaTriBanDau", GiaTriBanDau);
            htParam.Add("token", "6e22b116f5111220741848ccd290e9e9062522d88a1fb00ba9b168db7a480271");
            Utils ut = new Utils();
            string sJsonKetQua = ut.CallHTTP(strLinkServer, htParam);

            var results = JsonConvert.DeserializeObject<dynamic>(sJsonKetQua);
            string mess = results.msg.ToString();
            if (results.status.ToString() == "True" && mess == "Xử lý thành công.")
            {
                ghiThanhCong = true;
            }
        }
        catch (Exception ex)
        {
             
        }
        return ghiThanhCong;

    }
    
    public static string UploadFilesToRemoteUrl(string url, IList<byte[]> files, NameValueCollection nvc)
    {

        string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

        var request = (HttpWebRequest)WebRequest.Create(url);
        request.ContentType = "multipart/form-data; boundary=" + boundary;
        request.Method = "POST";
        request.KeepAlive = true;
        var postQueue = new ByteArrayCustomQueue();

        var formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";

        foreach (string key in nvc.Keys)
        {
            var formitem = string.Format(formdataTemplate, key, nvc[key]);
            var formitembytes = Encoding.UTF8.GetBytes(formitem);
            postQueue.Write(formitembytes);
        }

        var headerTemplate = "\r\n--" + boundary + "\r\n" +
            "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
            "Content-Type: application/zip\r\n\r\n";

        var i = 0;
        foreach (var file in files)
        {
            var header = string.Format(headerTemplate, "file" + i, "file" + i + ".zip");
            var headerbytes = Encoding.UTF8.GetBytes(header);
            postQueue.Write(headerbytes);
            postQueue.Write(file);
            i++;
        }

        postQueue.Write(Encoding.UTF8.GetBytes("\r\n--" + boundary + "--"));

        request.ContentLength = postQueue.Length;

        using (var requestStream = request.GetRequestStream())
        {
            postQueue.CopyToStream(requestStream);
            requestStream.Close();
        }

        var webResponse2 = request.GetResponse();

        using (var stream2 = webResponse2.GetResponseStream())
        using (var reader2 = new StreamReader(stream2))
        {

            var res = reader2.ReadToEnd();
            webResponse2.Close();
            return res;
        }
    }
    public static string ChuyenSo(string number)
    {
        string[] strTachPhanSauDauPhay;
        if (number.Contains('.') || number.Contains(','))
        {
            strTachPhanSauDauPhay = number.Split(',', '.');
            return (ChuyenSo(strTachPhanSauDauPhay[0]) + "phẩy " + ChuyenSo(strTachPhanSauDauPhay[1]));
        }

        string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
        string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
        string doc;
        int i, j, k, n, len, found, ddv, rd;

        len = number.Length;
        number += "ss";
        doc = "";
        found = 0;
        ddv = 0;
        rd = 0;

        i = 0;
        while (i < len)
        {
            //So chu so o hang dang duyet
            n = (len - i + 2) % 3 + 1;

            //Kiem tra so 0
            found = 0;
            for (j = 0; j < n; j++)
            {
                if (number[i + j] != '0')
                {
                    found = 1;
                    break;
                }
            }

            //Duyet n chu so
            if (found == 1)
            {
                rd = 1;
                for (j = 0; j < n; j++)
                {
                    ddv = 1;
                    switch (number[i + j])
                    {
                        case '0':
                            if (n - j == 3) doc += cs[0] + " ";
                            if (n - j == 2)
                            {
                                if (number[i + j + 1] != '0') doc += "linh ";
                                ddv = 0;
                            }
                            break;
                        case '1':
                            if (n - j == 3) doc += cs[1] + " ";
                            if (n - j == 2)
                            {
                                doc += "mười ";
                                ddv = 0;
                            }
                            if (n - j == 1)
                            {
                                if (i + j == 0) k = 0;
                                else k = i + j - 1;

                                if (number[k] != '1' && number[k] != '0')
                                    doc += "mốt ";
                                else
                                    doc += cs[1] + " ";
                            }
                            break;
                        case '5':
                            if ((i + j == len - 1) || (i + j + 3 == len - 1))
                                doc += "lăm ";
                            else
                                doc += cs[5] + " ";
                            break;
                        default:
                            doc += cs[(int)number[i + j] - 48] + " ";
                            break;
                    }

                    //Doc don vi nho
                    if (ddv == 1)
                    {
                        doc += ((n - j) != 1) ? dv[n - j - 1] + " " : dv[n - j - 1];
                    }
                }
            }


            //Doc don vi lon
            if (len - i - n > 0)
            {
                if ((len - i - n) % 9 == 0)
                {
                    if (rd == 1)
                        for (k = 0; k < (len - i - n) / 9; k++)
                            doc += "tỉ ";
                    rd = 0;
                }
                else
                    if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
            }

            i += n;
        }

        if (len == 1)
            if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];

        return doc ;
    }
    public static string ChuyenTienDinhDangChu(string number)
    {
        string s = "";
        s = ChuyenSo(number);
        string lastRst = s.Substring(0, 1).ToUpper() + s.Substring(1);
        return lastRst.Trim()+ " đồng";
         

    }
    //public static string ChuyenTienDinhDangChu(string number)
    //{
    //    string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
    //    string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
    //    string doc;
    //    int i, j, k, n, len, found, ddv, rd;

    //    len = number.Length;
    //    number += "ss";
    //    doc = "";
    //    found = 0;
    //    ddv = 0;
    //    rd = 0;

    //    i = 0;
    //    while (i < len)
    //    {
    //        //So chu so o hang dang duyet
    //        n = (len - i + 2) % 3 + 1;

    //        //Kiem tra so 0
    //        found = 0;
    //        for (j = 0; j < n; j++)
    //        {
    //            if (number[i + j] != '0')
    //            {
    //                found = 1;
    //                break;
    //            }
    //        }

    //        //Duyet n chu so
    //        if (found == 1)
    //        {
    //            rd = 1;
    //            for (j = 0; j < n; j++)
    //            {
    //                ddv = 1;
    //                switch (number[i + j])
    //                {
    //                    case '0':
    //                        if (n - j == 3) doc += cs[0] + " ";
    //                        if (n - j == 2)
    //                        {
    //                            if (number[i + j + 1] != '0') doc += "lẻ ";
    //                            ddv = 0;
    //                        }
    //                        break;
    //                    case '1':
    //                        if (n - j == 3) doc += cs[1] + " ";
    //                        if (n - j == 2)
    //                        {
    //                            doc += "mười ";
    //                            ddv = 0;
    //                        }
    //                        if (n - j == 1)
    //                        {
    //                            if (i + j == 0) k = 0;
    //                            else k = i + j - 1;

    //                            if (number[k] != '1' && number[k] != '0')
    //                                doc += "mốt ";
    //                            else
    //                                doc += cs[1] + " ";
    //                        }
    //                        break;
    //                    case '5':
    //                        if (i + j == len - 1)
    //                            doc += "lăm ";
    //                        else
    //                            doc += cs[5] + " ";
    //                        break;
    //                    default:
    //                        doc += cs[(int)number[i + j] - 48] + " ";
    //                        break;
    //                }

    //                //Doc don vi nho
    //                if (ddv == 1)
    //                {
    //                    doc += dv[n - j - 1] + " ";
    //                }
    //            }
    //        }


    //        //Doc don vi lon
    //        if (len - i - n > 0)
    //        {
    //            if ((len - i - n) % 9 == 0)
    //            {
    //                if (rd == 1)
    //                    for (k = 0; k < (len - i - n) / 9; k++)
    //                        doc += "tỉ ";
    //                rd = 0;
    //            }
    //            else
    //                if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
    //        }

    //        i += n;
    //    }

    //    if (len == 1)
    //        if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];

    //    return doc;
    //}

    //public static string ChuyenTienDinhDangChu(decimal number)
    //{
    //    string s = number.ToString("#.##");
    //    string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
    //    string[] hang = new string[] { "", "nghìn", "triệu", "tỷ" };
    //    int i, j, donvi, chuc, tram;
    //    string str = " ";
    //    bool booAm = false;
    //    decimal decS = 0;
    //    //Tung addnew
    //    try
    //    {
    //        decS = Convert.ToDecimal(s.ToString());
    //    }
    //    catch
    //    {
    //    }
    //    if (decS < 0)
    //    {
    //        decS = -decS;
    //        s = decS.ToString();
    //        booAm = true;
    //    }
    //    i = s.Length;
    //    if (i == 0)
    //        str = so[0] + str;
    //    else
    //    {
    //        j = 0;
    //        while (i > 0)
    //        {
    //            donvi = Convert.ToInt32(s.Substring(i - 1, 1));
    //            i--;
    //            if (i > 0)
    //                chuc = Convert.ToInt32(s.Substring(i - 1, 1));
    //            else
    //                chuc = -1;
    //            i--;
    //            if (i > 0)
    //                tram = Convert.ToInt32(s.Substring(i - 1, 1));
    //            else
    //                tram = -1;
    //            i--;
    //            if ((donvi > 0) || (chuc > 0) || (tram > 0) || (j == 3))
    //                str = hang[j] + str;
    //            j++;
    //            if (j > 3) j = 1;
    //            if ((donvi == 1) && (chuc > 1))
    //                str = "một " + str;
    //            else
    //            {
    //                if ((donvi == 5) && (chuc > 0))
    //                    str = "lăm " + str;
    //                else if (donvi > 0)
    //                    str = so[donvi] + " " + str;
    //            }
    //            if (chuc < 0)
    //                break;
    //            else
    //            {
    //                if ((chuc == 0) && (donvi > 0)) str = "lẻ " + str;
    //                if (chuc == 1) str = "mười " + str;
    //                if (chuc > 1) str = so[chuc] + " mươi " + str;
    //            }
    //            if (tram < 0) break;
    //            else
    //            {
    //                if ((tram > 0) || (chuc > 0) || (donvi > 0)) str = so[tram] + " trăm " + str;
    //            }
    //            str = " " + str;
    //        }
    //    }
    //    if (booAm) str = "Âm " + str;


    //    return str.Substring(0, 1).ToUpper() + str.Substring(1) + "đồng chẵn";
    //}

    //public static string ChuyenTienDinhDangChu(double number)
    //{
    //    string s = number.ToString("#.##");
    //    string[] so = new string[] { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
    //    string[] hang = new string[] { "", "nghìn", "triệu", "tỷ" };
    //    int i, j, donvi, chuc, tram;
    //    string str = " ";
    //    bool booAm = false;
    //    double decS = 0;
    //    //Tung addnew
    //    try
    //    {
    //        decS = Convert.ToDouble(s.ToString());
    //    }
    //    catch
    //    {
    //    }
    //    if (decS < 0)
    //    {
    //        decS = -decS;
    //        s = decS.ToString();
    //        booAm = true;
    //    }
    //    i = s.Length;
    //    if (i == 0)
    //        str = so[0] + str;
    //    else
    //    {
    //        j = 0;
    //        while (i > 0)
    //        {
    //            donvi = Convert.ToInt32(s.Substring(i - 1, 1));
    //            i--;
    //            if (i > 0)
    //                chuc = Convert.ToInt32(s.Substring(i - 1, 1));
    //            else
    //                chuc = -1;
    //            i--;
    //            if (i > 0)
    //                tram = Convert.ToInt32(s.Substring(i - 1, 1));
    //            else
    //                tram = -1;
    //            i--;
    //            if ((donvi > 0) || (chuc > 0) || (tram > 0) || (j == 3))
    //                str = hang[j] + str;
    //            j++;
    //            if (j > 3) j = 1;
    //            if ((donvi == 1) && (chuc > 1))
    //                str = "một " + str;
    //            else
    //            {
    //                if ((donvi == 5) && (chuc > 0))
    //                    str = "lăm " + str;
    //                else if (donvi > 0)
    //                    str = so[donvi] + " " + str;
    //            }
    //            if (chuc < 0)
    //                break;
    //            else
    //            {
    //                if ((chuc == 0) && (donvi > 0)) str = "lẻ " + str;
    //                if (chuc == 1) str = "mười " + str;
    //                if (chuc > 1) str = so[chuc] + " mươi " + str;
    //            }
    //            if (tram < 0) break;
    //            else
    //            {
    //                if ((tram > 0) || (chuc > 0) || (donvi > 0)) str = so[tram] + " trăm " + str;
    //            }
    //            str = " " + str;
    //        }
    //    }
    //    if (booAm) str = "Âm " + str;
    //    return str.Substring(0, 1).ToUpper() + str.Substring(1) + "đồng chẵn";
    //}

    public static void HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
    {
        log4net.ILog log = log4net.LogManager.GetLogger(typeof(Utils));
        log.Debug(string.Format("Uploading {0} to {1}", file, url));
        string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
        byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

        HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
        wr.ContentType = "multipart/form-data; boundary=" + boundary;
        wr.Method = "POST";
        wr.KeepAlive = true;
        wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

        Stream rs = wr.GetRequestStream();

        string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
        foreach (string key in nvc.Keys)
        {
            rs.Write(boundarybytes, 0, boundarybytes.Length);
            string formitem = string.Format(formdataTemplate, key, nvc[key]);
            byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
            rs.Write(formitembytes, 0, formitembytes.Length);
        }
        rs.Write(boundarybytes, 0, boundarybytes.Length);

        string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
        string header = string.Format(headerTemplate, paramName, file, contentType);
        byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
        rs.Write(headerbytes, 0, headerbytes.Length);

        FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
        byte[] buffer = new byte[4096];
        int bytesRead = 0;
        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
        {
            rs.Write(buffer, 0, bytesRead);
        }
        fileStream.Close();

        byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
        rs.Write(trailer, 0, trailer.Length);
        rs.Close();

        WebResponse wresp = null;
        try
        {
            wresp = wr.GetResponse();
            Stream stream2 = wresp.GetResponseStream();
            StreamReader reader2 = new StreamReader(stream2);
            log.Debug(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
        }
        catch (Exception ex)
        {
            log.Error("Error uploading file", ex);
            if (wresp != null)
            {
                wresp.Close();
                wresp = null;
            }
        }
        finally
        {
            wr = null;
        }
    }
    //public static string GetValue(int ID_QLLH, string ParamName)
    //{
    //    string _value = "";
    //    SqlParameter[] par = new SqlParameter[]{
    //        new SqlParameter("@ID_QLLH",ID_QLLH),
    //        new SqlParameter("@ParamName",ParamName)

    //    };

    //    SqlDataHelper helper = new SqlDataHelper();
    //    object objData = helper.ExecuteScalar("sp_Config_GetByParamName", par);
    //    if (objData != null)
    //    {
    //        _value = objData.ToString();
    //    }
    //    return _value;
    //}
 
}
public class ByteArrayCustomQueue
{

    private LinkedList<byte[]> arrays = new LinkedList<byte[]>();

    /// <summary>
    /// Writes the specified data.
    /// </summary>
    /// <param name="data">The data.</param>
    public void Write(byte[] data)
    {
        arrays.AddLast(data);
    }

    /// <summary>
    /// Gets the length.
    /// </summary>
    /// <value>
    /// The length.
    /// </value>
    public int Length { get { return arrays.Sum(x => x.Length); } }

    /// <summary>
    /// Copies to stream.
    /// </summary>
    /// <param name="requestStream">The request stream.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void CopyToStream(Stream requestStream)
    {
        foreach (var array in arrays)
        {
            requestStream.Write(array, 0, array.Length);
        }
    }

   
}