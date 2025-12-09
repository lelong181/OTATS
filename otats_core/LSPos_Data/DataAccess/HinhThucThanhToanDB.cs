using LSPos_Data.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HinhThucThanhToanDB
{
    private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(DanhMucDB));

    public static SqlDataHelper db = new SqlDataHelper();

    public HinhThucThanhToanDB()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public List<HinhThucThanhToanModel> getListHinhThucThanhToan()
    {
        List<HinhThucThanhToanModel> httt = new List<HinhThucThanhToanModel>();

        try
        {
            DataTable dt = db.ExecuteDataSet("sp_HinhThucThanhToan_GetAll").Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                HinhThucThanhToanModel item = GetObjectFromDataRowUtil<HinhThucThanhToanModel>.ToOject(dr);
                httt.Add(item);
            }
            return httt;

        }
        catch (Exception ex)
        {
            log.Error(ex);
            return httt;
        }
    }

    public bool ThemHinhThucThanhToan(HinhThucThanhToanModel obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("Name", obj.Name),
                    new SqlParameter("VirtualPaymentClientURL", obj.VirtualPaymentClientURL),
                    new SqlParameter("Version", obj.Version),
                    new SqlParameter("MerchantCode", obj.MerchantCode),
                    new SqlParameter("MerchantName", obj.MerchantName),
                    new SqlParameter("ServiceCode", obj.ServiceCode),
                    new SqlParameter("CountryCode", obj.CountryCode),
                    new SqlParameter("PayType", obj.PayType),
                    new SqlParameter("Ccy", obj.Ccy),
                    new SqlParameter("MasterMerCode", obj.MasterMerCode),
                    new SqlParameter("MerchantType", obj.MerchantType),
                    new SqlParameter("TerminalId", obj.TerminalId),
                    new SqlParameter("TerminalName", obj.TerminalName),
                    new SqlParameter("User", obj.User),
                    new SqlParameter("Password", obj.Password),
                    new SqlParameter("AccessCode", obj.AccessCode),
                    new SqlParameter("Hascode", obj.Hascode),
                    new SqlParameter("Currency", obj.Currency),
                    new SqlParameter("ReturnURL", obj.ReturnURL),
                };
            int i = db.ExecuteNonQuery("sp_HinhThucThanhToan_AddNew", pars);
            if (i > 0)
            {
                ret = true;
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return ret;
    }

    public bool EditHinhThucThanhToan(HinhThucThanhToanModel obj)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("Id", obj.Id),
                    new SqlParameter("Name", obj.Name),
                    new SqlParameter("VirtualPaymentClientURL", obj.VirtualPaymentClientURL),
                    new SqlParameter("Version", obj.Version),
                    new SqlParameter("MerchantCode", obj.MerchantCode),
                    new SqlParameter("MerchantName", obj.MerchantName),
                    new SqlParameter("ServiceCode", obj.ServiceCode),
                    new SqlParameter("CountryCode", obj.CountryCode),
                    new SqlParameter("PayType", obj.PayType),
                    new SqlParameter("Ccy", obj.Ccy),
                    new SqlParameter("MasterMerCode", obj.MasterMerCode),
                    new SqlParameter("MerchantType", obj.MerchantType),
                    new SqlParameter("TerminalId", obj.TerminalId),
                    new SqlParameter("TerminalName", obj.TerminalName),
                    new SqlParameter("User", obj.User),
                    new SqlParameter("Password", obj.Password),
                    new SqlParameter("AccessCode", obj.AccessCode),
                    new SqlParameter("Hascode", obj.Hascode),
                    new SqlParameter("Currency", obj.Currency),
                    new SqlParameter("ReturnURL", obj.ReturnURL),
                };
            int i = db.ExecuteNonQuery("sp_HinhThucThanhToan_Edit", pars);
            if (i > 0)
            {
                ret = true;
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return ret;
    }

    public bool DeleteHinhThucThanhToan(int ID)
    {
        bool ret = false;
        try
        {
            SqlParameter[] pars = new SqlParameter[] {
                    new SqlParameter("@ID", ID)
                };
            int i = db.ExecuteNonQuery("sp_HinhThucThanhToan_Delete", pars);
            if (i > 0)
            {
                ret = true;
            }

        }
        catch (Exception ex)
        {
            log.Error(ex);
        }
        return ret;
    }
}

