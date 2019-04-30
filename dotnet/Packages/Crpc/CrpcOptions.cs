using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Branch.Packages.Crpc
{
	public class CrpcOptions
	{
		public string[] InternalKeys { get; set; }
	}
}
