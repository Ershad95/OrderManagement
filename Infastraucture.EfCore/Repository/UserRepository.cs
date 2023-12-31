﻿using Application.Repository;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public UserRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<User?> GetAsync(Guid guid, CancellationToken cancellationToken)
    {
        var user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Guid == guid,
            cancellationToken: cancellationToken);
        return user;
    }

    public async Task<User?> GetAsync(string username, string password, CancellationToken cancellationToken)
    {
        var user = await _applicationDbContext.Users.FirstOrDefaultAsync(
            x => x.UserName == username && x.Password == password, cancellationToken: cancellationToken);
        return user;
    }
    
     public async Task<bool> CheckUserNameIsAvailibleAsync(string username, CancellationToken cancellationToken)
        {
            var result = await _applicationDbContext.Users.AnyAsync(
                x => x.UserName == username, cancellationToken: cancellationToken);
            return result;
        }

    public async Task AddAsync(User user,CancellationToken cancellationToken)
    {
        await _applicationDbContext.Users.AddAsync(user,cancellationToken);
    }
}