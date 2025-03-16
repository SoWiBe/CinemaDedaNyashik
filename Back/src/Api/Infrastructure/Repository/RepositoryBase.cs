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

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().Where(predicate);
    }

    public void Add(T entity)
    {
        _context.Entry(entity).State = EntityState.Added;
        _context.SaveChanges();
    }

    public void Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void Remove(T entity)
    {
        _context.Entry(entity).State = EntityState.Deleted;
        _context.SaveChanges();
    }

    public void AddRange(IEnumerable<T> entities)
    {
        foreach (var entity in entities)
        {
            _context.Entry(entity).State = EntityState.Added;
        }
        _context.SaveChanges();
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