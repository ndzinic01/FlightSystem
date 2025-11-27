using Microsoft.EntityFrameworkCore;
using FlightSystem.Models;

namespace FlightSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts)
            : base(opts)
        {

        }

        public DbSet<Airport> Airports { get; set; } = null!;
        public DbSet<Aircraft> Aircrafts { get; set; } = null!;
        public DbSet<Flight> Flights { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<AdditionalBaggage> AdditionalBaggages { get; set; } = null!;
        public DbSet<Airline> Airlines { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<Destination> Destinations { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplicationDbSeeder.Seed(modelBuilder);

            // -----------------------------
            // DESTINATION RELATIONSHIPS
            // -----------------------------
            modelBuilder.Entity<Destination>()
                .HasOne(d => d.FromAirport)
                .WithMany()
                .HasForeignKey(d => d.FromAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Destination>()
                .HasOne(d => d.ToAirport)
                .WithMany()
                .HasForeignKey(d => d.ToAirportId)
                .OnDelete(DeleteBehavior.Restrict);

            // -----------------------------
            // FLIGHT RELATIONSHIPS
            // -----------------------------
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Destination)
                .WithMany()
                .HasForeignKey(f => f.DestinationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Airline)
                .WithMany(a => a.Flights)
                .HasForeignKey(f => f.AirlineId)
                .OnDelete(DeleteBehavior.Restrict);

            //Flight → Aircraft(ako ga budeš dodala)
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Aircraft)
                .WithMany(a => a.Flights)
                .HasForeignKey(f => f.AircraftId)
                .OnDelete(DeleteBehavior.Restrict);

            //-----------------------------
            //RESERVATION RELATIONSHIPS
            //---------------------------- -
           modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Flight)
                .WithMany()
                .HasForeignKey(r => r.FlightId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.AdditionalBaggage)
                .WithMany()
                .HasForeignKey(r => r.AdditionalBaggageId)
                .OnDelete(DeleteBehavior.Restrict);

            // -----------------------------
            // NOTIFICATION RELATIONSHIPS
            // -----------------------------
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Flight)
                .WithMany()
                .HasForeignKey(n => n.FlightId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}




