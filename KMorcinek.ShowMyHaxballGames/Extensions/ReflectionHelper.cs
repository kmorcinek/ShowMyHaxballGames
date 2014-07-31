using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace KMorcinek.ShowMyHaxballGames.Extensions
{
    public static class ReflectionHelper
    {
        private static PropertyInfo GetPropertyInfoInternal(LambdaExpression propertyAccessor)
        {
            try
            {
                MemberExpression memberExpression;

                if (propertyAccessor.Body is UnaryExpression)
                {
                    UnaryExpression ue = (UnaryExpression)propertyAccessor.Body;
                    memberExpression = (MemberExpression)ue.Operand;
                }
                else
                {
                    memberExpression = (MemberExpression)propertyAccessor.Body;
                }

                return (PropertyInfo)(memberExpression).Member;
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException(
                    "Cannot extract property from expression. Only expression accessing property (like item=>item.Value) are allowed.", e);
            }
        }

        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object>> property)
        {
            return GetPropertyInfoInternal(property);
        }

        public static PropertyInfo GetPropertyInfo<T>(T @object, Expression<Func<T, object>> property)
        {
            return GetPropertyInfoInternal(property);
        }


        public static string GetPropertyName<T>(Expression<Func<T, object>> property)
        {
            return GetPropertyInfoInternal(property).Name;
        }

        public static string GetPropertyName<T>(T @object, Expression<Func<T, object>> property)
        {
            return GetPropertyInfoInternal(property).Name;
        }

        public static T GetCustomAttribute<T>(Type t, bool inherit) where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(t, typeof(T), inherit);
        }
    }
}