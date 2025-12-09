
using LSPosMVC.Common;
using LSPos_Data.Data;
using LSPos_Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/chucnang")]
    [EnableCors(origins: "*", "*", "*")]
    public class ChucNangController : ApiController
    {

        /// <summary>
        /// Get all chức năng
        /// </summary>
        /// <returns></returns>
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
                    if (!userinfo.IsAdmin)
                    {
                        ChucNangData chucNangData = new ChucNangData();
                        List<MenuModels> lstChucnang = new List<MenuModels>();
                        lstChucnang = chucNangData.getDanhSachQuyen_WEB(userinfo.ID_QLLH);

                        if (userinfo.Level == 2)//user quan ly cap 2
                        {
                            List<MenuModels> lst = chucNangData.GetChucNangID_QuanLy(userinfo.ID_QuanLy);
                            if (lst != null && lst.Count > 0)
                                lstChucnang = lst;
                        }

                        response = Request.CreateResponse(HttpStatusCode.OK, lstChucnang);
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
        [Route("getallv2")]
        public HttpResponseMessage getv2()
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
                    if (!userinfo.IsAdmin)
                    {
                        ChucNangData chucNangData = new ChucNangData();
                        List<MenuModels> lstChucnang = new List<MenuModels>();
                        lstChucnang = chucNangData.getDanhSachQuyen_WEB(userinfo.ID_QLLH);

                        if (userinfo.Level == 2)//user quan ly cap 2
                        {
                            List<MenuModels> lst = chucNangData.GetChucNangID_QuanLy(userinfo.ID_QuanLy);
                            if (lst != null && lst.Count > 0)
                                lstChucnang = lst;
                        }

                        response = Request.CreateResponse(HttpStatusCode.OK, lstChucnang);
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
        [EnableCors(origins: "*", "*", "*")]
        [Route("tree")]
        public HttpResponseMessage gettree()
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
                    List<MenuModels> lstDanhMuc = new List<MenuModels>();

                    ChucNangData chucNangData = new ChucNangData();

                    lstDanhMuc = chucNangData.getmenu(userinfo.ID_QLLH);
                    if (userinfo.Level == 2)//user quan ly cap 2
                    {
                        //List<MenuModels> lst = chucNangData.GetChucNangID_QuanLy_v1(userinfo.ID_QuanLy);
                        List<MenuModels> lst = chucNangData.GetChucNangID_QuanLy(userinfo.ID_QuanLy);
                        //if (lst != null && lst.Count > 0)
                            lstDanhMuc = lst;
                    }
                    var roots = new Program();
                    List<Node> tree = roots.RawCollectionToTree(lstDanhMuc).ToList();

                    //string json = JsonConvert.SerializeObject(tree, Formatting.Indented,
                    //    new JsonSerializerSettings
                    //    {
                    //        PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    //        NullValueHandling = NullValueHandling.Ignore
                    //    });

                    response = Request.CreateResponse(HttpStatusCode.OK, tree);
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
        [Route("treenhanvien")]
        public HttpResponseMessage gettreenhanvien()
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
                    List<MenuModels> lstDanhMuc = new List<MenuModels>();

                    ChucNangData chucNangData = new ChucNangData();
                    lstDanhMuc = chucNangData.getDanhSachQuyen_WEB(userinfo.ID_QLLH);
                    var roots = new Program();
                    var tree = roots.RawCollectionToTree(lstDanhMuc).ToList();

                    //string json = JsonConvert.SerializeObject(tree, Formatting.Indented,
                    //    new JsonSerializerSettings
                    //    {
                    //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    //        NullValueHandling = NullValueHandling.Ignore
                    //    });

                    response = Request.CreateResponse(HttpStatusCode.OK, tree);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }


        class Node
        {
            [JsonProperty(PropertyName = "nodes")]
            public List<Node> Children = new List<Node>();

            public bool ShouldSerializeChildren()
            {
                return (Children.Count > 0);
            }

            //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public Node Parent { get; set; }
            public int Id { get; set; }
            public int? ParentId { get; set; }

            [JsonProperty(PropertyName = "text")]
            public string Name { get; set; }
            public string Icon { get; set; }
            public string Url { get; set; }
            public string URL_NEW { get; set; }
        }
        class Program
        {
            public IEnumerable<Node> RawCollectionToTree(List<MenuModels> collection)
            {
                var treeDictionary = new Dictionary<int?, Node>();

                collection.ForEach(x => treeDictionary.Add(x.Id, new Node { Id = x.Id, ParentId = x.ParentId, Name = x.Name, Icon = x.icon, Url = x.Url, URL_NEW = x.URL_NEW }));

                foreach (var item in treeDictionary.Values)
                {
                    if (item.ParentId != null)
                    {
                        Node proposedParent;

                        if (treeDictionary.TryGetValue(item.ParentId, out proposedParent))
                        {
                            item.Parent = proposedParent;

                            proposedParent.Children.Add(item);
                        }
                    }

                }
                return treeDictionary.Values.Where(x => x.Parent == null);
            }
        }

    }
}
