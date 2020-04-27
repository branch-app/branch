using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Branch.Global.Attributes;

namespace Branch
{
	public class Program
	{
		public async static Task Main(string[] args)
		{
			if (args.Length == 0)
				throw new FormatException("No service command was specified");

			var command = args[0];
			var methodInfo = GetCommandMethodInfo(command);
			var invoke = (Task) methodInfo.Invoke(null, new object[] { args.Skip(1).ToArray() });

			await invoke;
		}

		public static MethodInfo GetCommandMethodInfo(string commandName)
		{
			var assembly = Assembly.GetExecutingAssembly();

			return assembly.GetTypes()
				.SelectMany(t => t.GetMethods())
				.FirstOrDefault(m =>
				{
					var attributes = m.GetCustomAttributes(typeof(ServiceEntryAttribute), false);

					if (attributes.Length == 0)
						return false;

					if (attributes.Length > 1)
						throw new IndexOutOfRangeException("Only one ServiceEntryAttribute should be on any one type");

					var attribute = attributes[0] as ServiceEntryAttribute;

					return attribute.CommandName == commandName;
				});
		}
	}
}
