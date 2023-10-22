using PLPlayersAPI.Models.DTOs;

namespace PLPlayersAPI.Services.PositionServices
{
    public interface IPositionService
    {
        Task<IEnumerable<PositionDTO>> GetAllPositionsAsync();
        Task<PositionDTO?> GetPositionByIdAsync(int positionId);
    }
}
