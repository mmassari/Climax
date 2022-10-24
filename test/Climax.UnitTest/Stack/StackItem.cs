namespace Climax.UnitTest
{
	public abstract class StackItem
	{
		public abstract StackItemType ItemType { get; }
		public abstract string Fullname { get; }
		public string Name { get; protected set; }
		protected StackItem(string name)
		{
			Name = name;
		}
	}
}