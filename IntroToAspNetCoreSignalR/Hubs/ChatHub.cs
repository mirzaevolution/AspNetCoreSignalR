using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
namespace IntroToAspNetCoreSignalR.Hubs
{
    public interface IChatHub
    {
        Task ReceiveMessage(string user, string message);
    }
    //public class ChatHub:Hub
    //{
    //    public async Task SendMessage(string user, string message)
    //    {
            
    //        await Clients.All.SendAsync("ReceiveMessage", user, message);
    //    }
    //}
    public class ChatHub : Hub<IChatHub>
    {
        
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.ReceiveMessage(user, message);
        }
    }
}
