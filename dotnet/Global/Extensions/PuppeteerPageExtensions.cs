using System.Threading.Tasks;
using Newtonsoft.Json;
using PuppeteerSharp;
using PuppeteerSharp.Input;
using ServiceStack.Redis;

namespace Branch.Global.Extensions
{
	public static class PuppeteerPageExtensions
	{
		public static async Task TypeAndSubmitAsync(this Page page, string str)
		{
			await page.Keyboard.TypeAsync(str, new TypeOptions { Delay = 5 });
			await Task.WhenAll(
				Task.Delay(1750),
				page.ClickAsync("input[type=submit]")
			);
		}
	}
}
