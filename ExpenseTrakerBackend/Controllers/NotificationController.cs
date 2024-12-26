using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PracticeCrud.Common.SignalR.Notification;

namespace PracticeCrud.Controllers
{
    [Route("api/notification")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationController(INotificationService notificationService, IHubContext<NotificationHub> hubContext)
        {
             _hubContext = hubContext;
             _notificationRepository = notificationService;
        }


        [HttpGet("{userId}/{markAsRead}")]
        public async Task<IActionResult> GetNotifications(int userId, bool markAsRead)
        {
            var notifications = await _notificationRepository.GetNotificationsAsync(userId, markAsRead);
            await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotifications", notifications);
            return Ok(notifications);
        }
    }
}
