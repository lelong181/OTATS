using System;
using System.Collections;
using System.Reflection;
using System.Text;
using Ticket.Utils;

namespace Ticket
{
	public class BaseModel : ICloneable
	{
		public int userID = 0;

		protected int auditType = -100;

		public object Clone()
		{
			object newObject = Activator.CreateInstance(GetType());
			FieldInfo[] fields = newObject.GetType().GetFields();
			int i = 0;
			FieldInfo[] fields2 = GetType().GetFields();
			foreach (FieldInfo fi in fields2)
			{
				Type ICloneType = fi.FieldType.GetInterface("ICloneable", ignoreCase: true);
				if (ICloneType != null)
				{
					ICloneable IClone = (ICloneable)fi.GetValue(this);
					if (IClone == null)
					{
						continue;
					}
					fields[i].SetValue(newObject, IClone.Clone());
				}
				else
				{
					fields[i].SetValue(newObject, fi.GetValue(this));
				}
				Type IEnumerableType = fi.FieldType.GetInterface("IEnumerable", ignoreCase: true);
				if (IEnumerableType != null)
				{
					IEnumerable IEnum = (IEnumerable)fi.GetValue(this);
					Type IListType = fields[i].FieldType.GetInterface("IList", ignoreCase: true);
					Type IDicType = fields[i].FieldType.GetInterface("IDictionary", ignoreCase: true);
					int j = 0;
					if (IListType != null)
					{
						IList list = (IList)fields[i].GetValue(newObject);
						foreach (object obj in IEnum)
						{
							ICloneType = obj.GetType().GetInterface("ICloneable", ignoreCase: true);
							if (ICloneType != null)
							{
								ICloneable clone2 = (ICloneable)obj;
								list[j] = clone2.Clone();
							}
							j++;
						}
					}
					else if (IDicType != null)
					{
						IDictionary dic = (IDictionary)fields[i].GetValue(newObject);
						j = 0;
						foreach (DictionaryEntry de in IEnum)
						{
							ICloneType = de.Value.GetType().GetInterface("ICloneable", ignoreCase: true);
							if (ICloneType != null)
							{
								ICloneable clone = (ICloneable)de.Value;
								dic[de.Key] = clone.Clone();
							}
							j++;
						}
					}
				}
				i++;
			}
			return newObject;
		}

		public StringBuilder ToXML()
		{
			PropertyInfo[] propertiesName = GetType().GetProperties();
			string name = GetType().Name;
			name = name.Substring(0, name.Length - 5);
			StringBuilder xml = new StringBuilder();
			xml.Append(string.Concat("<record id='", GetType().GetProperty(name + "iD").GetValue(this, null), "'>"));
			for (int i = 0; i < propertiesName.Length; i++)
			{
				xml.Append(string.Concat("<", propertiesName[i].Name, ">", propertiesName[i].GetValue(this, null), "</", propertiesName[i].Name, ">"));
			}
			return xml.Append("</record>");
		}

		public virtual string GetOrderBy(string FieldName, string OrderBy)
		{
			return "Order by " + FieldName + " " + OrderBy;
		}

		public virtual string[] GetAuditFields()
		{
			PropertyInfo[] list = GetType().GetProperties();
			string[] r = new string[list.Length];
			for (int i = 0; i < list.Length; i++)
			{
				r[i] = list[i].Name;
			}
			return r;
		}

		public virtual string CompareTo(BaseModel model)
		{
			StringBuilder result = new StringBuilder();
			string modelName = GetType().Name;
			string fID = modelName.Substring(0, modelName.Length - 5) + "iD";
			string[] fields = GetAuditFields();
			if (model == null)
			{
				string[] array = fields;
				foreach (string field2 in array)
				{
					if (!field2.Equals(fID))
					{
						object value2 = PropertyUtils.GetValue(this, field2);
						result.Append($"- {field2}: {value2}<br>");
					}
				}
			}
			else
			{
				if (GetType() != model.GetType())
				{
					return "None";
				}
				string[] array2 = fields;
				foreach (string field in array2)
				{
					if (!field.Equals(fID))
					{
						object value1 = PropertyUtils.GetValue(this, field);
						object value3 = PropertyUtils.GetValue(model, field);
						if (!value1.Equals(value3))
						{
							result.Append($"- {field}: {value3} -> {value1}<br>");
						}
					}
				}
			}
			return (result.Length > 0) ? result.ToString() : "None";
		}

		public void SetUserID(int id)
		{
			userID = id;
		}

		public int GetUserID()
		{
			return userID;
		}

		public void SetAuditType(int id)
		{
			auditType = id;
		}

		public virtual int GetAuditType()
		{
			return auditType;
		}
	}
}
