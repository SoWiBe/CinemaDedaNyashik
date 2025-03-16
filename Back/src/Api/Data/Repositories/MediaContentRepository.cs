using Back.Api.Data.Repositories.Core;
using Back.Api.Infrastructure.Repository;
using Back.Api.Models;

namespace Back.Api.Data.Repositories;

public class MediaContentRepository : RepositoryBase<MediaContent>, IMediaContentRepository
{
    public MediaContentRepository(AppDbContext context) : base(context){}
}