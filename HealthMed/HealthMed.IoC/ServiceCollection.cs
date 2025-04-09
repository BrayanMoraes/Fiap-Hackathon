using HealthMed.Business.Services;
using HealthMed.Domain.Interfaces.Repository;
using HealthMed.Domain.Interfaces.Services;
using HealthMed.Infra.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HealthMed.IoC
{
    public static class ServiceCollection
    {

        public static void AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IPacienteService, PacienteService>();
            serviceCollection.AddScoped<IMedicoService, MedicoService>();
            serviceCollection.AddScoped<IMedicoEspecialidadeService, MedicoEspecialidadeService>();
            serviceCollection.AddScoped<IPacienteRepository, PacienteRepository>();
            serviceCollection.AddScoped<IMedicoRepository, MedicoRepository>();
            serviceCollection.AddScoped<IMedicoEspecialidadeRepository, MedicoEspecialidadeRepository>();
        }

        public static void AddAuthJwT(this IServiceCollection serviceCollection)
        {
            var key = Encoding.ASCII.GetBytes("sua-chave-secreta-muito-segura");

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
    }
}
