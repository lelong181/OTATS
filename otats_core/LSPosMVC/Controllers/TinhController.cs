using LSPosMVC.Common;
using LSPos_Data.Data;
using System;
using System.Collections.Generic;
using System.Data;
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
    [RoutePrefix("api/tinh")]
    [EnableCors(origins: "*", "*", "*")]
    public class TinhController : ApiController
    {
        [HttpGet]
        [Route("getall_backup")]
        public HttpResponseMessage getall_backup()
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
                    Tinh_dl tinh_dl = new Tinh_dl();
                    List<Tinh> dsTinh = tinh_dl.GetTinhAll().OrderBy(x => x.TenTinh).ToList();                    

                    response = Request.CreateResponse(HttpStatusCode.OK, dsTinh);
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
                    DiaChiData diachi = new DiaChiData();
                    DataTable dtTinh = diachi.usp_vuongtm_getdanhsachtinh(userinfo.ID_QLLH);

                    response = Request.CreateResponse(HttpStatusCode.OK, dtTinh);
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
        [Route("getallbykhuvuc")]
        public HttpResponseMessage getallbykhuvuc([FromUri]int ID_KhuVuc)
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
                    Tinh_dl tinh_dl = new Tinh_dl();
                    List<Tinh> dsTinh = tinh_dl.GetTinhAll().Where(x => x.ID_KhuVuc == ID_KhuVuc).OrderBy(x => x.TenTinh).ToList();

                    response = Request.CreateResponse(HttpStatusCode.OK, dsTinh);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        public class ResultHCObj
        {
            public int Id { set; get; }
            public string TenNhom { set; get; }
            public int ParentId { set; get; }
            public List<ResultHCObj> ListChild { set; get; }
        }

        public void TaoNhom(NhomOBJ obj, List<NhomOBJ> lstDanhMuc, ResultHCObj resultObj)
        {
            resultObj.Id = obj.ID_Nhom;
            resultObj.TenNhom = obj.TenNhom;
            resultObj.ParentId = obj.ID_PARENT;

            var query1 = lstDanhMuc.Where(person => person.ID_PARENT == obj.ID_Nhom);

            List<ResultHCObj> li = new List<ResultHCObj>();
            foreach (NhomOBJ obj1 in query1)
            {
                ResultHCObj objcon = new ResultHCObj();
                TaoNhom(obj1, lstDanhMuc, objcon);
                li.Add(objcon);
            }
            resultObj.ListChild = li;
        }

        [HttpGet]
        [Route("treenhom")]
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
                    List<NhomOBJ> lstDanhMuc = new List<NhomOBJ>();
                    if (userinfo.Level == 1)
                    {
                        lstDanhMuc = NhomDB.getDS_Nhom(userinfo.ID_QLLH);
                    }
                    else
                    {
                        lstDanhMuc = NhomDB.getDS_Nhom_ByIdTaiKhoan(userinfo.ID_QuanLy);
                    }

                    List<ResultHCObj> list = new List<ResultHCObj>();

                    foreach (NhomOBJ obj in lstDanhMuc)
                    {
                        IEnumerable<NhomOBJ> findCha = lstDanhMuc.Where(person => person.ID_Nhom == obj.ID_PARENT);

                        bool flag = true;
                        foreach (NhomOBJ i in findCha)
                        {
                            flag = false;
                            break;
                        }

                        if (flag)
                        {
                            ResultHCObj resultObj = new ResultHCObj();

                            TaoNhom(obj, lstDanhMuc, resultObj);
                            
                            list.Add(resultObj);
                        }
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
    }
}
