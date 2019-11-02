using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreSignalRWithAuth.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
namespace AspNetCoreSignalRWithAuth.Controllers
{
    [Authorize]
    public class SignalRController : Controller
    {
        private readonly IHubContext<ChatHub> _chatHubContext;
        public SignalRController(IHubContext<ChatHub> hubContext)
        {
            _chatHubContext = hubContext;
        }
        public IActionResult ChatApp()
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