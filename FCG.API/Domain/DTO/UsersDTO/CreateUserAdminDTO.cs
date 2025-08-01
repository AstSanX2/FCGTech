﻿using FCG.Domain.Entities;

namespace FCG.API.Domain.DTO.UsersDTO
{
    public class CreateUserAdminDTO : CreateUserDTO
    {
        public CreateUserAdminDTO()
        {

        }

        public override User ToEntity()
        {
            return new User
            {
                Name = Name,
                Email = Email,
                Password = Password,
                Role = Enums.UserRole.Admin
            };
        }
    }
}
