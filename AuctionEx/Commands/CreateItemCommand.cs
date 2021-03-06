﻿using AuctionEx.Domain;
using AuctionEx.Persistence.EventStorage;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand>
    {
        private readonly IEventService _eventService;

        public CreateItemCommandHandler(IEventService eventService)
        {
            _eventService = eventService;
            _eventService = eventService;
        }

        public async Task<Unit> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var item = new Item(request.Name, request.CurrentPrice);
            await _eventService.PersistAsync(item);
            return Unit.Value;
        }
    }
}
