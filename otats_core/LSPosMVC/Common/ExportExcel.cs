using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using Aspose.Cells;
using System.Web.UI;
using System.ComponentModel;

namespace LSPosMVC.Common
{
    public class ExportExcel
    {
        public class ExportImageDTO
        {
            public byte[] url { get; set; }

        }
        public static byte[] GetBytesFromFileImage(string fullFilePath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)
            FileStream fs = File.OpenRead(fullFilePath);
            try
            {
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                return bytes;
            }
            finally
            {
                fs.Close();
            }
        }
        public Page Page { get; private set; }

        public bool ExportTemplateReport(string sReportFileName, string ReportName, DataSet dsData, DataTable dtVariable, ref MemoryStream fileStream)
        {
            string filePath;
            string templatefolder;

            WorkbookDesigner designer;
            try
            {
                templatefolder = System.Web.Configuration.WebConfigurationManager.AppSettings["ReportTemplatesFolder"];
                filePath = AppDomain.CurrentDomain.BaseDirectory + templatefolder + @"\" + sReportFileName;

                if (!File.Exists(filePath))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "javascriptfunction", "goBack()", true);
                    return false;
                }
                Workbook wb = new Workbook(filePath);
                designer = new WorkbookDesigner(wb);
                designer.SetDataSource(dsData);
                if (dtVariable != null)
                {
                    int intCols = dtVariable.Columns.Count;
                    for (int i = 0; i <= intCols - 1; i++)
                        designer.SetDataSource(dtVariable.Columns[i].ColumnName.ToString(), dtVariable.Rows[0].ItemArray[i].ToString());
                }
                string teamplate = ReportName + DateTime.Now.ToString("yyyyMMdd-HHMMss") + ".xls";
                designer.Process();
                designer.Workbook.CalculateFormula();
                fileStream = designer.Workbook.SaveToStream();
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
            return true;
        }

        public bool ExportTemplate(string sReportFileName, DataSet dsData, DataTable dtVariable, ref string filename)
        {
            string filePath;
            string templatefolder;

            WorkbookDesigner designer;
            try
            {
                templatefolder = System.Web.Configuration.WebConfigurationManager.AppSettings["ReportTemplatesFolder"];
                filePath = AppDomain.CurrentDomain.BaseDirectory + templatefolder + @"\" + sReportFileName;

                if (!File.Exists(filePath))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "javascriptfunction", "goBack()", true);
                    return false;
                }

                Workbook wb = new Workbook(filePath);
                designer = new WorkbookDesigner(wb);
                designer.SetDataSource(dsData);
                if (dtVariable != null)
                {
                    int intCols = dtVariable.Columns.Count;
                    for (int i = 0; i <= intCols - 1; i++)
                        designer.SetDataSource(dtVariable.Columns[i].ColumnName.ToString(), dtVariable.Rows[0].ItemArray[i].ToString());
                }
                string fileNameNV = "TemplateImport";
                string teamplate = fileNameNV + DateTime.Now.ToString("yyyyMMdd-HHMMss") + ".xls";
                designer.Process();
                designer.Workbook.CalculateFormula();
                designer.Workbook.Save(HttpContext.Current.Response, teamplate, ContentDisposition.Attachment, new XlsSaveOptions());
                filename = teamplate;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
            return true;
        }

        public bool ExportTemplateTable(string sReportFileName, DataTable dtData, DataTable dtVariable, ref string filename)
        {
            string filePath;
            string templatefolder;
            WorkbookDesigner designer;
            try
            {
                templatefolder = System.Web.Configuration.WebConfigurationManager.AppSettings["ReportTemplatesFolder"];
                filePath = AppDomain.CurrentDomain.BaseDirectory + templatefolder + @"\" + sReportFileName;

                if (!File.Exists(filePath))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "javascriptfunction", "goBack()", true);
                    return false;
                }
                Workbook wb = new Workbook(filePath);
                designer = new WorkbookDesigner(wb);
                designer.SetDataSource(dtData);
                if (dtVariable != null)
                {
                    int intCols = dtVariable.Columns.Count;
                    for (int i = 0; (i <= (intCols - 1)); i++)
                    {
                        designer.SetDataSource(dtVariable.Columns[i].ColumnName.ToString(), dtVariable.Rows[0].ItemArray[i].ToString());
                    }

                }
                string fileNameNV = "TemplateError";
                string teamplate = fileNameNV + DateTime.Now.ToString("yyyyMMdd-HHMMss") + ".xls";
                designer.Process();
                designer.Workbook.CalculateFormula();
                designer.Workbook.Save(HttpContext.Current.Response, teamplate, ContentDisposition.Attachment, new XlsSaveOptions());
                filename = teamplate;
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }

            return true;
        }

        public bool ExportTemplateToTeamplateImportStreamGird(string sReportFileName, DataSet dsData, DataTable dtVariable, ref MemoryStream stream)
        {
            string filePath;
            string templatefolder;

            WorkbookDesigner designer;
            try
            {
                templatefolder = System.Web.Configuration.WebConfigurationManager.AppSettings["ReportTemplatesFolder"];
                filePath = AppDomain.CurrentDomain.BaseDirectory + templatefolder + @"\" + sReportFileName;

                if (!File.Exists(filePath))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "javascriptfunction", "goBack()", true);
                    return false;
                }
                Workbook wb = new Workbook(filePath);
                designer = new WorkbookDesigner(wb);
                designer.SetDataSource(dsData);
               
                if (dtVariable != null)
                {
                    int intCols = dtVariable.Columns.Count;
                    for (int i = 0; i <= intCols - 1; i++)
                        designer.SetDataSource(dtVariable.Columns[i].ColumnName.ToString(), dtVariable.Rows[0].ItemArray[i].ToString());
                }
                designer.Process();
                designer.Workbook.CalculateFormula();
                stream = designer.Workbook.SaveToStream();
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
            return true;
        }

        public bool ExportTemplateToStreamGird(string sReportFileName, DataSet dsData, DataTable dtVariable, ref MemoryStream stream)
        {
            string filePath;
            string templatefolder;

            WorkbookDesigner designer;
            try
            {
                templatefolder = System.Web.Configuration.WebConfigurationManager.AppSettings["ReportTemplatesFolder"];
                filePath = AppDomain.CurrentDomain.BaseDirectory + templatefolder + @"\" + sReportFileName;

                if (!File.Exists(filePath))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "javascriptfunction", "goBack()", true);

                    return false;
                }

                Workbook wb = new Workbook(filePath);
                designer = new WorkbookDesigner(wb);
                designer.SetDataSource(dsData);
                Aspose.Cells.Worksheet wSheet = designer.Workbook.Worksheets[0];
                ExportImageDTO image = new ExportImageDTO();
                Common cm = new Common();
                
                if (File.Exists(System.Web.HttpContext.Current.Server.MapPath((dsData.Tables[2].Rows[0]["url_Image"].ToString()))))
                {
                    image.url = GetBytesFromFileImage(System.Web.HttpContext.Current.Server.MapPath(dsData.Tables[1].Rows[0]["url_Image"].ToString()));
                }
                else
                {
                    if (dsData.Tables[2].Rows[0]["url_Image"].ToString() =="" && dsData.Tables[2].Rows[0]["TenCongTy"].ToString() == "")
                    {
                        wSheet.Cells.HideRow(2);
                        wSheet.Cells.HideRow(1);
                        wSheet.Cells.HideRow(0);
                    }

                }
                if (image.url != null)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(image.url);
                    int pictureIndex = wSheet.Pictures.Add(1, 2, ms);
                    Aspose.Cells.Drawing.Picture picture = wSheet.Pictures[pictureIndex];
                    picture.Width = 180;
                    picture.Height = 136;
                }

                if (dtVariable != null)
                {
                    int intCols = dtVariable.Columns.Count;
                    for (int i = 0; i <= intCols - 1; i++)
                        designer.SetDataSource(dtVariable.Columns[i].ColumnName.ToString(), dtVariable.Rows[0].ItemArray[i].ToString());
                }
                
                designer.Process();
                designer.Workbook.CalculateFormula();
                stream = designer.Workbook.SaveToStream();
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
            return true;
        }

        public bool ExportTemplateToStreamGirdDonHangChiTiet(string sReportFileName, DataSet dsData, DataTable dtVariable, ref MemoryStream stream)
        {
            string filePath;
            string templatefolder;

            WorkbookDesigner designer;
            try
            {
                templatefolder = System.Web.Configuration.WebConfigurationManager.AppSettings["ReportTemplatesFolder"];
                filePath = AppDomain.CurrentDomain.BaseDirectory + templatefolder + @"\" + sReportFileName;

                if (!File.Exists(filePath))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "javascriptfunction", "goBack()", true);

                    return false;
                }

                Workbook wb = new Workbook(filePath);
                designer = new WorkbookDesigner(wb);
                designer.SetDataSource(dsData);
                Aspose.Cells.Worksheet wSheet = designer.Workbook.Worksheets[0];
                ExportImageDTO image = new ExportImageDTO();
                Common cm = new Common();

                if (File.Exists(System.Web.HttpContext.Current.Server.MapPath((dsData.Tables[4].Rows[0]["url_Image"].ToString()))))
                {
                    image.url = GetBytesFromFileImage(System.Web.HttpContext.Current.Server.MapPath(dsData.Tables[1].Rows[0]["url_Image"].ToString()));
                }
                else
                {
                    if (dsData.Tables[1].Rows[0]["url_Image"].ToString() != "" && dsData.Tables[4].Rows[0]["TenCongTy"].ToString() != "")
                    {
                        wSheet.Cells.HideRow(2);
                        wSheet.Cells.HideRow(1);
                        wSheet.Cells.HideRow(0);
                    }

                }
                if (image.url != null)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(image.url);
                    int pictureIndex = wSheet.Pictures.Add(1, 2, ms);
                    Aspose.Cells.Drawing.Picture picture = wSheet.Pictures[pictureIndex];
                    picture.Width = 180;
                    picture.Height = 136;
                }

                if (dtVariable != null)
                {
                    int intCols = dtVariable.Columns.Count;
                    for (int i = 0; i <= intCols - 1; i++)
                        designer.SetDataSource(dtVariable.Columns[i].ColumnName.ToString(), dtVariable.Rows[0].ItemArray[i].ToString());
                }
                designer.Process();
                designer.Workbook.CalculateFormula();
                stream = designer.Workbook.SaveToStream();
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }
            return true;
        }

        public bool ExportTemplateToStream(string sReportFileName, DataTable dtData, DataTable dtVariable, ref MemoryStream stream)
        {
            string filePath;
            string templatefolder;
            WorkbookDesigner designer;
            try
            {
                templatefolder = System.Web.Configuration.WebConfigurationManager.AppSettings["ReportTemplatesFolder"];
                filePath = AppDomain.CurrentDomain.BaseDirectory + templatefolder + @"\" + sReportFileName;

                if (!File.Exists(filePath))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "javascriptfunction", "goBack()", true);
                    return false;
                }
                Workbook wb = new Workbook(filePath);
                designer = new WorkbookDesigner(wb);
                designer.SetDataSource(dtData);
                if (dtVariable != null)
                {
                    int intCols = dtVariable.Columns.Count;
                    for (int i = 0; (i <= (intCols - 1)); i++)
                    {
                        designer.SetDataSource(dtVariable.Columns[i].ColumnName.ToString(), dtVariable.Rows[0].ItemArray[i].ToString());
                    }
                }
                string fileNameNV = "TemplateError";
                string teamplate = fileNameNV + DateTime.Now.ToString("yyyyMMdd-HHMMss") + ".xls";
                designer.Process();
                designer.Workbook.CalculateFormula();
                stream = designer.Workbook.SaveToStream();
            }
            catch (Exception ex)
            {
                LSPos_Data.Utilities.Log.Error(ex);
                return false;
            }

            return true;
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
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

    }
}