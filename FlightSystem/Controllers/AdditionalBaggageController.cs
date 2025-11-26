using FlightSystem.DTOs.AdditionalBaggage;
using FlightSystem.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AdditionalBaggageController : ControllerBase
{
    private readonly IAdditionalBaggageService _service;

    public AdditionalBaggageController(IAdditionalBaggageService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AdditionalBaggageAddDTO dto)
    {
        return Ok(await _service.CreateAsync(dto));
    }

    [HttpPut]
    public async Task<IActionResult> Update(AdditionalBaggageUpdateDTO dto)
    {
        var result = await _service.UpdateAsync(dto);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound();
        return Ok(new { message = "Additional baggage removed successfully" });
    }
}
