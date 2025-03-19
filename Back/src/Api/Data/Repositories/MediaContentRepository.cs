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
        var isExist = await _context.MediaContents.AnyAsync(x => x.Title.Equals(dto.Title), cancellationToken: cancellationToken);
        if (isExist)
            throw new CustomException("Данный контент был найден, пожалуйста, введите другой!");
        
        var mediaContent = _mapper.Map<MediaContent>(dto);
        await _context.MediaContents.AddAsync(mediaContent, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return mediaContent;
    }
}