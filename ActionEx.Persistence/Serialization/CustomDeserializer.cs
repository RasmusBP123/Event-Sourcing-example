using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static AuctionEx.Persistence.EventStorage.EventRepository;

namespace AuctionEx.Persistence.Serialization
{
    public static class CustomDeserializer
    {
        public static object Deserialzie(this ResolvedEvent resolvedEvent)
        {
            var meta = JsonConvert.DeserializeObject<EventMeta>(
                Encoding.UTF8.GetString(resolvedEvent.Event.Metadata));
            var dataType = Type.GetType(meta.EventType);
            var jsonData = Encoding.UTF8.GetString(resolvedEvent.Event.Data);
            var data = JsonConvert.DeserializeObject(jsonData, dataType);
            return data;
        }
    }
}
