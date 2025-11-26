using FlightSystem.Data;
using FlightSystem.DTOs.AdditionalBaggage;
using FlightSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightSystem.Services
{
    public class AdditionalBaggageService : IAdditionalBaggageService
    {
        private readonly ApplicationDbContext _db;

        public AdditionalBaggageService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<AdditionalBaggageGetDTO>> GetAllAsync()
        {
            return await _db.AdditionalBaggages
                .Select(x => new AdditionalBaggageGetDTO
                {
                    Id = x.Id,
                    Type = x.Type,
                    Price = x.Price
                })
                .ToListAsync();
        }

        public async Task<AdditionalBaggageGetDTO?> GetByIdAsync(int id)
        {
            var x = await _db.AdditionalBaggages.FindAsync(id);
            if (x == null) return null;

            return new AdditionalBaggageGetDTO
            {
                Id = x.Id,
                Type = x.Type,
                Price = x.Price
            };
        }

        public async Task<AdditionalBaggageGetDTO> CreateAsync(AdditionalBaggageAddDTO dto)
        {
            var bag = new AdditionalBaggage
            {
                Type = dto.Type,
                Price = dto.Price
            };

            _db.AdditionalBaggages.Add(bag);
            await _db.SaveChangesAsync();

            return await GetByIdAsync(bag.Id);
        }

        public async Task<AdditionalBaggageGetDTO?> UpdateAsync(AdditionalBaggageUpdateDTO dto)
        {
            var bag = await _db.AdditionalBaggages.FindAsync(dto.Id);
            if (bag == null) return null;

            if (dto.Type != null) bag.Type = dto.Type;
            if (dto.Price.HasValue) bag.Price = dto.Price.Value;

            await _db.SaveChangesAsync();
            return await GetByIdAsync(bag.Id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var bag = await _db.AdditionalBaggages.FindAsync(id);
            if (bag == null) return false;

            _db.AdditionalBaggages.Remove(bag);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

