using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Branch.Clients.Sqs
{
	public class SqsClient
	{
		public AmazonSQSClient Client { get; }

		public string QueueUrl { get; }

		public SqsClient(IOptions<SqsConfig> options)
		{
			var opts = options.Value;
			var config = new AmazonSQSConfig
			{
				RegionEndpoint = RegionEndpoint.GetBySystemName(opts.Region),
			};

			Client = new AmazonSQSClient(opts.AccessKeyId, opts.SecretAccessKey, config);
			QueueUrl = opts.QueueUrl;
		}

		public async Task<SendMessageResponse> SendMessageAsync<T>(T body)
		{
			return await Client.SendMessageAsync(new SendMessageRequest
			{
				MessageBody = JsonConvert.SerializeObject(body),
				QueueUrl = QueueUrl,
			});
		}

		public async Task<SendMessageBatchResponse> SendMessageBatchAsync<T>(IEnumerable<T> entry)
		{
			var request = new SendMessageBatchRequest
			{
				Entries = entry.Select(e => new SendMessageBatchRequestEntry { MessageBody = JsonConvert.SerializeObject(e) }).ToList(),
				QueueUrl = QueueUrl,
			};

			return await Client.SendMessageBatchAsync(request);
		}
	}
}
