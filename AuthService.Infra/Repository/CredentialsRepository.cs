using AuthService.Domain.Interfaces;
using AuthService.Domain.Models;
using AuthService.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infra.Repository;

public class CredentialsRepository : ICredentialsRepository
{
    private readonly DbSet<LoginCredential> _context;
    
    public CredentialsRepository(AppDbContext context)
    {
        _context = context.Credentials;
    }
    
    public async Task Register(LoginCredential loginCredential)
    {
        await _context.AddAsync(loginCredential);
    }

    public async Task<LoginCredential?> GetCredentialByUsername(string username)
    {
        return await _context.FirstOrDefaultAsync(x => x.Username == username);
    }

    public async Task<bool> IsUsernameInDb(string username)
    {
        return await _context.AnyAsync(x => x.Username == username );
    }
}