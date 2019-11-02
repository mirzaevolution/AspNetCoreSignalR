using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSignalRWithAuth.Hubs
{
    public interface IChatHub
    {
        Task ReceiveMessage(string user, string message);
    }
}
