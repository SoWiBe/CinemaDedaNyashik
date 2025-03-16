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
    /// Получить все сущности
    /// </summary>
    /// <returns>Список всех сущностей</returns>
    IEnumerable<T> GetAll();

    /// <summary>
    /// Найти сущности по предикату
    /// </summary>
    /// <param name="predicate">Предикат для поиска</param>
    /// <returns>Список найденных сущностей</returns>
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Добавить новую сущность
    /// </summary>
    /// <param name="entity">Сущность для добавления</param>
    void Add(T entity);

    /// <summary>
    /// Обновить существующую сущность
    /// </summary>
    /// <param name="entity">Сущность для обновления</param>
    void Update(T entity);

    /// <summary>
    /// Удалить сущность
    /// </summary>
    /// <param name="entity">Сущность для удаления</param>
    void Remove(T entity);

    /// <summary>
    /// Добавить диапазон сущностей
    /// </summary>
    /// <param name="entities">Коллекция сущностей для добавления</param>
    void AddRange(IEnumerable<T> entities);

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