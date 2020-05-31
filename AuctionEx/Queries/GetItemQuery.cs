using AuctionEx.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuctionEx.Queries
{
    public class GetItemQuery : IRequest<Item>
    {
        public GetItemQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
