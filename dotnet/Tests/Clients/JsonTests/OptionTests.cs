using System;
using Xunit;
using Branch.Clients.Json;
using System.Threading.Tasks;
using Branch.Clients.Http.Models;
using System.Collections.Generic;

namespace Branch.Tests.Clients.JsonTests
{
	public class OptionTests
	{

		[Fact]
		public void RespectOptions()
		{
			var options = new Options
			{
				Timeout = TimeSpan.FromMilliseconds(2500),
			};

			var client = new JsonClient("https://example.com", options);

			Assert.Equal(client.Client.Options.Timeout, options.Timeout);
		}
	}
}
