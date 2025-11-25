//using FlightSystem.Data;
//using FlightSystem.DTOs.Aircraft;
//using FlightSystem.Models;
//using FlightSystem.Services.Interfaces;
//using Microsoft.EntityFrameworkCore;

//namespace FlightSystem.Services
//{
//    public class AircraftService : IAircraftService
//    {
//        private readonly ApplicationDbContext _db;

//        public AircraftService(ApplicationDbContext db)
//        {
//            _db = db;
//        }

//        public List<AircraftGetDto> GetAll()
//        {
//            return _db.Aircrafts
//                .Select(x => new AircraftGetDto
//                {
//                    Id = x.Id,
//                    Model = x.Model,
//                    Manufacturer = x.Manufacturer,
//                    Capacity = x.Capacity
//                })
//                .ToList();
//        }

//        public AircraftGetDto? GetById(int id)
//        {
//            return _db.Aircrafts
//                .Where(x => x.Id == id)
//                .Select(x => new AircraftGetDto
//                {
//                    Id = x.Id,
//                    Model = x.Model,
//                    Manufacturer = x.Manufacturer,
//                    Capacity = x.Capacity
//                })
//                .FirstOrDefault();
//        }

//        public AircraftGetDto Create(AircraftCreateDto dto)
//        {
//            var aircraft = new Aircraft
//            {
//                Model = dto.Model,
//                Manufacturer = dto.Manufacturer,
//                Capacity = dto.Capacity
//            };

//            _db.Aircrafts.Add(aircraft);
//            _db.SaveChanges();

//            return new AircraftGetDto
//            {
//                Id = aircraft.Id,
//                Model = aircraft.Model,
//                Manufacturer = aircraft.Manufacturer,
//                Capacity = aircraft.Capacity
//            };
//        }

//        public AircraftGetDto? Update(int id, AircraftCreateDto dto)
//        {
//            var aircraft = _db.Aircrafts.Find(id);
//            if (aircraft == null) return null;

//            aircraft.Model = dto.Model;
//            aircraft.Manufacturer = dto.Manufacturer;
//            aircraft.Capacity = dto.Capacity;

//            _db.SaveChanges();

//            return new AircraftGetDto
//            {
//                Id = aircraft.Id,
//                Model = aircraft.Model,
//                Manufacturer = aircraft.Manufacturer,
//                Capacity = aircraft.Capacity
//            };
//        }

//        public bool Delete(int id)
//        {
//            var aircraft = _db.Aircrafts.Find(id);
//            if (aircraft == null) return false;

//            _db.Aircrafts.Remove(aircraft);
//            _db.SaveChanges();
//            return true;
//        }
//    }
//}

