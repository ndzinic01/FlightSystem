using FlightSystem.DTOs.Notification;

public interface INotificationService
{
    Task<List<NotificationGetDTO>> GetAllAsync();
    Task<List<NotificationGetDTO>> GetByUserAsync(int userId);
    Task<NotificationGetDTO?> GetByIdAsync(int id);
    Task<NotificationGetDTO> CreateAsync(NotificationAddDTO dto);
    Task<NotificationGetDTO?> UpdateAsync(NotificationUpdateDTO dto);
    Task<NotificationGetDTO?> ReplyAsync(NotificationReplyDTO dto);
    Task<bool> DeleteAsync(int id);
}


