using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using S_Buck_HW_4.Models;
using S_Buck_HW_4.Models.Database;

namespace S_Buck_HW_4
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserStock>().HasKey(e => new {e.UserID, e.Symbol});
            modelBuilder.Entity<UserStockFavorite>().HasKey(e => new {e.UserID, e.Symbol});
            modelBuilder.Entity<UserStockTrade>().HasKey(e => new {e.UserID, e.Symbol, e.TradeDateTime});
        }


        public DbSet<StockCompany> StockCompanies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserStock> UserStocks { get; set; }
        public DbSet<UserStockFavorite> UserStockFavorites { get; set; }
        public DbSet<UserStockTrade> UserStockTrades { get; set; }
    }
}
