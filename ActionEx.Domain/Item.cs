using AuctionEx.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionEx.Domain
{
    public class Item : BaseAggregateRoot<Item, Guid>
    {
        public string Name { get; set; }
        public double CurrentPrice { get; set; }

        private Item()
        {}

        public Item(string name, double currentPrice)
        {
            Id = Guid.NewGuid();
            Name = name;
            CurrentPrice = currentPrice;

            AddEvent(new ItemCreatedEvent(this));
        }

        protected override void Apply(IDomainEvent<Guid> @event)
        {
            switch (@event)
            {
                case ItemCreatedEvent item: 
                    Id = item.Id;
                    Name = item.Name;
                    CurrentPrice = item.Price;
                    break;
                case MadeBidEvent bid:
                    Id = bid.Id;
                    Name = bid.Name;
                    CurrentPrice = bid.Price;
                    break;
            }
        }

        public void MakeBid(double price)
        {
            if(price <= CurrentPrice)
                throw new Exception("Price cannot be lower than current price");

            CurrentPrice = price;
            AddEvent(new MadeBidEvent(this));
        }

    }

    internal class MadeBidEvent : BaseDomainEvent<Item, Guid>
    {
        private MadeBidEvent() { }
        public MadeBidEvent(Item item) : base(item)
        {
            Id = Guid.NewGuid();
            Name = item.Name;
            Price = item.CurrentPrice;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }

    internal class ItemCreatedEvent : BaseDomainEvent<Item, Guid>
    {
        private ItemCreatedEvent() { }
        public ItemCreatedEvent(Item item) : base(item)
        {
            Id = item.Id;
            Name = item.Name;
            Price = item.CurrentPrice;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
