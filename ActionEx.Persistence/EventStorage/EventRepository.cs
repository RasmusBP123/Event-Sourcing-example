using AuctionEx.Domain;
using AuctionEx.Domain.Abstractions;
using AuctionEx.Persistence.EventStorage.EventStoreFactory;
using AuctionEx.Persistence.Serialization;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuctionEx.Persistence.EventStorage
{
    public class EventRepository : IEventRepository
    {
        private readonly IEventStoreWrapper _eventStore;
        private readonly IEventDeserializer _eventDeserializer;
        private readonly string _streamBaseName;

        public EventRepository(IEventStoreWrapper eventStore, IEventDeserializer eventDeserializer)
        {
            _eventStore = eventStore;
            _eventDeserializer = eventDeserializer;
            var aggregateType = typeof(Item);
            _streamBaseName = aggregateType.Name;
        }

        private static byte[] Serialize(object data)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        }

        public async Task AppendAsync(Item item)
        {
            var connection = await _eventStore.Connect();
            var streamName = GetStreamName(item.Id);
            var eventData = new List<EventData>();

            foreach (var eve in item.Events)
            {
                var eData = Map(eve);
                eventData.Add(eData);
            }

            var expectedVersion = item.Events.Count() - 2;

            await connection.AppendToStreamAsync(streamName, expectedVersion, eventData);
            item.ClearEvents();
            //using(var transaction = await connection.StartTransactionAsync(streamName, item.Version))
            //{
            //    try
            //    {
            //        foreach (var ev in item.Events)
            //        {
            //            var eventData = Map(ev);
            //            await transaction.WriteAsync(eventData);
            //        }
            //       var result = await transaction.CommitAsync();

            //    }
            //    catch (Exception ex)
            //    {
            //        transaction.Rollback();
            //        throw ex;
            //    }
            //}
        }

        public async Task<Item> RehydrateAsync(Guid aggregateId)
        {
            var connection = await _eventStore.Connect();
            var streamName = GetStreamName(aggregateId);
            var events = new List<IDomainEvent<Guid>>();

            StreamEventsSlice currentSlice;
            long nextSliceStart = StreamPosition.Start;
            do
            {
                currentSlice = await connection.ReadStreamEventsForwardAsync(streamName, nextSliceStart, 200, false);
                nextSliceStart = currentSlice.NextEventNumber;

                events.AddRange(currentSlice.Events.Select(Map));
            }
            while (!currentSlice.IsEndOfStream);

            //aggregate.Load(currentSlice.Events.Select(
            //resolvedEvent => resolvedEvent.Deserialzie()).ToArray());

            var result = BaseAggregateRoot<Item, Guid>.Create(events.OrderBy(e => e.AggregateVersion));
            return result;
        }

        private IDomainEvent<Guid> Map(ResolvedEvent resolvedEvent)
        {
            var meta = System.Text.Json.JsonSerializer.Deserialize<EventMeta>(resolvedEvent.Event.Metadata);
            var e =  _eventDeserializer.Deserialize<Guid>(meta.EventType, resolvedEvent.Event.Data);
            return e;
        }

        private static EventData Map(IDomainEvent<Guid> @event)
        {
            var json = System.Text.Json.JsonSerializer.Serialize((dynamic)@event);
            var data = Encoding.UTF8.GetBytes(json);

            var eventType = @event.GetType();
            var meta = new EventMeta()
            {
                EventType = eventType.AssemblyQualifiedName
            };
            var metaJson = System.Text.Json.JsonSerializer.Serialize(meta);
            var metadata = Encoding.UTF8.GetBytes(metaJson);

            var eventPayload = new EventData(Guid.NewGuid(), eventType.Name, true, data, metadata);
            return eventPayload;
        }

        private string GetStreamName(Guid aggregateId)
        {
            var streamName = $"{_streamBaseName}_{aggregateId}";
            return streamName;
        }

        internal struct EventMeta
        {
            public string EventType { get; set; }
        }
    }
}
