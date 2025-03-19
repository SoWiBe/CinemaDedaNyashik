using System.Linq.Expressions;
using Back.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Back.Api.Infrastructure.Repository;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly AppDbContext _context;
    protected readonly DbSet<T> DbSet;

    public RepositoryBase(AppDbContext context)
    {
        _context = context;
        DbSet = context.Set<T>();
    }

    public T? Get(object? id)
    {
        return _context.Set<T>().Find(id);
    }

    public async Task<T?> GetAsync(object? id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().FindAsync(id, cancellationToken);
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().ToListAsync(cancellationToken);
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().Where(predicate);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().Where(predicate).ToListAsync(cancellationToken);
    }

    public void Add(T entity)
    {
        _context.Entry(entity).State = EntityState.Added;
        _context.SaveChanges();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Entry(entity).State = EntityState.Added;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Remove(T entity)
    {
        _context.Entry(entity).State = EntityState.Deleted;
        _context.SaveChanges();
    }

    public async Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Entry(entity).State = EntityState.Deleted;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            _context.Entry(entity).State = EntityState.Added;
        }
        _context.SaveChanges();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            _context.Entry(entity).State = EntityState.Added;
        }
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }
        _context.SaveChanges();
    }

    public IQueryable<T> Query(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().Where(predicate).AsQueryable();
    }

    public IQueryable<T> FindAsQueryable()
    {
        return _context.Set<T>().AsQueryable<T>();
    }

}