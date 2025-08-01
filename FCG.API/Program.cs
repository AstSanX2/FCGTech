using FCG.API.Application.Services;
using FCG.API.Domain.Interfaces.Repositories;
using FCG.API.Domain.Interfaces.Services;
using FCG.API.Helpers;
using FCG.API.Helpers.Extensions;
using FCG.API.Infraestructure.Migration;
using FCG.API.Infraestructure.Options;
using FCG.API.Infraestructure.Repositories;
using FCG.API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text;

namespace FCG.API
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

                var settings = MongoClientSettings.FromConnectionString(connection);
                settings.ServerApi = new ServerApi(ServerApiVersion.V1);
                var client = new MongoClient(settings);
                return client;
            });
            builder.Services.AddSingleton(s =>
            {
                var section = config.GetSection("MongoDB");
                var connectionString = section.GetValue<string>("ConnectionString");

                var mongoUrl = new MongoUrl(connectionString);
                var databaseName = mongoUrl.DatabaseName;

                if (string.IsNullOrEmpty(databaseName))
                {
                    throw new InvalidOperationException("Database name must be specified in the connection string.");
                }

                var client = s.GetRequiredService<IMongoClient>();
                return client.GetDatabase(databaseName);
            });

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IGameRepository, GameRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IGameService, GameService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FGC", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Entre com um Jwt Token válido",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x =>
                {
                    var jwtSettings = config.GetJwtOptions()!;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key!)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddTransient<GlobalRequestMiddleware>();
            builder.Services.AddHostedService<MongoSeeder>();

            builder.Services.Configure<EnvironmentOptions>(
                builder.Configuration.GetSection("Environment"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || true)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<GlobalRequestMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
