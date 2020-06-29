using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;

namespace FilmApi.Utils
{
    public static class LinqExtensions 
    {
        private static PropertyInfo GetPropertyInfo(Type type, string name) 
        {
            var properties = type.GetProperties();
            var matchedProperty = properties.FirstOrDefault(p => p.Name == name);
            if (matchedProperty == null)
                throw new ArgumentException("name");
            return matchedProperty;
        }
        private static LambdaExpression GetOrderExpression(Type type, PropertyInfo info) 
        {
            var paramExpression = Expression.Parameter(type);
            var propAcces = Expression.PropertyOrField(paramExpression, info.Name);
            var expression = Expression.Lambda(propAcces, paramExpression);
            return expression;
        }
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> query, string name) 
        {
            var propInfo = GetPropertyInfo(typeof(T), name);
            var propAccess = GetOrderExpression(typeof(T), propInfo);
            var method = typeof(Enumerable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IEnumerable<T>)genericMethod.Invoke(null, new object[] { query, propAccess.Compile() })!;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string name)
        {
            var propInfo = GetPropertyInfo(typeof(T), name);
            var access = GetOrderExpression(typeof(T), propInfo);

            var method = typeof(Queryable).GetMethods().FirstOrDefault(m => m.Name == "OrderBy" && m.GetParameters().Length == 2);
            var genericMethod = method.MakeGenericMethod(typeof(T), propInfo.PropertyType);
            return (IQueryable<T>)genericMethod.Invoke(null, new object[] { query, access })!;
        }

        public static IEnumerable<T> OrderBy<T> (this IEnumerable<T> query, string [] names) 
        {
            foreach (var name in names)
            {
                query = query.OrderBy(name);
            }
            return query;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string[] names) 
        {
            foreach (var name in names) 
            {
                query = query.OrderBy(name);
            }
            return query;
        }
    }
}