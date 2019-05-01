using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Branch.Apps.ServiceHalo2.Models;
using Branch.Clients.Sqs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sentry;

namespace Branch.Apps.ServiceHalo2.Services
{
	public class QueueService : HostedService
	{
		private readonly BnetClient _bnetClient;
		private readonly SqsClient _sqsClient;
		private readonly IHub _sentry;

		public QueueService(BnetClient bnetClient, SqsClient sqsClient, IHub sentry)
		{
			_bnetClient = bnetClient;
			_sqsClient = sqsClient;
			_sentry = sentry;
		}

		protected override async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			while (!cancellationToken.IsCancellationRequested)
			{
				var request = new ReceiveMessageRequest(_sqsClient.QueueUrl)
				{
					WaitTimeSeconds = 20,
					MaxNumberOfMessages = 5,
				};
				var response = await _sqsClient.Client.ReceiveMessageAsync(request, cancellationToken);

				await processQueue(response.Messages);
				await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);
			}
		}

		private async Task processQueue(List<Message> messages)
		{
			if (messages.Count == 0)
				return;

			var messagesToDelete = new List<Message>();

			foreach (var message in messages)
			{
				var parsed = JsonConvert.DeserializeObject<QueueEvent>(message.Body);
				var deleteMessage = false;

				switch(parsed.Type)
				{
					case QueueEventTypes.CacheServiceRecord:
						var payload = parsed.Payload as ServiceRecordPayload;
						deleteMessage = await runAndIgnoreException(_bnetClient.CacheServiceRecord(payload.Gamertag));
						break;

					case QueueEventTypes.CacheRecentMatches:
						// TODO(0xdeafcafe): Write logic for this
						break;

					default:
						throw new NotSupportedException("not yet");
				}

				if (deleteMessage)
					messagesToDelete.Add(message);
			}

			if (messagesToDelete.Count > 1)
				await _sqsClient.DeleteMessageBatchAsync(messagesToDelete);
		}

		private async Task<bool> runAndIgnoreException(Task<bool> task)
		{
			try { return await task; }
			catch (Exception ex)
			{
				_sentry.CaptureException(ex);

				return false;
			}
		}
	}
}
