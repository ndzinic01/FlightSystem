using FlightSystem.DTOs.Aircraft;
using FlightSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlightSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AircraftController : ControllerBase
    {
        private readonly IAircraftService _service;

        public AircraftController(IAircraftService service)
        {
            _service = service;
        }

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var aircraft = _service.GetById(id);
            if (aircraft == null)
                return NotFound("Aircraft not found");

            return Ok(aircraft);
        }

        [HttpPost("add")]
        public IActionResult Create(AircraftCreateDto dto)
        {
            var result = _service.Create(dto);
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public IActionResult Update(int id, AircraftCreateDto dto)
        {
            var result = _service.Update(id, dto);
            if (result == null)
                return NotFound("Aircraft not found");

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var ok = _service.Delete(id);
            if (!ok)
                return NotFound("Aircraft not found");

            return Ok("Deleted");
        }
    }
}
