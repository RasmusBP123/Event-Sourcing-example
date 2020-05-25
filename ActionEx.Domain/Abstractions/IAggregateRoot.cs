using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionEx.Domain.Abstractions
{
    public interface IAggregateRoot<out TKey> : IEntity<TKey>
    {
        public long Version { get; }
        IReadOnlyCollection<IDomainEvent<TKey>> Events { get; }
        void ClearEvents();
    }
}
