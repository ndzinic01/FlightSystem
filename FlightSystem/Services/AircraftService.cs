using FlightSystem.Data;
using FlightSystem.DTOs.Aircraft;
using FlightSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightSystem.Services
{
    public class AircraftService : IAircraftService
    {
        private readonly ApplicationDbContext _db;

        public AircraftService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<AircraftGetDTO>> GetAllAsync()
        {
            return await _db.Aircrafts
                .Select(a => new AircraftGetDTO
                {
                    Id = a.Id,
                    Model = a.Model,
                    RegistrationNumber = a.RegistrationNumber,
                    YearManufacturer = a.YearManufacturer,
                    Manufacturer = a.Manufacturer,
                    Capacity = a.Capacity,
                    Status = a.Status
                })
                .ToListAsync();
        }

        public async Task<AircraftGetDTO?> GetByIdAsync(int id)
        {
            return await _db.Aircrafts
                .Where(a => a.Id == id)
                .Select(a => new AircraftGetDTO
                {
                    Id = a.Id,
                    Model = a.Model,
                    RegistrationNumber = a.RegistrationNumber,
                    YearManufacturer = a.YearManufacturer,
                    Manufacturer = a.Manufacturer,
                    Capacity = a.Capacity,
                    Status = a.Status
                })
                .FirstOrDefaultAsync();
        }

        public async Task<AircraftGetDTO> AddAsync(AircraftAddUpdateDTO dto)
        {
            var aircraft = new Aircraft
            {
                Model = dto.Model,
                RegistrationNumber = dto.RegistrationNumber,
                YearManufacturer = dto.YearManufacturer,
                Manufacturer = dto.Manufacturer,
                Capacity = dto.Capacity,
                Status = dto.Status
            };

            _db.Aircrafts.Add(aircraft);
            await _db.SaveChangesAsync();

            return new AircraftGetDTO
            {
                Id = aircraft.Id,
                Model = aircraft.Model,
                RegistrationNumber = aircraft.RegistrationNumber,
                YearManufacturer = aircraft.YearManufacturer,
                Manufacturer = aircraft.Manufacturer,
                Capacity = aircraft.Capacity,
                Status = aircraft.Status
            };
        }

        public async Task<AircraftGetDTO?> UpdateAsync(int id, AircraftAddUpdateDTO dto)
        {
            var aircraft = await _db.Aircrafts.FindAsync(id);
            if (aircraft == null)
                return null;

            aircraft.Model = dto.Model;
            aircraft.RegistrationNumber = dto.RegistrationNumber;
            aircraft.YearManufacturer = dto.YearManufacturer;
            aircraft.Manufacturer = dto.Manufacturer;
            aircraft.Capacity = dto.Capacity;
            aircraft.Status = dto.Status;

            await _db.SaveChangesAsync();

            return new AircraftGetDTO
            {
                Id = aircraft.Id,
                Model = aircraft.Model,
                RegistrationNumber = aircraft.RegistrationNumber,
                YearManufacturer = aircraft.YearManufacturer,
                Manufacturer = aircraft.Manufacturer,
                Capacity = aircraft.Capacity,
                Status = aircraft.Status
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var aircraft = await _db.Aircrafts.FindAsync(id);
            if (aircraft == null)
                return false;

            _db.Aircrafts.Remove(aircraft);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}


