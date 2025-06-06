﻿using FCG.API.Domain.DTO.Bases;
using FCG.API.Domain.DTO.Bases.Interfaces;
using FCG.API.Domain.DTO.UsersDTO;
using FCG.API.Domain.Interfaces.Repositories;
using FCG.API.Domain.Interfaces.Services;
using FCG.API.Domain.Models.Response;
using FCG.Domain.Entities;
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

        public async Task<ResponseModel<ProjectUserDTO>> CreateAsync(CreateUserDTO createDto)
        {
            return await ProcessCriate(createDto);
        }

        public async Task<ResponseModel<ProjectUserDTO>> CreateAdminAsync(CreateUserAdminDTO createDto)
        {
            return await ProcessCriate(createDto);
        }

        public async Task UpdateAsync(ObjectId id, UpdateUserDTO updateDto)
        {
            await UserRepository.UpdateAsync(id, updateDto);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            await UserRepository.DeleteAsync(id);
        }

        public async Task<User> GetAdmin()
        {
            return await UserRepository.FindOneAsync(x => x.Role == Domain.Enums.UserRole.Admin);
        }

        private async Task<ResponseModel<ProjectUserDTO>> ProcessCriate<T>(T createDto) where T : BaseCreateDTO<User>, IValidator
        {
            var validationResult = createDto.Validate();

            if (validationResult.HasError)
            {
                return ResponseModel<ProjectUserDTO>.BadRequest(validationResult.ToString());
            }

            var result = await UserRepository.CreateAsync(createDto);

            var resultModel = await UserRepository.GetByIdAsync<ProjectUserDTO>(result._id);
            return ResponseModel<ProjectUserDTO>.Created(resultModel);
        }
    }
}
