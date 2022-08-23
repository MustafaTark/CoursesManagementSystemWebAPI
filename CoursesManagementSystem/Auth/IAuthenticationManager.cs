using CoursesManagementSystem.DTO;

namespace CoursesManagementSystem.Auth
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUser(UserForAuthenticationDTO userForAuth);
        Task<string> CreateToken();
    }
}
