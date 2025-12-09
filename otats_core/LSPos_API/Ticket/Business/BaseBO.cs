using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Ticket.Utils;

namespace Ticket.Business
{
	public class BaseBO
	{
		protected BaseFacade baseFacade = null;

		protected BaseBO()
		{
		}

		public virtual decimal Insert(BaseModel model)
		{
			try
			{
				return baseFacade.Insert(model);
			}
			catch (Exception ex)
			{
				throw new Exception("Can not Insert to database" + ex.Message);
			}
		}

		public virtual void InsertTrans(BaseModel model, SqlConnection sqlConn, SqlTransaction sqlTran)
		{
			try
			{
				baseFacade.Insert(model);
			}
			catch (Exception ex)
			{
				throw new Exception("Can not Insert to database" + ex.Message);
			}
		}

		public virtual void Update(BaseModel model, string field)
		{
			try
			{
				baseFacade.Update(model, field);
			}
			catch (Exception ex)
			{
				throw new Exception("Can not update to database" + ex.Message);
			}
		}

		public virtual void Update(BaseModel model)
		{
			try
			{
				baseFacade.Update(model);
			}
			catch (Exception ex)
			{
				throw new Exception("Can not update to database" + ex.Message);
			}
		}

		public virtual void Delete(int IDValue)
		{
			try
			{
				baseFacade.Delete(IDValue);
			}
			catch (Exception ex)
			{
				throw new Exception("Can not delete from database" + ex.Message);
			}
		}

		public virtual void DeleteByExpression(Expression exp)
		{
			try
			{
				baseFacade.DeleteByExpression(exp);
			}
			catch (Exception ex)
			{
				throw new Exception("Can not delete from database" + ex.Message);
			}
		}

		public virtual void Delete(long IDValue)
		{
			try
			{
				baseFacade.Delete(IDValue);
			}
			catch (Exception ex)
			{
				throw new Exception("Can not delete from database" + ex.Message);
			}
		}

		public virtual void Delete(ArrayList list)
		{
			try
			{
				baseFacade.Delete(list);
			}
			catch (Exception ex)
			{
				throw new Exception("Can not delete from database" + ex.Message);
			}
		}

		public BaseModel FindByPK(long value)
		{
			try
			{
				return baseFacade.FindByPK(value);
			}
			catch (Exception fx)
			{
				throw new Exception("Can not find user " + fx.Message);
			}
		}

		public ArrayList FindByPK(ArrayList list)
		{
			try
			{
				return baseFacade.FindByPK(list);
			}
			catch (Exception fx)
			{
				throw new Exception("Can not find user " + fx.Message);
			}
		}

		public ArrayList FindByPK(string list)
		{
			try
			{
				return baseFacade.FindByPK(list);
			}
			catch (Exception fx)
			{
				throw new Exception("Can not find user " + fx.Message);
			}
		}

		public BaseModel FindByUK(string field, string value)
		{
			return baseFacade.FindByUK(field, value);
		}

		public ArrayList FindByExpression(Expression exp)
		{
			try
			{
				return baseFacade.FindByExpression(exp);
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}

		public ArrayList FindByExpressionWithOrder(Expression exp, string FieldOrder, string Order)
		{
			try
			{
				return baseFacade.FindByExpressionWithOrder(exp, FieldOrder, Order);
			}
			catch (Exception e)
			{
				throw new Exception("Can not execute the FindByExpressionWithOrder method: " + e.Message);
			}
		}

		public ArrayList FindByAttribute(string field, string value)
		{
			try
			{
				return baseFacade.FindByAttr(field, value);
			}
			catch (Exception e)
			{
				throw new Exception("Can not execute the FindByExpression method: " + e.Message);
			}
		}

		public ArrayList FindByAttributeWithOrder(string field, string value, string FieldOrder, string Order)
		{
			try
			{
				return baseFacade.FindByAttrWithOrder(field, value, FieldOrder, Order);
			}
			catch (Exception e)
			{
				throw new Exception("Can not execute the FindByExpression method: " + e.Message);
			}
		}

		public ArrayList FindByAttribute(string field, int value)
		{
			try
			{
				return baseFacade.FindByAttr(field, value);
			}
			catch (Exception e)
			{
				throw new Exception("Can not execute the FindByExpression method: " + e.Message);
			}
		}

		public ArrayList FindByAttributeWithOrder(string field, int value, string FieldOrder, string Order)
		{
			try
			{
				return baseFacade.FindByAttrWithOrder(field, value, FieldOrder, Order);
			}
			catch (Exception e)
			{
				throw new Exception("Can not execute the FindByExpression method: " + e.Message);
			}
		}

		public string GetMax(string field, string field1, int value)
		{
			try
			{
				return baseFacade.FindByMax(field, field1, value.ToString()).ToString();
			}
			catch (Exception e)
			{
				throw new Exception("Can not execute the GetMax method: " + e.Message);
			}
		}

		public string GetMaxRoot(string field)
		{
			try
			{
				return baseFacade.FindByMaxRoot(field).ToString();
			}
			catch (Exception e)
			{
				throw new Exception("Can not execute the GetMax method: " + e.Message);
			}
		}

		public string GetMinRoot(string field)
		{
			try
			{
				return baseFacade.FindByMinRoot(field).ToString();
			}
			catch (Exception e)
			{
				throw new Exception("Can not execute the GetMax method: " + e.Message);
			}
		}

		public ArrayList FindAll()
		{
			try
			{
				return baseFacade.FindAll();
			}
			catch (Exception fx)
			{
				throw new Exception(fx.Message);
			}
		}

		public ArrayList FindByStatus(byte status, string op)
		{
			Expression exp = new Expression("Status", status, op);
			return FindByExpression(exp);
		}

		public virtual void DeleteByAttribute(string name, string value)
		{
			try
			{
				baseFacade.DeleteByAttribute(name, value);
			}
			catch (Exception fx)
			{
				throw new Exception("Can not delete any entity with condition " + name + " = " + value + ": " + fx.Message);
			}
		}

		public virtual void DeleteByAttribute(string name, long value)
		{
			try
			{
				baseFacade.DeleteByAttribute(name, value);
			}
			catch (Exception fx)
			{
				throw new Exception("Can not delete any entity with condition " + name + " = " + value + ": " + fx.Message);
			}
		}

		public Hashtable LazyLoad()
		{
			try
			{
				return baseFacade.LazyLoad();
			}
			catch (Exception fx)
			{
				throw new Exception(fx.Message);
			}
		}

		public Hashtable LazyLoad(string name)
		{
			try
			{
				return baseFacade.LazyLoad(name);
			}
			catch (Exception fx)
			{
				throw new Exception(fx.Message);
			}
		}

		public bool CheckExist(string field, string value)
		{
			try
			{
				return baseFacade.CheckExist(field, value);
			}
			catch (Exception fx)
			{
				throw new Exception(fx.Message);
			}
		}

		public bool CheckExist(string field, long value)
		{
			try
			{
				return baseFacade.CheckExist(field, value);
			}
			catch (Exception fx)
			{
				throw new Exception(fx.Message);
			}
		}

		public string GenerateNo(string code)
		{
			try
			{
				return baseFacade.GenerateNo(code);
			}
			catch (Exception fx)
			{
				throw new Exception(fx.Message);
			}
		}

		public string GenerateNo1(string code, int ClassID)
		{
			try
			{
				return baseFacade.GenerateNo1(code, ClassID);
			}
			catch (Exception fx)
			{
				throw new Exception(fx.Message);
			}
		}

		public string GenerateNo2(string code, int ClassID)
		{
			try
			{
				return baseFacade.GenerateNo2(code, ClassID);
			}
			catch (Exception fx)
			{
				throw new Exception(fx.Message);
			}
		}

		public string Audit(BaseModel obj)
		{
			return baseFacade.Audit(obj);
		}

		public virtual DataTable LoadDataFromSP(string procedureName, string nameSetToTable, string[] paramName, object[] paramValue)
		{
			try
			{
				return baseFacade.LoadDataFromSP(procedureName, nameSetToTable, paramName, paramValue);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public virtual DataTable LoadDataFromSP(string procedureName, string[] paramName, object[] paramValue, int timeout = 30)
		{
			try
			{
				return baseFacade.LoadDataFromSP(procedureName, procedureName, paramName, paramValue, timeout);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public virtual DataTable LoadDataFromSPNotTimeOut(string procedureName, string[] paramName, object[] paramValue, int timeout = 30)
		{
			try
			{
				return baseFacade.LoadDataFromSPNotTimeOut(procedureName, procedureName, paramName, paramValue, timeout);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public string GenerateNo3(string code)
		{
			try
			{
				return baseFacade.GenerateNo3(code);
			}
			catch (Exception fx)
			{
				throw new Exception("Cannot find" + fx.Message);
			}
		}

		public DateTime GetSystemDate()
		{
			return baseFacade.GetSystemDate();
		}
	}
}
