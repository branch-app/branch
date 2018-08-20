using System;
using System.Reflection;
using Newtonsoft.Json.Schema;

namespace Apollo.Models
{
	public class VersionDescriber
	{
		public MethodInfo Method { get; set; }

		public Type RequestType { get; set; }

		public Type ResponseType { get; set; }

		public JSchema JsonSchema { get; set; }
	}
}
