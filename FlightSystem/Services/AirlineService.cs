using FlightSystem.Data;
using FlightSystem.DTOs.Airline;
using FlightSystem.Models;
using FlightSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightSystem.Services
{
    public class AirlineService : IAirlineService
    {
        private readonly ApplicationDbContext _db;

        public AirlineService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<AirlineGetDTO>> GetAllAsync()
        {
            return await _db.Airlines
                .Select(a => new AirlineGetDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    LogoURL = a.LogoURL
                })
                .ToListAsync();
        }

        public async Task<AirlineGetDTO?> GetByIdAsync(int id)
        {
            return await _db.Airlines
                .Where(a => a.Id == id)
                .Select(a => new AirlineGetDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    LogoURL = a.LogoURL
                })
                .FirstOrDefaultAsync();
        }

        public async Task<AirlineGetDTO> AddAsync(AirlineAddUpdateDTO dto)
        {
            var airline = new Airline
            {
                Name = dto.Name,
                LogoURL = dto.LogoURL
            };

            _db.Airlines.Add(airline);
            await _db.SaveChangesAsync();

            return new AirlineGetDTO
            {
                Id = airline.Id,
                Name = airline.Name,
                LogoURL = airline.LogoURL
            };
        }

        public async Task<AirlineGetDTO?> UpdateAsync(int id, AirlineAddUpdateDTO dto)
        {
            var airline = await _db.Airlines.FindAsync(id);
            if (airline == null)
                return null;

            airline.Name = dto.Name;
            airline.LogoURL = dto.LogoURL;

            await _db.SaveChangesAsync();

            return new AirlineGetDTO
            {
                Id = airline.Id,
                Name = airline.Name,
                LogoURL = airline.LogoURL
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var airline = await _db.Airlines.FindAsync(id);
            if (airline == null)
                return false;

            _db.Airlines.Remove(airline);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}

