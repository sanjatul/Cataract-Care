using Microsoft.AspNetCore.Identity;

namespace Cataract_Care.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? FullName { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
