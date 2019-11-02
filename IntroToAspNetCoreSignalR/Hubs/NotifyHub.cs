using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace IntroToAspNetCoreSignalR.Hubs
{
    public enum Status
    {
        Add = 1,
        Update = 2,
        Delete = 3
    }
    public interface INotifyHub
    {
        Task ReceiveNotification(string message, Status status);
        Task ReceiveUpdateNotification(string message, Status status, string token);
    }
    public class NotifyHub:Hub<INotifyHub>
    {
        public async Task Notify(string message, Status status)
        {
            await Clients.Others.ReceiveNotification(message, status);
        }
    }
}
