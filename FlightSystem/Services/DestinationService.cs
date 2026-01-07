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

        // ===================== GET ALL =====================
        public async Task<List<DestinationGetDTO>> GetAllAsync()
        {
            return await _db.Destinations
                .Include(x => x.FromAirport)
                    .ThenInclude(a => a.City)
                .Include(x => x.ToAirport)
                    .ThenInclude(a => a.City)
                .Select(x => new DestinationGetDTO
                {
                    Id = x.Id,
                    FromCity = x.FromAirport.City.Name,
                    ToCity = x.ToAirport.City.Name,
                    FromAirportCode = x.FromAirport.Code,
                    ToAirportCode = x.ToAirport.Code,
                    IsActive = x.IsActive
                })
                .ToListAsync();
        }

        // ===================== GET BY ID =====================
        public async Task<DestinationGetDTO?> GetByIdAsync(int id)
        {
            var d = await _db.Destinations
                .Include(x => x.FromAirport)
                    .ThenInclude(a => a.City)
                .Include(x => x.ToAirport)
                    .ThenInclude(a => a.City)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (d == null)
                return null;

            return new DestinationGetDTO
            {
                Id = d.Id,
                FromCity = d.FromAirport.City.Name,
                ToCity = d.ToAirport.City.Name,
                FromAirportCode = d.FromAirport.Code,
                ToAirportCode = d.ToAirport.Code,
                IsActive = d.IsActive
            };
        }

        // ===================== CREATE =====================
        public async Task<DestinationGetDTO> CreateAsync(DestinationAddDTO dto)
        {
            var dest = new Destination
            {
                FromAirportId = dto.FromAirportId,
                ToAirportId = dto.ToAirportId,
                IsActive = dto.IsActive,
            };

            _db.Destinations.Add(dest);
            await _db.SaveChangesAsync();

            var fullDest = await _db.Destinations
                .Include(x => x.FromAirport)
                    .ThenInclude(a => a.City)
                .Include(x => x.ToAirport)
                    .ThenInclude(a => a.City)
                .FirstAsync(x => x.Id == dest.Id);

            return new DestinationGetDTO
            {
                Id = fullDest.Id,
                FromCity = fullDest.FromAirport.City.Name,
                ToCity = fullDest.ToAirport.City.Name,
                FromAirportCode = fullDest.FromAirport.Code,
                ToAirportCode = fullDest.ToAirport.Code,
                IsActive = fullDest.IsActive
            };
        }

        // ===================== UPDATE =====================
        public async Task<DestinationUpdateDTO?> UpdateAsync(DestinationUpdateDTO dto)
        {
            var dest = await _db.Destinations.FindAsync(dto.Id);
            if (dest == null)
                return null;

            dest.IsActive = dto.IsActive;

            await _db.SaveChangesAsync();

            return new DestinationUpdateDTO
            {
                Id = dest.Id,
                IsActive = dest.IsActive
            };
        }

        // ===================== DELETE =====================
        public async Task<bool> DeleteAsync(int id)
        {
            var dest = await _db.Destinations.FindAsync(id);
            if (dest == null)
                return false;

            _db.Destinations.Remove(dest);
            await _db.SaveChangesAsync();
            return true;
        }

        

    }
}


