using FCG.API.Domain.DTO.Bases.Interfaces;
using FCG.API.Domain.DTO.Bases;
using FCG.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using FCG.API.Domain.Models.Validation;
using FCG.API.Helpers.Extensions;

namespace FCG.API.Domain.DTO.AuthenticationDTO
{
    public class RegisterUserDTO : BaseCreateDTO<User>, IValidator
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public override User ToEntity()
        {
            return new User
            {
                Name = Name,
                Email = Email,
                Password = Password.ToHash(),
                Role = Enums.UserRole.UserApp
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
