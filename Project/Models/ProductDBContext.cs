using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace Project.Models
{
    public class ProductDBContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}