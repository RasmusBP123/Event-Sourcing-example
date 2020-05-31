using AuctionEx.Domain;
using AuctionEx.Persistence.EventStorage;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AuctionEx.Commands
{
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
