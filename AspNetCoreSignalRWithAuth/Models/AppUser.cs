using Microsoft.AspNetCore.Identity;

namespace AspNetCoreSignalRWithAuth.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
