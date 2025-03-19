using AutoMapper;
using Back.Api.Data.Repositories.Core;
using Back.Api.Infrastructure.Dto.MediaContent;
using Back.Api.Infrastructure.Exceptions;
using Back.Api.Infrastructure.Repository;
using Back.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Back.Api.Data.Repositories;

public class MediaContentRepository : RepositoryBase<MediaContent>, IMediaContentRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public MediaContentRepository(AppDbContext context, IMapper mapper) : base(context)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<MediaContent> Create(MediaContentDto dto, CancellationToken cancellationToken)
    {
        var isExist = await _context.MediaContents.AnyAsync(x => x.Title == dto.Title, cancellationToken: cancellationToken);
        if (isExist)
            throw new CustomException("Данный контент был найден, пожалуйста, введите другой!");
        
        isExist = await _context.Users.AnyAsync(x => x.TelegramUserId == dto.UserId, cancellationToken: cancellationToken);
        if (!isExist)
            throw new CustomException("Данный пользователь не был найден, пожалуйста, введите другой!");
        
        var mediaContent = _mapper.Map<MediaContent>(dto);
        await _context.MediaContents.AddAsync(mediaContent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return mediaContent;
    }

    public async Task<MediaContent> GetMyRandom(long telegramUserId, CancellationToken cancellationToken)
    {
        var user = await GetUser(telegramUserId, cancellationToken);
        
        if (user.MediaContents?.Any() == false)
            throw new CustomException("У вас пока что нет контента для получения!");

        var random = new Random();
        
        var randomItem = user.MediaContents!
            .OrderBy(x => x.Title)
            .Skip(random.Next(user.MediaContents!.Count))
            .Take(1)
            .FirstOrDefault();
        
        if (randomItem is null)
            throw new CustomException("Ничего не выпало(((");

        return randomItem;
    }

    public async Task<IEnumerable<MediaContent>> GetMyList(long telegramUserId, CancellationToken cancellationToken)
    {
        var user = await GetUser(telegramUserId, cancellationToken);
        
        if (user.MediaContents?.Any() == false)
            throw new CustomException("У вас пока что нет контента для получения!");
        
        return user.MediaContents!;
    }

    public async Task<MediaContent> GetRandom(CancellationToken cancellationToken)
    {
        var mediaContents = await _context.MediaContents.ToListAsync(cancellationToken);
        
        var random = new Random();
        
        var randomItem = mediaContents
            .OrderBy(x => x.Title)
            .Skip(random.Next(mediaContents!.Count))
            .Take(1)
            .FirstOrDefault();
        
        if (randomItem is null)
            throw new CustomException("Ничего не выпало(((");

        return randomItem;
    }

    private async Task<User> GetUser(long telegramUserId, CancellationToken cancellationToken)
    {
        var user = await _context.Users.Include(x => x.MediaContents).FirstOrDefaultAsync(x => x.TelegramUserId == telegramUserId, cancellationToken);
        if (user is null)
            throw new CustomException("Данный пользователь не был найден, пожалуйста, введите другой!");
        
        return user;
    }
}