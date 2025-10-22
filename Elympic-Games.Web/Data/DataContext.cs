using Elympic_Games.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace Elympic_Games.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<GameType> GameTypes { get; set; }

        public DbSet<Match> Matches { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Classification> Classifications { get; set; }

        public DbSet<Arena> Arena { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketOrder> TicketOrders { get; set; }

        public DbSet<Cart> Carts { get; set; }



        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product must have an User
            modelBuilder.Entity<Product>()
                .HasOne(p => p.User)
                .WithMany()
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderDetail>()
              .Property(o => o.TotalPriceByDetail)
              .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
              .Property(o => o.TotalPrice)
              .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Ticket>()
              .Property(t => t.Price)
              .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Match>()
                .HasOne(m => m.TeamOne)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.TeamTwo)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Arena>()
                .HasOne(a => a.City)
                .WithMany()
                .HasForeignKey("CityId")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.GameType)
                .WithMany()
                .HasForeignKey(e => e.GameTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
