using FlightSystem.Data;
using FlightSystem.DTOs.Destination;
using FlightSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightSystem.Services
{
    public class DestinationService : IDestinationService
    {
        private readonly ApplicationDbContext _db;

        public DestinationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<DestinationGetDTO>> GetAllAsync()
        {
            return await _db.Destinations
                .Include(x => x.FromAirport)
                .Include(x => x.ToAirport)
                .Select(x => new DestinationGetDTO
                {
                    Id = x.Id,
                    FromAirportId = x.FromAirportId,
                    FromAirportName = x.FromAirport.Name,
                    ToAirportId = x.ToAirportId,
                    ToAirportName = x.ToAirport.Name,
                    IsActive = x.IsActive
                })
                .ToListAsync();
        }

        public async Task<DestinationGetDTO?> GetByIdAsync(int id)
        {
            var d = await _db.Destinations
                .Include(x => x.FromAirport)
                .Include(x => x.ToAirport)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (d == null) return null;

            return new DestinationGetDTO
            {
                Id = d.Id,
                FromAirportId = d.FromAirportId,
                FromAirportName = d.FromAirport.Name,
                ToAirportId = d.ToAirportId,
                ToAirportName = d.ToAirport.Name,
                IsActive = d.IsActive
            };
        }

        public async Task<DestinationGetDTO> CreateAsync(DestinationAddDTO dto)
        {
            var dest = new FlightSystem.Models.Destination
            {
                FromAirportId = dto.FromAirportId,
                ToAirportId = dto.ToAirportId,
                IsActive = true
            };

            _db.Destinations.Add(dest);
            await _db.SaveChangesAsync();

            var fromAirport = await _db.Airports.FindAsync(dto.FromAirportId);
            var toAirport = await _db.Airports.FindAsync(dto.ToAirportId);

            return new DestinationGetDTO
            {
                Id = dest.Id,
                FromAirportId = dto.FromAirportId,
                FromAirportName = fromAirport?.Name ?? "",
                ToAirportId = dto.ToAirportId,
                ToAirportName = toAirport?.Name ?? "",
                IsActive = true
            };
        }

        public async Task<DestinationGetDTO?> UpdateAsync(DestinationUpdateDTO dto)
        {
            var dest = await _db.Destinations.FindAsync(dto.Id);
            if (dest == null) return null;

            dest.FromAirportId = dto.FromAirportId;
            dest.ToAirportId = dto.ToAirportId;
            dest.IsActive = dto.IsActive;

            await _db.SaveChangesAsync();

            var fromAirport = await _db.Airports.FindAsync(dto.FromAirportId);
            var toAirport = await _db.Airports.FindAsync(dto.ToAirportId);

            return new DestinationGetDTO
            {
                Id = dest.Id,
                FromAirportId = dest.FromAirportId,
                FromAirportName = fromAirport?.Name ?? "",
                ToAirportId = dest.ToAirportId,
                ToAirportName = toAirport?.Name ?? "",
                IsActive = dest.IsActive
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var dest = await _db.Destinations.FindAsync(id);
            if (dest == null) return false;

            _db.Destinations.Remove(dest);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}

