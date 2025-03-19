using AutoMapper;
using Back.Api.Data.Repositories.Core;
using Back.Api.Infrastructure.Dto.Users;
using Back.Api.Infrastructure.Exceptions;
using Back.Api.Infrastructure.Repository;
using Back.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Back.Api.Data.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UserRepository(AppDbContext context, IMapper mapper) : base(context)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<User> Create(UserDto dto, CancellationToken cancellationToken)
    {
        var isExist = await _context.Users.AnyAsync(x => x.TelegramUserId == dto.TelegramUserId, cancellationToken: cancellationToken);
        if (isExist)
            throw new CustomException("Данный пользователь был найден!");
        
        var user = _mapper.Map<User>(dto);
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<User> GetById(long telegramUserId, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.TelegramUserId == telegramUserId, cancellationToken: cancellationToken);
        if (user is null)
            throw new CustomException("Данный пользователь не был найден!");

        return user;
    }

    public async Task DeleteAll(CancellationToken cancellationToken)
    {
        var users = await _context.Users.ToListAsync(cancellationToken);
        _context.Users.RemoveRange(users);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteById(long telegramUserId, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.TelegramUserId == telegramUserId, cancellationToken: cancellationToken);
        if (user is null)
            throw new CustomException("Данный пользователь не был найден!");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}