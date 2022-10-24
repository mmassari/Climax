namespace Climax
{
	internal abstract class OptionBase : IOption
	{
		public string Name { get; protected set; }
		public string Description { get; protected set; }
	}
}
