using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntroToAspNetCoreSignalR.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace IntroToAspNetCoreSignalR.Controllers
{
    public class SignalRController : Controller
    {
        IHubContext<ChatHub> _chatHubContext;
        public SignalRController(IHubContext<ChatHub> hubContext)
        {
            _chatHubContext = hubContext;
        }
        public IActionResult SampleOne()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> InvokeTimeNotification()
        {
            await _chatHubContext.Clients.All.SendAsync("ReceiveMessage", "Server Notification", $"Server time: {DateTimeOffset.Now}");
            return Ok();
        }
    }
}