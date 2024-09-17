using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using sqs_consumer_worker._2.Infrastructure.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class SqsConsumerWorker : BackgroundService
{
    private readonly IAmazonSQS _sqsClient;
    private readonly ILogger<SqsConsumerWorker> _logger;
    private readonly SqsSettings _sqsSettings;

    public SqsConsumerWorker(IAmazonSQS sqsClient, ILogger<SqsConsumerWorker> logger, IOptions<SqsSettings> sqsSettings)
    {
        _sqsClient = sqsClient;
        _logger = logger;
        _sqsSettings = sqsSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker SQS iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = _sqsSettings.QueueUrl,
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 5
            };

            try
            {
                var response = await _sqsClient.ReceiveMessageAsync(request, stoppingToken);

                foreach (var message in response.Messages)
                {
                    _logger.LogInformation($"Mensagem recebida: {message.Body}");


                    var deleteRequest = new DeleteMessageRequest
                    {
                        QueueUrl = _sqsSettings.QueueUrl,
                        ReceiptHandle = message.ReceiptHandle
                    };
                    await _sqsClient.DeleteMessageAsync(deleteRequest, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consumir mensagens da fila SQS");
            }

            await Task.Delay(2000, stoppingToken);
        }
    }
}
