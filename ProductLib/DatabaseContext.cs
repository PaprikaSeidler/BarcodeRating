using Microsoft.EntityFrameworkCore;
using BarcodeRatingLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeRatingLib
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}
