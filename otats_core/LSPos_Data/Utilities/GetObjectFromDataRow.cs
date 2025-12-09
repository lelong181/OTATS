using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;


public static class GetObjectFromDataRowUtil<T> where T : new()
{

    public static T ToOject(DataRow dr)
    {
        T obj = new T();
        foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
        {
            if (dr.Table.Columns.IndexOf(propertyInfo.Name) >= 0)
            {

                if (!string.IsNullOrWhiteSpace(dr[propertyInfo.Name].ToString()))
                {
                    var t = propertyInfo.PropertyType;

                    if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    {
                        if (string.IsNullOrWhiteSpace(dr[propertyInfo.Name].ToString()))
                        {
                            propertyInfo.SetValue(obj, null);
                        }
                        else
                        {
                            t = Nullable.GetUnderlyingType(t);
                            var value = Convert.ChangeType(dr[propertyInfo.Name], t);
                            propertyInfo.SetValue(obj, value);
                        }
                    }
                    else
                    {
                        var value = Convert.ChangeType(dr[propertyInfo.Name], propertyInfo.PropertyType);
                        propertyInfo.SetValue(obj, value);
                    }
                }
                else
                {
                    propertyInfo.SetValue(obj, null);
                }
            }
            else
            {
                propertyInfo.SetValue(obj, null);
            }
        }
        //return obj;
        return (T)Convert.ChangeType(obj, typeof(T));
    }
}
