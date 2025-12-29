using FlightSystem.Data;
using FlightSystem.DTOs.Flight;
using FlightSystem.Models;
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

        public async Task<List<FlightGetDTO>> GetAll()
        {
            return await _db.Flights
                .Include(x => x.Destination).ThenInclude(d => d.FromAirport)
                .Include(x => x.Destination).ThenInclude(d => d.ToAirport)
                .Include(x => x.Airline)
                .Include(x => x.Aircraft)
                .Select(x => new FlightGetDTO
                {
                    Id = x.Id,
                    Code = x.Code,
                    Destination = $"{x.Destination.FromAirport.Name} → {x.Destination.ToAirport.Name}",
                    Airline = x.Airline.Name,
                    Aircraft = $"{x.Aircraft.Manufacturer} {x.Aircraft.Model}",
                    DepartureTime = x.DepartureTime,
                    ArrivalTime = x.ArrivalTime,
                    Status = x.Status,
                    Price = x.Price,
                    AvailableSeats = x.AvailableSeats
                })
                .ToListAsync();
        }

        public async Task<FlightGetDTO?> GetById(int id)
        {
            var flight = await _db.Flights
                .Include(x => x.Destination).ThenInclude(d => d.FromAirport)
                .Include(x => x.Destination).ThenInclude(d => d.ToAirport)
                .Include(x => x.Airline)
                .Include(x => x.Aircraft)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (flight == null) return null;

            return new FlightGetDTO
            {
                Id = flight.Id,
                Code = flight.Code,
                Destination = $"{flight.Destination.FromAirport.Name} → {flight.Destination.ToAirport.Name}",
                Airline = flight.Airline.Name,
                Aircraft = $"{flight.Aircraft.Manufacturer} {flight.Aircraft.Model}",
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,
                Status = flight.Status,
                Price = flight.Price,
                AvailableSeats = flight.AvailableSeats
            };
        }

        public async Task<FlightGetDTO> Create(FlightCreateDTO dto)
        {
            var flight = new Flight
            {
                Code = dto.Code,
                DestinationId = dto.DestinationId,
                AirlineId = dto.AirlineId,
                AircraftId = dto.AircraftId,
                DepartureTime = dto.DepartureTime,
                ArrivalTime = dto.ArrivalTime,
                Status = dto.Status,
                Price = dto.Price,
                AvailableSeats = dto.AvailableSeats
            };

            _db.Flights.Add(flight);
            await _db.SaveChangesAsync();

            // Map back to DTO
            return await GetById(flight.Id);
        }

        public async Task<FlightGetDTO?> Update(int id, FlightUpdateDTO dto)
        {
            var flight = await _db.Flights.FindAsync(id);
            if (flight == null) return null;

            flight.Code = dto.Code;
            flight.DestinationId = dto.DestinationId;
            flight.AirlineId = dto.AirlineId;
            flight.AircraftId = dto.AircraftId;
            flight.DepartureTime = dto.DepartureTime;
            flight.ArrivalTime = dto.ArrivalTime;
            flight.Price = dto.Price;
            flight.AvailableSeats = dto.AvailableSeats;
            flight.Status = dto.Status;

            await _db.SaveChangesAsync();

            return await GetById(id);
        }

        public async Task<bool> Delete(int id)
        {
            var flight = await _db.Flights.FindAsync(id);
            if (flight == null) return false;

            _db.Flights.Remove(flight);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<List<FlightGetDTO>> GetByDestination(int destinationId)
        {
            return await _db.Flights
                .Where(x => x.DestinationId == destinationId)
                .Include(x => x.Destination).ThenInclude(d => d.FromAirport)
                .Include(x => x.Destination).ThenInclude(d => d.ToAirport)
                .Include(x => x.Airline)
                .Include(x => x.Aircraft)
                .Select(x => new FlightGetDTO
                {
                    Id = x.Id,
                    Code = x.Code,
                    Destination = $"{x.Destination.FromAirport.Name} → {x.Destination.ToAirport.Name}",
                    Airline = x.Airline.Name,
                    Aircraft = $"{x.Aircraft.Manufacturer} {x.Aircraft.Model}",
                    DepartureTime = x.DepartureTime,
                    ArrivalTime = x.ArrivalTime,
                    Status = x.Status,
                    Price = x.Price,
                    AvailableSeats = x.AvailableSeats
                })
                .ToListAsync();
        }

    }
}





