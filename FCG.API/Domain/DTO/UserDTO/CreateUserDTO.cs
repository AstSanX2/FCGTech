using FCG.API.Domain.DTO.Bases;
using FCG.Domain.Entities;

namespace FCG.API.Domain.DTO.UserDTO
{
    public class CreateUserDTO : BaseCreateDTO<User>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public override User ToEntity()
        {
            return new User
            {
                Name = Name,
                Email = Email,
                Password = Password
            };
        }
    }
}
