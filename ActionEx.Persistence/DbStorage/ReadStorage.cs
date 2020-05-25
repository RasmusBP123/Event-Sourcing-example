using AuctionEx.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionEx.Persistence.DbStorage
{
    public class ReadStorage : DbContext
    {
        public ReadStorage(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }

    }
}
