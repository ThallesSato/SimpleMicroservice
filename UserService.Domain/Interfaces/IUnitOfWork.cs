namespace UserService.Domain.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}