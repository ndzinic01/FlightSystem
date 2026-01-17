using FlightSystem.DTOs.Notification;
using FlightSystem.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _service;

    public NotificationController(INotificationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        return Ok(await _service.GetByUserAsync(userId));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(NotificationAddDTO dto)
    {
        return Ok(await _service.CreateAsync(dto));
    }

    [HttpPut]
    public async Task<IActionResult> Update(NotificationUpdateDTO dto)
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

    [HttpPost("reply")]
    public async Task<IActionResult> Reply(NotificationReplyDTO dto)
    {
        var result = await _service.ReplyAsync(dto);
        if (result == null) return NotFound();
        return Ok(result);
    }
    // 🔥 BROADCAST - Pošalji svim korisnicima
    [HttpPost("broadcast")]
    public async Task<IActionResult> Broadcast(BroadcastNotificationDTO dto)
    {
        var count = await _service.BroadcastToAllUsersAsync(dto);
        return Ok(new { message = $"Notifikacija poslana na {count} korisnika." });
    }

    // 🔥 Otkazan let
    [HttpPost("flight-cancelled/{flightId}")]
    public async Task<IActionResult> NotifyFlightCancellation(int flightId, [FromBody] string reason)
    {
        var count = await _service.NotifyFlightCancellationAsync(flightId, reason);
        return Ok(new { message = $"Notifikovano {count} korisnika o otkazivanju leta." });
    }

    // 🔥 Promjena vremena
    [HttpPost("flight-rescheduled/{flightId}")]
    public async Task<IActionResult> NotifyFlightReschedule(int flightId, [FromBody] DateTime newDepartureTime)
    {
        var count = await _service.NotifyFlightRescheduleAsync(flightId, newDepartureTime);
        return Ok(new { message = $"Notifikovano {count} korisnika o promjeni vremena." });
    }

    // 🔥 Zakašnjenje
    [HttpPost("flight-delayed/{flightId}")]
    public async Task<IActionResult> NotifyFlightDelay(int flightId, [FromBody] int delayMinutes)
    {
        var count = await _service.NotifyFlightDelayAsync(flightId, delayMinutes);
        return Ok(new { message = $"Notifikovano {count} korisnika o zakašnjenju." });
    }

}
