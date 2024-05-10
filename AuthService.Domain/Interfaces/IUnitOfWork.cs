namespace AuthService.Domain.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}