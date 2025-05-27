using FCG.API.Domain.DTO.GameDTO;
using FCG.API.Domain.Models.Response;
using MongoDB.Bson;

namespace FCG.API.Domain.Interfaces.Services
{
    public interface IGameService
    {
        Task<ResponseModel<ProjectGameDTO>> CreateAsync(CreateGameDTO createDto);
        Task DeleteAsync(ObjectId id);
        Task<List<ProjectGameDTO>> FindGamesAsync(FilterGameDTO filterDto);
        Task<List<ProjectGameDTO>> GetAllAsync();
        Task<ProjectGameDTO?> GetByIdAsync(ObjectId id);
        Task UpdateAsync(ObjectId id, UpdateGameDTO updateDto);
    }
}
