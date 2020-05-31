using AuctionEx.Persistence.EventStorage;
using AuctionEx.Persistence.EventStorage.EventStoreFactory;
using AuctionEx.Persistence.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace AuctionEx.Persistence
{
    public static class IocContainer
    {
        public static IServiceCollection RegisterPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEventStoreWrapper>(ctx =>
            {
                var connectionString = configuration.GetConnectionString("eventstore");
                var logger = ctx.GetRequiredService<ILogger<EventStoreWrapper>>();
                return new EventStoreWrapper(new Uri(connectionString), logger);
            });

            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IEventRepository, EventRepository>();

            return services;
        }
    }
}
