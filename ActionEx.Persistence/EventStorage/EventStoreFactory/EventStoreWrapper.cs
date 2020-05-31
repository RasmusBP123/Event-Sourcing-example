using EventStore.ClientAPI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuctionEx.Persistence.EventStorage.EventStoreFactory
{
    public class EventStoreWrapper : IEventStoreWrapper
    {
        private readonly Lazy<Task<IEventStoreConnection>> _lazyConnection;
        private readonly Uri _connString;
        private readonly ILogger<EventStoreWrapper> _logger;

        public EventStoreWrapper(Uri connString, ILogger<EventStoreWrapper> logger)
        {
            _connString = connString;
            _logger = logger;

            _lazyConnection = new Lazy<Task<IEventStoreConnection>>(() =>
            {
                return Task.Run(async () =>
                {
                    var connection = SetupConnection();

                    await connection.ConnectAsync();

                    return connection;
                });
            });
        }

        private IEventStoreConnection SetupConnection()
        {
            var settings = ConnectionSettings.Create().Build();
            var connection = EventStoreConnection.Create(settings, _connString);

            return connection;
        }

        public Task<IEventStoreConnection> Connect()
        {
            return _lazyConnection.Value;
        }

        public void Dispose()
        {
            if (!_lazyConnection.IsValueCreated)
                return;

            _lazyConnection.Value.Result.Dispose();
        }
    }
}
