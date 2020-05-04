using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using SqlKata;
using SqlKata.Compilers;
using SqlKata.Execution;


namespace Program
{
    public  static class MyModule
    {
        internal static class QueryHelper
        {
            internal static XQuery CastToXQuery(Query query, string method = null)
            {
                var xQuery = query as XQuery;

                if (xQuery is null)
                {
                    if (method == null)
                    {
                        throw new InvalidOperationException($"Execution methods can only be used with `XQuery` instances, consider using the `QueryFactory.Query()` to create executable queries, check https://sqlkata.com/docs/execution/setup#xquery-class for more info");
                    }
                    else
                    {
                        throw new InvalidOperationException($"The method ${method} can only be used with `XQuery` instances, consider using the `QueryFactory.Query()` to create executable queries, check https://sqlkata.com/docs/execution/setup#xquery-class for more info");
                    }
                }

                return xQuery;

            }

            internal static QueryFactory CreateQueryFactory(Query query)
            {
                var xQuery = CastToXQuery(query);

                var factory = new QueryFactory(xQuery.Connection, xQuery.Compiler);

                factory.Logger = xQuery.Logger;

                return factory;
            }
        }
        public static T ConvertToObj<T>(dynamic result)
        {

            Type t = typeof(T);
            var fullname = t.FullName;
            IDictionary<string, object> dic = result;
            Assembly asm = Assembly.GetExecutingAssembly();
            Object obj = asm.CreateInstance(fullname, true);
            Type objType = obj.GetType();
            foreach (var k in dic)
            {
                PropertyInfo info = null;
                info = objType.GetProperty(k.Key);
                info.SetValue(obj, dic[k.Key]);
            }
           
            return (T)obj;
        }

        public static List<T> MyGet<T>(this Query query)
        {
            List<T> temp = new List<T>();
            var result = query.Get<dynamic>();
            
            foreach (var r in result)
            {
                var obj =(T)ConvertToObj<T>(r);
                temp.Add(obj);
            }
            return temp;
        }

        public static T FirstOrDefault<T>(this Query query)
        {
            return QueryHelper.CreateQueryFactory(query).FirstOrDefault<T>(query);
        }

        public static T MyFirstOrDefault<T>(this Query query)
        {
            var result =  FirstOrDefault<dynamic>(query);
            var obj = (T)ConvertToObj<T>(result);
            return obj;
        }


        

    }
}
