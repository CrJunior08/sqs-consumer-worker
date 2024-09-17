using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using sqs_consumer_worker.Model;
using sqs_consumer_worker.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace sqs_consumer_worker
{
    public class SqsConsumerWorker : BackgroundService
    {
        private readonly ILogger<SqsConsumerWorker> _logger;
        private readonly SQSService _sqsService;
        private readonly AppSettings _settings;

        public SqsConsumerWorker(ILogger<SqsConsumerWorker> logger, IOptions<AppSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
            _sqsService = new SQSService(_settings.QueueUrl, _settings.MaxMessages, _settings.WaitTimeSeconds);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Checking for new messages...");
                await _sqsService.ReceiveMessagesAsync(stoppingToken);

                // Delay para evitar sobrecarga
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}