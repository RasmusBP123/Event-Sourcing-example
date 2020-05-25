using AuctionEx.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuctionEx.Persistence.EventStorage
{
    public interface IEventService
    {
        Task PersistAsync(Item item);
        Task<Item> RehydrateAsync(Guid aggregateId);
    }
}
