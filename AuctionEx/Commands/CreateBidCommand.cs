using AuctionEx.Persistence.EventStorage;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuctionEx.Commands
{
    public class CreateBidCommand : IRequest
    {
        public Guid Id { get; }
        public double Price { get; }
        public CreateBidCommand(Guid id, double price)
        {
            Id = id;
            Price = price;
        }
    }

    public class CreateBidCommandHandler : IRequestHandler<CreateBidCommand>
    {
        private readonly IEventService _eventService;

        public CreateBidCommandHandler(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<Unit> Handle(CreateBidCommand request, CancellationToken cancellationToken)
        {
            var item = await _eventService.RehydrateAsync(request.Id);
            item.MakeBid(request.Price);

            await _eventService.PersistAsync(item);

            return Unit.Value;
        }
    }
}
