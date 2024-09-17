using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using sqs_consumer_worker.Utils;
using sqs_consumer_worker.Models;

namespace sqs_consumer_worker.Services
{
    public class SQSService
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly string _queueUrl;
        private readonly int _maxMessages;
        private readonly int _waitTimeSeconds;

        public SQSService(string queueUrl, int maxMessages, int waitTimeSeconds)
        {
            _queueUrl = queueUrl;
            _maxMessages = maxMessages;
            _waitTimeSeconds = waitTimeSeconds;
            _sqsClient = new AmazonSQSClient(new AmazonSQSConfig
            {
                ServiceURL = Environment.GetEnvironmentVariable("AWS_SERVICE_URL") ?? "https://localhost.localstack.cloud:4566"
            });
        }

        public async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
        {
            var msgResponse = await GetMessagesAsync(cancellationToken);
            if (msgResponse.Messages.Count > 0)
            {
                foreach (var msg in msgResponse.Messages)
                {
                    var sqsMessage = new SQSMessageModel
                    {
                        MessageId = msg.MessageId,
                        Body = msg.Body
                    };

                    if (ProcessMessage(sqsMessage))
                    {
                        await DeleteMessageAsync(msg);
                    }
                }
            }
        }

        private async Task<ReceiveMessageResponse> GetMessagesAsync(CancellationToken cancellationToken)
        {
            return await _sqsClient.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = _queueUrl,
                MaxNumberOfMessages = _maxMessages,
                WaitTimeSeconds = _waitTimeSeconds
            }, cancellationToken);
        }

        private bool ProcessMessage(SQSMessageModel message)
        {
            Logger.LogInfo($"Message ID: {message.MessageId}");
            Logger.LogInfo($"Message Body: {message.Body}");
            return true;
        }

        private async Task DeleteMessageAsync(Message message)
        {
            Logger.LogInfo($"Deleting message {message.MessageId} from queue...");
            await _sqsClient.DeleteMessageAsync(_queueUrl, message.ReceiptHandle);
        }
    }
}
