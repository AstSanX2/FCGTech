using FCG.API.Domain.DTO.Bases;
using FCG.API.Domain.DTO.Bases.Interfaces;
using FCG.API.Domain.Models.Validation;
using FCG.API.Helpers.Extensions;
using FCG.Domain.Entities;

namespace FCG.API.Domain.DTO.UsersDTO
{
    public class CreateUserDTO : BaseCreateDTO<User>, IValidator
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public CreateUserDTO()
        {

        }

        public override User ToEntity()
        {
            return new User
            {
                Name = Name,
                Email = Email,
                Password = Password.ToHash()
            };
        }

        public ValidationResultModel Validate()
        {
            var response = new ValidationResultModel();

            if (string.IsNullOrWhiteSpace(Name))
                response.AddError("Nome não preenchido");

            if (string.IsNullOrWhiteSpace(Email))
            {
                response.AddError("Email não preenchido");
            }
            else if (!IsValidEmail(Email))
            {
                response.AddError("Formato de email inválido");
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                response.AddError("Senha não preenchida");
            }
            else if (!IsSecurePassword(Password!))
            {
                response.AddError("Senha fraca: deve conter pelo menos 8 caracteres, incluindo letras, números e símbolos.");
            }

            return response;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsSecurePassword(string password)
        {
            if (password.Length < 8)
                return false;

            var hasLetter = password.Any(char.IsLetter);
            var hasDigit = password.Any(char.IsDigit);
            var hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasLetter && hasDigit && hasSpecial;
        }

    }
}
