using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PuppeteerSharp;

namespace Branch.Global.Services
{
	public class ChromieTalkie : IDisposable
	{
		private ILogger _logger;
		private string _puppeteerEndpoint;
		private Browser _browser;

		public ChromieTalkie(ILoggerFactory loggerFactory, IOptionsMonitor<string> options)
		{
			_logger = loggerFactory.CreateLogger(typeof(ChromieTalkie));
			_puppeteerEndpoint = options.Get("RemotePuppeteerEndpoint");

			if (_puppeteerEndpoint == null)
			{
				_logger.LogInformation("No remote puppeteer browser specified, downloading browser");

				var task = new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);

				task.Wait();
			}
		}

		private async Task EnsureBrowserReady()
		{
			if (_puppeteerEndpoint == null)
			{
				_browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });

				return;
			}

			_browser = await Puppeteer.ConnectAsync(new ConnectOptions { BrowserWSEndpoint = _puppeteerEndpoint });
		}

		public void Dispose()
		{
			_browser.Dispose();
		}
	}
}
