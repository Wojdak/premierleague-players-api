using PLPlayersAPI.Models.DTOs;

namespace PLPlayersAPI.Services.NationalityServices
{
    public interface INationalityService
    {
        Task<IEnumerable<NationalityDTO>> GetAllNationalitiesAsync();
        Task<NationalityDTO?> GetNationalityByIdAsync(int clubId);
    }
}
