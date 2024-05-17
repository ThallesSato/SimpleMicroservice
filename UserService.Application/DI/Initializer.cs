using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;
using UserService.Infra.Context;
using UserService.Infra.Repository;

namespace UserService.Application.DI;

public static class Initializer
{
    public static void ConfigureDi(this IServiceCollection services)
    {
        //bd
        services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("AuthDb"));

        // repositorios
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
        
        //servicos
        services.AddScoped(typeof(IUserService), typeof(Services.UserService));
    }
}