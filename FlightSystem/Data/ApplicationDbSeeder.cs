using FlightSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FlightSystem.Data
{
    public static class ApplicationDbSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // --------- Countries ---------
            var countries = new List<Country>
            {
                new Country { Id = 1, Name = "Bosnia and Herzegovina" },
                new Country { Id = 2, Name = "Croatia" },
                new Country { Id = 3, Name = "Turkey" },
                new Country { Id = 4, Name = "Italy" },
                new Country { Id = 5, Name = "Spain" },
                new Country { Id = 6, Name = "France" }
            };
            modelBuilder.Entity<Country>().HasData(countries);

            // --------- Cities ---------
            var cities = new List<City>
            {
                new City { Id = 1, CountryId = 1, Name = "Sarajevo" },
                new City { Id = 2, CountryId = 1, Name = "Mostar" },

                new City { Id = 3, CountryId = 2, Name = "Zagreb" },
                new City { Id = 4, CountryId = 2, Name = "Split" },

                new City { Id = 5, CountryId = 3, Name = "Istanbul" },
                new City { Id = 6, CountryId = 3, Name = "Antalya" },

                new City { Id = 7, CountryId = 4, Name = "Rome" },
                new City { Id = 8, CountryId = 4, Name = "Milan" },

                new City { Id = 9, CountryId = 5, Name = "Madrid" },
                new City { Id = 10, CountryId = 5, Name = "Barcelona" },

                new City { Id = 11, CountryId = 6, Name = "Paris" },
                new City { Id = 12, CountryId = 6, Name = "Nice" }
            };
            modelBuilder.Entity<City>().HasData(cities);

            // --------- Airports ---------
            var airports = new List<Airport>
            {
                new Airport { Id = 1, CityId = 1, Name = "Sarajevo International Airport (SJJ)", IsActive = true },
                new Airport { Id = 2, CityId = 2, Name = "Mostar Airport (OMO)", IsActive = true },

                new Airport { Id = 3, CityId = 3, Name = "Franjo Tuđman Airport Zagreb (ZAG)", IsActive = true },
                new Airport { Id = 4, CityId = 4, Name = "Split Airport (SPU)", IsActive = true },

                new Airport { Id = 5, CityId = 5, Name = "Istanbul Airport (IST)", IsActive = true },
                new Airport { Id = 6, CityId = 6, Name = "Antalya Airport (AYT)", IsActive = true },

                new Airport { Id = 7, CityId = 7, Name = "Rome Fiumicino Airport (FCO)", IsActive = true },
                new Airport { Id = 8, CityId = 8, Name = "Milan Malpensa Airport (MXP)", IsActive = true },

                new Airport { Id = 9, CityId = 9, Name = "Madrid Barajas Airport (MAD)", IsActive = true },
                new Airport { Id = 10, CityId = 10, Name = "Barcelona El Prat Airport (BCN)", IsActive = true },

                new Airport { Id = 11, CityId = 11, Name = "Charles de Gaulle Airport (CDG)", IsActive = true },
                new Airport { Id = 12, CityId = 12, Name = "Nice Côte d’Azur Airport (NCE)", IsActive = true }
            };
            modelBuilder.Entity<Airport>().HasData(airports);

            // --------- Destinations ---------
            var destinations = new List<Destination>
            {
                new Destination { Id = 1, FromAirportId = 1, ToAirportId = 3, IsActive = true }, // Sarajevo → Zagreb
                new Destination { Id = 2, FromAirportId = 1, ToAirportId = 11, IsActive = true }, // Sarajevo → Pariz
                new Destination { Id = 3, FromAirportId = 3, ToAirportId = 7, IsActive = true }, // Zagreb → Rim
                new Destination { Id = 4, FromAirportId = 5, ToAirportId = 1, IsActive = true }, // Istanbul → Sarajevo
                new Destination { Id = 5, FromAirportId = 7, ToAirportId = 10, IsActive = true }, // Rim → Barcelona
                new Destination { Id = 6, FromAirportId = 10, ToAirportId = 11, IsActive = true } // Barcelona → Pariz
            };
            modelBuilder.Entity<Destination>().HasData(destinations);

            // --------- Airlines ---------
            var airlines = new List<Airline>
            {
                new Airline { Id = 1, Name = "Turkish Airlines", LogoURL = "https://www.aeromobile.net/wp-content/uploads/2023/03/TurkishAirlines_logo.jpg" },
                new Airline { Id = 2, Name = "Lufthansa", LogoURL = "https://cdn.freebiesupply.com/logos/large/2x/lufthansa-2-logo-png-transparent.png" },
                new Airline { Id = 3, Name = "Croatia Airlines", LogoURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/0/0f/Croatia_Airlines_Logo_2.svg/744px-Croatia_Airlines_Logo_2.svg.png" },
                new Airline { Id = 4, Name = "Air France", LogoURL = "https://images.seeklogo.com/logo-png/46/2/air-france-logo-png_seeklogo-464865.png" },
                new Airline { Id = 5, Name = "Pegasus Airlines", LogoURL = "https://logos-world.net/wp-content/uploads/2023/06/Pegasus-Airlines-Logo.png" }
            };
            modelBuilder.Entity<Airline>().HasData(airlines);

            // --------- Aircraft ---------
            var aircrafts = new List<Aircraft>
            {
                new Aircraft { Id = 1, Model = "Boeing 737", Manufacturer = "Boeing", Capacity = 160, RegistrationNumber = "TKA123", YearManufacturer = 2012, Status = true },
                new Aircraft { Id = 2, Model = "Airbus A320", Manufacturer = "Airbus", Capacity = 180, RegistrationNumber = "LHF456", YearManufacturer = 2016, Status = true },
                new Aircraft { Id = 3, Model = "Airbus A321", Manufacturer = "Airbus", Capacity = 220, RegistrationNumber = "CAF789", YearManufacturer = 2018, Status = true }
            };
            modelBuilder.Entity<Aircraft>().HasData(aircrafts);

            // --------- Flights ---------
            var flights = new List<Flight>
            {
                new Flight { Id = 1, Code = "FS-101", DestinationId = 1, AirlineId = 3, AircraftId = 2, DepartureTime = DateTime.Now.AddDays(2), ArrivalTime = DateTime.Now.AddDays(2).AddHours(1), Status = FlightStatus.Scheduled, Price = 120, AvailableSeats = 100 },
                new Flight { Id = 2, Code = "FS-202", DestinationId = 2, AirlineId = 4, AircraftId = 1, DepartureTime = DateTime.Now.AddDays(4), ArrivalTime = DateTime.Now.AddDays(4).AddHours(2), Status = FlightStatus.Scheduled, Price = 180, AvailableSeats = 140 }
            };
            modelBuilder.Entity<Flight>().HasData(flights);

            // --------- Additional Baggage ---------
            modelBuilder.Entity<AdditionalBaggage>().HasData(
                new AdditionalBaggage { Id = 1, Type = "Extra 10kg Baggage", Price = 20 },
                new AdditionalBaggage { Id = 2, Type = "Extra 20kg Baggage", Price = 35 },
                new AdditionalBaggage { Id = 3, Type = "Extra 30kg Baggage", Price = 55 }
            );

            // --------- Users ---------
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FirstName = "Admin", LastName = "Admin", Username = "admin", PasswordHash = "Hash", PasswordSalt = "Salt", Email = "admin@mail.com", PhoneNumber = "000000000", Role = "Admin", IsActive = true }
            );

            // --------- Reservations (Demo) ---------
            modelBuilder.Entity<Reservation>().HasData(
                new Reservation { Id = 1, UserId = 1, FlightId = 1, SeatNumber = "12A", AdditionalBaggageId = 1, TotalPrice = 140, Status = StatusReservation.Confirmed }
            );
        }
    }
}

