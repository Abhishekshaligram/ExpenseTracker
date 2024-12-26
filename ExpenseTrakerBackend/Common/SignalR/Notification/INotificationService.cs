using PracticeCrud.Model.General;
using System.Collections;

namespace PracticeCrud.Common.SignalR.Notification
{
    public interface INotificationService
    {
        Task<List<NotificationResModel>>GetNotificationsAsync(int userId, bool markAsRead);
    }
}
