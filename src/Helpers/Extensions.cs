using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Climax
{
	public static class TypedDefaultExtensions
	{
		public static IEnumerable<Type> GetDecoratedTypes<T>(this Assembly assembly)
			where T : class
		{
			return assembly.GetTypes()
								.Where(c => c.IsNested == false &&
									  c.HasCustomAttribute<T>());
		}
		public static IEnumerable<PropertyInfo> GetDecoratedStaticProperties<T>(
			this Assembly assembly, string type) where T : class
		{
			return assembly.GetTypes()
								.First(c => c.Name == type)
								.GetProperties(BindingFlags.Static | BindingFlags.Public)
								.WhereAttribute<T>();
		}
		public static bool HasCustomAttribute<T>(this Type type) =>
			type.GetCustomAttributes(typeof(T), false).Length > 0;
		public static bool HasCustomAttribute<T>(this PropertyInfo property) =>
			property.GetCustomAttributes(typeof(T), false).Length > 0;
		public static bool HasCustomAttribute<T>(this MethodInfo method) =>
			method.GetCustomAttributes(typeof(T), false).Length > 0;


		public static IEnumerable<PropertyInfo> GetDecoratedStaticProperties<TType,TAttribute>(
			this Assembly assembly) 
			where TType : class 
			where TAttribute:class
		{
			var ret = new List<PropertyInfo>();

			foreach (var type in assembly.GetTypes().WhereAttribute<TType>())
				foreach (var prop in type.GetProperties(BindingFlags.Static | BindingFlags.Public)
							.WhereAttribute<TAttribute>())
					ret.Add(prop);

			return ret;
		}
		public static IEnumerable<Type> WhereAttribute<T>(this Type[] types) =>
			types.Where(c => c.GetCustomAttributes(typeof(T), false).Length > 0);
		public static IEnumerable<PropertyInfo> WhereAttribute<T>(this PropertyInfo[] types) =>
			types.Where(c => c.GetCustomAttributes(typeof(T), false).Length > 0);
		public static T GetCustomAttribute<T>(this ICustomAttributeProvider provider) where T : class
		{
			var att = provider.GetCustomAttributes(typeof(T), false);
			return att != null && att.Length > 0 ? att[0] as T : null;
		}

		public static bool CustomAttributeIsDefined<T>(this MethodInfo method) where T : class
		{
			var att = method.GetCustomAttributes(typeof(T), false);
			return att != null && att.Length > 0 ? true : false;
		}
		public static object ToDefault(this Type targetType)
		{
			if (targetType == null)
				throw new NullReferenceException();

			var mi = typeof(TypedDefaultExtensions)
				 .GetMethod("_ToDefaultHelper", BindingFlags.Static | BindingFlags.NonPublic);

			var generic = mi.MakeGenericMethod(targetType);

			var returnValue = generic.Invoke(null, new object[0]);
			return returnValue;
		}

		static T _ToDefaultHelper<T>()
		{
			return default(T);
		}
		public static T GetTfromString<T>(this string mystring)
		{
			var foo = TypeDescriptor.GetConverter(typeof(T));
			return (T)(foo.ConvertFromInvariantString(mystring));
		}
	}
}
