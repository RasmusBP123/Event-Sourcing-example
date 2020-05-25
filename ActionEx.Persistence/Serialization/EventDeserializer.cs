using AuctionEx.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AuctionEx.Persistence.Serialization
{
    public class EventDeserializer : IEventDeserializer
    {
        private readonly IEnumerable<Assembly> _assemblies;

        public EventDeserializer(IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies ?? new[] { Assembly.GetExecutingAssembly() };
        }

        public IDomainEvent<Guid> Deserialize<Guid>(string type, byte[] data)
        {
            var jsonData = Encoding.UTF8.GetString(data);
            return this.Deserialize<Guid>(type, jsonData);
        }

        public IDomainEvent<Guid> Deserialize<Guid>(string type, string data)
        {
            var eventType = _assemblies.Select(a => a.GetType(type, false))
                                .FirstOrDefault(t => t != null) ?? Type.GetType(type);
            if (null == eventType)
                throw new ArgumentOutOfRangeException(nameof(type), $"invalid event type: {type}");

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject(data, eventType,
                new Newtonsoft.Json.JsonSerializerSettings()
                {
                    ConstructorHandling = Newtonsoft.Json.ConstructorHandling.AllowNonPublicDefaultConstructor,
                    ContractResolver = new PrivateSetterContractResolver()
                });

            return (IDomainEvent<Guid>)result;
        }
    }
}
