using PLPlayersAPI.Models;
using PLPlayersAPI.Models.DTOs;

namespace PLPlayersAPI.Services.NationalityServices
{
    public interface INationalityService
    {
        Task<IEnumerable<NationalityDTO>> GetAllNationalitiesAsync();
        Task<NationalityDTO?> GetNationalityByIdAsync(int nationalityId);
        Task<int> AddNationalityAsync(Nationality nationality);
        Task<int?> UpdateNationalityAsync(int nationalityId, Nationality nationality);
        Task<bool> DeleteNationalityAsync(int nationalityId);
    }
}
