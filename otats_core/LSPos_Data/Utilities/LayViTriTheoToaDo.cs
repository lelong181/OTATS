using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

/// <summary>
/// Summary description for LayViTriTheoToaDo
/// </summary>
public class LayViTriTheoToaDo
{
   
    public LayViTriTheoToaDo()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    static log4net.ILog logs = log4net.LogManager.GetLogger(typeof(LayViTriTheoToaDo));
    public static string GetDiaDiemTheoToaDo(double ViDo, double KinhDo)
    {
        #region lấy điểm theo tọa độ
        string diachi = "";
        try
        {
            //AIzaSyBnwO1ETMtZC7AonESIQbpnwNaPvBhqVnI
            string apiKey = "";
            if (ConfigurationManager.AppSettings["GOOGLEAPIKEY"] != null)
            {
                apiKey = ConfigurationManager.AppSettings["GOOGLEAPIKEY"];

            }
            string url = "https://maps.google.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false&key=" + apiKey;
            url = string.Format(url, ViDo, KinhDo);
            WebRequest request = WebRequest.Create(url);
            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    DataSet dsResult = new DataSet();
                    dsResult.ReadXml(reader);
                   
                    diachi = dsResult.Tables["result"].Rows[0]["formatted_address"].ToString();
                    diachi = diachi.Replace("Unnamed Road,", "").Trim();
                    return diachi;
                }
            }
        }
        catch (Exception ex)
        {
            logs.Error(ex);
        }
        return diachi;
        #endregion
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
        string url = "https://maps.google.com/maps/api/geocode/xml?address={0}&sensor=false";
        if (apiKey != "")
        {
            url = "https://maps.google.com/maps/api/geocode/xml?address={0}&sensor=false&key=" + apiKey;
        }
      
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


    public Location GetTinhQuanHuyenPhuongXaDiaChi(string ViDo, string KinhDo)
    {
        
        // string apiKey = "AIzaSyBBqaCCSfggAJ7baakb944kCA_O5ir8S5M";
        string apiKey = "";

        if (ConfigurationManager.AppSettings["GOOGLEAPIKEY"] != null)
        {
            apiKey = ConfigurationManager.AppSettings["GOOGLEAPIKEY"];

        }

        #region lấy điểm theo tọa độ
        Location obj = new Location();
        obj.Latitude = Convert.ToDouble( ViDo );
        obj.Longitude = Convert.ToDouble(KinhDo);
        string url = "https://maps.google.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false&key=" + apiKey;
        //string url = "https://maps.google.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false";
        url = string.Format(url, ViDo, KinhDo);
        WebRequest request = WebRequest.Create(url);
        using (WebResponse response = (HttpWebResponse)request.GetResponse())
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                XmlNode result = doc.DocumentElement.ChildNodes[1];

                foreach (XmlNode address_component in result.ChildNodes)
                {
                    if (address_component.Name == "address_component")
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
                    else if (address_component.Name == "formatted_address")
                    {
                        obj.DiaChi = address_component.InnerText;
                    }
                }
                return obj;
                //return dsResult.Tables["result"].Rows[0]["formatted_address"].ToString();
            }
        }

        #endregion
    }

}
 