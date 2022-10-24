using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Climax.UnitTest
{
	public enum StackItemType { Method, Class, Parameter, Property, Initializer }
	public static class ExecutionStack
	{
		public static List<StackItem> ExecutionItems { get; private set; }
		public static void Initialize() =>
			ExecutionItems = new List<StackItem>();
		public static void AddInitializer(MethodBase method) =>
			ExecutionItems.Add(new StackItemInitializer(method));
		public static void AddRootInitializer(MethodBase method) =>
			ExecutionItems.Add(new StackItemInitializer(method, "RootInitializer"));
		public static void AddMethod(MethodBase method, params object[] parameters) =>
			ExecutionItems.Add(new StackItemMethod(method, parameters));
		public static void AddProperty(PropertyInfo property, object value) =>
			ExecutionItems.Add(new StackItemProperty(property, value));
		public static StackItemMethod GetExecutedMethod() =>
			ExecutionItems.FirstOrDefault(c => c.ItemType == StackItemType.Method) as StackItemMethod;
		public static T GetParameterValue<T>(string name) =>
			(T)GetExecutedMethod().Parameters.FirstOrDefault(c => c.Key.Name == name).Value;
		public static T GetParameterValue<T>(int index) =>
			(T)GetExecutedMethod().Parameters.ElementAt(index).Value;
		public static bool IsPropertyDefined(string name) => ExecutionItems.Any(c => c.ItemType == StackItemType.Property && c.Name == name);
		public static T GetPropertyValue<T>(string name)
		{
			var property = ExecutionItems.FirstOrDefault(c => c.ItemType == StackItemType.Property && c.Name == name);
			if (property is null)
				throw new NullReferenceException();

			return (T)((StackItemProperty)property).Value;
		}

		internal static List<StackItemInitializer> GetInitializers() => ExecutionItems
				.Where(c => c.ItemType == StackItemType.Initializer)
				.Select(c => c as StackItemInitializer)
				.ToList();

	}
}