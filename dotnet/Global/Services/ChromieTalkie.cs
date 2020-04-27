using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Branch.Global.Libraries;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PuppeteerSharp;

namespace Branch.Global.Services
{
	public class ChromieTalkie : IDisposable
	{
		public class Config
		{
			public string RemoteEndpoint { get; set; }
		}

		private ILogger _logger;
		private Config _config;
		public Browser Browser { get; private set; }

		public ChromieTalkie(ILoggerFactory loggerFactory, IOptionsMonitor<Config> options)
		{
			_logger = loggerFactory.CreateLogger(typeof(ChromieTalkie));
			_config = options.CurrentValue;

			if (_config.RemoteEndpoint == null)
			{
				_logger.LogInformation("No remote puppeteer browser specified, downloading browser");

				var task = new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

				task.Wait();
			}

			EnsureBrowserReady().Wait();
		}

		private async Task EnsureBrowserReady()
		{
			if (_config.RemoteEndpoint == null)
			{
				Browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

				return;
			}

			var chromeClient = new JsonClient($"http://{_config.RemoteEndpoint}");
			var options = new HttpClientOptions { Headers = new Dictionary<string, string> { { "Host", "" } } };
			var meta = await chromeClient.Do<ChromeInstanceMeta>("GET", "/json/version", options);
			var url = meta.WebSocketDebuggerUrl;
			var id = url.Split("/").Last();
			var finalRemoteUrl = $"ws://{_config.RemoteEndpoint}/devtools/browser/{id}";

			Browser = await Puppeteer.ConnectAsync(new ConnectOptions { BrowserWSEndpoint = finalRemoteUrl });
		}

		public void Dispose()
		{
			Browser?.Dispose();
		}

		private class ChromeInstanceMeta
		{
			[JsonProperty("webSocketDebuggerUrl")]
			public string WebSocketDebuggerUrl { get; set; }
		}
	}
}
