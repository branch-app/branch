using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
			_config = options.Get("RemoteEndpoint");

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

			Browser = await Puppeteer.ConnectAsync(new ConnectOptions { BrowserWSEndpoint = _config.RemoteEndpoint });
		}

		public void Dispose()
		{
			Browser?.Dispose();
		}
	}
}
