using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuctionEx.Persistence.EventStorage.EventStoreFactory
{
    public interface IEventStoreWrapper
    {
        Task<IEventStoreConnection> Connect();
    }
}
