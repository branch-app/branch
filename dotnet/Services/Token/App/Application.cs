using Branch.Global.Services;
using Microsoft.Extensions.Logging;

namespace Branch.Services.Token.App
{
	public partial class Application
	{
		public ILogger _logger { get; }
		private ChromieTalkie _chromieTalkie { get; }

		public Application(ILoggerFactory loggerFactory, ChromieTalkie chromieTalkie)
		{
			_logger = loggerFactory.CreateLogger(typeof(Application));
			_chromieTalkie = chromieTalkie;
		}
	}
}
