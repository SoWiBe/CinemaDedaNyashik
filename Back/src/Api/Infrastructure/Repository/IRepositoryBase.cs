using System.Linq.Expressions;

namespace Back.Api.Infrastructure.Repository;

public interface IRepositoryBase<T> where T : class
{
    /// <summary>
    /// Получить сущность по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <returns>Сущность или null, если не найдена</returns>
    T? Get(object? id);

    /// <summary>
    /// Получить сущность по идентификатору ASYNC
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Сущность или null, если не найдена</returns>
    Task<T?> GetAsync(object? id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить все сущности
    /// </summary>
    /// <returns>Список всех сущностей</returns>
    IEnumerable<T> GetAll();
    
    /// <summary>
    /// Получить все сущности ASYNC
    /// </summary>
    /// <returns>Список всех сущностей</returns>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Найти сущности по предикату
    /// </summary>
    /// <param name="predicate">Предикат для поиска</param>
    /// <returns>Список найденных сущностей</returns>
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Найти сущности по предикату ASYNC
    /// </summary>
    /// <param name="predicate">Предикат для поиска</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Список найденных сущностей</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавить новую сущность
    /// </summary>
    /// <param name="entity">Сущность для добавления</param>
    void Add(T entity);

    /// <summary>
    /// Добавить новую сущность ASYNC
    /// </summary>
    /// <param name="entity">Сущность для добавления</param>
    /// <param name="cancellationToken"></param>
    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить существующую сущность
    /// </summary>
    /// <param name="entity">Сущность для обновления</param>
    void Update(T entity);

    /// <summary>
    /// Обновить существующую сущность ASYNC
    /// </summary>
    /// <param name="entity">Сущность для обновления</param>
    /// <param name="cancellationToken"></param>
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить сущность
    /// </summary>
    /// <param name="entity">Сущность для удаления</param>
    void Remove(T entity);

    /// <summary>
    /// Удалить сущность ASYNC
    /// </summary>
    /// <param name="entity">Сущность для удаления</param>
    /// <param name="cancellationToken"></param>
    Task RemoveAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавить диапазон сущностей
    /// </summary>
    /// <param name="entities">Коллекция сущностей для добавления</param>
    void AddRange(IEnumerable<T> entities);

    /// <summary>
    /// Добавить диапазон сущностей ASYNC
    /// </summary>
    /// <param name="entities">Коллекция сущностей для добавления</param>
    /// <param name="cancellationToken"></param>
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить диапазон сущностей
    /// </summary>
    /// <param name="entities">Коллекция сущностей для удаления</param>
    void RemoveRange(IEnumerable<T> entities);

    /// <summary>
    /// Создать запрос с предикатом
    /// </summary>
    /// <param name="predicate">Предикат для запроса</param>
    /// <returns>IQueryable с примененным предикатом</returns>
    IQueryable<T> Query(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Получить все сущности как IQueryable
    /// </summary>
    /// <returns>IQueryable всех сущностей</returns>
    IQueryable<T> FindAsQueryable();
}