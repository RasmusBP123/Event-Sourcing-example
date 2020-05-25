using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionEx.Domain.Abstractions
{
    public interface IDomainEvent<out TKey>
    {
        long AggregateVersion { get; }
        TKey AggregateId { get; }
    }
}
