using FlightSystem.Data;
using FlightSystem.DTOs.Airports;
using FlightSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightSystem.Services
{
    public class AirportService : IAirportService
    {
        private readonly ApplicationDbContext _db;

        public AirportService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<AirportGetDTO>> GetAllAsync()
        {
            return await _db.Airports
                .Include(a => a.City)
                .Select(a => new AirportGetDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    CityId = a.CityId,
                    CityName = a.City.Name,
                    IsActive = a.IsActive,
                    Code = a.Code,
                })
                .ToListAsync();
        }

        public async Task<AirportGetDTO?> GetByIdAsync(int id)
        {
            return await _db.Airports
                .Include(a => a.City)
                .Where(a => a.Id == id)
                .Select(a => new AirportGetDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    CityId = a.CityId,
                    CityName = a.City.Name,
                    IsActive = a.IsActive,
                    Code = a.Code,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<AirportGetDTO> AddAsync(AirportAddUpdateDTO dto)
        {
            var airport = new Airport
            {
                Name = dto.Name,
                CityId = dto.CityId,
                IsActive = dto.IsActive,
                Code = dto.Code,
            };

            _db.Airports.Add(airport);
            await _db.SaveChangesAsync();

            var city = await _db.Cities.FindAsync(dto.CityId);

            return new AirportGetDTO
            {
                Id = airport.Id,
                Name = airport.Name,
                CityId = airport.CityId,
                CityName = city?.Name ?? "",
                IsActive = airport.IsActive,
                Code = airport.Code,
            };
        }

        public async Task<AirportGetDTO?> UpdateAsync(int id, AirportAddUpdateDTO dto)
        {
            var airport = await _db.Airports.FindAsync(id);

            if (airport == null)
                return null;

            airport.Name = dto.Name;
            airport.CityId = dto.CityId;
            airport.IsActive = dto.IsActive;
            airport.Code  = dto.Code;

            await _db.SaveChangesAsync();

            var city = await _db.Cities.FindAsync(dto.CityId);

            return new AirportGetDTO
            {
                Id = airport.Id,
                Name = airport.Name,
                CityId = airport.CityId,
                CityName = city?.Name ?? "",
                IsActive = airport.IsActive,
                Code = airport.Code,
               
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var airport = await _db.Airports.FindAsync(id);
            if (airport == null)
                return false;

            _db.Airports.Remove(airport);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<AirportGetDTO>> GetByCityId(int cityId)
        {
            return await _db.Airports
                .Include(a => a.City)
                .Where(a => a.CityId == cityId && a.IsActive) // Samo aktivni aerodomi
                .Select(a => new AirportGetDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    CityId = a.CityId,
                    CityName = a.City.Name,
                    IsActive = a.IsActive,
                    Code = a.Code,
                })
                .ToListAsync();
        }
    }
}

