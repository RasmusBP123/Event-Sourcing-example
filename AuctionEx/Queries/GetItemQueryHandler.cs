using AuctionEx.Domain;
using AuctionEx.Persistence.EventStorage;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AuctionEx.Queries
{
    public class GetItemQueryHandler : IRequestHandler<GetItemQuery, Item>
    {
        private readonly IEventService _eventService;

        public GetItemQueryHandler(IEventService eventService)
        {
            _eventService = eventService;
        }

        public async Task<Item> Handle(GetItemQuery request, CancellationToken cancellationToken)
        {
            var item = await _eventService.RehydrateAsync(request.Id);
            return item;
        }
    }
}
