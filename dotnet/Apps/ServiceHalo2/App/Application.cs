using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Branch.Apps.ServiceHalo2.Database;
using Branch.Clients.Identity;
using Branch.Clients.Sqs;
using Microsoft.Extensions.Logging;

namespace Branch.Apps.ServiceHalo2.App
{
	public partial class Application
	{
		private readonly ILogger _logger;
		private readonly IdentityClient _identityClient;
		private readonly SqsClient _sqsClient;

		public Application(ILoggerFactory loggerFactory, IdentityClient identityClient, SqsClient sqsClient)
		{
			_logger = loggerFactory.CreateLogger(nameof(Application));
			_identityClient = identityClient;
			_sqsClient = sqsClient;
		}
	}
}
