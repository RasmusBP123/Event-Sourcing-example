using AuctionEx.Domain;
using AuctionEx.Persistence.EventStorage;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Text;

namespace AuctionEx.Persistence.Serialization
{
    public static class CustomDeserializer
    {
        public static ItemCreatedEvent Deserialize(this ResolvedEvent resolvedEvent)
        {
            var meta = JsonConvert.DeserializeObject<EventMetadata>(
                 Encoding.UTF8.GetString(resolvedEvent.Event.Metadata));
            var dataType = Type.GetType(meta.ClrType);
            var jsonData = Encoding.UTF8.GetString(resolvedEvent.Event.Data);
            var data = JsonConvert.DeserializeObject<ItemCreatedEvent>(jsonData);
            return data;
        }
    }
}
