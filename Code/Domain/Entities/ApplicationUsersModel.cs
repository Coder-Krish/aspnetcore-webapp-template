using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUsers: IdentityUser
    {
        public string FullName { get; set; }
        public bool IsActive { get; set; }
    }
}
