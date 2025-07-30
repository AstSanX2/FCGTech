using FCG.API.Helpers.Extensions;
using FCG.Domain.Entities;
using MongoDB.Driver;

namespace FCG.API.Infraestructure.Migration
{
    /// <summary>
    /// Serviço de "seeding" que verifica se já existe algum usuário admin.
    /// Caso não exista, insere um usuário padrão.
    /// </summary>
    public class MongoSeeder : IHostedService
    {
        private readonly ILogger<MongoSeeder> _logger;
        private readonly IMongoCollection<User> _userCollection;

        // Para evitar rodar mais de uma vez, simplesmente encerraremos após o primeiro Run.
        private bool _alreadySeeded = false;

        public MongoSeeder(IConfiguration config,
            ILogger<MongoSeeder> logger)
        {
            _logger = logger;

            try
            {
                // Configura a conexão com o MongoDB e obtém a coleção "Users"
                var section = config.GetSection("MongoDB");
                var connection = section.GetSection("ConnectionString").Value;
                var url = new MongoUrlBuilder(connection);

                var client = new MongoClient(new MongoClientSettings()
                {
                    Server = url.Server
                });

                // Aqui assumimos que a coleção de usuários chamará "Users"
                _userCollection = client.GetDatabase(url.DatabaseName).GetCollection<User>(nameof(User));
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro na conexão com o MongoDB: {e.Message} \n\n stack trace: \n\n {e.StackTrace}");
            }

        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_alreadySeeded)
                    return;

                _logger.LogInformation("Iniciando MongoSeeder para verificar usuário admin...");

                // Filtro para encontrar se já existe algum usuário com perfil "Admin"
                var filtro = Builders<User>.Filter.Eq(u => u.Role, Domain.Enums.UserRole.Admin);

                // Verifica se existe
                var existeAdmin = await _userCollection.Find(filtro).AnyAsync(cancellationToken);

                if (!existeAdmin)
                {
                    _logger.LogInformation("Nenhum usuário Admin encontrado. Criando usuário admin padrão...");

                    var admin = new User
                    {
                        Name = "admin",
                        Password = "Senha@123".ToHash(),
                        Role = Domain.Enums.UserRole.Admin,
                        Email = "admin@fcg.com"
                    };

                    await _userCollection.InsertOneAsync(admin, cancellationToken: cancellationToken);

                    _logger.LogInformation("Usuário Admin inserido com sucesso.");
                }
                else
                {
                    _logger.LogInformation("Já existe ao menos um usuário Admin. Pulando criação.");
                }

                _alreadySeeded = true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro na conexão com o MongoDB: {e.Message} \n\n stack trace: \n\n {e.StackTrace}");
            }

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
