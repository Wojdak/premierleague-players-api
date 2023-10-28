using PLPlayersAPI.Models;
using PLPlayersAPI.Models.DTOs;

namespace PLPlayersAPI.Services.ClubServices
{
    public interface IClubService
    {
        Task<IEnumerable<ClubDTO>> GetAllClubsAsync();
        Task<ClubDTO?> GetClubByIdAsync(int clubId);
        Task<int> AddClubAsync(Club club);
        Task<int?> UpdateClubAsync(int clubId, Club club);
        Task<bool> DeleteClubAsync(int clubId);
    }
}
