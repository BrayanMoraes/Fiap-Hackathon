using HealthMed.AgendaConsumer;
using HealthMed.Domain.Interfaces.Queue;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Infra.Repositories;
using HealthMed.IoC;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args)
   .ConfigureServices((context, services) =>
   {
       services.AddSql(context.Configuration);
       services.AddRabbitMq();

       // Repositório
       services.AddScoped<IAgendaRepository, AgendaRepository>();

       // Consumidores
       services.AddSingleton<IQueueConsumer, AgendaAddConsumer>();
       services.AddSingleton<IQueueConsumer, AgendaUpdateConsumer>();
   });

var app = builder.Build();

using var scope = app.Services.CreateScope();
var consumers = scope.ServiceProvider.GetServices<IQueueConsumer>();
foreach (var consumer in consumers)
{
    consumer.StartConsuming();
}

Console.WriteLine("Consumidores iniciados. Pressione [Enter] para sair.");
Console.ReadLine();

await app.RunAsync();