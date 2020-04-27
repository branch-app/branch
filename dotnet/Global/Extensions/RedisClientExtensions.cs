using Newtonsoft.Json;
using ServiceStack.Redis;

namespace Branch.Global.Extensions
{
	public static class RedisClientExtensions
	{
		public static T GetJson<T>(this IRedisClient client, string key)
			where T : class
		{
			var str = client.Get<string>(key);

			if (str == null)
				return null;

			return JsonConvert.DeserializeObject<T>(str);
		}
	}
}
