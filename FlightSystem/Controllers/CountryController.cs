using FlightSystem.DTOs.Country;
using FlightSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _service;

        public CountryController(ICountryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAll());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var country = await _service.GetById(id);
            if (country == null) return NotFound();
            return Ok(country);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CountryAddUpdateDTO dto)
        {
            var result = await _service.Create(dto);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CountryAddUpdateDTO dto)
        {
            var updated = await _service.Update(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.Delete(id);
            if (!success) return NotFound();
            return Ok(new { message = "Country deleted successfully." });
        }
    }
}

