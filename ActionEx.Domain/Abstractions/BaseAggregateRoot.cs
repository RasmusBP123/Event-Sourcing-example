using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AuctionEx.Domain.Abstractions
{
    public abstract class BaseAggregateRoot<TA, TKey> : BaseEntity<TKey>, IAggregateRoot<TKey>
        where TA : class, IAggregateRoot<TKey>
    {
        private readonly Queue<object> _events = new Queue<object>();
        public IReadOnlyCollection<object> Events => _events.ToImmutableArray();
        
        public long Version { get; set; } = -2;

        protected BaseAggregateRoot() 
        { }

        protected BaseAggregateRoot(TKey id) : base(id)
        { }


        public void ClearEvents()
        {
            _events.Clear();
        }

        protected void AddEvent(object @event)
        {
            _events.Enqueue(@event);

            Apply(@event);
            Version++;
        }

        protected abstract void Apply(object @event);
        protected abstract void When(dynamic e);
        public void Load(IEnumerable<ItemCreatedEvent> history)
        {
            foreach (var e in history)
            {
                When(e);
                Version++;
            }
        }

    }
}
