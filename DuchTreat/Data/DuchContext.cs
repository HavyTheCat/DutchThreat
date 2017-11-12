using DuchTreat.Data.Entities;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuchTreat.Data
{
    public class DuchContext : IdentityDbContext<StoreUser>
    {
        public DuchContext(DbContextOptions<DuchContext> options) : base(options)
        {       
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        
    }
}
