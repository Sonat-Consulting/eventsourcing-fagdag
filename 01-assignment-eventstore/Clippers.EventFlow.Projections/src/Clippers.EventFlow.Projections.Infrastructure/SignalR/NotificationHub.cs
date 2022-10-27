using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Clippers.EventFlow.Projections.Infrastructure.SignalR
{
    public interface INotificationHubClient
    {
        Task SendNotification(string message);
    }
    public class NotificationHub : Hub, INotificationHubClient
    {
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("SendNotification",message);
        }

      
    }
}