using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // Configurar o AWS SQS
        services.AddDefaultAWSOptions(hostContext.Configuration.GetAWSOptions());
        services.AddAWSService<IAmazonSQS>();

        // Registrar o worker que vai consumir as mensagens
        services.AddHostedService<SqsConsumerWorker>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

await host.RunAsync();
