using FlightSystem.DTOs.Flight;
using FlightSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _service;

        public FlightController(IFlightService service)
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
            var result = _service.GetById(id);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPost("create")]
        public IActionResult Create(FlightCreateDto dto)
        {
            return Ok(_service.Create(dto));
        }

        [HttpPut("update/{id}")]
        public IActionResult Update(int id, FlightCreateDto dto)
        {
            var result = _service.Update(id, dto);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            bool ok = _service.Delete(id);
            if (!ok) return NotFound();

            return Ok("Deleted");
        }
    }
}
