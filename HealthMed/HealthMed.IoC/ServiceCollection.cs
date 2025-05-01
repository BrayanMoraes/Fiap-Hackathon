using HealthMed.Business.Services;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Infra.Data;
using HealthMed.Infra.Repositories;
using HealthMed.Infra.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System.Text;

namespace HealthMed.IoC
{
    public static class ServiceCollection
    {

        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IPacienteService, PacienteService>();
            serviceCollection.AddScoped<IAgendaService, AgendaService>();
            serviceCollection.AddScoped<IMedicoService, MedicoService>();
            serviceCollection.AddScoped<IMedicoEspecialidadeService, MedicoEspecialidadeService>();
            serviceCollection.AddScoped<IPacienteRepository, PacienteRepository>();
            serviceCollection.AddScoped<IMedicoRepository, MedicoRepository>();
            serviceCollection.AddScoped<IMedicoEspecialidadeRepository, MedicoEspecialidadeRepository>();
            serviceCollection.AddScoped<IAgendaRepository, AgendaRepository>();
        }

        public static void AddAuthJwT(this IServiceCollection serviceCollection)
        {
            var key = Encoding.ASCII.GetBytes("D43493B1-FD3A-49DB-83FF-531A61A5313A");

            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        }

        public static void AddRabbitMq(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IConnection>(sp =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost", // Substitua pelo hostname do RabbitMQ
                    Port = 32002,            // Porta padrão do RabbitMQ
                    UserName = "admin",     // Substitua pelo usuário do RabbitMQ
                    Password = "fi@ph@ck@th0n"      // Substitua pela senha do RabbitMQ
                };
                return factory.CreateConnectionAsync().GetAwaiter().GetResult();
            });
        }

        public static void AddSql(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
    }
}
