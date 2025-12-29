using FlightSystem.DTOs.Destination;
using FlightSystem.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DestinationController : ControllerBase
{
    private readonly IDestinationService _service;

    public DestinationController(IDestinationService service)
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
    public async Task<IActionResult> Create(DestinationAddDTO dto)
    {
        return Ok(await _service.CreateAsync(dto));
    }

    [HttpPut("toggle-active")]
    public async Task<IActionResult> ToggleActive(DestinationUpdateDTO dto)
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
        return Ok(new { message = "Deleted successfully" });
    }
   

}

