using PLPlayersAPI.Models.DTOs;
using PLPlayersAPI.Models;

namespace PLPlayersAPI.Services.UserServices
{
    public interface IUserService
    {
        public Task<string?> LoginUser(UserDTO userDTO);
        public string CreateToken(User user);
        public Task<UserDTO> RegisterUser(UserDTO userDTO);
    }
}
