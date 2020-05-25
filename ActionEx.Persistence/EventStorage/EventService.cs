using AuctionEx.Domain;
using AuctionEx.Domain.Abstractions;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuctionEx.Persistence.EventStorage
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task PersistAsync(Item item)
        {
            if(item == null)
                throw new ArgumentNullException();
            if(!item.Events.Any())
                return;

            await _eventRepository.AppendAsync(item);
        }

        public async Task<Item> RehydrateAsync(Guid aggregateId)
        {
            return await _eventRepository.RehydrateAsync(aggregateId);
        }
    }
}
