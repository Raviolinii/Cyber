using Microsoft.AspNetCore.Identity;

namespace Cyber.Models
{
    public class AdminPanelModel
    {
        public string BlockedUserId { get; set; }
        public string DeletedUserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserToChangePassId { get; set; }
        public List<UserModel> UserList { get; set; }
    }
}
