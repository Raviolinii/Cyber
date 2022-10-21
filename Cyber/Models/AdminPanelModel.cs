using Microsoft.AspNetCore.Identity;

namespace Cyber.Models
{
    public class AdminPanelModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<IdentityUser> UserList { get; set; }
    }
}
