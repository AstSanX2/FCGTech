using FCG.API.Domain.DTO.UsersDTO;

namespace FCG.API.Domain.DTO.AuthenticationDTO
{
    public class AuthenticationTokenDTO
    {
        public string Token { get; set; } = string.Empty;
        public DateTime? ExpiresOn { get; set; }
        public ProjectUserDTO? UserInfo { get; set; }
    }
}
