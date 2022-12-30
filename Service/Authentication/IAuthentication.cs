using dotnet_rpg.Models;

namespace dotnet_rpg.Service.Authentication
{
    public interface IAuthentication
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}
