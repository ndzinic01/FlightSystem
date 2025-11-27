using FlightSystem.DTOs.User;
using FlightSystem.Services.Interfaces;
using FlightSystem.Services;
using Microsoft.AspNetCore.Mvc;
using FlightSystem.DTOs.Login;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
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
    public async Task<IActionResult> Create(UserAddDTO dto)
    {
        return Ok(await _service.CreateAsync(dto));
    }

    [HttpPut]
    public async Task<IActionResult> Update(UserUpdateDTO dto)
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
        return Ok(new { message = "User deleted successfully" });
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        // Prosljeđuje se lista dozvoljenih rola (flexible)
        var result = await _service.LoginAsync(dto, "Admin");

        if (result == null)
            return Unauthorized(new { message = "Invalid credentials or insufficient role" });

        return Ok(result);
    }

}

