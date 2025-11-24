using FlightSystem.Data;
using FlightSystem.DTOs.Flight;
using FlightSystem.Models;
using FlightSystem.Services;
using Microsoft.EntityFrameworkCore;


namespace FlightSystem.Services
{
    public class FlightService : IFlightService
    {
        private readonly ApplicationDbContext _db;

        public FlightService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<FlightGetDto> GetAll()
        {
            return _db.Flights
                .Include(f => f.OriginAirport)
                .Include(f => f.DestinationAirport)
                .Include(f => f.Aircraft)
                .Select(f => new FlightGetDto
                {
                    Id = f.Id,
                    FlightNumber = f.FlightNumber,

                    OriginAirportId = f.OriginAirportId,
                    OriginAirportName = f.OriginAirport!.Name,

                    DestinationAirportId = f.DestinationAirportId,
                    DestinationAirportName = f.DestinationAirport!.Name,

                    AircraftId = f.AircraftId,
                    AircraftModel = f.Aircraft!.Model,

                    DurationHours = f.Duration.TotalHours,
                    BasePrice = f.BasePrice
                })
                .ToList();
        }

        public FlightGetDto? GetById(int id)
        {
            return _db.Flights
                .Include(f => f.OriginAirport)
                .Include(f => f.DestinationAirport)
                .Include(f => f.Aircraft)
                .Where(f => f.Id == id)
                .Select(f => new FlightGetDto
                {
                    Id = f.Id,
                    FlightNumber = f.FlightNumber,

                    OriginAirportId = f.OriginAirportId,
                    OriginAirportName = f.OriginAirport!.Name,

                    DestinationAirportId = f.DestinationAirportId,
                    DestinationAirportName = f.DestinationAirport!.Name,

                    AircraftId = f.AircraftId,
                    AircraftModel = f.Aircraft!.Model,

                    DurationHours = f.Duration.TotalHours,
                    BasePrice = f.BasePrice
                })
                .FirstOrDefault();
        }

        public FlightGetDto Create(FlightCreateDto dto)
        {
            var flight = new Flight
            {
                FlightNumber = dto.FlightNumber,
                OriginAirportId = dto.OriginAirportId,
                DestinationAirportId = dto.DestinationAirportId,
                AircraftId = dto.AircraftId,
                Duration = TimeSpan.FromHours(dto.DurationHours),
                BasePrice = dto.BasePrice
            };

            _db.Flights.Add(flight);
            _db.SaveChanges();

            return GetById(flight.Id)!;
        }

        public FlightGetDto? Update(int id, FlightCreateDto dto)
        {
            var flight = _db.Flights.Find(id);
            if (flight == null) return null;

            flight.FlightNumber = dto.FlightNumber;
            flight.OriginAirportId = dto.OriginAirportId;
            flight.DestinationAirportId = dto.DestinationAirportId;
            flight.AircraftId = dto.AircraftId;
            flight.Duration = TimeSpan.FromHours(dto.DurationHours);
            flight.BasePrice = dto.BasePrice;

            _db.SaveChanges();

            return GetById(flight.Id);
        }

        public bool Delete(int id)
        {
            var flight = _db.Flights.Find(id);
            if (flight == null) return false;

            _db.Flights.Remove(flight);
            _db.SaveChanges();
            return true;
        }
    }
}

