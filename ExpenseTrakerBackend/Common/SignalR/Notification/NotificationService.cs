using Dapper;
using PracticeCrud.Common.Config;
using PracticeCrud.Model.Budget;
using PracticeCrud.Model.General;
using System.Data;

namespace PracticeCrud.Common.SignalR.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;
        private readonly IBaseRepository _baseRepository;
        public NotificationService(IConfiguration configuration, IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;
            _configuration = configuration;
        }
        public async Task<List<NotificationResModel>> GetNotificationsAsync(int userId, bool markAsRead)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            param.Add("@MarkAsRead", markAsRead);
            var result = await _baseRepository.QueryAsync<NotificationResModel>("SP_GetUserNotifications", param, commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
    }
}
