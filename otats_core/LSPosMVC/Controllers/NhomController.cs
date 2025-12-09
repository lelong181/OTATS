using LSPos_Data.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Data;
using LSPosMVC.Common;
using System.Web.Http.Cors;

namespace LSPosMVC.Controllers
{
    public class NhomNew
    {
        public int ID_Nhom { get; set; }
        public int iD_PARENT { get; set; }
        public string TenNhom { get; set; }
        public int ID_QLLH { get; set; }
        public string MaNhom { get; set; }
        public string SiteCode { get; set; }
        public DateTime NgayTao { get; set; }
        public int SoLuongNhanVien { get; set; }
        public int SoLuongQuanLy { get; set; }
        public string TenHienThi_NhanVien { get; set; }
        public string TenHienThi_QuanLy { get; set; }
        public int TrangThai { get; set; }
        public List<NhomNew> childs { get; set; }
    }

    public class ResultObj
    {
        public int ID_Nhom { get; set; }
        public int iD_PARENT { get; set; }
        public string TenNhom { get; set; }
        public int ID_QLLH { get; set; }
        public string MaNhom { get; set; }
        public string SiteCode { get; set; }
        public DateTime NgayTao { get; set; }
        public int SoLuongNhanVien { get; set; }
        public int SoLuongQuanLy { get; set; }
        public string TenHienThi_NhanVien { get; set; }
        public string TenHienThi_QuanLy { get; set; }
        public int TrangThai { get; set; }
        public List<ResultObj> childs { get; set; }
    }
    [Authorize]
    [RoutePrefix("api/nhomnhanvien")]
    [EnableCors(origins: "*", "*", "*")]
    public class NhomController : ApiController
    {
        public static void test(NhomNew nn, List<NhomOBJ> lstDanhMuc)
        {
            List<NhomOBJ> lstTmp = new List<NhomOBJ>();
            foreach (NhomOBJ tmp in lstDanhMuc)
            {
                if ((nn.iD_PARENT == tmp.ID_PARENT) || (nn.ID_Nhom == tmp.ID_PARENT))
                {
                    NhomNew nhomNew = new NhomNew();
                    nhomNew.ID_Nhom = tmp.ID_Nhom;
                    nhomNew.iD_PARENT = tmp.ID_PARENT;
                    nhomNew.TenNhom = tmp.TenNhom;
                    nhomNew.TenHienThi_NhanVien = tmp.TenHienThi_NhanVien;
                    nhomNew.childs = new List<NhomNew>();
                    nn.childs.Add(nhomNew);
                }
                else
                {
                    lstTmp.Add(tmp);
                }
            }
            foreach (NhomNew s in nn.childs)
            {
                test(s, lstTmp);
            }
        }
        [HttpGet]
        [Route("getalltree")]
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
                    NhomNew nhomNew = new NhomNew();
                    List<NhomOBJ> lstDanhMuc = new List<NhomOBJ>();
                    if (userinfo.Level == 1)
                    {
                        lstDanhMuc = NhomDB.getDS_Nhom(userinfo.ID_QLLH);
                    }
                    else
                    {
                        lstDanhMuc = NhomDB.getDS_Nhom_ByIdTaiKhoan(userinfo.ID_QuanLy);

                    }
                    //IEnumerable<NhomOBJ> findCha = lstDanhMuc.Where(person => person.ID_Nhom == obj.ID_PARENT);
                    nhomNew.iD_PARENT = 0;
                    nhomNew.TenNhom = "";
                    nhomNew.TenHienThi_NhanVien = "";
                    nhomNew.childs = new List<NhomNew>();
                    test(nhomNew, lstDanhMuc);
                    response = Request.CreateResponse(HttpStatusCode.OK, nhomNew);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.NotModified, ex);
                LSPos_Data.Utilities.Log.Error(ex);
            }

            return response;
        }

        public void TaoNhom(NhomOBJ obj, List<NhomOBJ> lstDanhMuc, ResultObj resultObj)
        {
            resultObj.ID_Nhom = obj.ID_Nhom;
            resultObj.TenNhom = obj.TenNhom;
            resultObj.iD_PARENT = obj.ID_PARENT;
            resultObj.TenHienThi_NhanVien = obj.TenHienThi_NhanVien;
            resultObj.MaNhom = obj.MaNhom;
            resultObj.SiteCode = obj.SiteCode;
            var query1 = lstDanhMuc.Where(person => person.ID_PARENT == obj.ID_Nhom);

            List<ResultObj> li = new List<ResultObj>();
            foreach (NhomOBJ obj1 in query1)
            {
                ResultObj objcon = new ResultObj();
                TaoNhom(obj1, lstDanhMuc, objcon);
                li.Add(objcon);
            }
            resultObj.childs = li;
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
                    BaoCaoCommon baocao = new BaoCaoCommon();
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    List<NhomOBJ> lstDanhMuc = new List<NhomOBJ>();
                    if (userinfo.Level == 1)
                    {
                        lstDanhMuc = NhomData.getDS_Nhom(userinfo.ID_QLLH, lang);
                    }
                    else
                    {
                        lstDanhMuc = NhomData.getDS_NhomByTaiKhoan(userinfo.ID_QuanLy, userinfo.ID_QuanLy, lang);
                    }

                    List<ResultObj> list = new List<ResultObj>();

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
                            ResultObj resultObj = new ResultObj();

                            TaoNhom(obj, lstDanhMuc, resultObj);

                            list.Add(resultObj);
                        }
                    }
                    ResultObj nhomNew = new ResultObj();
                    nhomNew.iD_PARENT = 0;
                    nhomNew.TenNhom = "";
                    nhomNew.MaNhom = "";
                    nhomNew.TenHienThi_NhanVien = "";
                    nhomNew.childs = new List<ResultObj>();
                    list.Add(nhomNew);
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
        [Route("getallcombox")]
        public HttpResponseMessage getgetallcombox()
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
                    NhomNew nhomNew = new NhomNew();
                    List<NhomOBJ> lstDanhMuc = new List<NhomOBJ>();
                    if (userinfo.Level == 1)
                    {
                        //lstDanhMuc = NhomDB.getDS_Nhom_Active(userinfo.ID_QLLH);
                        lstDanhMuc = NhomDB.getDS_Nhom(userinfo.ID_QLLH);
                        lstDanhMuc.RemoveAt(0);

                    }
                    else
                    {
                        lstDanhMuc = NhomDB.getDS_Nhom_ByIdTaiKhoan(userinfo.ID_QuanLy);
                        if (lstDanhMuc.Count == 0)
                        {
                            lstDanhMuc = NhomDB.getDS_Nhom_Active(userinfo.ID_QLLH);
                        }
                    }
                    //nhomNew.iD_PARENT = 0;
                    //nhomNew.TenNhom = "";
                    //nhomNew.childs = new List<NhomNew>();
                    //test(nhomNew, lstDanhMuc);
                    response = Request.CreateResponse(HttpStatusCode.OK, lstDanhMuc);
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
        [Route("getNhombyID")]
        public HttpResponseMessage getNhombyID([FromUri] NhomNew tieuchi)
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
                    NhomOBJ dm = NhomDB.get_NhomById(tieuchi.ID_Nhom);
                    response = Request.CreateResponse(HttpStatusCode.OK, dm);
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
        [Route("insertNhom")]
        public HttpResponseMessage addnhom([FromBody] NhomNew NhomNewAdd)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    return response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }

                NhomData nhomdata = new NhomData();
                DataTable checkmanhom = nhomdata.CheckmaNhom(NhomNewAdd.MaNhom, userinfo.ID_QLLH);
                if (checkmanhom.Rows.Count > 0)
                {
                    return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_manhomdatontai" });
                }
                else
                {
                    NhomOBJ nhomNew = new NhomOBJ();
                    nhomNew.TenNhom = NhomNewAdd.TenNhom;
                    nhomNew.MaNhom = NhomNewAdd.MaNhom;
                    nhomNew.SiteCode = NhomNewAdd.SiteCode;
                    if (NhomNewAdd.iD_PARENT == -2)
                    {
                        nhomNew.ID_PARENT = 0;
                    }
                    else
                    {
                        nhomNew.ID_PARENT = NhomNewAdd.iD_PARENT;
                    }
                    nhomNew.ID_QLLH = userinfo.ID_QLLH;
                    if (NhomDB.Them(nhomNew))
                    {
                        NhomOBJ nhom = NhomDB.get_NhomByTenNhom(nhomNew.TenNhom);
                        if (nhom != null)
                        {
                            NhomDB.PhanNhom(nhom.ID_Nhom, userinfo.ID_QuanLy);//phan nhom cho tai khoan hien tai
                            //phân nhóm quản lý cho các tài khoản đang quản lý nhóm cấp trên.
                            if (nhomNew.ID_PARENT > 0)
                            {
                                User_dl u = new User_dl();
                                List<UserInfo> lstU = u.GetListUserByNhom(NhomNewAdd.iD_PARENT, userinfo.ID_QLLH);
                                foreach (UserInfo us in lstU)
                                {
                                    NhomDB.PhanNhom(nhom.ID_Nhom, us.ID_QuanLy);
                                }

                            }
                        }
                        return response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_themnhomthanhcong" });
                    }
                    else
                    {
                        return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_themnhomthatbai" });
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
        [Route("editNhom")]
        public HttpResponseMessage editnhom([FromBody] NhomNew NhomNewAdd)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    return response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                NhomData nhomdata = new NhomData();
                DataTable checkmanhom = nhomdata.CheckeditmaNhom(NhomNewAdd.MaNhom, userinfo.ID_QLLH, NhomNewAdd.ID_Nhom);
                if (checkmanhom.Rows.Count > 0)
                {
                    return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_manhomdatontai" });
                }
                else
                {
                    NhomOBJ nhomNew = new NhomOBJ();
                    nhomNew.ID_Nhom = NhomNewAdd.ID_Nhom;
                    nhomNew.TenNhom = NhomNewAdd.TenNhom;
                    nhomNew.MaNhom = NhomNewAdd.MaNhom;
                    nhomNew.SiteCode = NhomNewAdd.SiteCode;
                    if (NhomNewAdd.iD_PARENT == -2)
                    {
                        nhomNew.ID_PARENT = 0;
                    }
                    else
                    {
                        nhomNew.ID_PARENT = NhomNewAdd.iD_PARENT;
                    }
                    nhomNew.ID_QLLH = userinfo.ID_QLLH;
                    if (NhomDB.Sua(nhomNew))
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_themnhomthanhcong" });
                    }
                    else
                    {
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_themnhomthatbai" });
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
        [Route("deleteNhom")]
        public HttpResponseMessage deletenhom([FromUri] int ID_Nhom)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response = Request.CreateResponse(HttpStatusCode.NotFound, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);

            try
            {
                UserInfo userinfo = utilsCommon.checkAuthorization();

                if (userinfo == null)
                {
                    return response = Request.CreateResponse(HttpStatusCode.Unauthorized, Config.DU_LIEU_TOKEN_TRUY_CAP_KHONG_DUNG);
                }
                NhomData nhomdata = new NhomData();
                DataTable checktenhom = nhomdata.CheckXoaNhomNV(ID_Nhom, userinfo.ID_QLLH);
                if (checktenhom.Rows.Count > 0)
                {
                    return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_khongthexoanhomdatontainhanvien" });
                }
                else
                {

                    NhomOBJ nhomNew = new NhomOBJ();
                    nhomNew.ID_Nhom = ID_Nhom;
                    if (NhomDB.Xoa(nhomNew))
                    {
                        return response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoanhomthanhcong" });
                    }
                    else
                    {
                        return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoanhomthatbaivuilongthulai" });
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
        [Route("getall")]
        public HttpResponseMessage getall()
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

                    BaoCaoCommon baocao = new BaoCaoCommon();
                    string lang = baocao.GetCurrentLanguages(userinfo.ID_QLLH, userinfo.ID_QuanLy);

                    List<NhomOBJ> lstDanhMuc = new List<NhomOBJ>();
                    if (userinfo.Level == 1)
                    {
                        lstDanhMuc = NhomData.getDS_Nhom(userinfo.ID_QLLH, lang);
                    }
                    else
                    {
                        lstDanhMuc = NhomData.getDS_NhomByTaiKhoan(userinfo.ID_QuanLy, userinfo.ID_QLLH, lang);
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, lstDanhMuc);
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
        [Route("getlistchietkhau")]
        public HttpResponseMessage getlistchietkhau()
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
                    NhomData nhomdata = new NhomData();
                    DataTable dt = nhomdata.getlistchietkhau();

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


        [HttpPost]
        [Route("setchietkhau")]
        public HttpResponseMessage setchietkhau(int idnhom, decimal hoahong)
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
                    NhomData nhomdata = new NhomData();
                    if (nhomdata.setchietkhau(idnhom, hoahong))
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "Cập nhật dữ liệu thành công"});
                    else
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "Cập nhật dữ liệu thất bại" });
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

