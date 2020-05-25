using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionEx.Domain.Abstractions
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}
