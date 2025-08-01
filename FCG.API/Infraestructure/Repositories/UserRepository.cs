﻿using FCG.API.Domain.Interfaces.Repositories;
using FCG.Domain.Entities;
using MongoDB.Driver;

namespace FCG.API.Infraestructure.Repositories
{
    public class UserRepository(IMongoDatabase database) : BaseRepository<User>(database), IUserRepository
    {
    }
}
