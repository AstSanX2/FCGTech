
using FCG.API.Helpers;
using FCG.Application.Services;
using FCG.Domain.Interfaces.Repositories;
using FCG.Domain.Interfaces.Services;
using FCG.Infraestructure.Repositories;
using MongoDB.Driver;

namespace FCG
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(new ObjectIdJsonConverter());
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var config = builder.Configuration;
            builder.Services.AddSingleton<IMongoClient>(s =>
            {
                var section = config.GetSection("MongoDB");
                var connection = section.GetSection("ConnectionString").Value;

                var url = new MongoUrlBuilder(connection);

                var client = new MongoClient(new MongoClientSettings()
                {
                    Server = url.Server
                });
                return client;
            });

            builder.Services.AddSingleton(s =>
            {
                var section = config.GetSection("MongoDB");
                var connection = section.GetSection("ConnectionString").Value;
                var url = new MongoUrlBuilder(connection);
                return s.GetService<IMongoClient>()!.GetDatabase(url.DatabaseName);
            });

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
