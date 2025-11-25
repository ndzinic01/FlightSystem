//using FlightSystem.Data;
//using FlightSystem.DTOs.Airports;
//using FlightSystem.Models;
//using FlightSystem.Services.Interfaces;
//using Microsoft.EntityFrameworkCore;

//namespace FlightSystem.Services
//{
//    public class AirportService : IAirportService
//    {
//        private readonly ApplicationDbContext _db;

//        public AirportService(ApplicationDbContext db)
//        {
//            _db = db;
//        }

//        public async Task<List<AirportGetDto>> GetAll(string? search)
//        {
//            var query = _db.Airports.AsQueryable();

//            if (!string.IsNullOrWhiteSpace(search))
//                query = query.Where(a => a.Name.Contains(search) || a.City.Contains(search));

//            return await query
//                .Select(a => new AirportGetDto
//                {
//                    Id = a.Id,
//                    IATA = a.IATA,
//                    Name = a.Name,
//                    City = a.City
//                })
//                .ToListAsync();
//        }

//        public async Task<AirportGetDto?> GetById(int id)
//        {
//            var airport = await _db.Airports.FindAsync(id);
//            if (airport == null) return null;

//            return new AirportGetDto
//            {
//                Id = airport.Id,
//                IATA = airport.IATA,
//                Name = airport.Name,
//                City = airport.City
//            };
//        }

//        public async Task<AirportGetDto> Create(AirportCreateDto dto)
//        {
//            var airport = new Airport
//            {
//                IATA = dto.IATA,
//                Name = dto.Name,
//                City = dto.City
//            };

//            _db.Airports.Add(airport);
//            await _db.SaveChangesAsync();

//            return new AirportGetDto
//            {
//                Id = airport.Id,
//                IATA = airport.IATA,
//                Name = airport.Name,
//                City = airport.City
//            };
//        }

//        public async Task<AirportGetDto?> Update(int id, AirportUpdateDto dto)
//        {
//            var airport = await _db.Airports.FindAsync(id);
//            if (airport == null) return null;

//            airport.IATA = dto.IATA;
//            airport.Name = dto.Name;
//            airport.City = dto.City;

//            await _db.SaveChangesAsync();

//            return new AirportGetDto
//            {
//                Id = airport.Id,
//                IATA = airport.IATA,
//                Name = airport.Name,
//                City = airport.City
//            };
//        }

//        public async Task<bool> Delete(int id)
//        {
//            var airport = await _db.Airports.FindAsync(id);
//            if (airport == null) return false;

//            _db.Airports.Remove(airport);
//            await _db.SaveChangesAsync();
//            return true;
//        }
//    }
//}
