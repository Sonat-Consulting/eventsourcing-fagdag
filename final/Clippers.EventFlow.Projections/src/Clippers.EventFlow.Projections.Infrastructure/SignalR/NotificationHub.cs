using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Clippers.EventFlow.Projections.Infrastructure.SignalR
{
    public interface INotificationHubClient
    {
        Task SendNotification2(string message);
    }
    public class NotificationHub : Hub, INotificationHubClient
    {
        //private IHubContext<NotificationHub, INotificationHubClient> _notificationHub;
        //public NotificationHub(IHubContext<NotificationHub, INotificationHubClient> notificationHub)
        //{
        //    _notificationHub = notificationHub;
        //}

        //private IHubContext<NotificationHub> _notificationHub;
        //public NotificationHub(IHubContext<NotificationHub> notificationHub) 
        //{
        //    _notificationHub = notificationHub;
        //}

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await SendNotification("Testing");
        }
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("SendNotification",message);
        }

        public Task SendNotification2(string message)
        {
            throw new NotImplementedException();
        }
    }
}