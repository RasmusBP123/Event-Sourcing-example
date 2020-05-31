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

        protected override void Apply(object @event)
        {
            switch (@event)
            {
                case ItemCreatedEvent item: 
                    Id = item.Id;
                    Name = item.Name;
                    CurrentPrice = item.Price;
                    break;
            }
        }

        protected override void When(object @event)
        {
            switch (@event)
            {
                case ItemCreatedEvent item:
                    Id = item.Id;
                    Name = item.Name;
                    CurrentPrice = item.Price;
                    break;
            }
        }
    }


    public class ItemCreatedEvent
    {
        public ItemCreatedEvent() { }
        public ItemCreatedEvent(Item item)
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
