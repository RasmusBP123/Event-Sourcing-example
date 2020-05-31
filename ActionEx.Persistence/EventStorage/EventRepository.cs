using AuctionEx.Domain;
using AuctionEx.Persistence.EventStorage.EventStoreFactory;
using AuctionEx.Persistence.Serialization;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionEx.Persistence.EventStorage
{
    public class EventRepository : IEventRepository
    {
        private readonly IEventStoreWrapper _eventStore;
        private readonly string _streamBaseName;

        public EventRepository(IEventStoreWrapper eventStore)
        {
            _eventStore = eventStore;
            var aggregateType = typeof(Item);
            _streamBaseName = aggregateType.Name;
        }

        public async Task AppendAsync(Item item)
        {
            var connection = await _eventStore.Connect();
            var streamName = GetStreamName(item.Id);
            var eventData = item.Events.Select(x => new EventData(Guid.NewGuid(), 
                                                                  type: x.GetType().Name,
                                                                  true, 
                                                                  Serialize(x), 
                                                                  metadata : Serialize(new EventMetadata{ClrType = x.GetType().AssemblyQualifiedName }))).ToArray();

            await connection.AppendToStreamAsync(streamName, item.Version, eventData);
            item.ClearEvents();
        }

        public async Task<Item> RehydrateAsync(Guid aggregateId)
        {
            var connection = await _eventStore.Connect();
            var streamName = GetStreamName(aggregateId);
            var events = new List<object>();

            var aggregate = (Item)Activator.CreateInstance(typeof(Item), true);
            
            StreamEventsSlice currentSlice;

            long nextSliceStart = StreamPosition.Start;
            do
            {
                currentSlice = await connection.ReadStreamEventsForwardAsync(streamName, nextSliceStart, 1024, false);
                nextSliceStart = currentSlice.NextEventNumber;
            }
            while (!currentSlice.IsEndOfStream);

            aggregate.Load(currentSlice.Events.Select(
                        resolvedEvent => resolvedEvent.Deserialize()).ToArray());

            return aggregate;
        }

        private string GetStreamName(Guid aggregateId)
        {
            var streamName = $"{_streamBaseName}_{aggregateId}";
            return streamName;
        }

        private static byte[] Serialize(object data)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        }
    }
}
