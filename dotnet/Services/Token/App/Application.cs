using System;
using Branch.Global.Libraries;
using Branch.Services.Token.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServiceStack.Redis;

namespace Branch.Services.Token.App
{
	public partial class Application
	{
		public ILogger _logger { get; }
		public IRedisClientsManager _redisClientsManager { get; }
		private ChromieTalkie _chromieTalkie { get; }
		private Config _config { get; }

		public Application(
			ILoggerFactory loggerFactory,
			IRedisClientsManager redisClientsManager,
			ChromieTalkie chromieTalkie,
			IOptionsMonitor<Config> options
		)
		{
			_logger = loggerFactory.CreateLogger(typeof(Application));
			_redisClientsManager = redisClientsManager;
			_chromieTalkie = chromieTalkie;
			_config = options.CurrentValue;
		}
	}
}
