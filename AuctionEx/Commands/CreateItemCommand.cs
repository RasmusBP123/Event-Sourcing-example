using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuctionEx.Commands
{
    public class CreateItemCommand : IRequest
    {
        public CreateItemCommand(string name, double currentPrice)
        {
            Name = name;
            CurrentPrice = currentPrice;
        }

        public string Name { get; }
        public double CurrentPrice { get; }
    }
}
