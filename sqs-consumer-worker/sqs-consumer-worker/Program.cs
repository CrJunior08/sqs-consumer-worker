using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using sqs_consumer_worker;
using sqs_consumer_worker.Services;
using sqs_consumer_worker.Model;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureAppConfiguration(config =>
{
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddEnvironmentVariables();
});

builder.ConfigureServices((hostContext, services) =>
{
    services.Configure<AppSettings>(hostContext.Configuration.GetSection("SQS"));
    services.AddHostedService<SqsConsumerWorker>();
});

var app = builder.Build();
await app.RunAsync();
