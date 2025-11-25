using FlightSystem.Data;
using FlightSystem.DTOs.Country;
using FlightSystem.Models;
using Microsoft.EntityFrameworkCore;


namespace FlightSystem.Services
{
    public class CountryService : ICountryService
    {
        private readonly ApplicationDbContext _db;

        public CountryService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<CountryGetDTO>> GetAll()
        {
            return await _db.Countries
                .Select(c => new CountryGetDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToListAsync();
            
        }
        public async Task<CountryGetDTO?> GetById(int id)
        {
            var country = await _db.Countries.FindAsync(id);

            if (country == null) return null;
            return new CountryGetDTO
            {
                Id = country.Id,
                Name = country.Name,
            };

        }
        public async Task<CountryGetDTO> Create(CountryAddUpdateDTO dto)
        {
            var country = new Country
            {
                Name = dto.Name,
            };
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();
            return new CountryGetDTO { Id = country.Id, Name = country.Name, };
        }
        public async Task<CountryGetDTO?> Update(int id,CountryAddUpdateDTO dto)
        {
            var country = await _db.Countries.FindAsync(id);
            if (country == null) return null;
            country.Name = dto.Name;
            await _db.SaveChangesAsync();
            return new CountryGetDTO() { Id = country.Id,Name = country.Name, };

        }
        public async Task<bool> Delete(int id)
        {
            var country = await _db.Countries.FindAsync(id);
            if (country == null) return false;
            _db.Countries.Remove(country);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
