using FCG.API.Domain.Interfaces.Repositories;
using FCG.Domain.Entities;
using MongoDB.Driver;

namespace FCG.API.Infraestructure.Repositories
{
    public class GameRepository(IMongoDatabase database) : BaseRepository<Game>(database), IGameRepository
    {
    }
}
