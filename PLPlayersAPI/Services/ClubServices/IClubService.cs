using PLPlayersAPI.Models.DTOs;

namespace PLPlayersAPI.Services.ClubServices
{
    public interface IClubService
    {
        Task<IEnumerable<ClubDTO>> GetAllClubsAsync();
        Task<ClubDTO?> GetClubByIdAsync(int clubId);
    }
}
