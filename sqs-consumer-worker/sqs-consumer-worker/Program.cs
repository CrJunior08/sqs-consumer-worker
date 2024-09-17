using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using sqs_consumer_worker;
using sqs_consumer_worker.Services;
using sqs_consumer_worker.Model;

var builder = Host.CreateDefaultBuilder(args);

// Adiciona a configura��o do appsettings.json
builder.ConfigureAppConfiguration(config =>
{
    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddEnvironmentVariables();
});

// Injeta o servi�o e o worker no ciclo de vida da aplica��o
builder.ConfigureServices((hostContext, services) =>
{
    services.Configure<AppSettings>(hostContext.Configuration.GetSection("SQS"));
    services.AddHostedService<SqsConsumerWorker>();
});

var app = builder.Build();
await app.RunAsync();
