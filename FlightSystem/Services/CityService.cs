using FlightSystem.Data;
using FlightSystem.DTOs.City;
using FlightSystem.Models;
using FlightSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightSystem.Services
{
    public class CityService : ICityService
    {
        private readonly ApplicationDbContext _db;

        public CityService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<CityGetDTO>> GetAll()
        {
            return await _db.Cities
                .Include(c => c.Country)
                .Select(c => new CityGetDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    CountryId = c.CountryId,
                    CountryName = c.Country.Name
                })
                .ToListAsync();
        }

        public async Task<CityGetDTO?> GetById(int id)
        {
            var city = await _db.Cities.Include(c => c.Country).FirstOrDefaultAsync(c => c.Id == id);
            if (city == null) return null;
            return new CityGetDTO
            {
                Id = city.Id,
                Name = city.Name,
                CountryId = city.CountryId,
                CountryName = city.Country.Name
            };
        }

        public async Task<CityGetDTO> Create(CityAddUpdateDTO dto)
        {
            var city = new City
            {
                Name = dto.Name,
                CountryId = dto.CountryId
            };
            _db.Cities.Add(city);
            await _db.SaveChangesAsync();
            return await GetById(city.Id);
        }

        public async Task<CityGetDTO?> Update(int id, CityAddUpdateDTO dto)
        {
            var city = await _db.Cities.FindAsync(id);
            if (city == null) return null;
            city.Name = dto.Name;
            city.CountryId = dto.CountryId;
            await _db.SaveChangesAsync();
            return await GetById(city.Id);
        }

        public async Task<bool> Delete(int id)
        {
            var city = await _db.Cities.FindAsync(id);
            if (city == null) return false;
            _db.Cities.Remove(city);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<List<CityGetDTO>> GetByCountryId(int countryId)
        {
            return await _db.Cities
                .Include(c => c.Country)
                .Where(c => c.CountryId == countryId)
                .Select(c => new CityGetDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    CountryId = c.CountryId,
                    CountryName = c.Country.Name
                })
                .ToListAsync();
        }
    }
}

