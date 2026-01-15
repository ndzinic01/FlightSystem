using FlightSystem.Data;
using FlightSystem.DTOs.Reservation;
using FlightSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightSystem.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _db;

        public ReservationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<ReservationGetDTO>> GetAllAsync()
        {
            return await _db.Reservations
                .Include(r => r.User)
                .Include(r => r.Flight)
                    .ThenInclude(f => f.Destination)
                    .ThenInclude(d => d.FromAirport)
                    .ThenInclude(a => a.City) 
                .Include(r => r.Flight)
                    .ThenInclude(f => f.Destination)
                    .ThenInclude(d => d.ToAirport)
                    .ThenInclude(a => a.City) 
                .Include(r => r.AdditionalBaggage)
                .Select(r => new ReservationGetDTO
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    UserFullName = r.User.FirstName + " " + r.User.LastName,
                    FlightId = r.FlightId,
                    FlightNumber = r.Flight.Code,
                    
                    Destination = r.Flight.Destination.FromAirport.City.Name + " → " + r.Flight.Destination.ToAirport.City.Name,
                    SeatNumber = r.SeatNumber,
                    AdditionalBaggageId = r.AdditionalBaggageId,
                    AdditionalBaggageType = r.AdditionalBaggage != null ? r.AdditionalBaggage.Type : null,
                    TotalPrice = r.TotalPrice,
                    Status = r.Status,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ReservationGetDTO?> GetByIdAsync(int id)
        {
            var r = await _db.Reservations
                .Include(x => x.User)
                .Include(x => x.Flight)
                    .ThenInclude(f => f.Destination)
                    .ThenInclude(d => d.FromAirport)
                    .ThenInclude(a => a.City) 
                .Include(x => x.Flight)
                    .ThenInclude(f => f.Destination)
                    .ThenInclude(d => d.ToAirport)
                    .ThenInclude(a => a.City) 
                .Include(x => x.AdditionalBaggage)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (r == null) return null;

            return new ReservationGetDTO
            {
                Id = r.Id,
                UserId = r.UserId,
                UserFullName = r.User.FirstName + " " + r.User.LastName,
                FlightId = r.FlightId,
                FlightNumber = r.Flight.Code,
                // 🔥 DODANO: Destinacija sa gradovima
                Destination = r.Flight.Destination.FromAirport.City.Name + " → " + r.Flight.Destination.ToAirport.City.Name,
                SeatNumber = r.SeatNumber,
                AdditionalBaggageId = r.AdditionalBaggageId,
                AdditionalBaggageType = r.AdditionalBaggage?.Type,
                TotalPrice = r.TotalPrice,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            };
        }

        public async Task<ReservationGetDTO> CreateAsync(ReservationAddDTO dto)
        {
            // --- Automatski izračun cijene ---
            var flight = await _db.Flights.FindAsync(dto.FlightId);
            if (flight == null)
                throw new Exception("Flight not found.");

            // Zabrana rezervacije ako je let otkazan
            if (flight.Status == FlightStatus.Cancelled)
                throw new Exception("Reservation not allowed. Flight has been cancelled.");

            // Ako nema više slobodnih mjesta
            if (flight.AvailableSeats <= 0)
                throw new Exception("No available seats for this flight!");

            // Provjera da li je isto sjedalo već rezervisano
            bool seatAlreadyTaken = await _db.Reservations
                .AnyAsync(r => r.FlightId == dto.FlightId && r.SeatNumber == dto.SeatNumber);
            if (seatAlreadyTaken)
                throw new Exception($"Seat {dto.SeatNumber} is already reserved!");

            decimal total = flight.Price;

            if (dto.AdditionalBaggageId.HasValue)
            {
                var bag = await _db.AdditionalBaggages.FindAsync(dto.AdditionalBaggageId);
                if (bag != null) total += bag.Price;
            }

            var reservation = new FlightSystem.Models.Reservation
            {
                UserId = dto.UserId,
                FlightId = dto.FlightId,
                SeatNumber = dto.SeatNumber,
                AdditionalBaggageId = dto.AdditionalBaggageId,
                TotalPrice = total,
                Status = dto.Status
            };

            _db.Reservations.Add(reservation);
            flight.AvailableSeats--;
            await _db.SaveChangesAsync();

            return await GetByIdAsync(reservation.Id);
        }

        public async Task<ReservationGetDTO?> UpdateAsync(ReservationUpdateDTO dto)
        {
            var r = await _db.Reservations.FindAsync(dto.Id);
            if (r == null) return null;

            if (dto.SeatNumber != null) r.SeatNumber = dto.SeatNumber;
            if (dto.AdditionalBaggageId.HasValue) r.AdditionalBaggageId = dto.AdditionalBaggageId;
            if (dto.Status.HasValue) r.Status = dto.Status.Value;

            await _db.SaveChangesAsync();
            return await GetByIdAsync(r.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var r = await _db.Reservations.FindAsync(id);
            if (r == null) return false;

            _db.Reservations.Remove(r);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

