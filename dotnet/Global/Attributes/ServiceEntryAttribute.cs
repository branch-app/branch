using System;

namespace Branch.Global.Attributes
{
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public class ServiceEntryAttribute : Attribute
	{
		public string CommandName { get; }

		public ServiceEntryAttribute(string commandName)
		{
			CommandName = commandName;
		}
	}
}
