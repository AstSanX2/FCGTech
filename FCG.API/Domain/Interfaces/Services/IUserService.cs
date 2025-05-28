using FCG.API.Domain.DTO.UsersDTO;
using FCG.API.Domain.Models.Response;
using MongoDB.Bson;

namespace FCG.API.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<ResponseModel<ProjectUserDTO>> CreateAsync(CreateUserDTO createDto);
        Task DeleteAsync(ObjectId id);
        Task<List<ProjectUserDTO>> FindUsersAsync(FilterUserDTO filterDto);
        Task<List<ProjectUserDTO>> GetAllAsync();
        Task<ProjectUserDTO?> GetByIdAsync(ObjectId id);
        Task UpdateAsync(ObjectId id, UpdateUserDTO updateDto);
    }
}
