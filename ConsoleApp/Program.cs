using System;
using System.IO;
using System.Threading.Tasks;
using ConsoleApp.Handlers;
using Core;
using Microsoft.Extensions.Configuration;
using Rebus.Transport.InMem;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // 0. Setup configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var defaultConnection = configuration.GetConnectionString("DefaultConnection");
            var rebusConfig = new RebusConfiguration(
                configuration["Rebus:Transport"],
                configuration["Rebus:Subscription"], 
                configuration["Rebus:DefaultQueue"]);

            // 1. Service registration pipeline...
            var services = new ServiceCollection();
            services.AutoRegisterHandlersFromAssemblyOf<MessageHandler>();

            // 1.1. Configure Rebus
            services.AddRebus(configure => configure
                .Logging(l => l.None())
                .Transport(t => t.UseSqlServer(defaultConnection, rebusConfig.Transport, rebusConfig.DefaultQueue))
                .Subscriptions(s => s.StoreInSqlServer(defaultConnection, rebusConfig.Subscription))
                .Routing(r => r.TypeBased().Map<MessageTransport>(rebusConfig.DefaultQueue)));

            // 2. Application starting pipeline...
            var provider = services.BuildServiceProvider();

            provider.UseRebus();

            var bus = provider.GetRequiredService<IBus>();

            Task.WaitAll(bus.Subscribe<MessageTransport>());

            Console.ReadKey();
        }
    }
}
