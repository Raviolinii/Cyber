using Microsoft.AspNetCore.Identity;

namespace Cyber.Models
{
    public class UserModel : IdentityUser
    {
        public bool PasswordChanged { get; set; } = false;
        public bool PasswordExpirationEnabled { get; set; } = false;
        public DateTime PasswordExpirationDate { get; set; }
    }
}
