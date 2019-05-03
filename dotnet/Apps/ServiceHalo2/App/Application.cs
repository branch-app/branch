using System;
using Branch.Apps.ServiceHalo2.Database;
using Branch.Clients.Identity;
using Branch.Clients.Sqs;
using Branch.Packages.Extensions;
using Microsoft.Extensions.Logging;

namespace Branch.Apps.ServiceHalo2.App
{
	public partial class Application
	{
		private readonly ILogger _logger;
		private readonly IdentityClient _identityClient;
		private readonly SqsClient _sqsClient;
		private readonly IServiceProvider _serviceProvider;

		public Application(ILoggerFactory loggerFactory, IdentityClient identityClient, SqsClient sqsClient, IServiceProvider serviceProvider)
		{
			_logger = loggerFactory.CreateLogger(nameof(Application));
			_identityClient = identityClient;
			_sqsClient = sqsClient;
			_serviceProvider = serviceProvider;
		}

		private DatabaseClient _dbClient
		{
			get { return _serviceProvider.GetOrActivateService<DatabaseClient>(); }
		}
	}
}
