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
        private readonly Queue<IDomainEvent<TKey>> _events = new Queue<IDomainEvent<TKey>>();

        protected BaseAggregateRoot() { }

        protected BaseAggregateRoot(TKey id) : base(id)
        {
        }

        public IReadOnlyCollection<IDomainEvent<TKey>> Events => _events.ToImmutableArray();

        public long Version { get; private set; }

        public void ClearEvents()
        {
            _events.Clear();
        }

        protected void AddEvent(IDomainEvent<TKey> @event)
        {
            _events.Enqueue(@event);

            Apply(@event);

            Version++;
        }

        static BaseAggregateRoot()
        {
            LazyCtor = new Lazy<ConstructorInfo>(() =>
            {
                var aggregateType = typeof(TA);
                var ctor = aggregateType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                    null, new Type[0], new ParameterModifier[0]);
                return ctor;
            });
        }

        protected abstract void Apply(IDomainEvent<TKey> @event);

        private static readonly Lazy<ConstructorInfo> LazyCtor;

        public static TA Create(IEnumerable<IDomainEvent<TKey>> events)
        {
            if (null == events || !events.Any())
                throw new ArgumentNullException(nameof(events));

            var cTor = LazyCtor.Value;
            var result = (TA)cTor.Invoke(new object[0]);

            var baseAggregate = result as BaseAggregateRoot<TA, TKey>;
            if (baseAggregate != null)
                foreach (var @event in events)
                {
                    baseAggregate.AddEvent(@event);
                }

            result.ClearEvents();

            return result;
        }
    }
}
