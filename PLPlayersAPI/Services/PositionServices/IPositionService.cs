using PLPlayersAPI.Models.DTOs;
using PLPlayersAPI.Models;

public interface IPositionService
{
    Task<IEnumerable<PositionDTO>> GetAllPositionsAsync();
    Task<PositionDTO?> GetPositionByIdAsync(int positionId);
    Task<int> AddPositionAsync(Position position);
    Task<int?> UpdatePositionAsync(int positionId, Position position);
    Task<bool> DeletePositionAsync(int positionId);
}
