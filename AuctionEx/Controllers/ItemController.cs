using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuctionEx.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuctionEx.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : Controller
    {
        private readonly IMediator _mediator;

        public ItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateItem()
        {
            var command = new CreateItemCommand("Smykker fra Romerriget", 2500);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("bid")]
        public async Task<IActionResult> MakeBid()
        {
            var command = new CreateBidCommand(new Guid("a583c96a-c7e3-4748-81e7-beb0cbd284f7"), 8000);
            return Ok(await _mediator.Send(command));
        }
    }
}