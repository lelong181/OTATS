using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

namespace LSPosMVC.Common
{

    public class ImportValidate
    {
        public static object Information { get; private set; }

        public static bool TrimRow(ref DataRow row, bool isClearNA = false)
        {
            string value;
            string colName;
            bool isRow = false;
            try
            {
                DataTable dtData = row.Table;
                foreach (DataColumn col in dtData.Columns)
                {
                    colName = col.ColumnName;
                    value = row[colName].ToString().Trim();
                    if (isClearNA)
                    {
                        if (value == "#N/A")
                            value = "";
                        row[colName] = value;
                        if (value != "")
                            isRow = true;
                    }
                    else
                    {
                        row[colName] = value;
                        if (value != "#N/A" & value != "")
                            isRow = true;
                    }
                }
                return isRow;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EmptyValue(string colName, ref DataRow row, ref DataRow rowError, ref bool isError, string sError)
        {
            string value;
            try
            {
                value = row[colName].ToString().Trim();
                row[colName] = value;
                if (Validator.EmptyValue(value))
                {
                    rowError[colName] = sError;
                    isError = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void IsValidList(string colText, string colNumber, ref DataRow row, ref DataRow rowError, ref bool isError, string sError, string lang = "vi")
        {
            string value;
            try
            {
                EmptyValue(colText, ref row, ref rowError, ref isError, sError);
                if (rowError[colText].ToString() != "")
                {
                    rowError[colText] = sError + ((lang == "vi") ? " chưa nhập" : " no input");
                    return;
                }

                value = row[colNumber].ToString().Trim().Replace(",", "");
                row[colNumber] = value;
                if (!Validator.IsNumeric(value))
                {
                    rowError[colText] += sError + ((lang == "vi") ? " sai định dạng" : " invalid format");
                    isError = true;
                }
                else if (value.Length > 30)
                {
                    rowError[colNumber] += sError + ((lang == "vi") ? " độ dài số không được vượt quá 30 ký tự" : " number length cannot exceed 30 characters");
                    isError = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void IsValidNumber(string colName, ref DataRow row, ref DataRow rowError, ref bool isError, string sError, bool isNegative, bool False, string lang = "vi")
        {
            string value;
            // Warning!!! Optional parameters not supported
            try
            {
                value = row[colName].ToString().Trim().Replace(",", "");
                row[colName] = value;
                if (!Validator.IsNumeric(value))
                {
                    rowError[colName] = (rowError[colName] + sError);
                    isError = true;
                }
                else if ((value.Length > 30))
                {
                    rowError[colName] += ((lang == "vi") ? "Độ dài số không được vượt quá 30 ký tự" : "Number length cannot exceed 30 characters");
                    isError = true;
                }
                else if ((isNegative && (double.Parse(value) < 0)))
                {
                    rowError[colName] += ((lang == "vi") ? "Không được phép nhập số âm" : "Negative value is not allowed");
                    isError = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void IsValidDate(string colName, ref DataRow row, ref DataRow rowError, ref bool isError, string sError, string lang)
        {
            string value;
            try
            {
                value = row[colName].ToString().Trim();
                row[colName] = value;
                if (Validator.EmptyValue(value))
                {
                    rowError[colName] = (rowError[colName] + sError);
                    isError = true;
                    return;
                }

                if (!Validator.ValidateDate(value, ref sError, lang))
                {
                    rowError[colName] = (rowError[colName] + sError);
                    isError = true;
                    return;
                }

                row[colName] = Validator.FormatDate(value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void IsValidEmail(string colName, ref DataRow row, ref DataRow rowError, ref bool isError, string sError)
        {
            string value;
            try
            {
                value = row[colName].ToString().Trim();
                row[colName] = value;
                if (!Validator.IsValidEmail(value))
                {
                    rowError[colName] = (rowError[colName] + sError);
                    isError = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}

static class Validator
{
    public static bool IsNumeric(string s)
    {
        float output;
        return float.TryParse(s, out output);
    }

    public static string FormatDate(string str)
    {
        object day = str.Split('/')[0];
        object month = str.Split('/')[1];
        object year = str.Split('/')[2];
        DateTime date = new DateTime(int.Parse(year.ToString()), int.Parse(month.ToString()), int.Parse(day.ToString()));
        return String.Format("{0:dd/MM/yyyy}", date);
        //return string.Format(new DateTime(int.Parse(year.ToString()), int.Parse(month.ToString()), int.Parse(day.ToString())), "dd/MMM/yyyy");
        //return String.Format(new DateTime(Int32.Parse(year.ToString()), int.Parse(month.ToString()), int.Parse(day.ToString())), "dd/MMM/yyyy");
    }

    public static bool ValidateDate(string str, ref string _error, string lang = "vi")
    {
        if ((str.Split('/').Length != 3))
        {
            _error = (lang == "vi") ? "Sai định dạng: dd/MM/yyyy" : "Invalid format: dd/MM/yyyy";
            // Warning!!! Optional parameters not supported
            return false;
        }

        object day = str.Split('/')[0];
        object month = str.Split('/')[1];
        object year = str.Split('/')[2];
        if ((!IsNumeric(day.ToString()) || (!IsNumeric(month.ToString()) || !IsNumeric(year.ToString()))))
        {
            _error = (lang == "vi") ? "Sai định dạng: dd/MM/yyyy" : "Invalid format: dd/MM/yyyy";
            return false;
        }

        if ((int.Parse(year.ToString()) < 1900 || (int.Parse(year.ToString()) > 2999)))
        {
            _error = (lang == "vi") ? "Năm không được nhỏ hơn 1900 và lớn hơn 2999" : "Neither can year be sooner than 1990 nor later than 2999";
            return false;
        }

        if ((int.Parse(month.ToString()) < 1 || (int.Parse(month.ToString()) > 12)))
        {
            _error = (lang == "vi") ? "Tháng không được nhỏ hơn 1 và lớn hơn 12" : "Neither can month be smaller than 1 nor larger than 12";
            return false;
        }

        if (int.Parse(day.ToString()) < 1 || int.Parse(day.ToString()) > System.DateTime.DaysInMonth(int.Parse(year.ToString()), int.Parse(month.ToString())))
        {
            _error = (lang == "vi") ? "Ngày không được nhỏ hơn 1 và lớn hơn số ngày trong tháng" : "Neither can day be smaller than 1 nor larger than the number of days in the month";
            return false;
        }

        return true;
    }

    public static bool EmptyValue(string s)
    {
        // return true if s is null string
        return (s.Trim() == null || s.Trim().Length == 0);
    }

    public static bool IsValidEmail(string strIn)
    {
        if (!EmptyValue(strIn))
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        else
            return true;
    }

    public static bool IsAlphaNumeric(string s)
    {
        if (!EmptyValue(s))
        {
            char[] chars = s.ToCharArray();
            foreach (char c in chars)
            {
                if (Regex.IsMatch(c.ToString(), "^[-_,A-Za-z0-9]$"))
                    return true;
            }
            return false;
        }
        else
            return true;
    }
    public static bool IsNumeric2(string s)
    {
        if (!EmptyValue(s))
        {
            char[] chars = s.ToCharArray();
            foreach (char c in chars)
            {
                if (Regex.IsMatch(c.ToString(), "^[0-9]*$"))
                    return false;
            }
            return true;
        }
        else
            return false;
    }

    //public static bool mf_KiemtraURL(string sv_Email)
    //{
    //    try
    //    {
    //        if (mf_blength(sv_Email) == false)
    //            return true;
    //        if (mf_Kiemtrakytudacbiet(sv_Email, "-,@,.,_"))
    //            return false;

    //        int sl = 0;

    //        sl = 0;
    //        for (int i = 0; i <= sv_Email.Length - 1; i++)
    //        {
    //            if (sv_Email.Substring(i, 1) == ".")
    //                sl += 1;
    //        }

    //        if (sl > 2)
    //            return false;

    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //public static bool mf_Kiemtrakytudacbiet(string _sKytu, string sv_ExceptionCharacter = "")
    //{
    //    try
    //    {
    //        if (_sKytu.IndexOf("@", 0) >= 0 | _sKytu.IndexOf("#", 0) >= 0 | _sKytu.IndexOf("$", 0) >= 0 | _sKytu.IndexOf("%", 0) >= 0 | _sKytu.IndexOf("!", 0) >= 0 | _sKytu.IndexOf("^", 0) >= 0 | _sKytu.IndexOf("&", 0) >= 0 | _sKytu.IndexOf("*", 0) >= 0 | _sKytu.IndexOf("(", 0) >= 0 | _sKytu.IndexOf(")", 0) >= 0 | _sKytu.IndexOf("+", 0) >= 0 | _sKytu.IndexOf("=", 0) >= 0 | _sKytu.IndexOf("<", 0) >= 0 | _sKytu.IndexOf(">", 0) >= 0)
    //        {
    //            if (mf_ObjectIsNull(sv_ExceptionCharacter) == false)
    //            {
    //                string sv_listKytudacbiet = @"!,@,#,$,%,^,&,*,(,),=,+,{,},[,],|,\,',/,?,>,<";
    //                string[] _sv_sValues = sv_listKytudacbiet.Split(",");
    //                sv_listKytudacbiet = "";
    //                for (int kk = 0; kk <= _sv_sValues.Length - 1; kk++)
    //                {
    //                    if (sv_ExceptionCharacter.IndexOf(mf_ObjectToString(_sv_sValues[kk]), 0) < 0)
    //                        sv_listKytudacbiet += mf_ObjectToString(_sv_sValues[kk]) + ",";
    //                }
    //                sv_listKytudacbiet = sv_listKytudacbiet.Trim(",");
    //                _sv_sValues = sv_listKytudacbiet.Split(",");
    //                for (int ii = 0; ii <= _sv_sValues.Length - 1; ii++)
    //                {
    //                    if (_sKytu.IndexOf(mf_ObjectToString(_sv_sValues[ii]), 0) >= 0)
    //                    {
    //                        return true;
    //                        break;
    //                    }
    //                }
    //            }
    //            else
    //                return true;
    //        }
    //        else
    //            return false;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    //public static bool mf_blength(object obj)
    //{
    //    try
    //    {
    //        bool flag = false;
    //        if (obj == null)
    //            return false;
    //        if (obj is DataTable)
    //        {
    //            if ((DataTable)obj.Rows.Count > 0)
    //                flag = true;
    //        }
    //        else if (obj is DataRow[])
    //        {
    //            if ((DataRow[])obj.Length > 0)
    //                flag = true;
    //        }
    //        else if (obj is DataSet)
    //        {
    //            if ((DataSet)obj != null)
    //            {
    //                if ((DataSet)obj.Tables[0].Rows.Count > 0)
    //                    flag = true;
    //            }
    //        }
    //        else if (obj is DataRow)
    //        {
    //            if ((DataRow)obj != null)
    //                flag = true;
    //        }
    //        else if ((mf_ObjectToString(obj).Length > 0))
    //            flag = true;
    //        return flag;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    public static string mf_ObjectToString(object obj)
    {
        try
        {
            if (obj == null)
                return "";
            return obj.ToString().Trim();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //public static bool mf_ObjectIsNull(object obj)
    //{
    //    try
    //    {
    //        if (obj == null)
    //            return true;
    //        else if (mf_blength(obj) == false)
    //            return true;
    //        else
    //            return false;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
}
