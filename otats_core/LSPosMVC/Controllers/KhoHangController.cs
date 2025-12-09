using Ksmart_DataSon.DataAccess;
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
using System.Runtime.Remoting.Messaging;
using System.Data.SqlClient;

namespace LSPosMVC.Controllers
{
    [Authorize]
    [RoutePrefix("api/khohang")]
    public class KhoHangController : ApiController
    {
        [HttpGet]
        [Route("getbyidhang")]
        public HttpResponseMessage getbymahang([FromUri] int idmathang)
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
                    DonHangData donHangData = new DonHangData();

                    DataTable dt = KhoDB.DanhSachKho_MatHang(idmathang, userinfo.ID_QLLH);

                    List<KhoOBJ> list = new List<KhoOBJ>();

                    foreach (DataRow dr in dt.Rows)
                    {
                        KhoOBJ k = new KhoOBJ();
                        k.ID_Kho = int.Parse(dr["ID_Kho"].ToString());
                        k.TenKho = dr["TenKho"].ToString().Trim();
                        list.Add(k);
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

        [HttpGet]
        [Route("getlist")]
        public HttpResponseMessage getlist()
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
                    KhoDB khoDB = new KhoDB();

                    List<KhoOBJ> list = khoDB.GetListDanhSach(userinfo.ID_QLLH);

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
                    KhoDB khoDB = new KhoDB();

                    DataTable list = khoDB.GetDataDanhSach(userinfo.ID_QLLH);

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
        [Route("getlistnhanvienbyidkho")]
        public HttpResponseMessage getlistnhanvienbyidkho([FromUri] int idkho)
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
                    NhanVien_dl mh = new NhanVien_dl();
                    List<NhanVien> dt = mh.GetDSNhanVien_TheoKho(userinfo.ID_QLLH, userinfo.ID_QuanLy, idkho);

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
        [Route("getlistmathangbyidkho")]
        public HttpResponseMessage getlistmathangbyidkho([FromUri] int idkho)
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
                    MatHang_dl mh = new MatHang_dl();
                    List<MatHang> dt = mh.GetMatHangAll_TheoKho(idkho);

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
        [Route("deletekho")]
        public HttpResponseMessage deletekho([FromUri] int idkho)
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
                    MatHang_dl mh = new MatHang_dl();
                    List<MatHang> dt = mh.GetMatHangAll_TheoKho(idkho);

                    KhoDB km_dl = new KhoDB();
                    if (dt != null && dt.Count() > 0)
                        response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_khohangdatontaimathanghoacchuamathangduocxuatkhongthexoa" });
                    else
                    {
                        if (km_dl.Xoa(idkho, userinfo.ID_QuanLy))
                        {

                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_xoakhohangthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_xoakhohangthatbai" });
                        }
                    }
                    
                    return response;
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
        [Route("phanquyennhanvienkho")]
        public HttpResponseMessage phanquyennhanvienkho([FromUri] int idkho, [FromBody] List<int> listidnhanvien)
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
                    SqlDataHelper helper = new SqlDataHelper();

                    int cnt = 0;
                    SqlParameter[] procParams = new SqlParameter[1]
                {
                    new SqlParameter("@ID_Kho", idkho)
                };
                    helper.ExecuteNonQuery("sp_Kho_XoaNhanVien", procParams);
                    foreach (int id in listidnhanvien)
                    {

                        DataSet dataSet = helper.ExecuteDataSet("Select * from Kho_NhanVien where ID_Kho = " + idkho + " and ID_NhanVien = " + id);
                        if (dataSet.Tables[0].Rows.Count > 0)
                        {
                            
                        }
                        else
                        {

                            int num = helper.ExecuteNonQuery("Insert into Kho_NhanVien(ID_Kho,ID_NhanVien) values(" + idkho + "," + id + ")");
                            cnt++;
                        }
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_phanquyenthanhcong" });

                    return response;
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
        [Route("themsuakho")]
        public HttpResponseMessage themsuakho([FromBody] KhoOBJ obj)
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
                    obj.ID_QLLH = userinfo.ID_QLLH;
                    //obj.TrangThai = 1;
                    KhoDB khoDB = new KhoDB();

                    if (obj.ID_Kho > 0)
                    {
                        if (khoDB.Sua(obj, userinfo.ID_QuanLy))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_suakhohangthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_suakhohangthatbai" });
                        }
                    }
                    else
                    {
                        if (khoDB.Them(obj))
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, msg = "label_themkhohangthanhcong" });
                        }
                        else
                        {
                            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, msg = "label_themkhohangthatbai" });
                        }

                    }


                    return response;
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
