using FCG.API.Domain.DTO.AuthenticationDTO;
using FCG.API.Domain.Models.Response;
using MongoDB.Bson;

namespace FCG.API.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<ResponseModel<ObjectId>> Register(RegisterUserDTO registerRequest);
        Task<ResponseModel<AuthenticationTokenDTO>> Login(LoginUserDTO loginUserRequest);
    }
}
