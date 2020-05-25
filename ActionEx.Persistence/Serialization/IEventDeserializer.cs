using AuctionEx.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionEx.Persistence.Serialization
{
    public interface IEventDeserializer
    {
        IDomainEvent<Guid> Deserialize<Guid>(string type, byte[] data);
        IDomainEvent<Guid> Deserialize<Guid>(string type, string data);
    }
}
