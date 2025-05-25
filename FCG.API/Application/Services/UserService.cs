using FCG.API.Domain.DTO.UserDTO;
using FCG.API.Domain.Interfaces.Repositories;
using FCG.API.Domain.Interfaces.Services;
using MongoDB.Bson;

namespace FCG.API.Application.Services
{
    public class UserService(IUserRepository UserRepository) : IUserService
    {
        public async Task<List<ProjectUserDTO>> GetAllAsync()
        {
            return await UserRepository.GetAllAsync<ProjectUserDTO>();
        }

        public async Task<ProjectUserDTO?> GetByIdAsync(ObjectId id)
        {
            return await UserRepository.GetByIdAsync<ProjectUserDTO>(id);
        }

        public async Task<List<ProjectUserDTO>> FindUsersAsync(FilterUserDTO filterDto)
        {
            return await UserRepository.FindAsync<ProjectUserDTO>(filterDto);
        }

        public async Task<ProjectUserDTO> CreateAsync(CreateUserDTO createDto)
        {
            var result = await UserRepository.CreateAsync(createDto);
            return await UserRepository.GetByIdAsync<ProjectUserDTO>(result._id);
        }

        public async Task UpdateAsync(ObjectId id, UpdateUserDTO updateDto)
        {
            await UserRepository.UpdateAsync(id, updateDto);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            await UserRepository.DeleteAsync(id);
        }
    }
}
