using FCG.Domain.Entities;
using FCG.Domain.Interfaces.Repositories;
using MongoDB.Driver;

namespace FCG.Infraestructure.Repositories
{
    public class UserRepository(IMongoDatabase database) : BaseRepository<User>(database), IUserRepository
    {
    }
}
