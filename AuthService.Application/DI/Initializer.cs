using AuthService.Application.Interfaces;
using AuthService.Application.Services;
using AuthService.Domain.Interfaces;
using AuthService.Infra.Context;
using AuthService.Infra.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Application.DI;

public static class Initializer
{
    public static void ConfigureDI(this IServiceCollection services)
    {
        //bd
        services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("AuthDb"));

        // repositorios
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        services.AddScoped(typeof(ICredentialsRepository), typeof(CredentialsRepository));
        
        //servicos
        services.AddScoped(typeof(IAuthService), typeof(Services.AuthService));
        services.AddScoped(typeof(ITokenService), typeof(TokenService));
        
    }
}