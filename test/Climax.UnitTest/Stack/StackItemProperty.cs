using System.Reflection;

namespace Climax.UnitTest
{
	public class StackItemProperty : StackItem
	{
		public override StackItemType ItemType => StackItemType.Property;
		public override string Fullname => $"{Property.DeclaringType.Name}>{Property.Name}";
		public object Value { get; set; } = null;
		public PropertyInfo Property { get; set; }
		public StackItemProperty(PropertyInfo property, object value) : base(property.Name)
		{
			Property = property;
			Value = value;
		}
	}
}