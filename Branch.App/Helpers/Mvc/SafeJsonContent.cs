using System.Web.Mvc;
using Newtonsoft.Json;

namespace Branch.App.Helpers.Mvc
{
	public static class SafeJsonContent
	{
		public static ContentResult Create<T>(T content)
		{
			var data = JsonConvert.SerializeObject(content, Formatting.None,
				new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore});

			return new ContentResult
			{
				Content = data, 
				ContentType = "application/json"
			};
		}
	}
}