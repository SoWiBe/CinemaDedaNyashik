using System.ComponentModel.DataAnnotations;
using Back.Api.Models.Core;

namespace Back.Api.Models;

/// <summary>
/// Класс пользователя
/// </summary>
public class User : BaseModel
{
    /// <summary>
    /// Уникальный идентификатор пользователя Telegram
    /// </summary>
    public long TelegramUserId { get; set; }
    /// <summary>
    /// Имя пользователя в Telegram (может быть null)
    /// </summary>
    [MaxLength(50)] public string? Username { get; set; }
    /// <summary>
    /// Имя пользователя (First Name)
    /// </summary>
    [Required] [MaxLength(100)] public string FirstName { get; set; } = null!;
    /// <summary>
    /// Фамилия пользователя (Last Name) - может быть null
    /// </summary>
    [MaxLength(100)] public string? LastName { get; set; }
    /// <summary>
    /// Время последнего взаимодействия с ботом
    /// </summary>
    public DateTime? LastInteraction { get; set; }
    /// <summary>
    /// Дата создания записи
    /// </summary>
    [Required] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    [Required] public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Обновление времени последнего взаимодействия
    /// </summary>
    public void UpdateLastInteraction() { LastInteraction = DateTime.UtcNow; }
    /// <summary>
    /// Обновление времени модификации
    /// </summary>
    public void UpdateModified() { UpdatedAt = DateTime.UtcNow; }

}