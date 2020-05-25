using AuctionEx.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuctionEx.Persistence.EventStorage
{
    public interface IEventRepository
    {
        Task<Item> RehydrateAsync(Guid aggregateId);
        Task AppendAsync(Item item);
    }
}
