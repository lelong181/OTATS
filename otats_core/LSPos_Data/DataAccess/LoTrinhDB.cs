using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LoTrinhDB
/// </summary>
public class LoTrinhDB
{
    public static SqlDataHelper db = new SqlDataHelper();
    public LoTrinhDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static List<gpsnhanvien> GetViTriTatCaNV(int idcty, int ID_QuanLy, int ID_Nhom,int loctrangthai)
    {
        List<gpsnhanvien> rs = new List<gpsnhanvien>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idcty", idcty),
                new SqlParameter("@ID_QuanLy", ID_QuanLy),
                 new SqlParameter("@ID_Nhom", ID_Nhom),
                 new SqlParameter("@loctrangthai", loctrangthai)
            };
            DataTable dt = db.ExecuteDataSet("sp_QL_getViTriTatCaNV", par).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
 
                    rs.Add(new gpsnhanvien
                    {
                        idnhanvien = int.Parse(dr["idnhanvien"].ToString()),
                        tennhanvien = dr["TenNhanVien"].ToString(),
                        KinhDo = double.Parse(dr["KinhDo"].ToString()),
                        ViDo = double.Parse(dr["ViDo"].ToString()),
                        dangtructuyen = int.Parse(dr["dangtructuyen"].ToString()),
                        thoigiancapnhat = DateTime.Parse(dr["ReceivedTime"].ToString()).ToString("dd/MM/yyyy HH:mm:ss")
                    });
                    
                }
                catch
                {
                    rs.Add(new gpsnhanvien
                    {
                        tennhanvien = null,
                        KinhDo = 0,
                        ViDo = 0,
                        dangtructuyen = 0,
                        thoigiancapnhat = null
                    });
                }
            }
            return rs;
        }
        catch
        {
            return rs;
        }
    }
    public static List<gpsnhanvien> GetViTriTatCaNVOnline(int idcty, int ID_QuanLy,int trangthai)
    {
        
        List<gpsnhanvien> rs = new List<gpsnhanvien>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idcty", idcty),
                new SqlParameter("@ID_QuanLy", ID_QuanLy)
            };
            DataTable dt = db.ExecuteDataSet("sp_QL_getViTriTatCaNV", par).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    if (trangthai == -1) // tat ca
                    {
                        rs.Add(new gpsnhanvien
                        {
                            idnhanvien = int.Parse(dr["idnhanvien"].ToString()),
                            tennhanvien = dr["TenNhanVien"].ToString(),
                            KinhDo = double.Parse(dr["KinhDo"].ToString()),
                            ViDo = double.Parse(dr["ViDo"].ToString()),
                            dangtructuyen = int.Parse(dr["dangtructuyen"].ToString()),
                            thoigiancapnhat = DateTime.Parse(dr["ReceivedTime"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                            ID_Nhom = dr["ID_Nhom"].ToString() != "" ? int.Parse(dr["ID_Nhom"].ToString())  : 0
                        });

                    }
                    else 
                    {
                        if (int.Parse(dr["dangtructuyen"].ToString()) == trangthai)
                        {
                            rs.Add(new gpsnhanvien
                            {
                                idnhanvien = int.Parse(dr["idnhanvien"].ToString()),
                                tennhanvien = dr["TenNhanVien"].ToString(),
                                KinhDo = double.Parse(dr["KinhDo"].ToString()),
                                ViDo = double.Parse(dr["ViDo"].ToString()),
                                dangtructuyen = int.Parse(dr["dangtructuyen"].ToString()),
                                thoigiancapnhat = DateTime.Parse(dr["ReceivedTime"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                                ID_Nhom = dr["ID_Nhom"].ToString() != "" ? int.Parse(dr["ID_Nhom"].ToString()) : 0
                            });
                        }
                    }
                    
                }
                catch
                {
                    rs.Add(new gpsnhanvien
                    {
                        tennhanvien = null,
                        KinhDo = 0,
                        ViDo = 0,
                        dangtructuyen = 0,
                        thoigiancapnhat = null
                    });
                }
            }
            return rs;
        }
        catch
        {
            return rs;
        }
    }
    public static List<gpsnhanvien> GetViTriTatCaNVTheoToaDo(int idcty, int ID_QuanLy,float KinhDo, float ViDo,int sombankinh,int trangthai)
    {
        List<gpsnhanvien> rs = new List<gpsnhanvien>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idcty", idcty),
                new SqlParameter("@ID_QuanLy", ID_QuanLy),
                new SqlParameter("@KinhDo", KinhDo),
                new SqlParameter("@ViDo", ViDo),
                new SqlParameter("@mbankinh", sombankinh),
            };
            DataTable dt = db.ExecuteDataSet("sp_QL_getViTriTatCaNV_TheoToaDo", par).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    if(trangthai == -1 || int.Parse(dr["dangtructuyen"].ToString()) == trangthai)
                    {
                        rs.Add(new gpsnhanvien
                        {
                            idnhanvien = int.Parse(dr["idnhanvien"].ToString()),
                            tennhanvien = dr["TenNhanVien"].ToString(),
                            KinhDo = double.Parse(dr["KinhDo"].ToString()),
                            ViDo = double.Parse(dr["ViDo"].ToString()),
                            dangtructuyen = int.Parse(dr["dangtructuyen"].ToString()),
                            thoigiancapnhat = DateTime.Parse(dr["ReceivedTime"].ToString()).ToString("dd/MM/yyyy HH:mm:ss"),
                            ID_Nhom = dr["ID_Nhom"].ToString() != "" ? int.Parse(dr["ID_Nhom"].ToString()) : 0
                        });
                    }
                }
                catch
                {
                    rs.Add(new gpsnhanvien
                    {
                        tennhanvien = null,
                        KinhDo = 0,
                        ViDo = 0,
                        dangtructuyen = 0,
                        thoigiancapnhat = null
                    });
                }
            }
            return rs;
        }
        catch
        {
            return rs;
        }
    }

    public static gpsnhanvien GetViTriHienTaiNV(int idnhanvien)
    {
        gpsnhanvien td = new gpsnhanvien();
        DataTable dt = new DataTable();


        try
        {
            SqlParameter[] param = new SqlParameter[] {
                new SqlParameter("@IDNhanVien", idnhanvien),
            };

            dt = db.ExecuteDataSet("sp_QL_getViTriOnlineTheoIDNV", param).Tables[0];
            DataRow dr = dt.Rows[0];

            td.KinhDo = (dr["KinhDo"] == null || dr["KinhDo"].ToString().Trim() == "") ? 0 : double.Parse(dr["KinhDo"].ToString());
            td.ViDo = (dr["ViDo"] == null || dr["ViDo"].ToString().Trim() == "") ? 0 : double.Parse(dr["ViDo"].ToString());
            td.thoigiancapnhat = (dr["ReceivedTime"] == null || dr["ReceivedTime"].ToString().Trim() == "") ? "" : DateTime.Parse(dr["ReceivedTime"].ToString()).ToString("dd/MM/yyyy HH:mm:ss");
            td.tennhanvien = NhanVien_dl.TenNhanVien(idnhanvien);
            return td;
        }
        catch
        {
            return null;
        }
    }

    public static List<LichSuDiChuyenOBJ> LichSuDiChuyenTheoNhanVien(int idnhanvien, DateTime tungay, DateTime denngay)
    {
        List<LichSuDiChuyenOBJ> rs = new List<LichSuDiChuyenOBJ>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idnhanvien", idnhanvien),
                new SqlParameter("@tungay", tungay),
                new SqlParameter("@denngay", denngay)
            };
            DataTable dt = db.ExecuteDataSet("sp_QL_LichSuDiChuyen_v2", par).Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                rs.Add(new LichSuDiChuyenOBJ
                {
                    nhanvien = dr["nhanvien"].ToString(),
                    thoigian = DateTime.Parse(dr["thoigian"].ToString()),
                    kinhdo = double.Parse(dr["kinhdo"].ToString()),
                    vido = double.Parse(dr["vido"].ToString()),
                    ghichu = dr["ghichu"].ToString(),
                    accuracy = dr["accuracy"].ToString() != "" ? double.Parse(dr["accuracy"].ToString()) : 0,
                    idkhachhang = dr["idkhachhang"].ToString() != "" ? int.Parse(dr["idkhachhang"].ToString()) : 0,
                    tenkhachhang = dr["tenkhachhang"].ToString(),
                    diachikhachhang = dr["diachikhachhang"].ToString(),
                    thoigiantaidiem = dr["thoigiantaidiem"].ToString() != "" ? DateTime.Parse(dr["thoigiantaidiem"].ToString()) : new DateTime(1900,01,01),
                    thoigianvaodiem = dr["thoigianvaodiem"].ToString() != "" ? DateTime.Parse(dr["thoigianvaodiem"].ToString()) : new DateTime(1900, 01, 01),
                    thoigianradiem = dr["thoigianradiem"].ToString() != "" ? DateTime.Parse(dr["thoigianradiem"].ToString()) : new DateTime(1900, 01, 01),
                    type = dr["type"].ToString() != "" ? int.Parse(dr["type"].ToString()) : 1,
                });
            }

            return rs;
        }
        catch
        {
            return rs;
        }
    }
    public static DataTable LichSuDiChuyenTheoNhanVien_Online(int idnhanvien, DateTime tungay, DateTime denngay)
    {
        List<LichSuDiChuyenOBJ> rs = new List<LichSuDiChuyenOBJ>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idnhanvien", idnhanvien),
                new SqlParameter("@tungay", tungay),
                new SqlParameter("@denngay", denngay)
            };
            DataTable dt = db.ExecuteDataSet("sp_QL_LichSuDiChuyen_Online", par).Tables[0];



            return dt;
        }
        catch
        {
            return null;
        }
    }
    public static DataTable LichSuDiChuyenTheoNhanVien_Online_Offline(int idnhanvien, DateTime tungay, DateTime denngay, int ID_QLLH, int ID_QuanLy)
    {
        List<LichSuDiChuyenOBJ> rs = new List<LichSuDiChuyenOBJ>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idnhanvien", idnhanvien),
                new SqlParameter("@tungay", tungay),
                new SqlParameter("@denngay", denngay),
                new SqlParameter("@ID_QLLH", ID_QLLH),
                new SqlParameter("@ID_QuanLy", ID_QuanLy)
            };
            DataTable dt = db.ExecuteDataSet("sp_QL_LichSuDiChuyen_Online_Offline", par).Tables[0];



            return dt;
        }
        catch
        {
            return null;
        }
    }
    public static DataTable LichSuDiChuyenTheoNhanVien_Online_Offline_v2(int idnhanvien, DateTime tungay, DateTime denngay, int ID_QLLH, int ID_QuanLy)
    {
        List<LichSuDiChuyenOBJ> rs = new List<LichSuDiChuyenOBJ>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idnhanvien", idnhanvien),
                new SqlParameter("@tungay", tungay),
                new SqlParameter("@denngay", denngay),
                new SqlParameter("@ID_QLLH", ID_QLLH),
                new SqlParameter("@ID_QuanLy", ID_QuanLy)
            };
            DataTable dt = db.ExecuteDataSet("sp_QL_LichSuDiChuyen_Online_Offline_v2", par).Tables[0];



            return dt;
        }
        catch
        {
            return null;
        }
    }

    //public static DataTable LichSuDiChuyenTheoNhanVien_Online_Offline_LoTrinh(int idnhanvien, DateTime tungay, DateTime denngay)
    //{
    //    List<LichSuDiChuyenOBJ> rs = new List<LichSuDiChuyenOBJ>();
    //    try
    //    {
    //        SqlParameter[] par = new SqlParameter[]{
    //            new SqlParameter("@idnhanvien", idnhanvien),
    //            new SqlParameter("@tungay", tungay),
    //            new SqlParameter("@denngay", denngay)
    //        };
    //        DataTable dt = db.ExecuteDataSet("sp_QL_LichSuDiChuyen_Online_Offline_LoTrinh", par).Tables[0];



    //        return dt;
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}

    public static DataTable LichSuDiChuyenTheoNhanVien_Offline(int idnhanvien, DateTime tungay, DateTime denngay)
    {
        List<LichSuDiChuyenOBJ> rs = new List<LichSuDiChuyenOBJ>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idnhanvien", idnhanvien),
                new SqlParameter("@tungay", tungay),
                new SqlParameter("@denngay", denngay)
            };
            DataTable dt = db.ExecuteDataSet("sp_QL_LichSuDiChuyen_Offline", par).Tables[0];

            //foreach (DataRow dr in dt.Rows)
            //{
            //    rs.Add(new LichSuDiChuyenOBJ
            //    {
            //        nhanvien = dr["nhanvien"].ToString(),
            //        thoigian = DateTime.Parse(dr["thoigian"].ToString()),
            //        kinhdo = double.Parse(dr["kinhdo"].ToString()),
            //        vido = double.Parse(dr["vido"].ToString()),
            //        ghichu = dr["ghichu"].ToString(),
            //        accuracy = dr["accuracy"].ToString() != "" ? double.Parse(dr["accuracy"].ToString()) : 0,
            //        idkhachhang = dr["idkhachhang"].ToString() != "" ? int.Parse(dr["idkhachhang"].ToString()) : 0,
            //        tenkhachhang = dr["tenkhachhang"].ToString(),
            //        diachikhachhang = dr["diachikhachhang"].ToString(),
            //        thoigiantaidiem = dr["thoigiantaidiem"].ToString() != "" ? DateTime.Parse(dr["thoigiantaidiem"].ToString()) : new DateTime(1900, 01, 01),
            //        thoigianvaodiem = dr["thoigianvaodiem"].ToString() != "" ? DateTime.Parse(dr["thoigianvaodiem"].ToString()) : new DateTime(1900, 01, 01),
            //        thoigianradiem = dr["thoigianradiem"].ToString() != "" ? DateTime.Parse(dr["thoigianradiem"].ToString()) : new DateTime(1900, 01, 01),
            //        type = dr["type"].ToString() != "" ? int.Parse(dr["type"].ToString()) : 0,
            //    });
            //}

            return dt;
        }
        catch
        {
            return null;
        }
    }

    //public static List<LichSuDiChuyenOBJ> LichSuDiChuyenTheoNhanVien_Offline(int idnhanvien, DateTime tungay, DateTime denngay)
    //{
    //    List<LichSuDiChuyenOBJ> rs = new List<LichSuDiChuyenOBJ>();
    //    try
    //    {
    //        SqlParameter[] par = new SqlParameter[]{
    //            new SqlParameter("@idnhanvien", idnhanvien),
    //            new SqlParameter("@tungay", tungay),
    //            new SqlParameter("@denngay", denngay)
    //        };
    //        DataTable dt = db.ExecuteDataSet("sp_QL_LichSuDiChuyen_Offline", par).Tables[0];

    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            rs.Add(new LichSuDiChuyenOBJ
    //            {
    //                nhanvien = dr["nhanvien"].ToString(),
    //                thoigian = DateTime.Parse(dr["thoigian"].ToString()),
    //                kinhdo = double.Parse(dr["kinhdo"].ToString()),
    //                vido = double.Parse(dr["vido"].ToString()),
    //                ghichu = dr["ghichu"].ToString(),
    //                accuracy = dr["accuracy"].ToString() != "" ? double.Parse(dr["accuracy"].ToString()) : 0,
    //                idkhachhang = dr["idkhachhang"].ToString() != "" ? int.Parse(dr["idkhachhang"].ToString()) : 0,
    //                tenkhachhang = dr["tenkhachhang"].ToString(),
    //                diachikhachhang = dr["diachikhachhang"].ToString(),
    //                thoigiantaidiem = dr["thoigiantaidiem"].ToString() != "" ? DateTime.Parse(dr["thoigiantaidiem"].ToString()) : new DateTime(1900, 01, 01),
    //                thoigianvaodiem = dr["thoigianvaodiem"].ToString() != "" ? DateTime.Parse(dr["thoigianvaodiem"].ToString()) : new DateTime(1900, 01, 01),
    //                thoigianradiem = dr["thoigianradiem"].ToString() != "" ? DateTime.Parse(dr["thoigianradiem"].ToString()) : new DateTime(1900, 01, 01),
    //                type = dr["type"].ToString() != "" ? int.Parse(dr["type"].ToString()) : 0,
    //            });
    //        }

    //        return rs;
    //    }
    //    catch
    //    {
    //        return rs;
    //    }
    //}

    public static List<LichSuDiChuyenOBJ> LichSuDiChuyenTheoBaoCaoKM(int idnhanvien, DateTime tungay, DateTime denngay)
    {
        List<LichSuDiChuyenOBJ> rs = new List<LichSuDiChuyenOBJ>();
        try
        {
            SqlParameter[] par = new SqlParameter[]{
                new SqlParameter("@idnhanvien", idnhanvien),
                new SqlParameter("@tungay", tungay),
                new SqlParameter("@denngay", denngay)
            };
            DataTable dt = db.ExecuteDataSet("sp_QL_LichSuDiChuyenTuBCKM", par).Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                rs.Add(new LichSuDiChuyenOBJ
                {
                    nhanvien = dr["nhanvien"].ToString(),
                    thoigian = DateTime.Parse(dr["thoigian"].ToString()),
                    kinhdo = double.Parse(dr["kinhdo"].ToString()),
                    vido = double.Parse(dr["vido"].ToString()),
                    ghichu = dr["ghichu"].ToString()

                });
            }

            return rs;
        }
        catch
        {
            return rs;
        }
    }

}