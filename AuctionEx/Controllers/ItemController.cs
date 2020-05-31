using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuctionEx.Commands;
using AuctionEx.Queries;
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

        [HttpGet("get")]
        public async Task<IActionResult> GetItem()
        {
            var query = await _mediator.Send(new GetItemQuery(new Guid("a33fc4ad-dbfa-4313-8533-05ce8a9ca86f")));
            return Ok(query);
        }
    }
}