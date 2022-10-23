using Interfaces.Models;

namespace NotifyJob.Interfaces;

public interface INotificationService
{
    Task NotifyUser(long chatId, FlatDTO[] flat);
}
