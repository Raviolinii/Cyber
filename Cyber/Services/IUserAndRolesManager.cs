using Microsoft.AspNetCore.Identity;

namespace Cyber.Services
{
    public interface IUserAndRolesManager
    {
        public Task<IdentityResult> AddUser(string name, string email, string password);
        public Task<IdentityResult> AddUserToRole(string email, string role);
        public Task<IdentityResult> DeleteUser(string email);
    }
}
