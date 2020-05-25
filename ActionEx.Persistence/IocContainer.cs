using AuctionEx.Persistence.DbStorage;
using AuctionEx.Persistence.EventStorage;
using AuctionEx.Persistence.EventStorage.EventStoreFactory;
using AuctionEx.Persistence.Serialization;
using EventStore.ClientAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionEx.Persistence
{
    public static class IocContainer
    {
        public static IServiceCollection RegisterPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            //var connection = EventStoreConnection.Create(new Uri(configuration.GetConnectionString("eventstore")));
            //connection.ConnectAsync().Wait();

            services.AddSingleton<IEventStoreWrapper>(ctx =>
            {
                var connectionString = configuration.GetConnectionString("eventstore");
                var logger = ctx.GetRequiredService<ILogger<EventStoreWrapper>>();
                return new EventStoreWrapper(new Uri(connectionString), logger);
            });

            services.AddScoped<IEventDeserializer, EventDeserializer>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddDbContext<ReadStorage>(cfg =>
            {
                cfg.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
            });
            return services;
        }
    }
}

//var data = Encoding.UTF8.GetBytes("{\"a\":\"2\"}");
//var metadata = Encoding.UTF8.GetBytes("{}");
//var evt = new EventData(Guid.NewGuid(), "testEvent", true, data, metadata);

//connection.AppendToStreamAsync("test-stream", ExpectedVersion.Any, evt).Wait();
//var streamEvents = connection.ReadStreamEventsForwardAsync("test-stream", 0, 1, false).Result;
//var returnedEvent = streamEvents.Events[0].Event;
//Console.WriteLine("Read event with data: {0}, metadata: {1}",
//Encoding.UTF8.GetString(returnedEvent.Data),
//Encoding.UTF8.GetString(returnedEvent.Metadata));