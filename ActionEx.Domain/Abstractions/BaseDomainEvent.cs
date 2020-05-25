using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionEx.Domain.Abstractions
{
    public abstract class BaseDomainEvent<TA, TKey> : IDomainEvent<TKey>
             where TA : IAggregateRoot<TKey>
    {
        protected BaseDomainEvent() { }

        protected BaseDomainEvent(TA aggregateRoot)
        {
            if (aggregateRoot == null)
                throw new ArgumentNullException(nameof(aggregateRoot));

            this.AggregateVersion = aggregateRoot.Version;
            this.AggregateId = aggregateRoot.Id;
        }

        public long AggregateVersion { get; private set; }
        public TKey AggregateId { get; private set; }
    }
}
