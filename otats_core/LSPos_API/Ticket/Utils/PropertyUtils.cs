using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace Ticket.Utils
{
	public class PropertyUtils
	{
		public static int pCustomerID;

		public static string pCustomerCode;

		public static string pCustomerName;

		public static string pCustomerAddress;

		public static string pCustomerTaxCode;

		public static int pMaterialID;

		public static string pMaterialCode;

		public static string pMaterialName;

		public static string pMaterialUnitQty;

		public static string pMaterialSalePrice;

		public static int pProductID;

		public static string pProductCode;

		public static string pProductName;

		public static string pProductUnitQty;

		public static string pProductSalePrice;

		public static int pContractID;

		public static string pContractCode;

		public static string pContractName;

		public static string pstrSearch;

		public static int pDepartmentID;

		public static string pDepartmentCode;

		public static string pDepartmentName;

		public static int pResourceID;

		public static string pResourceCode;

		public static string pResourceName;

		public static int pStockHolderID;

		public static string pStockHolderCode;

		public static string pStockHolderName;

		public static int pStockListID;

		public static string pStockCode;

		public static string pStockName;

		public static int pSaleOrderID;

		public static string pSaleOrderCode;

		public static string pContent;

		public static int pBuyingOrderID;

		public static string pBuyingOrderCode;

		public static string pNote;

		public static int pManufactoryID;

		public static string pManufactoryCode;

		public static string pManufactoryName;

		public static int pEmployeeID;

		public static string pEmployeeCode;

		public static string pLastName;

		public static string pFirstName;

		public static int pSuggestionID;

		public static string pSuggestionCode;

		public static string pSuggestionName;

		public static int pAssetID;

		public static string pAssetCode;

		public static string pAssetName;

		public static int pAssetVoucherID;

		public static string pCurrencyCode;

		public static int pMaintenanceID;

		public static string pMaintenanceCode;

		public static int pTransportLineID;

		public static string pTransportLineCode;

		public static int pTruckID;

		public static string pTruckNo;

		public static int pTruckOwnerID;

		public static string pTruckOwnerCode;

		public static int pVoucherItemID;

		public static string pVoucherItemName;

		public static object getObject(DataRow dr, object model)
		{
			PropertyInfo[] propertiesName = model.GetType().GetProperties();
			for (int i = 0; i < propertiesName.Length; i++)
			{
				object value = dr[propertiesName[i].Name];
				if (value != DBNull.Value)
				{
					propertiesName[i].SetValue(model, value, null);
				}
			}
			return model;
		}

		public static object getObject(string fullname)
		{
			return Activator.CreateInstance(Type.GetType(fullname));
		}

		public static object getObject(DataRow dr, string fullname)
		{
			object model = Activator.CreateInstance(Type.GetType(fullname));
			return getObject(dr, model);
		}

		public static BaseModel getModel(DataRow dr, string name)
		{
			return (BaseModel)getObject(dr, "EzTicket." + name);
		}

		public static object PopulateObject(SqlDataReader dr, object model)
		{
			PropertyInfo[] propertiesName = model.GetType().GetProperties();
			for (int i = 0; i < propertiesName.Length; i++)
			{
				object value = dr[propertiesName[i].Name];
				if (value != DBNull.Value)
				{
					propertiesName[i].SetValue(model, value, null);
				}
			}
			return model;
		}

		public static object PopulateObject(SqlDataReader dr, string fullname)
		{
			object model = Activator.CreateInstance(Type.GetType(fullname));
			return PopulateObject(dr, model);
		}

		public static BaseModel PopulateModel(SqlDataReader dr, string name)
		{
			return (BaseModel)PopulateObject(dr, "EzTicket." + name);
		}

		public static object GetValue(object model, string fieldname)
		{
			PropertyInfo p = model.GetType().GetProperty(fieldname);
			return (p != null) ? p.GetValue(model, null) : null;
		}

		public static string ToList(ArrayList list, string c)
		{
			StringBuilder result = new StringBuilder();
			foreach (object item in list)
			{
				result.Append(item.ToString() + c);
			}
			string r = result.ToString();
			return (r.Length > 1) ? r.Substring(0, result.Length - c.Length) : "";
		}

		public static string ToListWithComma(ArrayList list)
		{
			return ToList(list, ", ");
		}

		public static string ToList(ArrayList list, string fieldname, string c)
		{
			StringBuilder result = new StringBuilder();
			foreach (object item in list)
			{
				result.Append(string.Concat(GetValue(item, fieldname), c));
			}
			string r = result.ToString();
			return (r.Length > 1) ? r.Substring(0, result.Length - c.Length) : "";
		}

		public static string ToListWithComma(ArrayList list, string fieldname)
		{
			return ToList(list, fieldname, ", ");
		}

		public static string ToList1(ArrayList list, string fieldname, string c)
		{
			StringBuilder result = new StringBuilder();
			foreach (object item in list)
			{
				result.Append(string.Concat("'", GetValue(item, fieldname), "'", c));
			}
			string r = result.ToString();
			return (r.Length > 1) ? r.Substring(0, result.Length - c.Length) : "";
		}

		public static string ToListWithComma1(ArrayList list, string fieldname)
		{
			return ToList1(list, fieldname, ", ");
		}
	}
}
