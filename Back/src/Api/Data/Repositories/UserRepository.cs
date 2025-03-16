using Back.Api.Data.Repositories.Core;
using Back.Api.Infrastructure.Repository;
using Back.Api.Models;

namespace Back.Api.Data.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }
}