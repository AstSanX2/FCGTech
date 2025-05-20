using FCG.Domain.Entities;
using FCG.Domain.Interfaces.Repositories;
using FCG.Domain.Interfaces.Services;

namespace FCG.Application.Services
{
    public class UserService(IUserRepository repo) : BaseService<User>(repo), IUserService
    {
    }
}
