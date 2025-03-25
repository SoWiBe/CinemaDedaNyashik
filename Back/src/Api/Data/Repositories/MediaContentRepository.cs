using AutoMapper;
using Back.Api.Data.Repositories.Core;
using Back.Api.Endpoints.Responses;
using Back.Api.Infrastructure.Dto.MediaContent;
using Back.Api.Infrastructure.Exceptions;
using Back.Api.Infrastructure.Repository;
using Back.Api.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
        
        var user = await _context.Users.FirstOrDefaultAsync(x => x.TelegramUserId == dto.UserId, cancellationToken: cancellationToken);
        if (user is null)
            throw new CustomException("Данный пользователь не был найден, пожалуйста, введите другой!");
        
        var mediaContent = _mapper.Map<MediaContent>(dto);
        mediaContent.UserId = user.Id;
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
            .Where(x => x.Status != MediaContentStatus.Deleted)
            .OrderBy(x => x.Title)
            .Skip(random.Next(user.MediaContents!.Count()))
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
        
        return user.MediaContents!.Where(x => x.Status != MediaContentStatus.Deleted).ToList(); 
    }

    public async Task<MediaContent> GetRandom(CancellationToken cancellationToken)
    {
        var mediaContents = await _context.MediaContents.ToListAsync(cancellationToken);
        
        var random = new Random();
        
        var randomItem = mediaContents
            .Where(x => x.Status != MediaContentStatus.Deleted)
            .OrderBy(x => x.Title)
            .Skip(random.Next(mediaContents!.Count()))
            .Take(1)
            .FirstOrDefault();
        
        if (randomItem is null)
            throw new CustomException("Ничего не выпало(((");

        return randomItem;
    }

    public async Task<bool> DeleteMediaContent(long mediaContentId, CancellationToken cancellationToken)
    {
        var mediaContent = await _context.MediaContents
            .FirstOrDefaultAsync(x => x.Id == mediaContentId, cancellationToken);

        if (mediaContent is null)
        {
            Log.Error("Контент был не найден");
            return false;
        }

        mediaContent.Status = MediaContentStatus.Deleted;
        
        _context.MediaContents.Update(mediaContent);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAllMediaContent(long telegramUserId, CancellationToken cancellationToken)
    {
        var mediaContents = await _context.MediaContents.Where(x => x.UserId == telegramUserId)
            .ToListAsync(cancellationToken);

        if (!mediaContents.Any())
        {
            Log.Error("Контент был не найден");
            return false;
        }
        
        foreach (var mediaContent in mediaContents)
            mediaContent.Status = MediaContentStatus.Deleted;

        _context.MediaContents.UpdateRange(mediaContents);
        await _context.SaveChangesAsync(cancellationToken);
        
        return true; 
    }

    public async Task<bool> SetInverseStatusContent(long mediaContentId, CancellationToken cancellationToken)
    {
        var mediaContent = await _context.MediaContents.FirstOrDefaultAsync(x 
            => x.Id == mediaContentId && x.Status != MediaContentStatus.Deleted, cancellationToken);
        
        if (mediaContent is null)
        {
            Log.Error("Контент был не найден или удален");
            return false;
        }
        
        mediaContent.Status = mediaContent.Status == MediaContentStatus.Success 
            ? MediaContentStatus.Waiting 
            : MediaContentStatus.Success;

        _context.MediaContents.Update(mediaContent);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    private async Task<User> GetUser(long telegramUserId, CancellationToken cancellationToken)
    {
        var user = await _context.Users.Include(x => x.MediaContents).FirstOrDefaultAsync(x => x.TelegramUserId == telegramUserId, cancellationToken);
        if (user is null)
            throw new CustomException("Данный пользователь не был найден, пожалуйста, введите другой!");
        
        return user;
    }
}