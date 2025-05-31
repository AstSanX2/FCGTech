using FCG.API.Domain.DTO.GameDTO;
using FCG.API.Domain.Interfaces.Repositories;
using FCG.API.Domain.Interfaces.Services;
using FCG.API.Domain.Models.Response;
using MongoDB.Bson;

namespace FCG.API.Application.Services
{
    public class GameService(IGameRepository GameRepository) : IGameService
    {
        public async Task<List<ProjectGameDTO>> GetAllAsync()
        {
            return await GameRepository.GetAllAsync<ProjectGameDTO>();
        }

        public async Task<ProjectGameDTO?> GetByIdAsync(ObjectId id)
        {
            return await GameRepository.GetByIdAsync<ProjectGameDTO>(id);
        }

        public async Task<List<ProjectGameDTO>> FindGamesAsync(FilterGameDTO filterDto)
        {
            return await GameRepository.FindAsync<ProjectGameDTO>(filterDto);
        }

        public async Task<ResponseModel<ProjectGameDTO>> CreateAsync(CreateGameDTO createDto)
        {
            var validationResult = createDto.Validate();

            if (validationResult.HasError)
            {
                return ResponseModel<ProjectGameDTO>.BadRequest(validationResult.ToString());
            }

            var result = await GameRepository.CreateAsync(createDto);

            var resultModel = await GameRepository.GetByIdAsync<ProjectGameDTO>(result._id);
            return ResponseModel<ProjectGameDTO>.Created(resultModel);
        }

        public async Task UpdateAsync(ObjectId id, UpdateGameDTO updateDto)
        {
            await GameRepository.UpdateAsync(id, updateDto);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            await GameRepository.DeleteAsync(id);
        }
    }
}
